using System;
using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	[Serializable]
	public struct PhysicsResolutionSettings
	{
		public int SolverIterations;

		public int SolverVelocityIterations;

		public CollisionDetectionMode CollisionDetectionMode;

		public float MaxDepenetrationVelocity;
	}
}
