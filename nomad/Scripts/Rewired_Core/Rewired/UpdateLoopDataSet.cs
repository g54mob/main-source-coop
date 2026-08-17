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
		private class LekOuUGrNiJQymRIUarezlhdIICX
		{
			public readonly UpdateLoopType qWvsBwAgpWFHFQDHilXitMVMfnpQ;

			public T DhLNpdtvprrJQTnOyPIjVauQOoVE;

			public LekOuUGrNiJQymRIUarezlhdIICX(UpdateLoopType P_0)
			{
				qWvsBwAgpWFHFQDHilXitMVMfnpQ = P_0;
			}
		}

		private const int ccFQevmePyCwlJqdvqkQxxcEorFu = 0;

		private LekOuUGrNiJQymRIUarezlhdIICX GRaFNUaWxMmxGQoyCmXXnFnEEFTF;

		private int APRHJToUEnLLBxiejYuIrMADiRYj;

		public readonly int fixedUpdateSetIndex = -1;

		private readonly int[] VKXbBKrXBBuxntoKGeGqZeVkxOQC;

		private readonly LekOuUGrNiJQymRIUarezlhdIICX[] CVVIdhjiljNjkINztoItWqxRyQgA;

		private UpdateLoopType ehGUNnaRKCFyiApIpnKSLgBlFxmP = (UpdateLoopType)(-1);

		public T Current => GRaFNUaWxMmxGQoyCmXXnFnEEFTF.DhLNpdtvprrJQTnOyPIjVauQOoVE;

		public int Count => APRHJToUEnLLBxiejYuIrMADiRYj;

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= APRHJToUEnLLBxiejYuIrMADiRYj)
				{
					throw new IndexOutOfRangeException();
				}
				return CVVIdhjiljNjkINztoItWqxRyQgA[index].DhLNpdtvprrJQTnOyPIjVauQOoVE;
			}
			set
			{
				if (index < 0 || index >= APRHJToUEnLLBxiejYuIrMADiRYj)
				{
					throw new IndexOutOfRangeException();
				}
				CVVIdhjiljNjkINztoItWqxRyQgA[index].DhLNpdtvprrJQTnOyPIjVauQOoVE = value;
			}
		}

		public UpdateLoopDataSet(UpdateLoopSetting P_0)
			: this(P_0, (Func<T>)null)
		{
		}

		public UpdateLoopDataSet(UpdateLoopSetting P_0, Func<T> P_1)
		{
			VKXbBKrXBBuxntoKGeGqZeVkxOQC = new int[3];
			ArrayTools.Fill(VKXbBKrXBBuxntoKGeGqZeVkxOQC, -1);
			List<LekOuUGrNiJQymRIUarezlhdIICX> list = new List<LekOuUGrNiJQymRIUarezlhdIICX>();
			int num = 0;
			using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
			{
				List<UpdateLoopType> list2 = tList.list;
				EnumConverter.ToUpdateLoopTypes(P_0, list2);
				for (int i = 0; i < list2.Count; i++)
				{
					LekOuUGrNiJQymRIUarezlhdIICX lekOuUGrNiJQymRIUarezlhdIICX = new LekOuUGrNiJQymRIUarezlhdIICX(list2[i]);
					if (P_1 != null)
					{
						T dhLNpdtvprrJQTnOyPIjVauQOoVE = P_1();
						lekOuUGrNiJQymRIUarezlhdIICX.DhLNpdtvprrJQTnOyPIjVauQOoVE = dhLNpdtvprrJQTnOyPIjVauQOoVE;
					}
					list.Add(lekOuUGrNiJQymRIUarezlhdIICX);
					VKXbBKrXBBuxntoKGeGqZeVkxOQC[(int)list2[i]] = num;
					if (list2[i] == UpdateLoopType.FixedUpdate)
					{
						fixedUpdateSetIndex = num;
					}
					num++;
				}
			}
			CVVIdhjiljNjkINztoItWqxRyQgA = list.ToArray();
			APRHJToUEnLLBxiejYuIrMADiRYj = CVVIdhjiljNjkINztoItWqxRyQgA.Length;
			SetUpdateLoop(CVVIdhjiljNjkINztoItWqxRyQgA[0].qWvsBwAgpWFHFQDHilXitMVMfnpQ);
		}

		public void SetUpdateLoop(UpdateLoopType updateLoop)
		{
			if (ehGUNnaRKCFyiApIpnKSLgBlFxmP != updateLoop)
			{
				ehGUNnaRKCFyiApIpnKSLgBlFxmP = updateLoop;
				GRaFNUaWxMmxGQoyCmXXnFnEEFTF = CVVIdhjiljNjkINztoItWqxRyQgA[VKXbBKrXBBuxntoKGeGqZeVkxOQC[(int)updateLoop]];
			}
		}

		public T Get(int index)
		{
			if (index < 0 || index >= APRHJToUEnLLBxiejYuIrMADiRYj)
			{
				throw new IndexOutOfRangeException();
			}
			return CVVIdhjiljNjkINztoItWqxRyQgA[index].DhLNpdtvprrJQTnOyPIjVauQOoVE;
		}

		public T Get(UpdateLoopType updateLoop)
		{
			return CVVIdhjiljNjkINztoItWqxRyQgA[VKXbBKrXBBuxntoKGeGqZeVkxOQC[(int)updateLoop]].DhLNpdtvprrJQTnOyPIjVauQOoVE;
		}

		public void Set(int index, T item)
		{
			if (index < 0 || index >= APRHJToUEnLLBxiejYuIrMADiRYj)
			{
				throw new IndexOutOfRangeException();
			}
			CVVIdhjiljNjkINztoItWqxRyQgA[index].DhLNpdtvprrJQTnOyPIjVauQOoVE = item;
		}

		public UpdateLoopType GetUpdateLoopType(int index)
		{
			if (index < 0 || index >= APRHJToUEnLLBxiejYuIrMADiRYj)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return CVVIdhjiljNjkINztoItWqxRyQgA[index].qWvsBwAgpWFHFQDHilXitMVMfnpQ;
		}
	}
}
