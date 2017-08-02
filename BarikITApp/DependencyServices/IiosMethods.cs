using System;
namespace BarikITApp
{
	public interface IiosMethods
	{
		string MD5Conversion(string text);
		string GetUniqueDeviceId();
		byte[] ResizeImageIOS(byte[] imageData, float width, float height);
	}
}
