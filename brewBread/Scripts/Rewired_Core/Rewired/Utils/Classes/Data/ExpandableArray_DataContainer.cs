using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ExpandableArray_DataContainer<T> where T : class, ExpandableArray_DataContainer<T>.qARAlhhogsFTixiCVGpIfnVauIuJb, new()
	{
		public interface qARAlhhogsFTixiCVGpIfnVauIuJb : IComparable<T>
		{
			void Set(T P_0);

			bool Equals(T P_0);

			void Clear();
		}

		public readonly T injector;

		private T[] MkIQcLiiGSSdDGDnhPcZzTRYKLoi;

		private int BtoNWhGGVPfRYJEhpxLhTDafoZVK;

		private int LBXwpJrloMdLoFHrRAChALSDmSzH;

		private int XupaqrEiZlDgeEyOTRmMGiOHUTXYb;

		private int znYQrrwkRodTkAwSLgZqLQjomPhv;

		private bool mrAGTKJJhYIIlRrdTkdsAcBtWke;

		public int Count => BtoNWhGGVPfRYJEhpxLhTDafoZVK;

		public int Length => BtoNWhGGVPfRYJEhpxLhTDafoZVK;

		public int MaxLength => LBXwpJrloMdLoFHrRAChALSDmSzH;

		public int FreeSpace => LBXwpJrloMdLoFHrRAChALSDmSzH - BtoNWhGGVPfRYJEhpxLhTDafoZVK;

		public T this[int index]
		{
			get
			{
				if (index >= BtoNWhGGVPfRYJEhpxLhTDafoZVK)
				{
					throw new IndexOutOfRangeException();
				}
				return MkIQcLiiGSSdDGDnhPcZzTRYKLoi[index];
			}
		}

		public ExpandableArray_DataContainer(int P_0, bool P_1 = true, int P_2 = 0)
		{
			injector = new T();
			MkIQcLiiGSSdDGDnhPcZzTRYKLoi = new T[P_0];
			BtoNWhGGVPfRYJEhpxLhTDafoZVK = 0;
			LBXwpJrloMdLoFHrRAChALSDmSzH = P_0;
			mrAGTKJJhYIIlRrdTkdsAcBtWke = P_1;
			XupaqrEiZlDgeEyOTRmMGiOHUTXYb = P_2;
			for (int i = 0; i < LBXwpJrloMdLoFHrRAChALSDmSzH; i++)
			{
				MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i] = new T();
			}
		}

		public int Inject()
		{
			int result = AddData(injector);
			if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
			{
				injector.Clear();
			}
			return result;
		}

		public int InjectIfUnique()
		{
			int result = AddIfUnique(injector);
			if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
			{
				injector.Clear();
			}
			return result;
		}

		public int AddData(T item)
		{
			if (BtoNWhGGVPfRYJEhpxLhTDafoZVK >= LBXwpJrloMdLoFHrRAChALSDmSzH)
			{
				if (XupaqrEiZlDgeEyOTRmMGiOHUTXYb <= 0)
				{
					return -1;
				}
				rOFrVKEBUqMjVNRgiWiPYFyZOvEo();
			}
			int btoNWhGGVPfRYJEhpxLhTDafoZVK = BtoNWhGGVPfRYJEhpxLhTDafoZVK;
			MkIQcLiiGSSdDGDnhPcZzTRYKLoi[btoNWhGGVPfRYJEhpxLhTDafoZVK].Set(item);
			BtoNWhGGVPfRYJEhpxLhTDafoZVK = btoNWhGGVPfRYJEhpxLhTDafoZVK + 1;
			return btoNWhGGVPfRYJEhpxLhTDafoZVK;
		}

		public int AddIfUnique(T item)
		{
			int num = IndexOfData(item);
			if (num >= 0)
			{
				return num;
			}
			return AddData(item);
		}

		public bool ContainsData(T item)
		{
			for (int i = 0; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK; i++)
			{
				if (MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOfData(T item)
		{
			for (int i = 0; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK; i++)
			{
				if (MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		public void Clear()
		{
			if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
			{
				injector.Clear();
				for (int i = 0; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK; i++)
				{
					MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i].Clear();
				}
			}
			BtoNWhGGVPfRYJEhpxLhTDafoZVK = 0;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= BtoNWhGGVPfRYJEhpxLhTDafoZVK)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index == BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1)
			{
				RemoveLast();
				return;
			}
			if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
			{
				MkIQcLiiGSSdDGDnhPcZzTRYKLoi[index].Clear();
			}
			for (int i = index; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1; i++)
			{
				MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i].Set(MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i + 1]);
			}
			if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
			{
				MkIQcLiiGSSdDGDnhPcZzTRYKLoi[BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1].Clear();
			}
			BtoNWhGGVPfRYJEhpxLhTDafoZVK--;
		}

		public void RemoveLast()
		{
			if (BtoNWhGGVPfRYJEhpxLhTDafoZVK != 0)
			{
				if (mrAGTKJJhYIIlRrdTkdsAcBtWke)
				{
					MkIQcLiiGSSdDGDnhPcZzTRYKLoi[BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1].Clear();
				}
				BtoNWhGGVPfRYJEhpxLhTDafoZVK--;
			}
		}

		public void Resize(int size)
		{
			if (size <= 0)
			{
				throw new Exception("Size must be greater than 0.");
			}
			if (size == LBXwpJrloMdLoFHrRAChALSDmSzH)
			{
				return;
			}
			T[] array = new T[size];
			int num = Math.Min(size, LBXwpJrloMdLoFHrRAChALSDmSzH);
			for (int i = 0; i < num; i++)
			{
				array[i] = MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i];
			}
			if (size > LBXwpJrloMdLoFHrRAChALSDmSzH)
			{
				for (int j = num; j < size; j++)
				{
					array[j] = new T();
				}
			}
			else if (BtoNWhGGVPfRYJEhpxLhTDafoZVK > size)
			{
				BtoNWhGGVPfRYJEhpxLhTDafoZVK = size;
			}
			LBXwpJrloMdLoFHrRAChALSDmSzH = size;
			MkIQcLiiGSSdDGDnhPcZzTRYKLoi = array;
		}

		public void SortAscending()
		{
			if (BtoNWhGGVPfRYJEhpxLhTDafoZVK == 0)
			{
				return;
			}
			for (int i = 0; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1; i++)
			{
				for (int j = i + 1; j < BtoNWhGGVPfRYJEhpxLhTDafoZVK; j++)
				{
					if (MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j].CompareTo(MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i]) < 0)
					{
						T val = MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i];
						MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i] = MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j];
						MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j] = val;
					}
				}
			}
		}

		public void SortDescending()
		{
			if (BtoNWhGGVPfRYJEhpxLhTDafoZVK == 0)
			{
				return;
			}
			for (int i = 0; i < BtoNWhGGVPfRYJEhpxLhTDafoZVK - 1; i++)
			{
				for (int j = i + 1; j < BtoNWhGGVPfRYJEhpxLhTDafoZVK; j++)
				{
					if (MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j].CompareTo(MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i]) > 0)
					{
						T val = MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i];
						MkIQcLiiGSSdDGDnhPcZzTRYKLoi[i] = MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j];
						MkIQcLiiGSSdDGDnhPcZzTRYKLoi[j] = val;
					}
				}
			}
		}

		private void rOFrVKEBUqMjVNRgiWiPYFyZOvEo()
		{
			znYQrrwkRodTkAwSLgZqLQjomPhv++;
			Resize(LBXwpJrloMdLoFHrRAChALSDmSzH + znYQrrwkRodTkAwSLgZqLQjomPhv * XupaqrEiZlDgeEyOTRmMGiOHUTXYb);
		}
	}
}
