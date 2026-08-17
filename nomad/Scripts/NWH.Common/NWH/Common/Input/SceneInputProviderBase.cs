using UnityEngine;

namespace NWH.Common.Input
{
	public abstract class SceneInputProviderBase : InputProvider
	{
		[Tooltip("    If true a button press will be required to unlock camera rotation.")]
		public bool requireCameraRotationModifier = true;

		[Tooltip("    If true a button press will be required to unlock camera panning.")]
		public bool requireCameraPanningModifier = true;

		public virtual bool ChangeCamera()
		{
			return false;
		}

		public virtual Vector2 CameraRotation()
		{
			return Vector2.zero;
		}

		public virtual Vector2 CameraPanning()
		{
			return Vector2.zero;
		}

		public virtual bool CameraRotationModifier()
		{
			return !requireCameraRotationModifier;
		}

		public virtual bool CameraPanningModifier()
		{
			return !requireCameraPanningModifier;
		}

		public virtual float CameraZoom()
		{
			return 0f;
		}

		public virtual bool ChangeVehicle()
		{
			return false;
		}

		public virtual Vector2 CharacterMovement()
		{
			return Vector2.zero;
		}

		public virtual bool ToggleGUI()
		{
			return false;
		}
	}
}
