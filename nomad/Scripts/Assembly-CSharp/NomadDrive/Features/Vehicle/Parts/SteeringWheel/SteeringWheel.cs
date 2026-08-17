using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Enums;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.SteeringWheel
{
	public class SteeringWheel : AttachableObject
	{
		public Transform steeringWheelPivotTransform;

		public float steeringWheelAngle = 45f;

		[SerializeField]
		private ControlRotationAxis rotationAxis = ControlRotationAxis.Z;

		public SteeringWheelSlot SteeringWheelSlot { get; set; }

		[Header("Hand Rigs")]
		[field: SerializeField]
		public Transform LeftHandRig { get; private set; }

		[field: SerializeField]
		public Transform RightHandRig { get; private set; }

		public ControlRotationAxis RotationAxis => rotationAxis;

		public override bool Weaved()
		{
			return true;
		}
	}
}
