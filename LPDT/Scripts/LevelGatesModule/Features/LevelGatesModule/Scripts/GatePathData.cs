using UnityEngine;

namespace Features.LevelGatesModule.Scripts
{
	public struct GatePathData
	{
		public IGate PathGate;

		public Vector3 IntersectionStart;

		public static bool operator ==(GatePathData lhs, GatePathData rhs)
		{
			return lhs.PathGate == rhs.PathGate;
		}

		public static bool operator !=(GatePathData lhs, GatePathData rhs)
		{
			return !(lhs == rhs);
		}
	}
}
