using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HeathenEngineering.Serializable
{
	[Serializable]
	public class KeyedDataLibrary
	{
		private List<KeyedObject> Values;

		[NonSerialized]
		private Dictionary<string, KeyedObject> Index;

		[NonSerialized]
		private bool isIndexed;

		public object this[string key]
		{
			get
			{
				return GetValue(key);
			}
			set
			{
				SetValue(key, value);
			}
		}

		public KeyedDataLibrary()
		{
			Values = new List<KeyedObject>();
			Index = new Dictionary<string, KeyedObject>();
			isIndexed = false;
		}

		public void BuildIndex()
		{
			if (isIndexed)
			{
				return;
			}
			if (Index == null)
			{
				Index = new Dictionary<string, KeyedObject>();
			}
			else if (Index.Count > 0)
			{
				Index.Clear();
			}
			foreach (KeyedObject value in Values)
			{
				Index.Add(value.Key, value);
			}
		}

		public bool Contains(string key)
		{
			BuildIndex();
			return Index.ContainsKey(key);
		}

		public object GetValue(string key)
		{
			BuildIndex();
			if (Index.ContainsKey(key))
			{
				return Index[key].Data;
			}
			return null;
		}

		public T GetValue<T>(string key)
		{
			BuildIndex();
			if (Index.ContainsKey(key))
			{
				return (T)Index[key].Data;
			}
			return default(T);
		}

		public void SetValue(string key, object value)
		{
			BuildIndex();
			if (Index.ContainsKey(key))
			{
				Index[key].Data = value;
				return;
			}
			KeyedObject keyedObject = new KeyedObject
			{
				Key = key,
				Data = value
			};
			Values.Add(keyedObject);
			Index.Add(key, keyedObject);
		}

		public void Remove(string key)
		{
			BuildIndex();
			if (Index.ContainsKey(key))
			{
				Values.Remove(Index[key]);
				Index.Remove(key);
			}
		}

		public static byte[] Serialize(KeyedDataLibrary Library)
		{
			byte[] result = null;
			if (Library != null)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, Library);
				result = memoryStream.ToArray();
				memoryStream.Dispose();
			}
			return result;
		}

		public static KeyedDataLibrary Deserialize(byte[] Buffer)
		{
			KeyedDataLibrary keyedDataLibrary = null;
			if (Buffer != null && Buffer.Length != 0)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream(Buffer);
				keyedDataLibrary = binaryFormatter.Deserialize(memoryStream) as KeyedDataLibrary;
				memoryStream.Dispose();
			}
			if (keyedDataLibrary != null)
			{
				keyedDataLibrary.isIndexed = false;
				keyedDataLibrary.BuildIndex();
			}
			return keyedDataLibrary;
		}
	}
}
