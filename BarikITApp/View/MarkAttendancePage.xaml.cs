using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BarikITApp
{
	public partial class MarkAttendancePage : ContentPage
	{
		List<EventModel> _listEvent = null;
		List<JobModel> _listJob = null;
		string selectedJob = string.Empty;
		public MarkAttendancePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

           
            if(StaticMethods.IsConnectedToInternet())
			GetEvents().Wait();
            else
				GetJobEventsFromLocalDatabase();  

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			btnNext.Clicked += BtnNext_Clicked;
			pickerEvent.SelectedIndexChanged += PickerEvent_SelectedIndexChanged;
			pickerJob.SelectedIndexChanged += PickerJob_SelectedIndexChanged;

		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			btnNext.Clicked -= BtnNext_Clicked;

		}
		async void backTapped(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		async void BtnNext_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(lblEvent.Text))
				await Navigation.PushAsync(new MarkAttendanceSubmitPage(lblEvent.Text, selectedJob));
			else
				StaticMethods.ShowToast("Please select Event");
		}
		async void eventTapped(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				pickerEvent.Focus();
			});
		}
		async void jobTapped(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				pickerJob.Focus();
			});
		}

		void PickerEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			lblEvent.Text = pickerEvent.Items[pickerEvent.SelectedIndex];
		}

		void PickerJob_SelectedIndexChanged(object sender, EventArgs e)
		{
			lblJob.Text = pickerJob.Items[pickerJob.SelectedIndex];
			if (!ReferenceEquals(_listJob, null))
			{
				for (int i = 0; i < _listJob.Count; i++)
				{
					var array = _listJob[i].name.Split(':');
					var val = array[1];
					if (lblJob.Text.Equals(val))
					{
						var ar = _listJob[i].code.Split(':');
						selectedJob = ar[1];
						break;
					}
				}
			}
		}

		private async Task GetEvents()
		{
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{

						_listEvent = WebService.GetEvents();

					}).ContinueWith(async
		t =>
		{
			try
			{

				if (!ReferenceEquals(_listEvent, null))
				{
					Device.BeginInvokeOnMainThread(async () =>
				{
					var tblEventList = new List<tblEvent>();


					for (int i = 0; i < _listEvent.Count; i++)
					{
								tblEventList.Add(new tblEvent
								{ 
									errorcode=_listEvent[i].errorcode,
									eventcode=_listEvent[i].eventcode,
									eventdescription=_listEvent[i].eventdescription,
									returnvalue=_listEvent[i].returnvalue,
								
								});
						var array = _listEvent[i].eventcode.Split(':');

						pickerEvent.Items.Add(array[1].ToString());
					}
					GetJobs().Wait();
					AddEventsToLocalDatabase(tblEventList);    

				});

				}
				else
				{
					StaticMethods.ShowToast("Something went wrong please try again later!");
				}

				StaticMethods.DismissLoader();
			}
			catch (Exception ex)
			{

			}

		}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task GetJobs()
		{
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						_listJob = WebService.GetJobs();
					}).ContinueWith(async
		t =>
		{
                var tblJobs = new List<tblJob>();
			if (!ReferenceEquals(_listJob, null))
			{
				for (int i = 0; i < _listJob.Count; i++)
				{
                        tblJobs.Add(new tblJob
					{
                            code = _listJob[i].code,
                            errorcode = _listJob[i].errorcode,
                            name = _listJob[i].name,
                            returnvalue = _listJob[i].returnvalue,

					});

					var array = _listJob[i].name.Split(':');

					pickerJob.Items.Add(array[1].ToString());

				}
                    AddJobsToLocalDatabase(tblJobs);
			}
			else
			{
				StaticMethods.ShowToast("Something went wrong please try again later!");
			}

			StaticMethods.DismissLoader();

		}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
        private async Task AddEventsToLocalDatabase(List<tblEvent> eventsModel)
		{
			try
			{
				Device.BeginInvokeOnMainThread(async () =>
				{ 
				
var db = DependencyService.Get<ISQLite>().GetConnection();
db.DropTable<tblEvent>();
db.CreateTable<tblEvent>();
				EventService es = new EventService();
                  
await es.AddEvents(eventsModel);
				});

			}
			catch (Exception ex)
			{

			}
		}
        private async void AddJobsToLocalDatabase(List<tblJob> jobsModel)
		{
			try
			{
				Device.BeginInvokeOnMainThread(async () =>
				{

					var db = DependencyService.Get<ISQLite>().GetConnection();
                    db.DropTable<tblJob>();
                    db.CreateTable<tblJob>();
                    JobService es = new JobService();
					await es.AddJobs(jobsModel);
				});

			}
			catch (Exception ex)
			{

			}
		}
		
        private async Task GetJobEventsFromLocalDatabase()
        {
            try
            {
                EventService es = new EventService();
                var listEvents = await es.GetEvents();

                JobService js = new JobService();
                var listJobs = await js.GetJobs();

                System.Diagnostics.Debug.WriteLine("Testing"+listEvents.Count.ToString());

                //Filling out Job picker
				if (!ReferenceEquals(listJobs, null))
				{
					for (int i = 0; i < listJobs.Count; i++)
					{
                        var array = listJobs[i].name.Split(':');

						pickerJob.Items.Add(array[1].ToString());

					}
					
				}
				//Filling out Event Picker
                if (!ReferenceEquals(listEvents, null))
				{
					for (int i = 0; i < listEvents.Count; i++)
					{
						var array = listEvents[i].eventcode.Split(':');

                        pickerEvent.Items.Add(array[1].ToString());

					}

				}

            }
            catch (Exception ex)
            {

            }
        }

	}
}
