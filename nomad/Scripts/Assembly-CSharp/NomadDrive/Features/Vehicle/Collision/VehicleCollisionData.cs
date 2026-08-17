using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	public struct VehicleCollisionData
	{
		public Vector3 ImpactPoint;

		public Vector3 ImpactNormal;

		public float Force;

		public Vector3 RelativeVelocity;

		public float RelativeSpeed;

		public CollisionSeverity Severity;

		public uint CollidedObjectNetId;
	}
}
