using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public class ComputeCredential : ServiceCredential, IOidcTokenProvider, IGoogleCredential, ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders
	{
		public new class Initializer : ServiceCredential.Initializer
		{
			public string OidcTokenUrl { get; }

			public Initializer()
				: this(GoogleAuthConsts.EffectiveComputeTokenUrl)
			{
			}

			public Initializer(string tokenUrl)
				: this(tokenUrl, GoogleAuthConsts.EffectiveComputeOidcTokenUrl)
			{
			}

			public Initializer(string tokenUrl, string oidcTokenUrl)
				: base(tokenUrl)
			{
				OidcTokenUrl = oidcTokenUrl;
			}

			internal Initializer(ComputeCredential other)
				: base(other)
			{
				OidcTokenUrl = other.OidcTokenUrl;
			}
		}

		public const string MetadataServerUrl = "http://169.254.169.254";

		private static readonly Lazy<Task<bool>> isRunningOnComputeEngineCached = new Lazy<Task<bool>>(() => IsRunningOnComputeEngineNoCache());

		private const int MetadataServerPingTimeoutInMilliseconds = 500;

		private const int MetadataServerPingAttempts = 3;

		private const string MetadataFlavor = "Metadata-Flavor";

		private const string GoogleMetadataHeader = "Google";

		public string OidcTokenUrl { get; }

		bool IGoogleCredential.HasExplicitScopes => false;

		bool IGoogleCredential.SupportsExplicitScopes => false;

		public ComputeCredential()
			: this(new Initializer())
		{
		}

		public ComputeCredential(Initializer initializer)
			: base(initializer)
		{
			OidcTokenUrl = initializer.OidcTokenUrl;
		}

		IGoogleCredential IGoogleCredential.WithQuotaProject(string quotaProject)
		{
			return new ComputeCredential(new Initializer(this)
			{
				QuotaProject = quotaProject
			});
		}

		IGoogleCredential IGoogleCredential.MaybeWithScopes(IEnumerable<string> scopes)
		{
			return this;
		}

		IGoogleCredential IGoogleCredential.WithUserForDomainWideDelegation(string user)
		{
			throw new InvalidOperationException("ComputeCredential does not support Domain-Wide Delegation");
		}

		IGoogleCredential IGoogleCredential.WithHttpClientFactory(IHttpClientFactory httpClientFactory)
		{
			return new ComputeCredential(new Initializer(this)
			{
				HttpClientFactory = httpClientFactory
			});
		}

		public override async Task<bool> RequestAccessTokenAsync(CancellationToken taskCancellationToken)
		{
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, base.TokenServerUrl);
			httpRequestMessage.Headers.Add("Metadata-Flavor", "Google");
			base.Token = await TokenResponse.FromHttpResponseAsync(await base.HttpClient.SendAsync(httpRequestMessage, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false), base.Clock, ServiceCredential.Logger).ConfigureAwait(continueOnCapturedContext: false);
			return true;
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
			string text = OidcTokenUrl + "?audience=" + options.TargetAudience;
			if (options.TokenFormat == OidcTokenFormat.Full || options.TokenFormat == OidcTokenFormat.FullWithLicences)
			{
				text += "&format=full";
				if (options.TokenFormat == OidcTokenFormat.FullWithLicences)
				{
					text += "&licenses=true";
				}
			}
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, text);
			httpRequestMessage.Headers.Add("Metadata-Flavor", "Google");
			caller.Token = await TokenResponse.FromHttpResponseAsync(await base.HttpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), base.Clock, ServiceCredential.Logger).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}

		public static Task<bool> IsRunningOnComputeEngine()
		{
			return isRunningOnComputeEngineCached.Value;
		}

		private static async Task<bool> IsRunningOnComputeEngineNoCache()
		{
			ServiceCredential.Logger.Info("Checking connectivity to ComputeEngine metadata server.");
			using (HttpClient httpClient = new HttpClient())
			{
				for (int i = 0; i < 3; i++)
				{
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					cancellationTokenSource.CancelAfter(500);
					try
					{
						HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, GoogleAuthConsts.EffectiveMetadataServerUrl);
						httpRequestMessage.Headers.Add("Metadata-Flavor", "Google");
						if ((await httpClient.SendAsync(httpRequestMessage, cancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false)).Headers.TryGetValues("Metadata-Flavor", out var values) && values.Contains("Google"))
						{
							return true;
						}
						ServiceCredential.Logger.Info("Response came from a source other than the Google Compute Engine metadata server.");
						return false;
					}
					catch (Exception ex) when (ex is HttpRequestException || ex is WebException || ex is OperationCanceledException)
					{
					}
				}
			}
			ServiceCredential.Logger.Debug("Could not reach the Google Compute Engine metadata service. That is expected if this application is not running on GCE.");
			return false;
		}
	}
}
