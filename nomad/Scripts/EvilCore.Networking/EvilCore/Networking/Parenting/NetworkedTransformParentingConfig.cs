using System;
using UnityEngine;

namespace EvilCore.Networking.Parenting
{
	[Serializable]
	public struct NetworkedTransformParentingConfig
	{
		[Tooltip("Position offset from parent transform when parenting occurs")]
		public Vector3 PositionOffset;

		[Tooltip("Rotation offset (in Euler angles) from parent transform when parenting occurs")]
		public Vector3 RotationOffset;

		[Tooltip("If enabled, child's position axes will be locked and not follow parent's position")]
		public bool KeepPositionAxes;

		[Tooltip("If enabled, child's rotation axes will be locked and not follow parent's rotation")]
		public bool KeepRotationAxes;

		private string Summary => ("Follows: " + ((!KeepPositionAxes) ? "Position" : "") + " " + ((!KeepRotationAxes) ? "Rotation" : "")).Trim();
	}
}
