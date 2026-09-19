using System;
using System.Net;

namespace Google.Apis.Auth.OAuth2.Responses
{
	public class TokenResponseException : Exception
	{
		public TokenErrorResponse Error { get; }

		public HttpStatusCode? StatusCode { get; }

		public TokenResponseException(TokenErrorResponse error)
			: this(error, null)
		{
		}

		public TokenResponseException(TokenErrorResponse error, HttpStatusCode? statusCode)
			: base(error.ToString())
		{
			Error = error;
			StatusCode = statusCode;
		}
	}
}
