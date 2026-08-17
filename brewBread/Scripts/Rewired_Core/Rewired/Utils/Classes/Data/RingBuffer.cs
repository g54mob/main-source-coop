using System;
using System.Collections;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class RingBuffer<T> : IEnumerable, IEnumerable<T>, ICollection<T>
	{
		[Serializable]
		public struct EUaENEscwmvyAomFEBuDbjduuWHF : IDisposable, IEnumerator, IEnumerator<T>
		{
			private RingBuffer<T> buffer;

			private int index;

			private int version;

			private T current;

			public T Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == buffer.BnPuTevKonSCNieJesLlCkEZXpqL + 1)
					{
						throw new InvalidOperationException();
					}
					return Current;
				}
			}

			internal EUaENEscwmvyAomFEBuDbjduuWHF(RingBuffer<T> P_0)
			{
				buffer = P_0;
				index = 0;
				version = P_0.ZMmXJIUpRkkINbjuQpRQDgAgOgdk;
				current = default(T);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (version == buffer.ZMmXJIUpRkkINbjuQpRQDgAgOgdk && (uint)index < (uint)buffer.BnPuTevKonSCNieJesLlCkEZXpqL)
				{
					current = buffer[index];
					index++;
					return true;
				}
				return TXujFZWZoalxahmedVbObmnbHsJg();
			}

			private bool TXujFZWZoalxahmedVbObmnbHsJg()
			{
				if (version != buffer.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
				{
					throw new InvalidOperationException("RingBuffer was changed.");
				}
				index = buffer.BnPuTevKonSCNieJesLlCkEZXpqL + 1;
				current = default(T);
				return false;
			}

			void IEnumerator.Reset()
			{
				if (version != buffer.ZMmXJIUpRkkINbjuQpRQDgAgOgdk)
				{
					throw new InvalidOperationException("RingBuffer was changed.");
				}
				index = 0;
				current = default(T);
			}
		}

		private readonly T[] vhgmYKGNVGCYssLrjcOzzIQgRUKC;

		private readonly int eAZTOidCgblheDmZfgTMJcXnhJLfb;

		private int gszfWHxacAIWrKDMmHnJLYlsmUId;

		private int SqieEccdXGIwYFEwfPenZizjrjEpc;

		private int BnPuTevKonSCNieJesLlCkEZXpqL;

		private int zSFrnoxWRSnWBlzFVAmdvdggxHAC;

		private int ZMmXJIUpRkkINbjuQpRQDgAgOgdk;

		private IEqualityComparer<T> csVFVCmLCxbZLAudZQNydzCxlEqy = EqualityComparerNoAlloc<T>.Default;

		public int Count => BnPuTevKonSCNieJesLlCkEZXpqL;

		public int Capacity => eAZTOidCgblheDmZfgTMJcXnhJLfb;

		public int OverrunCount => zSFrnoxWRSnWBlzFVAmdvdggxHAC;

		public IEqualityComparer<T> EqualityComparer
		{
			get
			{
				return csVFVCmLCxbZLAudZQNydzCxlEqy;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<T>.Default;
				}
				csVFVCmLCxbZLAudZQNydzCxlEqy = value;
			}
		}

		public T this[int index]
		{
			get
			{
				int num = ZuxpZZKVJhMzPyDDNHYEFIPrBzDc(index);
				if (!zpmeSEnESLhcjqJsZQpxRMCtmMEi(num))
				{
					throw new IndexOutOfRangeException();
				}
				return vhgmYKGNVGCYssLrjcOzzIQgRUKC[num];
			}
			set
			{
				int num = ZuxpZZKVJhMzPyDDNHYEFIPrBzDc(index);
				if (!zpmeSEnESLhcjqJsZQpxRMCtmMEi(num))
				{
					throw new IndexOutOfRangeException();
				}
				vhgmYKGNVGCYssLrjcOzzIQgRUKC[num] = value;
			}
		}

		int ICollection<T>.Count => Count;

		bool ICollection<T>.IsReadOnly => false;

		public RingBuffer(int P_0)
		{
			if (P_0 <= 0)
			{
				throw new ArgumentOutOfRangeException("capacity must be > 0.");
			}
			vhgmYKGNVGCYssLrjcOzzIQgRUKC = new T[P_0];
			eAZTOidCgblheDmZfgTMJcXnhJLfb = P_0;
			Clear();
		}

		public void Enqueue(T item)
		{
			gszfWHxacAIWrKDMmHnJLYlsmUId = ((gszfWHxacAIWrKDMmHnJLYlsmUId < eAZTOidCgblheDmZfgTMJcXnhJLfb - 1) ? (gszfWHxacAIWrKDMmHnJLYlsmUId + 1) : 0);
			if (BnPuTevKonSCNieJesLlCkEZXpqL == 0)
			{
				SqieEccdXGIwYFEwfPenZizjrjEpc = 0;
			}
			else if (gszfWHxacAIWrKDMmHnJLYlsmUId == SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				SqieEccdXGIwYFEwfPenZizjrjEpc = ((SqieEccdXGIwYFEwfPenZizjrjEpc < eAZTOidCgblheDmZfgTMJcXnhJLfb - 1) ? (SqieEccdXGIwYFEwfPenZizjrjEpc + 1) : 0);
				zSFrnoxWRSnWBlzFVAmdvdggxHAC++;
			}
			vhgmYKGNVGCYssLrjcOzzIQgRUKC[gszfWHxacAIWrKDMmHnJLYlsmUId] = item;
			if (BnPuTevKonSCNieJesLlCkEZXpqL < eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				BnPuTevKonSCNieJesLlCkEZXpqL++;
			}
		}

		public bool EnqueueIfUnique(T item)
		{
			if (Contains(item))
			{
				return false;
			}
			Enqueue(item);
			return true;
		}

		public T Dequeue()
		{
			if (BnPuTevKonSCNieJesLlCkEZXpqL == 0)
			{
				throw new Exception("There are no items in the buffer.");
			}
			T result = vhgmYKGNVGCYssLrjcOzzIQgRUKC[SqieEccdXGIwYFEwfPenZizjrjEpc];
			if (SqieEccdXGIwYFEwfPenZizjrjEpc == gszfWHxacAIWrKDMmHnJLYlsmUId)
			{
				Clear();
				return result;
			}
			vhgmYKGNVGCYssLrjcOzzIQgRUKC[SqieEccdXGIwYFEwfPenZizjrjEpc] = default(T);
			SqieEccdXGIwYFEwfPenZizjrjEpc = ((SqieEccdXGIwYFEwfPenZizjrjEpc < eAZTOidCgblheDmZfgTMJcXnhJLfb - 1) ? (SqieEccdXGIwYFEwfPenZizjrjEpc + 1) : 0);
			zSFrnoxWRSnWBlzFVAmdvdggxHAC = 0;
			BnPuTevKonSCNieJesLlCkEZXpqL--;
			ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
			return result;
		}

		public T Peek()
		{
			if (gszfWHxacAIWrKDMmHnJLYlsmUId < 0)
			{
				throw new Exception("There are no items in the buffer.");
			}
			return vhgmYKGNVGCYssLrjcOzzIQgRUKC[SqieEccdXGIwYFEwfPenZizjrjEpc];
		}

		public bool Contains(T item)
		{
			return FrGCmUNWAfezMCNhSSxfRiUaizHuA(item, csVFVCmLCxbZLAudZQNydzCxlEqy) >= 0;
		}

		public bool Contains(T item, IEqualityComparer<T> comparer)
		{
			return FrGCmUNWAfezMCNhSSxfRiUaizHuA(item, comparer) >= 0;
		}

		public int IndexOf(T item)
		{
			return IndexOf(item, csVFVCmLCxbZLAudZQNydzCxlEqy);
		}

		public int IndexOf(T item, IEqualityComparer<T> comparer)
		{
			return AxJlLLuiLVIZcGtNVZBqiWLxaGVQ(FrGCmUNWAfezMCNhSSxfRiUaizHuA(item, comparer));
		}

		public bool Remove(T item)
		{
			return Remove(item, csVFVCmLCxbZLAudZQNydzCxlEqy);
		}

		public bool Remove(T item, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			if (Count == 0)
			{
				return false;
			}
			int num = FrGCmUNWAfezMCNhSSxfRiUaizHuA(item, comparer);
			if (num < 0)
			{
				return false;
			}
			jAiUknyYYQbCZysNRKsTvGajYKMd(num);
			return true;
		}

		public void RemoveAt(int index)
		{
			jAiUknyYYQbCZysNRKsTvGajYKMd(ZuxpZZKVJhMzPyDDNHYEFIPrBzDc(index));
		}

		public int RemoveAll(T item)
		{
			return RemoveAll(item, csVFVCmLCxbZLAudZQNydzCxlEqy);
		}

		public int RemoveAll(T item, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			int num = 0;
			for (int num2 = Count - 1; num2 >= 0; num2--)
			{
				if (comparer.Equals(this[num2], item))
				{
					RemoveAt(num2);
					num++;
				}
			}
			return num;
		}

		public void Clear()
		{
			if (BnPuTevKonSCNieJesLlCkEZXpqL > 0)
			{
				if (gszfWHxacAIWrKDMmHnJLYlsmUId >= SqieEccdXGIwYFEwfPenZizjrjEpc)
				{
					Array.Clear(vhgmYKGNVGCYssLrjcOzzIQgRUKC, SqieEccdXGIwYFEwfPenZizjrjEpc, gszfWHxacAIWrKDMmHnJLYlsmUId - SqieEccdXGIwYFEwfPenZizjrjEpc + 1);
				}
				else
				{
					Array.Clear(vhgmYKGNVGCYssLrjcOzzIQgRUKC, 0, gszfWHxacAIWrKDMmHnJLYlsmUId + 1);
					Array.Clear(vhgmYKGNVGCYssLrjcOzzIQgRUKC, SqieEccdXGIwYFEwfPenZizjrjEpc, eAZTOidCgblheDmZfgTMJcXnhJLfb - SqieEccdXGIwYFEwfPenZizjrjEpc);
				}
				BnPuTevKonSCNieJesLlCkEZXpqL = 0;
			}
			gszfWHxacAIWrKDMmHnJLYlsmUId = -1;
			SqieEccdXGIwYFEwfPenZizjrjEpc = -1;
			zSFrnoxWRSnWBlzFVAmdvdggxHAC = 0;
			ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
		}

		private int FrGCmUNWAfezMCNhSSxfRiUaizHuA(T P_0)
		{
			return FrGCmUNWAfezMCNhSSxfRiUaizHuA(P_0, csVFVCmLCxbZLAudZQNydzCxlEqy);
		}

		private int FrGCmUNWAfezMCNhSSxfRiUaizHuA(T P_0, IEqualityComparer<T> P_1)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("comparer");
			}
			if (BnPuTevKonSCNieJesLlCkEZXpqL == 0)
			{
				return -1;
			}
			if (gszfWHxacAIWrKDMmHnJLYlsmUId >= SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				for (int i = SqieEccdXGIwYFEwfPenZizjrjEpc; i <= gszfWHxacAIWrKDMmHnJLYlsmUId; i++)
				{
					if (P_1.Equals(vhgmYKGNVGCYssLrjcOzzIQgRUKC[i], P_0))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = 0; j <= gszfWHxacAIWrKDMmHnJLYlsmUId; j++)
				{
					if (P_1.Equals(vhgmYKGNVGCYssLrjcOzzIQgRUKC[j], P_0))
					{
						return j;
					}
				}
				for (int k = SqieEccdXGIwYFEwfPenZizjrjEpc; k < eAZTOidCgblheDmZfgTMJcXnhJLfb; k++)
				{
					if (P_1.Equals(vhgmYKGNVGCYssLrjcOzzIQgRUKC[k], P_0))
					{
						return k;
					}
				}
			}
			return -1;
		}

		private void jAiUknyYYQbCZysNRKsTvGajYKMd(int P_0)
		{
			if (!zpmeSEnESLhcjqJsZQpxRMCtmMEi(P_0))
			{
				throw new IndexOutOfRangeException();
			}
			if (P_0 == SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				Dequeue();
				return;
			}
			if (P_0 != gszfWHxacAIWrKDMmHnJLYlsmUId)
			{
				if (gszfWHxacAIWrKDMmHnJLYlsmUId > SqieEccdXGIwYFEwfPenZizjrjEpc)
				{
					Array.Copy(vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0 + 1, vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0, gszfWHxacAIWrKDMmHnJLYlsmUId - P_0);
				}
				else if (P_0 < gszfWHxacAIWrKDMmHnJLYlsmUId)
				{
					Array.Copy(vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0 + 1, vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0, gszfWHxacAIWrKDMmHnJLYlsmUId - P_0);
				}
				else
				{
					Array.Copy(vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0 + 1, vhgmYKGNVGCYssLrjcOzzIQgRUKC, P_0, eAZTOidCgblheDmZfgTMJcXnhJLfb - P_0 - 1);
					vhgmYKGNVGCYssLrjcOzzIQgRUKC[eAZTOidCgblheDmZfgTMJcXnhJLfb - 1] = vhgmYKGNVGCYssLrjcOzzIQgRUKC[0];
					if (gszfWHxacAIWrKDMmHnJLYlsmUId > 0)
					{
						Array.Copy(vhgmYKGNVGCYssLrjcOzzIQgRUKC, 1, vhgmYKGNVGCYssLrjcOzzIQgRUKC, 0, gszfWHxacAIWrKDMmHnJLYlsmUId);
					}
				}
			}
			vhgmYKGNVGCYssLrjcOzzIQgRUKC[gszfWHxacAIWrKDMmHnJLYlsmUId] = default(T);
			gszfWHxacAIWrKDMmHnJLYlsmUId = ((gszfWHxacAIWrKDMmHnJLYlsmUId > 0) ? (gszfWHxacAIWrKDMmHnJLYlsmUId - 1) : (eAZTOidCgblheDmZfgTMJcXnhJLfb - 1));
			ZMmXJIUpRkkINbjuQpRQDgAgOgdk++;
			BnPuTevKonSCNieJesLlCkEZXpqL--;
		}

		private bool zpmeSEnESLhcjqJsZQpxRMCtmMEi(int P_0)
		{
			if (BnPuTevKonSCNieJesLlCkEZXpqL == 0)
			{
				return false;
			}
			if (gszfWHxacAIWrKDMmHnJLYlsmUId >= SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				if (P_0 >= SqieEccdXGIwYFEwfPenZizjrjEpc)
				{
					return P_0 <= gszfWHxacAIWrKDMmHnJLYlsmUId;
				}
				return false;
			}
			if (P_0 < SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				return P_0 <= gszfWHxacAIWrKDMmHnJLYlsmUId;
			}
			return true;
		}

		private int AxJlLLuiLVIZcGtNVZBqiWLxaGVQ(int P_0)
		{
			if ((uint)P_0 >= (uint)eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				return -1;
			}
			if (!zpmeSEnESLhcjqJsZQpxRMCtmMEi(P_0))
			{
				return -1;
			}
			if (P_0 >= SqieEccdXGIwYFEwfPenZizjrjEpc)
			{
				return P_0 - SqieEccdXGIwYFEwfPenZizjrjEpc;
			}
			return P_0 + eAZTOidCgblheDmZfgTMJcXnhJLfb - SqieEccdXGIwYFEwfPenZizjrjEpc;
		}

		private int ZuxpZZKVJhMzPyDDNHYEFIPrBzDc(int P_0)
		{
			if ((uint)P_0 >= (uint)BnPuTevKonSCNieJesLlCkEZXpqL)
			{
				return -1;
			}
			P_0 = SqieEccdXGIwYFEwfPenZizjrjEpc + P_0;
			if (P_0 >= eAZTOidCgblheDmZfgTMJcXnhJLfb)
			{
				P_0 -= eAZTOidCgblheDmZfgTMJcXnhJLfb;
			}
			return P_0;
		}

		void ICollection<T>.Add(T P_0)
		{
			Enqueue(P_0);
		}

		void ICollection<T>.Clear()
		{
			Clear();
		}

		bool ICollection<T>.Contains(T P_0)
		{
			return Contains(P_0);
		}

		void ICollection<T>.CopyTo(T[] P_0, int P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("array");
			}
			if (P_1 < 0 || P_1 + Count > P_0.Length)
			{
				throw new ArgumentException("array is too small to hold the collection.");
			}
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				P_0[P_1 + i] = this[i];
			}
		}

		bool ICollection<T>.Remove(T P_0)
		{
			return Remove(P_0);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new EUaENEscwmvyAomFEBuDbjduuWHF(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new EUaENEscwmvyAomFEBuDbjduuWHF(this);
		}
	}
}
