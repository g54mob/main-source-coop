using System;
using System.Collections.Generic;

namespace Den.Tools
{
	public class TypeDict
	{
		private Dictionary<Type, object> dict = new Dictionary<Type, object>();

		public TypeDict()
		{
		}

		public TypeDict(object[] arr)
		{
			Add(arr);
		}

		public void Add<T>(T val)
		{
			dict.Add(typeof(T), val);
		}

		public void TryAdd<T>(T val)
		{
			if (!dict.ContainsKey(typeof(T)))
			{
				dict.Add(typeof(T), val);
			}
		}

		public void ForceAdd<T>(T val)
		{
			if (dict.ContainsKey(typeof(T)))
			{
				dict[typeof(T)] = val;
			}
			else
			{
				dict.Add(typeof(T), val);
			}
		}

		public void Add(object[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				Type type = arr[i].GetType();
				if (!dict.ContainsKey(type))
				{
					dict.Add(type, arr[i]);
				}
			}
		}

		public bool TryGetValue<T>(out T val)
		{
			object value;
			bool result = dict.TryGetValue(typeof(T), out value);
			val = (T)value;
			return result;
		}

		public T GetValue<T>()
		{
			return (T)dict[typeof(T)];
		}

		public bool ContainsKey<T>()
		{
			return dict.ContainsKey(typeof(T));
		}

		public void Clear()
		{
			dict.Clear();
		}
	}
	public class TypeDict<T2>
	{
		private Dictionary<Type, T2> dict = new Dictionary<Type, T2>();

		public void Add<T1>(T2 val)
		{
			dict.Add(typeof(T1), val);
		}

		public void TryAdd<T1>(T2 val)
		{
			if (!dict.ContainsKey(typeof(T1)))
			{
				dict.Add(typeof(T1), val);
			}
		}

		public void ForceAdd<T1>(T2 val)
		{
			if (dict.ContainsKey(typeof(T1)))
			{
				dict[typeof(T1)] = val;
			}
			else
			{
				dict.Add(typeof(T1), val);
			}
		}

		public bool TryGetValue<T1>(out T2 t2)
		{
			return dict.TryGetValue(typeof(T1), out t2);
		}

		public T2 GetValue<T1>()
		{
			return dict[typeof(T1)];
		}

		public bool ContainsKey<T1>()
		{
			return dict.ContainsKey(typeof(T1));
		}

		public void Clear()
		{
			dict.Clear();
		}
	}
}
