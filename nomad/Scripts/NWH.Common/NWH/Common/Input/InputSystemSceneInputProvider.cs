using UnityEngine;

namespace NWH.Common.Input
{
	public class InputSystemSceneInputProvider : SceneInputProviderBase
	{
		public SceneInputActions sceneInputActions;

		private bool _rotationModifier;

		private bool _panningModifier;

		public override void Awake()
		{
			base.Awake();
			sceneInputActions = new SceneInputActions();
			sceneInputActions.Enable();
			sceneInputActions.CameraControls.CameraRotationModifier.started += delegate
			{
				_rotationModifier = true;
			};
			sceneInputActions.CameraControls.CameraRotationModifier.canceled += delegate
			{
				_rotationModifier = false;
			};
			sceneInputActions.CameraControls.CameraPanningModifier.started += delegate
			{
				_panningModifier = true;
			};
			sceneInputActions.CameraControls.CameraPanningModifier.canceled += delegate
			{
				_panningModifier = false;
			};
		}

		public override bool ChangeCamera()
		{
			return sceneInputActions.CameraControls.ChangeCamera.triggered;
		}

		public override Vector2 CameraRotation()
		{
			return sceneInputActions.CameraControls.CameraRotation.ReadValue<Vector2>();
		}

		public override Vector2 CameraPanning()
		{
			return sceneInputActions.CameraControls.CameraPanning.ReadValue<Vector2>();
		}

		public override bool CameraRotationModifier()
		{
			if (!_rotationModifier)
			{
				return !requireCameraRotationModifier;
			}
			return true;
		}

		public override bool CameraPanningModifier()
		{
			if (!_panningModifier)
			{
				return !requireCameraPanningModifier;
			}
			return true;
		}

		public override float CameraZoom()
		{
			return sceneInputActions.CameraControls.CameraZoom.ReadValue<float>() * 0.1f;
		}

		public override bool ChangeVehicle()
		{
			return sceneInputActions.SceneControls.ChangeVehicle.triggered;
		}

		public override Vector2 CharacterMovement()
		{
			return sceneInputActions.SceneControls.FPSMovement.ReadValue<Vector2>();
		}

		public override bool ToggleGUI()
		{
			return sceneInputActions.SceneControls.ToggleGUI.triggered;
		}
	}
}
