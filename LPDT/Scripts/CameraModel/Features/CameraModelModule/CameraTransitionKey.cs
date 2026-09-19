using System;
using UnityEngine;

namespace Features.CameraModelModule
{
	[Serializable]
	public struct CameraTransitionKey : IEquatable<CameraTransitionKey>
	{
		[SerializeField]
		private CameraType _from;

		[SerializeField]
		private CameraType _to;

		public CameraType From => _from;

		public CameraType To => _to;

		public CameraTransitionKey(CameraType from, CameraType to)
		{
			_from = from;
			_to = to;
		}

		public bool Equals(CameraTransitionKey other)
		{
			if (_from == other._from)
			{
				return _to == other._to;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is CameraTransitionKey other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((int)_from * 397) ^ (int)_to;
		}

		public static bool operator ==(CameraTransitionKey left, CameraTransitionKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CameraTransitionKey left, CameraTransitionKey right)
		{
			return !left.Equals(right);
		}
	}
}
