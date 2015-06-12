using System;
using System.Threading;
using System.Net.Http;
using System.IO;

namespace LinkOM
{
	public class LoginService
	{
		public LoginObject Login(string username,string password)
		{
			LoginObject obj = new LoginObject ();

			string url = Settings.InstanceURL;
			
			url=url+"/api/logon";

			var logon = new
			{
				Item = new
				{
					UserName = username,
					Password = password
				}
			};

			try {

				string results= ConnectWebAPI.Request(url,logon);

				obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginObject> (results);

				return obj;
				
			} catch (Exception ex) {

				return null;
			}
		}

//		public async System.Threading.Tasks.Task<string> GetCommomImageAsync(int? FileId, double? width, double? height)
//		{
//			string toReturn = string.Empty;
//
//			StreamContent responseContent;
//
//			byte[] imgByte;
//
//			string methodName = string.Empty;
//			string fileName = string.Empty;
//
//			try
//			{
//				if (FileId.HasValue)
//				{
//					fileName = FileId.ToString() + ".png";
//					methodName = "/FileReference/ServeImage?fileId=" + FileId + "&width=" + width + "&height=" + height + "";
//				}
//				else
//				{
//					Random r = new Random();
//					int randnumber = r.Next();
//
//					fileName = randnumber.ToString() + "-Logo" + ".png";
//					methodName = "/FileReference/GetLogoImage";
//				}
//
//				HttpClient httpClient = new HttpClient();
//
//				using (HttpResponseMessage response = await httpClient.GetAsync(methodName).ConfigureAwait(false))
//				{
//					if (response.IsSuccessStatusCode)
//					{
//						responseContent = response.Content as StreamContent;
//						imgByte = await responseContent.ReadAsByteArrayAsync();
//
//						if (imgByte.Length > 0)
//						{
//							imageFile = await imageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
//
//							using (MemoryStream stream = new MemoryStream(imgByte, true))
//							{
//								using (Stream outputStream = await imageFile.OpenStreamForWriteAsync())
//								{
//									await stream.CopyToAsync(outputStream);
//								}
//							}
//
//							//await System.Threading.Tasks.Task.Delay(1000);
//							toReturn = imageFile.Path;
//						}
//					}
//				}
//			}
//			catch (Exception)
//			{
//				throw;
//			}
//
//			return toReturn;
//		}
	}
}

