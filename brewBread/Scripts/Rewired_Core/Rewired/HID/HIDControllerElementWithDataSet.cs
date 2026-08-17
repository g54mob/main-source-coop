using System;
using System.Collections.Generic;
using Rewired.Config;
using Rewired.Utils;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal abstract class HIDControllerElementWithDataSet : HIDControllerElement
	{
		internal abstract class kGaQcrZjSXfDykZKCSLIIAsSOyoLA
		{
			private int xKvhZZMrJuAqapnMzSSUIIvPeizR;

			private int[] IKaHcMMMReldkZgFUUrjWQfbHzQi;

			protected OlubodalIgGxzKDJlbCWXQONFDog[] ReyFXLxUNcPvXNGMfpODOqFiLRfR;

			public OlubodalIgGxzKDJlbCWXQONFDog SgJVPHrjFyHgCSEpxtgUijWhNNop;

			private int aVOlNuRVvCOJdZFGeiVVgfpKNgOsA;

			private int yVrZIXjSxHcfgFdpNDzhuqBYIrTH = -1;

			private bool jYTNgflwwEgvgbZuuTHYnPAnuRao;

			protected int rDwgYRBSkOPRjVvObsjyXpnizJLT => xKvhZZMrJuAqapnMzSSUIIvPeizR;

			protected int[] hOdpiFRUiFyATBokejgRyDMQCvoc => IKaHcMMMReldkZgFUUrjWQfbHzQi;

			public UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE
			{
				set
				{
					if (yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)updateLoopType)
					{
						yVrZIXjSxHcfgFdpNDzhuqBYIrTH = (int)updateLoopType;
						aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)updateLoopType];
						SgJVPHrjFyHgCSEpxtgUijWhNNop = ReyFXLxUNcPvXNGMfpODOqFiLRfR[aVOlNuRVvCOJdZFGeiVVgfpKNgOsA];
					}
				}
			}

			public kGaQcrZjSXfDykZKCSLIIAsSOyoLA()
			{
			}

			public void tZBbwtEuDoJAugBSKQuoLXJXFmdBA(UpdateLoopSetting P_0, Func<UpdateLoopType, OlubodalIgGxzKDJlbCWXQONFDog> P_1)
			{
				if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
				{
					Logger.LogError("Already initialized!");
					return;
				}
				IKaHcMMMReldkZgFUUrjWQfbHzQi = new int[3];
				xKvhZZMrJuAqapnMzSSUIIvPeizR = 0;
				List<OlubodalIgGxzKDJlbCWXQONFDog> list = new List<OlubodalIgGxzKDJlbCWXQONFDog>();
				using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
				{
					List<UpdateLoopType> list2 = tList.list;
					EnumConverter.ToUpdateLoopTypes(P_0, list2);
					for (int i = 0; i < list2.Count; i++)
					{
						IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)list2[i]] = xKvhZZMrJuAqapnMzSSUIIvPeizR;
						xKvhZZMrJuAqapnMzSSUIIvPeizR++;
						list.Add(P_1(list2[i]));
					}
				}
				ReyFXLxUNcPvXNGMfpODOqFiLRfR = list.ToArray();
				SgJVPHrjFyHgCSEpxtgUijWhNNop = ReyFXLxUNcPvXNGMfpODOqFiLRfR[0];
				jYTNgflwwEgvgbZuuTHYnPAnuRao = true;
			}

			private void QDxWNKcwdDhGZqeZuGVQgCpodYzY(UpdateLoopType P_0, OlubodalIgGxzKDJlbCWXQONFDog P_1)
			{
				ReyFXLxUNcPvXNGMfpODOqFiLRfR[IKaHcMMMReldkZgFUUrjWQfbHzQi[(int)P_0]] = P_1;
			}

			public virtual void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
			{
				if (yVrZIXjSxHcfgFdpNDzhuqBYIrTH != (int)P_0)
				{
					PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
				}
			}

			public void jpwugzufXqktYbXkMYboQpqCbQgL()
			{
				for (int i = 0; i < xKvhZZMrJuAqapnMzSSUIIvPeizR; i++)
				{
					ReyFXLxUNcPvXNGMfpODOqFiLRfR[i].jpwugzufXqktYbXkMYboQpqCbQgL();
				}
			}
		}

		internal abstract class OlubodalIgGxzKDJlbCWXQONFDog
		{
			public readonly UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

			public OlubodalIgGxzKDJlbCWXQONFDog(UpdateLoopType P_0)
			{
				PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
			}

			public abstract void jpwugzufXqktYbXkMYboQpqCbQgL();
		}

		internal kGaQcrZjSXfDykZKCSLIIAsSOyoLA dataSet;

		public HIDControllerElementWithDataSet(kGaQcrZjSXfDykZKCSLIIAsSOyoLA P_0, byte P_1, HIDInfo P_2)
			: base(P_1, P_2)
		{
			dataSet = P_0;
		}

		public virtual void Update(UpdateLoopType updateLoop)
		{
			if (dataSet != null)
			{
				dataSet.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(updateLoop);
			}
		}
	}
}
