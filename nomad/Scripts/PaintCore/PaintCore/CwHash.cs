using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwHash
	{
		[SerializeField]
		private int v;

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return v.GetHashCode();
		}

		public CwHash(int newValue)
		{
			v = newValue;
		}

		public static implicit operator int(CwHash hash)
		{
			return hash.v;
		}

		public static implicit operator CwHash(int index)
		{
			return new CwHash(index);
		}

		public override string ToString()
		{
			return v.ToString();
		}
	}
}
