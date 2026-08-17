using System;
using System.Collections.Generic;
using Rewired.Config;
using Rewired.Utils;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class UpdateLoopDataSet<T> where T : class
	{
		private class xsMJBGmRwWlPzYApiztwYCiVQxLi
		{
			public readonly UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

			public T lPMziJCFRnvinaKeGNwjXRpuzYwf;

			public xsMJBGmRwWlPzYApiztwYCiVQxLi(UpdateLoopType P_0)
			{
				PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
			}
		}

		private const int lriuCiYYRPHdAMqSIEfZVzNaaDbi = 0;

		private xsMJBGmRwWlPzYApiztwYCiVQxLi dQvTvZqdFSzAQjIiXjyfRtrnHABD;

		private int BnPuTevKonSCNieJesLlCkEZXpqL;

		public readonly int fixedUpdateSetIndex = -1;

		private readonly int[] IKaHcMMMReldkZgFUUrjWQfbHzQi;

		private readonly xsMJBGmRwWlPzYApiztwYCiVQxLi[] TIBkwaYFfrlorrgEzKrMTlQMftSC;

		private UpdateLoopType VrMfQjbtWyjujWgdTnMyJaEVdoEy = (UpdateLoopType)(-1);

		public T Current => dQvTvZqdFSzAQjIiXjyfRtrnHABD.lPMziJCFRnvinaKeGNwjXRpuzYwf;

		public int Count => BnPuTevKonSCNieJesLlCkEZXpqL;

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= BnPuTevKonSCNieJesLlCkEZXpqL)
				{
					throw new IndexOutOfRangeException();
				}
				return TIBkwaYFfrlorrgEzKrMTlQMftSC[index].lPMziJCFRnvinaKeGNwjXRpuzYwf;
			}
			set
			{
				if (index < 0 || index >= BnPuTevKonSCNieJesLlCkEZXpqL)
				{
					throw new IndexOutOfRangeException();
				}
				TIBkwaYFfrlorrgEzKrMTlQMftSC[index].lPMziJCFRnvinaKeGNwjXRpuzYwf = value;
			}
		}

		public UpdateLoopDataSet(UpdateLoopSetting P_0)
			: this(P_0, (Func<T>)null)
		{
		}

		public UpdateLoopDataSet(UpdateLoopSetting P_0, Func<T> P_1)
		{
			IKaHcMMMReldkZgFUUrjWQfbHzQi = new int[3];
			ArrayTools.Fill(IKaHcMMMReldkZgFUUrjWQfbHzQi, -1);
			List<xsMJBGmRwWlPzYApiztwYCiVQxLi> list = new List<xsMJBGmRwWlPzYApiztwYCiVQxLi>();
			int num = 0;
			using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
			{
				List<UpdateLoopType> list2 = tList.list;
				EnumConverter.ToUpdateLoopTypes(P_0, list2);
				for (int i = 0; i < list2.Count; i++)
				{
					xsMJBGmRwWlPzYApiztwYCiVQxLi xsMJBGmRwWlPzYApiztwYCiVQxLi2 = new xsMJBGmRwWlPzYApiztwYCiVQxLi(list2[i]);
					if (P_1 != null)
					{
						T lPMziJCFRnvinaKeGNwjXRpuzYwf = P_1();
						xsMJBGmRwWlPzYApiztwYCiVQxLi2.lPMziJCFRnvinaKeGNwjXRpuzYwf = lPMziJCFRnvinaKeGNwjXRpuzYwf;
					}
					list.Add(xsMJBGmRwWlPzYApiztwYCiVQxLi2);
					IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)list2[i]] = num;
					if (list2[i] == UpdateLoopType.FixedUpdate)
					{
						fixedUpdateSetIndex = num;
					}
					num++;
				}
			}
			TIBkwaYFfrlorrgEzKrMTlQMftSC = list.ToArray();
			BnPuTevKonSCNieJesLlCkEZXpqL = TIBkwaYFfrlorrgEzKrMTlQMftSC.Length;
			SetUpdateLoop(TIBkwaYFfrlorrgEzKrMTlQMftSC[0].PiEdbgjMHSsksYjRKSGckYZEsfAE);
		}

		public void SetUpdateLoop(UpdateLoopType updateLoop)
		{
			if (VrMfQjbtWyjujWgdTnMyJaEVdoEy != updateLoop)
			{
				VrMfQjbtWyjujWgdTnMyJaEVdoEy = updateLoop;
				dQvTvZqdFSzAQjIiXjyfRtrnHABD = TIBkwaYFfrlorrgEzKrMTlQMftSC[IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)updateLoop]];
			}
		}

		public T Get(int index)
		{
			if (index < 0 || index >= BnPuTevKonSCNieJesLlCkEZXpqL)
			{
				throw new IndexOutOfRangeException();
			}
			return TIBkwaYFfrlorrgEzKrMTlQMftSC[index].lPMziJCFRnvinaKeGNwjXRpuzYwf;
		}

		public T Get(UpdateLoopType updateLoop)
		{
			return TIBkwaYFfrlorrgEzKrMTlQMftSC[IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)updateLoop]].lPMziJCFRnvinaKeGNwjXRpuzYwf;
		}

		public void Set(int index, T item)
		{
			if (index < 0 || index >= BnPuTevKonSCNieJesLlCkEZXpqL)
			{
				throw new IndexOutOfRangeException();
			}
			TIBkwaYFfrlorrgEzKrMTlQMftSC[index].lPMziJCFRnvinaKeGNwjXRpuzYwf = item;
		}

		public UpdateLoopType GetUpdateLoopType(int index)
		{
			if (index < 0 || index >= BnPuTevKonSCNieJesLlCkEZXpqL)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return TIBkwaYFfrlorrgEzKrMTlQMftSC[index].PiEdbgjMHSsksYjRKSGckYZEsfAE;
		}
	}
}
