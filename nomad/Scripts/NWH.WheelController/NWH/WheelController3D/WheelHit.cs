using System;
using UnityEngine;

namespace NWH.WheelController3D
{
	[Serializable]
	public struct WheelHit
	{
		[Tooltip("Collider that was hit. If no hit, null.")]
		public Collider collider;

		[Tooltip("    The normal at the point of contact")]
		public Vector3 normal;

		[Tooltip("    The point of contact between the wheel and the ground.")]
		public Vector3 point;
	}
}
