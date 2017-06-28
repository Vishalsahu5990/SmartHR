using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BarikITApp
{
	public partial class AuditDetailsPage : ContentPage
	{
		InspectionModel inspectionModel = null;
		public AuditDetailsPage()
		{
			InitializeComponent();
		}
		public AuditDetailsPage(InspectionModel inspection_model)
		{
			InitializeComponent();
			inspectionModel = inspection_model;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			SetData();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
		private void SetData()
		{ 
		try
			{
				Device.BeginInvokeOnMainThread( () => 
				{
					lblLocationName.Text = inspectionModel.LocationName;
					lblRecordNo.Text = inspectionModel.InspectionId.ToString();
					lblName.Text = inspectionModel.Name;
					lblScheduleDate.Text = inspectionModel.ScheduleDate.ToString();
					lblMaxPoint.Text = inspectionModel.MaxPoints.ToString();
					lblScore.Text = inspectionModel.Score.ToString();
					lblActionOwner.Text = inspectionModel.ActionOwner;
					lblPointsAwarded.Text = inspectionModel.PointsAwarded;
					lblPersonResponsible.Text = inspectionModel.PersonResponsible;

				});
			}
			catch (Exception ex)
			{

			}
		}
	}
}
