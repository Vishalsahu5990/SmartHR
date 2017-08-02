using System;
using SQLite.Net.Attributes;

namespace BarikITApp
{
	public class tblJob
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string returnvalue { get; set; }
		public string name { get; set; }
		public string code { get; set; }
		public string errorcode { get; set; }
	}
}
