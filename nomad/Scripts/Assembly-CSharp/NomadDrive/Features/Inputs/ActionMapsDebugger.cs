using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Inputs
{
	public class ActionMapsDebugger : MonoBehaviour
	{
		[SerializeField]
		private float lookHorizontal;

		[SerializeField]
		private float lookVertical;

		[SerializeField]
		private bool isCameraZoomButtonDown;

		[SerializeField]
		private bool isCameraZoomButtonUp;

		[SerializeField]
		private bool isPrimaryInteractionButtonDown;

		[SerializeField]
		private bool isPrimaryInteractionButtonUp;

		[SerializeField]
		private bool isSecondaryInteractionButtonDown;

		[SerializeField]
		private bool isSecondaryInteractionButtonUp;

		[SerializeField]
		private float moveHorizontal;

		[SerializeField]
		private float moveVertical;

		[SerializeField]
		private bool isSprintButton;

		[SerializeField]
		private bool isJumpButtonDown;

		[SerializeField]
		private bool isJumpButtonUp;

		[SerializeField]
		private bool isCrouchButtonDown;

		[SerializeField]
		private bool isCrouchButtonUp;

		[SerializeField]
		private bool isStandupButton;

		[SerializeField]
		private bool isStandupButtonDown;

		[SerializeField]
		private float throttle;

		[SerializeField]
		private float brakes;

		[SerializeField]
		private float steering;

		[SerializeField]
		private float handbrake;

		[SerializeField]
		private bool isHornButton;

		[SerializeField]
		private bool rotateObjectButton;

		[SerializeField]
		private bool isFinishObjectPlacingButtonDown;

		[SerializeField]
		private float moveObjectHorizontal;

		[SerializeField]
		private float moveObjectVertical;

		[SerializeField]
		private float moveObjectDirect;

		[FormerlySerializedAs("isUseObjectButtonDown")]
		[SerializeField]
		private bool isPrimaryUseObjectButtonDown;

		[SerializeField]
		private bool isDropObjectButtonDown;

		[SerializeField]
		private bool isThrowObjectButtonDown;

		[SerializeField]
		private bool isThrowObjectButtonUp;

		[SerializeField]
		private bool isEnterPlacementModeButtonDown;

		private void Update()
		{
			lookHorizontal = BaseInputs.GetLookHorizontal();
			lookVertical = BaseInputs.GetLookVertical();
			isCameraZoomButtonDown = BaseInputs.IsCameraZoomButtonDown();
			isCameraZoomButtonUp = BaseInputs.IsCameraZoomButtonUp();
			isPrimaryInteractionButtonDown = BaseInputs.IsPrimaryInteractionButtonDown();
			isPrimaryInteractionButtonUp = BaseInputs.IsPrimaryInteractionButtonUp();
			isSecondaryInteractionButtonDown = BaseInputs.IsSecondaryInteractionButtonDown();
			isSecondaryInteractionButtonUp = BaseInputs.IsSecondaryInteractionButtonUp();
			moveHorizontal = OnFootInputs.GetMoveHorizontal();
			moveVertical = OnFootInputs.GetMoveVertical();
			isSprintButton = OnFootInputs.IsSprintButton();
			isJumpButtonDown = OnFootInputs.IsJumpButtonDown();
			isJumpButtonUp = OnFootInputs.IsJumpButtonUp();
			isCrouchButtonDown = OnFootInputs.IsCrouchButtonDown();
			isCrouchButtonUp = OnFootInputs.IsCrouchButtonUp();
			isStandupButton = SittingInputs.IsStandupButton();
			isStandupButtonDown = SittingInputs.IsStandupButtonDown();
			throttle = DrivingInputs.GetThrottle();
			brakes = DrivingInputs.GetBrakes();
			steering = DrivingInputs.GetSteering();
			handbrake = DrivingInputs.GetHandbrake();
			isHornButton = DrivingInputs.IsHornButton();
			rotateObjectButton = PlacingObjectInputs.GetRotateObjectButton();
			isFinishObjectPlacingButtonDown = PlacingObjectInputs.IsFinishObjectPlacingButtonDown();
			moveObjectHorizontal = PlacingObjectInputs.GetMoveObjectHorizontalValue();
			moveObjectVertical = PlacingObjectInputs.GetMoveObjectVerticalValue();
			moveObjectDirect = PlacingObjectInputs.GetMoveObjectDirectValue();
			isPrimaryUseObjectButtonDown = EquippingInputs.IsUseObjectButtonDown(HeldItemUseInputType.Primary);
			isThrowObjectButtonDown = EquippingInputs.IsThrowObjectButtonDown();
			isThrowObjectButtonUp = EquippingInputs.IsThrowObjectButtonUp();
			isEnterPlacementModeButtonDown = EquippingInputs.IsEnterPlacementModeButtonDown();
		}
	}
}
