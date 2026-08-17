using System;
using System.IO;
using System.Security.Cryptography;

namespace EvilCore.EvilSave
{
	public class AesEncryptionProcessor : IStreamProcessor
	{
		private const int KeySize = 256;

		private const int BlockSize = 128;

		private const int IvSize = 16;

		private const int SaltSize = 16;

		private const int Iterations = 10000;

		private readonly string _password;

		public AesEncryptionProcessor(string password)
		{
			_password = password;
		}

		public byte[] Process(byte[] input)
		{
			using Aes aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			byte[] array = new byte[16];
			using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
			{
				randomNumberGenerator.GetBytes(array);
			}
			using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(_password, array, 10000, HashAlgorithmName.SHA256);
			aes.Key = rfc2898DeriveBytes.GetBytes(32);
			aes.GenerateIV();
			using MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 0, 16);
			memoryStream.Write(aes.IV, 0, 16);
			using (ICryptoTransform transform = aes.CreateEncryptor())
			{
				using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
				cryptoStream.Write(input, 0, input.Length);
				cryptoStream.FlushFinalBlock();
			}
			return memoryStream.ToArray();
		}

		public byte[] Unprocess(byte[] input)
		{
			if (input.Length < 32)
			{
				throw new InvalidOperationException("Encrypted data is too short");
			}
			byte[] array = new byte[16];
			byte[] array2 = new byte[16];
			Buffer.BlockCopy(input, 0, array, 0, 16);
			Buffer.BlockCopy(input, 16, array2, 0, 16);
			using Aes aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(_password, array, 10000, HashAlgorithmName.SHA256);
			aes.Key = rfc2898DeriveBytes.GetBytes(32);
			aes.IV = array2;
			using MemoryStream stream = new MemoryStream(input, 32, input.Length - 16 - 16);
			using ICryptoTransform transform = aes.CreateDecryptor();
			using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
			using MemoryStream memoryStream = new MemoryStream();
			cryptoStream.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
	}
}
