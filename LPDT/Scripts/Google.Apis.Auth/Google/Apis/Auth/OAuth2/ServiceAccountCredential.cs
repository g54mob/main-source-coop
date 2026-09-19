using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Http;
using Google.Apis.Json;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public class ServiceAccountCredential : ServiceCredential, IOidcTokenProvider, IGoogleCredential, ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders, IBlobSigner
	{
		public new class Initializer : ServiceCredential.Initializer
		{
			public string Id { get; private set; }

			public string ProjectId { get; set; }

			public string User { get; set; }

			public IEnumerable<string> Scopes { get; set; }

			public RSA Key { get; set; }

			public string KeyId { get; set; }

			public bool UseJwtAccessWithScopes { get; set; }

			public Initializer(string id)
				: this(id, "https://oauth2.googleapis.com/token")
			{
			}

			public Initializer(string id, string tokenServerUrl)
				: base(tokenServerUrl)
			{
				Id = id;
				Scopes = new List<string>();
			}

			internal Initializer(ServiceAccountCredential other)
				: base(other)
			{
				Id = other.Id;
				ProjectId = other.ProjectId;
				User = other.User;
				Scopes = other.Scopes;
				Key = other.Key;
				KeyId = other.KeyId;
				UseJwtAccessWithScopes = other.UseJwtAccessWithScopes;
			}

			public Initializer FromPrivateKey(string privateKey)
			{
				RSAParameters parameters = Pkcs8.DecodeRsaParameters(privateKey);
				Key = RSA.Create();
				Key.ImportParameters(parameters);
				return this;
			}

			public Initializer FromCertificate(X509Certificate2 certificate)
			{
				Key = certificate.GetRSAPrivateKey();
				return this;
			}
		}

		private class JwtCacheEntry
		{
			public Task<string> JwtTask { get; }

			public string Uri { get; }

			public DateTime ExpiryUtc { get; }

			public JwtCacheEntry(Task<string> jwtTask, string uri, DateTime expiryUtc)
			{
				JwtTask = jwtTask;
				Uri = uri;
				ExpiryUtc = expiryUtc;
			}
		}

		private const string Sha256Oid = "2.16.840.1.101.3.4.2.1";

		private const string ScopedTokenCacheKey = "SCOPED_TOKEN";

		protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		internal static readonly TimeSpan JwtLifetime = TimeSpan.FromMinutes(60.0);

		internal static readonly TimeSpan JwtCacheExpiryWindow = TimeSpan.FromMinutes(5.0);

		internal const int JwtCacheMaxSize = 100;

		private readonly object _jwtLock = new object();

		private LinkedList<JwtCacheEntry> _jwts;

		private Dictionary<string, LinkedListNode<JwtCacheEntry>> _jwtCache;

		public string Id { get; }

		public string ProjectId { get; }

		public string User { get; }

		public IEnumerable<string> Scopes { get; }

		public RSA Key { get; }

		public string KeyId { get; }

		public bool UseJwtAccessWithScopes { get; }

		internal bool HasExplicitScopes => Scopes?.Any() ?? false;

		bool IGoogleCredential.HasExplicitScopes => HasExplicitScopes;

		bool IGoogleCredential.SupportsExplicitScopes => true;

		public ServiceAccountCredential(Initializer initializer)
			: base(initializer)
		{
			Id = initializer.Id.ThrowIfNullOrEmpty("initializer.Id");
			ProjectId = initializer.ProjectId;
			User = initializer.User;
			Scopes = initializer.Scopes?.ToList().AsReadOnly() ?? Enumerable.Empty<string>();
			Key = initializer.Key.ThrowIfNull("initializer.Key");
			KeyId = initializer.KeyId;
			UseJwtAccessWithScopes = initializer.UseJwtAccessWithScopes;
		}

		public static ServiceAccountCredential FromServiceAccountData(Stream credentialData)
		{
			return (GoogleCredential.FromStream(credentialData).UnderlyingCredential as ServiceAccountCredential) ?? throw new InvalidOperationException("JSON data does not represent a valid service account credential.");
		}

		public ServiceAccountCredential WithUseJwtAccessWithScopes(bool useJwtAccessWithScopes)
		{
			return new ServiceAccountCredential(new Initializer(this)
			{
				UseJwtAccessWithScopes = useJwtAccessWithScopes
			});
		}

		IGoogleCredential IGoogleCredential.WithQuotaProject(string quotaProject)
		{
			return new ServiceAccountCredential(new Initializer(this)
			{
				QuotaProject = quotaProject
			});
		}

		IGoogleCredential IGoogleCredential.MaybeWithScopes(IEnumerable<string> scopes)
		{
			return new ServiceAccountCredential(new Initializer(this)
			{
				Scopes = scopes
			});
		}

		IGoogleCredential IGoogleCredential.WithUserForDomainWideDelegation(string user)
		{
			return new ServiceAccountCredential(new Initializer(this)
			{
				User = user
			});
		}

		IGoogleCredential IGoogleCredential.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			return new ServiceAccountCredential(new Initializer(this)
			{
				HttpClientFactory = httpClientFactory
			});
		}

		public override async Task<bool> RequestAccessTokenAsync(CancellationToken taskCancellationToken)
		{
			GoogleAssertionTokenRequest googleAssertionTokenRequest = new GoogleAssertionTokenRequest
			{
				Assertion = CreateAssertionFromPayload(CreatePayload())
			};
			ServiceCredential.Logger.Debug("Request a new access token. Assertion data is: " + googleAssertionTokenRequest.Assertion);
			base.Token = await googleAssertionTokenRequest.ExecuteAsync(base.HttpClient, base.TokenServerUrl, taskCancellationToken, base.Clock, ServiceCredential.Logger).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		public override async Task<string> GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!HasExplicitScopes && authUri == null)
			{
				throw new GoogleApiException(base.TokenServerUrl, "Invalid OAuth scope or ID token audience provided. A valid authUri and/or OAuth scope is required to proceed.");
			}
			if (User != null || (HasExplicitScopes && !UseJwtAccessWithScopes))
			{
				return await base.GetAccessTokenForRequestAsync(authUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			string jwtKey = (HasExplicitScopes ? "SCOPED_TOKEN" : authUri);
			return await GetOrCreateJwtAccessTokenAsync(authUri, jwtKey).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<OidcToken> GetOidcTokenAsync(OidcTokenOptions options, CancellationToken cancellationToken = default(CancellationToken))
		{
			options.ThrowIfNull("options");
			TokenRefreshManager tokenRefreshManager = null;
			tokenRefreshManager = new TokenRefreshManager((CancellationToken ct) => RefreshOidcTokenAsync(tokenRefreshManager, options, ct), base.Clock, ServiceCredential.Logger);
			return Task.FromResult(new OidcToken(tokenRefreshManager));
		}

		private async Task<bool> RefreshOidcTokenAsync(TokenRefreshManager caller, OidcTokenOptions options, CancellationToken cancellationToken)
		{
			DateTime utcNow = base.Clock.UtcNow;
			DateTime expiryUtc = utcNow + JwtLifetime;
			string assertion = CreateJwtAccessTokenForOidc(options, utcNow, expiryUtc);
			caller.Token = await new GoogleAssertionTokenRequest
			{
				Assertion = assertion
			}.ExecuteAsync(base.HttpClient, base.TokenServerUrl, cancellationToken, base.Clock, ServiceCredential.Logger).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		private Task<string> GetOrCreateJwtAccessTokenAsync(string authUri, string jwtKey)
		{
			DateTime nowUtc = base.Clock.UtcNow;
			lock (_jwtLock)
			{
				if (_jwtCache == null)
				{
					_jwtCache = new Dictionary<string, LinkedListNode<JwtCacheEntry>>();
					_jwts = new LinkedList<JwtCacheEntry>();
				}
				if (_jwtCache.TryGetValue(jwtKey, out var value))
				{
					JwtCacheEntry value2 = value.Value;
					if (value2.ExpiryUtc - JwtCacheExpiryWindow > nowUtc)
					{
						return value2.JwtTask;
					}
					_jwtCache.Remove(jwtKey);
					_jwts.Remove(value);
				}
				DateTime expiryUtc = nowUtc + JwtLifetime;
				Task<string> task = Task.Run(() => CreateJwtAccessToken(authUri, nowUtc, expiryUtc));
				LinkedListNode<JwtCacheEntry> value3 = _jwts.AddFirst(new JwtCacheEntry(task, jwtKey, expiryUtc));
				_jwtCache.Add(jwtKey, value3);
				if (_jwtCache.Count > 100)
				{
					LinkedListNode<JwtCacheEntry> last = _jwts.Last;
					_jwts.RemoveLast();
					_jwtCache.Remove(last.Value.Uri);
				}
				return task;
			}
		}

		private string CreateJwtAccessToken(string authUri, DateTime issueUtc, DateTime expiryUtc)
		{
			JsonWebSignature.Payload payload = ((!HasExplicitScopes) ? new JsonWebSignature.Payload
			{
				Audience = authUri
			} : new GoogleJsonWebSignature.Payload
			{
				Scope = string.Join(" ", Scopes)
			});
			payload.Issuer = Id;
			payload.Subject = Id;
			payload.IssuedAtTimeSeconds = (long)(issueUtc - UnixEpoch).TotalSeconds;
			payload.ExpirationTimeSeconds = (long)(expiryUtc - UnixEpoch).TotalSeconds;
			return CreateAssertionFromPayload(payload);
		}

		private string CreateJwtAccessTokenForOidc(OidcTokenOptions options, DateTime issueUtc, DateTime expiryUtc)
		{
			JsonWebSignature.Payload payload = new JsonWebSignature.Payload
			{
				Issuer = Id,
				Subject = Id,
				Audience = "https://oauth2.googleapis.com/token",
				IssuedAtTimeSeconds = (long)(issueUtc - UnixEpoch).TotalSeconds,
				ExpirationTimeSeconds = (long)(expiryUtc - UnixEpoch).TotalSeconds,
				TargetAudience = options.TargetAudience
			};
			return CreateAssertionFromPayload(payload);
		}

		private string CreateAssertionFromPayload(JsonWebSignature.Payload payload)
		{
			string value = CreateSerializedHeader();
			string value2 = NewtonsoftJsonSerializer.Instance.Serialize(payload);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(TokenEncodingHelpers.UrlSafeBase64Encode(value)).Append('.').Append(TokenEncodingHelpers.UrlSafeBase64Encode(value2));
			string base64Value = CreateSignature(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
			stringBuilder.Append('.').Append(TokenEncodingHelpers.UrlSafeEncode(base64Value));
			return stringBuilder.ToString();
		}

		public string CreateSignature(byte[] data)
		{
			data.ThrowIfNull("data");
			using SHA256 sHA = SHA256.Create();
			byte[] hash = sHA.ComputeHash(data);
			return Convert.ToBase64String(Key.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
		}

		public Task<string> SignBlobAsync(byte[] blob, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.FromResult(CreateSignature(blob));
		}

		private string CreateSerializedHeader()
		{
			GoogleJsonWebSignature.Header obj = new GoogleJsonWebSignature.Header
			{
				Algorithm = "RS256",
				Type = "JWT",
				KeyId = KeyId
			};
			return NewtonsoftJsonSerializer.Instance.Serialize(obj);
		}

		private GoogleJsonWebSignature.Payload CreatePayload()
		{
			int num = (int)(base.Clock.UtcNow - UnixEpoch).TotalSeconds;
			return new GoogleJsonWebSignature.Payload
			{
				Issuer = Id,
				Audience = base.TokenServerUrl,
				IssuedAtTimeSeconds = num,
				ExpirationTimeSeconds = num + 3600,
				Subject = User,
				Scope = string.Join(" ", Scopes)
			};
		}
	}
}
