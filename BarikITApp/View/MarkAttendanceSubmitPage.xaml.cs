using System;
using System.Collections.Generic;
using Plugin.Media;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using SQLite.Net.Interop;

namespace BarikITApp
{
	public partial class MarkAttendanceSubmitPage : ContentPage
	{
		SQLite.Net.SQLiteConnection db = null;
		//private static IEmployeeService employeeService { get; } = DependencyService.Get<IEmployeeService>();
		Plugin.Media.Abstractions.MediaFile profileData = null;
		bool clicked = false;
		double Lattitude = 0;
		double Longitude = 0;
		string EventCode = string.Empty;
		string JobCode = string.Empty;
		MediaFile file = null;
		EmployeeModel _employeeModel = null;
		public MarkAttendanceSubmitPage()
		{
			InitializeComponent();

			PrepareData();


			clicked = false;
			NavigationPage.SetHasNavigationBar(this, false);
			///Location initialization
			App.locator = Plugin.Geolocator.CrossGeolocator.Current;


			if (!App.locator.IsListening)
				App.locator.StartListeningAsync(new System.TimeSpan(1), 2);
		}
		public MarkAttendanceSubmitPage(EmployeeModel empModel)
		{
			InitializeComponent();
			_employeeModel = empModel;

			PrepareData();


			clicked = false;
			NavigationPage.SetHasNavigationBar(this, false);
			///Location initialization
			App.locator = Plugin.Geolocator.CrossGeolocator.Current;


			if (!App.locator.IsListening)
				App.locator.StartListeningAsync(new System.TimeSpan(1), 2);
		}
		public MarkAttendanceSubmitPage(string eventCode, string jobCode)
		{
			InitializeComponent();

			PrepareData();

			clicked = false;
			NavigationPage.SetHasNavigationBar(this, false);
			EventCode = eventCode;
			JobCode = jobCode;
			///Location initialization 
			App.locator = Plugin.Geolocator.CrossGeolocator.Current;


			if (!App.locator.IsListening)
				App.locator.StartListeningAsync(new System.TimeSpan(1), 2);


		}

		async void BtnSubmit_Clicked(object sender, EventArgs e)
		{
			//Clicked is a boolean check to prevent the multiple method call
			if (!clicked)
			{
				clicked = true;
				if (!ReferenceEquals(_employeeModel, null))
					{
						SubmitAttendance(_employeeModel).Wait();
					}
				else if (!ReferenceEquals(profileData, null))
				{



						var isSettingOn = CrossSecureStorage.Current.GetValue("UploadManually", "");

						if (isSettingOn.Equals("true"))
						{
							SubmitAttendance().Wait();
						}
						else
						{
							var bytes = StaticMethods.StreamToByte(profileData.GetStream());
							string imageData = Convert.ToBase64String(bytes);
							var emp = new List<tblEmployee>();
							emp.Add(new tblEmployee
							{
								GpsLocation = Lattitude.ToString() + "," + Longitude.ToString(),
								ProfileImage = imageData,
								EventCode = EventCode,
								JobCode = JobCode,
								UploadedTime = System.DateTime.Now.ToString("f"),
								IsUploaded = false

							});
							Device.BeginInvokeOnMainThread(async () =>
							{
								EmployeeService es = new EmployeeService();
								await es.AddEmployees(emp);
								StaticMethods.ShowToast("Attendance has been saved in Offline Mode");
								App.Current.MainPage = new NavigationPage(new HomePage());
							});
						}

					


				}
				else
				{
                    clicked = false;
					StaticMethods.ShowToast("Please upload picture.");
				}

			}
		}

		async void BtnCancel_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new HomePage();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			App.locator.PositionChanged += position_Changed;
			btnSubmit.Clicked += BtnSubmit_Clicked;
			btnCancel.Clicked += BtnCancel_Clicked;



		}
		private async void PrepareData()
		{


			try
			{

				db = DependencyService.Get<ISQLite>().GetConnection();
				db.CreateTable<tblEmployee>();

				if (!ReferenceEquals(_employeeModel, null))
				{
					imgProfile.Source = _employeeModel.profileImageSource;
					imgProfile.IsVisible = true;
					frameProfile.IsVisible = false;
				}
				//EmployeeService es = new EmployeeService();
				//var empList = await es.GetEmployees();


			}
			catch (Exception ex)
			{

			}
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			App.locator.PositionChanged -= position_Changed;
		}
		void position_Changed(object obj, PositionEventArgs e)
		{

			updateGPSDataList(e.Position);
		}

		// Update GPS location displays and database
		private async void updateGPSDataList(Plugin.Geolocator.Abstractions.Position position)
		{
			Lattitude = position.Latitude;
			Longitude = position.Longitude;
		}

		async void backTapped(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
		async void imgProfileTapped(object sender, EventArgs e)
		{

			GetPhoto(true);

		}

		async void profileTapped(object sender, EventArgs e)
		{

			GetPhoto(true);

		}
		private async void GetPhoto(bool isFromCamera)
		{
			try
			{


				await CrossMedia.Current.Initialize();


				if (isFromCamera)
				{
					if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
					{

						DisplayAlert("No Camera", ":( No camera available.", "OK");
						return;
					}
					profileData = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
					{
						Directory = "Sample",
						Name = "test.jpg"
					});
				}
				else
				{
					profileData = await CrossMedia.Current.PickPhotoAsync();
				}

				if (profileData == null)
					return;
				imgProfile.IsVisible = true;
				frameProfile.IsVisible = false;


				imgProfile.Source = ImageSource.FromStream(() =>
	{
		var stream = profileData.GetStream();

		return stream;
	});

			}
			catch (Exception ex)
			{

			}

		}
		private async Task SubmitAttendance()
		{
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						ret = WebService.SubmitAttendance(Lattitude, Longitude, StaticMethods.StreamToByte(profileData.GetStream()), EventCode, JobCode);
					}).ContinueWith(async
		t =>
		{
			if (ret.Equals("Success;"))
			{
				StaticMethods.ShowToast("Attendance has been submited successfully!");
				App.Current.MainPage = new NavigationPage(new HomePage());
			}
			else
			{
				StaticMethods.ShowToast("Something went wrong please try again later!");
			}

			StaticMethods.DismissLoader();

		}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task SubmitAttendance(EmployeeModel empModel)
		{
			string ret = string.Empty;
			string image = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						if (!string.IsNullOrEmpty(_employeeModel.ProfileBase64))
						{
							image = _employeeModel.ProfileBase64;
							ret = WebService.SubmitAttendance(_employeeModel.GpsLocation, image, _employeeModel.EventCode, _employeeModel.JobCode);

						}
						else if (!ReferenceEquals(profileData, null))
						{
							var bytearray = StaticMethods.StreamToByte(profileData.GetStream());
							image = Convert.ToBase64String(bytearray);
							ret = WebService.SubmitAttendance(_employeeModel.GpsLocation, image, _employeeModel.EventCode, _employeeModel.JobCode);

						}
						else
						{
							StaticMethods.ShowToast("Please upload picture.");
						}



					}).ContinueWith(async
	t =>
{
	if (ret.Equals("Success;"))
	{
		var emp = new List<tblEmployee>();
		emp.Add(new tblEmployee
					{Id=_employeeModel.Id,
						GpsLocation = _employeeModel.GpsLocation,
						ProfileImage = _employeeModel.ProfileImage,
			EventCode = _employeeModel.EventCode,
			JobCode = _employeeModel.JobCode,
			UploadedTime = _employeeModel.UploadedTime,
						IsUploaded = true

		});
		Device.BeginInvokeOnMainThread(async () =>
				{
					try
					{ 

						StaticMethods.ShowToast("Attendance has been submited successfully!");
						EmployeeService es = new EmployeeService();
						await es.DeleteEmployee(emp[0]);

						App.Current.MainPage = new NavigationPage(new HomePage());
					}
					catch (Exception ex)
					{

					}
				});

	}
	else
	{
		StaticMethods.ShowToast("Something went wrong please try again later!");
	}

	StaticMethods.DismissLoader();

}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}
