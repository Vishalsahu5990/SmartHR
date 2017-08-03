using System;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions; 
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BarikITApp
{
	public partial class App : Application 
	{
		public static double ScreenHeight;
		public static double ScreenWidth;
		public static IGeolocator locator; 
		public App()
		{
			InitializeComponent();
			locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			var userId = CrossSecureStorage.Current.GetValue("UserId", null);
			if (userId!=null)
			{
				StaticDataModel.UserId = userId;
				StaticDataModel.AccessToken=CrossSecureStorage.Current.GetValue("access_token", null);
				MainPage = new NavigationPage(new HomePage()); 
			} 
			else
			{   
			
				MainPage = new NavigationPage(new LoginPage());  
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
{
	if (args.IsConnected)
	{
		StaticMethods.ShowToast("Connected to internet.");
	}
	else
	{
		StaticMethods.ShowToast("Disconnected to internet.");
	}

};
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
