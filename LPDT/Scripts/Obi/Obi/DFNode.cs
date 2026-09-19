using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public struct DFNode
	{
		public Vector4 distancesA;

		public Vector4 distancesB;

		public Vector4 center;

		public int firstChild;

		private int pad0;

		private int pad1;

		private int pad2;

		public DFNode(Vector4 center)
		{
			distancesA = Vector4.zero;
			distancesB = Vector4.zero;
			this.center = center;
			firstChild = -1;
			pad0 = 0;
			pad1 = 0;
			pad2 = 0;
		}

		public float Sample(Vector3 position)
		{
			Vector3 normalizedPos = GetNormalizedPos(position);
			Vector4 vector = distancesA + (distancesB - distancesA) * normalizedPos[0];
			float num = vector[0] + (vector[2] - vector[0]) * normalizedPos[1];
			float num2 = vector[1] + (vector[3] - vector[1]) * normalizedPos[1];
			return num + (num2 - num) * normalizedPos[2];
		}

		public Vector3 GetNormalizedPos(Vector3 position)
		{
			float num = center[3] * 2f;
			return new Vector3((position[0] - (center[0] - center[3])) / num, (position[1] - (center[1] - center[3])) / num, (position[2] - (center[2] - center[3])) / num);
		}

		public int GetOctant(Vector3 position)
		{
			int num = 0;
			if (position[0] > center[0])
			{
				num |= 4;
			}
			if (position[1] > center[1])
			{
				num |= 2;
			}
			if (position[2] > center[2])
			{
				num |= 1;
			}
			return num;
		}
	}
}
