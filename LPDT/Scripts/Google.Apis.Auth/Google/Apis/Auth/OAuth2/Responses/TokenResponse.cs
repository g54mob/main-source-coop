using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Json;
using Google.Apis.Logging;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2.Responses
{
	public class TokenResponse
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		internal const int TokenRefreshTimeWindowSeconds = 360;

		internal const int TokenHardExpiryTimeWindowSeconds = 300;

		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[JsonProperty("expires_in")]
		public long? ExpiresInSeconds { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("scope")]
		public string Scope { get; set; }

		[JsonProperty("id_token")]
		public string IdToken { get; set; }

		[Obsolete("Use IssuedUtc instead")]
		[JsonProperty(Order = 1)]
		public DateTime Issued
		{
			get
			{
				return IssuedUtc.ToLocalTime();
			}
			set
			{
				IssuedUtc = value.ToUniversalTime();
			}
		}

		[JsonProperty(Order = 2)]
		public DateTime IssuedUtc { get; set; }

		[JsonProperty("accessToken")]
		private string ImpersonatedAccessToken
		{
			set
			{
				AccessToken = value;
			}
		}

		[JsonProperty("token")]
		private string ImpersonatedIdToken
		{
			set
			{
				IdToken = value;
			}
		}

		[JsonProperty("expireTime")]
		private string ImpersonatedAccessTokenExpireTime { get; set; }

		public bool IsExpired(IClock clock)
		{
			if ((AccessToken != null || IdToken != null) && ExpiresInSeconds.HasValue)
			{
				return IssuedUtc.AddSeconds(ExpiresInSeconds.Value - 360) <= clock.UtcNow;
			}
			return true;
		}

		internal bool IsEffectivelyExpired(IClock clock)
		{
			if ((AccessToken != null || IdToken != null) && ExpiresInSeconds.HasValue)
			{
				return IssuedUtc.AddSeconds(ExpiresInSeconds.Value - 300) <= clock.UtcNow;
			}
			return true;
		}

		public static async Task<TokenResponse> FromHttpResponseAsync(HttpResponseMessage response, IClock clock, ILogger logger)
		{
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			string text2 = "";
			try
			{
				if (!response.IsSuccessStatusCode)
				{
					text2 = "TokenErrorResponse";
					HttpRequestMessage requestMessage = response.RequestMessage;
					throw new TokenResponseException((requestMessage != null && requestMessage.RequestUri?.AbsoluteUri.StartsWith("https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/") == true) ? new TokenErrorResponse
					{
						Error = text
					} : NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(text), response.StatusCode);
				}
				HttpRequestMessage requestMessage2 = response.RequestMessage;
				TokenResponse tokenResponse;
				if (requestMessage2 != null && requestMessage2.RequestUri?.AbsoluteUri.StartsWith(GoogleAuthConsts.EffectiveComputeOidcTokenUrl) == true)
				{
					tokenResponse = new TokenResponse
					{
						IdToken = text
					};
				}
				else
				{
					text2 = "TokenResponse";
					tokenResponse = NewtonsoftJsonSerializer.Instance.Deserialize<TokenResponse>(text);
				}
				tokenResponse.IssuedUtc = clock.UtcNow;
				TokenResponse tokenResponse2 = tokenResponse;
				if (tokenResponse2.AccessToken == null)
				{
					tokenResponse2.AccessToken = tokenResponse.IdToken;
				}
				if (!string.IsNullOrEmpty(tokenResponse.ImpersonatedAccessTokenExpireTime))
				{
					tokenResponse.ExpiresInSeconds = (long)(DateTimeOffset.Parse(tokenResponse.ImpersonatedAccessTokenExpireTime, CultureInfo.InvariantCulture) - clock.UtcNow).TotalSeconds;
				}
				if (!tokenResponse.ExpiresInSeconds.HasValue && tokenResponse.IdToken != null)
				{
					SignedToken<JsonWebSignature.Header, JsonWebSignature.Payload> signedToken = SignedToken<JsonWebSignature.Header, JsonWebSignature.Payload>.FromSignedToken(tokenResponse.IdToken);
					if (signedToken.Payload.ExpirationTimeSeconds.HasValue)
					{
						DateTime unixEpoch = UnixEpoch;
						DateTime dateTime = unixEpoch.AddSeconds(signedToken.Payload.ExpirationTimeSeconds.Value);
						tokenResponse.ExpiresInSeconds = (long)(dateTime - tokenResponse.IssuedUtc).TotalSeconds;
					}
				}
				return tokenResponse;
			}
			catch (JsonException exception)
			{
				logger.Error(exception, "Exception was caught when deserializing " + text2 + ". Content is: " + text);
				throw new TokenResponseException(new TokenErrorResponse
				{
					Error = "Server response does not contain a JSON object. Status code is: " + response.StatusCode
				}, response.StatusCode);
			}
		}
	}
}
