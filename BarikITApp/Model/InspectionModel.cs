using System;
namespace BarikITApp
{
	public class InspectionModel
	{
		public int InspectionId { get; set; }
		public int LocationId { get; set; }
		public int ChecklistId { get; set; }
		public int SchedulingDetailsID { get; set; }
		public string Name { get; set; }
		public string ScheduleDate { get; set; }
		public string EndDate { get; set; }
		public string StartDate { get; set; }
		public object MainContractorWWO { get; set; }
		public string PersonResponsible { get; set; }
		public string ActionOwner { get; set; }
		public string MaxPoints { get; set; }
		public string PointsAwarded { get; set; }
		public string Score { get; set; }
		public string CurrentStatus { get; set; }
		public int CanExecute { get; set; }
		public object ModuleCode { get; set; }
		public string InspectionDate { get; set; }
		public string AppoveDate { get; set; }
		public string RectificationDate { get; set; }
		public string ReviewedBy { get; set; }
		public string ApprovedBy { get; set; }
		public string RectfiedBy { get; set; }
		public int CanDelete { get; set; }
		public int CanReSchedule { get; set; }
		public int IsOccuranceChanged { get; set; }
		public int IsReScheduled { get; set; }
		public string ScheduleTypeCode { get; set; }
		public object CustomDays { get; set; }
		public string Progress { get; set; }
		public string CheckListName { get; set; }
		public string LocationName { get; set; }
		public object Comments { get; set; }
		public string background_color { get; set; }

	}
}
