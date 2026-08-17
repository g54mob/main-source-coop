using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal sealed class KeyedGetSetValueStore<TKey> : IDictionary<TKey, object>, ICollection<KeyValuePair<TKey, object>>, IEnumerable<KeyValuePair<TKey, object>>, IEnumerable
	{
		private readonly Dictionary<TKey, object> itFrICAJCfoFXDfPQSEyJfSLqhaA;

		private readonly bool ljcRKLlMKsZaueVrRjGZyImMcUwEA;

		public int Count => itFrICAJCfoFXDfPQSEyJfSLqhaA.Count;

		public bool isReadOnlyCollection => ljcRKLlMKsZaueVrRjGZyImMcUwEA;

		ICollection<TKey> IDictionary<TKey, object>.Keys => itFrICAJCfoFXDfPQSEyJfSLqhaA.Keys;

		ICollection<object> IDictionary<TKey, object>.Values => itFrICAJCfoFXDfPQSEyJfSLqhaA.Values;

		object IDictionary<TKey, object>.this[TKey P_0]
		{
			get
			{
				return itFrICAJCfoFXDfPQSEyJfSLqhaA[P_0];
			}
			set
			{
				qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
				itFrICAJCfoFXDfPQSEyJfSLqhaA[key] = value2;
			}
		}

		int ICollection<KeyValuePair<TKey, object>>.Count => itFrICAJCfoFXDfPQSEyJfSLqhaA.Count;

		bool ICollection<KeyValuePair<TKey, object>>.IsReadOnly => ljcRKLlMKsZaueVrRjGZyImMcUwEA;

		public KeyedGetSetValueStore(Dictionary<TKey, object> P_0, bool P_1)
		{
			itFrICAJCfoFXDfPQSEyJfSLqhaA = P_0;
			ljcRKLlMKsZaueVrRjGZyImMcUwEA = P_1;
		}

		public KeyedGetSetValueStore(bool P_0)
		{
			ljcRKLlMKsZaueVrRjGZyImMcUwEA = P_0;
			itFrICAJCfoFXDfPQSEyJfSLqhaA = new Dictionary<TKey, object>();
		}

		public void AddItem<TValue>(TKey key, IGetSetValue<TValue> item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			itFrICAJCfoFXDfPQSEyJfSLqhaA.Add(key, item);
		}

		public IGetSetValue<TValue> GetItem<TValue>(TKey key)
		{
			if (!itFrICAJCfoFXDfPQSEyJfSLqhaA.TryGetValue(key, out var value) || !(value is IGetSetValue<TValue> result))
			{
				CVcPfMnfWgicoQBKttfPShKGIiFBA(key, typeof(TValue));
				return null;
			}
			return result;
		}

		public bool RemoveItem<TValue>(TKey key)
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.Remove(key);
		}

		public bool ContainsKey(TKey key)
		{
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.ContainsKey(key);
		}

		public void Clear()
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			itFrICAJCfoFXDfPQSEyJfSLqhaA.Clear();
		}

		public bool ContainsValue<TValue>(TKey key)
		{
			if (itFrICAJCfoFXDfPQSEyJfSLqhaA.TryGetValue(key, out var value))
			{
				return value is IGetSetValue<TValue>;
			}
			return false;
		}

		public TValue GetValue<TValue>(TKey key)
		{
			if (!TryGetValue<TValue>(key, out var value))
			{
				CVcPfMnfWgicoQBKttfPShKGIiFBA(key, typeof(TValue));
			}
			return value;
		}

		public void SetValue<TValue>(TKey key, TValue value)
		{
			if (!TrySetValue(key, value))
			{
				CVcPfMnfWgicoQBKttfPShKGIiFBA(key, typeof(TValue));
			}
		}

		public bool TryGetValue<TValue>(TKey key, out TValue value)
		{
			if (!itFrICAJCfoFXDfPQSEyJfSLqhaA.TryGetValue(key, out var value2) || !(value2 is IGetValue<TValue> getValue))
			{
				value = default(TValue);
				Logger.LogError(PmsklqZYysERcKCAPQDotLfXJFHv(key, typeof(TValue)), requiredThreadSafety: true);
				return false;
			}
			value = getValue.GetValue();
			return true;
		}

		public bool TrySetValue<TValue>(TKey key, TValue value)
		{
			ISetValue<TValue> setValue;
			if (!itFrICAJCfoFXDfPQSEyJfSLqhaA.TryGetValue(key, out var value2) || (setValue = value2 as GetSetValue<TValue>) == null)
			{
				Logger.LogError(PmsklqZYysERcKCAPQDotLfXJFHv(key, typeof(TValue)), requiredThreadSafety: true);
				return false;
			}
			setValue.SetValue(value);
			return true;
		}

		private void qAWgaXgLeOGqRRfZhgFeCgkuOHVC()
		{
			if (ljcRKLlMKsZaueVrRjGZyImMcUwEA)
			{
				throw new Exception("The collection is read-only.");
			}
		}

		private static void CVcPfMnfWgicoQBKttfPShKGIiFBA(TKey P_0, Type P_1)
		{
			throw new Exception(PmsklqZYysERcKCAPQDotLfXJFHv(P_0, P_1));
		}

		private static string PmsklqZYysERcKCAPQDotLfXJFHv(TKey P_0, Type P_1)
		{
			string[] obj = new string[5] { "Value with key ", null, null, null, null };
			TKey val = P_0;
			obj[1] = val?.ToString();
			obj[2] = " of type ";
			obj[3] = P_1?.ToString();
			obj[4] = " not found.";
			return string.Concat(obj);
		}

		private void DdBAxOXvrnoLjxqOKQWLxYLemexK(TKey P_0, object P_1)
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			itFrICAJCfoFXDfPQSEyJfSLqhaA.Add(P_0, P_1);
		}

		void IDictionary<TKey, object>.Add(TKey P_0, object P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in DdBAxOXvrnoLjxqOKQWLxYLemexK
			this.DdBAxOXvrnoLjxqOKQWLxYLemexK(P_0, P_1);
		}

		private bool sTPyJVUFksKWPAMhMJErIjQpGOgD(TKey P_0)
		{
			return ContainsKey(P_0);
		}

		bool IDictionary<TKey, object>.ContainsKey(TKey P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in sTPyJVUFksKWPAMhMJErIjQpGOgD
			return this.sTPyJVUFksKWPAMhMJErIjQpGOgD(P_0);
		}

		private bool fbhsDyyNZSdcnnpidgHrKSuRWMzQ(TKey P_0)
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.Remove(P_0);
		}

		bool IDictionary<TKey, object>.Remove(TKey P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fbhsDyyNZSdcnnpidgHrKSuRWMzQ
			return this.fbhsDyyNZSdcnnpidgHrKSuRWMzQ(P_0);
		}

		private bool vKhQAWGGnoMRunNTDLmihozmyuwO(TKey P_0, out object P_1)
		{
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.TryGetValue(P_0, out P_1);
		}

		bool IDictionary<TKey, object>.TryGetValue(TKey P_0, out object P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in vKhQAWGGnoMRunNTDLmihozmyuwO
			return this.vKhQAWGGnoMRunNTDLmihozmyuwO(P_0, out P_1);
		}

		private void zMMHatsrbmahwHuEsgETmPkZcALk(KeyValuePair<TKey, object> P_0)
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			((ICollection<KeyValuePair<TKey, object>>)itFrICAJCfoFXDfPQSEyJfSLqhaA).Add(P_0);
		}

		void ICollection<KeyValuePair<TKey, object>>.Add(KeyValuePair<TKey, object> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zMMHatsrbmahwHuEsgETmPkZcALk
			this.zMMHatsrbmahwHuEsgETmPkZcALk(P_0);
		}

		private void NdEdtMDQGrbeXtpwHPmfsBXjGbbeA()
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			((ICollection<KeyValuePair<TKey, object>>)itFrICAJCfoFXDfPQSEyJfSLqhaA).Clear();
		}

		void ICollection<KeyValuePair<TKey, object>>.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in NdEdtMDQGrbeXtpwHPmfsBXjGbbeA
			this.NdEdtMDQGrbeXtpwHPmfsBXjGbbeA();
		}

		private bool igJqGawbQqqfAfxrhMusifNwZiMl(KeyValuePair<TKey, object> P_0)
		{
			return ((ICollection<KeyValuePair<TKey, object>>)itFrICAJCfoFXDfPQSEyJfSLqhaA).Contains(P_0);
		}

		bool ICollection<KeyValuePair<TKey, object>>.Contains(KeyValuePair<TKey, object> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in igJqGawbQqqfAfxrhMusifNwZiMl
			return this.igJqGawbQqqfAfxrhMusifNwZiMl(P_0);
		}

		private void RAPkgpTKHNXqwiQjRIqEMLxrHBi(KeyValuePair<TKey, object>[] P_0, int P_1)
		{
			((ICollection<KeyValuePair<TKey, object>>)itFrICAJCfoFXDfPQSEyJfSLqhaA).CopyTo(P_0, P_1);
		}

		void ICollection<KeyValuePair<TKey, object>>.CopyTo(KeyValuePair<TKey, object>[] P_0, int P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in RAPkgpTKHNXqwiQjRIqEMLxrHBi
			this.RAPkgpTKHNXqwiQjRIqEMLxrHBi(P_0, P_1);
		}

		private bool IDrBZARfjpGfMKUEkDpFilEbMOyRA(KeyValuePair<TKey, object> P_0)
		{
			qAWgaXgLeOGqRRfZhgFeCgkuOHVC();
			return ((ICollection<KeyValuePair<TKey, object>>)itFrICAJCfoFXDfPQSEyJfSLqhaA).Remove(P_0);
		}

		bool ICollection<KeyValuePair<TKey, object>>.Remove(KeyValuePair<TKey, object> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IDrBZARfjpGfMKUEkDpFilEbMOyRA
			return this.IDrBZARfjpGfMKUEkDpFilEbMOyRA(P_0);
		}

		private IEnumerator<KeyValuePair<TKey, object>> RenkvYLHItGBxJDpgDFyIgIFJjCJc()
		{
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.GetEnumerator();
		}

		IEnumerator<KeyValuePair<TKey, object>> IEnumerable<KeyValuePair<TKey, object>>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in RenkvYLHItGBxJDpgDFyIgIFJjCJc
			return this.RenkvYLHItGBxJDpgDFyIgIFJjCJc();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return itFrICAJCfoFXDfPQSEyJfSLqhaA.GetEnumerator();
		}
	}
}
