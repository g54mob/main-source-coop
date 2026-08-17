using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ExpandableArray_DataContainer<T> where T : class, ExpandableArray_DataContainer<T>.QTrGBtIHwYobnOntzxzKKGyOQdhE, new()
	{
		public interface QTrGBtIHwYobnOntzxzKKGyOQdhE : IComparable<T>
		{
			void MKxeORgAEMftvJBFwnZRaVeDmxxoA(T P_0);

			bool PqWttESsBdekfHEfPEQloXKyyvBHA(T P_0);

			void bAhOAKYgQytoBnloVDPGAvEFsKQAA();
		}

		public readonly T injector;

		private T[] oTSNskVmQAmTIJHXyloPIpJijWogA;

		private int bHMOSQCIbCGkhhHYEDmOGUqiCzfQ;

		private int UTuDkwFSyNdKRcOdUGxNFSgdKUqS;

		private int PTvhlfjMmZJRqXjBmHDgtqKLwPsQ;

		private int ODiMotSRtsROrmNkeaOwjBDINHss;

		private bool tYMEZxggVPyBlohslabeIEFnQhsA;

		public int Count => bHMOSQCIbCGkhhHYEDmOGUqiCzfQ;

		public int Length => bHMOSQCIbCGkhhHYEDmOGUqiCzfQ;

		public int MaxLength => UTuDkwFSyNdKRcOdUGxNFSgdKUqS;

		public int FreeSpace => UTuDkwFSyNdKRcOdUGxNFSgdKUqS - bHMOSQCIbCGkhhHYEDmOGUqiCzfQ;

		public T this[int index]
		{
			get
			{
				if (index >= bHMOSQCIbCGkhhHYEDmOGUqiCzfQ)
				{
					throw new IndexOutOfRangeException();
				}
				return oTSNskVmQAmTIJHXyloPIpJijWogA[index];
			}
		}

		public ExpandableArray_DataContainer(int P_0, bool P_1 = true, int P_2 = 0)
		{
			injector = new T();
			oTSNskVmQAmTIJHXyloPIpJijWogA = new T[P_0];
			bHMOSQCIbCGkhhHYEDmOGUqiCzfQ = 0;
			UTuDkwFSyNdKRcOdUGxNFSgdKUqS = P_0;
			tYMEZxggVPyBlohslabeIEFnQhsA = P_1;
			PTvhlfjMmZJRqXjBmHDgtqKLwPsQ = P_2;
			for (int i = 0; i < UTuDkwFSyNdKRcOdUGxNFSgdKUqS; i++)
			{
				oTSNskVmQAmTIJHXyloPIpJijWogA[i] = new T();
			}
		}

		public int Inject()
		{
			int result = AddData(injector);
			if (tYMEZxggVPyBlohslabeIEFnQhsA)
			{
				injector.bAhOAKYgQytoBnloVDPGAvEFsKQAA();
			}
			return result;
		}

		public int InjectIfUnique()
		{
			int result = AddIfUnique(injector);
			if (tYMEZxggVPyBlohslabeIEFnQhsA)
			{
				injector.bAhOAKYgQytoBnloVDPGAvEFsKQAA();
			}
			return result;
		}

		public int AddData(T item)
		{
			if (bHMOSQCIbCGkhhHYEDmOGUqiCzfQ >= UTuDkwFSyNdKRcOdUGxNFSgdKUqS)
			{
				if (PTvhlfjMmZJRqXjBmHDgtqKLwPsQ <= 0)
				{
					return -1;
				}
				skvmeOmtqbqcbXiXSeRfrlfiDhGY();
			}
			int num = bHMOSQCIbCGkhhHYEDmOGUqiCzfQ;
			oTSNskVmQAmTIJHXyloPIpJijWogA[num].MKxeORgAEMftvJBFwnZRaVeDmxxoA(item);
			bHMOSQCIbCGkhhHYEDmOGUqiCzfQ = num + 1;
			return num;
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
			for (int i = 0; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ; i++)
			{
				if (oTSNskVmQAmTIJHXyloPIpJijWogA[i].PqWttESsBdekfHEfPEQloXKyyvBHA(item))
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOfData(T item)
		{
			for (int i = 0; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ; i++)
			{
				if (oTSNskVmQAmTIJHXyloPIpJijWogA[i].PqWttESsBdekfHEfPEQloXKyyvBHA(item))
				{
					return i;
				}
			}
			return -1;
		}

		public void Clear()
		{
			if (tYMEZxggVPyBlohslabeIEFnQhsA)
			{
				injector.bAhOAKYgQytoBnloVDPGAvEFsKQAA();
				for (int i = 0; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ; i++)
				{
					oTSNskVmQAmTIJHXyloPIpJijWogA[i].bAhOAKYgQytoBnloVDPGAvEFsKQAA();
				}
			}
			bHMOSQCIbCGkhhHYEDmOGUqiCzfQ = 0;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= bHMOSQCIbCGkhhHYEDmOGUqiCzfQ)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index == bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1)
			{
				RemoveLast();
				return;
			}
			if (tYMEZxggVPyBlohslabeIEFnQhsA)
			{
				oTSNskVmQAmTIJHXyloPIpJijWogA[index].bAhOAKYgQytoBnloVDPGAvEFsKQAA();
			}
			for (int i = index; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1; i++)
			{
				oTSNskVmQAmTIJHXyloPIpJijWogA[i].MKxeORgAEMftvJBFwnZRaVeDmxxoA(oTSNskVmQAmTIJHXyloPIpJijWogA[i + 1]);
			}
			if (tYMEZxggVPyBlohslabeIEFnQhsA)
			{
				oTSNskVmQAmTIJHXyloPIpJijWogA[bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1].bAhOAKYgQytoBnloVDPGAvEFsKQAA();
			}
			bHMOSQCIbCGkhhHYEDmOGUqiCzfQ--;
		}

		public void RemoveLast()
		{
			if (bHMOSQCIbCGkhhHYEDmOGUqiCzfQ != 0)
			{
				if (tYMEZxggVPyBlohslabeIEFnQhsA)
				{
					oTSNskVmQAmTIJHXyloPIpJijWogA[bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1].bAhOAKYgQytoBnloVDPGAvEFsKQAA();
				}
				bHMOSQCIbCGkhhHYEDmOGUqiCzfQ--;
			}
		}

		public void Resize(int size)
		{
			if (size <= 0)
			{
				throw new Exception("Size must be greater than 0.");
			}
			if (size == UTuDkwFSyNdKRcOdUGxNFSgdKUqS)
			{
				return;
			}
			T[] array = new T[size];
			int num = Math.Min(size, UTuDkwFSyNdKRcOdUGxNFSgdKUqS);
			for (int i = 0; i < num; i++)
			{
				array[i] = oTSNskVmQAmTIJHXyloPIpJijWogA[i];
			}
			if (size > UTuDkwFSyNdKRcOdUGxNFSgdKUqS)
			{
				for (int j = num; j < size; j++)
				{
					array[j] = new T();
				}
			}
			else if (bHMOSQCIbCGkhhHYEDmOGUqiCzfQ > size)
			{
				bHMOSQCIbCGkhhHYEDmOGUqiCzfQ = size;
			}
			UTuDkwFSyNdKRcOdUGxNFSgdKUqS = size;
			oTSNskVmQAmTIJHXyloPIpJijWogA = array;
		}

		public void SortAscending()
		{
			if (bHMOSQCIbCGkhhHYEDmOGUqiCzfQ == 0)
			{
				return;
			}
			for (int i = 0; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1; i++)
			{
				for (int j = i + 1; j < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ; j++)
				{
					if (oTSNskVmQAmTIJHXyloPIpJijWogA[j].CompareTo(oTSNskVmQAmTIJHXyloPIpJijWogA[i]) < 0)
					{
						T val = oTSNskVmQAmTIJHXyloPIpJijWogA[i];
						oTSNskVmQAmTIJHXyloPIpJijWogA[i] = oTSNskVmQAmTIJHXyloPIpJijWogA[j];
						oTSNskVmQAmTIJHXyloPIpJijWogA[j] = val;
					}
				}
			}
		}

		public void SortDescending()
		{
			if (bHMOSQCIbCGkhhHYEDmOGUqiCzfQ == 0)
			{
				return;
			}
			for (int i = 0; i < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ - 1; i++)
			{
				for (int j = i + 1; j < bHMOSQCIbCGkhhHYEDmOGUqiCzfQ; j++)
				{
					if (oTSNskVmQAmTIJHXyloPIpJijWogA[j].CompareTo(oTSNskVmQAmTIJHXyloPIpJijWogA[i]) > 0)
					{
						T val = oTSNskVmQAmTIJHXyloPIpJijWogA[i];
						oTSNskVmQAmTIJHXyloPIpJijWogA[i] = oTSNskVmQAmTIJHXyloPIpJijWogA[j];
						oTSNskVmQAmTIJHXyloPIpJijWogA[j] = val;
					}
				}
			}
		}

		private void skvmeOmtqbqcbXiXSeRfrlfiDhGY()
		{
			ODiMotSRtsROrmNkeaOwjBDINHss++;
			Resize(UTuDkwFSyNdKRcOdUGxNFSgdKUqS + ODiMotSRtsROrmNkeaOwjBDINHss * PTvhlfjMmZJRqXjBmHDgtqKLwPsQ);
		}
	}
}
