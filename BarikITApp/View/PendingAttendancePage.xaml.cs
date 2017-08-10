using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class PendingAttendancePage : ContentPage
	{
		public PendingAttendancePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			SetData();

		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as EmployeeModel;
			await Navigation.PushAsync(new MarkAttendanceSubmitPage(item));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			flowlistview.FlowItemTapped-= Flowlistview_FlowItemTapped;
		}
		async void backTapped(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
		private async void SetData()
		{


			try
			{
				 
				var db = DependencyService.Get<ISQLite>().GetConnection();
				var empModel = new List< EmployeeModel>();

				EmployeeService es = new EmployeeService();
				var empList = await es.GetEmployees();
                var name= CrossSecureStorage.Current.GetValue("UserName", "N/A");
				if (empList != null)
				{
					for (int i = 0; i < empList.Count; i++)
					{
						empModel.Add(new EmployeeModel 
						{
							Id=empList[i].Id,
                            Name=name,
							GpsLocation=empList[i].GpsLocation,
							EventCode=empList[i].EventCode,
							JobCode=empList[i].JobCode,
							UploadedTime=empList[i].UploadedTime,
							profileImageSource=GetImage(empList[i].ProfileImage),
							ProfileBase64=empList[i].ProfileImage,
							IsUploaded=false
							                        
						}); 
					}
					flowlistview.FlowItemsSource = empModel;
				}

			}
			catch (Exception ex)
			{

			}
		}
		public ImageSource GetImage( string imageBase64)
		{ 
		 return  Xamarin.Forms.ImageSource.FromStream(
            () => new MemoryStream(Convert.FromBase64String(imageBase64)));
   
		}


	}
}
