using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zorro.Core.Serizalization
{
	public static class FlatJsonSerializer
	{
		public class DeserializeResult
		{
			public List<object> Objects;

			public DeserializeResult(List<object> objects)
			{
				Objects = objects;
			}

			public bool TryGetObjects<T>(out IEnumerable<T> objects)
			{
				objects = Objects.FindAll((object o) => o is T).ConvertAll((object o) => (T)o);
				return objects.Any();
			}

			public bool TryGetObject<T>(out T obj)
			{
				obj = (T)Objects.Find((object o) => o is T);
				return obj != null;
			}
		}

		public static string Serialize(object[] objects)
		{
			if (objects == null || objects.Length == 0)
			{
				throw new Exception("No objects to serialize!");
			}
			string text = "";
			foreach (object obj in objects)
			{
				string text2 = JsonUtility.ToJson(obj);
				text = text + obj.GetType().AssemblyQualifiedName + "|" + text2 + "|";
			}
			return text.Substring(0, text.Length - 1);
		}

		public static DeserializeResult Deserialize(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				throw new Exception("No json to deserialize!");
			}
			List<object> list = new List<object>();
			string[] array = json.Split('|');
			for (int i = 0; i < array.Length; i += 2)
			{
				Type type = Type.GetType(array[i]);
				object obj = JsonUtility.FromJson(array[i + 1], type);
				list.Add(obj);
				list[list.Count - 1] = obj;
			}
			return new DeserializeResult(list);
		}
	}
}
