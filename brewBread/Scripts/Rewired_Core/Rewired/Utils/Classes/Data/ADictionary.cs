using System;
using System.Collections;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ADictionary<TKey, TValue> : IEnumerable, IDictionary, ICollection, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		internal struct Entry
		{
			public int hashCode;

			public int next;

			public TKey key;

			public TValue value;
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Enumerator : IDisposable, IEnumerator, IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
		{
			private ADictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

			private int IXQaoUCUZGEGQAwPFohIYgSPNjoiB;

			private int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

			private KeyValuePair<TKey, TValue> SgJVPHrjFyHgCSEpxtgUijWhNNop;

			private int hVLAXWNvglyqqvtcCiHFclAncMWO;

			internal const int DictEntry = 1;

			internal const int KeyValuePair = 2;

			public KeyValuePair<TKey, TValue> Current => SgJVPHrjFyHgCSEpxtgUijWhNNop;

			object IEnumerator.Current
			{
				get
				{
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
					{
						throw new Exception();
					}
					if (hVLAXWNvglyqqvtcCiHFclAncMWO == 1)
					{
						return new DictionaryEntry(SgJVPHrjFyHgCSEpxtgUijWhNNop.Key, SgJVPHrjFyHgCSEpxtgUijWhNNop.Value);
					}
					return new KeyValuePair<TKey, TValue>(SgJVPHrjFyHgCSEpxtgUijWhNNop.Key, SgJVPHrjFyHgCSEpxtgUijWhNNop.Value);
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
					{
						throw new Exception();
					}
					return new DictionaryEntry(SgJVPHrjFyHgCSEpxtgUijWhNNop.Key, SgJVPHrjFyHgCSEpxtgUijWhNNop.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
					{
						throw new Exception();
					}
					return SgJVPHrjFyHgCSEpxtgUijWhNNop.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
					{
						throw new Exception();
					}
					return SgJVPHrjFyHgCSEpxtgUijWhNNop.Value;
				}
			}

			internal Enumerator(ADictionary<TKey, TValue> P_0, int P_1)
			{
				gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
				IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.ZMmXJIUpRkkINbjuQpRQDgAgOgdk;
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
				hVLAXWNvglyqqvtcCiHFclAncMWO = P_1;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
			}

			public bool MoveNext()
			{
				if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
				{
					throw new Exception();
				}
				while ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn._count)
				{
					if (gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].hashCode >= 0)
					{
						SgJVPHrjFyHgCSEpxtgUijWhNNop = new KeyValuePair<TKey, TValue>(gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].key, gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].value);
						PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
						return true;
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
				}
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			public void Dispose()
			{
			}

			void IEnumerator.Reset()
			{
				if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
				{
					throw new Exception();
				}
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public sealed class KeyCollection : IEnumerable, IEnumerable<TKey>, ICollection, ICollection<TKey>
		{
			[Serializable]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			[CustomObfuscation(rename = false)]
			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<TKey>
			{
				private ADictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

				private int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

				private int IXQaoUCUZGEGQAwPFohIYgSPNjoiB;

				private TKey szsPnNMRosmkGGKYRlfkUtAelqA;

				public TKey Current => szsPnNMRosmkGGKYRlfkUtAelqA;

				object IEnumerator.Current
				{
					get
					{
						if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
						{
							throw new Exception();
						}
						return szsPnNMRosmkGGKYRlfkUtAelqA;
					}
				}

				internal Enumerator(ADictionary<TKey, TValue> P_0)
				{
					gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
					IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.ZMmXJIUpRkkINbjuQpRQDgAgOgdk;
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
					{
						throw new Exception();
					}
					while ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn._count)
					{
						if (gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].hashCode >= 0)
						{
							szsPnNMRosmkGGKYRlfkUtAelqA = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].key;
							PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
							return true;
						}
						PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
					{
						throw new Exception();
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
				}
			}

			private ADictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

			public int Count => gtOwGaSStjjjqPkwkkQpbgQQomCn.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)gtOwGaSStjjjqPkwkkQpbgQQomCn).SyncRoot;

			public KeyCollection(ADictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || index > array.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (array.Length - index < gtOwGaSStjjjqPkwkkQpbgQQomCn.Count)
				{
					throw new Exception();
				}
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn._count;
				Entry[] entries = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			void ICollection<TKey>.Add(TKey P_0)
			{
				throw new Exception();
			}

			void ICollection<TKey>.Clear()
			{
				throw new Exception();
			}

			bool ICollection<TKey>.Contains(TKey P_0)
			{
				return gtOwGaSStjjjqPkwkkQpbgQQomCn.ContainsKey(P_0);
			}

			bool ICollection<TKey>.Remove(TKey P_0)
			{
				throw new Exception();
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new Exception();
				}
				if (array.GetLowerBound(0) != 0)
				{
					throw new Exception();
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < gtOwGaSStjjjqPkwkkQpbgQQomCn.Count)
				{
					throw new Exception();
				}
				if (array is TKey[] array2)
				{
					CopyTo(array2, index);
					return;
				}
				if (!(array is object[] array3))
				{
					throw new Exception();
				}
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn._count;
				Entry[] entries = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		[Serializable]
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		public sealed class ValueCollection : IEnumerable, ICollection, ICollection<TValue>, IEnumerable<TValue>
		{
			[Serializable]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			[CustomObfuscation(rename = false)]
			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<TValue>
			{
				private ADictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

				private int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

				private int IXQaoUCUZGEGQAwPFohIYgSPNjoiB;

				private TValue RfseuwiVRZUJKPFxDqhpYqNpoDuV;

				public TValue Current => RfseuwiVRZUJKPFxDqhpYqNpoDuV;

				object IEnumerator.Current
				{
					get
					{
						if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1)
						{
							throw new Exception();
						}
						return RfseuwiVRZUJKPFxDqhpYqNpoDuV;
					}
				}

				internal Enumerator(ADictionary<TKey, TValue> P_0)
				{
					gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
					IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.ZMmXJIUpRkkINbjuQpRQDgAgOgdk;
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
					{
						throw new Exception();
					}
					while ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn._count)
					{
						if (gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].hashCode >= 0)
						{
							RfseuwiVRZUJKPFxDqhpYqNpoDuV = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].value;
							PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
							return true;
						}
						PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn._count + 1;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
					{
						throw new Exception();
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
				}
			}

			private ADictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

			public int Count => gtOwGaSStjjjqPkwkkQpbgQQomCn.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)gtOwGaSStjjjqPkwkkQpbgQQomCn).SyncRoot;

			public ValueCollection(ADictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < gtOwGaSStjjjqPkwkkQpbgQQomCn.Count)
				{
					throw new Exception();
				}
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn._count;
				Entry[] entries = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			void ICollection<TValue>.Add(TValue P_0)
			{
				throw new Exception();
			}

			bool ICollection<TValue>.Remove(TValue P_0)
			{
				throw new Exception();
			}

			void ICollection<TValue>.Clear()
			{
				throw new Exception();
			}

			bool ICollection<TValue>.Contains(TValue P_0)
			{
				return gtOwGaSStjjjqPkwkkQpbgQQomCn.ContainsValue(P_0);
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(gtOwGaSStjjjqPkwkkQpbgQQomCn);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new Exception();
				}
				if (array.GetLowerBound(0) != 0)
				{
					throw new Exception();
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < gtOwGaSStjjjqPkwkkQpbgQQomCn.Count)
				{
					throw new Exception();
				}
				if (array is TValue[] array2)
				{
					CopyTo(array2, index);
					return;
				}
				if (!(array is object[] array3))
				{
					throw new Exception();
				}
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn._count;
				Entry[] entries = gtOwGaSStjjjqPkwkkQpbgQQomCn._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		private int[] JRAtgWgLrfDRuSJOiKQULiuRWxCl;

		internal Entry[] _entries;

		internal int _count;

		private int ZMmXJIUpRkkINbjuQpRQDgAgOgdk;

		private int JcBBAHIAaohONoRvXaksQMlIiKSz;

		private int oklhWJbpIyGuhHoQsdtnWmUIGVQx;

		private int gnsEgIUPDZGSRDOouaeyYVNFYxprA;

		private IEqualityComparer<TKey> toVgbxhYiVFNzBqoRpxaDNzBeHrF;

		private IEqualityComparer<TValue> BWAEIJVQdNILOtNzQeilDQcmiWJBb;

		private KeyCollection mRJsAJSfWTOusfaseyppRkuiypFP;

		private ValueCollection VePwhFxSBkhXLQKWMwBJJwSoAsVl;

		private readonly object YmPJVmEIVpGMAdftFocwGnzawvaZb = new object();

		private static readonly bool xZKbNABvFicyafZagjtMHeawbvWPb = ReflectionTools.IsValueType(typeof(TKey));

		private static readonly bool BPkiiTlArwbipfwUHrbxCyzAuCsy = ReflectionTools.IsValueType(typeof(TValue));

		private const string FEHSYqJUHgdMQREzOqNogNTrQMlb = "Version";

		private const string JtEctPpMbgGoSaONyyZNUGsVexXm = "HashSize";

		private const string NiHfTUhZZPBSvxDirOSSUbNqpkwFb = "KeyValuePairs";

		private const string HlUAtFSnxTEPvqADfXeSFTCTKfnK = "Comparer";

		public int Count => _count - gnsEgIUPDZGSRDOouaeyYVNFYxprA;

		public int TotalCount => _count;

		public KeyCollection Keys
		{
			get
			{
				if (mRJsAJSfWTOusfaseyppRkuiypFP == null)
				{
					mRJsAJSfWTOusfaseyppRkuiypFP = new KeyCollection(this);
				}
				return mRJsAJSfWTOusfaseyppRkuiypFP;
			}
		}

		public ValueCollection Values
		{
			get
			{
				if (VePwhFxSBkhXLQKWMwBJJwSoAsVl == null)
				{
					VePwhFxSBkhXLQKWMwBJJwSoAsVl = new ValueCollection(this);
				}
				return VePwhFxSBkhXLQKWMwBJJwSoAsVl;
			}
		}

		public IEqualityComparer<TKey> KeyComparer
		{
			get
			{
				return toVgbxhYiVFNzBqoRpxaDNzBeHrF;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TKey>.Default;
				}
				toVgbxhYiVFNzBqoRpxaDNzBeHrF = value;
			}
		}

		public IEqualityComparer<TValue> ValueComparer
		{
			get
			{
				return BWAEIJVQdNILOtNzQeilDQcmiWJBb;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TValue>.Default;
				}
				BWAEIJVQdNILOtNzQeilDQcmiWJBb = value;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				int num = IndexOfKey(key);
				if (num < 0)
				{
					TKey val = key;
					throw new KeyNotFoundException("Key \"" + val?.ToString() + " does not exist.");
				}
				return _entries[num].value;
			}
			set
			{
				PcDQXUadsLAlVcLLblWJXXzcEHhCA(key, value, false);
			}
		}

		public int IndexOfFirst
		{
			get
			{
				for (int i = 0; i < _count; i++)
				{
					if (_entries[i].hashCode >= 0)
					{
						return i;
					}
				}
				return -1;
			}
		}

		public int IndexOfLast
		{
			get
			{
				for (int num = _count - 1; num >= 0; num--)
				{
					if (_entries[num].hashCode >= 0)
					{
						return num;
					}
				}
				return -1;
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (mRJsAJSfWTOusfaseyppRkuiypFP == null)
				{
					mRJsAJSfWTOusfaseyppRkuiypFP = new KeyCollection(this);
				}
				return mRJsAJSfWTOusfaseyppRkuiypFP;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if (VePwhFxSBkhXLQKWMwBJJwSoAsVl == null)
				{
					VePwhFxSBkhXLQKWMwBJJwSoAsVl = new ValueCollection(this);
				}
				return VePwhFxSBkhXLQKWMwBJJwSoAsVl;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot => YmPJVmEIVpGMAdftFocwGnzawvaZb;

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => Keys;

		ICollection IDictionary.Values => Values;

		object IDictionary.this[object key]
		{
			get
			{
				if (kiujzptUwmTvUnXAMFvmoCuUpoDe(key))
				{
					int num = IndexOfKey((TKey)key);
					if (num >= 0)
					{
						return _entries[num].value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				iGUAMOJZDehgdBroHNKeJJmGumDBA<TValue>(value, "value");
				try
				{
					TKey key2 = (TKey)key;
					try
					{
						this[key2] = (TValue)value;
					}
					catch (InvalidCastException)
					{
						throw new Exception();
					}
				}
				catch (InvalidCastException)
				{
					throw new Exception();
				}
			}
		}

		public ADictionary()
			: this(0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IEqualityComparer<TKey> P_0)
			: this(0, P_0, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IEqualityComparer<TKey> P_0, IEqualityComparer<TValue> P_1)
			: this(0, P_0, P_1)
		{
		}

		public ADictionary(int P_0)
			: this(P_0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(int P_0, IEqualityComparer<TKey> P_1)
			: this(P_0, P_1, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(int P_0, IEqualityComparer<TKey> P_1, IEqualityComparer<TValue> P_2)
		{
			if (P_0 < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			if (P_0 > 0)
			{
				zQQfvDZMmpVqPPLYlLuSJXXpwJcI(P_0);
			}
			toVgbxhYiVFNzBqoRpxaDNzBeHrF = P_1 ?? EqualityComparerNoAlloc<TKey>.Default;
			BWAEIJVQdNILOtNzQeilDQcmiWJBb = P_2 ?? EqualityComparerNoAlloc<TValue>.Default;
		}

		public ADictionary(IDictionary<TKey, TValue> P_0)
			: this(P_0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IDictionary<TKey, TValue> P_0, IEqualityComparer<TKey> P_1)
			: this(P_0, P_1, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IDictionary<TKey, TValue> P_0, IEqualityComparer<TKey> P_1, IEqualityComparer<TValue> P_2)
			: this(P_0?.Count ?? 0, P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			foreach (KeyValuePair<TKey, TValue> item in P_0)
			{
				Add(item.Key, item.Value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			PcDQXUadsLAlVcLLblWJXXzcEHhCA(key, value, true);
		}

		public void Clear()
		{
			if (_count > 0)
			{
				for (int i = 0; i < JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length; i++)
				{
					JRAtgWgLrfDRuSJOiKQULiuRWxCl[i] = -1;
				}
				Array.Clear(_entries, 0, _count);
				oklhWJbpIyGuhHoQsdtnWmUIGVQx = -1;
				_count = 0;
				gnsEgIUPDZGSRDOouaeyYVNFYxprA = 0;
				ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
				JcBBAHIAaohONoRvXaksQMlIiKSz++;
			}
		}

		public bool ContainsKey(TKey key)
		{
			return IndexOfKey(key) >= 0;
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		public bool Remove(TKey key)
		{
			if (!xZKbNABvFicyafZagjtMHeawbvWPb && key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (JRAtgWgLrfDRuSJOiKQULiuRWxCl != null)
			{
				int num = toVgbxhYiVFNzBqoRpxaDNzBeHrF.GetHashCode(key) & 0x7FFFFFFF;
				int num2 = num % JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length;
				int num3 = -1;
				for (int num4 = JRAtgWgLrfDRuSJOiKQULiuRWxCl[num2]; num4 >= 0; num4 = _entries[num4].next)
				{
					if (_entries[num4].hashCode == num && toVgbxhYiVFNzBqoRpxaDNzBeHrF.Equals(_entries[num4].key, key))
					{
						if (num3 < 0)
						{
							JRAtgWgLrfDRuSJOiKQULiuRWxCl[num2] = _entries[num4].next;
						}
						else
						{
							_entries[num3].next = _entries[num4].next;
						}
						_entries[num4].hashCode = -1;
						_entries[num4].next = oklhWJbpIyGuhHoQsdtnWmUIGVQx;
						_entries[num4].key = default(TKey);
						_entries[num4].value = default(TValue);
						oklhWJbpIyGuhHoQsdtnWmUIGVQx = num4;
						gnsEgIUPDZGSRDOouaeyYVNFYxprA++;
						ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
						return true;
					}
					num3 = num4;
				}
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = IndexOfKey(key);
			if (num >= 0)
			{
				value = _entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		public TValue GetValueSafe(TKey key)
		{
			int num = IndexOfKey(key);
			if (num >= 0)
			{
				return _entries[num].value;
			}
			return default(TValue);
		}

		public int IndexOfKey(TKey key)
		{
			if (!xZKbNABvFicyafZagjtMHeawbvWPb && key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (JRAtgWgLrfDRuSJOiKQULiuRWxCl != null)
			{
				int num = toVgbxhYiVFNzBqoRpxaDNzBeHrF.GetHashCode(key) & 0x7FFFFFFF;
				for (int num2 = JRAtgWgLrfDRuSJOiKQULiuRWxCl[num % JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length]; num2 >= 0; num2 = _entries[num2].next)
				{
					if (_entries[num2].hashCode == num && toVgbxhYiVFNzBqoRpxaDNzBeHrF.Equals(_entries[num2].key, key))
					{
						return num2;
					}
				}
			}
			return -1;
		}

		public int IndexOfValue(TValue value)
		{
			Entry[] entries = _entries;
			if (!BPkiiTlArwbipfwUHrbxCyzAuCsy && value == null)
			{
				for (int i = 0; i < _count; i++)
				{
					if (entries[i].hashCode >= 0 && entries[i].value == null)
					{
						return i;
					}
				}
			}
			else
			{
				IEqualityComparer<TValue> bWAEIJVQdNILOtNzQeilDQcmiWJBb = BWAEIJVQdNILOtNzQeilDQcmiWJBb;
				for (int j = 0; j < _count; j++)
				{
					if (entries[j].hashCode >= 0 && bWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(entries[j].value, value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		public bool IsValidAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			return _entries[index].hashCode >= 0;
		}

		public TKey GetKeyAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return _entries[index].key;
		}

		public TValue GetValueAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return _entries[index].value;
		}

		public KeyValuePair<TKey, TValue> GetEntryAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
		}

		public bool TryGetKeyAt(int index, out TKey key)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				key = default(TKey);
				return false;
			}
			key = _entries[index].key;
			return true;
		}

		public bool TryGetValueAt(int index, out TValue value)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				value = default(TValue);
				return false;
			}
			value = _entries[index].value;
			return true;
		}

		public bool TryGetEntryAt(int index, out KeyValuePair<TKey, TValue> entry)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
			return true;
		}

		public bool GetNextIndex(ref int index)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					return true;
				}
				index++;
			}
			return false;
		}

		public int GetNextIndex(int index)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				return -1;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					return index;
				}
				index++;
			}
			return -1;
		}

		public bool GetNextKey(ref int index, out TKey key)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				key = default(TKey);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					key = _entries[index].key;
					return true;
				}
				index++;
			}
			key = default(TKey);
			return false;
		}

		public bool GetNextValue(ref int index, out TValue value)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				value = default(TValue);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					value = _entries[index].value;
					return true;
				}
				index++;
			}
			value = default(TValue);
			return false;
		}

		public bool GetNextEntry(ref int index, out KeyValuePair<TKey, TValue> entry)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
					return true;
				}
				index++;
			}
			entry = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		public bool GetPreviousIndex(ref int index)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					return true;
				}
				index--;
			}
			return false;
		}

		public int GetPreviousIndex(int index)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				return -1;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					return index;
				}
				index--;
			}
			return -1;
		}

		public bool GetPreviousKey(ref int index, out TKey key)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				key = default(TKey);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					key = _entries[index].key;
					return true;
				}
				index--;
			}
			key = default(TKey);
			return false;
		}

		public bool GetPreviousValue(ref int index, out TValue value)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				value = default(TValue);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					value = _entries[index].value;
					return true;
				}
				index--;
			}
			value = default(TValue);
			return false;
		}

		public bool GetPreviousEntry(ref int index, out KeyValuePair<TKey, TValue> entry)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
					return true;
				}
				index--;
			}
			entry = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		public bool RemoveAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				return false;
			}
			Remove(_entries[index].key);
			return true;
		}

		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Length - index < Count)
			{
				throw new Exception();
			}
			int count = _count;
			Entry[] entries = _entries;
			for (int i = 0; i < count; i++)
			{
				if (entries[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
				}
			}
		}

		private void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(int P_0)
		{
			int num = zypbFehfyfztYNVvjmYRNOvWsJrn.UvQNGTcTMwWeHdorpnIinpDcxJXN(P_0);
			JRAtgWgLrfDRuSJOiKQULiuRWxCl = new int[num];
			for (int i = 0; i < JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length; i++)
			{
				JRAtgWgLrfDRuSJOiKQULiuRWxCl[i] = -1;
			}
			_entries = new Entry[num];
			oklhWJbpIyGuhHoQsdtnWmUIGVQx = -1;
		}

		private void PcDQXUadsLAlVcLLblWJXXzcEHhCA(TKey P_0, TValue P_1, bool P_2)
		{
			if (!xZKbNABvFicyafZagjtMHeawbvWPb && P_0 == null)
			{
				throw new ArgumentNullException("key");
			}
			if (JRAtgWgLrfDRuSJOiKQULiuRWxCl == null)
			{
				zQQfvDZMmpVqPPLYlLuSJXXpwJcI(0);
			}
			int num = toVgbxhYiVFNzBqoRpxaDNzBeHrF.GetHashCode(P_0) & 0x7FFFFFFF;
			int num2 = num % JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length;
			for (int num3 = JRAtgWgLrfDRuSJOiKQULiuRWxCl[num2]; num3 >= 0; num3 = _entries[num3].next)
			{
				if (_entries[num3].hashCode == num && toVgbxhYiVFNzBqoRpxaDNzBeHrF.Equals(_entries[num3].key, P_0))
				{
					if (P_2)
					{
						throw new ArgumentException("An element with the same key already exists in the dictionary.");
					}
					_entries[num3].value = P_1;
					ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
					return;
				}
			}
			int count;
			if (gnsEgIUPDZGSRDOouaeyYVNFYxprA > 0)
			{
				count = oklhWJbpIyGuhHoQsdtnWmUIGVQx;
				oklhWJbpIyGuhHoQsdtnWmUIGVQx = _entries[count].next;
				gnsEgIUPDZGSRDOouaeyYVNFYxprA--;
			}
			else
			{
				if (_count == _entries.Length)
				{
					tDOYzectlTkeiSukMnmpKmfTjmIr();
					num2 = num % JRAtgWgLrfDRuSJOiKQULiuRWxCl.Length;
				}
				count = _count;
				_count++;
			}
			_entries[count].hashCode = num;
			_entries[count].next = JRAtgWgLrfDRuSJOiKQULiuRWxCl[num2];
			_entries[count].key = P_0;
			_entries[count].value = P_1;
			JRAtgWgLrfDRuSJOiKQULiuRWxCl[num2] = count;
			ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
			JcBBAHIAaohONoRvXaksQMlIiKSz++;
		}

		private void tDOYzectlTkeiSukMnmpKmfTjmIr()
		{
			tDOYzectlTkeiSukMnmpKmfTjmIr(zypbFehfyfztYNVvjmYRNOvWsJrn.mVjFYUzdheHVgaWkregCbtxvzWbY(_count), false);
		}

		private void tDOYzectlTkeiSukMnmpKmfTjmIr(int P_0, bool P_1)
		{
			int[] array = new int[P_0];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			Entry[] array2 = new Entry[P_0];
			Array.Copy(_entries, 0, array2, 0, _count);
			if (P_1)
			{
				for (int j = 0; j < _count; j++)
				{
					if (array2[j].hashCode != -1)
					{
						array2[j].hashCode = toVgbxhYiVFNzBqoRpxaDNzBeHrF.GetHashCode(array2[j].key) & 0x7FFFFFFF;
					}
				}
			}
			for (int k = 0; k < _count; k++)
			{
				if (array2[k].hashCode >= 0)
				{
					int num = array2[k].hashCode % P_0;
					array2[k].next = array[num];
					array[num] = k;
				}
			}
			JRAtgWgLrfDRuSJOiKQULiuRWxCl = array;
			_entries = array2;
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> P_0)
		{
			Add(P_0.Key, P_0.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num >= 0 && BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(_entries[num].value, P_0.Value))
			{
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num >= 0 && BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(_entries[num].value, P_0.Value))
			{
				Remove(P_0.Key);
				return true;
			}
			return false;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			CopyTo(P_0, P_1);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new Exception();
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new Exception();
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Length - index < Count)
			{
				throw new Exception();
			}
			if (array is KeyValuePair<TKey, TValue>[] array2)
			{
				CopyTo(array2, index);
				return;
			}
			if (array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				Entry[] entries = _entries;
				for (int i = 0; i < _count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
					}
				}
				return;
			}
			if (!(array is object[] array4))
			{
				throw new Exception();
			}
			try
			{
				int count = _count;
				Entry[] entries2 = _entries;
				for (int j = 0; j < count; j++)
				{
					if (entries2[j].hashCode >= 0)
					{
						array4[index++] = new KeyValuePair<TKey, TValue>(entries2[j].key, entries2[j].value);
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new Exception();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			iGUAMOJZDehgdBroHNKeJJmGumDBA<TValue>(value, "value");
			try
			{
				TKey key2 = (TKey)key;
				try
				{
					Add(key2, (TValue)value);
				}
				catch (InvalidCastException)
				{
					throw new Exception();
				}
			}
			catch (InvalidCastException)
			{
				throw new Exception();
			}
		}

		bool IDictionary.Contains(object key)
		{
			if (kiujzptUwmTvUnXAMFvmoCuUpoDe(key))
			{
				return ContainsKey((TKey)key);
			}
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		void IDictionary.Remove(object key)
		{
			if (kiujzptUwmTvUnXAMFvmoCuUpoDe(key))
			{
				Remove((TKey)key);
			}
		}

		private static bool kiujzptUwmTvUnXAMFvmoCuUpoDe(object P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("key");
			}
			return P_0 is TKey;
		}

		private static void iGUAMOJZDehgdBroHNKeJJmGumDBA<_0001>(object P_0, string P_1)
		{
			if (P_0 == null && default(_0001) != null)
			{
				throw new ArgumentNullException(P_1);
			}
		}
	}
}
