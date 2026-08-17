using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Utils;

internal abstract class hbZPQLtaSNORhjIfKDnNgXuOkWyd : LDJGvqLnFydDhJMnXduxzIERUQI
{
	internal abstract class mdJxcuSvBwFnERIGiHmufaVmYVwHA
	{
		private int vnVvQKwBlRifeGBYFhckdsayEobhA;

		private int[] MUEVziaFtgOOyhDypGyKnzQxZUnK;

		protected CvBbYwvZfLeRTAPLFSXaxjnvmVmC[] OdbrPCRFWXQUboXWVEJvvNuudOAY;

		public CvBbYwvZfLeRTAPLFSXaxjnvmVmC QXQwwaATLYfidsLWJWzyoKftgSWJ;

		private int buiUmDriPOwZuhwquctlMCmcyQhI;

		private int QhXWwRvbneRqKEtgBnLANhxjlVKf = -1;

		private bool OOncTEcYWqsnepcEGoNwtqJfrvHsA;

		public UpdateLoopType RPVUBzqAvDpisbkalcYLBiMnbMLk
		{
			set
			{
				if (QhXWwRvbneRqKEtgBnLANhxjlVKf != (int)updateLoopType)
				{
					QhXWwRvbneRqKEtgBnLANhxjlVKf = (int)updateLoopType;
					buiUmDriPOwZuhwquctlMCmcyQhI = MUEVziaFtgOOyhDypGyKnzQxZUnK[(int)updateLoopType];
					QXQwwaATLYfidsLWJWzyoKftgSWJ = OdbrPCRFWXQUboXWVEJvvNuudOAY[buiUmDriPOwZuhwquctlMCmcyQhI];
				}
			}
		}

		public mdJxcuSvBwFnERIGiHmufaVmYVwHA()
		{
		}

		public void RrfyasJXRbuSVOUXSIgFqrLRnnFl(UpdateLoopSetting P_0, Func<UpdateLoopType, CvBbYwvZfLeRTAPLFSXaxjnvmVmC> P_1)
		{
			if (OOncTEcYWqsnepcEGoNwtqJfrvHsA)
			{
				Logger.LogError("Already initialized!");
				return;
			}
			MUEVziaFtgOOyhDypGyKnzQxZUnK = new int[3];
			vnVvQKwBlRifeGBYFhckdsayEobhA = 0;
			List<CvBbYwvZfLeRTAPLFSXaxjnvmVmC> list = new List<CvBbYwvZfLeRTAPLFSXaxjnvmVmC>();
			using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
			{
				List<UpdateLoopType> list2 = tList.list;
				EnumConverter.ToUpdateLoopTypes(P_0, list2);
				for (int i = 0; i < list2.Count; i++)
				{
					MUEVziaFtgOOyhDypGyKnzQxZUnK[(int)list2[i]] = vnVvQKwBlRifeGBYFhckdsayEobhA;
					vnVvQKwBlRifeGBYFhckdsayEobhA++;
					list.Add(P_1(list2[i]));
				}
			}
			OdbrPCRFWXQUboXWVEJvvNuudOAY = list.ToArray();
			QXQwwaATLYfidsLWJWzyoKftgSWJ = OdbrPCRFWXQUboXWVEJvvNuudOAY[0];
			OOncTEcYWqsnepcEGoNwtqJfrvHsA = true;
		}

		public virtual void KSeWPXQBXetJhSAfPModLxzjazzT(UpdateLoopType P_0)
		{
			if (QhXWwRvbneRqKEtgBnLANhxjlVKf != (int)P_0)
			{
				RPVUBzqAvDpisbkalcYLBiMnbMLk = P_0;
			}
		}
	}

	internal abstract class CvBbYwvZfLeRTAPLFSXaxjnvmVmC
	{
		public readonly UpdateLoopType mwRLHmzbRCZUZWvJreBbjZakuccv;

		public CvBbYwvZfLeRTAPLFSXaxjnvmVmC(UpdateLoopType P_0)
		{
			mwRLHmzbRCZUZWvJreBbjZakuccv = P_0;
		}
	}

	internal mdJxcuSvBwFnERIGiHmufaVmYVwHA wJDNgHtDIaBpUwHjXVHLEHnCDlvIA;

	public hbZPQLtaSNORhjIfKDnNgXuOkWyd(mdJxcuSvBwFnERIGiHmufaVmYVwHA P_0, byte P_1, HIDInfo P_2)
		: base(P_1, P_2)
	{
		wJDNgHtDIaBpUwHjXVHLEHnCDlvIA = P_0;
	}

	public virtual void DjOlFuhncTcNPdaRgDUKOzAnZaDl(UpdateLoopType P_0)
	{
		if (wJDNgHtDIaBpUwHjXVHLEHnCDlvIA != null)
		{
			wJDNgHtDIaBpUwHjXVHLEHnCDlvIA.KSeWPXQBXetJhSAfPModLxzjazzT(P_0);
		}
	}
}
