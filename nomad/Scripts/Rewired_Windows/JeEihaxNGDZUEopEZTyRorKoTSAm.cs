using System;
using System.Runtime.CompilerServices;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class JeEihaxNGDZUEopEZTyRorKoTSAm : LDJGvqLnFydDhJMnXduxzIERUQI
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class TouchpadInfo
	{
		public int maxTouches;

		public int minX;

		public int maxX;

		public int minY;

		public int maxY;

		public bool invertY;

		public bool reverseY;

		public TouchpadInfo(int P_0, int P_1, int P_2, int P_3, int P_4, bool P_5, bool P_6)
		{
			maxTouches = P_0;
			minX = P_1;
			maxX = P_2;
			minY = P_3;
			maxY = P_4;
			invertY = P_5;
			reverseY = P_6;
		}

		public void CalculateTouch(ref TouchData data)
		{
			int num = (reverseY ? (maxY - data.positionRawY) : data.positionRawY);
			data.positionX = MathTools.ValueInNewRange(data.positionRawX, minX, maxX, 0f, 1f);
			data.positionY = MathTools.ValueInNewRange(num, minY, maxY, 0f, 1f);
			data.positionAbsX = data.positionRawX;
			data.positionAbsY = num;
			if (data.positionAbsX > maxX)
			{
				data.positionAbsX = maxX;
			}
			if (data.positionAbsY > maxY)
			{
				data.positionAbsY = maxY;
			}
			if (data.positionAbsX < minX)
			{
				data.positionAbsX = minX;
			}
			if (data.positionAbsY < minY)
			{
				data.positionAbsY = minY;
			}
			if (invertY)
			{
				data.positionY *= -1f;
				data.positionAbsY *= -1;
			}
		}
	}

	private class qnqnBENQsueuVjCMANsseYVRDukF
	{
		public readonly TouchData[] lVrEZyFkDOrMMaHibWSQgPzImlTjB;

		public qnqnBENQsueuVjCMANsseYVRDukF(int P_0)
		{
			lVrEZyFkDOrMMaHibWSQgPzImlTjB = new TouchData[P_0];
		}
	}

	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal struct TouchData
	{
		public int touchId;

		public float timeStamp;

		public bool isTouching;

		public int positionRawX;

		public int positionRawY;

		public float positionX;

		public float positionY;

		public int positionAbsX;

		public int positionAbsY;

		public void Clear()
		{
			touchId = -1;
			timeStamp = 0f;
			isTouching = false;
			positionRawX = 0;
			positionRawY = 0;
			positionX = 0f;
			positionY = 0f;
			positionAbsX = 0;
			positionAbsY = 0;
		}
	}

	private TouchpadInfo JDcnvbQochKJRhGEuCkGvgAAFPbkA;

	private RingBuffer<qnqnBENQsueuVjCMANsseYVRDukF> lxoyIBWHFvbvBCYdsYIGeXdnZKEpA;

	private TouchData[] zMTrrOTAvxpsnVMBZczfQcVHKHMl;

	private Action<NativeBuffer, TouchData[]> mcMQFZhMgmbiztIcmodssTKzcNjH;

	public TouchData[] SBWbRIEBtbRxLkclWCpSvIwxSXTqA;

	private ObjectPool<qnqnBENQsueuVjCMANsseYVRDukF> ECyfhLoPeKaRIFnTzqvWBZLGtvkdA;

	public JeEihaxNGDZUEopEZTyRorKoTSAm(byte P_0, TouchpadInfo P_1, HIDInfo P_2, int P_3, Action<NativeBuffer, TouchData[]> P_4)
		: base(P_0, P_2)
	{
		JDcnvbQochKJRhGEuCkGvgAAFPbkA = P_1;
		mcMQFZhMgmbiztIcmodssTKzcNjH = P_4;
		lxoyIBWHFvbvBCYdsYIGeXdnZKEpA = new RingBuffer<qnqnBENQsueuVjCMANsseYVRDukF>(P_3);
		zMTrrOTAvxpsnVMBZczfQcVHKHMl = new TouchData[P_1.maxTouches];
		SBWbRIEBtbRxLkclWCpSvIwxSXTqA = new TouchData[P_1.maxTouches];
		for (int i = 0; i < SBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
		{
			SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].Clear();
		}
		ECyfhLoPeKaRIFnTzqvWBZLGtvkdA = new ObjectPool<qnqnBENQsueuVjCMANsseYVRDukF>(P_3, () => new qnqnBENQsueuVjCMANsseYVRDukF(JDcnvbQochKJRhGEuCkGvgAAFPbkA.maxTouches));
	}

	public virtual void wrPNEspjQKooBELAgfEcqcHikWFh(NativeBuffer P_0, double P_1)
	{
		if (mcMQFZhMgmbiztIcmodssTKzcNjH == null)
		{
			return;
		}
		mcMQFZhMgmbiztIcmodssTKzcNjH(P_0, zMTrrOTAvxpsnVMBZczfQcVHKHMl);
		lock (lxoyIBWHFvbvBCYdsYIGeXdnZKEpA)
		{
			qnqnBENQsueuVjCMANsseYVRDukF qnqnBENQsueuVjCMANsseYVRDukF2 = ECyfhLoPeKaRIFnTzqvWBZLGtvkdA.Get();
			for (int i = 0; i < JDcnvbQochKJRhGEuCkGvgAAFPbkA.maxTouches; i++)
			{
				qnqnBENQsueuVjCMANsseYVRDukF2.lVrEZyFkDOrMMaHibWSQgPzImlTjB[i] = zMTrrOTAvxpsnVMBZczfQcVHKHMl[i];
			}
			CollectionTools.Enqueue(ECyfhLoPeKaRIFnTzqvWBZLGtvkdA, lxoyIBWHFvbvBCYdsYIGeXdnZKEpA, qnqnBENQsueuVjCMANsseYVRDukF2, out var _);
		}
		JbICxyWtOOdbRnaBTgCpTJIrvgyU();
	}

	public void JbICxyWtOOdbRnaBTgCpTJIrvgyU()
	{
		for (int i = 0; i < SBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
		{
			SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].Clear();
		}
		lock (lxoyIBWHFvbvBCYdsYIGeXdnZKEpA)
		{
			int num = lxoyIBWHFvbvBCYdsYIGeXdnZKEpA.Count;
			while (num > 0)
			{
				qnqnBENQsueuVjCMANsseYVRDukF qnqnBENQsueuVjCMANsseYVRDukF2 = lxoyIBWHFvbvBCYdsYIGeXdnZKEpA.Dequeue();
				num--;
				for (int j = 0; j < qnqnBENQsueuVjCMANsseYVRDukF2.lVrEZyFkDOrMMaHibWSQgPzImlTjB.Length; j++)
				{
					JDcnvbQochKJRhGEuCkGvgAAFPbkA.CalculateTouch(ref qnqnBENQsueuVjCMANsseYVRDukF2.lVrEZyFkDOrMMaHibWSQgPzImlTjB[j]);
					SBWbRIEBtbRxLkclWCpSvIwxSXTqA[j] = qnqnBENQsueuVjCMANsseYVRDukF2.lVrEZyFkDOrMMaHibWSQgPzImlTjB[j];
				}
				ECyfhLoPeKaRIFnTzqvWBZLGtvkdA.Return(qnqnBENQsueuVjCMANsseYVRDukF2);
			}
		}
	}

	public bool VRQDYrjowDqtUNGMQEXSGOOHLRDj(int P_0)
	{
		for (int i = 0; i < SBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
		{
			if (SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching && SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].touchId == P_0)
			{
				return true;
			}
		}
		return false;
	}

	[CompilerGenerated]
	private qnqnBENQsueuVjCMANsseYVRDukF SewqPgkjBHDDJlVmHfxeuHUrYpyG()
	{
		return new qnqnBENQsueuVjCMANsseYVRDukF(JDcnvbQochKJRhGEuCkGvgAAFPbkA.maxTouches);
	}
}
