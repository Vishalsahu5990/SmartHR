using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class HomePage : ContentPage
	{
		List<tblEmployee> empList = null;
		public HomePage()
		{
			InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);


		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();


				
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			OnPrepare();
		}
		protected async void OnPrepare()
		{ 
		
		try
			{ 
				
var	db = DependencyService.Get<ISQLite>().GetConnection();
db.CreateTable<tblEmployee>();
var empModel = new List<EmployeeModel>();

EmployeeService es = new EmployeeService();
empList = await es.GetEmployees();
			}
			catch (Exception ex)
			{

			}}
		async void markAttendanceTapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new MarkAttendancePage());
		}
		async void allPendingAttendanceTapped(object sender, EventArgs e)
		{
			if (!ReferenceEquals(empList, null))
			{
				if (empList.Count > 0)
				{
					await Navigation.PushAsync(new PendingAttendancePage());
				}
				else
				{
					StaticMethods.ShowToast("No pending attendance available!");
				}
			}


		}
		async void uploadAttendanceTapped(object sender, EventArgs e)
		{
			await DisplayAlert("Message","Not Available","OK"); 
		}
		async void settingTapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SettingsPage());
		}




	}
}
