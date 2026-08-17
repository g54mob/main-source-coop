using System;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayEveryWare.EpicOnlineServices
{
	public class AndroidFileIOHelper
	{
		public static bool FileExists(string filePath)
		{
			using UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
			unityWebRequest.timeout = 2;
			unityWebRequest.SendWebRequest();
			while (!unityWebRequest.isDone)
			{
			}
			bool num = unityWebRequest.result == UnityWebRequest.Result.Success;
			if (!num)
			{
				Debug.LogError("AndroidFileIOHelper says that \"" + filePath + "\" does not exist.");
			}
			return num;
		}

		public static string ReadAllText(string filePath)
		{
			using UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
			unityWebRequest.timeout = 2;
			unityWebRequest.SendWebRequest();
			while (!unityWebRequest.isDone)
			{
			}
			return ProcessRequest(filePath, unityWebRequest);
		}

		private static string ProcessRequest(string filePath, UnityWebRequest request)
		{
			string result = null;
			switch (request.result)
			{
			case UnityWebRequest.Result.InProgress:
				Debug.LogWarning("AndroidFileIOHelper: For some reason the request is still in progress.");
				break;
			case UnityWebRequest.Result.ConnectionError:
			case UnityWebRequest.Result.ProtocolError:
			case UnityWebRequest.Result.DataProcessingError:
				Debug.LogError("AndroidFileIOHelper: UnityWebRequest for path \"" + filePath + ",\" failed with error code " + $"'{request.result},' and error, " + "\"" + request.error + ".\"");
				break;
			case UnityWebRequest.Result.Success:
				result = request.downloadHandler.text;
				break;
			default:
			{
				ArgumentException ex = new ArgumentException(string.Format("Unrecognized result returned from {0}: {1}.", "UnityWebRequest", request.result));
				Debug.LogException(ex);
				throw ex;
			}
			}
			return result;
		}
	}
}
