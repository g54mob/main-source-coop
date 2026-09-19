using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.State;
using GameAnalyticsSDK.Net.Tasks;
using GameAnalyticsSDK.Net.Utilities;
using GameAnalyticsSDK.Net.Validators;

namespace GameAnalyticsSDK.Net.Http
{
	internal class GAHTTPApi
	{
		private static readonly GAHTTPApi _instance = new GAHTTPApi();

		private static string protocol = "https";

		private static string hostName = "api.gameanalytics.com";

		private static string version = "v2";

		private static string remoteConfigsVersion = "v1";

		private static string baseUrl = getBaseUrl();

		private static string remoteConfigsBaseUrl = getRemoteConfigsBaseUrl();

		private static string initializeUrlPath = "init";

		private static string eventsUrlPath = "events";

		private bool useGzip;

		public static GAHTTPApi Instance => _instance;

		private static string getBaseUrl()
		{
			return protocol + "://" + hostName + "/" + version;
		}

		private static string getRemoteConfigsBaseUrl()
		{
			return protocol + "://" + hostName + "/remote_configs/" + remoteConfigsVersion;
		}

		private GAHTTPApi()
		{
			useGzip = true;
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		}

		private bool MyRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool result = true;
			if (sslPolicyErrors != SslPolicyErrors.None)
			{
				for (int i = 0; i < chain.ChainStatus.Length; i++)
				{
					if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
					{
						chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
						chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
						if (!chain.Build((X509Certificate2)certificate))
						{
							result = false;
						}
					}
				}
			}
			return result;
		}

		public KeyValuePair<EGAHTTPApiResponse, JSONObject> RequestInitReturningDict(string configsHash)
		{
			string gameKey = GAState.GameKey;
			string text = remoteConfigsBaseUrl + "/" + initializeUrlPath + "?game_key=" + gameKey + "&interval_seconds=0&configs_hash=" + configsHash;
			GALogger.D("Sending 'init' URL: " + text);
			string text2 = GAState.GetInitAnnotations().ToString();
			JSONObject value;
			if (string.IsNullOrEmpty(text2))
			{
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(EGAHTTPApiResponse.JsonEncodeFailed, value);
			}
			string text3 = "";
			HttpStatusCode responseCode = (HttpStatusCode)0;
			string responseMessage = "";
			string text4 = "";
			try
			{
				byte[] array = CreatePayloadData(text2, gzip: false);
				HttpWebRequest httpWebRequest = CreateRequest(text, array, gzip: false);
				text4 = httpWebRequest.Headers[HttpRequestHeader.Authorization];
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(array, 0, array.Length);
				}
				using HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				using Stream stream2 = httpWebResponse.GetResponseStream();
				using StreamReader streamReader = new StreamReader(stream2);
				string text5 = streamReader.ReadToEnd();
				responseCode = httpWebResponse.StatusCode;
				responseMessage = httpWebResponse.StatusDescription;
				text3 = text5;
			}
			catch (WebException ex)
			{
				if (ex.Response != null)
				{
					using HttpWebResponse httpWebResponse2 = (HttpWebResponse)ex.Response;
					using Stream stream3 = httpWebResponse2.GetResponseStream();
					using StreamReader streamReader2 = new StreamReader(stream3);
					string text6 = streamReader2.ReadToEnd();
					responseCode = httpWebResponse2.StatusCode;
					responseMessage = httpWebResponse2.StatusDescription;
					text3 = text6;
				}
			}
			catch (Exception ex2)
			{
				GALogger.E(ex2.ToString());
			}
			GALogger.D("init request content : " + text3 + ", JSONstring: " + text2);
			JSONNode jSONNode = JSON.Parse(text3);
			EGAHTTPApiResponse eGAHTTPApiResponse = ProcessRequestResponse(responseCode, responseMessage, text3, "Init");
			if (eGAHTTPApiResponse != EGAHTTPApiResponse.Ok && eGAHTTPApiResponse != EGAHTTPApiResponse.Created && eGAHTTPApiResponse != EGAHTTPApiResponse.BadRequest)
			{
				GALogger.D("Failed Init Call. URL: " + text + ", Authorization: " + text4 + ", JSONString: " + text2);
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(eGAHTTPApiResponse, value);
			}
			if (jSONNode == null)
			{
				GALogger.D("Failed Init Call. Json decoding failed");
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(EGAHTTPApiResponse.JsonDecodeFailed, value);
			}
			if (eGAHTTPApiResponse == EGAHTTPApiResponse.BadRequest)
			{
				GALogger.D("Failed Init Call. Bad request. Response: " + jSONNode.ToString());
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(eGAHTTPApiResponse, value);
			}
			JSONObject jSONObject = GAValidator.ValidateAndCleanInitRequestResponse(jSONNode, eGAHTTPApiResponse == EGAHTTPApiResponse.Created);
			if (jSONObject == null)
			{
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(EGAHTTPApiResponse.BadResponse, value);
			}
			value = jSONObject;
			return new KeyValuePair<EGAHTTPApiResponse, JSONObject>(eGAHTTPApiResponse, value);
		}

		public KeyValuePair<EGAHTTPApiResponse, JSONNode> SendEventsInArray(List<JSONNode> eventArray)
		{
			if (eventArray.Count == 0)
			{
				GALogger.D("sendEventsInArray called with missing eventArray");
			}
			string gameKey = GAState.GameKey;
			string text = baseUrl + "/" + gameKey + "/" + eventsUrlPath;
			GALogger.D("Sending 'events' URL: " + text);
			string text2 = GAUtilities.ArrayOfObjectsToJsonString(eventArray);
			JSONNode value;
			if (text2.Length == 0)
			{
				GALogger.D("sendEventsInArray JSON encoding failed of eventArray");
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONNode>(EGAHTTPApiResponse.JsonEncodeFailed, value);
			}
			string text3 = "";
			HttpStatusCode responseCode = (HttpStatusCode)0;
			string responseMessage = "";
			string text4 = "";
			try
			{
				byte[] array = CreatePayloadData(text2, useGzip);
				HttpWebRequest httpWebRequest = CreateRequest(text, array, useGzip);
				text4 = httpWebRequest.Headers[HttpRequestHeader.Authorization];
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(array, 0, array.Length);
				}
				using HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				using Stream stream2 = httpWebResponse.GetResponseStream();
				using StreamReader streamReader = new StreamReader(stream2);
				string text5 = streamReader.ReadToEnd();
				responseCode = httpWebResponse.StatusCode;
				responseMessage = httpWebResponse.StatusDescription;
				text3 = text5;
			}
			catch (WebException ex)
			{
				if (ex.Response != null)
				{
					using HttpWebResponse httpWebResponse2 = (HttpWebResponse)ex.Response;
					using Stream stream3 = httpWebResponse2.GetResponseStream();
					using StreamReader streamReader2 = new StreamReader(stream3);
					string text6 = streamReader2.ReadToEnd();
					responseCode = httpWebResponse2.StatusCode;
					responseMessage = httpWebResponse2.StatusDescription;
					text3 = text6;
				}
			}
			catch (Exception ex2)
			{
				GALogger.E(ex2.ToString());
			}
			GALogger.D("events request content: " + text3);
			EGAHTTPApiResponse eGAHTTPApiResponse = ProcessRequestResponse(responseCode, responseMessage, text3, "Events");
			if (eGAHTTPApiResponse != EGAHTTPApiResponse.Ok && eGAHTTPApiResponse != EGAHTTPApiResponse.Created && eGAHTTPApiResponse != EGAHTTPApiResponse.BadRequest)
			{
				GALogger.D("Failed events Call. URL: " + text + ", Authorization: " + text4 + ", JSONString: " + text2);
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONNode>(eGAHTTPApiResponse, value);
			}
			JSONNode jSONNode = JSON.Parse(text3);
			if (jSONNode == null)
			{
				value = null;
				return new KeyValuePair<EGAHTTPApiResponse, JSONNode>(EGAHTTPApiResponse.JsonDecodeFailed, value);
			}
			if (eGAHTTPApiResponse == EGAHTTPApiResponse.BadRequest)
			{
				GALogger.D("Failed Events Call. Bad request. Response: " + jSONNode.ToString());
			}
			value = jSONNode;
			return new KeyValuePair<EGAHTTPApiResponse, JSONNode>(eGAHTTPApiResponse, value);
		}

		public void SendSdkErrorEvent(EGASdkErrorType type)
		{
			if (!GAState.IsEventSubmissionEnabled)
			{
				return;
			}
			string gameKey = GAState.GameKey;
			string gameSecret = GAState.GameSecret;
			if (GAValidator.ValidateSdkErrorEvent(gameKey, gameSecret, type))
			{
				string text = baseUrl + "/" + gameKey + "/" + eventsUrlPath;
				GALogger.D("Sending 'events' URL: " + text);
				string text2 = "";
				JSONObject sdkErrorEventAnnotations = GAState.GetSdkErrorEventAnnotations();
				string text3 = SdkErrorTypeToString(type);
				sdkErrorEventAnnotations.Add("type", text3);
				text2 = GAUtilities.ArrayOfObjectsToJsonString(new List<JSONNode> { sdkErrorEventAnnotations });
				if (string.IsNullOrEmpty(text2))
				{
					GALogger.W("sendSdkErrorEvent: JSON encoding failed.");
					return;
				}
				GALogger.D("sendSdkErrorEvent json: " + text2);
				byte[] bytes = Encoding.UTF8.GetBytes(text2);
				new SdkErrorTask(type, bytes, gameSecret).Execute(text);
			}
		}

		private byte[] CreatePayloadData(string payload, bool gzip)
		{
			byte[] array;
			if (gzip)
			{
				array = GAUtilities.GzipCompress(payload);
				GALogger.D("Gzip stats. Size: " + Encoding.UTF8.GetBytes(payload).Length + ", Compressed: " + array.Length + ", Content: " + payload);
			}
			else
			{
				array = Encoding.UTF8.GetBytes(payload);
			}
			return array;
		}

		private static string SdkErrorTypeToString(EGASdkErrorType value)
		{
			if (value == EGASdkErrorType.Rejected)
			{
				return "rejected";
			}
			return "";
		}

		private HttpWebRequest CreateRequest(string url, byte[] payloadData, bool gzip)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentLength = payloadData.Length;
			if (gzip)
			{
				httpWebRequest.Headers[HttpRequestHeader.ContentEncoding] = "gzip";
			}
			string gameSecret = GAState.GameSecret;
			httpWebRequest.Headers[HttpRequestHeader.Authorization] = GAUtilities.HmacWithKey(gameSecret, payloadData);
			httpWebRequest.ContentType = "application/json";
			return httpWebRequest;
		}

		private EGAHTTPApiResponse ProcessRequestResponse(HttpStatusCode responseCode, string responseMessage, string body, string requestId)
		{
			if (string.IsNullOrEmpty(body))
			{
				GALogger.D(requestId + " request. failed. Might be no connection. Description: " + responseMessage + ", Status code: " + responseCode);
				return EGAHTTPApiResponse.NoResponse;
			}
			switch (responseCode)
			{
			case HttpStatusCode.OK:
				return EGAHTTPApiResponse.Ok;
			case HttpStatusCode.Created:
				return EGAHTTPApiResponse.Created;
			case (HttpStatusCode)0:
			case HttpStatusCode.Unauthorized:
				GALogger.D(requestId + " request. 401 - Unauthorized.");
				return EGAHTTPApiResponse.Unauthorized;
			case HttpStatusCode.BadRequest:
				GALogger.D(requestId + " request. 400 - Bad Request.");
				return EGAHTTPApiResponse.BadRequest;
			case HttpStatusCode.InternalServerError:
				GALogger.D(requestId + " request. 500 - Internal Server Error.");
				return EGAHTTPApiResponse.InternalServerError;
			default:
				return EGAHTTPApiResponse.UnknownResponseCode;
			}
		}
	}
}
