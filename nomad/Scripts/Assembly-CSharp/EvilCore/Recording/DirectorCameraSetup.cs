using System;
using System.Collections.Generic;
using EvilCore.Managers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EvilCore.Recording
{
	public class DirectorCameraSetup : MonoBehaviour
	{
		private const int PriorityActive = 20;

		private const int PriorityInactive = 0;

		private string _cameraName = "Camera";

		private DirectorCameraBehavior _behaviorType;

		private float _holdDuration = 5f;

		private TransitionType _transitionType = TransitionType.Blend;

		private float _blendDuration = 1f;

		private CinemachineCamera _vcam;

		private IBehaviorConfigurator _configurator;

		private DirectorCameraData _currentData = new DirectorCameraData();

		private Volume _dofVolume;

		private DepthOfField _depthOfField;

		private static readonly Dictionary<DirectorCameraBehavior, Func<IBehaviorConfigurator>> ConfiguratorFactory = new Dictionary<DirectorCameraBehavior, Func<IBehaviorConfigurator>>
		{
			{
				DirectorCameraBehavior.Static,
				() => new StaticBehavior()
			},
			{
				DirectorCameraBehavior.Orbit,
				() => new OrbitBehavior()
			},
			{
				DirectorCameraBehavior.Dolly,
				() => new DollyBehavior()
			},
			{
				DirectorCameraBehavior.Follow,
				() => new FollowBehavior()
			},
			{
				DirectorCameraBehavior.PanTilt,
				() => new PanTiltBehavior()
			},
			{
				DirectorCameraBehavior.Zoom,
				() => new ZoomBehavior()
			},
			{
				DirectorCameraBehavior.Handheld,
				() => new HandheldBehavior()
			},
			{
				DirectorCameraBehavior.Turntable,
				() => new TurntableBehavior()
			}
		};

		private bool _isPositioning;

		private float _pitch;

		private float _yaw;

		private float _moveSpeed = 10f;

		private float _fastMoveMultiplier = 3f;

		private float _shiftGrowthRate = 1.4f;

		private float _mouseSensitivity = 3f;

		private float _scrollSensitivity = 5f;

		private float _accelerationRate = 20f;

		private float _decelerationRate = 40f;

		private float _currentSpeed;

		private float _shiftHoldTime;

		public string CameraName
		{
			get
			{
				return _cameraName;
			}
			set
			{
				_cameraName = value;
			}
		}

		public DirectorCameraBehavior BehaviorType => _behaviorType;

		public CinemachineCamera VirtualCamera => _vcam;

		public float HoldDuration
		{
			get
			{
				return _holdDuration;
			}
			set
			{
				_holdDuration = value;
			}
		}

		public TransitionType TransitionType
		{
			get
			{
				return _transitionType;
			}
			set
			{
				_transitionType = value;
			}
		}

		public float BlendDuration
		{
			get
			{
				return _blendDuration;
			}
			set
			{
				_blendDuration = value;
			}
		}

		public IBehaviorConfigurator Configurator => _configurator;

		public float Smoothing
		{
			get
			{
				return _currentData.smoothing;
			}
			set
			{
				_currentData.smoothing = value;
				_configurator?.SetSmoothing(value);
			}
		}

		public float HiddenSchemaInputSpeed
		{
			get
			{
				return _currentData.hiddenSchemaInputSpeed;
			}
			set
			{
				_currentData.hiddenSchemaInputSpeed = value;
			}
		}

		public bool ExecuteOnHide
		{
			get
			{
				return _currentData.executeOnHide;
			}
			set
			{
				_currentData.executeOnHide = value;
			}
		}

		public bool IsPositioning => _isPositioning;

		public void StartPositioning()
		{
			if (!(_vcam == null))
			{
				_isPositioning = true;
				_currentSpeed = _moveSpeed;
				_shiftHoldTime = 0f;
				Vector3 eulerAngles = _vcam.transform.eulerAngles;
				_yaw = eulerAngles.y;
				_pitch = eulerAngles.x;
				if (_pitch > 180f)
				{
					_pitch -= 360f;
				}
				GameCursor.Lock();
			}
		}

		public void StopPositioning()
		{
			_isPositioning = false;
			_shiftHoldTime = 0f;
			_currentData.position = _vcam.transform.position;
			_currentData.rotation = _vcam.transform.rotation;
			_configurator?.OnPositionUpdated(_vcam);
			GameCursor.Unlock();
		}

		public void UpdatePositioning()
		{
			if (_isPositioning && !(_vcam == null))
			{
				float num = Input.GetAxis("Mouse X") * _mouseSensitivity;
				float num2 = Input.GetAxis("Mouse Y") * _mouseSensitivity;
				_yaw += num;
				_pitch -= num2;
				_pitch = Mathf.Clamp(_pitch, -89f, 89f);
				Transform transform = _vcam.transform;
				transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
				float unscaledDeltaTime = Time.unscaledDeltaTime;
				bool key = Input.GetKey(KeyCode.LeftShift);
				if (key)
				{
					_shiftHoldTime += unscaledDeltaTime;
				}
				else
				{
					_shiftHoldTime = 0f;
				}
				float num4;
				if (key)
				{
					float num3 = 1f + _shiftHoldTime * _shiftGrowthRate + _shiftHoldTime * _shiftHoldTime * _shiftGrowthRate * 0.5f;
					num4 = _moveSpeed * _fastMoveMultiplier * num3;
				}
				else
				{
					num4 = _moveSpeed;
				}
				float num5 = ((_currentSpeed < num4) ? _accelerationRate : _decelerationRate);
				_currentSpeed = Mathf.MoveTowards(_currentSpeed, num4, num5 * unscaledDeltaTime);
				Vector3 zero = Vector3.zero;
				if (Input.GetKey(KeyCode.W))
				{
					zero += transform.forward;
				}
				if (Input.GetKey(KeyCode.S))
				{
					zero -= transform.forward;
				}
				if (Input.GetKey(KeyCode.D))
				{
					zero += transform.right;
				}
				if (Input.GetKey(KeyCode.A))
				{
					zero -= transform.right;
				}
				if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
				{
					zero += Vector3.up;
				}
				if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.C))
				{
					zero -= Vector3.up;
				}
				if (zero.sqrMagnitude > 0f)
				{
					transform.position += zero.normalized * (_currentSpeed * unscaledDeltaTime);
				}
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (Mathf.Abs(axis) > 0.001f)
				{
					_moveSpeed = Mathf.Clamp(_moveSpeed + axis * _scrollSensitivity, 0.5f, 100f);
				}
				if (Input.GetMouseButtonDown(1))
				{
					StopPositioning();
				}
			}
		}

		public void Initialize(string cameraName)
		{
			_cameraName = cameraName;
			GameObject gameObject = new GameObject("VCam_" + cameraName);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			_vcam = gameObject.AddComponent<CinemachineCamera>();
			_vcam.Priority = 0;
			SetupDepthOfField(gameObject);
			SetBehaviorType(DirectorCameraBehavior.Static);
		}

		public void SetBehaviorType(DirectorCameraBehavior type)
		{
			if (!(_vcam == null))
			{
				_configurator?.Cleanup(_vcam);
				_behaviorType = type;
				_currentData.behaviorType = type;
				if (ConfiguratorFactory.TryGetValue(type, out var value))
				{
					_configurator = value();
					_configurator.Configure(_vcam, _currentData);
				}
				else
				{
					_configurator = new StaticBehavior();
					_configurator.Configure(_vcam, _currentData);
				}
			}
		}

		public void UpdateRuntime(float deltaTime)
		{
			if (!(_vcam == null) && _configurator != null)
			{
				_configurator.UpdateRuntime(_vcam, deltaTime);
				UpdateAutoFocus();
			}
		}

		public void Activate()
		{
			if (!(_vcam == null))
			{
				_vcam.Priority = 20;
				if (_dofVolume != null)
				{
					_dofVolume.enabled = _currentData.depthOfField;
				}
			}
		}

		public void Deactivate()
		{
			if (!(_vcam == null))
			{
				_vcam.Priority = 0;
				if (_dofVolume != null)
				{
					_dofVolume.enabled = false;
				}
			}
		}

		public void SetPosition(Vector3 pos, Quaternion rot)
		{
			if (!(_vcam == null))
			{
				_vcam.transform.position = pos;
				_vcam.transform.rotation = rot;
				_currentData.position = pos;
				_currentData.rotation = rot;
			}
		}

		public void SetFieldOfView(float fov)
		{
			if (!(_vcam == null))
			{
				_vcam.Lens = new LensSettings
				{
					FieldOfView = fov,
					NearClipPlane = _vcam.Lens.NearClipPlane,
					FarClipPlane = _vcam.Lens.FarClipPlane
				};
				_currentData.fieldOfView = fov;
			}
		}

		public void ApplySettings(DirectorCameraData data)
		{
			_currentData = data;
			_cameraName = data.cameraName;
			_holdDuration = data.holdDuration;
			_transitionType = data.transitionType;
			_blendDuration = data.blendDuration;
			if (_vcam != null)
			{
				_vcam.transform.position = data.position;
				_vcam.transform.rotation = data.rotation;
				SetFieldOfView(data.fieldOfView);
			}
			SetBehaviorType(data.behaviorType);
			ApplyDepthOfFieldSettings();
		}

		public DirectorCameraData GetSettings()
		{
			if (_configurator != null && _vcam != null)
			{
				bool depthOfField = _currentData.depthOfField;
				float focusDistance = _currentData.focusDistance;
				bool nearBlur = _currentData.nearBlur;
				float nearBlurRange = _currentData.nearBlurRange;
				bool farBlur = _currentData.farBlur;
				float farBlurRange = _currentData.farBlurRange;
				bool autoFocusOnTarget = _currentData.autoFocusOnTarget;
				float smoothing = _currentData.smoothing;
				float hiddenSchemaInputSpeed = _currentData.hiddenSchemaInputSpeed;
				bool executeOnHide = _currentData.executeOnHide;
				_currentData = _configurator.ExtractSettings(_vcam);
				_currentData.depthOfField = depthOfField;
				_currentData.focusDistance = focusDistance;
				_currentData.nearBlur = nearBlur;
				_currentData.nearBlurRange = nearBlurRange;
				_currentData.farBlur = farBlur;
				_currentData.farBlurRange = farBlurRange;
				_currentData.autoFocusOnTarget = autoFocusOnTarget;
				_currentData.smoothing = smoothing;
				_currentData.hiddenSchemaInputSpeed = hiddenSchemaInputSpeed;
				_currentData.executeOnHide = executeOnHide;
			}
			_currentData.cameraName = _cameraName;
			_currentData.behaviorType = _behaviorType;
			_currentData.holdDuration = _holdDuration;
			_currentData.transitionType = _transitionType;
			_currentData.blendDuration = _blendDuration;
			if (_vcam != null)
			{
				_currentData.position = _vcam.transform.position;
				_currentData.rotation = _vcam.transform.rotation;
				_currentData.fieldOfView = _vcam.Lens.FieldOfView;
			}
			return _currentData;
		}

		public void Cleanup()
		{
			_configurator?.Cleanup(_vcam);
			if (_dofVolume != null && _dofVolume.profile != null)
			{
				UnityEngine.Object.Destroy(_dofVolume.profile);
			}
			if (_vcam != null)
			{
				UnityEngine.Object.Destroy(_vcam.gameObject);
			}
		}

		private void OnDestroy()
		{
			Cleanup();
		}

		private void SetupDepthOfField(GameObject vcamGo)
		{
			_dofVolume = vcamGo.AddComponent<Volume>();
			_dofVolume.isGlobal = true;
			_dofVolume.priority = 100f;
			_dofVolume.enabled = false;
			VolumeProfile volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			_depthOfField = volumeProfile.Add<DepthOfField>();
			_depthOfField.active = true;
			_depthOfField.focusMode.overrideState = true;
			_depthOfField.focusMode.value = DepthOfFieldMode.Manual;
			_depthOfField.nearFocusStart.overrideState = true;
			_depthOfField.nearFocusEnd.overrideState = true;
			_depthOfField.farFocusStart.overrideState = true;
			_depthOfField.farFocusEnd.overrideState = true;
			_dofVolume.profile = volumeProfile;
			ApplyDepthOfFieldSettings();
		}

		public void ApplyDepthOfFieldSettings()
		{
			if (!(_depthOfField == null))
			{
				float focusDistance = _currentData.focusDistance;
				if (_currentData.nearBlur)
				{
					_depthOfField.nearFocusStart.value = Mathf.Max(focusDistance - _currentData.nearBlurRange, 0.1f);
					_depthOfField.nearFocusEnd.value = focusDistance;
				}
				else
				{
					_depthOfField.nearFocusStart.value = 0f;
					_depthOfField.nearFocusEnd.value = 0f;
				}
				if (_currentData.farBlur)
				{
					_depthOfField.farFocusStart.value = focusDistance;
					_depthOfField.farFocusEnd.value = focusDistance + _currentData.farBlurRange;
				}
				else
				{
					_depthOfField.farFocusStart.value = 10000f;
					_depthOfField.farFocusEnd.value = 10000f;
				}
				if (_dofVolume != null)
				{
					_dofVolume.enabled = _currentData.depthOfField && _vcam != null && (int)_vcam.Priority > 0;
				}
			}
		}

		private void UpdateAutoFocus()
		{
			if (!_currentData.depthOfField || !_currentData.autoFocusOnTarget)
			{
				return;
			}
			Transform transform = null;
			if (_configurator is FollowBehavior { HasTarget: not false } followBehavior)
			{
				transform = followBehavior.TargetTransform;
			}
			else if (_configurator is ZoomBehavior { HasTarget: not false } zoomBehavior)
			{
				transform = zoomBehavior.TargetTransform;
			}
			if (!(transform == null) && !(_vcam == null))
			{
				float num = Vector3.Distance(_vcam.transform.position, transform.position);
				if (Mathf.Abs(num - _currentData.focusDistance) > 0.05f)
				{
					_currentData.focusDistance = num;
					ApplyDepthOfFieldSettings();
				}
			}
		}

		public void SetDepthOfFieldEnabled(bool enabled)
		{
			_currentData.depthOfField = enabled;
			ApplyDepthOfFieldSettings();
		}

		public void SetFocusDistance(float distance)
		{
			_currentData.focusDistance = distance;
			ApplyDepthOfFieldSettings();
		}

		public void SetNearBlur(bool enabled)
		{
			_currentData.nearBlur = enabled;
			ApplyDepthOfFieldSettings();
		}

		public void SetNearBlurRange(float range)
		{
			_currentData.nearBlurRange = range;
			ApplyDepthOfFieldSettings();
		}

		public void SetFarBlur(bool enabled)
		{
			_currentData.farBlur = enabled;
			ApplyDepthOfFieldSettings();
		}

		public void SetFarBlurRange(float range)
		{
			_currentData.farBlurRange = range;
			ApplyDepthOfFieldSettings();
		}

		public void SetAutoFocusOnTarget(bool enabled)
		{
			_currentData.autoFocusOnTarget = enabled;
		}

		public static void RegisterConfigurator(DirectorCameraBehavior type, Func<IBehaviorConfigurator> factory)
		{
			ConfiguratorFactory[type] = factory;
		}
	}
}
