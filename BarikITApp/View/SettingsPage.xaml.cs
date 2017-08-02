using System;
using System.Collections.Generic;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();

			var isSettingOn = CrossSecureStorage.Current.GetValue("UploadManually", "");
			if (!string.IsNullOrEmpty(isSettingOn))
			{
				if (isSettingOn.Equals("true"))
				{
					btnUploadAuto.BackgroundColor = Color.FromHex("#1390F0");
					btnUploadManually.BackgroundColor = Color.Silver;
				}
				else
				{
					btnUploadManually.BackgroundColor = Color.FromHex("#1390F0");
					btnUploadAuto.BackgroundColor = Color.Silver;
				}
			}
			else
			{ 
			CrossSecureStorage.Current.SetValue("UploadManually", "false");
			}

			NavigationPage.SetHasNavigationBar(this, false);
			btnUploadAuto.Clicked += (sender, e) =>
			 {
				 btnUploadAuto.BackgroundColor = Color.FromHex("#1390F0");
				 btnUploadManually.BackgroundColor = Color.Silver;
				                 
				CrossSecureStorage.Current.SetValue("UploadManually", "true");
			 
			};

			btnUploadManually.Clicked += (sender, e) =>
			 { 
btnUploadManually.BackgroundColor = Color.FromHex("#1390F0");
btnUploadAuto.BackgroundColor = Color.Silver;
			 CrossSecureStorage.Current.SetValue("UploadManually", "false");
			 };
		}
		async void backTapped(object sender, EventArgs e) 
		{
			await Navigation.PopAsync();
		}
	}
}
