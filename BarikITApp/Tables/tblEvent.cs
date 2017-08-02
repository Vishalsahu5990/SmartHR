using System;
using SQLite.Net.Attributes;

namespace BarikITApp
{
	public class tblEvent
	{
[PrimaryKey, AutoIncrement]
public int Id { get; set; }
public string returnvalue { get; set; }
public string eventcode { get; set; }
public string errorcode { get; set; }
public string eventdescription { get; set; }
  
	} 
}
