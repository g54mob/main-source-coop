using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public sealed class AccessTokenWithHeaders
	{
		public class Builder
		{
			public string QuotaProject { get; set; }

			public AccessTokenWithHeaders Build(string token)
			{
				return new AccessTokenWithHeaders(token, QuotaProject);
			}
		}

		private const string QuotaProjectHeaderName = "x-goog-user-project";

		private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> s_emptyHeaders = new ReadOnlyDictionary<string, IReadOnlyList<string>>(new Dictionary<string, IReadOnlyList<string>>());

		public string AccessToken { get; }

		public IReadOnlyDictionary<string, IReadOnlyList<string>> Headers { get; }

		public AccessTokenWithHeaders(string token, IReadOnlyDictionary<string, IReadOnlyList<string>> headers)
		{
			AccessToken = token;
			Headers = headers ?? s_emptyHeaders;
		}

		private AccessTokenWithHeaders(string token, string quotaProject = null)
			: this(token, (quotaProject == null) ? null : new ReadOnlyDictionary<string, IReadOnlyList<string>>(new Dictionary<string, IReadOnlyList<string>> { 
			{
				"x-goog-user-project",
				new List<string> { quotaProject }.AsReadOnly()
			} }))
		{
		}

		public void AddHeaders(HttpRequestHeaders requestHeaders)
		{
			requestHeaders.ThrowIfNull("requestHeaders");
			foreach (KeyValuePair<string, IReadOnlyList<string>> header in Headers)
			{
				requestHeaders.Add(header.Key, header.Value);
			}
		}

		public void AddHeaders(HttpRequestMessage request)
		{
			AddHeaders(request.ThrowIfNull("request").Headers);
		}
	}
}
