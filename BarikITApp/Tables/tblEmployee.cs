using System;
using SQLite.Net.Attributes;

namespace BarikITApp
{
	public class tblEmployee
	{
		[PrimaryKey, AutoIncrement] 
		public int Id { get; set; }
		public string GpsLocation { get; set; }
		public string ProfileImage { get; set; }
		public string EventCode { get; set; }
		public string JobCode { get; set; }
		public string UploadedTime { get; set; }
		public bool IsUploaded { get; set; } 



	}
}
