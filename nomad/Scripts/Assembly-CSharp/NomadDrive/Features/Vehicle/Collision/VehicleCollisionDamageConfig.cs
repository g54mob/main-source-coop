using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Collision Damage Config", fileName = "VehicleCollisionDamageConfig")]
	public class VehicleCollisionDamageConfig : ScriptableObject
	{
		[Header("Severity Gate")]
		[Tooltip("Collisions below this severity deal no player damage. Default Medium (Light bumps ignored).")]
		[SerializeField]
		private CollisionSeverity minSeverity = CollisionSeverity.Medium;

		[Header("Force -> Damage Mapping")]
		[Tooltip("Collision impulse (relativeVelocity * mass) at which damage equals minDamage. Roughly the detector's Medium threshold so the smallest qualifying crash yields the floor.")]
		[SerializeField]
		private float minDamageForce = 3000f;

		[Tooltip("Collision impulse at/above which damage equals maxDamage.")]
		[SerializeField]
		private float maxDamageForce = 30000f;

		[Header("Damage Range")]
		[SerializeField]
		[Min(0f)]
		private float minDamage = 5f;

		[SerializeField]
		[Min(0f)]
		private float maxDamage = 40f;

		public CollisionSeverity MinSeverity => minSeverity;

		public float MinDamageForce => minDamageForce;

		public float MaxDamageForce => maxDamageForce;

		public float MinDamage => minDamage;

		public float MaxDamage => maxDamage;
	}
}
