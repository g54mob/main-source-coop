using System;
using UnityEngine.Serialization;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[Serializable]
	public struct LiquidSnapEntry
	{
		[FormerlySerializedAs("capType")]
		public LiquidSnapTargetType targetType;

		public LiquidSnapPose pose;
	}
}
