using System;
using Google.Apis.Http;
using Google.Apis.Requests;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public static class RequestExtensions
	{
		public static T AddCredential<T>(this T request, ICredential credential) where T : ClientServiceRequest
		{
			request.ThrowIfNull("request");
			credential.ThrowIfNull("credential");
			if (credential is GoogleCredential googleCredential)
			{
				credential = googleCredential.UnderlyingCredential;
			}
			if (!(credential is IHttpExecuteInterceptor credential2))
			{
				throw new ArgumentException("Credential must implement IHttpExecuteInterceptor.", "credential");
			}
			request.Credential = credential2;
			return request;
		}
	}
}
