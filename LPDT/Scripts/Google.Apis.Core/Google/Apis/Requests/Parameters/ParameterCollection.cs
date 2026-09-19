using System;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Util;

namespace Google.Apis.Requests.Parameters
{
	public class ParameterCollection : List<KeyValuePair<string, string>>
	{
		public IEnumerable<string> this[string key] => GetAllMatches(key);

		public ParameterCollection()
		{
		}

		public ParameterCollection(IEnumerable<KeyValuePair<string, string>> collection)
			: base(collection)
		{
		}

		public void Add(string key, string value)
		{
			Add(new KeyValuePair<string, string>(key, value));
		}

		public bool ContainsKey(string key)
		{
			key.ThrowIfNullOrEmpty("key");
			string value;
			return TryGetValue(key, out value);
		}

		public bool TryGetValue(string key, out string value)
		{
			key.ThrowIfNullOrEmpty("key");
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.Current;
					if (current.Key.Equals(key))
					{
						value = current.Value;
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		public string GetFirstMatch(string key)
		{
			if (!TryGetValue(key, out var value))
			{
				throw new KeyNotFoundException("Parameter with the name '" + key + "' was not found.");
			}
			return value;
		}

		public IEnumerable<string> GetAllMatches(string key)
		{
			key.ThrowIfNullOrEmpty("key");
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				if (current.Key.Equals(key))
				{
					yield return current.Value;
				}
			}
		}

		public static ParameterCollection FromQueryString(string qs)
		{
			ParameterCollection parameterCollection = new ParameterCollection();
			string[] array = qs.Split(new char[1] { '&' });
			foreach (string text in array)
			{
				string[] array2 = text.Split(new char[1] { '=' });
				if (array2.Length == 2)
				{
					parameterCollection.Add(Uri.UnescapeDataString(array2[0]), Uri.UnescapeDataString(array2[1]));
					continue;
				}
				throw new ArgumentException($"Invalid query string [{qs}]. Invalid part [{text}]");
			}
			return parameterCollection;
		}

		public static ParameterCollection FromDictionary(IDictionary<string, object> dictionary)
		{
			ParameterCollection parameterCollection = new ParameterCollection();
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				IEnumerable enumerable = item.Value as IEnumerable;
				if (!(item.Value is string) && enumerable != null)
				{
					foreach (object item2 in enumerable)
					{
						parameterCollection.Add(item.Key, Utilities.ConvertToString(item2));
					}
				}
				else
				{
					parameterCollection.Add(item.Key, (item.Value == null) ? null : Utilities.ConvertToString(item.Value));
				}
			}
			return parameterCollection;
		}
	}
}
