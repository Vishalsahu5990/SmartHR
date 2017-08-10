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
using System.Net.Http.Headers;
using Plugin.SecureStorage;

namespace BarikITApp
{
	public static class WebService
	{
		static HttpClient client = new HttpClient();
		//Using form data
		public static UserModel GetLoginToken(string username, string password)
		{
			username = "3=nL*pa!2THd$ha95*28Z";
			password = "8a!QjtbHw5wJMCs%aN*J$";
			UserModel um = new UserModel();
			string ret = string.Empty;

			try
			{
				string url = Constants.BaseUrl + "token";

				Dictionary<string, string> parameters = new Dictionary<string, string>();

				parameters.Add("grant_type", "password");
				parameters.Add("username", username);
				parameters.Add("password", password);
				using (var httpClient = new HttpClient())
				{
					using (var content = new FormUrlEncodedContent(parameters))
					{
						content.Headers.Clear();
						content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

						var response1 = httpClient.PostAsync(url, content);


						using (StreamReader reader = new StreamReader(response1.Result.Content.ReadAsStreamAsync().Result))
						{
							var contents = reader.ReadToEnd();
							JObject jObj = JObject.Parse(contents);
							string token = jObj["access_token"].ToString();
							if (!string.IsNullOrEmpty(token))
							{
								CrossSecureStorage.Current.SetValue("access_token", token);
								um.access_token = jObj["access_token"].ToString();
								um.issued = jObj[".issued"].ToString();
								um.expires = jObj[".expires"].ToString();

								return um;
							}
							else
							{
								return null;
							}

						}

					}
				}

			}
			catch (Exception ex)
			{

				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();

			}
			return um;
		}
		public static UserModel LogIn(string username, string password, string token)
		{
			username = "acct2.aedxb@smartcitysystems.com";
			password = "123";
			string device_id = string.Empty;

			UserModel um = new UserModel();

			try
			{
				string url = Constants.BaseUrlFull + "ValidateLogin/Getvalues";
				HttpResponseMessage response = null;

				JObject j = new JObject();
				j.Add("username", username);
				j.Add("password", password);
				j.Add("deviceid", "001");
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var header = client.DefaultRequestHeaders;
				if (!header.Contains("Authorization"))
					client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{
					StaticMethods.ShowToast("success");
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var jarray = JArray.Parse(contents);
						var val = jarray[0].ToString();
						var jObj = JObject.Parse(val);

						if (jObj["empid"] != null)
						{
                            um.empid = jObj["empid"].ToString();
                            var array = um.empid.Split(':');
							if (array != null)
							{
                                um.empid = array[1];
							}

                            um.userName = jObj["empname"].ToString();
							var arrayUsername = um.userName.Split(':');
							if (arrayUsername != null)
							{
                                um.userName = arrayUsername[1];
							}


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


			finally
			{

				StaticMethods.DismissLoader();
			}
			return um;
		}
		public static List<EventModel> GetEvents()
		{


			List<EventModel> eventModel = null;

			try
			{
				string url = Constants.BaseUrlFull + "GetTaEvent";

				HttpResponseMessage response = null;

				JObject j = new JObject();
				j.Add("empid", StaticDataModel.UserId);
				j.Add("deviceid", "001");
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var header = client.DefaultRequestHeaders;
				if (!header.Contains("Authorization"))
					client.DefaultRequestHeaders.Add("Authorization", "bearer " + StaticDataModel.AccessToken);
				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{

					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();

						var val1 = JArray.Parse(contents);
						eventModel = JsonConvert.DeserializeObject<List<EventModel>>(contents.ToString());


						return eventModel;
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


			finally
			{

				StaticMethods.DismissLoader();
			}
			return eventModel;
		}
		public static List<JobModel> GetJobs()
		{

			List<JobModel> jobModel = null;

			try
			{
				string url = Constants.BaseUrlFull + "GetJob";
				HttpResponseMessage response = null;

				JObject j = new JObject();
				j.Add("empid", StaticDataModel.UserId);
				j.Add("deviceid", "001");
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var header = client.DefaultRequestHeaders;
				if (!header.Contains("Authorization"))
					client.DefaultRequestHeaders.Add("Authorization", "bearer " + StaticDataModel.AccessToken);
				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{

					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var val1 = JArray.Parse(contents);
						jobModel = JsonConvert.DeserializeObject<List<JobModel>>(contents.ToString());

						return jobModel;
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


			finally
			{

				StaticMethods.DismissLoader();
			}
			return jobModel;
		}
		public static string SubmitAttendance(double lat, double lng, byte[] image, string event_code, string job_code)
		{

			var ret = string.Empty;
			var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			try
			{
				string url = Constants.BaseUrlFull + "TransactionPost";
				HttpResponseMessage response = null;

				JObject j = new JObject();
				j.Add("empid", StaticDataModel.UserId);
				j.Add("deviceid", "001");
				j.Add("gps", lat.ToString() + "," + lng.ToString());
				j.Add("TADateTime", datetime);
				j.Add("photo", Convert.ToBase64String(image));
				j.Add("photocontenttype", "image/jpg");
				j.Add("eventcode", event_code);
				j.Add("jobcode", job_code);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var header = client.DefaultRequestHeaders;
				if (!header.Contains("Authorization"))
					client.DefaultRequestHeaders.Add("Authorization", "bearer " + StaticDataModel.AccessToken);
				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{

					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
var jarray = JArray.Parse(contents);
var val = jarray[0].ToString();
var jObj = JObject.Parse(val);
					var result=	jObj["returnvalue"].ToString();
						if (result == "Success;")
						{
							return result;
						}
						else
						{ 
var errorCode = jObj["errorcode"].ToString();

						if (!string.IsNullOrEmpty(contents))
							GetErrorDetails(contents,errorCode);
						else
							StaticMethods.ShowToast("Failed to Upload attendance!, Please try again later.");
					
						}
						//var val1 = JArray.Parse(contents);
						}
				}
			}
			catch (Exception ex)
			{
				//return null; 
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				StaticMethods.ShowToast(ex.Message);
				return "";
			}


			finally
			{

				StaticMethods.DismissLoader();
			}
			return "";
		}
public static string SubmitAttendance(string gpsLocation, string  image, string event_code, string job_code)
{

	var ret = string.Empty;
	var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

	try
	{
		string url = Constants.BaseUrlFull + "TransactionPost";
		HttpResponseMessage response = null;

		JObject j = new JObject();
		j.Add("empid", StaticDataModel.UserId);
		j.Add("deviceid", "001");
		j.Add("gps", gpsLocation);
		j.Add("TADateTime", datetime);
		j.Add("photo", image);
		j.Add("photocontenttype", "image/jpg");
		j.Add("eventcode", event_code);
		j.Add("jobcode", job_code);
		var json = JsonConvert.SerializeObject(j);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var header = client.DefaultRequestHeaders;
		if (!header.Contains("Authorization"))
			client.DefaultRequestHeaders.Add("Authorization", "bearer " + StaticDataModel.AccessToken);
		response = client.PostAsync(url, content).Result;
		if (response.IsSuccessStatusCode)
		{

			using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
			{
				var contents = reader.ReadToEnd();
				var jarray = JArray.Parse(contents);
				var val = jarray[0].ToString();
				var jObj = JObject.Parse(val);
				var result = jObj["returnvalue"].ToString();
				if (result == "Success;")
				{
					return result;
				}
				else
				{
					var errorCode = jObj["errorcode"].ToString();

					if (!string.IsNullOrEmpty(contents))
						GetErrorDetails(contents, errorCode);
					else
						StaticMethods.ShowToast("Failed to Upload attendance!, Please try again later.");

				}
				//var val1 = JArray.Parse(contents);
			}
		}
	}
	catch (Exception ex)
	{
		//return null; 
		Debug.WriteLine(@"ERROR {0}", ex.Message);
		StaticMethods.ShowToast(ex.Message);
		return "";
	}


	finally
	{

		StaticMethods.DismissLoader();
	}
	return "";
		}
		public static List<ErrorCodeModel> GetErrorDetails(string json,string error_code)
		{

			List<ErrorCodeModel> jobModel = null;

			try
			{
				string url = Constants.BaseUrlFull + "ErrorCode";
				HttpResponseMessage response = null;




				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var header = client.DefaultRequestHeaders;
				if (!header.Contains("Authorization"))
					client.DefaultRequestHeaders.Add("Authorization", "bearer " + StaticDataModel.AccessToken);
				response = client.PostAsync(url, content).Result;
				if (response.IsSuccessStatusCode)
				{

					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var val1 = JArray.Parse(contents);
						jobModel = JsonConvert.DeserializeObject<List<ErrorCodeModel>>(contents.ToString());
						for (int i = 0; i < jobModel.Count; i++)
						{
							var array = jobModel[i].code.Split(':');
							var code = array[1];
							if (error_code == code)
							{
								var arr = jobModel[i].description.Split(':');
								var desc = arr[1];
								StaticMethods.ShowToast(desc);
							}
						}
						return jobModel;
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


			finally
			{

				StaticMethods.DismissLoader();
			}
			return jobModel;
		}

	}
}
