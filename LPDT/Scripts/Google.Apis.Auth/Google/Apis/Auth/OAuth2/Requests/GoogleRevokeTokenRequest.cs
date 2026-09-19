using System;
using Google.Apis.Requests;
using Google.Apis.Requests.Parameters;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	internal class GoogleRevokeTokenRequest
	{
		private readonly Uri revokeTokenUrl;

		public Uri RevokeTokenUrl => revokeTokenUrl;

		[RequestParameter("token")]
		public string Token { get; set; }

		public GoogleRevokeTokenRequest(Uri revokeTokenUrl)
		{
			this.revokeTokenUrl = revokeTokenUrl;
		}

		public Uri Build()
		{
			RequestBuilder obj = new RequestBuilder
			{
				BaseUri = revokeTokenUrl
			};
			ParameterUtils.InitParameters(obj, this);
			return obj.BuildUri();
		}
	}
}
