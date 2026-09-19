using System;
using Features.AnimationModule.Scripts;
using Features.CameraModelModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class PlayerRotator : NetworkBehaviour
	{
		private const float BYTES_FROM_ZERO_TO_ONE = 255f;

		[Header("References")]
		[SerializeField]
		private CompositeAnimator _animator;

		[SerializeField]
		private GameObject _allObject;

		[SerializeField]
		private GameObject _lookPoint;

		[SerializeField]
		private NetworkObject _networkObject;

		[Header("Rotation Limits")]
		[SerializeField]
		private float _headLimitationXTop = 60f;

		[SerializeField]
		private float _headLimitationXBottom = -60f;

		[SerializeField]
		private float _headLimitationY = 60f;

		private static readonly int _isCrouchName = Animator.StringToHash("IsCrouch");

		private readonly string _forwardParameterName = "Forward";

		private readonly string _sideParameterName = "Side";

		[Header("Default Values")]
		[SerializeField]
		private float _defaultSideValue = 0.5f;

		[Header("Reset Lerp Settings")]
		[SerializeField]
		private float _resetAnimatorLerpSpeed = 10f;

		[SerializeField]
		private float _resetRotationLerpSpeed = 10f;

		[SerializeField]
		private float _ragdollRotationForce = 3.5f;

		[WeaverGenerated]
		[DefaultForProperty("_forwardQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte __forwardQuantized;

		[WeaverGenerated]
		[DefaultForProperty("_sideQuantized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte __sideQuantized;

		private CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		private PlayerMovableModel _playerMovableModel;

		private PlayersRagdollModel _playersRagdollModel;

		private float _currentBodyYRotation;

		private float _prevBodyYRotation;

		private float _lerpedForward;

		private float _lerpedSide;

		private byte _pendingForwardQuantized;

		private byte _pendingSideQuantized;

		private PlayerRagdollEntity _playerRagdollEntity;

		private bool _isResetRotation;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe byte _forwardQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerRotator._forwardQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[0];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerRotator._forwardQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[0] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe byte _sideQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerRotator._sideQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerRotator._sideQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[4] = (sbyte)value;
			}
		}

		public Transform RotationObject { get; set; }

		public Transform BodyYawObject
		{
			get
			{
				if (!(_allObject != null))
				{
					return null;
				}
				return _allObject.transform;
			}
		}

		public bool IsResetRotation
		{
			get
			{
				return _isResetRotation;
			}
			set
			{
				_isResetRotation = value;
			}
		}

		public void AddBodyYaw(float deltaDegrees)
		{
			_currentBodyYRotation += deltaDegrees;
		}

		[Inject]
		public void InjectDependencies(CameraModel cameraModel, MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel, PlayersRagdollModel playersRagdollModel)
		{
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_playersRagdollModel = playersRagdollModel;
		}

		private byte Quantize01(float value)
		{
			value = Mathf.Clamp01(value);
			return (byte)Mathf.RoundToInt(value * 255f);
		}

		private float Dequantize01(byte value)
		{
			return (float)(int)value / 255f;
		}

		public override void Spawned()
		{
			if (base.HasInputAuthority)
			{
				_playerMovableModel.Rotator = this;
			}
			if (_playersRagdollModel.PlayersRagdoll.ContainsKey(base.Object.StateAuthority.PlayerId))
			{
				_playerRagdollEntity = _playersRagdollModel.PlayersRagdoll[base.Object.StateAuthority.PlayerId];
			}
			else
			{
				_playersRagdollModel.OnPlayerRagdollAdded += InitializeRagdoll;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playersRagdollModel.OnPlayerRagdollAdded -= InitializeRagdoll;
			if ((object)_playerMovableModel.Rotator == this)
			{
				_playerMovableModel.Rotator = null;
			}
		}

		private void InitializeRagdoll(int playerId, PlayerRagdollEntity entity)
		{
			if (base.Object.StateAuthority.PlayerId == playerId)
			{
				_playerRagdollEntity = entity;
			}
		}

		public void LateUpdate()
		{
			if (!(_networkObject == null))
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer != _networkObject.InputAuthority)
				{
					UpdateProxy();
				}
				else if (_playerMovableModel.IsLookingAtCamera)
				{
					LookAtCamera();
				}
				else
				{
					LookByCamera();
				}
			}
		}

		private void UpdateProxy()
		{
			float b = Dequantize01(_forwardQuantized);
			float b2 = Dequantize01(_sideQuantized);
			float num = 10f;
			_lerpedForward = Mathf.Lerp(_lerpedForward, b, Time.deltaTime * num);
			_lerpedSide = Mathf.Lerp(_lerpedSide, b2, Time.deltaTime * num);
			_animator.SetFloat(_forwardParameterName, _lerpedForward);
			_animator.SetFloat(_sideParameterName, _lerpedSide);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				if (_pendingForwardQuantized != _forwardQuantized)
				{
					_forwardQuantized = _pendingForwardQuantized;
				}
				if (_pendingSideQuantized != _sideQuantized)
				{
					_sideQuantized = _pendingSideQuantized;
				}
			}
		}

		private void ApplyAndSync(float forward, float side)
		{
			_animator.SetFloat(_forwardParameterName, forward);
			_animator.SetFloat(_sideParameterName, side);
			_pendingForwardQuantized = Quantize01(forward);
			_pendingSideQuantized = Quantize01(side);
		}

		private void LookByCamera()
		{
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject == null)
			{
				return;
			}
			Vector3 eulerAngles = cameraObject.transform.rotation.eulerAngles;
			float num = eulerAngles.x;
			if (num > 180f)
			{
				num -= 360f;
			}
			num = Mathf.Clamp(num, _headLimitationXBottom, _headLimitationXTop);
			float y = eulerAngles.y;
			float num2 = Mathf.DeltaAngle(_currentBodyYRotation, y);
			float num3;
			float num4;
			if (Mathf.Abs(num2) <= _headLimitationY)
			{
				num3 = num2;
				num4 = _currentBodyYRotation;
			}
			else
			{
				num3 = Mathf.Sign(num2) * _headLimitationY;
				num4 = (_currentBodyYRotation = y - num3);
			}
			if (_playerMovableModel.RotationMode == PlayerRotationMode.HeadAndBodyRotation)
			{
				if (_playerRagdollEntity != null && _playerRagdollEntity.IsSimulated)
				{
					_playerRagdollEntity.RootPhysData.RigidBody.AddTorque(Vector3.up * ((num4 - _prevBodyYRotation) * _ragdollRotationForce));
				}
				else
				{
					_allObject.transform.rotation = Quaternion.Euler(0f, num4, 0f);
				}
			}
			_lookPoint.transform.rotation = cameraObject.transform.rotation;
			_prevBodyYRotation = num4;
			float forward = Mathf.InverseLerp(_headLimitationXBottom, _headLimitationXTop, num);
			float side = Mathf.InverseLerp(0f - _headLimitationY, _headLimitationY, num3);
			if (_animator.GetBool(_isCrouchName))
			{
				forward = 0f;
			}
			if (_playerMovableModel.RotationMode != PlayerRotationMode.NoRotation)
			{
				ApplyAndSync(forward, side);
			}
			if (_playerMovableModel.LocalMovable.IsMoving())
			{
				ResetRotation();
			}
		}

		private void LookAtCamera()
		{
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject == null)
			{
				return;
			}
			Quaternion rotation = Quaternion.LookRotation(cameraObject.transform.position - _allObject.transform.position);
			Vector3 eulerAngles = rotation.eulerAngles;
			float num = eulerAngles.x;
			if (num > 180f)
			{
				num -= 360f;
			}
			num = Mathf.Clamp(num, _headLimitationXBottom, _headLimitationXTop);
			float y = eulerAngles.y;
			float num2 = Mathf.DeltaAngle(_currentBodyYRotation, y);
			float num3;
			float num4;
			if (Mathf.Abs(num2) <= _headLimitationY)
			{
				num3 = num2;
				num4 = _currentBodyYRotation;
			}
			else
			{
				num3 = Mathf.Sign(num2) * _headLimitationY;
				num4 = (_currentBodyYRotation = y - num3);
			}
			if (_playerMovableModel.RotationMode == PlayerRotationMode.HeadAndBodyRotation)
			{
				if (_playerRagdollEntity != null && _playerRagdollEntity.IsSimulated)
				{
					_playerRagdollEntity.RootPhysData.RigidBody.AddTorque(Vector3.up * ((num4 - _prevBodyYRotation) * _ragdollRotationForce), ForceMode.Force);
				}
				else
				{
					_allObject.transform.rotation = Quaternion.Euler(0f, num4, 0f);
				}
			}
			_lookPoint.transform.rotation = rotation;
			_prevBodyYRotation = num4;
			float num5 = Mathf.InverseLerp(_headLimitationXBottom, _headLimitationXTop, num);
			float forward = (_animator.GetBool(_isCrouchName) ? 0f : num5);
			if (_playerMovableModel.RotationMode == PlayerRotationMode.HeadAndBodyRotation)
			{
				float side = Mathf.InverseLerp(0f - _headLimitationY, _headLimitationY, num3);
				ApplyAndSync(forward, side);
			}
			else if (_playerMovableModel.RotationMode == PlayerRotationMode.OnlyHeadRotation)
			{
				float num6 = y - base.transform.eulerAngles.y;
				if (num6 > 360f)
				{
					num6 -= 360f;
				}
				if (num6 < -180f)
				{
					num6 += 360f;
				}
				if (num6 > 180f)
				{
					num6 -= 360f;
				}
				float value = Mathf.Clamp(num6, 0f - _headLimitationY, _headLimitationY);
				float side2 = Mathf.InverseLerp(0f - _headLimitationY, _headLimitationY, value);
				ApplyAndSync(forward, side2);
			}
			if (_playerMovableModel.LocalMovable.IsMoving())
			{
				ResetRotation();
			}
		}

		public override void Render()
		{
			if (_isResetRotation)
			{
				_animator.SetFloat(_forwardParameterName, 0.5f);
				_animator.SetFloat(_sideParameterName, 0.5f);
				_allObject.transform.localEulerAngles = Vector3.zero;
			}
		}

		private void ResetRotation()
		{
			float side = Mathf.Lerp(Dequantize01(_sideQuantized), _defaultSideValue, Time.deltaTime * _resetAnimatorLerpSpeed);
			ApplyAndSync(Dequantize01(_forwardQuantized), side);
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject != null)
			{
				float y = cameraObject.transform.eulerAngles.y;
				_currentBodyYRotation = Mathf.LerpAngle(_currentBodyYRotation, y, Time.deltaTime * _resetRotationLerpSpeed);
				_allObject.transform.rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
				_lookPoint.transform.rotation = cameraObject.transform.rotation;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			_forwardQuantized = __forwardQuantized;
			_sideQuantized = __sideQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			__forwardQuantized = _forwardQuantized;
			__sideQuantized = _sideQuantized;
		}
	}
}
