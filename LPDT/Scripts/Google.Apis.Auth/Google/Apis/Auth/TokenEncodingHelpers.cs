using System;
using System.Text;

namespace Google.Apis.Auth
{
	internal static class TokenEncodingHelpers
	{
		internal static string Base64UrlToString(string base64Url)
		{
			return Encoding.UTF8.GetString(Base64UrlDecode(base64Url));
		}

		internal static byte[] Base64UrlDecode(string base64Url)
		{
			string text = base64Url.Replace('-', '+').Replace('_', '/');
			switch (text.Length % 4)
			{
			case 2:
				text += "==";
				break;
			case 3:
				text += "=";
				break;
			}
			return Convert.FromBase64String(text);
		}

		internal static string UrlSafeBase64Encode(string value)
		{
			return UrlSafeBase64Encode(Encoding.UTF8.GetBytes(value));
		}

		internal static string UrlSafeBase64Encode(byte[] bytes)
		{
			return UrlSafeEncode(Convert.ToBase64String(bytes));
		}

		internal static string UrlSafeEncode(string base64Value)
		{
			return base64Value.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
		}
	}
}
