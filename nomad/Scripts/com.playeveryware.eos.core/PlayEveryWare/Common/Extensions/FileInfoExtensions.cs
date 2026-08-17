using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace PlayEveryWare.Common.Extensions
{
	public static class FileInfoExtensions
	{
		private const int ReadBytesBufferSize = 8;

		public static string ComputeSHA(this FileInfo fileInfo)
		{
			using SHA1 sHA = SHA1.Create();
			using FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
			fileStream.Position = 0L;
			byte[] array = sHA.ComputeHash(fileStream);
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		[Conditional("ENABLE_FILEINFO_EXTENSIONS_DEBUG")]
		private static void LogInequalityReason(FileInfo one, FileInfo two, string reason)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("The following files were inequal because: " + reason);
			stringBuilder.AppendLine("File #1: \"" + one.FullName + "\"");
			stringBuilder.AppendLine("File #2: \"" + two.FullName + "\"");
			Debug.Log(stringBuilder.ToString());
		}

		public static bool AreContentsSemanticallyEqual(this FileInfo fileInfo, FileInfo other)
		{
			if (fileInfo.Exists != other.Exists)
			{
				return false;
			}
			if (!fileInfo.Exists && !other.Exists)
			{
				return true;
			}
			if (fileInfo.Length != other.Length)
			{
				return false;
			}
			if (string.Equals(fileInfo.FullName, other.FullName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			int num = (int)Math.Ceiling((double)fileInfo.Length / 8.0);
			using FileStream fileStream = fileInfo.OpenRead();
			using FileStream fileStream2 = other.OpenRead();
			byte[] array = new byte[8];
			byte[] array2 = new byte[8];
			for (int i = 0; i < num; i++)
			{
				int num2 = fileStream.Read(array, 0, 8);
				int num3 = fileStream2.Read(array2, 0, 8);
				if (num2 != num3)
				{
					return false;
				}
				if (BitConverter.ToInt64(array, 0) != BitConverter.ToInt64(array2, 0))
				{
					return false;
				}
			}
			return true;
		}
	}
}
