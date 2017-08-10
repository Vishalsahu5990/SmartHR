using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class LoginPage : ContentPage
	{
		UserModel um = null;
		public LoginPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			txtEmail.Focused += TxtEmail_Focused;
			txtPassword.Focused += TxtPassword_Focused;
			btnLogin.Clicked += BtnLogin_Clicked;

		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			txtEmail.Focused -= TxtEmail_Focused;
			txtPassword.Focused -= TxtPassword_Focused;
			btnLogin.Clicked -= BtnLogin_Clicked;
		}

		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.FromHex("#C1C1C1");
		}

		void TxtPassword_Focused(object sender, FocusEventArgs e)
		{
			txtPassword.PlaceholderColor = Color.FromHex("#C1C1C1");
		}

		void BtnLogin_Clicked(object sender, EventArgs e)
		{
			if (StaticMethods.IsConnectedToInternet())
			{
				if (Isvalidated())
				{
					GetLoginToken();
				}
			}


		}
		private async Task GetLoginToken()
		{
			StaticMethods.ShowLoader();
			UserModel um = null;
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{

						um = WebService.GetLoginToken(txtEmail.Text, txtPassword.Text);


					}).ContinueWith(async
		t =>
		{
			Device.BeginInvokeOnMainThread(async () =>
				{

					if (um != null)
					{
						StaticDataModel.AccessToken = um.access_token;
						SignIn(um.access_token).Wait();



					}
					else
					{
						StaticMethods.ShowToast("Authentication failed!");
					}


				});

		}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task SignIn(string token)
		{
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						um = new UserModel();
				um = WebService.LogIn(txtEmail.Text, txtPassword.Text, token);


					}).ContinueWith(async
		t =>
		{
                if (!string.IsNullOrEmpty(um.empid))
			{
                    StaticDataModel.UserId = um.empid;
					CrossSecureStorage.Current.SetValue("UserId", StaticDataModel.UserId);
                    CrossSecureStorage.Current.SetValue("UserName", um.userName);
				App.Current.MainPage = new NavigationPage(new HomePage());

			}
			else
			{
				StaticMethods.ShowToast("Authentication failed!");
			}

			StaticMethods.DismissLoader();

		}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private bool Isvalidated()
		{
			
			try
			{
				if (string.IsNullOrEmpty(txtEmail.Text))
				{
					txtEmail.PlaceholderColor = Color.Red;
					return false;
				}

				else if (string.IsNullOrEmpty(txtPassword.Text))
				{
					txtPassword.PlaceholderColor = Color.Red;
					return false;
				}
				else
				{
					return true;
				}

			}

			catch (Exception ex)
			{
				return false;
			}
			finally
			{

			}
		}
	}
}
