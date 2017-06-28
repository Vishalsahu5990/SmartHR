using System;
using System.Text;
using Acr.UserDialogs;
using Xamarin.Forms;

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

	}
}
