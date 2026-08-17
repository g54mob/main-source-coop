using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EvilCore.EvilSave
{
	public class SaveData
	{
		private static readonly byte[] Magic = new byte[4] { 69, 86, 73, 76 };

		public int Version;

		public Dictionary<string, byte[]> Entries = new Dictionary<string, byte[]>();

		public void Set<T>(string key, T value)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using EvilWriter evilWriter = new EvilWriter(memoryStream);
			evilWriter.Write(value);
			Entries[key] = memoryStream.ToArray();
		}

		public T Get<T>(string key, T defaultValue = default(T))
		{
			if (!Entries.TryGetValue(key, out var value))
			{
				return defaultValue;
			}
			using MemoryStream stream = new MemoryStream(value);
			using EvilReader evilReader = new EvilReader(stream);
			try
			{
				return evilReader.Read<T>();
			}
			catch
			{
				return defaultValue;
			}
		}

		public bool HasKey(string key)
		{
			return Entries.ContainsKey(key);
		}

		public void Remove(string key)
		{
			Entries.Remove(key);
		}

		public string[] GetKeys()
		{
			string[] array = new string[Entries.Count];
			Entries.Keys.CopyTo(array, 0);
			return array;
		}

		public byte[] Serialize()
		{
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter.Write(Magic);
			binaryWriter.Write(Version);
			binaryWriter.Write(Entries.Count);
			foreach (KeyValuePair<string, byte[]> entry in Entries)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(entry.Key);
				binaryWriter.Write((ushort)bytes.Length);
				binaryWriter.Write(bytes);
				binaryWriter.Write(entry.Value.Length);
				binaryWriter.Write(entry.Value);
			}
			return memoryStream.ToArray();
		}

		public static SaveData Deserialize(byte[] data)
		{
			using MemoryStream input = new MemoryStream(data);
			using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
			byte[] array = binaryReader.ReadBytes(4);
			if (array[0] != Magic[0] || array[1] != Magic[1] || array[2] != Magic[2] || array[3] != Magic[3])
			{
				throw new InvalidOperationException("Invalid save file: missing EVIL magic header");
			}
			SaveData saveData = new SaveData
			{
				Version = binaryReader.ReadInt32()
			};
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ushort count = binaryReader.ReadUInt16();
				byte[] bytes = binaryReader.ReadBytes(count);
				string key = Encoding.UTF8.GetString(bytes);
				int count2 = binaryReader.ReadInt32();
				byte[] value = binaryReader.ReadBytes(count2);
				saveData.Entries[key] = value;
			}
			return saveData;
		}
	}
}
