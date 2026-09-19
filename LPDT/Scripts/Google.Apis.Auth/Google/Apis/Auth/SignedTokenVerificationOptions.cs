using System;
using System.Collections.Generic;
using Google.Apis.Util;

namespace Google.Apis.Auth
{
	public sealed class SignedTokenVerificationOptions
	{
		private IClock _clock = SystemClock.Default;

		public IList<string> TrustedAudiences { get; } = new List<string>();

		public string CertificatesUrl { get; set; }

		public IList<string> TrustedIssuers { get; } = new List<string>();

		internal bool ForceCertificateRefresh { get; set; }

		public TimeSpan IssuedAtClockTolerance { get; set; } = TimeSpan.Zero;

		public TimeSpan ExpiryClockTolerance { get; set; } = TimeSpan.Zero;

		internal IClock Clock
		{
			get
			{
				return _clock;
			}
			set
			{
				_clock = value ?? throw new ArgumentNullException("Clock");
			}
		}

		internal SignedTokenVerification.CertificateCacheBase CertificateCache { get; set; }

		public SignedTokenVerificationOptions()
		{
		}

		public SignedTokenVerificationOptions(SignedTokenVerificationOptions other)
		{
			other.ThrowIfNull("other");
			foreach (string trustedAudience in other.TrustedAudiences)
			{
				TrustedAudiences.Add(trustedAudience);
			}
			foreach (string trustedIssuer in other.TrustedIssuers)
			{
				TrustedIssuers.Add(trustedIssuer);
			}
			CertificatesUrl = other.CertificatesUrl;
			ForceCertificateRefresh = other.ForceCertificateRefresh;
			IssuedAtClockTolerance = other.IssuedAtClockTolerance;
			ExpiryClockTolerance = other.ExpiryClockTolerance;
			CertificateCache = other.CertificateCache;
			Clock = other.Clock;
		}
	}
}
