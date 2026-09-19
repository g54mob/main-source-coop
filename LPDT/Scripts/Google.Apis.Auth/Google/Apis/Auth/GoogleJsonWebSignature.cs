using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Auth
{
	public class GoogleJsonWebSignature
	{
		public sealed class ValidationSettings
		{
			public IEnumerable<string> Audience { get; set; }

			public string HostedDomain { get; set; }

			public IClock Clock { get; set; }

			public bool ForceGoogleCertRefresh { get; set; }

			public TimeSpan IssuedAtClockTolerance { get; set; } = TimeSpan.FromSeconds(30.0);

			public TimeSpan ExpirationTimeClockTolerance { get; set; } = TimeSpan.FromSeconds(0.0);

			internal SignedTokenVerification.CertificateCacheBase CertificateCache { get; set; }

			public ValidationSettings()
			{
			}

			private ValidationSettings(ValidationSettings other)
			{
				Audience = other.Audience?.ToArray();
				HostedDomain = other.HostedDomain;
				Clock = other.Clock;
				ForceGoogleCertRefresh = other.ForceGoogleCertRefresh;
				IssuedAtClockTolerance = other.IssuedAtClockTolerance;
				ExpirationTimeClockTolerance = other.ExpirationTimeClockTolerance;
				CertificateCache = other.CertificateCache;
			}

			internal ValidationSettings Clone()
			{
				return new ValidationSettings(this);
			}

			internal SignedTokenVerificationOptions ToVerificationOptions()
			{
				SignedTokenVerificationOptions signedTokenVerificationOptions = new SignedTokenVerificationOptions
				{
					CertificatesUrl = "https://www.googleapis.com/oauth2/v3/certs",
					ForceCertificateRefresh = ForceGoogleCertRefresh,
					IssuedAtClockTolerance = IssuedAtClockTolerance,
					ExpiryClockTolerance = ExpirationTimeClockTolerance,
					CertificateCache = CertificateCache
				};
				signedTokenVerificationOptions.Clock = Clock ?? signedTokenVerificationOptions.Clock;
				foreach (string item in Audience ?? Enumerable.Empty<string>())
				{
					signedTokenVerificationOptions.TrustedAudiences.Add(item);
				}
				foreach (string validJwtIssuer in ValidJwtIssuers)
				{
					signedTokenVerificationOptions.TrustedIssuers.Add(validJwtIssuer);
				}
				return signedTokenVerificationOptions;
			}
		}

		public class Header : JsonWebSignature.Header
		{
		}

		public class Payload : JsonWebSignature.Payload
		{
			[JsonProperty("scope")]
			public string Scope { get; set; }

			[JsonProperty("prn")]
			public string Prn { get; set; }

			[JsonProperty("hd")]
			public string HostedDomain { get; set; }

			[JsonProperty("email")]
			public string Email { get; set; }

			[JsonProperty("email_verified")]
			public bool EmailVerified { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("given_name")]
			public string GivenName { get; set; }

			[JsonProperty("family_name")]
			public string FamilyName { get; set; }

			[JsonProperty("picture")]
			public string Picture { get; set; }

			[JsonProperty("locale")]
			public string Locale { get; set; }
		}

		internal const int MaxJwtLength = 10000;

		private const string SupportedJwtAlgorithm = "RS256";

		internal static readonly IEnumerable<string> ValidJwtIssuers = new string[2] { "https://accounts.google.com", "accounts.google.com" };

		public static Task<Payload> ValidateAsync(string jwt, IClock clock = null, bool forceGoogleCertRefresh = false)
		{
			return ValidateAsync(jwt, new ValidationSettings
			{
				Clock = clock,
				ForceGoogleCertRefresh = forceGoogleCertRefresh
			});
		}

		public static Task<Payload> ValidateAsync(string jwt, ValidationSettings validationSettings)
		{
			return ValidateInternalAsync(jwt, validationSettings);
		}

		internal static async Task<Payload> ValidateInternalAsync(string jwt, ValidationSettings validationSettings)
		{
			ValidationSettings validationSettings2 = validationSettings.ThrowIfNull("validationSettings").Clone();
			SignedTokenVerificationOptions options = validationSettings.ToVerificationOptions();
			SignedToken<Header, Payload> signedToken = SignedToken<Header, Payload>.FromSignedToken(jwt);
			Task task = SignedTokenVerification.VerifySignedTokenAsync(signedToken, options, default(CancellationToken));
			if (jwt.Length > 10000)
			{
				throw new InvalidJwtException($"JWT exceeds maximum allowed length of {10000}");
			}
			if (signedToken.Header.Algorithm != "RS256")
			{
				throw new InvalidJwtException("JWT algorithm must be 'RS256'");
			}
			if (validationSettings2.HostedDomain != null && signedToken.Payload.HostedDomain != validationSettings2.HostedDomain)
			{
				throw new InvalidJwtException("JWT contains invalid 'hd' claim.");
			}
			await task.ConfigureAwait(continueOnCapturedContext: false);
			return signedToken.Payload;
		}
	}
}
