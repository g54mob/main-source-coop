using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class LockDataSet
	{
		private ILockData[] lockDatas;

		private HeightData HeightData => (HeightData)lockDatas[0];

		public LockDataSet()
		{
			List<ILockData> list = new List<ILockData>
			{
				new HeightData(),
				new TexturesData(),
				new GrassData()
			};
			Type[] allLockTypes = typeof(ILockData).Subtypes();
			int i;
			for (i = 0; i < allLockTypes.Length; i++)
			{
				if (list.FindIndex((ILockData ld) => ld.GetType() == allLockTypes[i]) < 0)
				{
					list.Add(Activator.CreateInstance(allLockTypes[i]) as ILockData);
				}
			}
			lockDatas = list.ToArray();
		}

		public void Read(Terrain terrain, Lock lk)
		{
			for (int i = 0; i < lockDatas.Length; i++)
			{
				lockDatas[i].Read(terrain, lk);
			}
		}

		public static void Resize(LockDataSet src, LockDataSet dst)
		{
			for (int i = 0; i < dst.lockDatas.Length; i++)
			{
				dst.lockDatas[i].ResizeFrom(src.lockDatas[i]);
			}
		}

		public void WriteInThread(IApplyData applyData, bool relativeHeight)
		{
			if (!relativeHeight)
			{
				for (int i = 0; i < lockDatas.Length; i++)
				{
					lockDatas[i].WriteInThread(applyData);
				}
			}
			else if (applyData is HeightOutput200.IApplyHeightData applyData2)
			{
				(Matrix heightSrc, Matrix heightDst) tuple = HeightData.WriteWithHeightDelta(applyData2);
				Matrix item = tuple.heightSrc;
				Matrix item2 = tuple.heightDst;
				for (int j = 1; j < lockDatas.Length; j++)
				{
					lockDatas[j].ApplyHeightDelta(item, item2);
				}
			}
			else
			{
				for (int k = 1; k < lockDatas.Length; k++)
				{
					lockDatas[k].WriteInThread(applyData);
				}
			}
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain)
		{
			for (int i = 0; i < lockDatas.Length; i++)
			{
				lockDatas[i].WriteInApply(terrain, resizeTerrain);
			}
		}
	}
}
