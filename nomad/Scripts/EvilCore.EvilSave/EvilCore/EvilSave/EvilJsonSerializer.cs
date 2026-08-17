using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EvilCore.EvilSave
{
	public static class EvilJsonSerializer
	{
		[Serializable]
		private class JsonSaveData
		{
			public int version;

			public List<JsonEntry> entries = new List<JsonEntry>();
		}

		[Serializable]
		private class JsonEntry
		{
			public string key;

			public string base64Value;
		}

		public static string Serialize(SaveData data)
		{
			JsonSaveData jsonSaveData = new JsonSaveData
			{
				version = data.Version
			};
			foreach (KeyValuePair<string, byte[]> entry in data.Entries)
			{
				jsonSaveData.entries.Add(new JsonEntry
				{
					key = entry.Key,
					base64Value = Convert.ToBase64String(entry.Value)
				});
			}
			return JsonUtility.ToJson(jsonSaveData, prettyPrint: true);
		}

		public static SaveData Deserialize(string json)
		{
			JsonSaveData jsonSaveData = JsonUtility.FromJson<JsonSaveData>(json);
			SaveData saveData = new SaveData
			{
				Version = jsonSaveData.version
			};
			foreach (JsonEntry entry in jsonSaveData.entries)
			{
				saveData.Entries[entry.key] = Convert.FromBase64String(entry.base64Value);
			}
			return saveData;
		}

		public static byte[] SerializeToBytes(SaveData data)
		{
			return Encoding.UTF8.GetBytes(Serialize(data));
		}

		public static SaveData DeserializeFromBytes(byte[] bytes)
		{
			return Deserialize(Encoding.UTF8.GetString(bytes));
		}
	}
}
