using System;
using System.IO;
using System.Text;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.SecureStorage;

namespace BarikITApp
{
	public static class StaticMethods
	{
		public static void ShowLoader()
		{

			UserDialogs.Instance.ShowLoading();


		}
		public static void DismissLoader()
		{

			UserDialogs.Instance.HideLoading();

		}
		public static void ShowToast(string msg)
		{


			UserDialogs.Instance.Toast(msg);

		}
		public static string DeviceType()
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				return "ios";
			}
			else
			{
				return "android";
			}


		}
		public static bool IsConnectedToInternet()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					return true;
				}
				else
				{
					StaticMethods.ShowToast("No internet connectivity. Please connect to internet.");
					return false;
				}

			}

			catch (Exception ex)
			{

			}
			return false;
		}
		public static byte[] StreamToByte(Stream input)
		{
			byte[] array = null;
			try
			{


				using (MemoryStream ms = new MemoryStream())
				{
					input.CopyTo(ms);
					if (Device.OS == TargetPlatform.iOS)
					{
						array = DependencyService.Get<IiosMethods>().ResizeImageIOS(ms.ToArray(),150,150);	
					}
					else
					{ 
						array = DependencyService.Get<iAndroidMethods>().ResizeImageAndroid(ms.ToArray(),150,150);	
					
					}
					return array;
				}

			}
			catch (Exception ex)
			{
				return null;
			}
		}

				
   
			
		 
	}
}
