using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Foundation.Tasks;
using GameAnalyticsSDK.Net.Http;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.Utilities;

namespace GameAnalyticsSDK.Net.Tasks
{
	internal class SdkErrorTask
	{
		protected EGASdkErrorType type;

		protected byte[] payloadData;

		protected string hashHmac;

		protected string body = "";

		private const int MaxCount = 10;

		private static Dictionary<EGASdkErrorType, int> countMap = new Dictionary<EGASdkErrorType, int>();

		public SdkErrorTask(EGASdkErrorType type, byte[] payloadData, string secretKey)
		{
			this.type = type;
			this.payloadData = payloadData;
			hashHmac = GAUtilities.HmacWithKey(secretKey, payloadData);
		}

		public void Execute(string url)
		{
			AsyncTask.Run(delegate
			{
				DoInBackground(url);
			});
		}

		protected void DoInBackground(string url)
		{
			if (!countMap.ContainsKey(type))
			{
				countMap.Add(type, 0);
			}
			if (countMap[type] >= 10)
			{
				return;
			}
			HttpStatusCode responseCode = (HttpStatusCode)0;
			string responseDescription = "";
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentLength = payloadData.Length;
				httpWebRequest.Headers[HttpRequestHeader.Authorization] = hashHmac;
				httpWebRequest.ContentType = "application/json";
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(payloadData, 0, payloadData.Length);
				}
				using HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				using Stream stream2 = httpWebResponse.GetResponseStream();
				using StreamReader streamReader = new StreamReader(stream2);
				string text = streamReader.ReadToEnd();
				body = text;
			}
			catch (WebException ex)
			{
				if (ex.Response != null)
				{
					using HttpWebResponse httpWebResponse2 = (HttpWebResponse)ex.Response;
					using Stream stream3 = httpWebResponse2.GetResponseStream();
					using StreamReader streamReader2 = new StreamReader(stream3);
					string text2 = streamReader2.ReadToEnd();
					responseCode = httpWebResponse2.StatusCode;
					responseDescription = httpWebResponse2.StatusDescription;
					body = text2;
				}
			}
			catch (Exception ex2)
			{
				GALogger.E(ex2.ToString());
			}
			GALogger.D("sdk error request content : " + body);
			OnPostExecute(responseCode, responseDescription);
		}

		protected void OnPostExecute(HttpStatusCode responseCode, string responseDescription)
		{
			if (string.IsNullOrEmpty(body))
			{
				GALogger.D("sdk error failed. Might be no connection. Description: " + responseDescription + ", Status code: " + responseCode);
			}
			else if (responseCode != HttpStatusCode.OK)
			{
				GALogger.W("sdk error failed. response code not 200. status code: " + responseCode.ToString() + ", description: " + responseDescription + ", body: " + body);
			}
			else
			{
				countMap[type] += 1;
			}
		}
	}
}
