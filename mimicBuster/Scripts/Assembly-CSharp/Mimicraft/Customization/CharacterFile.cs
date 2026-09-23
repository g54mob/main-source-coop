using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class CharacterFile
	{
		public const string Extension = ".character";

		private const string Magic = "MCF";

		private const byte Version = 3;

		private const byte VersionWithoutVoice = 2;

		private const byte VersionWithoutBoxes = 1;

		private static bool IsReadable(byte version)
		{
			if (version != 3 && version != 2)
			{
				return version == 1;
			}
			return true;
		}

		public static byte[] Encode(string characterName, CharacterData data, CharacterRigDefinition rig = null)
		{
			byte[] array = CharacterCodec.Encode(data);
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter.Write(Encoding.ASCII.GetBytes("MCF"));
			binaryWriter.Write((byte)3);
			binaryWriter.Write(characterName ?? "");
			binaryWriter.Write(data?.RigId ?? "");
			WriteBoxes(binaryWriter, data, rig);
			binaryWriter.Write((byte)(data?.VoiceType ?? CharacterVoice.Male));
			binaryWriter.Write(array.Length);
			binaryWriter.Write(array);
			binaryWriter.Flush();
			return memoryStream.ToArray();
		}

		private static void WriteBoxes(BinaryWriter writer, CharacterData data, CharacterRigDefinition rig)
		{
			if (data == null || rig == null)
			{
				writer.Write(0);
				return;
			}
			List<(string, Vector3Int)> list = new List<(string, Vector3Int)>();
			foreach (CharacterPartData part in data.Parts)
			{
				CharacterPartDefinition characterPartDefinition = rig.Find(part?.PartId);
				if (characterPartDefinition != null)
				{
					list.Add((part.PartId, characterPartDefinition.BoxSize));
				}
			}
			writer.Write(list.Count);
			foreach (var (value, vector3Int) in list)
			{
				writer.Write(value);
				writer.Write(vector3Int.x);
				writer.Write(vector3Int.y);
				writer.Write(vector3Int.z);
			}
		}

		public static bool TryDecode(byte[] bytes, out string characterName, out CharacterData data)
		{
			Dictionary<string, Vector3Int> savedBoxes;
			return TryDecode(bytes, out characterName, out data, out savedBoxes);
		}

		public static bool TryDecode(byte[] bytes, out string characterName, out CharacterData data, out Dictionary<string, Vector3Int> savedBoxes)
		{
			characterName = "";
			data = null;
			savedBoxes = new Dictionary<string, Vector3Int>();
			if (bytes == null || bytes.Length < 5)
			{
				return false;
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(bytes, writable: false);
				using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MCF")
				{
					return false;
				}
				byte b = binaryReader.ReadByte();
				if (!IsReadable(b))
				{
					return false;
				}
				characterName = binaryReader.ReadString();
				string rigId = binaryReader.ReadString();
				if (b >= 2 && !TryReadBoxes(binaryReader, savedBoxes))
				{
					return false;
				}
				CharacterVoice voiceType = ((b >= 3) ? CharacterVoices.FromByte(binaryReader.ReadByte()) : CharacterVoice.Male);
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > memoryStream.Length - memoryStream.Position)
				{
					return false;
				}
				byte[] array = binaryReader.ReadBytes(num);
				if (array.Length == 0)
				{
					data = new CharacterData(rigId)
					{
						VoiceType = voiceType
					};
					return true;
				}
				if (!CharacterCodec.TryDecode(array, rigId, out data))
				{
					return false;
				}
				data.VoiceType = voiceType;
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CharacterFile] Dosya okunamadi: " + ex.Message);
				return false;
			}
		}

		private static bool TryReadBoxes(BinaryReader reader, Dictionary<string, Vector3Int> into)
		{
			int num = reader.ReadInt32();
			if (num < 0 || num > 1024)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadString();
				Vector3Int value = new Vector3Int(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
				if (!string.IsNullOrEmpty(text))
				{
					into[text] = value;
				}
			}
			return true;
		}

		public static bool TryReadName(byte[] bytes, out string characterName)
		{
			characterName = "";
			if (bytes == null || bytes.Length < 5)
			{
				return false;
			}
			try
			{
				using MemoryStream input = new MemoryStream(bytes, writable: false);
				using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MCF")
				{
					return false;
				}
				if (!IsReadable(binaryReader.ReadByte()))
				{
					return false;
				}
				characterName = binaryReader.ReadString();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
