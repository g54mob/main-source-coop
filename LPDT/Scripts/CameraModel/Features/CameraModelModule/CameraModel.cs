using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Features.CameraModelModule
{
	public class CameraModel
	{
		private readonly CameraSettingsConfiguration _cameraSettingsConfiguration;

		private readonly List<float> _sensitivityModifiers = new List<float>();

		private readonly Dictionary<CameraType, HashSet<CameraReasonEnum>> _cameraReasons = new Dictionary<CameraType, HashSet<CameraReasonEnum>>();

		private readonly Dictionary<CameraType, float> _cameraReasonMultipliers = new Dictionary<CameraType, float>();

		private readonly HashSet<LockCameraInputReasonEnum> _cameraInputLockReasons = new HashSet<LockCameraInputReasonEnum>();

		private readonly HashSet<PerlinDisableReasonEnum> _perlinDisableReasons = new HashSet<PerlinDisableReasonEnum>();

		private readonly HashSet<ZoomInputDisableReasonEnum> _zoomInputDisableReasons = new HashSet<ZoomInputDisableReasonEnum>();

		private readonly Dictionary<CameraType, CameraControllerBase> _cameras = new Dictionary<CameraType, CameraControllerBase>();

		private Camera _cameraObject;

		private float _sensitivity;

		private bool _isYAxisInverted;

		private float _baseSensitivityMultiplier = 1f;

		private CinemachineBrain _cachedBrain;

		public IReadOnlyDictionary<CameraType, CameraControllerBase> Cameras => _cameras;

		public CameraType ActiveCameraType { get; private set; } = CameraType.FPCamera;

		public Camera CameraObject
		{
			get
			{
				return _cameraObject;
			}
			set
			{
				_cameraObject = value;
				this.OnCameraObjectChanged?.Invoke(value);
			}
		}

		public Transform AimTransformOverride { get; set; }

		public Transform AimTransform
		{
			get
			{
				if (!(AimTransformOverride != null))
				{
					if (!(_cameraObject != null))
					{
						return null;
					}
					return _cameraObject.transform;
				}
				return AimTransformOverride;
			}
		}

		public CinemachineBrain CameraBrain
		{
			get
			{
				if (_cachedBrain == null)
				{
					if (_cameraObject == null)
					{
						return null;
					}
					_cameraObject.TryGetComponent<CinemachineBrain>(out _cachedBrain);
				}
				return _cachedBrain;
			}
		}

		public bool IsBlending
		{
			get
			{
				if (CameraBrain != null)
				{
					return CameraBrain.IsBlending;
				}
				return false;
			}
		}

		public float Sensitivity
		{
			get
			{
				return _sensitivity;
			}
			set
			{
				_sensitivity = value;
				UpdateSensitivity();
			}
		}

		public bool IsYAxisInverted
		{
			get
			{
				return _isYAxisInverted;
			}
			set
			{
				_isYAxisInverted = value;
				UpdateSensitivity();
			}
		}

		public float BaseSensitivityMultiplier
		{
			get
			{
				return _baseSensitivityMultiplier;
			}
			set
			{
				_baseSensitivityMultiplier = value;
				UpdateSensitivity();
			}
		}

		public event Action<CameraType> OnActiveCameraChanged;

		public event Action<CameraType, CameraType> OnCameraTransitionEnqueued;

		public event Action<Camera> OnCameraObjectChanged;

		public CameraModel(CameraSettingsConfiguration cameraSettingsConfiguration)
		{
			_cameraSettingsConfiguration = cameraSettingsConfiguration;
		}

		public void RegisterCamera(CameraType cameraType, CameraControllerBase controller)
		{
			_cameras[cameraType] = controller;
			ApplyCameraReasons();
			ApplyPerlinDisableReasons();
			ApplyZoomInputDisableReasons();
		}

		public void AddSensitivityModifier(float multiplier)
		{
			_sensitivityModifiers.Add(multiplier);
			UpdateSensitivity();
		}

		public void RemoveSensitivityModifier(float multiplier)
		{
			_sensitivityModifiers.Remove(multiplier);
			UpdateSensitivity();
		}

		public void ClearSensitivityModifiers()
		{
			_sensitivityModifiers.Clear();
			UpdateSensitivity();
		}

		private float GetEffectiveSensitivityMultiplier()
		{
			float num = _baseSensitivityMultiplier;
			foreach (float sensitivityModifier in _sensitivityModifiers)
			{
				num *= sensitivityModifier;
			}
			return num;
		}

		public void UpdateSensitivity()
		{
			float effectiveSensitivityMultiplier = GetEffectiveSensitivityMultiplier();
			float num = (_isYAxisInverted ? 1f : (-1f));
			foreach (KeyValuePair<CameraType, CameraControllerBase> camera in Cameras)
			{
				float num2 = 1f;
				if (_cameraSettingsConfiguration.CamerasSensitivityMultipliers.TryGetValue(camera.Key, out var value))
				{
					num2 = value;
				}
				camera.Value.SetXSensitivity(20f * (_sensitivity * effectiveSensitivityMultiplier) / 100f * num2);
				camera.Value.SetYSensitivity(num * 20f * (_sensitivity * effectiveSensitivityMultiplier) / 100f * num2);
			}
		}

		public void AddCameraInputLockReason(LockCameraInputReasonEnum reason)
		{
			if (reason != LockCameraInputReasonEnum.None && _cameraInputLockReasons.Add(reason))
			{
				ApplyCameraInputLockReasons();
			}
		}

		public void RemoveCameraInputLockReason(LockCameraInputReasonEnum reason)
		{
			if (reason != LockCameraInputReasonEnum.None && _cameraInputLockReasons.Remove(reason))
			{
				ApplyCameraInputLockReasons();
			}
		}

		public bool HasCameraInputLockReason(LockCameraInputReasonEnum reason)
		{
			return _cameraInputLockReasons.Contains(reason);
		}

		public void AddPerlinDisableReason(PerlinDisableReasonEnum reason)
		{
			if (reason != PerlinDisableReasonEnum.None && _perlinDisableReasons.Add(reason))
			{
				ApplyPerlinDisableReasons();
			}
		}

		public void RemovePerlinDisableReason(PerlinDisableReasonEnum reason)
		{
			if (reason != PerlinDisableReasonEnum.None && _perlinDisableReasons.Remove(reason))
			{
				ApplyPerlinDisableReasons();
			}
		}

		public bool HasPerlinDisableReason(PerlinDisableReasonEnum reason)
		{
			return _perlinDisableReasons.Contains(reason);
		}

		public void AddZoomInputDisableReason(ZoomInputDisableReasonEnum reason)
		{
			if (reason != ZoomInputDisableReasonEnum.None && _zoomInputDisableReasons.Add(reason))
			{
				ApplyZoomInputDisableReasons();
			}
		}

		public void RemoveZoomInputDisableReason(ZoomInputDisableReasonEnum reason)
		{
			if (reason != ZoomInputDisableReasonEnum.None && _zoomInputDisableReasons.Remove(reason))
			{
				ApplyZoomInputDisableReasons();
			}
		}

		public bool HasZoomInputDisableReason(ZoomInputDisableReasonEnum reason)
		{
			return _zoomInputDisableReasons.Contains(reason);
		}

		private void ApplyZoomInputDisableReasons()
		{
			bool zoomInputEnabled = _zoomInputDisableReasons.Count == 0;
			foreach (CameraControllerBase value in _cameras.Values)
			{
				value.SetZoomInputEnabled(zoomInputEnabled);
			}
		}

		private void ApplyPerlinDisableReasons()
		{
			bool perlinActive = _perlinDisableReasons.Count == 0;
			foreach (CameraControllerBase value in _cameras.Values)
			{
				value.SetPerlinActive(perlinActive).Forget();
			}
		}

		private void ApplyCameraInputLockReasons()
		{
			SetRotationBlocked(_cameraInputLockReasons.Count > 0);
		}

		private void SetRotationBlocked(bool isBlocked)
		{
			foreach (CameraControllerBase value in Cameras.Values)
			{
				value.SetInputEnabled(!isBlocked);
			}
		}

		public void AddCameraReason(CameraType cameraType, CameraReasonEnum reason)
		{
			if (reason != CameraReasonEnum.None)
			{
				if (!_cameraReasons.TryGetValue(cameraType, out var value))
				{
					value = new HashSet<CameraReasonEnum>();
					_cameraReasons[cameraType] = value;
				}
				if (value.Add(reason))
				{
					ApplyCameraReasons();
				}
			}
		}

		public void RemoveCameraReason(CameraType cameraType, CameraReasonEnum reason)
		{
			if (reason != CameraReasonEnum.None && _cameraReasons.TryGetValue(cameraType, out var value) && value.Remove(reason))
			{
				ApplyCameraReasons();
			}
		}

		public bool HasCameraReason(CameraType cameraType, CameraReasonEnum reason)
		{
			if (_cameraReasons.TryGetValue(cameraType, out var value))
			{
				return value.Contains(reason);
			}
			return false;
		}

		public void SetCameraReasonMultiplier(CameraType cameraType, float multiplier)
		{
			_cameraReasonMultipliers[cameraType] = multiplier;
			ApplyCameraReasons();
		}

		public float GetCameraReasonMultiplier(CameraType cameraType)
		{
			return _cameraReasonMultipliers.GetValueOrDefault(cameraType, 1f);
		}

		private void ApplyCameraReasons()
		{
			CameraType activeCameraType = ActiveCameraType;
			CameraType cameraType = CameraType.FPCamera;
			float num = 0f;
			foreach (CameraType key in Cameras.Keys)
			{
				float cameraReasonScore = GetCameraReasonScore(key);
				if (cameraReasonScore > num)
				{
					cameraType = key;
					num = cameraReasonScore;
				}
			}
			InvokeCameraTransition(activeCameraType, cameraType);
			foreach (KeyValuePair<CameraType, CameraControllerBase> camera in Cameras)
			{
				camera.Value.SetPriority((camera.Key == cameraType) ? 1 : 0);
			}
			if (ActiveCameraType != cameraType)
			{
				ActiveCameraType = cameraType;
				this.OnActiveCameraChanged?.Invoke(cameraType);
			}
		}

		private float GetCameraReasonScore(CameraType cameraType)
		{
			return (float)GetHighestCameraReason(cameraType) * GetCameraReasonMultiplier(cameraType);
		}

		private CameraReasonEnum GetHighestCameraReason(CameraType cameraType)
		{
			if (!_cameraReasons.TryGetValue(cameraType, out var value))
			{
				return CameraReasonEnum.None;
			}
			CameraReasonEnum cameraReasonEnum = CameraReasonEnum.None;
			foreach (CameraReasonEnum item in value)
			{
				if (item > cameraReasonEnum)
				{
					cameraReasonEnum = item;
				}
			}
			return cameraReasonEnum;
		}

		private void InvokeCameraTransition(CameraType currentType, CameraType transitedType)
		{
			this.OnCameraTransitionEnqueued?.Invoke(currentType, transitedType);
		}

		public void ResetSessionState()
		{
			_cameras.Clear();
			_cameraReasons.Clear();
			_cameraReasonMultipliers.Clear();
			_cameraInputLockReasons.Clear();
			_perlinDisableReasons.Clear();
			_zoomInputDisableReasons.Clear();
			_sensitivityModifiers.Clear();
			_cameraObject = null;
			_cachedBrain = null;
			ActiveCameraType = CameraType.FPCamera;
		}
	}
}
