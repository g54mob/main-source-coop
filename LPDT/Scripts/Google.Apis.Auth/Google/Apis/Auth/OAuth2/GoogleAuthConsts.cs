using System;

namespace Google.Apis.Auth.OAuth2
{
	public static class GoogleAuthConsts
	{
		public const string AuthorizationUrl = "https://accounts.google.com/o/oauth2/auth";

		public const string OidcAuthorizationUrl = "https://accounts.google.com/o/oauth2/v2/auth";

		public const string ApprovalUrl = "https://accounts.google.com/o/oauth2/approval";

		public const string TokenUrl = "https://accounts.google.com/o/oauth2/token";

		public const string OidcTokenUrl = "https://oauth2.googleapis.com/token";

		private const string DefaultMetadataAddress = "169.254.169.254";

		internal const string DefaultMetadataServerUrl = "http://169.254.169.254";

		private const string ComputeTokenUrlSuffix = "/computeMetadata/v1/instance/service-accounts/default/token";

		private const string ComputeOidcTokenUrlSuffix = "/computeMetadata/v1/instance/service-accounts/default/identity";

		public const string ComputeTokenUrl = "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/token";

		public const string RevokeTokenUrl = "https://oauth2.googleapis.com/revoke";

		public const string JsonWebKeySetUrl = "https://www.googleapis.com/oauth2/v3/certs";

		public const string IapKeySetUrl = "https://www.gstatic.com/iap/verify/public_key-jwk";

		[Obsolete("The OAuth out-of-band flow will be deprecated and this constant will be removed on October 3rd 2022. You can read more about deprecation here: https://developers.googleblog.com/2022/02/making-oauth-flows-safer.html#disallowed-oob.")]
		public const string InstalledAppRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

		public const string LocalhostRedirectUri = "http://localhost";

		internal const string IamServiceAccountEndpointCommonPrefix = "https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/";

		internal const string IamAccessTokenEndpointFormatString = "https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{0}:generateAccessToken";

		internal const string IamSignEndpointFormatString = "https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{0}:signBlob";

		internal const string IamIdTokenEndpointFormatString = "https://iamcredentials.googleapis.com/v1/projects/-/serviceAccounts/{0}:generateIdToken";

		internal const string IamScope = "https://www.googleapis.com/auth/iam";

		internal static string EffectiveComputeTokenUrl => GetEffectiveMetadataUrl("/computeMetadata/v1/instance/service-accounts/default/token", "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/token");

		internal static string EffectiveComputeOidcTokenUrl => GetEffectiveMetadataUrl("/computeMetadata/v1/instance/service-accounts/default/identity", "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/identity");

		internal static string EffectiveMetadataServerUrl => GetEffectiveMetadataUrl(null, "http://169.254.169.254");

		private static string GetEffectiveMetadataUrl(string suffix, string defaultValue)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("GCE_METADATA_HOST");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				return "http://" + environmentVariable + suffix;
			}
			return defaultValue;
		}
	}
}
