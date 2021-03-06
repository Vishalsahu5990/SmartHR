﻿using System;
using System.Security.Cryptography;
using System.Text;
using BarikITApp;
using BarikITApp.Droid;
using Java.Lang;
using Xamarin.Forms;
using Android.Graphics;
using System.IO;

[assembly: Dependency(typeof(AndroidMethods))]
namespace BarikITApp.Droid
{
	public class AndroidMethods : iAndroidMethods
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
			return Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

		}
		public byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray();
			}
		}
	}
}
