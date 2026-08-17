using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class IndexedDictionary<TKey, TValue> : IEnumerable, IDictionary, ICollection, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, Rewired.Utils.Interfaces.IReadOnlyList<TValue>, IReadOnlyList
	{
		private struct nnXpQNToQOcOXCGmLUrlooHLNULg
		{
			public TKey qwkeeArhjFVfINdRhYgfKXVRHPQW;

			public TValue yYOUbwIbBcyPAQvjeAEXVsjzLAln;

			public nnXpQNToQOcOXCGmLUrlooHLNULg(TKey P_0, TValue P_1)
			{
				qwkeeArhjFVfINdRhYgfKXVRHPQW = P_0;
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = P_1;
			}

			public KeyValuePair<TKey, TValue> uXAyDTEMHJilashiZEIjGfZgTJyo()
			{
				return new KeyValuePair<TKey, TValue>(qwkeeArhjFVfINdRhYgfKXVRHPQW, yYOUbwIbBcyPAQvjeAEXVsjzLAln);
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Enumerator : IDisposable, IEnumerator, IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
		{
			private IndexedDictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

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
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
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
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
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
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
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
					if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
					{
						throw new Exception();
					}
					return SgJVPHrjFyHgCSEpxtgUijWhNNop.Value;
				}
			}

			internal Enumerator(IndexedDictionary<TKey, TValue> P_0, int P_1)
			{
				gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
				IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.jaEouEThGebUjciilFRztgFLhYrT.Version;
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
				hVLAXWNvglyqqvtcCiHFclAncMWO = P_1;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
			}

			public bool MoveNext()
			{
				if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
				{
					throw new Exception();
				}
				if ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count)
				{
					SgJVPHrjFyHgCSEpxtgUijWhNNop = new KeyValuePair<TKey, TValue>(gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].qwkeeArhjFVfINdRhYgfKXVRHPQW, gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].yYOUbwIbBcyPAQvjeAEXVsjzLAln);
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
					return true;
				}
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			public void Dispose()
			{
			}

			void IEnumerator.Reset()
			{
				if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
				{
					throw new Exception();
				}
				PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
				SgJVPHrjFyHgCSEpxtgUijWhNNop = default(KeyValuePair<TKey, TValue>);
			}
		}

		[Serializable]
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		public sealed class KeyCollection : IEnumerable, IEnumerable<TKey>, ICollection, ICollection<TKey>
		{
			[Serializable]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			[CustomObfuscation(rename = false)]
			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<TKey>
			{
				private IndexedDictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

				private int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

				private int IXQaoUCUZGEGQAwPFohIYgSPNjoiB;

				private TKey szsPnNMRosmkGGKYRlfkUtAelqA;

				public TKey Current => szsPnNMRosmkGGKYRlfkUtAelqA;

				object IEnumerator.Current
				{
					get
					{
						if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
						{
							throw new Exception();
						}
						return szsPnNMRosmkGGKYRlfkUtAelqA;
					}
				}

				internal Enumerator(IndexedDictionary<TKey, TValue> P_0)
				{
					gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
					IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.jaEouEThGebUjciilFRztgFLhYrT.Version;
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
					{
						throw new Exception();
					}
					if ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count)
					{
						szsPnNMRosmkGGKYRlfkUtAelqA = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].qwkeeArhjFVfINdRhYgfKXVRHPQW;
						PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
						return true;
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
					{
						throw new Exception();
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					szsPnNMRosmkGGKYRlfkUtAelqA = default(TKey);
				}
			}

			private IndexedDictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

			public int Count => gtOwGaSStjjjqPkwkkQpbgQQomCn.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)gtOwGaSStjjjqPkwkkQpbgQQomCn).SyncRoot;

			public KeyCollection(IndexedDictionary<TKey, TValue> P_0)
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
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count;
				nnXpQNToQOcOXCGmLUrlooHLNULg[] items = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items;
				for (int i = 0; i < count; i++)
				{
					array[index++] = items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW;
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
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count;
				nnXpQNToQOcOXCGmLUrlooHLNULg[] items = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items;
				try
				{
					for (int i = 0; i < count; i++)
					{
						array3[index++] = items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW;
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public sealed class ValueCollection : IEnumerable, ICollection, ICollection<TValue>, IEnumerable<TValue>
		{
			[Serializable]
			[CustomObfuscation(rename = false)]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<TValue>
			{
				private IndexedDictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

				private int PqXTkiKXdxRLgXpucsKUqWEJhHUQ;

				private int IXQaoUCUZGEGQAwPFohIYgSPNjoiB;

				private TValue RfseuwiVRZUJKPFxDqhpYqNpoDuV;

				public TValue Current => RfseuwiVRZUJKPFxDqhpYqNpoDuV;

				object IEnumerator.Current
				{
					get
					{
						if (PqXTkiKXdxRLgXpucsKUqWEJhHUQ == 0 || PqXTkiKXdxRLgXpucsKUqWEJhHUQ == gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1)
						{
							throw new Exception();
						}
						return RfseuwiVRZUJKPFxDqhpYqNpoDuV;
					}
				}

				internal Enumerator(IndexedDictionary<TKey, TValue> P_0)
				{
					gtOwGaSStjjjqPkwkkQpbgQQomCn = P_0;
					IXQaoUCUZGEGQAwPFohIYgSPNjoiB = P_0.jaEouEThGebUjciilFRztgFLhYrT.Version;
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
					{
						throw new Exception();
					}
					if ((uint)PqXTkiKXdxRLgXpucsKUqWEJhHUQ < (uint)gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count)
					{
						RfseuwiVRZUJKPFxDqhpYqNpoDuV = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items[PqXTkiKXdxRLgXpucsKUqWEJhHUQ].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
						PqXTkiKXdxRLgXpucsKUqWEJhHUQ++;
						return true;
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count + 1;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (IXQaoUCUZGEGQAwPFohIYgSPNjoiB != gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT.Version)
					{
						throw new Exception();
					}
					PqXTkiKXdxRLgXpucsKUqWEJhHUQ = 0;
					RfseuwiVRZUJKPFxDqhpYqNpoDuV = default(TValue);
				}
			}

			private IndexedDictionary<TKey, TValue> gtOwGaSStjjjqPkwkkQpbgQQomCn;

			public int Count => gtOwGaSStjjjqPkwkkQpbgQQomCn.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)gtOwGaSStjjjqPkwkkQpbgQQomCn).SyncRoot;

			public ValueCollection(IndexedDictionary<TKey, TValue> P_0)
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
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count;
				nnXpQNToQOcOXCGmLUrlooHLNULg[] items = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items;
				for (int i = 0; i < count; i++)
				{
					array[index++] = items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
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
				int count = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._count;
				nnXpQNToQOcOXCGmLUrlooHLNULg[] items = gtOwGaSStjjjqPkwkkQpbgQQomCn.jaEouEThGebUjciilFRztgFLhYrT._items;
				try
				{
					for (int i = 0; i < count; i++)
					{
						array3[index++] = items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		private static readonly bool xZKbNABvFicyafZagjtMHeawbvWPb = ReflectionTools.IsValueType(typeof(TKey));

		private static readonly bool BPkiiTlArwbipfwUHrbxCyzAuCsy = ReflectionTools.IsValueType(typeof(TValue));

		private IEqualityComparer<TKey> toVgbxhYiVFNzBqoRpxaDNzBeHrF = EqualityComparerNoAlloc<TKey>.Default;

		private IEqualityComparer<TValue> BWAEIJVQdNILOtNzQeilDQcmiWJBb = EqualityComparerNoAlloc<TValue>.Default;

		private readonly AList<nnXpQNToQOcOXCGmLUrlooHLNULg> jaEouEThGebUjciilFRztgFLhYrT;

		private readonly ADictionary<TKey, int> TQQbkBaVnxdAiuxUzscJmhHnspdn;

		private bool kqntpHEZaAOJWNtUuQwFcOEYvlhX;

		public int Count => jaEouEThGebUjciilFRztgFLhYrT._count;

		public bool ContainsDuplicateKeys
		{
			get
			{
				if (!kqntpHEZaAOJWNtUuQwFcOEYvlhX)
				{
					return false;
				}
				return TQQbkBaVnxdAiuxUzscJmhHnspdn._count < jaEouEThGebUjciilFRztgFLhYrT._count;
			}
		}

		public bool AllowDuplicateKeys
		{
			get
			{
				return kqntpHEZaAOJWNtUuQwFcOEYvlhX;
			}
			set
			{
				if (kqntpHEZaAOJWNtUuQwFcOEYvlhX != value)
				{
					kqntpHEZaAOJWNtUuQwFcOEYvlhX = value;
					if (!value && ContainsDuplicateKeys)
					{
						throw new Exception("The dictionary contains duplicate keys and cannot be changed unless the keys are removed.");
					}
				}
			}
		}

		public TValue this[int index]
		{
			get
			{
				if ((uint)index >= (uint)jaEouEThGebUjciilFRztgFLhYrT._count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return jaEouEThGebUjciilFRztgFLhYrT._items[index].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}
			set
			{
				if ((uint)index >= (uint)jaEouEThGebUjciilFRztgFLhYrT._count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				jaEouEThGebUjciilFRztgFLhYrT._items[index].yYOUbwIbBcyPAQvjeAEXVsjzLAln = value;
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

		public ICollection<TKey> Keys => new KeyCollection(this);

		public ICollection<TValue> Values => new ValueCollection(this);

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		TValue IDictionary<TKey, TValue>.this[TKey P_0]
		{
			get
			{
				int num = IndexOfKey(P_0);
				if (num < 0)
				{
					TKey val = P_0;
					throw new KeyNotFoundException("Key \"" + val?.ToString() + "\" does not exist.");
				}
				return jaEouEThGebUjciilFRztgFLhYrT._items[num].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			}
			set
			{
				SetValue(key, value2);
			}
		}

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => new KeyCollection(this);

		ICollection IDictionary.Values => new ValueCollection(this);

		object IDictionary.this[object key]
		{
			get
			{
				return ((IDictionary<TKey, TValue>)this)[(TKey)key];
			}
			set
			{
				((IDictionary<TKey, TValue>)this)[(TKey)key] = (TValue)value;
			}
		}

		bool ICollection.IsSynchronized => ((ICollection)jaEouEThGebUjciilFRztgFLhYrT).IsSynchronized;

		object ICollection.SyncRoot => ((ICollection)jaEouEThGebUjciilFRztgFLhYrT).SyncRoot;

		TValue Rewired.Utils.Interfaces.IReadOnlyList<TValue>.this[int P_0] => this[P_0];

		int IReadOnlyList.Count => Count;

		object IReadOnlyList.this[int P_0] => this[P_0];

		public IndexedDictionary()
			: this(0, false)
		{
		}

		public IndexedDictionary(int P_0)
			: this(P_0, false)
		{
		}

		public IndexedDictionary(bool P_0)
			: this(0, P_0)
		{
		}

		public IndexedDictionary(int P_0, bool P_1)
		{
			if (P_0 < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			kqntpHEZaAOJWNtUuQwFcOEYvlhX = P_1;
			jaEouEThGebUjciilFRztgFLhYrT = new AList<nnXpQNToQOcOXCGmLUrlooHLNULg>(P_0);
			TQQbkBaVnxdAiuxUzscJmhHnspdn = new ADictionary<TKey, int>(P_0);
		}

		public IndexedDictionary(IDictionary<TKey, TValue> P_0)
			: this(P_0, false)
		{
		}

		public IndexedDictionary(IDictionary<TKey, TValue> P_0, bool P_1)
			: this(0, P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (ReflectionTools.DoesTypeImplement(P_0.GetType(), typeof(IndexedDictionary<TKey, TValue>)))
			{
				IndexedDictionary<TKey, TValue> indexedDictionary = (IndexedDictionary<TKey, TValue>)P_0;
				for (int i = 0; i < indexedDictionary.jaEouEThGebUjciilFRztgFLhYrT._count; i++)
				{
					Add(indexedDictionary.jaEouEThGebUjciilFRztgFLhYrT._items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW, indexedDictionary.jaEouEThGebUjciilFRztgFLhYrT._items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln);
				}
				return;
			}
			foreach (KeyValuePair<TKey, TValue> item in P_0)
			{
				Add(item.Key, item.Value);
			}
		}

		public TValue GetValue(TKey key)
		{
			return jaEouEThGebUjciilFRztgFLhYrT._items[TQQbkBaVnxdAiuxUzscJmhHnspdn[key]].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (!TQQbkBaVnxdAiuxUzscJmhHnspdn.TryGetValue(key, out var value2))
			{
				value = default(TValue);
				return false;
			}
			value = jaEouEThGebUjciilFRztgFLhYrT._items[value2].yYOUbwIbBcyPAQvjeAEXVsjzLAln;
			return true;
		}

		public TKey GetKeyAt(int index)
		{
			if ((uint)index >= (uint)jaEouEThGebUjciilFRztgFLhYrT._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return jaEouEThGebUjciilFRztgFLhYrT[index].qwkeeArhjFVfINdRhYgfKXVRHPQW;
		}

		public KeyValuePair<TKey, TValue> GetEntry(TKey key)
		{
			return jaEouEThGebUjciilFRztgFLhYrT[TQQbkBaVnxdAiuxUzscJmhHnspdn[key]].uXAyDTEMHJilashiZEIjGfZgTJyo();
		}

		public KeyValuePair<TKey, TValue> GetEntryAt(int index)
		{
			if ((uint)index >= (uint)jaEouEThGebUjciilFRztgFLhYrT._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return jaEouEThGebUjciilFRztgFLhYrT[index].uXAyDTEMHJilashiZEIjGfZgTJyo();
		}

		public bool TryGetEntry(TKey key, out KeyValuePair<TKey, TValue> entry)
		{
			if (!TQQbkBaVnxdAiuxUzscJmhHnspdn.TryGetValue(key, out var value))
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			entry = jaEouEThGebUjciilFRztgFLhYrT[value].uXAyDTEMHJilashiZEIjGfZgTJyo();
			return true;
		}

		public void Add(TKey key, TValue value)
		{
			bool num = TQQbkBaVnxdAiuxUzscJmhHnspdn.ContainsKey(key);
			if (num && !kqntpHEZaAOJWNtUuQwFcOEYvlhX)
			{
				TKey val = key;
				throw new ArgumentException("Key \"" + val?.ToString() + "\" is already in use.");
			}
			int value2 = jaEouEThGebUjciilFRztgFLhYrT.Add(new nnXpQNToQOcOXCGmLUrlooHLNULg(key, value));
			if (num)
			{
				TQQbkBaVnxdAiuxUzscJmhHnspdn[key] = value2;
			}
			else
			{
				TQQbkBaVnxdAiuxUzscJmhHnspdn.Add(key, value2);
			}
		}

		public void SetValue(TKey key, TValue value)
		{
			if (TQQbkBaVnxdAiuxUzscJmhHnspdn.TryGetValue(key, out var value2))
			{
				jaEouEThGebUjciilFRztgFLhYrT._items[value2].yYOUbwIbBcyPAQvjeAEXVsjzLAln = value;
				TQQbkBaVnxdAiuxUzscJmhHnspdn[key] = value2;
			}
			else
			{
				Add(key, value);
			}
		}

		public bool Remove(TKey key)
		{
			TQQbkBaVnxdAiuxUzscJmhHnspdn.Remove(key);
			if (kqntpHEZaAOJWNtUuQwFcOEYvlhX)
			{
				bool result = false;
				for (int num = jaEouEThGebUjciilFRztgFLhYrT._count - 1; num >= 0; num--)
				{
					if (toVgbxhYiVFNzBqoRpxaDNzBeHrF.Equals(jaEouEThGebUjciilFRztgFLhYrT._items[num].qwkeeArhjFVfINdRhYgfKXVRHPQW, key))
					{
						jaEouEThGebUjciilFRztgFLhYrT.RemoveAt(num);
						result = true;
					}
				}
				return result;
			}
			int num2 = IndexOfKey(key);
			if (num2 < 0)
			{
				return false;
			}
			RemoveAt(num2);
			return true;
		}

		public void RemoveAt(int index)
		{
			if ((uint)index >= (uint)jaEouEThGebUjciilFRztgFLhYrT._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			TKey qwkeeArhjFVfINdRhYgfKXVRHPQW = jaEouEThGebUjciilFRztgFLhYrT._items[index].qwkeeArhjFVfINdRhYgfKXVRHPQW;
			if (index < jaEouEThGebUjciilFRztgFLhYrT._count - 1)
			{
				for (int i = index + 1; i < jaEouEThGebUjciilFRztgFLhYrT.Count; i++)
				{
					TQQbkBaVnxdAiuxUzscJmhHnspdn[jaEouEThGebUjciilFRztgFLhYrT._items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW] = i - 1;
				}
			}
			jaEouEThGebUjciilFRztgFLhYrT.RemoveAt(index);
			TQQbkBaVnxdAiuxUzscJmhHnspdn.Remove(qwkeeArhjFVfINdRhYgfKXVRHPQW);
		}

		public void RemoveValue(TValue value)
		{
			int num = IndexOfValue(value);
			if (num >= 0)
			{
				_ = ref jaEouEThGebUjciilFRztgFLhYrT._items[num];
				RemoveAt(num);
			}
		}

		public int RemoveAll(TValue value)
		{
			int num = 0;
			for (int num2 = jaEouEThGebUjciilFRztgFLhYrT._count - 1; num2 >= 0; num2--)
			{
				_ = ref jaEouEThGebUjciilFRztgFLhYrT._items[num2];
				if (BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(jaEouEThGebUjciilFRztgFLhYrT._items[num2].yYOUbwIbBcyPAQvjeAEXVsjzLAln, value))
				{
					RemoveAt(num2);
					num++;
				}
			}
			return num;
		}

		public int IndexOfKey(TKey key)
		{
			if (!xZKbNABvFicyafZagjtMHeawbvWPb && key == null)
			{
				throw new ArgumentNullException("key");
			}
			int count = jaEouEThGebUjciilFRztgFLhYrT._count;
			for (int i = 0; i < count; i++)
			{
				if (toVgbxhYiVFNzBqoRpxaDNzBeHrF.Equals(jaEouEThGebUjciilFRztgFLhYrT._items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW, key))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfValue(TValue value)
		{
			int count = jaEouEThGebUjciilFRztgFLhYrT._count;
			for (int i = 0; i < count; i++)
			{
				if (BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(jaEouEThGebUjciilFRztgFLhYrT._items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln, value))
				{
					return i;
				}
			}
			return -1;
		}

		public bool ContainsKey(TKey key)
		{
			return TQQbkBaVnxdAiuxUzscJmhHnspdn.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}

		public void Clear()
		{
			jaEouEThGebUjciilFRztgFLhYrT.Clear();
			TQQbkBaVnxdAiuxUzscJmhHnspdn.Clear();
		}

		public void TrimExcess()
		{
			jaEouEThGebUjciilFRztgFLhYrT.TrimExcess();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> P_0)
		{
			Add(P_0.Key, P_0.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num < 0)
			{
				return false;
			}
			nnXpQNToQOcOXCGmLUrlooHLNULg nnXpQNToQOcOXCGmLUrlooHLNULg2 = jaEouEThGebUjciilFRztgFLhYrT._items[num];
			return BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(P_0.Value, nnXpQNToQOcOXCGmLUrlooHLNULg2.yYOUbwIbBcyPAQvjeAEXVsjzLAln);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("array");
			}
			if (P_1 < 0 || P_1 > P_0.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (P_0.Length - P_1 < Count)
			{
				throw new Exception();
			}
			int count = jaEouEThGebUjciilFRztgFLhYrT._count;
			for (int i = 0; i < count; i++)
			{
				P_0[P_1++] = new KeyValuePair<TKey, TValue>(jaEouEThGebUjciilFRztgFLhYrT._items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW, jaEouEThGebUjciilFRztgFLhYrT._items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln);
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> P_0)
		{
			if (kqntpHEZaAOJWNtUuQwFcOEYvlhX)
			{
				bool result = false;
				for (int num = jaEouEThGebUjciilFRztgFLhYrT._count - 1; num >= 0; num--)
				{
					nnXpQNToQOcOXCGmLUrlooHLNULg nnXpQNToQOcOXCGmLUrlooHLNULg2 = jaEouEThGebUjciilFRztgFLhYrT._items[num];
					if (BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(P_0.Value, nnXpQNToQOcOXCGmLUrlooHLNULg2.yYOUbwIbBcyPAQvjeAEXVsjzLAln))
					{
						jaEouEThGebUjciilFRztgFLhYrT.RemoveAt(num);
						result = true;
					}
				}
				return result;
			}
			int num2 = IndexOfKey(P_0.Key);
			if (num2 < 0)
			{
				return false;
			}
			nnXpQNToQOcOXCGmLUrlooHLNULg nnXpQNToQOcOXCGmLUrlooHLNULg3 = jaEouEThGebUjciilFRztgFLhYrT._items[num2];
			if (!BWAEIJVQdNILOtNzQeilDQcmiWJBb.Equals(P_0.Value, nnXpQNToQOcOXCGmLUrlooHLNULg3.yYOUbwIbBcyPAQvjeAEXVsjzLAln))
			{
				return false;
			}
			RemoveAt(num2);
			return true;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		void IDictionary.Add(object key, object value)
		{
			Add((TKey)key, (TValue)value);
		}

		bool IDictionary.Contains(object key)
		{
			return ContainsKey((TKey)key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		void IDictionary.Remove(object key)
		{
			Remove((TKey)key);
		}

		void ICollection.CopyTo(Array array, int index)
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
			int count = jaEouEThGebUjciilFRztgFLhYrT._count;
			for (int i = 0; i < count; i++)
			{
				array.SetValue(new KeyValuePair<TKey, TValue>(jaEouEThGebUjciilFRztgFLhYrT._items[i].qwkeeArhjFVfINdRhYgfKXVRHPQW, jaEouEThGebUjciilFRztgFLhYrT._items[i].yYOUbwIbBcyPAQvjeAEXVsjzLAln), index++);
			}
		}

		int Rewired.Utils.Interfaces.IReadOnlyList<TValue>.IndexOf(TValue P_0)
		{
			return IndexOfValue(P_0);
		}

		bool Rewired.Utils.Interfaces.IReadOnlyList<TValue>.Contains(TValue P_0)
		{
			return ContainsValue(P_0);
		}

		private int YyDfuADycuFzrCfOdyNiuOLLCbxqA(object P_0)
		{
			return IndexOfValue((TValue)P_0);
		}

		int IReadOnlyList.IndexOf(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in YyDfuADycuFzrCfOdyNiuOLLCbxqA
			return this.YyDfuADycuFzrCfOdyNiuOLLCbxqA(P_0);
		}

		private bool FbhyADGmbwCEGiEtpEmdYoIEgidA(object P_0)
		{
			return ContainsValue((TValue)P_0);
		}

		bool IReadOnlyList.Contains(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FbhyADGmbwCEGiEtpEmdYoIEgidA
			return this.FbhyADGmbwCEGiEtpEmdYoIEgidA(P_0);
		}
	}
}
