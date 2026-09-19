using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Google.Apis.Auth
{
	public class JsonWebToken
	{
		public class Header
		{
			[JsonProperty("typ")]
			public string Type { get; set; }

			[JsonProperty("cty")]
			public string ContentType { get; set; }
		}

		public class Payload
		{
			[JsonProperty("iss")]
			public string Issuer { get; set; }

			[JsonProperty("sub")]
			public string Subject { get; set; }

			[JsonProperty("aud")]
			public object Audience { get; set; }

			[JsonProperty("target_audience")]
			public string TargetAudience { get; set; }

			[JsonProperty("exp")]
			public long? ExpirationTimeSeconds { get; set; }

			[JsonProperty("nbf")]
			public long? NotBeforeTimeSeconds { get; set; }

			[JsonProperty("iat")]
			public long? IssuedAtTimeSeconds { get; set; }

			[JsonProperty("jti")]
			public string JwtId { get; set; }

			[JsonProperty("nonce")]
			public string Nonce { get; set; }

			[JsonProperty("typ")]
			public string Type { get; set; }

			[JsonIgnore]
			public IEnumerable<string> AudienceAsList
			{
				get
				{
					if (Audience is List<string> result)
					{
						return result;
					}
					List<string> list = new List<string>();
					if (Audience is string item)
					{
						list.Add(item);
					}
					return list;
				}
			}

			[JsonIgnore]
			internal DateTimeOffset? IssuedAt
			{
				get
				{
					if (IssuedAtTimeSeconds.HasValue)
					{
						DateTime unixEpoch = UnixEpoch;
						return unixEpoch.AddSeconds(IssuedAtTimeSeconds.Value);
					}
					return null;
				}
			}

			[JsonIgnore]
			internal DateTimeOffset? ExpiresAt
			{
				get
				{
					if (ExpirationTimeSeconds.HasValue)
					{
						DateTime unixEpoch = UnixEpoch;
						return unixEpoch.AddSeconds(ExpirationTimeSeconds.Value);
					}
					return null;
				}
			}
		}

		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
