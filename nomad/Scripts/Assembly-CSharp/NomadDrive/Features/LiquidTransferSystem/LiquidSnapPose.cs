using System;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[Serializable]
	public struct LiquidSnapPose
	{
		[Tooltip("Position offset in the target FillAnchor's local space.")]
		public Vector3 positionOffset;

		[Tooltip("Rotation offset (Euler) relative to the target FillAnchor.")]
		public Vector3 rotationOffset;
	}
}
