using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mimicraft.Customization
{
	public static class WeaponSkinEntryFile
	{
		public const string Extension = ".weaponskin";

		private const string Magic = "MWE";

		private const byte Version = 1;

		private const int MaxNameLength = 128;

		public static byte[] Encode(string weaponId, string skinName, WeaponSkinData data)
		{
			Dictionary<string, WeaponSkinData> dictionary = new Dictionary<string, WeaponSkinData>();
			if (!string.IsNullOrWhiteSpace(weaponId) && data != null)
			{
				dictionary[weaponId] = data;
			}
			byte[] array = WeaponSkinFile.Encode(dictionary);
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter.Write(Encoding.ASCII.GetBytes("MWE"));
			binaryWriter.Write((byte)1);
			binaryWriter.Write(weaponId ?? "");
			binaryWriter.Write(Trim(skinName));
			binaryWriter.Write(array.Length);
			binaryWriter.Write(array);
			return memoryStream.ToArray();
		}

		public static bool TryDecode(byte[] bytes, out string weaponId, out string skinName, out WeaponSkinData data)
		{
			data = null;
			if (!TryReadPayload(bytes, out weaponId, out skinName, out var payload))
			{
				return false;
			}
			if (!WeaponSkinFile.TryDecode(payload, out var skins))
			{
				return false;
			}
			if (skins.TryGetValue(weaponId, out data))
			{
				return data != null;
			}
			return false;
		}

		public static bool TryReadPayload(byte[] bytes, out string weaponId, out string skinName, out byte[] payload)
		{
			weaponId = "";
			skinName = "";
			payload = null;
			if (bytes == null || bytes.Length < 6)
			{
				return false;
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(bytes);
				using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MWE" || binaryReader.ReadByte() != 1)
				{
					return false;
				}
				weaponId = binaryReader.ReadString();
				skinName = binaryReader.ReadString();
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > memoryStream.Length - memoryStream.Position)
				{
					return false;
				}
				payload = binaryReader.ReadBytes(num);
				return payload.Length == num && !string.IsNullOrEmpty(weaponId);
			}
			catch (Exception)
			{
				payload = null;
				return false;
			}
		}

		public static bool TryReadHeader(string path, out string weaponId, out string skinName)
		{
			weaponId = "";
			skinName = "";
			try
			{
				using FileStream input = File.OpenRead(path);
				using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MWE" || binaryReader.ReadByte() != 1)
				{
					return false;
				}
				weaponId = binaryReader.ReadString();
				skinName = binaryReader.ReadString();
				return !string.IsNullOrEmpty(weaponId);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static string Trim(string name)
		{
			name = (name ?? "").Trim();
			if (name.Length > 128)
			{
				return name.Substring(0, 128);
			}
			return name;
		}
	}
}
