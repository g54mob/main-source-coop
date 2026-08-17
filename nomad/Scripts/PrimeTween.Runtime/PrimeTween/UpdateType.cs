using System;
using UnityEngine;

namespace PrimeTween
{
	[Serializable]
	public struct UpdateType : IEquatable<UpdateType>
	{
		public static readonly UpdateType Default = new UpdateType(_UpdateType.Default);

		public static readonly UpdateType Update = new UpdateType(_UpdateType.Update);

		public static readonly UpdateType LateUpdate = new UpdateType(_UpdateType.LateUpdate);

		public static readonly UpdateType FixedUpdate = new UpdateType(_UpdateType.FixedUpdate);

		[SerializeField]
		internal _UpdateType enumValue;

		internal UpdateType(_UpdateType enumValue)
		{
			this.enumValue = enumValue;
		}

		public static bool operator ==(UpdateType lhs, UpdateType rhs)
		{
			return lhs.enumValue == rhs.enumValue;
		}

		public bool Equals(UpdateType other)
		{
			return enumValue == other.enumValue;
		}

		public override bool Equals(object obj)
		{
			if (obj is UpdateType other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = (int)enumValue;
			return num.GetHashCode();
		}
	}
}
