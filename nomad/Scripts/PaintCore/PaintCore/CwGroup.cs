using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwGroup
	{
		[SerializeField]
		private int index;

		public CwGroup(int newIndex)
		{
			index = newIndex;
		}

		public static implicit operator int(CwGroup group)
		{
			return group.index;
		}

		public static implicit operator CwGroup(int index)
		{
			return new CwGroup(index);
		}

		public override string ToString()
		{
			return index.ToString();
		}
	}
}
