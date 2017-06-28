using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
			App.Current.MainPage = new NavigationPage(new HomePage());
			//if (Isvalidated())
			//{
			//	SignIn();
			//}
		}
private async Task SignIn()
{
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				um = new UserModel();
				um = WebService.LogIn(txtEmail.Text, txtPassword.Text,"");


			}).ContinueWith(async
		t =>
{
				if (!string.IsNullOrEmpty(um.userName))
	{
					StaticDataModel.UserId = um.userName;
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
