using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal sealed class KeyedGetSetValueStore<TKey> : IEnumerable, IDictionary<TKey, object>, ICollection<KeyValuePair<TKey, object>>, IEnumerable<KeyValuePair<TKey, object>>
	{
		private readonly Dictionary<TKey, object> vhgmYKGNVGCYssLrjcOzzIQgRUKC;

		private readonly bool ODlPwcUBWKkcPZhdyPenWcLejYlg;

		public int Count => vhgmYKGNVGCYssLrjcOzzIQgRUKC.Count;

		public bool isReadOnlyCollection => ODlPwcUBWKkcPZhdyPenWcLejYlg;

		ICollection<TKey> IDictionary<TKey, object>.Keys => vhgmYKGNVGCYssLrjcOzzIQgRUKC.Keys;

		ICollection<object> IDictionary<TKey, object>.Values => vhgmYKGNVGCYssLrjcOzzIQgRUKC.Values;

		object IDictionary<TKey, object>.this[TKey P_0]
		{
			get
			{
				return vhgmYKGNVGCYssLrjcOzzIQgRUKC[P_0];
			}
			set
			{
				DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
				vhgmYKGNVGCYssLrjcOzzIQgRUKC[key] = value2;
			}
		}

		int ICollection<KeyValuePair<TKey, object>>.Count => vhgmYKGNVGCYssLrjcOzzIQgRUKC.Count;

		bool ICollection<KeyValuePair<TKey, object>>.IsReadOnly => ODlPwcUBWKkcPZhdyPenWcLejYlg;

		public KeyedGetSetValueStore(Dictionary<TKey, object> P_0, bool P_1)
		{
			vhgmYKGNVGCYssLrjcOzzIQgRUKC = P_0;
			ODlPwcUBWKkcPZhdyPenWcLejYlg = P_1;
		}

		public KeyedGetSetValueStore(bool P_0)
		{
			ODlPwcUBWKkcPZhdyPenWcLejYlg = P_0;
			vhgmYKGNVGCYssLrjcOzzIQgRUKC = new Dictionary<TKey, object>();
		}

		public void AddItem<TValue>(TKey key, IGetSetValue<TValue> item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			vhgmYKGNVGCYssLrjcOzzIQgRUKC.Add(key, item);
		}

		public IGetSetValue<TValue> GetItem<TValue>(TKey key)
		{
			if (!vhgmYKGNVGCYssLrjcOzzIQgRUKC.TryGetValue(key, out var value) || !(value is IGetSetValue<TValue> result))
			{
				PZxFkmBVmHfynxOYMIsoTLrPXnqM(key, typeof(TValue));
				return null;
			}
			return result;
		}

		public bool RemoveItem<TValue>(TKey key)
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.Remove(key);
		}

		public bool ContainsKey(TKey key)
		{
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.ContainsKey(key);
		}

		public void Clear()
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			vhgmYKGNVGCYssLrjcOzzIQgRUKC.Clear();
		}

		public bool ContainsValue<TValue>(TKey key)
		{
			if (vhgmYKGNVGCYssLrjcOzzIQgRUKC.TryGetValue(key, out var value))
			{
				return value is IGetSetValue<TValue>;
			}
			return false;
		}

		public TValue GetValue<TValue>(TKey key)
		{
			if (!TryGetValue<TValue>(key, out var value))
			{
				PZxFkmBVmHfynxOYMIsoTLrPXnqM(key, typeof(TValue));
			}
			return value;
		}

		public void SetValue<TValue>(TKey key, TValue value)
		{
			if (!TrySetValue(key, value))
			{
				PZxFkmBVmHfynxOYMIsoTLrPXnqM(key, typeof(TValue));
			}
		}

		public bool TryGetValue<TValue>(TKey key, out TValue value)
		{
			if (!vhgmYKGNVGCYssLrjcOzzIQgRUKC.TryGetValue(key, out var value2) || !(value2 is IGetValue<TValue> getValue))
			{
				value = default(TValue);
				Logger.LogError(ymSCrGdloJCYacYSeBWIIAAOLyqT(key, typeof(TValue)), requiredThreadSafety: true);
				return false;
			}
			value = getValue.GetValue();
			return true;
		}

		public bool TrySetValue<TValue>(TKey key, TValue value)
		{
			ISetValue<TValue> setValue;
			if (!vhgmYKGNVGCYssLrjcOzzIQgRUKC.TryGetValue(key, out var value2) || (setValue = value2 as GetSetValue<TValue>) == null)
			{
				Logger.LogError(ymSCrGdloJCYacYSeBWIIAAOLyqT(key, typeof(TValue)), requiredThreadSafety: true);
				return false;
			}
			setValue.SetValue(value);
			return true;
		}

		private void DcXiEIErDgHhAMPiWzVHzKbFEhbaA()
		{
			if (ODlPwcUBWKkcPZhdyPenWcLejYlg)
			{
				throw new Exception("The collection is read-only.");
			}
		}

		private static void PZxFkmBVmHfynxOYMIsoTLrPXnqM(TKey P_0, Type P_1)
		{
			throw new Exception(ymSCrGdloJCYacYSeBWIIAAOLyqT(P_0, P_1));
		}

		private static string ymSCrGdloJCYacYSeBWIIAAOLyqT(TKey P_0, Type P_1)
		{
			string[] obj = new string[5] { "Value with key ", null, null, null, null };
			TKey val = P_0;
			obj[1] = val?.ToString();
			obj[2] = " of type ";
			obj[3] = P_1?.ToString();
			obj[4] = " not found.";
			return string.Concat(obj);
		}

		void IDictionary<TKey, object>.Add(TKey P_0, object P_1)
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			vhgmYKGNVGCYssLrjcOzzIQgRUKC.Add(P_0, P_1);
		}

		bool IDictionary<TKey, object>.ContainsKey(TKey P_0)
		{
			return ContainsKey(P_0);
		}

		bool IDictionary<TKey, object>.Remove(TKey P_0)
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.Remove(P_0);
		}

		bool IDictionary<TKey, object>.TryGetValue(TKey P_0, out object P_1)
		{
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.TryGetValue(P_0, out P_1);
		}

		void ICollection<KeyValuePair<TKey, object>>.Add(KeyValuePair<TKey, object> P_0)
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			((ICollection<KeyValuePair<TKey, object>>)vhgmYKGNVGCYssLrjcOzzIQgRUKC).Add(P_0);
		}

		void ICollection<KeyValuePair<TKey, object>>.Clear()
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			((ICollection<KeyValuePair<TKey, object>>)vhgmYKGNVGCYssLrjcOzzIQgRUKC).Clear();
		}

		bool ICollection<KeyValuePair<TKey, object>>.Contains(KeyValuePair<TKey, object> P_0)
		{
			return ((ICollection<KeyValuePair<TKey, object>>)vhgmYKGNVGCYssLrjcOzzIQgRUKC).Contains(P_0);
		}

		void ICollection<KeyValuePair<TKey, object>>.CopyTo(KeyValuePair<TKey, object>[] P_0, int P_1)
		{
			((ICollection<KeyValuePair<TKey, object>>)vhgmYKGNVGCYssLrjcOzzIQgRUKC).CopyTo(P_0, P_1);
		}

		bool ICollection<KeyValuePair<TKey, object>>.Remove(KeyValuePair<TKey, object> P_0)
		{
			DcXiEIErDgHhAMPiWzVHzKbFEhbaA();
			return ((ICollection<KeyValuePair<TKey, object>>)vhgmYKGNVGCYssLrjcOzzIQgRUKC).Remove(P_0);
		}

		IEnumerator<KeyValuePair<TKey, object>> IEnumerable<KeyValuePair<TKey, object>>.GetEnumerator()
		{
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC.GetEnumerator();
		}
	}
}
