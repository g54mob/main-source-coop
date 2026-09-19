using System;
using Features.AnimationModule.Scripts;
using Features.CameraModelModule;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayersEyeFocusModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class PlayerEyesRotator : NetworkBehaviour
	{
		private static readonly int _eyesHorizontal = Animator.StringToHash("EyesHorizontal");

		private static readonly int _eyesVertical = Animator.StringToHash("EyesVertical");

		[Header("References")]
		[SerializeField]
		private CompositeAnimator _animator;

		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private Transform _eyesPositionTransform;

		[Header("Rotation Limits")]
		[SerializeField]
		private float _eyesHorizontalLimit = 30f;

		[SerializeField]
		private float _eyesVerticalLimitTop = 20f;

		[SerializeField]
		private float _eyesVerticalLimitBottom = -20f;

		[Header("Distance Settings")]
		[SerializeField]
		private float _maxTargetDistance = 50f;

		[Header("Lerp Settings")]
		[SerializeField]
		private float _eyesLerpSpeed = 10f;

		[Header("Default Values")]
		[SerializeField]
		private float _defaultHorizontalValue;

		[SerializeField]
		private float _defaultVerticalValue;

		[Header("Camera Follow Behavior (No Target)")]
		[SerializeField]
		private bool _enableCameraFollow = true;

		[SerializeField]
		private float _cameraFollowStrength = 0.3f;

		[SerializeField]
		private float _cameraFollowSmoothing = 5f;

		[SerializeField]
		private float _cameraFollowDecaySpeed = 2f;

		[Header("Debug Visualization")]
		[SerializeField]
		private bool _showDebugGizmos = true;

		[SerializeField]
		private Color _targetLineColor = Color.green;

		[SerializeField]
		private Color _targetSphereColor = Color.yellow;

		[SerializeField]
		private Color _maxDistanceSphereColor = Color.red;

		[SerializeField]
		private float _targetSphereRadius = 0.3f;

		private PlayerEyesTargetsModel _targetsModel;

		private CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		[WeaverGenerated]
		[DefaultForProperty("_horizontalQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int __horizontalQuantized;

		[WeaverGenerated]
		[DefaultForProperty("_verticalQuantized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int __verticalQuantized;

		private const float QUANTIZE_STEP = 0.01f;

		private float _lerpedHorizontalValue;

		private float _lerpedVerticalValue;

		private EyesTargetData _currentTarget;

		private float _previousCameraYRotation;

		private float _previousCameraXRotation;

		private float _cameraFollowHorizontalOffset;

		private float _cameraFollowVerticalOffset;

		private bool _isSpawned;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int _horizontalQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerEyesRotator._horizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerEyesRotator._horizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int _verticalQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerEyesRotator._verticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerEyesRotator._verticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Inject]
		private void InjectDependencies(PlayerEyesTargetsModel targetsModel, CameraModel cameraModel, MultiplayerModel multiplayerModel)
		{
			_targetsModel = targetsModel;
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
			_targetsModel.OnEyesTargetAdded += OnTargetAdded;
			_targetsModel.OnEyesTargetRemoved += OnTargetRemoved;
			Camera camera = _cameraModel?.CameraObject;
			if (camera != null)
			{
				Vector3 eulerAngles = camera.transform.rotation.eulerAngles;
				_previousCameraYRotation = eulerAngles.y;
				_previousCameraXRotation = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_isSpawned = false;
			_targetsModel.OnEyesTargetAdded -= OnTargetAdded;
			_targetsModel.OnEyesTargetRemoved -= OnTargetRemoved;
		}

		private int Quantize(float value)
		{
			return Mathf.RoundToInt(value / 0.01f);
		}

		private float Dequantize(int value)
		{
			return (float)value * 0.01f;
		}

		private void OnTargetAdded(EyesTargetData target)
		{
		}

		private void OnTargetRemoved(EyesTargetData target)
		{
			if (_currentTarget == target)
			{
				_currentTarget = null;
			}
		}

		private void LateUpdate()
		{
			if (_multiplayerModel?.NetworkRunner == null || !_isSpawned)
			{
				return;
			}
			if (_multiplayerModel.NetworkRunner.LocalPlayer != _networkObject.StateAuthority)
			{
				if (_animator != null)
				{
					_lerpedHorizontalValue = Mathf.Lerp(_lerpedHorizontalValue, Dequantize(_horizontalQuantized), Time.deltaTime * _eyesLerpSpeed);
					_lerpedVerticalValue = Mathf.Lerp(_lerpedVerticalValue, Dequantize(_verticalQuantized), Time.deltaTime * _eyesLerpSpeed);
					_animator.SetFloat(_eyesHorizontal, _lerpedHorizontalValue);
					_animator.SetFloat(_eyesVertical, _lerpedVerticalValue);
				}
				return;
			}
			Camera camera = _cameraModel?.CameraObject;
			if (!(camera == null) && !(_animator == null))
			{
				EyesTargetData eyesTargetData = FindBestTarget(camera);
				if (eyesTargetData != null && eyesTargetData.Transform != null)
				{
					_currentTarget = eyesTargetData;
					UpdateEyesRotation(camera, eyesTargetData.Transform.position);
					_cameraFollowHorizontalOffset = 0f;
					_cameraFollowVerticalOffset = 0f;
				}
				else
				{
					UpdateCameraFollowBehavior(camera);
				}
				ApplyLerpedValues();
			}
		}

		private Vector3 GetEyesPosition(Camera cam)
		{
			if (!(_eyesPositionTransform != null))
			{
				return cam.transform.position;
			}
			return _eyesPositionTransform.position;
		}

		private EyesTargetData FindBestTarget(Camera cam)
		{
			if (_targetsModel?.EyesTargets == null || _targetsModel.EyesTargets.Count == 0)
			{
				return null;
			}
			Vector3 eyesPosition = GetEyesPosition(cam);
			Vector3 forward = cam.transform.forward;
			Vector3 right = cam.transform.right;
			Vector3 up = cam.transform.up;
			EyesTargetData result = null;
			int num = int.MinValue;
			float num2 = float.MaxValue;
			foreach (EyesTargetData eyesTarget in _targetsModel.EyesTargets)
			{
				if (eyesTarget?.Transform == null)
				{
					continue;
				}
				float num3 = Vector3.Distance(eyesPosition, eyesTarget.Transform.position);
				if (num3 > _maxTargetDistance)
				{
					continue;
				}
				Vector3 normalized = (eyesTarget.Transform.position - eyesPosition).normalized;
				Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, up).normalized;
				float num4 = Vector3.SignedAngle(forward, normalized2, up);
				Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, right).normalized;
				float num5 = Vector3.SignedAngle(forward, normalized3, right);
				if (!(num4 < 0f - _eyesHorizontalLimit) && !(num4 > _eyesHorizontalLimit) && !(num5 < _eyesVerticalLimitBottom) && !(num5 > _eyesVerticalLimitTop))
				{
					if (eyesTarget.Priority > num)
					{
						result = eyesTarget;
						num = eyesTarget.Priority;
						num2 = num3;
					}
					else if (eyesTarget.Priority == num && num3 < num2)
					{
						result = eyesTarget;
						num2 = num3;
					}
				}
			}
			return result;
		}

		private void UpdateEyesRotation(Camera cam, Vector3 targetPosition)
		{
			Vector3 eyesPosition = GetEyesPosition(cam);
			Vector3 forward = cam.transform.forward;
			Vector3 right = cam.transform.right;
			Vector3 up = cam.transform.up;
			Vector3 normalized = (targetPosition - eyesPosition).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, up).normalized;
			float value = Vector3.SignedAngle(forward, normalized2, up);
			value = Mathf.Clamp(value, 0f - _eyesHorizontalLimit, _eyesHorizontalLimit);
			Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, right).normalized;
			float value2 = Vector3.SignedAngle(forward, normalized3, right);
			value2 = Mathf.Clamp(value2, _eyesVerticalLimitBottom, _eyesVerticalLimitTop);
			float value3 = Mathf.InverseLerp(0f - _eyesHorizontalLimit, _eyesHorizontalLimit, value) * 2f - 1f;
			float value4 = Mathf.InverseLerp(_eyesVerticalLimitBottom, _eyesVerticalLimitTop, value2) * 2f - 1f;
			_horizontalQuantized = Quantize(value3);
			_verticalQuantized = Quantize(value4);
		}

		private void UpdateCameraFollowBehavior(Camera cam)
		{
			if (!_enableCameraFollow)
			{
				ReturnToDefault();
				return;
			}
			Vector3 eulerAngles = cam.transform.rotation.eulerAngles;
			float num = eulerAngles.x;
			if (num > 180f)
			{
				num -= 360f;
			}
			float y = eulerAngles.y;
			float num2 = Mathf.DeltaAngle(_previousCameraYRotation, y);
			float num3 = num - _previousCameraXRotation;
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			else if (num3 < -180f)
			{
				num3 += 360f;
			}
			float value = Mathf.Clamp(num2, 0f - _eyesHorizontalLimit, _eyesHorizontalLimit);
			float value2 = Mathf.Clamp(0f - num3, _eyesVerticalLimitBottom, _eyesVerticalLimitTop);
			float num4 = Mathf.InverseLerp(0f - _eyesHorizontalLimit, _eyesHorizontalLimit, value) * 2f - 1f;
			float num5 = Mathf.InverseLerp(_eyesVerticalLimitBottom, _eyesVerticalLimitTop, value2) * 2f - 1f;
			float b = num4 * _cameraFollowStrength;
			float b2 = num5 * _cameraFollowStrength;
			if (Mathf.Abs(num2) < 0.1f && Mathf.Abs(num3) < 0.1f)
			{
				b = 0f;
				b2 = 0f;
			}
			_cameraFollowHorizontalOffset = Mathf.Lerp(_cameraFollowHorizontalOffset, b, Time.deltaTime * ((Mathf.Abs(num2) > 0.1f) ? _cameraFollowSmoothing : _cameraFollowDecaySpeed));
			_cameraFollowVerticalOffset = Mathf.Lerp(_cameraFollowVerticalOffset, b2, Time.deltaTime * ((Mathf.Abs(num3) > 0.1f) ? _cameraFollowSmoothing : _cameraFollowDecaySpeed));
			_horizontalQuantized = Quantize(_defaultHorizontalValue + _cameraFollowHorizontalOffset);
			_verticalQuantized = Quantize(_defaultVerticalValue + _cameraFollowVerticalOffset);
			_previousCameraYRotation = y;
			_previousCameraXRotation = num;
		}

		private void ReturnToDefault()
		{
			_horizontalQuantized = Quantize(_defaultHorizontalValue);
			_verticalQuantized = Quantize(_defaultVerticalValue);
		}

		private void ApplyLerpedValues()
		{
			if (!(_animator == null))
			{
				_lerpedHorizontalValue = Mathf.Lerp(_lerpedHorizontalValue, Dequantize(_horizontalQuantized), Time.deltaTime * _eyesLerpSpeed);
				_lerpedVerticalValue = Mathf.Lerp(_lerpedVerticalValue, Dequantize(_verticalQuantized), Time.deltaTime * _eyesLerpSpeed);
				_animator.SetFloat(_eyesHorizontal, _lerpedHorizontalValue);
				_animator.SetFloat(_eyesVertical, _lerpedVerticalValue);
			}
		}

		private void OnDrawGizmos()
		{
			if (!_showDebugGizmos)
			{
				return;
			}
			Camera camera = _cameraModel?.CameraObject;
			if (camera == null)
			{
				return;
			}
			Vector3 eyesPosition = GetEyesPosition(camera);
			Gizmos.color = _maxDistanceSphereColor;
			Gizmos.DrawWireSphere(eyesPosition, _maxTargetDistance);
			if (_targetsModel?.EyesTargets != null)
			{
				foreach (EyesTargetData eyesTarget in _targetsModel.EyesTargets)
				{
					if (!(eyesTarget?.Transform == null))
					{
						bool flag = Vector3.Distance(eyesPosition, eyesTarget.Transform.position) <= _maxTargetDistance;
						bool flag2 = _currentTarget == eyesTarget;
						Gizmos.color = (flag2 ? _targetSphereColor : (flag ? Color.cyan : Color.gray));
						Gizmos.DrawWireSphere(eyesTarget.Transform.position, _targetSphereRadius);
						if (flag)
						{
							Gizmos.color = (flag2 ? _targetLineColor : Color.cyan);
							Gizmos.DrawLine(eyesPosition, eyesTarget.Transform.position);
						}
					}
				}
			}
			if (_currentTarget != null && _currentTarget.Transform != null)
			{
				Gizmos.color = _targetLineColor;
				Gizmos.DrawLine(eyesPosition, _currentTarget.Transform.position);
				Gizmos.color = _targetSphereColor;
				Gizmos.DrawWireSphere(_currentTarget.Transform.position, _targetSphereRadius * 1.5f);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			_horizontalQuantized = __horizontalQuantized;
			_verticalQuantized = __verticalQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			__horizontalQuantized = _horizontalQuantized;
			__verticalQuantized = _verticalQuantized;
		}
	}
}
