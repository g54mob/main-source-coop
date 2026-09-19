using System;
using System.Collections.Generic;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2.Requests
{
	public class GoogleAuthorizationCodeRequestUrl : AuthorizationCodeRequestUrl
	{
		[RequestParameter("access_type", RequestParameterType.Query)]
		public string AccessType { get; set; }

		[RequestParameter("prompt", RequestParameterType.Query)]
		public string Prompt { get; set; }

		[RequestParameter("approval_prompt", RequestParameterType.Query)]
		[Obsolete("Unused for Google OpenID; use the 'Prompt' property instead.")]
		public string ApprovalPrompt { get; set; }

		[RequestParameter("login_hint", RequestParameterType.Query)]
		public string LoginHint { get; set; }

		[RequestParameter("include_granted_scopes", RequestParameterType.Query)]
		public string IncludeGrantedScopes { get; set; }

		[RequestParameter("nonce", RequestParameterType.Query)]
		public string Nonce { get; set; }

		[RequestParameter("user_defined_query_params", RequestParameterType.UserDefinedQueries)]
		public IEnumerable<KeyValuePair<string, string>> UserDefinedQueryParams { get; set; }

		public GoogleAuthorizationCodeRequestUrl(Uri authorizationServerUrl)
			: base(authorizationServerUrl)
		{
			AccessType = "offline";
		}
	}
}
