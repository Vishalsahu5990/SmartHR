using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.SecureStorage;

namespace BarikITApp.Droid
{
	[Activity(Label = "SmartHR", Icon = "@drawable/logo", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			//loader initialization
			UserDialogs.Init(() => (Activity)Forms.Context);
			SecureStorageImplementation.StoragePassword = "SmartHr@123";
			LoadApplication(new App());
		}
	}
}
