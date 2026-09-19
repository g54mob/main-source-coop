using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public class OidcTokenOptions
	{
		public string TargetAudience { get; }

		public OidcTokenFormat TokenFormat { get; }

		private OidcTokenOptions(string targetAudience, OidcTokenFormat tokenFormat)
		{
			TargetAudience = targetAudience.ThrowIfNull("targetAudience");
			TokenFormat = Utilities.CheckEnumValue(tokenFormat, "tokenFormat");
		}

		public static OidcTokenOptions FromTargetAudience(string targetAudience)
		{
			return new OidcTokenOptions(targetAudience, OidcTokenFormat.Full);
		}

		public OidcTokenOptions WithTargetAudience(string targetAudience)
		{
			return new OidcTokenOptions(targetAudience, TokenFormat);
		}

		public OidcTokenOptions WithTokenFormat(OidcTokenFormat tokenFormat)
		{
			return new OidcTokenOptions(TargetAudience, tokenFormat);
		}
	}
}
