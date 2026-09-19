using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Google.Apis.Auth
{
	public class JsonWebSignature
	{
		public class Header : JsonWebToken.Header
		{
			[JsonProperty("alg")]
			public string Algorithm { get; set; }

			[JsonProperty("jku")]
			public string JwkUrl { get; set; }

			[JsonProperty("jwk")]
			public string Jwk { get; set; }

			[JsonProperty("kid")]
			public string KeyId { get; set; }

			[JsonProperty("x5u")]
			public string X509Url { get; set; }

			[JsonProperty("x5t")]
			public string X509Thumbprint { get; set; }

			[JsonProperty("x5c")]
			public string X509Certificate { get; set; }

			[JsonProperty("crit")]
			public IList<string> critical { get; set; }
		}

		public class Payload : JsonWebToken.Payload
		{
		}

		public static Task<Payload> VerifySignedTokenAsync(string signedJwt, SignedTokenVerificationOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return VerifySignedTokenAsync<Payload>(signedJwt, options, cancellationToken);
		}

		public static async Task<TPayload> VerifySignedTokenAsync<TPayload>(string signedJwt, SignedTokenVerificationOptions options = null, CancellationToken cancellationToken = default(CancellationToken)) where TPayload : Payload
		{
			SignedToken<Header, TPayload> signedToken = SignedToken<Header, TPayload>.FromSignedToken(signedJwt);
			await SignedTokenVerification.VerifySignedTokenAsync(signedToken, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return signedToken.Payload;
		}
	}
}
