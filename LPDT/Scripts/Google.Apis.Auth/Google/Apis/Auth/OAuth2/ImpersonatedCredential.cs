using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public sealed class ImpersonatedCredential : ServiceCredential, IOidcTokenProvider, IGoogleCredential, ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders, IBlobSigner
	{
		public new sealed class Initializer : ServiceCredential.Initializer
		{
			public string TargetPrincipal { get; }

			public IEnumerable<string> DelegateAccounts { get; set; }

			public IEnumerable<string> Scopes { get; set; }

			public TimeSpan Lifetime { get; set; }

			public Initializer(string targetPrincipal)
				: base(string.Format("https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{0}:generateAccessToken", targetPrincipal.ThrowIfNull("targetPrincipal")))
			{
				TargetPrincipal = targetPrincipal;
				Lifetime = TimeSpan.FromHours(1.0);
			}

			internal Initializer(ImpersonatedCredential other)
				: base(other)
			{
				TargetPrincipal = other.TargetPrincipal;
				DelegateAccounts = other.DelegateAccounts;
				Scopes = other.Scopes;
				Lifetime = other.Lifetime;
			}

			internal Initializer(Initializer other)
				: base(other)
			{
				TargetPrincipal = other.TargetPrincipal;
				DelegateAccounts = other.DelegateAccounts?.ToList().AsReadOnly() ?? Enumerable.Empty<string>();
				Scopes = other.Scopes?.ToList().AsReadOnly() ?? Enumerable.Empty<string>();
				Lifetime = other.Lifetime;
			}
		}

		public GoogleCredential SourceCredential => base.HttpClientInitializers.OfType<GoogleCredential>().Single();

		public string TargetPrincipal { get; }

		public IEnumerable<string> DelegateAccounts { get; }

		public IEnumerable<string> Scopes { get; }

		public TimeSpan Lifetime { get; }

		bool IGoogleCredential.HasExplicitScopes => Scopes?.Any() ?? false;

		bool IGoogleCredential.SupportsExplicitScopes => true;

		internal static ImpersonatedCredential Create(GoogleCredential sourceCredential, Initializer initializer)
		{
			initializer.ThrowIfNull("initializer");
			sourceCredential.ThrowIfNull("sourceCredential");
			if (initializer.Lifetime < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("Lifetime", "Must be greater or equal to Zero");
			}
			if (!(sourceCredential.UnderlyingCredential is ServiceAccountCredential) && !(sourceCredential.UnderlyingCredential is UserCredential))
			{
				throw new InvalidOperationException("Only ServiceAccountCredential and UserCredential support impersonation.");
			}
			initializer = new Initializer(initializer);
			initializer.HttpClientInitializers.Add(sourceCredential.CreateScoped("https://www.googleapis.com/auth/iam"));
			return new ImpersonatedCredential(initializer);
		}

		private ImpersonatedCredential(Initializer initializer)
			: base(initializer)
		{
			TargetPrincipal = initializer.TargetPrincipal;
			DelegateAccounts = initializer.DelegateAccounts;
			Scopes = initializer.Scopes;
			Lifetime = initializer.Lifetime;
		}

		IGoogleCredential IGoogleCredential.WithQuotaProject(string quotaProject)
		{
			return new ImpersonatedCredential(new Initializer(this)
			{
				QuotaProject = quotaProject
			});
		}

		IGoogleCredential IGoogleCredential.MaybeWithScopes(IEnumerable<string> scopes)
		{
			return new ImpersonatedCredential(new Initializer(this)
			{
				Scopes = scopes
			});
		}

		IGoogleCredential IGoogleCredential.WithUserForDomainWideDelegation(string user)
		{
			throw new InvalidOperationException("ImpersonatedCredential does not support Domain-Wide Delegation");
		}

		IGoogleCredential IGoogleCredential.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			return new ImpersonatedCredential(new Initializer(this)
			{
				HttpClientFactory = httpClientFactory
			});
		}

		public override async Task<bool> RequestAccessTokenAsync(CancellationToken taskCancellationToken)
		{
			base.Token = await new ImpersonationAccessTokenRequest
			{
				DelegateAccounts = DelegateAccounts,
				Scopes = Scopes,
				Lifetime = $"{(int)Lifetime.TotalSeconds}s"
			}.ExecuteAsync(base.HttpClient, base.TokenServerUrl, base.Clock, ServiceCredential.Logger, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		public Task<OidcToken> GetOidcTokenAsync(OidcTokenOptions options, CancellationToken cancellationToken = default(CancellationToken))
		{
			options.ThrowIfNull("options");
			TokenRefreshManager tokenRefreshManager = null;
			tokenRefreshManager = new TokenRefreshManager((CancellationToken ct) => RefreshOidcTokenAsync(tokenRefreshManager, options, ct), base.Clock, ServiceCredential.Logger);
			return Task.FromResult(new OidcToken(tokenRefreshManager));
		}

		private async Task<bool> RefreshOidcTokenAsync(TokenRefreshManager caller, OidcTokenOptions oidcTokenOptions, CancellationToken cancellationToken)
		{
			ImpersonationOIdCTokenRequest request = new ImpersonationOIdCTokenRequest
			{
				DelegateAccounts = DelegateAccounts,
				Audience = oidcTokenOptions.TargetAudience,
				IncludeEmail = true
			};
			string url = $"https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{TargetPrincipal}:generateIdToken";
			caller.Token = await request.ExecuteAsync(base.HttpClient, url, base.Clock, ServiceCredential.Logger, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		public async Task<string> SignBlobAsync(byte[] blob, CancellationToken cancellationToken = default(CancellationToken))
		{
			return (await new ImpersonationSignBlobRequest
			{
				DelegateAccounts = DelegateAccounts,
				Payload = blob
			}.ExecuteAsync<ImpersonationSignBlobResponse>(url: $"https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{TargetPrincipal}:signBlob", httpClient: base.HttpClient, cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).SignedBlob;
		}
	}
}
