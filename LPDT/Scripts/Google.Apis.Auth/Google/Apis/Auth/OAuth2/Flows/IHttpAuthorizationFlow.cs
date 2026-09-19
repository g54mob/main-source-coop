using System;
using Google.Apis.Http;

namespace Google.Apis.Auth.OAuth2.Flows
{
	internal interface IHttpAuthorizationFlow : IAuthorizationCodeFlow, IDisposable
	{
		IHttpAuthorizationFlow WithHttpClientFactory(IHttpClientFactory httpClientFactory);
	}
}
