using System;
namespace BarikITApp
{
	public class EmployeeModel
	{
public int Id { get; set; }
public string Name { get; set; }
public string GpsLocation { get; set; }
public string ProfileImage { get; set; }
public string EventCode { get; set; }
public string JobCode { get; set; }
public string UploadedTime { get; set; }
public string ProfileBase64 { get; set; }
public bool IsUploaded { get; set; }
public Xamarin.Forms.ImageSource profileImageSource { get; set;}
	}
}
