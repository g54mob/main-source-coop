using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Recording
{
	public class DirectorUIController : MonoBehaviour
	{
		private const string CameraNameControl = "DirectorCameraNameField";

		private const string PresetNameControl = "DirectorPresetNameField";

		private DirectorManager _manager;

		private bool _isVisible;

		private bool _needFocusClear;

		private Vector2 _cameraListScroll;

		private Vector2 _settingsScroll;

		private int _selectedCameraIndex = -1;

		private string _newCameraName = "Camera";

		private string _presetName = "MyPreset";

		private string[] _availablePresets = Array.Empty<string>();

		private int _selectedPresetIndex;

		private bool _showPresetPanel;

		private List<Transform> _targetCandidates = new List<Transform>();

		private bool _showTargetPicker;

		private Vector2 _targetPickerScroll;

		private Action<Transform> _onTargetPicked;

		private float _panelOpacity = 0.85f;

		private GUIStyle _headerStyle;

		private GUIStyle _panelStyle;

		private GUIStyle _labelStyle;

		private GUIStyle _buttonStyle;

		private GUIStyle _activeButtonStyle;

		private GUIStyle _boxStyle;

		private bool _stylesInitialized;

		private float _lastAppliedOpacity;

		private bool _isPanelHidden;

		public bool IsVisible => _isVisible;

		public bool IsPanelHidden => _isPanelHidden;

		public void Show(DirectorManager manager)
		{
			_manager = manager;
			_isVisible = true;
			_isPanelHidden = false;
			_selectedCameraIndex = manager.ActiveCameraIndex;
			_needFocusClear = true;
			RefreshPresets();
		}

		public void Hide()
		{
			_isVisible = false;
		}

		public void TogglePanelVisibility()
		{
			_isPanelHidden = !_isPanelHidden;
		}

		private void InitStyles()
		{
			if (!_stylesInitialized || !(Mathf.Abs(_lastAppliedOpacity - _panelOpacity) < 0.01f))
			{
				float panelOpacity = _panelOpacity;
				GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
				gUIStyle.normal.background = MakeTex(2, 2, new Color(0.15f, 0.15f, 0.15f, panelOpacity));
				gUIStyle.alignment = TextAnchor.MiddleLeft;
				gUIStyle.fontSize = 14;
				gUIStyle.fontStyle = FontStyle.Bold;
				gUIStyle.padding = new RectOffset(10, 10, 5, 5);
				_headerStyle = gUIStyle;
				_headerStyle.normal.textColor = new Color(1f, 1f, 1f, panelOpacity);
				GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.box);
				gUIStyle2.normal.background = MakeTex(2, 2, new Color(0.18f, 0.18f, 0.18f, panelOpacity));
				gUIStyle2.padding = new RectOffset(8, 8, 8, 8);
				_panelStyle = gUIStyle2;
				_labelStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12
				};
				_labelStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f, panelOpacity);
				GUIStyle gUIStyle3 = new GUIStyle(GUI.skin.button);
				gUIStyle3.fontSize = 11;
				gUIStyle3.normal.background = MakeTex(2, 2, new Color(0.25f, 0.25f, 0.25f, panelOpacity));
				_buttonStyle = gUIStyle3;
				_buttonStyle.normal.textColor = new Color(1f, 1f, 1f, panelOpacity);
				GUIStyle gUIStyle4 = new GUIStyle(_buttonStyle);
				gUIStyle4.normal.background = MakeTex(2, 2, new Color(0.3f, 0.6f, 0.9f, panelOpacity));
				gUIStyle4.fontStyle = FontStyle.Bold;
				_activeButtonStyle = gUIStyle4;
				_activeButtonStyle.normal.textColor = new Color(1f, 1f, 1f, panelOpacity);
				GUIStyle gUIStyle5 = new GUIStyle(GUI.skin.box);
				gUIStyle5.normal.background = MakeTex(2, 2, new Color(0.12f, 0.12f, 0.12f, panelOpacity * 0.95f));
				gUIStyle5.padding = new RectOffset(5, 5, 5, 5);
				_boxStyle = gUIStyle5;
				_stylesInitialized = true;
				_lastAppliedOpacity = _panelOpacity;
			}
		}

		private void OnGUI()
		{
			if (!_isVisible || _manager == null)
			{
				return;
			}
			InitStyles();
			if (_isPanelHidden)
			{
				return;
			}
			if (_needFocusClear)
			{
				GUI.FocusControl(null);
				_needFocusClear = false;
			}
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
			{
				string nameOfFocusedControl = GUI.GetNameOfFocusedControl();
				if (nameOfFocusedControl != "DirectorCameraNameField" && nameOfFocusedControl != "DirectorPresetNameField")
				{
					GUI.FocusControl(null);
					Event.current.Use();
				}
			}
			float width = 420f;
			float height = (float)Screen.height - 40f;
			Rect rect = new Rect(20f, 20f, width, height);
			GUI.Box(rect, "", _panelStyle);
			GUILayout.BeginArea(rect);
			DrawHeader();
			DrawCameraList();
			DrawSelectedCameraSettings();
			DrawTransportControls();
			DrawPresetControls();
			GUILayout.EndArea();
		}

		private void DrawHiddenModeHint()
		{
		}

		private void DrawHeader()
		{
			GUILayout.BeginHorizontal(_headerStyle, GUILayout.Height(30f));
			GUILayout.Label("DIRECTOR MODE", _headerStyle);
			GUILayout.FlexibleSpace();
			float num = GUILayout.HorizontalSlider(_panelOpacity, 0.1f, 1f, GUILayout.Width(80f));
			if (Mathf.Abs(num - _panelOpacity) > 0.01f)
			{
				_panelOpacity = num;
			}
			if (GUILayout.Button("X", _buttonStyle, GUILayout.Width(30f), GUILayout.Height(25f)))
			{
				_manager.ExitDirectorMode();
			}
			GUILayout.EndHorizontal();
		}

		private void DrawCameraList()
		{
			GUILayout.Space(5f);
			GUILayout.Label("Cameras", _labelStyle);
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("DirectorCameraNameField");
			_newCameraName = GUILayout.TextField(_newCameraName, GUILayout.Width(200f));
			if (GUILayout.Button("+ Add", _buttonStyle, GUILayout.Width(60f)))
			{
				_manager.AddCamera(_newCameraName);
				_selectedCameraIndex = _manager.Cameras.Count - 1;
				_manager.ActivateCamera(_selectedCameraIndex);
			}
			GUILayout.EndHorizontal();
			float height = Mathf.Min((float)_manager.Cameras.Count * 28f + 10f, 200f);
			_cameraListScroll = GUILayout.BeginScrollView(_cameraListScroll, _boxStyle, GUILayout.Height(height));
			for (int i = 0; i < _manager.Cameras.Count; i++)
			{
				DirectorCameraSetup directorCameraSetup = _manager.Cameras[i];
				_ = _selectedCameraIndex;
				bool num = i == _manager.ActiveCameraIndex;
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(style: num ? _activeButtonStyle : _buttonStyle, text: num ? ("> " + directorCameraSetup.CameraName) : ("  " + directorCameraSetup.CameraName), options: new GUILayoutOption[1] { GUILayout.Height(24f) }))
				{
					_selectedCameraIndex = i;
					_manager.ActivateCamera(i);
				}
				if (GUILayout.Button("-", _buttonStyle, GUILayout.Width(25f), GUILayout.Height(24f)))
				{
					_manager.RemoveCamera(i);
					if (_selectedCameraIndex >= _manager.Cameras.Count)
					{
						_selectedCameraIndex = _manager.Cameras.Count - 1;
					}
					GUILayout.EndHorizontal();
					break;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}

		private void DrawSelectedCameraSettings()
		{
			if (_selectedCameraIndex < 0 || _selectedCameraIndex >= _manager.Cameras.Count)
			{
				return;
			}
			DirectorCameraSetup directorCameraSetup = _manager.Cameras[_selectedCameraIndex];
			GUILayout.Space(5f);
			GUILayout.Label("Settings: " + directorCameraSetup.CameraName, _labelStyle);
			float a = (float)Screen.height - 340f;
			_settingsScroll = GUILayout.BeginScrollView(_settingsScroll, _boxStyle, GUILayout.Height(Mathf.Max(a, 200f)));
			GUILayout.Label("Behavior Type", _labelStyle);
			DirectorCameraBehavior[] array = (DirectorCameraBehavior[])Enum.GetValues(typeof(DirectorCameraBehavior));
			int num = Array.IndexOf(array, directorCameraSetup.BehaviorType);
			string[] names = Enum.GetNames(typeof(DirectorCameraBehavior));
			int num2 = GUILayout.SelectionGrid(num, names, 3, _buttonStyle);
			if (num2 != num && num2 >= 0 && num2 < array.Length)
			{
				directorCameraSetup.SetBehaviorType(array[num2]);
			}
			GUILayout.Space(5f);
			if (directorCameraSetup.VirtualCamera != null)
			{
				float fieldOfView = directorCameraSetup.VirtualCamera.Lens.FieldOfView;
				GUILayout.Label($"Field of View: {fieldOfView:F0}", _labelStyle);
				float num3 = GUILayout.HorizontalSlider(fieldOfView, 10f, 120f);
				if (Mathf.Abs(num3 - fieldOfView) > 0.1f)
				{
					directorCameraSetup.SetFieldOfView(num3);
				}
			}
			GUILayout.Label($"Smoothing: {directorCameraSetup.Smoothing:F2}", _labelStyle);
			float num4 = GUILayout.HorizontalSlider(directorCameraSetup.Smoothing, 0f, 1f);
			if (Mathf.Abs(num4 - directorCameraSetup.Smoothing) > 0.01f)
			{
				directorCameraSetup.Smoothing = num4;
			}
			directorCameraSetup.ExecuteOnHide = GUILayout.Toggle(directorCameraSetup.ExecuteOnHide, " Execute on Hide (H)", _buttonStyle);
			float hiddenSchemaInputSpeed = directorCameraSetup.HiddenSchemaInputSpeed;
			GUILayout.Label($"Hidden Schema Input Speed: {hiddenSchemaInputSpeed:F1}", _labelStyle);
			float num5 = GUILayout.HorizontalSlider(hiddenSchemaInputSpeed, 0.1f, 5f);
			if (Mathf.Abs(num5 - hiddenSchemaInputSpeed) > 0.01f)
			{
				directorCameraSetup.HiddenSchemaInputSpeed = num5;
			}
			GUILayout.Space(5f);
			DrawBehaviorSpecificSettings(directorCameraSetup);
			GUILayout.Space(5f);
			DrawDepthOfFieldSettings(directorCameraSetup);
			GUILayout.Space(5f);
			GUILayout.Label("Position Tools", _labelStyle);
			if (directorCameraSetup.IsPositioning)
			{
				GUILayout.Label("WASD: Move  |  Mouse: Look  |  Shift: Accelerate  |  Scroll: Speed", _labelStyle);
				GUILayout.Label("Right Click to confirm", _labelStyle);
				if (GUILayout.Button("Stop Positioning", _activeButtonStyle))
				{
					directorCameraSetup.StopPositioning();
				}
			}
			else if (GUILayout.Button("Position Camera (WASD + Mouse)", _buttonStyle))
			{
				directorCameraSetup.StartPositioning();
			}
			if (directorCameraSetup.VirtualCamera != null)
			{
				Vector3 position = directorCameraSetup.VirtualCamera.transform.position;
				GUILayout.Label($"Pos: ({position.x:F1}, {position.y:F1}, {position.z:F1})", _labelStyle);
			}
			GUILayout.EndScrollView();
		}

		private void DrawBehaviorSpecificSettings(DirectorCameraSetup cam)
		{
			IBehaviorConfigurator configurator = cam.Configurator;
			if (configurator == null)
			{
				return;
			}
			if (!(configurator is OrbitBehavior orbit))
			{
				if (!(configurator is FollowBehavior follow))
				{
					if (!(configurator is DollyBehavior dolly))
					{
						if (!(configurator is ZoomBehavior))
						{
							if (!(configurator is HandheldBehavior handheld))
							{
								if (!(configurator is PanTiltBehavior))
								{
									if (configurator is TurntableBehavior turntable)
									{
										DrawTurntableSettings(turntable);
									}
								}
								else
								{
									DrawPanTiltSettings(cam);
								}
							}
							else
							{
								DrawHandheldSettings(handheld);
							}
						}
						else
						{
							DrawZoomSettings(cam);
						}
					}
					else
					{
						DrawDollySettings(dolly, cam);
					}
				}
				else
				{
					DrawFollowSettings(follow);
				}
			}
			else
			{
				DrawOrbitSettings(orbit);
			}
		}

		private void DrawOrbitSettings(OrbitBehavior orbit)
		{
			GUILayout.Label("Orbit Settings", _labelStyle);
			OrbitSettings orbitSettings = orbit.ExtractSettings(null).orbitSettings;
			GUILayout.Label($"Radius: {orbitSettings.radius:F1}", _labelStyle);
			float num = GUILayout.HorizontalSlider(orbitSettings.radius, 1f, 50f);
			if (Mathf.Abs(num - orbitSettings.radius) > 0.01f)
			{
				orbit.SetRadius(num);
			}
			GUILayout.Label($"Height: {orbitSettings.height:F1}", _labelStyle);
			float num2 = GUILayout.HorizontalSlider(orbitSettings.height, -20f, 20f);
			if (Mathf.Abs(num2 - orbitSettings.height) > 0.01f)
			{
				orbit.SetHeight(num2);
			}
			GUILayout.Label($"Speed: {orbitSettings.speed:F1} deg/s", _labelStyle);
			float num3 = GUILayout.HorizontalSlider(orbitSettings.speed, 1f, 180f);
			if (Mathf.Abs(num3 - orbitSettings.speed) > 0.01f)
			{
				orbit.SetSpeed(num3);
			}
			float parameterSmoothTime = orbit.ParameterSmoothTime;
			GUILayout.Label($"Param Smooth Time: {parameterSmoothTime:F2}s", _labelStyle);
			float num4 = GUILayout.HorizontalSlider(parameterSmoothTime, 0f, 1f);
			if (Mathf.Abs(num4 - parameterSmoothTime) > 0.01f)
			{
				orbit.SetParameterSmoothTime(num4);
			}
			Vector3 targetPosition = orbitSettings.targetPosition;
			GUILayout.Label($"Center: ({targetPosition.x:F1}, {targetPosition.y:F1}, {targetPosition.z:F1})", _labelStyle);
			if (GUILayout.Button("Set Center to Where Camera Looks", _buttonStyle))
			{
				DirectorCameraSetup directorCameraSetup = _manager.Cameras[_selectedCameraIndex];
				if (directorCameraSetup.VirtualCamera != null)
				{
					Transform transform = directorCameraSetup.VirtualCamera.transform;
					RaycastHit hitInfo;
					Vector3 targetPosition2 = (Physics.Raycast(transform.position, transform.forward, out hitInfo, 500f) ? hitInfo.point : (transform.position + transform.forward * orbitSettings.radius));
					orbit.SetTargetPosition(targetPosition2);
				}
			}
		}

		private void DrawFollowSettings(FollowBehavior follow)
		{
			GUILayout.Label("Follow Settings", _labelStyle);
			DrawTargetPicker(follow.TargetTransform, follow.HasTarget, delegate(Transform t)
			{
				follow.SetTarget(t);
			}, delegate
			{
				follow.ClearTarget();
			});
			FollowSettings followSettings = follow.ExtractSettings(null).followSettings;
			GUILayout.Label($"Distance: {followSettings.distance:F1}", _labelStyle);
			float num = GUILayout.HorizontalSlider(followSettings.distance, 1f, 50f);
			if (Mathf.Abs(num - followSettings.distance) > 0.01f)
			{
				follow.SetDistance(num);
			}
			GUILayout.Label($"Height: {followSettings.height:F1}", _labelStyle);
			float num2 = GUILayout.HorizontalSlider(followSettings.height, -10f, 30f);
			if (Mathf.Abs(num2 - followSettings.height) > 0.01f)
			{
				follow.SetHeight(num2);
			}
			GUILayout.Label($"Angle: {followSettings.angle:F0} deg", _labelStyle);
			float num3 = GUILayout.HorizontalSlider(followSettings.angle, 0f, 360f);
			if (Mathf.Abs(num3 - followSettings.angle) > 0.1f)
			{
				follow.SetAngle(num3);
			}
			GUILayout.Label($"Follow Smooth: {followSettings.followSmoothTime:F2}s", _labelStyle);
			float num4 = GUILayout.HorizontalSlider(followSettings.followSmoothTime, 0.01f, 2f);
			if (Mathf.Abs(num4 - followSettings.followSmoothTime) > 0.01f)
			{
				follow.SetFollowSmoothTime(num4);
			}
			GUILayout.Label($"Look-At Smooth: {followSettings.lookAtSmoothTime:F2}s", _labelStyle);
			float num5 = GUILayout.HorizontalSlider(followSettings.lookAtSmoothTime, 0.01f, 2f);
			if (Mathf.Abs(num5 - followSettings.lookAtSmoothTime) > 0.01f)
			{
				follow.SetLookAtSmoothTime(num5);
			}
			GUILayout.Label($"Look-At Height Offset: {followSettings.lookAtHeightOffset:F1}", _labelStyle);
			float num6 = GUILayout.HorizontalSlider(followSettings.lookAtHeightOffset, -5f, 10f);
			if (Mathf.Abs(num6 - followSettings.lookAtHeightOffset) > 0.01f)
			{
				follow.SetLookAtHeightOffset(num6);
			}
			GUILayout.Space(3f);
			GUILayout.Label($"Shake Amplitude: {followSettings.shakeAmplitude:F2}", _labelStyle);
			float num7 = GUILayout.HorizontalSlider(followSettings.shakeAmplitude, 0f, 3f);
			if (Mathf.Abs(num7 - followSettings.shakeAmplitude) > 0.01f)
			{
				follow.SetShakeAmplitude(num7);
			}
			GUILayout.Label($"Shake Frequency: {followSettings.shakeFrequency:F2}", _labelStyle);
			float num8 = GUILayout.HorizontalSlider(followSettings.shakeFrequency, 0f, 5f);
			if (Mathf.Abs(num8 - followSettings.shakeFrequency) > 0.01f)
			{
				follow.SetShakeFrequency(num8);
			}
		}

		private void DrawDollySettings(DollyBehavior dolly, DirectorCameraSetup cam)
		{
			GUILayout.Label("Dolly Settings", _labelStyle);
			DollySettings dollySettings = dolly.ExtractSettings(null).dollySettings;
			GUILayout.Label("Direction", _labelStyle);
			string[] names = Enum.GetNames(typeof(DollyDirection));
			int direction = (int)dollySettings.direction;
			int num = GUILayout.SelectionGrid(direction, names, 3, _buttonStyle);
			if (num != direction && cam.VirtualCamera != null)
			{
				dolly.SetDirection((DollyDirection)num, cam.VirtualCamera.transform);
			}
			GUILayout.Label($"Speed: {dollySettings.speed:F1}", _labelStyle);
			float num2 = GUILayout.HorizontalSlider(dollySettings.speed, 0.1f, 20f);
			if (Mathf.Abs(num2 - dollySettings.speed) > 0.01f)
			{
				dolly.SetSpeed(num2);
			}
			if (GUILayout.Button("Set Look-At to Camera Target", _buttonStyle) && cam.VirtualCamera != null)
			{
				Transform transform = cam.VirtualCamera.transform;
				RaycastHit hitInfo;
				Vector3 lookAtTarget = (Physics.Raycast(transform.position, transform.forward, out hitInfo, 500f) ? hitInfo.point : (transform.position + transform.forward * 10f));
				dolly.SetLookAtTarget(lookAtTarget);
			}
			if (dollySettings.lookAtTarget)
			{
				Vector3 lookAtPosition = dollySettings.lookAtPosition;
				GUILayout.Label($"Look-At: ({lookAtPosition.x:F1}, {lookAtPosition.y:F1}, {lookAtPosition.z:F1})", _labelStyle);
				if (GUILayout.Button("Clear Look-At", _buttonStyle))
				{
					dolly.ClearLookAtTarget();
				}
			}
		}

		private void DrawZoomSettings(DirectorCameraSetup cam)
		{
			GUILayout.Label("Zoom Settings", _labelStyle);
			ZoomBehavior configurator = cam.Configurator as ZoomBehavior;
			if (configurator != null)
			{
				DrawTargetPicker(configurator.TargetTransform, configurator.HasTarget, delegate(Transform t)
				{
					configurator.SetTarget(t);
				}, delegate
				{
					configurator.ClearTarget();
				});
				ZoomSettings zoomSettings = configurator.ExtractSettings(null).zoomSettings;
				GUILayout.Label($"Start FOV: {zoomSettings.startFOV:F0}", _labelStyle);
				float num = GUILayout.HorizontalSlider(zoomSettings.startFOV, 10f, 120f);
				if (Mathf.Abs(num - zoomSettings.startFOV) > 0.1f)
				{
					configurator.SetStartFOV(num);
				}
				GUILayout.Label($"End FOV: {zoomSettings.endFOV:F0}", _labelStyle);
				float num2 = GUILayout.HorizontalSlider(zoomSettings.endFOV, 10f, 120f);
				if (Mathf.Abs(num2 - zoomSettings.endFOV) > 0.1f)
				{
					configurator.SetEndFOV(num2);
				}
				GUILayout.Label($"Duration: {zoomSettings.duration:F1}s", _labelStyle);
				float num3 = GUILayout.HorizontalSlider(zoomSettings.duration, 0.5f, 30f);
				if (Mathf.Abs(num3 - zoomSettings.duration) > 0.01f)
				{
					configurator.SetDuration(num3);
				}
				GUILayout.Space(3f);
				GUILayout.Label($"Distance: {zoomSettings.distance:F1}", _labelStyle);
				float num4 = GUILayout.HorizontalSlider(zoomSettings.distance, 1f, 50f);
				if (Mathf.Abs(num4 - zoomSettings.distance) > 0.01f)
				{
					configurator.SetDistance(num4);
				}
				GUILayout.Label($"Height: {zoomSettings.height:F1}", _labelStyle);
				float num5 = GUILayout.HorizontalSlider(zoomSettings.height, -10f, 30f);
				if (Mathf.Abs(num5 - zoomSettings.height) > 0.01f)
				{
					configurator.SetHeight(num5);
				}
				GUILayout.Label($"Angle: {zoomSettings.angle:F0} deg", _labelStyle);
				float num6 = GUILayout.HorizontalSlider(zoomSettings.angle, 0f, 360f);
				if (Mathf.Abs(num6 - zoomSettings.angle) > 0.1f)
				{
					configurator.SetAngle(num6);
				}
				GUILayout.Label($"Follow Smooth: {zoomSettings.followSmoothTime:F2}s", _labelStyle);
				float num7 = GUILayout.HorizontalSlider(zoomSettings.followSmoothTime, 0.01f, 2f);
				if (Mathf.Abs(num7 - zoomSettings.followSmoothTime) > 0.01f)
				{
					configurator.SetFollowSmoothTime(num7);
				}
				GUILayout.Label($"Look-At Smooth: {zoomSettings.lookAtSmoothTime:F2}s", _labelStyle);
				float num8 = GUILayout.HorizontalSlider(zoomSettings.lookAtSmoothTime, 0.01f, 2f);
				if (Mathf.Abs(num8 - zoomSettings.lookAtSmoothTime) > 0.01f)
				{
					configurator.SetLookAtSmoothTime(num8);
				}
				GUILayout.Label($"Look-At Height Offset: {zoomSettings.lookAtHeightOffset:F1}", _labelStyle);
				float num9 = GUILayout.HorizontalSlider(zoomSettings.lookAtHeightOffset, -5f, 10f);
				if (Mathf.Abs(num9 - zoomSettings.lookAtHeightOffset) > 0.01f)
				{
					configurator.SetLookAtHeightOffset(num9);
				}
				GUILayout.Space(3f);
				GUILayout.Label($"Shake Amplitude: {zoomSettings.shakeAmplitude:F2}", _labelStyle);
				float num10 = GUILayout.HorizontalSlider(zoomSettings.shakeAmplitude, 0f, 3f);
				if (Mathf.Abs(num10 - zoomSettings.shakeAmplitude) > 0.01f)
				{
					configurator.SetShakeAmplitude(num10);
				}
				GUILayout.Label($"Shake Frequency: {zoomSettings.shakeFrequency:F2}", _labelStyle);
				float num11 = GUILayout.HorizontalSlider(zoomSettings.shakeFrequency, 0f, 5f);
				if (Mathf.Abs(num11 - zoomSettings.shakeFrequency) > 0.01f)
				{
					configurator.SetShakeFrequency(num11);
				}
			}
		}

		private void DrawHandheldSettings(HandheldBehavior handheld)
		{
			GUILayout.Label("Handheld Settings", _labelStyle);
			HandheldSettings handheldSettings = handheld.ExtractSettings(null).handheldSettings;
			GUILayout.Label($"Amplitude: {handheldSettings.amplitudeGain:F2}", _labelStyle);
			float num = GUILayout.HorizontalSlider(handheldSettings.amplitudeGain, 0f, 3f);
			if (Mathf.Abs(num - handheldSettings.amplitudeGain) > 0.01f)
			{
				handheld.SetAmplitude(num);
			}
			GUILayout.Label($"Frequency: {handheldSettings.frequencyGain:F2}", _labelStyle);
			float num2 = GUILayout.HorizontalSlider(handheldSettings.frequencyGain, 0f, 5f);
			if (Mathf.Abs(num2 - handheldSettings.frequencyGain) > 0.01f)
			{
				handheld.SetFrequency(num2);
			}
		}

		private void DrawPanTiltSettings(DirectorCameraSetup cam)
		{
			GUILayout.Label("Pan/Tilt Settings", _labelStyle);
		}

		private void DrawTurntableSettings(TurntableBehavior turntable)
		{
			GUILayout.Label("Turntable Settings", _labelStyle);
			TurntableSettings turntableSettings = turntable.ExtractSettings(null).turntableSettings;
			GUILayout.Label($"Speed: {turntableSettings.speed:F1} deg/s", _labelStyle);
			float num = GUILayout.HorizontalSlider(turntableSettings.speed, 1f, 180f);
			if (Mathf.Abs(num - turntableSettings.speed) > 0.01f)
			{
				turntable.SetSpeed(num);
			}
			GUILayout.Label($"Pitch: {turntableSettings.pitch:F1} deg", _labelStyle);
			float num2 = GUILayout.HorizontalSlider(turntableSettings.pitch, -89f, 89f);
			if (Mathf.Abs(num2 - turntableSettings.pitch) > 0.01f)
			{
				turntable.SetPitch(num2);
			}
			GUILayout.Label($"Offset X: {turntableSettings.offset.x:F2}", _labelStyle);
			float x = GUILayout.HorizontalSlider(turntableSettings.offset.x, -10f, 10f);
			GUILayout.Label($"Offset Y: {turntableSettings.offset.y:F2}", _labelStyle);
			float y = GUILayout.HorizontalSlider(turntableSettings.offset.y, -10f, 10f);
			GUILayout.Label($"Offset Z: {turntableSettings.offset.z:F2}", _labelStyle);
			float z = GUILayout.HorizontalSlider(turntableSettings.offset.z, -10f, 10f);
			Vector3 vector = new Vector3(x, y, z);
			if ((vector - turntableSettings.offset).sqrMagnitude > 0.0001f)
			{
				turntable.SetOffset(vector);
			}
			float parameterSmoothTime = turntable.ParameterSmoothTime;
			GUILayout.Label($"Param Smooth Time: {parameterSmoothTime:F2}s", _labelStyle);
			float num3 = GUILayout.HorizontalSlider(parameterSmoothTime, 0f, 1f);
			if (Mathf.Abs(num3 - parameterSmoothTime) > 0.01f)
			{
				turntable.SetParameterSmoothTime(num3);
			}
		}

		private void DrawTransportControls()
		{
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("|<", _buttonStyle, GUILayout.Width(35f)))
			{
				_manager.PreviousCamera();
			}
			if (GUILayout.Button(">|", _buttonStyle, GUILayout.Width(35f)))
			{
				_manager.NextCamera();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Free Camera:", _labelStyle, GUILayout.Width(85f));
			DirectorFreeCameraController componentInChildren = _manager.GetComponentInChildren<DirectorFreeCameraController>();
			if (componentInChildren != null)
			{
				if (componentInChildren.IsControlling)
				{
					if (GUILayout.Button("Stop Free Cam (RMB)", _activeButtonStyle))
					{
						componentInChildren.StopControlling();
					}
				}
				else if (GUILayout.Button("Start Free Cam", _buttonStyle))
				{
					componentInChildren.StartControlling();
				}
			}
			else if (GUILayout.Button("Create Free Cam", _buttonStyle))
			{
				GameObject gameObject = new GameObject("DirectorFreeCam");
				gameObject.transform.SetParent(_manager.transform);
				gameObject.AddComponent<DirectorFreeCameraController>();
				if (_manager.ActiveCamera?.VirtualCamera != null)
				{
					gameObject.transform.position = _manager.ActiveCamera.VirtualCamera.transform.position;
					gameObject.transform.rotation = _manager.ActiveCamera.VirtualCamera.transform.rotation;
				}
			}
			GUILayout.EndHorizontal();
		}

		private void DrawPresetControls()
		{
			GUILayout.Space(5f);
			_showPresetPanel = GUILayout.Toggle(_showPresetPanel, "Presets", _buttonStyle);
			if (!_showPresetPanel)
			{
				return;
			}
			GUILayout.BeginVertical(_boxStyle);
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("DirectorPresetNameField");
			_presetName = GUILayout.TextField(_presetName, GUILayout.Width(200f));
			if (GUILayout.Button("Save", _buttonStyle, GUILayout.Width(50f)))
			{
				DirectorPresetService.SavePreset(_manager.ExportPreset(_presetName), _presetName);
				RefreshPresets();
			}
			GUILayout.EndHorizontal();
			if (_availablePresets.Length != 0)
			{
				_selectedPresetIndex = Mathf.Clamp(_selectedPresetIndex, 0, _availablePresets.Length - 1);
				_selectedPresetIndex = GUILayout.SelectionGrid(_selectedPresetIndex, _availablePresets, 2, _buttonStyle);
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Load", _buttonStyle))
				{
					DirectorPresetData directorPresetData = DirectorPresetService.LoadPreset(_availablePresets[_selectedPresetIndex]);
					if (directorPresetData != null)
					{
						_manager.ImportPreset(directorPresetData);
					}
				}
				if (GUILayout.Button("Delete", _buttonStyle))
				{
					DirectorPresetService.DeletePreset(_availablePresets[_selectedPresetIndex]);
					RefreshPresets();
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.Label("No presets saved", _labelStyle);
			}
			GUILayout.EndVertical();
		}

		private void RefreshPresets()
		{
			_availablePresets = DirectorPresetService.GetAvailablePresets();
		}

		private void DrawDepthOfFieldSettings(DirectorCameraSetup cam)
		{
			DirectorCameraData settings = cam.GetSettings();
			bool depthOfField = settings.depthOfField;
			bool flag = GUILayout.Toggle(depthOfField, " Depth of Field", _buttonStyle);
			if (flag != depthOfField)
			{
				cam.SetDepthOfFieldEnabled(flag);
			}
			if (!flag)
			{
				return;
			}
			GUILayout.Label($"Focus Distance: {settings.focusDistance:F1}m", _labelStyle);
			float num = GUILayout.HorizontalSlider(settings.focusDistance, 0.5f, 100f);
			if (Mathf.Abs(num - settings.focusDistance) > 0.05f)
			{
				cam.SetFocusDistance(num);
			}
			bool nearBlur = settings.nearBlur;
			bool flag2 = GUILayout.Toggle(nearBlur, " Near Blur (close objects)", _buttonStyle);
			if (flag2 != nearBlur)
			{
				cam.SetNearBlur(flag2);
			}
			if (flag2)
			{
				GUILayout.Label($"  Range: {settings.nearBlurRange:F1}m", _labelStyle);
				float num2 = GUILayout.HorizontalSlider(settings.nearBlurRange, 0.5f, 50f);
				if (Mathf.Abs(num2 - settings.nearBlurRange) > 0.05f)
				{
					cam.SetNearBlurRange(num2);
				}
			}
			bool farBlur = settings.farBlur;
			bool flag3 = GUILayout.Toggle(farBlur, " Far Blur (distant objects)", _buttonStyle);
			if (flag3 != farBlur)
			{
				cam.SetFarBlur(flag3);
			}
			if (flag3)
			{
				GUILayout.Label($"  Range: {settings.farBlurRange:F1}m", _labelStyle);
				float num3 = GUILayout.HorizontalSlider(settings.farBlurRange, 0.5f, 100f);
				if (Mathf.Abs(num3 - settings.farBlurRange) > 0.05f)
				{
					cam.SetFarBlurRange(num3);
				}
			}
			bool autoFocusOnTarget = settings.autoFocusOnTarget;
			bool flag4 = GUILayout.Toggle(autoFocusOnTarget, " Auto Focus on Target", _buttonStyle);
			if (flag4 != autoFocusOnTarget)
			{
				cam.SetAutoFocusOnTarget(flag4);
			}
		}

		private void ScanTargets(Transform cameraTransform)
		{
			_targetCandidates.Clear();
			_showTargetPicker = false;
			if (cameraTransform == null)
			{
				return;
			}
			RaycastHit[] array = Physics.SphereCastAll(cameraTransform.position, 3f, cameraTransform.forward, 500f);
			HashSet<Transform> hashSet = new HashSet<Transform>();
			RaycastHit[] array2 = array;
			foreach (RaycastHit raycastHit in array2)
			{
				Transform transform = raycastHit.transform;
				if (!(transform == cameraTransform) && hashSet.Add(transform))
				{
					_targetCandidates.Add(transform);
				}
			}
			if (_targetCandidates.Count > 0)
			{
				_showTargetPicker = true;
			}
		}

		private bool DrawTargetPicker(Transform currentTarget, bool hasTarget, Action<Transform> onAssign, Action onClear)
		{
			if (hasTarget && currentTarget != null)
			{
				GUILayout.Label("Target: " + currentTarget.name, _labelStyle);
				if (GUILayout.Button("Clear Target", _buttonStyle))
				{
					onClear();
				}
			}
			else
			{
				GUILayout.Label("Target: None", _labelStyle);
			}
			DirectorCameraSetup directorCameraSetup = _manager.Cameras[_selectedCameraIndex];
			if (GUILayout.Button("Scan for Targets", _buttonStyle) && directorCameraSetup.VirtualCamera != null)
			{
				_onTargetPicked = onAssign;
				ScanTargets(directorCameraSetup.VirtualCamera.transform);
			}
			if (!_showTargetPicker || _targetCandidates.Count == 0)
			{
				return false;
			}
			GUILayout.Label($"Found {_targetCandidates.Count} objects:", _labelStyle);
			_targetPickerScroll = GUILayout.BeginScrollView(_targetPickerScroll, _boxStyle, GUILayout.Height(Mathf.Min((float)_targetCandidates.Count * 26f, 130f)));
			for (int num = _targetCandidates.Count - 1; num >= 0; num--)
			{
				if (_targetCandidates[num] == null)
				{
					_targetCandidates.RemoveAt(num);
				}
				else
				{
					Transform transform = _targetCandidates[num];
					float num2 = ((directorCameraSetup.VirtualCamera != null) ? Vector3.Distance(directorCameraSetup.VirtualCamera.transform.position, transform.position) : 0f);
					if (GUILayout.Button($"{transform.name}  ({num2:F1}m)", _buttonStyle))
					{
						_onTargetPicked?.Invoke(transform);
						_showTargetPicker = false;
						_targetCandidates.Clear();
					}
				}
			}
			GUILayout.EndScrollView();
			return true;
		}

		private static Texture2D MakeTex(int width, int height, Color color)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}
	}
}
