using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class SharedValuesHolder : ISerializationCallbackReceiver
	{
		[NonSerialized]
		private Dictionary<(Type type, string name), object> sharedValues = new Dictionary<(Type, string), object>();

		private Type[] serializedTypes = new Type[0];

		private string[] serializedNames = new string[0];

		private object[] serializedVals = new object[0];

		public SharedValuesHolder()
		{
		}

		public SharedValuesHolder(SharedValuesHolder src)
		{
			sharedValues = new Dictionary<(Type, string), object>(sharedValues);
		}

		public object GetValue(Type holderType, string name)
		{
			if (sharedValues.TryGetValue((holderType, name), out var value))
			{
				return value;
			}
			return null;
		}

		public T GetValue<T>(Type holderType, string name)
		{
			if (sharedValues.TryGetValue((holderType, name), out var value))
			{
				return (T)value;
			}
			throw new Exception("Shared value " + name + " has not been added to holder");
		}

		public void SetValue(Type type, string name, object val)
		{
			(Type, string) key = (type, name);
			if (sharedValues.ContainsKey(key))
			{
				sharedValues[key] = val;
			}
			else
			{
				sharedValues.Add(key, val);
			}
		}

		public void SetDefaults(object obj)
		{
			Type type = obj.GetType();
			foreach (SharedValueDefaultAttribute customAttribute in type.GetCustomAttributes<SharedValueDefaultAttribute>())
			{
				(Type, string) key = (type, customAttribute.name);
				if (!sharedValues.ContainsKey(key))
				{
					sharedValues.Add(key, customAttribute.val);
				}
				else if (customAttribute.val.GetType() != sharedValues[key].GetType())
				{
					sharedValues[key] = customAttribute.val;
				}
			}
		}

		public (Type, string)[] GetTypeNames()
		{
			(Type, string)[] array = new(Type, string)[sharedValues.Count];
			sharedValues.Keys.CopyTo(array, 0);
			return array;
		}

		public void OnBeforeSerialize()
		{
			if (serializedTypes.Length != sharedValues.Count)
			{
				serializedTypes = new Type[sharedValues.Count];
				serializedNames = new string[sharedValues.Count];
				serializedVals = new object[sharedValues.Count];
			}
			int num = 0;
			foreach (KeyValuePair<(Type, string), object> sharedValue in sharedValues)
			{
				(Type, string) key = sharedValue.Key;
				serializedTypes[num] = key.Item1;
				serializedNames[num] = key.Item2;
				serializedVals[num] = sharedValue.Value;
				num++;
			}
		}

		public void OnAfterDeserialize()
		{
			sharedValues.Clear();
			for (int i = 0; i < serializedTypes.Length; i++)
			{
				sharedValues.Add((serializedTypes[i], serializedNames[i]), serializedVals[i]);
			}
		}
	}
}
