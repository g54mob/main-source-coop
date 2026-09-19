using System;
using Google.Apis.Requests;
using Google.Apis.Requests.Parameters;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class AuthorizationCodeRequestUrl : AuthorizationRequestUrl
	{
		public AuthorizationCodeRequestUrl(Uri authorizationServerUrl)
			: base(authorizationServerUrl)
		{
			base.ResponseType = "code";
		}

		public Uri Build()
		{
			RequestBuilder obj = new RequestBuilder
			{
				BaseUri = base.AuthorizationServerUrl
			};
			ParameterUtils.InitParameters(obj, this);
			return obj.BuildUri();
		}
	}
}
