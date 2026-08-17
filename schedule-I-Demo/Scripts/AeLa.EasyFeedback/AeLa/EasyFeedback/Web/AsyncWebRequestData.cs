using UnityEngine;
using UnityEngine.Networking;

namespace AeLa.EasyFeedback.Web
{
	internal readonly struct AsyncWebRequestData
	{
		public UnityWebRequest Request { get; }

		public AsyncOperation Operation { get; }

		public bool OperationIsDone => Operation.isDone;

		public bool RequestIsError => Request.error != null;

		public string ErrorText
		{
			get
			{
				if (Request.result == UnityWebRequest.Result.ProtocolError)
				{
					return Request.downloadHandler.text;
				}
				if (RequestIsError)
				{
					return Request.error;
				}
				return string.Empty;
			}
		}

		public AsyncWebRequestData(UnityWebRequest request, AsyncOperation operation)
		{
			Request = request;
			Operation = operation;
		}
	}
}
