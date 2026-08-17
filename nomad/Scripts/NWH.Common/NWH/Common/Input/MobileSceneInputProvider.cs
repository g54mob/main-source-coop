using UnityEngine;

namespace NWH.Common.Input
{
	public class MobileSceneInputProvider : SceneInputProviderBase
	{
		public MobileInputButton changeCameraButton;

		public MobileInputButton changeVehicleButton;

		public override bool ChangeCamera()
		{
			if (changeCameraButton != null)
			{
				return changeCameraButton.hasBeenClicked;
			}
			return false;
		}

		public override bool ChangeVehicle()
		{
			if (changeVehicleButton != null)
			{
				return changeVehicleButton.hasBeenClicked;
			}
			return false;
		}

		public override Vector2 CharacterMovement()
		{
			return Vector2.zero;
		}

		public override bool ToggleGUI()
		{
			return false;
		}

		public override Vector2 CameraRotation()
		{
			return Vector2.zero;
		}

		public override Vector2 CameraPanning()
		{
			return Vector2.zero;
		}

		public override bool CameraRotationModifier()
		{
			return false;
		}

		public override bool CameraPanningModifier()
		{
			return false;
		}

		public override float CameraZoom()
		{
			return 0f;
		}
	}
}
