using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

namespace AeLa.EasyFeedback.Web
{
	internal static class WebInterface
	{
		public static WebResponse Get(string uri, Action<AsyncWebRequestData> onStatusUpdate = null)
		{
			return WaitForResponse(MakeGet(uri), onStatusUpdate);
		}

		public static IEnumerator GetCoroutine(string uri, Action<WebResponse> onResponseReturned)
		{
			return WaitForResponseCoroutine(MakeGet(uri), onResponseReturned);
		}

		public static WebResponse Post(string uri, WWWForm data, Action<AsyncWebRequestData> onStatusUpdate = null)
		{
			return WaitForResponse(MakePost(uri, data), onStatusUpdate);
		}

		public static WebResponse Post(string uri, string contentType, byte[] data, Action<AsyncWebRequestData> onStatusUpdate = null)
		{
			return WaitForResponse(MakePost(uri, contentType, data), onStatusUpdate);
		}

		public static IEnumerator PostCoroutine(string uri, WWWForm data, Action<WebResponse> onResponseReturned)
		{
			return WaitForResponseCoroutine(MakePost(uri, data), onResponseReturned);
		}

		public static IEnumerator PostCoroutine(string uri, string contentType, byte[] data, Action<WebResponse> onResponseReturned)
		{
			return WaitForResponseCoroutine(MakePost(uri, contentType, data), onResponseReturned);
		}

		public static WebResponse Put(string uri, string contentType = null, byte[] data = null, Action<AsyncWebRequestData> onStatusUpdate = null)
		{
			return WaitForResponse(MakePut(uri, contentType, data), onStatusUpdate);
		}

		public static IEnumerator PutCoroutine(string uri, string contentType, byte[] data, Action<WebResponse> onResponseReturned)
		{
			return WaitForResponseCoroutine(MakePut(uri, contentType, data), onResponseReturned);
		}

		private static AsyncWebRequestData MakeGet(string uri)
		{
			return MakeRequest(uri, WebRequestMethod.GET);
		}

		private static AsyncWebRequestData MakePost(string uri, WWWForm data)
		{
			return MakeRequest(uri, data);
		}

		private static AsyncWebRequestData MakePost(string uri, string contentType, byte[] data)
		{
			return MakeRequest(uri, WebRequestMethod.POST, contentType, data);
		}

		private static AsyncWebRequestData MakePut(string uri, string contentType = null, byte[] data = null)
		{
			return MakeRequest(uri, WebRequestMethod.PUT, contentType, data);
		}

		private static AsyncWebRequestData MakeRequest(string uri, WebRequestMethod method, string contentType = null, byte[] data = null)
		{
			CheckCertificateValidationCallback();
			UnityWebRequest request = ConstructWebRequest(uri, method, contentType, data);
			return new AsyncWebRequestData(request, SendWebRequest(request));
		}

		private static AsyncWebRequestData MakeRequest(string uri, WWWForm data)
		{
			CheckCertificateValidationCallback();
			UnityWebRequest unityWebRequest = ConstructWebRequest(uri, data);
			unityWebRequest.chunkedTransfer = false;
			return new AsyncWebRequestData(unityWebRequest, SendWebRequest(unityWebRequest));
		}

		private static WebResponse WaitForResponse(AsyncWebRequestData requestData, Action<AsyncWebRequestData> onStatusUpdate = null)
		{
			while (!requestData.OperationIsDone)
			{
				onStatusUpdate?.Invoke(requestData);
			}
			return WebResponse.GetResponse(requestData);
		}

		private static IEnumerator WaitForResponseCoroutine(AsyncWebRequestData requestData, Action<WebResponse> onResponseReturned = null)
		{
			while (!requestData.OperationIsDone)
			{
				yield return null;
			}
			onResponseReturned?.Invoke(WebResponse.GetResponse(requestData));
			requestData.Request.Dispose();
		}

		private static UnityWebRequest ConstructWebRequest(string uri, WebRequestMethod method, string contentType = null, byte[] data = null)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, GetRequestMethodString(method))
			{
				downloadHandler = new DownloadHandlerBuffer()
			};
			if (data != null)
			{
				UploadHandler uploadHandler = new UploadHandlerRaw(data);
				uploadHandler.contentType = contentType;
				unityWebRequest.uploadHandler = uploadHandler;
			}
			else if (contentType != null)
			{
				unityWebRequest.SetRequestHeader("Content-Type", contentType);
			}
			return unityWebRequest;
		}

		private static UnityWebRequest ConstructWebRequest(string uri, WWWForm data)
		{
			return UnityWebRequest.Post(uri, data);
		}

		private static AsyncOperation SendWebRequest(UnityWebRequest request)
		{
			return request.SendWebRequest();
		}

		private static string GetRequestMethodString(WebRequestMethod method)
		{
			return method switch
			{
				WebRequestMethod.GET => "GET", 
				WebRequestMethod.POST => "POST", 
				WebRequestMethod.PUT => "PUT", 
				_ => throw new NotImplementedException("No string conversion for provided WebRequestMethod value."), 
			};
		}

		private static void CheckCertificateValidationCallback()
		{
			if (ServicePointManager.ServerCertificateValidationCallback != new RemoteCertificateValidationCallback(RemoteCertificateValidationCallback))
			{
				ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
			}
		}

		private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
	}
}
