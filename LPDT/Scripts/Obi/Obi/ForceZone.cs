using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public struct ForceZone
	{
		public enum ForceMode
		{
			Force = 0,
			Acceleration = 1,
			Wind = 2
		}

		public enum ZoneType
		{
			Directional = 0,
			Radial = 1,
			Vortex = 2,
			Void = 3
		}

		public enum DampingDirection
		{
			All = 0,
			ForceDirection = 1,
			SurfaceDirection = 2
		}

		public Color color;

		public ZoneType type;

		public ForceMode mode;

		public DampingDirection dampingDir;

		public float intensity;

		public float minDistance;

		public float maxDistance;

		public float falloffPower;

		public float damping;
	}
}
