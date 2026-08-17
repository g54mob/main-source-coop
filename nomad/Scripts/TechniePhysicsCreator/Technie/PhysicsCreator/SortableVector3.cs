using System;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	public class SortableVector3 : IComparable<SortableVector3>
	{
		public Vector3 value;

		public float sortValue;

		public SortableVector3(Vector3 v, float s)
		{
			value = v;
			sortValue = s;
		}

		public int CompareTo(SortableVector3 other)
		{
			if (sortValue > other.sortValue)
			{
				return 1;
			}
			if (sortValue < other.sortValue)
			{
				return -1;
			}
			return 0;
		}
	}
}
