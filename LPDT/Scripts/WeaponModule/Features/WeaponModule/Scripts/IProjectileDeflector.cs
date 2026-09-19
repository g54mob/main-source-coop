using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	public interface IProjectileDeflector
	{
		ProjectileHitResponse ResolveHit(Collider hitCollider, Vector3 travelPoint, Vector3 travelDirection, float travelBack, float projectileRadius, out ProjectileDeflection deflection);
	}
}
