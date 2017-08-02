using System;
namespace BarikITApp
{
	public interface iAndroidMethods
	{
string MD5Conversion(string text);
string GetUniqueDeviceId();
		byte[] ResizeImageAndroid(byte[] imageData, float width, float height);
	}
}
