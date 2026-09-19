using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Util;
using Newtonsoft.Json.Linq;

namespace Google.Apis.Auth
{
	internal static class SignedTokenVerification
	{
		internal abstract class CertificateCacheBase
		{
			internal struct CachedCertificates
			{
				private readonly DateTimeOffset _cachedSince;

				public IEnumerable<AsymmetricAlgorithm> Certificates { get; }

				public CachedCertificates(IEnumerable<AsymmetricAlgorithm> certificates, DateTimeOffset cachedSince)
				{
					_cachedSince = cachedSince;
					Certificates = certificates;
				}

				public bool Expired(DateTimeOffset now)
				{
					return _cachedSince + DefaultCacheValidity <= now;
				}
			}

			internal static readonly TimeSpan DefaultCacheValidity = TimeSpan.FromHours(1.0);

			private readonly IDictionary<string, CachedCertificates> _cache = new Dictionary<string, CachedCertificates>();

			private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1);

			private readonly IClock _clock;

			protected CertificateCacheBase(IClock clock)
			{
				_clock = clock ?? SystemClock.Default;
			}

			public async Task<IEnumerable<AsymmetricAlgorithm>> GetCertificatesAsync(string certificatesLocation, Func<JToken, AsymmetricAlgorithm> certificateFactory, bool forceCertificateRefresh, CancellationToken cancellationToken)
			{
				certificatesLocation.ThrowIfNullOrEmpty("certificatesLocation");
				certificateFactory.ThrowIfNull("certificateFactory");
				await _cacheLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (forceCertificateRefresh || !_cache.TryGetValue(certificatesLocation, out var value) || value.Expired(_clock.UtcNow))
					{
						IEnumerable<JToken> source = JToken.Parse(await FetchCertificatesAsync(certificatesLocation).ConfigureAwait(continueOnCapturedContext: false))["keys"]?.AsEnumerable() ?? throw new ArgumentException("Only JWK formatted keys are currently supported. No 'keys' element was found in " + certificatesLocation);
						if (!source.Any())
						{
							throw new ArgumentException("No JWKs were found on " + certificatesLocation + ". The 'keys' element was empty.");
						}
						value = new CachedCertificates(source.Select((JToken key) => certificateFactory(key)).ToList(), _clock.UtcNow);
						_cache[certificatesLocation] = value;
					}
					return value.Certificates;
				}
				finally
				{
					_cacheLock.Release();
				}
			}

			protected abstract Task<string> FetchCertificatesAsync(string certificatesLocation);
		}

		internal sealed class CertificateCache : CertificateCacheBase
		{
			public CertificateCache()
				: base(SystemClock.Default)
			{
			}

			protected override async Task<string> FetchCertificatesAsync(string certificatesLocation)
			{
				using HttpClient httpClient = new HttpClient();
				return await httpClient.GetStringAsync(certificatesLocation).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private static readonly CertificateCache s_certificateCache = new CertificateCache();

		internal static async Task VerifySignedTokenAsync<TJswHeader, TJswPayload>(SignedToken<TJswHeader, TJswPayload> signedToken, SignedTokenVerificationOptions options, CancellationToken cancellationToken) where TJswHeader : JsonWebSignature.Header where TJswPayload : JsonWebSignature.Payload
		{
			signedToken.ThrowIfNull("signedToken");
			options = ((options == null) ? new SignedTokenVerificationOptions() : new SignedTokenVerificationOptions(options));
			SignedTokenVerificationOptions signedTokenVerificationOptions = options;
			if (signedTokenVerificationOptions.CertificateCache == null)
			{
				signedTokenVerificationOptions.CertificateCache = s_certificateCache;
			}
			object obj = signedToken.Header.Algorithm switch
			{
				"RS256" => VerifyRS256TokenAsync(signedToken, options, cancellationToken), 
				"ES256" => VerifyES256TokenAsync(signedToken, options, cancellationToken), 
				_ => throw new InvalidJwtException("Signing algorithm must be either RS256 or ES256."), 
			};
			if (options.TrustedIssuers.Count > 0 && !options.TrustedIssuers.Contains(signedToken.Payload.Issuer))
			{
				string text = string.Join(", ", options.TrustedIssuers.Select((string x) => "'" + x + "'"));
				throw new InvalidJwtException("JWT issuer incorrect. Must be one of: " + text);
			}
			if (options.TrustedAudiences.Count > 0 && signedToken.Payload.AudienceAsList.Except(options.TrustedAudiences).Any())
			{
				throw new InvalidJwtException("JWT contains untrusted 'aud' claim.");
			}
			DateTimeOffset dateTimeOffset = signedToken.Payload.IssuedAt ?? throw new InvalidJwtException("JWT must contain 'iat' and 'exp' claims");
			DateTimeOffset dateTimeOffset2 = signedToken.Payload.ExpiresAt ?? throw new InvalidJwtException("JWT must contain 'iat' and 'exp' claims");
			DateTime utcNow = options.Clock.UtcNow;
			if (utcNow + options.IssuedAtClockTolerance < dateTimeOffset)
			{
				throw new InvalidJwtException("JWT is not yet valid.");
			}
			if (utcNow - options.ExpiryClockTolerance > dateTimeOffset2)
			{
				throw new InvalidJwtException("JWT has expired.");
			}
			await ((Task)obj).ConfigureAwait(false);
		}

		private static async Task VerifyRS256TokenAsync<TJswHeader, TJswPayload>(SignedToken<TJswHeader, TJswPayload> signedToken, SignedTokenVerificationOptions options, CancellationToken cancellationToken) where TJswHeader : JsonWebSignature.Header where TJswPayload : JsonWebSignature.Payload
		{
			foreach (RSA item in await GetCertificatesAsync(options, "https://www.googleapis.com/oauth2/v3/certs", FromKeyToRsa, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
			{
				if (item.VerifyHash(signedToken.Sha256Hash, signedToken.Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
				{
					return;
				}
			}
			throw new InvalidJwtException("JWT invalid, unable to verify signature.");
		}

		internal static RSA FromKeyToRsa(JToken key)
		{
			RSA rSA = RSA.Create();
			rSA.ImportParameters(new RSAParameters
			{
				Modulus = TokenEncodingHelpers.Base64UrlDecode((string?)key["n"]),
				Exponent = TokenEncodingHelpers.Base64UrlDecode((string?)key["e"])
			});
			return rSA;
		}

		private static async Task VerifyES256TokenAsync<TJswHeader, TJswPayload>(SignedToken<TJswHeader, TJswPayload> signedToken, SignedTokenVerificationOptions options, CancellationToken cancellationToken) where TJswHeader : JsonWebSignature.Header where TJswPayload : JsonWebSignature.Payload
		{
			foreach (ECDsa item in await GetCertificatesAsync(options, "https://www.gstatic.com/iap/verify/public_key-jwk", FromKeyToECDsa, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
			{
				if (item.VerifyHash(signedToken.Sha256Hash, signedToken.Signature))
				{
					return;
				}
			}
			throw new InvalidJwtException("JWT invalid, unable to verify signature.");
			static ECDsa BuildEcdsa(byte[] x, byte[] y)
			{
				ECDsa eCDsa = ECDsa.Create();
				eCDsa.ImportParameters(new ECParameters
				{
					Curve = ECCurve.NamedCurves.nistP256,
					Q = new ECPoint
					{
						X = x,
						Y = y
					}
				});
				return eCDsa;
			}
			static ECDsa FromKeyToECDsa(JToken key)
			{
				if ((string?)key["kty"] != "EC" && (string?)key["crv"] != "P-256")
				{
					throw new ArgumentException("For ES256 verification only certificates with kty='EC' and crv='P-256' are supported. Encountered: kty=" + (string?)key["kty"] + " and crv=" + (string?)key["crv"] + ".");
				}
				byte[] x = TokenEncodingHelpers.Base64UrlDecode((string?)key["x"]);
				byte[] y = TokenEncodingHelpers.Base64UrlDecode((string?)key["y"]);
				return BuildEcdsa(x, y);
			}
		}

		private static async Task<IEnumerable<AsymmetricAlgorithm>> GetCertificatesAsync(SignedTokenVerificationOptions options, string defaultCertificateLocation, Func<JToken, AsymmetricAlgorithm> certificateFactory, CancellationToken cancellationToken)
		{
			return await options.CertificateCache.GetCertificatesAsync(options.CertificatesUrl ?? defaultCertificateLocation, certificateFactory, options.ForceCertificateRefresh, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
