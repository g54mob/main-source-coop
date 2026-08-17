using System;
using UnityEngine;

namespace NWH.WheelController3D
{
	[Serializable]
	public class Spring
	{
		[Tooltip("    How much is spring currently compressed. 0 means fully relaxed and 1 fully compressed.")]
		public float compression;

		[Tooltip("    Current force the spring is exerting in [N].")]
		public float force;

		[Tooltip("Force curve where X axis represents spring travel [0,1] and Y axis represents force coefficient [0, 1].\r\nForce coefficient is multiplied by maxForce to get the final spring force.")]
		public AnimationCurve forceCurve;

		[Tooltip("    Current length of the spring.")]
		public float length;

		[Tooltip("    Maximum force spring can exert.")]
		public float maxForce = 16000f;

		[Tooltip("    Length of fully relaxed spring.")]
		public float maxLength = 0.35f;

		[Tooltip("    Length of the spring during the previous physics update.")]
		public float prevLength;

		[Tooltip("    Rate of change of the length of the spring in [m/s].")]
		public float compressionVelocity;
	}
}
