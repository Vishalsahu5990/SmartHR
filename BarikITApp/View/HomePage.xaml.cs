using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class HomePage : ContentPage
	{
		
		public HomePage()
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);


		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
				}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			}




	}
}
