using UnityEngine;

namespace EvilCore.Recording
{
	public class DirectorHiddenControls
	{
		private HiddenParameterMode _activeMode;

		private DirectorManager _manager;

		public HiddenParameterMode ActiveMode => _activeMode;

		public bool HasActiveMode => _activeMode != HiddenParameterMode.None;

		public DirectorHiddenControls(DirectorManager manager)
		{
			_manager = manager;
		}

		public void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				ToggleMode(HiddenParameterMode.FieldOfView);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				ToggleMode(HiddenParameterMode.Speed);
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				ToggleMode(HiddenParameterMode.Height);
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				ToggleMode(HiddenParameterMode.Radius);
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				_activeMode = HiddenParameterMode.None;
			}
			if (_activeMode == HiddenParameterMode.None)
			{
				return;
			}
			DirectorCameraSetup activeCamera = _manager.ActiveCamera;
			if (!(activeCamera == null))
			{
				float hiddenSchemaInputSpeed = activeCamera.GetSettings().hiddenSchemaInputSpeed;
				float unscaledDeltaTime = Time.unscaledDeltaTime;
				float num = 0f;
				if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow))
				{
					num = 1f;
				}
				else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
				{
					num = -1f;
				}
				if (!(Mathf.Abs(num) < 0.001f))
				{
					ApplyParameterChange(activeCamera, num, hiddenSchemaInputSpeed, unscaledDeltaTime);
				}
			}
		}

		private void ToggleMode(HiddenParameterMode mode)
		{
			_activeMode = ((_activeMode != mode) ? mode : HiddenParameterMode.None);
		}

		private void ApplyParameterChange(DirectorCameraSetup cam, float direction, float inputSpeed, float dt)
		{
			float num = direction * inputSpeed * dt;
			switch (_activeMode)
			{
			case HiddenParameterMode.FieldOfView:
				if (!(cam.VirtualCamera == null))
				{
					float fieldOfView = Mathf.Clamp(cam.VirtualCamera.Lens.FieldOfView + num * 30f, 10f, 120f);
					cam.SetFieldOfView(fieldOfView);
				}
				break;
			case HiddenParameterMode.Speed:
				if (cam.Configurator is OrbitBehavior orbitBehavior2)
				{
					float speed = Mathf.Clamp(orbitBehavior2.ExtractSettings(null).orbitSettings.speed + num * 20f, 1f, 180f);
					orbitBehavior2.SetSpeed(speed);
				}
				else if (cam.Configurator is TurntableBehavior turntableBehavior2)
				{
					float speed2 = Mathf.Clamp(turntableBehavior2.ExtractSettings(null).turntableSettings.speed + num * 20f, 1f, 180f);
					turntableBehavior2.SetSpeed(speed2);
				}
				break;
			case HiddenParameterMode.Height:
				if (cam.Configurator is OrbitBehavior orbitBehavior3)
				{
					float height = Mathf.Clamp(orbitBehavior3.ExtractSettings(null).orbitSettings.height + num * 5f, -20f, 20f);
					orbitBehavior3.SetHeight(height);
				}
				else if (cam.Configurator is TurntableBehavior turntableBehavior3)
				{
					float pitch = Mathf.Clamp(turntableBehavior3.ExtractSettings(null).turntableSettings.pitch + num * 10f, -89f, 89f);
					turntableBehavior3.SetPitch(pitch);
				}
				break;
			case HiddenParameterMode.Radius:
				if (cam.Configurator is OrbitBehavior orbitBehavior)
				{
					float radius = Mathf.Clamp(orbitBehavior.ExtractSettings(null).orbitSettings.radius + num * 5f, 1f, 50f);
					orbitBehavior.SetRadius(radius);
				}
				else if (cam.Configurator is TurntableBehavior turntableBehavior)
				{
					Vector3 offset = turntableBehavior.ExtractSettings(null).turntableSettings.offset;
					offset.z = Mathf.Clamp(offset.z + num * 5f, -10f, 10f);
					turntableBehavior.SetOffset(offset);
				}
				break;
			}
		}

		public void Reset()
		{
			_activeMode = HiddenParameterMode.None;
		}
	}
}
