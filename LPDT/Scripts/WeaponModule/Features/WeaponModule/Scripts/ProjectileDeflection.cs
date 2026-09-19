using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	public readonly struct ProjectileDeflection
	{
		public Vector3 Point { get; }

		public Vector3 Normal { get; }

		public Transform DeflectorRoot { get; }

		public Transform ShieldedRoot { get; }

		public ProjectileDeflection(Vector3 point, Vector3 normal, Transform deflectorRoot, Transform shieldedRoot)
		{
			Point = point;
			Normal = normal;
			DeflectorRoot = deflectorRoot;
			ShieldedRoot = shieldedRoot;
		}
	}
}
