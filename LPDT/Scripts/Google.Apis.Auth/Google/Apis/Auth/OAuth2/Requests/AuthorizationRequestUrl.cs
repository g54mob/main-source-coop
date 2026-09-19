using System;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class AuthorizationRequestUrl
	{
		private readonly Uri authorizationServerUrl;

		[RequestParameter("response_type", RequestParameterType.Query)]
		public string ResponseType { get; set; }

		[RequestParameter("client_id", RequestParameterType.Query)]
		public string ClientId { get; set; }

		[RequestParameter("redirect_uri", RequestParameterType.Query)]
		public string RedirectUri { get; set; }

		[RequestParameter("scope", RequestParameterType.Query)]
		public string Scope { get; set; }

		[RequestParameter("state", RequestParameterType.Query)]
		public string State { get; set; }

		public Uri AuthorizationServerUrl => authorizationServerUrl;

		public AuthorizationRequestUrl(Uri authorizationServerUrl)
		{
			this.authorizationServerUrl = authorizationServerUrl;
		}
	}
}
