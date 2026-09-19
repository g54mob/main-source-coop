using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using GameAnalyticsSDK.Net.Utilities.Zip.GZip;

namespace GameAnalyticsSDK.Net.Utilities
{
	internal static class GAUtilities
	{
		private static readonly DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static byte[] GzipCompress(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return new byte[0];
			}
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipOutputStream destination = new GZipOutputStream(memoryStream))
			{
				using MemoryStream memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(data));
				memoryStream2.CopyTo(destination);
			}
			return memoryStream.ToArray();
		}

		public static string HmacWithKey(string key, byte[] data)
		{
			using HMACSHA256 hMACSHA = new HMACSHA256(Encoding.UTF8.GetBytes(key));
			return Convert.ToBase64String(hMACSHA.ComputeHash(data));
		}

		public static bool StringMatch(string s, string pattern)
		{
			if (s == null || pattern == null)
			{
				return false;
			}
			return Regex.IsMatch(s, pattern);
		}

		public static string JoinStringArray(string[] v, string delimiter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			for (int num = v.Length; i < num; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(delimiter);
				}
				stringBuilder.Append(v[i]);
			}
			return stringBuilder.ToString();
		}

		public static bool StringArrayContainsString(string[] array, string search)
		{
			if (array.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(search))
				{
					return true;
				}
			}
			return false;
		}

		public static long TimeIntervalSince1970()
		{
			return (long)(DateTime.Now.ToUniversalTime() - origin).TotalSeconds;
		}

		public static string ArrayOfObjectsToJsonString(List<JSONNode> arr)
		{
			JSONArray jSONArray = new JSONArray();
			foreach (JSONNode item in arr)
			{
				jSONArray.Add(item);
			}
			return jSONArray.ToString();
		}

		public static void CopyTo(this Stream input, Stream output)
		{
			byte[] array = new byte[16384];
			int count;
			while ((count = input.Read(array, 0, array.Length)) > 0)
			{
				output.Write(array, 0, count);
			}
		}
	}
}
