using System;
using System.Collections;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class RingBuffer<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		[Serializable]
		public struct gvGVbWGfZCWCRIDuymBTENuUNaCv : IEnumerator<T>, IEnumerator, IDisposable
		{
			private RingBuffer<T> buffer;

			private int index;

			private int version;

			private T current;

			T IEnumerator<T>.Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == buffer.TyScMDVIhHkvNsZcHoKuteXcTTio + 1)
					{
						throw new InvalidOperationException();
					}
					return this.Current;
				}
			}

			internal gvGVbWGfZCWCRIDuymBTENuUNaCv(RingBuffer<T> P_0)
			{
				buffer = P_0;
				index = 0;
				version = P_0.bRGADrkhgJuwyJKjAxKiJhLtEHSPA;
				current = default(T);
			}

			public void Dispose()
			{
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			public bool MoveNext()
			{
				if (version == buffer.bRGADrkhgJuwyJKjAxKiJhLtEHSPA && (uint)index < (uint)buffer.TyScMDVIhHkvNsZcHoKuteXcTTio)
				{
					current = buffer[index];
					index++;
					return true;
				}
				return yIFhLvaqXYeegiCgpDDDHiwSXkvQA();
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private bool yIFhLvaqXYeegiCgpDDDHiwSXkvQA()
			{
				if (version != buffer.bRGADrkhgJuwyJKjAxKiJhLtEHSPA)
				{
					throw new InvalidOperationException("RingBuffer was changed.");
				}
				index = buffer.TyScMDVIhHkvNsZcHoKuteXcTTio + 1;
				current = default(T);
				return false;
			}

			void IEnumerator.Reset()
			{
				if (version != buffer.bRGADrkhgJuwyJKjAxKiJhLtEHSPA)
				{
					throw new InvalidOperationException("RingBuffer was changed.");
				}
				index = 0;
				current = default(T);
			}
		}

		private readonly T[] PAtYEPqMvBCsmFpzVnTaVsLCpOei;

		private readonly int rjfarvhGZRWgjCrNKLQSgJaWzjVJA;

		private int bJJLOqXqNyFBXmYqkOxvkVKribRJ;

		private int BvUbivCFpmZpRDgfKRLtnJYJCKBN;

		private int TyScMDVIhHkvNsZcHoKuteXcTTio;

		private int vTrTWGeUCqRMczAgdywcqvEoQdik;

		private int bRGADrkhgJuwyJKjAxKiJhLtEHSPA;

		private IEqualityComparer<T> MFdHsmQpdlIHyLfocvymvDrGbNxA = EqualityComparerNoAlloc<T>.Default;

		public int Count => TyScMDVIhHkvNsZcHoKuteXcTTio;

		public int Capacity => rjfarvhGZRWgjCrNKLQSgJaWzjVJA;

		public int OverrunCount => vTrTWGeUCqRMczAgdywcqvEoQdik;

		public IEqualityComparer<T> EqualityComparer
		{
			get
			{
				return MFdHsmQpdlIHyLfocvymvDrGbNxA;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<T>.Default;
				}
				MFdHsmQpdlIHyLfocvymvDrGbNxA = value;
			}
		}

		public T this[int index]
		{
			get
			{
				int num = tzszEVGRWVAeGpDxUlNjeVoEHbrU(index);
				if (!EAvAHyabLGHMeTAWQCmHHbTiIekmb(num))
				{
					throw new IndexOutOfRangeException();
				}
				return PAtYEPqMvBCsmFpzVnTaVsLCpOei[num];
			}
			set
			{
				int num = tzszEVGRWVAeGpDxUlNjeVoEHbrU(index);
				if (!EAvAHyabLGHMeTAWQCmHHbTiIekmb(num))
				{
					throw new IndexOutOfRangeException();
				}
				PAtYEPqMvBCsmFpzVnTaVsLCpOei[num] = value;
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
			PAtYEPqMvBCsmFpzVnTaVsLCpOei = new T[P_0];
			rjfarvhGZRWgjCrNKLQSgJaWzjVJA = P_0;
			Clear();
		}

		public void Enqueue(T item)
		{
			bJJLOqXqNyFBXmYqkOxvkVKribRJ = ((bJJLOqXqNyFBXmYqkOxvkVKribRJ < rjfarvhGZRWgjCrNKLQSgJaWzjVJA - 1) ? (bJJLOqXqNyFBXmYqkOxvkVKribRJ + 1) : 0);
			if (TyScMDVIhHkvNsZcHoKuteXcTTio == 0)
			{
				BvUbivCFpmZpRDgfKRLtnJYJCKBN = 0;
			}
			else if (bJJLOqXqNyFBXmYqkOxvkVKribRJ == BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				BvUbivCFpmZpRDgfKRLtnJYJCKBN = ((BvUbivCFpmZpRDgfKRLtnJYJCKBN < rjfarvhGZRWgjCrNKLQSgJaWzjVJA - 1) ? (BvUbivCFpmZpRDgfKRLtnJYJCKBN + 1) : 0);
				vTrTWGeUCqRMczAgdywcqvEoQdik++;
			}
			PAtYEPqMvBCsmFpzVnTaVsLCpOei[bJJLOqXqNyFBXmYqkOxvkVKribRJ] = item;
			if (TyScMDVIhHkvNsZcHoKuteXcTTio < rjfarvhGZRWgjCrNKLQSgJaWzjVJA)
			{
				TyScMDVIhHkvNsZcHoKuteXcTTio++;
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
			if (TyScMDVIhHkvNsZcHoKuteXcTTio == 0)
			{
				throw new Exception("There are no items in the buffer.");
			}
			T result = PAtYEPqMvBCsmFpzVnTaVsLCpOei[BvUbivCFpmZpRDgfKRLtnJYJCKBN];
			if (BvUbivCFpmZpRDgfKRLtnJYJCKBN == bJJLOqXqNyFBXmYqkOxvkVKribRJ)
			{
				Clear();
				return result;
			}
			PAtYEPqMvBCsmFpzVnTaVsLCpOei[BvUbivCFpmZpRDgfKRLtnJYJCKBN] = default(T);
			BvUbivCFpmZpRDgfKRLtnJYJCKBN = ((BvUbivCFpmZpRDgfKRLtnJYJCKBN < rjfarvhGZRWgjCrNKLQSgJaWzjVJA - 1) ? (BvUbivCFpmZpRDgfKRLtnJYJCKBN + 1) : 0);
			vTrTWGeUCqRMczAgdywcqvEoQdik = 0;
			TyScMDVIhHkvNsZcHoKuteXcTTio--;
			bRGADrkhgJuwyJKjAxKiJhLtEHSPA++;
			return result;
		}

		public T Peek()
		{
			if (bJJLOqXqNyFBXmYqkOxvkVKribRJ < 0)
			{
				throw new Exception("There are no items in the buffer.");
			}
			return PAtYEPqMvBCsmFpzVnTaVsLCpOei[BvUbivCFpmZpRDgfKRLtnJYJCKBN];
		}

		public bool Contains(T item)
		{
			return grvTlmoriOvyOhLyjwuaVlsRSrmf(item, MFdHsmQpdlIHyLfocvymvDrGbNxA) >= 0;
		}

		public bool Contains(T item, IEqualityComparer<T> comparer)
		{
			return grvTlmoriOvyOhLyjwuaVlsRSrmf(item, comparer) >= 0;
		}

		public int IndexOf(T item)
		{
			return IndexOf(item, MFdHsmQpdlIHyLfocvymvDrGbNxA);
		}

		public int IndexOf(T item, IEqualityComparer<T> comparer)
		{
			return lEyueCLmCZFITDvUCurViAzBdMzU(grvTlmoriOvyOhLyjwuaVlsRSrmf(item, comparer));
		}

		public bool Remove(T item)
		{
			return Remove(item, MFdHsmQpdlIHyLfocvymvDrGbNxA);
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
			int num = grvTlmoriOvyOhLyjwuaVlsRSrmf(item, comparer);
			if (num < 0)
			{
				return false;
			}
			IFaHVJPDqsqUSzjevlNJUJKZBnjC(num);
			return true;
		}

		public void RemoveAt(int index)
		{
			IFaHVJPDqsqUSzjevlNJUJKZBnjC(tzszEVGRWVAeGpDxUlNjeVoEHbrU(index));
		}

		public int RemoveAll(T item)
		{
			return RemoveAll(item, MFdHsmQpdlIHyLfocvymvDrGbNxA);
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
			if (TyScMDVIhHkvNsZcHoKuteXcTTio > 0)
			{
				if (bJJLOqXqNyFBXmYqkOxvkVKribRJ >= BvUbivCFpmZpRDgfKRLtnJYJCKBN)
				{
					Array.Clear(PAtYEPqMvBCsmFpzVnTaVsLCpOei, BvUbivCFpmZpRDgfKRLtnJYJCKBN, bJJLOqXqNyFBXmYqkOxvkVKribRJ - BvUbivCFpmZpRDgfKRLtnJYJCKBN + 1);
				}
				else
				{
					Array.Clear(PAtYEPqMvBCsmFpzVnTaVsLCpOei, 0, bJJLOqXqNyFBXmYqkOxvkVKribRJ + 1);
					Array.Clear(PAtYEPqMvBCsmFpzVnTaVsLCpOei, BvUbivCFpmZpRDgfKRLtnJYJCKBN, rjfarvhGZRWgjCrNKLQSgJaWzjVJA - BvUbivCFpmZpRDgfKRLtnJYJCKBN);
				}
				TyScMDVIhHkvNsZcHoKuteXcTTio = 0;
			}
			bJJLOqXqNyFBXmYqkOxvkVKribRJ = -1;
			BvUbivCFpmZpRDgfKRLtnJYJCKBN = -1;
			vTrTWGeUCqRMczAgdywcqvEoQdik = 0;
			bRGADrkhgJuwyJKjAxKiJhLtEHSPA++;
		}

		private int sZvXrvbuvaTYkcJcGqIsNdLlIjmd(T P_0)
		{
			return grvTlmoriOvyOhLyjwuaVlsRSrmf(P_0, MFdHsmQpdlIHyLfocvymvDrGbNxA);
		}

		private int grvTlmoriOvyOhLyjwuaVlsRSrmf(T P_0, IEqualityComparer<T> P_1)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("comparer");
			}
			if (TyScMDVIhHkvNsZcHoKuteXcTTio == 0)
			{
				return -1;
			}
			if (bJJLOqXqNyFBXmYqkOxvkVKribRJ >= BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				for (int i = BvUbivCFpmZpRDgfKRLtnJYJCKBN; i <= bJJLOqXqNyFBXmYqkOxvkVKribRJ; i++)
				{
					if (P_1.Equals(PAtYEPqMvBCsmFpzVnTaVsLCpOei[i], P_0))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = 0; j <= bJJLOqXqNyFBXmYqkOxvkVKribRJ; j++)
				{
					if (P_1.Equals(PAtYEPqMvBCsmFpzVnTaVsLCpOei[j], P_0))
					{
						return j;
					}
				}
				for (int k = BvUbivCFpmZpRDgfKRLtnJYJCKBN; k < rjfarvhGZRWgjCrNKLQSgJaWzjVJA; k++)
				{
					if (P_1.Equals(PAtYEPqMvBCsmFpzVnTaVsLCpOei[k], P_0))
					{
						return k;
					}
				}
			}
			return -1;
		}

		private void IFaHVJPDqsqUSzjevlNJUJKZBnjC(int P_0)
		{
			if (!EAvAHyabLGHMeTAWQCmHHbTiIekmb(P_0))
			{
				throw new IndexOutOfRangeException();
			}
			if (P_0 == BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				Dequeue();
				return;
			}
			if (P_0 != bJJLOqXqNyFBXmYqkOxvkVKribRJ)
			{
				if (bJJLOqXqNyFBXmYqkOxvkVKribRJ > BvUbivCFpmZpRDgfKRLtnJYJCKBN)
				{
					Array.Copy(PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0 + 1, PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0, bJJLOqXqNyFBXmYqkOxvkVKribRJ - P_0);
				}
				else if (P_0 < bJJLOqXqNyFBXmYqkOxvkVKribRJ)
				{
					Array.Copy(PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0 + 1, PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0, bJJLOqXqNyFBXmYqkOxvkVKribRJ - P_0);
				}
				else
				{
					Array.Copy(PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0 + 1, PAtYEPqMvBCsmFpzVnTaVsLCpOei, P_0, rjfarvhGZRWgjCrNKLQSgJaWzjVJA - P_0 - 1);
					PAtYEPqMvBCsmFpzVnTaVsLCpOei[rjfarvhGZRWgjCrNKLQSgJaWzjVJA - 1] = PAtYEPqMvBCsmFpzVnTaVsLCpOei[0];
					if (bJJLOqXqNyFBXmYqkOxvkVKribRJ > 0)
					{
						Array.Copy(PAtYEPqMvBCsmFpzVnTaVsLCpOei, 1, PAtYEPqMvBCsmFpzVnTaVsLCpOei, 0, bJJLOqXqNyFBXmYqkOxvkVKribRJ);
					}
				}
			}
			PAtYEPqMvBCsmFpzVnTaVsLCpOei[bJJLOqXqNyFBXmYqkOxvkVKribRJ] = default(T);
			bJJLOqXqNyFBXmYqkOxvkVKribRJ = ((bJJLOqXqNyFBXmYqkOxvkVKribRJ > 0) ? (bJJLOqXqNyFBXmYqkOxvkVKribRJ - 1) : (rjfarvhGZRWgjCrNKLQSgJaWzjVJA - 1));
			bRGADrkhgJuwyJKjAxKiJhLtEHSPA++;
			TyScMDVIhHkvNsZcHoKuteXcTTio--;
		}

		private bool EAvAHyabLGHMeTAWQCmHHbTiIekmb(int P_0)
		{
			if (TyScMDVIhHkvNsZcHoKuteXcTTio == 0)
			{
				return false;
			}
			if (bJJLOqXqNyFBXmYqkOxvkVKribRJ >= BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				if (P_0 >= BvUbivCFpmZpRDgfKRLtnJYJCKBN)
				{
					return P_0 <= bJJLOqXqNyFBXmYqkOxvkVKribRJ;
				}
				return false;
			}
			if (P_0 < BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				return P_0 <= bJJLOqXqNyFBXmYqkOxvkVKribRJ;
			}
			return true;
		}

		private int lEyueCLmCZFITDvUCurViAzBdMzU(int P_0)
		{
			if ((uint)P_0 >= (uint)rjfarvhGZRWgjCrNKLQSgJaWzjVJA)
			{
				return -1;
			}
			if (!EAvAHyabLGHMeTAWQCmHHbTiIekmb(P_0))
			{
				return -1;
			}
			if (P_0 >= BvUbivCFpmZpRDgfKRLtnJYJCKBN)
			{
				return P_0 - BvUbivCFpmZpRDgfKRLtnJYJCKBN;
			}
			return P_0 + rjfarvhGZRWgjCrNKLQSgJaWzjVJA - BvUbivCFpmZpRDgfKRLtnJYJCKBN;
		}

		private int tzszEVGRWVAeGpDxUlNjeVoEHbrU(int P_0)
		{
			if ((uint)P_0 >= (uint)TyScMDVIhHkvNsZcHoKuteXcTTio)
			{
				return -1;
			}
			P_0 = BvUbivCFpmZpRDgfKRLtnJYJCKBN + P_0;
			if (P_0 >= rjfarvhGZRWgjCrNKLQSgJaWzjVJA)
			{
				P_0 -= rjfarvhGZRWgjCrNKLQSgJaWzjVJA;
			}
			return P_0;
		}

		private void nayGZAjuznLOFsWDChiLPLRdMZNT(T P_0)
		{
			Enqueue(P_0);
		}

		void ICollection<T>.Add(T P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in nayGZAjuznLOFsWDChiLPLRdMZNT
			this.nayGZAjuznLOFsWDChiLPLRdMZNT(P_0);
		}

		private void ZfuOXDuphqHSLlswamtaRmtMiuOL()
		{
			Clear();
		}

		void ICollection<T>.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ZfuOXDuphqHSLlswamtaRmtMiuOL
			this.ZfuOXDuphqHSLlswamtaRmtMiuOL();
		}

		private bool VvEdRekLkbqbcEeODsketJltqGCxB(T P_0)
		{
			return Contains(P_0);
		}

		bool ICollection<T>.Contains(T P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in VvEdRekLkbqbcEeODsketJltqGCxB
			return this.VvEdRekLkbqbcEeODsketJltqGCxB(P_0);
		}

		private void fqYqIeSItsebLFaAFDnHbMDnmeejA(T[] P_0, int P_1)
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

		void ICollection<T>.CopyTo(T[] P_0, int P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fqYqIeSItsebLFaAFDnHbMDnmeejA
			this.fqYqIeSItsebLFaAFDnHbMDnmeejA(P_0, P_1);
		}

		private bool ofyGQrMaUgPSldFcJmAouRvbhQsW(T P_0)
		{
			return Remove(P_0);
		}

		bool ICollection<T>.Remove(T P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ofyGQrMaUgPSldFcJmAouRvbhQsW
			return this.ofyGQrMaUgPSldFcJmAouRvbhQsW(P_0);
		}

		private IEnumerator<T> xrfekDAyuNkfBOZoaeXUblUUqlGIA()
		{
			return new gvGVbWGfZCWCRIDuymBTENuUNaCv(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xrfekDAyuNkfBOZoaeXUblUUqlGIA
			return this.xrfekDAyuNkfBOZoaeXUblUUqlGIA();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new gvGVbWGfZCWCRIDuymBTENuUNaCv(this);
		}
	}
}
