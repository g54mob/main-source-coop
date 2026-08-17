using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.WheelController3D
{
	[Serializable]
	public class Friction
	{
		[Tooltip("    Current force in friction direction.")]
		public float force;

		[Tooltip("    Current slip in friction direction.")]
		public float slip;

		[Range(0f, 2f)]
		[FormerlySerializedAs("forceCoefficient")]
		[Tooltip("    Multiplies the Y value (grip) of the friction graph.\r\n    Formerly known as 'forceCoefficient'.")]
		public float grip = 1f;

		[Range(0f, 2f)]
		[FormerlySerializedAs("slipCoefficient")]
		[Tooltip("    Mutliplies the X value (slip) of the friction graph.\r\n    Formerly known as 'slipCoefficient'.")]
		public float stiffness = 1f;

		[Tooltip("    Speed at the point of contact with the surface.")]
		public float speed;

		public float loadFactor = 1.5f;
	}
}
