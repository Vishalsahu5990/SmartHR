using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BarikITApp
{
	public static class WebService
	{
		static HttpClient client = new HttpClient();
		public  static UserModel LogIn(string username, string password, string device_id)
		{
			username = "acct2.aedxb@smartcitysystems.com"; 
			password = "123";

			if (StaticMethods.DeviceType() == "ios")
			{
				device_id = DependencyService.Get<IiosMethods>().GetUniqueDeviceId();
			}
			else
			{
				device_id = DependencyService.Get<iAndroidMethods>().GetUniqueDeviceId();
			}
			UserModel um = new UserModel();

			try
			{
				string url = Constants.BaseUrl + "ValidateLogin";
				HttpResponseMessage response = null;

				JObject j = new JObject();
				j.Add("username", username);
				j.Add("password", password);
				j.Add("deviceid", password);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{
					StaticMethods.ShowToast("success");
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject jObj = JObject.Parse(contents); 

						if (jObj["userName"] != null)
						{
							um.access_token = jObj["access_token"].ToString();
							um.token_type = jObj["token_type"].ToString();
							um.expires_in = Convert.ToInt32(jObj["expires_in"].ToString());
							um.userName = jObj["userName"].ToString();
							um.issued = jObj[".issued"].ToString();
							um.expires = jObj[".expires"].ToString();

						}
						else
						{

							StaticMethods.ShowToast(jObj["error_description"].ToString());
						}
						return um;
					}
				}
			}
			catch (Exception ex)
			{
				//return null;
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				StaticMethods.ShowToast(ex.Message);
				return null;
			}

			//try
			//{
			//	string url = Constants.BaseUrl + "token";
			//	HttpResponseMessage response = null;

			//	JObject j = new JObject();
			//	j.Add("username", username);
			//	j.Add("password", password);
			//	//j.Add("deviceid", device_id);
			//	j.Add("grant_type", "password");

			//	HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

			//	request.ContentType = "application/x-www-form-urlencoded";

			//	request.Method = "POST";
			//	var stream = await request.GetRequestStreamAsync();
			//	using (var writer = new StreamWriter(stream))
			//	{
			//		writer.Write("grant_type=password&username=" + username + "&password=" + password);
			//		writer.Flush();
			//		writer.Dispose();
			//	}

			//	var response1 = await request.GetResponseAsync();
			//	var respStream = response1.GetResponseStream();

			//	using (StreamReader reader = new StreamReader(respStream))
			//	{
			//		var contents = reader.ReadToEnd();
			//		JObject jObj = JObject.Parse(contents);


			//		if (jObj["userName"] != null)
			//		{
			//			um.access_token = jObj["access_token"].ToString();
			//			um.token_type = jObj["token_type"].ToString();
			//			um.expires_in = Convert.ToInt32(jObj["expires_in"].ToString());
			//			um.userName = jObj["userName"].ToString();
			//			um.issued = jObj[".issued"].ToString();
			//			um.expires = jObj[".expires"].ToString();

			//		}
			//		else
			//		{

			//			StaticMethods.ShowToast(jObj["error_description"].ToString());
			//		}
			//		return um;
			//	}


			//}
			//catch (Exception ex)
			//{
			//	//return null;
			//	Debug.WriteLine(@"ERROR {0}", ex.Message);
			//	StaticMethods.ShowToast(ex.Message);
			//	//return null;
			//}
			finally
			{
				
				StaticMethods.DismissLoader();
			}
			return um;
		}
		public static List<InspectionModel> GetInspections()
		{
			
			List<InspectionModel> inspection = null;

			try
			{
				string url = Constants.BaseUrl + "Audit/GetInspectionDetails?LoggedUser=" + StaticDataModel.UserId + "&auditTypeCd=WS-ADT";
				HttpResponseMessage response = null;

				JObject j = new JObject();


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{

					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						//JObject jObj = JObject.Parse(contents);
						var datavalues = JValue.Parse(contents);
						inspection = datavalues.ToObject<List<InspectionModel>>();


					}

				}
				return inspection;
			}
			catch (Exception ex)
			{
				return null;
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				inspection = null;
				StaticMethods.DismissLoader();
			}
		}
	}
}
