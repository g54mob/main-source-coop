using System.Collections.Generic;
using Google.Apis.Http;

namespace Google.Apis.Auth.OAuth2
{
	internal interface IGoogleCredential : ICredential, IConfigurableHttpClientInitializer, ITokenAccess, ITokenAccessWithHeaders
	{
		string QuotaProject { get; }

		bool HasExplicitScopes { get; }

		bool SupportsExplicitScopes { get; }

		IGoogleCredential WithQuotaProject(string quotaProject);

		IGoogleCredential MaybeWithScopes(IEnumerable<string> scopes);

		IGoogleCredential WithUserForDomainWideDelegation(string user);

		IGoogleCredential WithHttpClientFactory(IHttpClientFactory httpClientFactory);
	}
}
