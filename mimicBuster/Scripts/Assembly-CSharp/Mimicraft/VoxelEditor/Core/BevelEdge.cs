using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public readonly struct BevelEdge
	{
		public readonly int Axis;

		public readonly Vector3Int Signs;

		public readonly Vector3Int Cell;

		public readonly int Length;

		public int Start => Cell[Axis];

		public int End => Cell[Axis] + Length - 1;

		public BevelEdge(int axis, Vector3Int signs, Vector3Int cell, int length)
		{
			Axis = axis;
			Signs = signs;
			Cell = cell;
			Length = length;
		}

		public void GetLine(out Vector3 a, out Vector3 b)
		{
			a = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				if (i != Axis)
				{
					a[i] = ((Signs[i] > 0) ? ((float)Cell[i] + 1f) : ((float)Cell[i]));
				}
			}
			b = a;
			a[Axis] = Start;
			b[Axis] = (float)End + 1f;
		}
	}
}
