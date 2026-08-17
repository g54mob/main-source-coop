using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
	internal sealed class FixedTypeKeyHashtable<TValue>
	{
		private readonly struct HashEntry
		{
			public readonly Type Type;

			public readonly TValue Value;

			public HashEntry(Type key, TValue value)
			{
				Type = key;
				Value = value;
			}
		}

		private readonly HashEntry[][] table;

		private readonly int indexFor;

		public FixedTypeKeyHashtable(KeyValuePair<Type, TValue>[] values, float loadFactor = 0.75f)
		{
			int num = (int)((float)values.Length / loadFactor);
			int num2;
			for (num2 = 1; num2 < num; num2 <<= 1)
			{
			}
			table = new HashEntry[num2][];
			indexFor = table.Length - 1;
			for (int i = 0; i < values.Length; i++)
			{
				KeyValuePair<Type, TValue> keyValuePair = values[i];
				int hashCode = RuntimeHelpers.GetHashCode(keyValuePair.Key);
				HashEntry[] array = table[hashCode & indexFor];
				if (array == null)
				{
					array = new HashEntry[1]
					{
						new HashEntry(keyValuePair.Key, keyValuePair.Value)
					};
				}
				else
				{
					HashEntry[] array2 = new HashEntry[array.Length + 1];
					Array.Copy(array, array2, array.Length);
					array = array2;
					array[array.Length - 1] = new HashEntry(keyValuePair.Key, keyValuePair.Value);
				}
				table[hashCode & indexFor] = array;
			}
		}

		public TValue Get(Type type)
		{
			int hashCode = RuntimeHelpers.GetHashCode(type);
			HashEntry[] array = table[hashCode & indexFor];
			if (array != null)
			{
				if (array[0].Type == type)
				{
					return array[0].Value;
				}
				for (int i = 1; i < array.Length; i++)
				{
					if (array[i].Type == type)
					{
						return array[i].Value;
					}
				}
			}
			throw new KeyNotFoundException("Type was not found, Type: " + type.FullName);
		}

		public bool TryGet(Type type, out TValue value)
		{
			int hashCode = RuntimeHelpers.GetHashCode(type);
			HashEntry[] array = table[hashCode & indexFor];
			if (array != null)
			{
				if (array[0].Type == type)
				{
					value = array[0].Value;
					return true;
				}
				for (int i = 1; i < array.Length; i++)
				{
					if (array[i].Type == type)
					{
						value = array[i].Value;
						return true;
					}
				}
			}
			value = default(TValue);
			return false;
		}
	}
}
