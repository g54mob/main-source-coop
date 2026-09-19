using System;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Json;
using Google.Apis.Util;

namespace Google.Apis.Auth
{
	internal class SignedToken<TJwsHeader, TJwsPayload> where TJwsHeader : JsonWebSignature.Header where TJwsPayload : JsonWebSignature.Payload
	{
		private readonly Lazy<byte[]> _sha256;

		internal string EncodedHeader { get; }

		internal string EncodedPayload { get; }

		internal TJwsHeader Header { get; }

		internal TJwsPayload Payload { get; }

		internal byte[] Signature { get; }

		internal byte[] Sha256Hash => _sha256.Value;

		private SignedToken(string encodedHeader, string encodedPayload, TJwsHeader header, TJwsPayload payload, byte[] signature)
		{
			EncodedHeader = encodedHeader.ThrowIfNullOrEmpty("encodedHeader");
			EncodedPayload = encodedPayload.ThrowIfNullOrEmpty("encodedPayload");
			Header = header.ThrowIfNull("header");
			Payload = payload.ThrowIfNull("payload");
			Signature = signature;
			_sha256 = new Lazy<byte[]>(InitSha256);
		}

		internal static SignedToken<TJwsHeader, TJwsPayload> FromSignedToken(string signedToken)
		{
			signedToken.ThrowIfNull("signedToken");
			signedToken.ThrowIfNullOrEmpty("signedToken");
			string[] array = signedToken.Split(new char[1] { '.' });
			if (array.Length != 3)
			{
				throw new InvalidJwtException("JWT must consist of Header, Payload, and Signature");
			}
			string text = array[0];
			string text2 = array[1];
			TJwsHeader header = NewtonsoftJsonSerializer.Instance.Deserialize<TJwsHeader>(TokenEncodingHelpers.Base64UrlToString(text));
			TJwsPayload payload = NewtonsoftJsonSerializer.Instance.Deserialize<TJwsPayload>(TokenEncodingHelpers.Base64UrlToString(text2));
			byte[] signature = TokenEncodingHelpers.Base64UrlDecode(array[2]);
			return new SignedToken<TJwsHeader, TJwsPayload>(text, text2, header, payload, signature);
		}

		private byte[] InitSha256()
		{
			using SHA256 sHA = SHA256.Create();
			return sHA.ComputeHash(Encoding.ASCII.GetBytes(EncodedHeader + "." + EncodedPayload));
		}
	}
}
