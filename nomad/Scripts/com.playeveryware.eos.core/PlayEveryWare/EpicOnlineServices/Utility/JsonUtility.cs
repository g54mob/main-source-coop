using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class JsonUtility
	{
		private static readonly JsonSerializerSettings s_serializerSettings = new JsonSerializerSettings
		{
			Converters = new JsonConverter[3]
			{
				new GuidConverter(),
				new StringEnumConverter(),
				new VersionConverter()
			}
		};

		private static bool TryFromJson<T>(string json, out T obj)
		{
			obj = default(T);
			try
			{
				obj = JsonConvert.DeserializeObject<T>(json, s_serializerSettings);
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to parse object of type \"" + typeof(T).FullName + "\" from JSON: \"" + json + "\". Exception: \"" + ex.Message + "\"");
				return false;
			}
		}

		public static string ToJson(object obj, bool pretty = false)
		{
			return JsonConvert.SerializeObject(obj, pretty ? Formatting.Indented : Formatting.None, s_serializerSettings);
		}

		public static T FromJson<T>(string json)
		{
			TryFromJson<T>(json, out var obj);
			return obj;
		}

		public static T FromJsonFile<T>(string filepath)
		{
			return FromJson<T>(FileSystemUtility.ReadAllText(filepath));
		}

		public static void FromJsonOverwrite<T>(string json, T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			try
			{
				JsonConvert.PopulateObject(json, obj, s_serializerSettings);
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to populate object of type \"" + typeof(T).FullName + "\" from JSON: \"" + json + "\". Exception: \"" + ex.Message + "\"");
			}
		}
	}
}
