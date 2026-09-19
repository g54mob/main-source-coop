using System.Collections.Generic;
using System.Linq;

namespace RSG.Muffin.MuffinExtensions.Dictionary
{
	public static class DictionaryExtensions
	{
		public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToAdd)
		{
			foreach (KeyValuePair<TKey, TValue> item in dictionaryToAdd)
			{
				dictionary.Add(item.Key, item.Value);
			}
		}

		public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair)
		{
			dictionary.Add(keyValuePair.Key, keyValuePair.Value);
		}

		public static void RemoveAt<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, int id)
		{
			dictionary.Remove(dictionary.ElementAt(id).Key);
		}
	}
}
