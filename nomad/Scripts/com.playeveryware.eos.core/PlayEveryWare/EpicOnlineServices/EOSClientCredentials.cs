using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PlayEveryWare.EpicOnlineServices.Utility;

namespace PlayEveryWare.EpicOnlineServices
{
	public class EOSClientCredentials : IEquatable<EOSClientCredentials>
	{
		public string ClientId;

		public string ClientSecret;

		private static readonly Regex s_invalidEncryptionKeyRegex;

		public string EncryptionKey { get; set; }

		[JsonIgnore]
		public bool IsComplete
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(ClientId))
				{
					return !string.IsNullOrWhiteSpace(ClientSecret);
				}
				return false;
			}
		}

		static EOSClientCredentials()
		{
			s_invalidEncryptionKeyRegex = new Regex("[^0-9a-fA-F]");
		}

		public EOSClientCredentials()
		{
			EncryptionKey = GenerateEncryptionKey();
		}

		public static string GenerateEncryptionKey()
		{
			byte[] array = new byte[32];
			using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			randomNumberGenerator.GetBytes(array);
			return BitConverter.ToString(array).Replace("-", string.Empty);
		}

		[JsonConstructor]
		public EOSClientCredentials(string clientId, string clientSecret, string encryptionKey)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
			EncryptionKey = encryptionKey;
		}

		public static bool IsEncryptionKeyValid(string encryptionKey)
		{
			if (encryptionKey != null && encryptionKey.Length == 64)
			{
				return !s_invalidEncryptionKeyRegex.Match(encryptionKey).Success;
			}
			return false;
		}

		public bool IsEncryptionKeyValid()
		{
			return IsEncryptionKeyValid(EncryptionKey);
		}

		public bool Equals(EOSClientCredentials other)
		{
			if (other == null)
			{
				return false;
			}
			if (ClientId == other.ClientId)
			{
				return ClientSecret == other.ClientSecret;
			}
			return false;
		}

		public override bool Equals(object other)
		{
			if (other is EOSClientCredentials other2)
			{
				return Equals(other2);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashUtility.Combine(ClientId, ClientSecret);
		}
	}
}
