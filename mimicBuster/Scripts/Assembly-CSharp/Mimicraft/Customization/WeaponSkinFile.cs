using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class WeaponSkinFile
	{
		public const string Extension = ".weapons";

		private const string Magic = "MWS";

		private const byte Version = 3;

		private const byte VersionWithoutAds = 1;

		private const byte VersionWithoutSound = 2;

		private const int MaxWeapons = 64;

		private static bool IsReadable(byte version)
		{
			if (version != 3 && version != 2)
			{
				return version == 1;
			}
			return true;
		}

		public static byte[] Encode(IReadOnlyDictionary<string, WeaponSkinData> skins)
		{
			List<string> list = new List<string>();
			List<WeaponSkinData> list2 = new List<WeaponSkinData>();
			CharacterData characterData = new CharacterData("weapons");
			if (skins != null)
			{
				foreach (KeyValuePair<string, WeaponSkinData> skin in skins)
				{
					if (!string.IsNullOrWhiteSpace(skin.Key) && skin.Value != null)
					{
						list.Add(skin.Key);
						list2.Add(skin.Value);
						if (skin.Value.Grid != null && skin.Value.Grid.Count > 0)
						{
							characterData.Set(skin.Key, skin.Value.Grid);
						}
					}
				}
			}
			byte[] array = CharacterCodec.Encode(characterData);
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter.Write(Encoding.ASCII.GetBytes("MWS"));
			binaryWriter.Write((byte)3);
			binaryWriter.Write(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				WeaponSkinData weaponSkinData = list2[i];
				binaryWriter.Write(list[i]);
				Write(binaryWriter, weaponSkinData.LeftGrip);
				Write(binaryWriter, weaponSkinData.RightGrip);
				Write(binaryWriter, weaponSkinData.Muzzle);
				binaryWriter.Write(weaponSkinData.HasPoints);
				Write(binaryWriter, weaponSkinData.AdsPos);
				binaryWriter.Write(weaponSkinData.HasAdsPos);
				binaryWriter.Write((byte)weaponSkinData.Sound);
			}
			binaryWriter.Write(array.Length);
			binaryWriter.Write(array);
			binaryWriter.Flush();
			return memoryStream.ToArray();
		}

		public static bool TryDecode(byte[] bytes, out Dictionary<string, WeaponSkinData> skins)
		{
			skins = new Dictionary<string, WeaponSkinData>();
			if (bytes == null || bytes.Length < 5)
			{
				return false;
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(bytes, writable: false);
				using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MWS")
				{
					return false;
				}
				byte b = binaryReader.ReadByte();
				if (!IsReadable(b))
				{
					return false;
				}
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > 64)
				{
					return false;
				}
				List<string> list = new List<string>(num);
				for (int i = 0; i < num; i++)
				{
					string text = binaryReader.ReadString();
					WeaponSkinData weaponSkinData = new WeaponSkinData
					{
						LeftGrip = ReadVector(binaryReader),
						RightGrip = ReadVector(binaryReader),
						Muzzle = ReadVector(binaryReader)
					};
					weaponSkinData.HasPoints = binaryReader.ReadBoolean();
					if (b >= 2)
					{
						weaponSkinData.AdsPos = ReadVector(binaryReader);
						weaponSkinData.HasAdsPos = binaryReader.ReadBoolean();
					}
					if (b >= 3)
					{
						weaponSkinData.Sound = WeaponSounds.FromByte(binaryReader.ReadByte());
					}
					if (!string.IsNullOrWhiteSpace(text))
					{
						list.Add(text);
						skins[text] = weaponSkinData;
					}
				}
				int num2 = binaryReader.ReadInt32();
				if (num2 < 0 || num2 > memoryStream.Length - memoryStream.Position)
				{
					return false;
				}
				if (num2 == 0)
				{
					return true;
				}
				if (!CharacterCodec.TryDecode(binaryReader.ReadBytes(num2), "weapons", out var character, 512))
				{
					return false;
				}
				foreach (CharacterPartData part in character.Parts)
				{
					if (part != null && skins.TryGetValue(part.PartId, out var value))
					{
						value.Grid = part.Grid;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WeaponSkinFile] Dosya okunamadi: " + ex.Message);
				return false;
			}
		}

		private static void Write(BinaryWriter writer, Vector3 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
			writer.Write(value.z);
		}

		private static Vector3 ReadVector(BinaryReader reader)
		{
			return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
