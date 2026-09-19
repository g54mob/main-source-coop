using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Http;

namespace Google.Apis.Auth.OAuth2
{
	public class GoogleCredential : ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders, IOidcTokenProvider, IBlobSigner
	{
		private static readonly DefaultCredentialProvider defaultCredentialProvider = new DefaultCredentialProvider();

		private protected readonly IGoogleCredential credential;

		public virtual bool IsCreateScopedRequired
		{
			get
			{
				if (credential.SupportsExplicitScopes)
				{
					return !credential.HasExplicitScopes;
				}
				return false;
			}
		}

		public string QuotaProject => credential.QuotaProject;

		public ICredential UnderlyingCredential => credential;

		internal GoogleCredential(IGoogleCredential credential)
		{
			this.credential = credential;
		}

		public static Task<GoogleCredential> GetApplicationDefaultAsync()
		{
			return defaultCredentialProvider.GetDefaultCredentialAsync();
		}

		public static Task<GoogleCredential> GetApplicationDefaultAsync(CancellationToken cancellationToken)
		{
			return defaultCredentialProvider.GetDefaultCredentialAsync().WithCancellationToken(cancellationToken);
		}

		public static GoogleCredential GetApplicationDefault()
		{
			return Task.Run(() => GetApplicationDefaultAsync()).Result;
		}

		public static GoogleCredential FromStream(Stream stream)
		{
			return defaultCredentialProvider.CreateDefaultCredentialFromStream(stream);
		}

		public static Task<GoogleCredential> FromStreamAsync(Stream stream, CancellationToken cancellationToken)
		{
			return defaultCredentialProvider.CreateDefaultCredentialFromStreamAsync(stream, cancellationToken);
		}

		public static GoogleCredential FromFile(string path)
		{
			using FileStream stream = File.OpenRead(path);
			return FromStream(stream);
		}

		public static async Task<GoogleCredential> FromFileAsync(string path, CancellationToken cancellationToken)
		{
			using FileStream f = File.OpenRead(path);
			return await FromStreamAsync(f, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public static GoogleCredential FromJson(string json)
		{
			return defaultCredentialProvider.CreateDefaultCredentialFromJson(json);
		}

		public static GoogleCredential FromJsonParameters(JsonCredentialParameters credentialParameters)
		{
			return defaultCredentialProvider.CreateDefaultCredentialFromParameters(credentialParameters);
		}

		public static GoogleCredential FromAccessToken(string accessToken, IAccessMethod accessMethod = null)
		{
			if (accessMethod == null)
			{
				accessMethod = new BearerToken.AuthorizationHeaderAccessMethod();
			}
			return new GoogleCredential(new AccessTokenCredential(accessToken, accessMethod));
		}

		public static GoogleCredential FromComputeCredential(ComputeCredential computeCredential = null)
		{
			return new GoogleCredential(computeCredential ?? new ComputeCredential());
		}

		public virtual GoogleCredential CreateScoped(IEnumerable<string> scopes)
		{
			IGoogleCredential googleCredential = credential.MaybeWithScopes(scopes);
			if (googleCredential != credential)
			{
				return new GoogleCredential(googleCredential);
			}
			return this;
		}

		public GoogleCredential CreateScoped(params string[] scopes)
		{
			return CreateScoped((IEnumerable<string>)scopes);
		}

		public virtual GoogleCredential CreateWithUser(string user)
		{
			return new GoogleCredential(credential.WithUserForDomainWideDelegation(user));
		}

		public virtual GoogleCredential CreateWithQuotaProject(string quotaProject)
		{
			return new GoogleCredential(credential.WithQuotaProject(quotaProject));
		}

		public virtual GoogleCredential CreateWithHttpClientFactory(IHttpClientFactory factory)
		{
			return new GoogleCredential(credential.WithHttpClientFactory(factory));
		}

		void IConfigurableHttpClientInitializer.Initialize(ConfigurableHttpClient httpClient)
		{
			credential.Initialize(httpClient);
		}

		Task<string> ITokenAccess.GetAccessTokenForRequestAsync(string authUri, CancellationToken cancellationToken)
		{
			return credential.GetAccessTokenForRequestAsync(authUri, cancellationToken);
		}

		Task<AccessTokenWithHeaders> ITokenAccessWithHeaders.GetAccessTokenWithHeadersForRequestAsync(string authUri, CancellationToken cancellationToken)
		{
			return credential.GetAccessTokenWithHeadersForRequestAsync(authUri, cancellationToken);
		}

		public Task<OidcToken> GetOidcTokenAsync(OidcTokenOptions options, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!(credential is IOidcTokenProvider oidcTokenProvider))
			{
				throw new InvalidOperationException("UnderlyingCredential is not an OIDC token provider. Only ServiceAccountCredential, ComputeCredential, ImpersonatedCredential are supported OIDC token providers.");
			}
			return oidcTokenProvider.GetOidcTokenAsync(options, cancellationToken);
		}

		public async Task<string> SignBlobAsync(byte[] blob, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!(UnderlyingCredential is IBlobSigner))
			{
				throw new InvalidOperationException("UnderlyingCredential is not a blob signer. Only ServiceAccountCredential, ImpersonatedCredential are supported blob signers.");
			}
			return await (UnderlyingCredential as IBlobSigner).SignBlobAsync(blob, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public GoogleCredential Impersonate(ImpersonatedCredential.Initializer initializer)
		{
			return new GoogleCredential(ImpersonatedCredential.Create(this, initializer));
		}

		public static GoogleCredential FromServiceAccountCredential(ServiceAccountCredential credential)
		{
			return new GoogleCredential(credential);
		}
	}
}
