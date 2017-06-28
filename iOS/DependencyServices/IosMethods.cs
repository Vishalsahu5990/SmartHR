using System;
using System.Security.Cryptography;
using System.Text;
using BarikITApp.iOS;
using Xamarin.Forms;

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
	}
}
