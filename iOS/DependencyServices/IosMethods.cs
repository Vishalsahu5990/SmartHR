using System;
using System.Security.Cryptography;
using System.Text;
using BarikITApp.iOS;
using Xamarin.Forms;
using System.Drawing;
using UIKit;
using CoreGraphics;

[assembly: Dependency(typeof(IosMethods))]
namespace BarikITApp.iOS
{
	public class IosMethods : IiosMethods
	{
		public string MD5Conversion(string input)
		{
			try
			{
				string key = System.String.Empty;
				key = "test"; // Need to configure the key in web config

				TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
				MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

				byte[] byteHash, byteBuff;
				string strTempKey = key;

				byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
				objHashMD5 = null;
				objDESCrypto.Key = byteHash;
				objDESCrypto.Mode = CipherMode.ECB; ////CBC, CFB

				byteBuff = ASCIIEncoding.ASCII.GetBytes(input);
				return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
			}
			catch (System.Exception ex)
			{
				return "Wrong Input. " + ex.Message;

			}
		}
		public string GetUniqueDeviceId()
		{
			return UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
		}
		public  byte[] ResizeImageIOS(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);
			UIImageOrientation orientation = originalImage.Orientation;

			//create a 24bit RGB image
			using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
												 (int)width, (int)height, 8,
												 (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
												 CGImageAlphaInfo.PremultipliedFirst))
			{

				RectangleF imageRect = new RectangleF(0, 0, width, height);

				// draw the image
				context.DrawImage(imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

				// save the image as a jpeg
				return resizedImage.AsJPEG().ToArray();
			}
		}
		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}
	}
}
