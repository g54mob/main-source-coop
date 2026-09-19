using System;
using System.Text;

namespace RSG.Muffin.XOREncryptionSubmodule.XOREncryption.Scripts
{
	public static class XOREncryption
	{
		public static string EncryptString(string input, string key)
		{
			return Convert.ToBase64String(XorEncryptDecrypt(Encoding.UTF8.GetBytes(input), key));
		}

		public static string DecryptString(string input, string key)
		{
			byte[] bytes = XorEncryptDecrypt(Convert.FromBase64String(input), key);
			return Encoding.UTF8.GetString(bytes);
		}

		public static bool IsBase64String(string input)
		{
			try
			{
				Convert.FromBase64String(input);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		private static byte[] XorEncryptDecrypt(byte[] data, string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			int num = bytes.Length;
			for (int i = 0; i < data.Length; i++)
			{
				data[i] ^= bytes[i % num];
			}
			return data;
		}
	}
}
