using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class CollectItemWatchingEyeLook : NetworkBehaviour
	{
		private const float QuantizeStep = 0.01f;

		[SerializeField]
		private Transform _sphere;

		[SerializeField]
		private Transform _headForwardTransform;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private CollectItemWatchingEyeConfig _config;

		private Quaternion _restLocalRotation;

		private float _lerpedHorizontalValue;

		private float _lerpedVerticalValue;

		private bool _isSpawned;

		[WeaverGenerated]
		[DefaultForProperty("EyesHorizontalQuantized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _EyesHorizontalQuantized;

		[WeaverGenerated]
		[DefaultForProperty("EyesVerticalQuantized", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _EyesVerticalQuantized;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int EyesHorizontalQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemWatchingEyeLook.EyesHorizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemWatchingEyeLook.EyesHorizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int EyesVerticalQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemWatchingEyeLook.EyesVerticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemWatchingEyeLook.EyesVerticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		private void Awake()
		{
			_config = (CollectItemWatchingEyeConfig)_monoItem.DefaultConfig;
			_restLocalRotation = _sphere.localRotation;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			_sphere.localRotation = _restLocalRotation;
			base.Despawned(runner, hasState);
		}

		private void LateUpdate()
		{
			if (!_isSpawned)
			{
				return;
			}
			if (base.HasStateAuthority)
			{
				Transform transform = FindBestPlayerLookTarget();
				if (transform != null)
				{
					UpdateEyesRotation(transform.position);
				}
				else
				{
					ReturnToDefault();
				}
			}
			ApplyLerpedValues();
		}

		private Transform FindBestPlayerLookTarget()
		{
			if (_spawnedPlayersModel.Players.Count == 0)
			{
				return null;
			}
			Vector3 position = _sphere.position;
			Vector3 forward = _headForwardTransform.forward;
			Vector3 right = _headForwardTransform.right;
			Vector3 up = _headForwardTransform.up;
			Transform result = null;
			float num = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (value?.NetworkObject == null || _simplePointGrabable.GrabbedByPlayers.Contains(player.Key.PlayerId))
				{
					continue;
				}
				Transform transform = ResolvePlayerLookTransform(value.NetworkObject);
				if (!(transform == null))
				{
					float num2 = Vector3.Distance(position, transform.position);
					if (!(num2 > _config.MaxTargetDistance) && IsWithinEyeLimits(position, forward, right, up, transform.position) && !(num2 >= num))
					{
						num = num2;
						result = transform;
					}
				}
			}
			return result;
		}

		private Transform ResolvePlayerLookTransform(NetworkObject playerObject)
		{
			CharacterMovableBase componentInChildren = playerObject.GetComponentInChildren<CharacterMovableBase>();
			if (componentInChildren != null && componentInChildren.CameraPositionTransform != null)
			{
				return componentInChildren.CameraPositionTransform;
			}
			return playerObject.transform;
		}

		private void UpdateEyesRotation(Vector3 targetPosition)
		{
			Vector3 position = _sphere.position;
			Vector3 forward = _headForwardTransform.forward;
			Vector3 right = _headForwardTransform.right;
			Vector3 up = _headForwardTransform.up;
			Vector3 normalized = (targetPosition - position).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, up).normalized;
			float value = Vector3.SignedAngle(forward, normalized2, up);
			value = Mathf.Clamp(value, 0f - _config.EyesHorizontalLimit, _config.EyesHorizontalLimit);
			Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, right).normalized;
			float value2 = Vector3.SignedAngle(forward, normalized3, right);
			value2 = Mathf.Clamp(value2, _config.EyesVerticalLimitBottom, _config.EyesVerticalLimitTop);
			float value3 = Mathf.InverseLerp(0f - _config.EyesHorizontalLimit, _config.EyesHorizontalLimit, value) * 2f - 1f;
			float value4 = Mathf.InverseLerp(_config.EyesVerticalLimitBottom, _config.EyesVerticalLimitTop, value2) * 2f - 1f;
			EyesHorizontalQuantized = Quantize(value3);
			EyesVerticalQuantized = Quantize(value4);
		}

		private void ReturnToDefault()
		{
			EyesHorizontalQuantized = 0;
			EyesVerticalQuantized = Quantize(Mathf.InverseLerp(_config.EyesVerticalLimitBottom, _config.EyesVerticalLimitTop, 0f) * 2f - 1f);
		}

		private void ApplyLerpedValues()
		{
			_lerpedHorizontalValue = Mathf.Lerp(_lerpedHorizontalValue, Dequantize(EyesHorizontalQuantized), Time.deltaTime * _config.EyesLerpSpeed);
			_lerpedVerticalValue = Mathf.Lerp(_lerpedVerticalValue, Dequantize(EyesVerticalQuantized), Time.deltaTime * _config.EyesLerpSpeed);
			float angle = Mathf.Lerp(0f - _config.EyesHorizontalLimit, _config.EyesHorizontalLimit, (_lerpedHorizontalValue + 1f) * 0.5f);
			float angle2 = Mathf.Lerp(_config.EyesVerticalLimitBottom, _config.EyesVerticalLimitTop, (_lerpedVerticalValue + 1f) * 0.5f);
			_sphere.localRotation = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.AngleAxis(angle2, Vector3.right) * _restLocalRotation;
		}

		private bool IsWithinEyeLimits(Vector3 eyesPosition, Vector3 headForward, Vector3 headRight, Vector3 headUp, Vector3 targetPosition)
		{
			Vector3 normalized = (targetPosition - eyesPosition).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, headUp).normalized;
			float num = Vector3.SignedAngle(headForward, normalized2, headUp);
			if (num < 0f - _config.EyesHorizontalLimit || num > _config.EyesHorizontalLimit)
			{
				return false;
			}
			Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, headRight).normalized;
			float num2 = Vector3.SignedAngle(headForward, normalized3, headRight);
			if (num2 >= _config.EyesVerticalLimitBottom)
			{
				return num2 <= _config.EyesVerticalLimitTop;
			}
			return false;
		}

		private int Quantize(float value)
		{
			return Mathf.RoundToInt(value / 0.01f);
		}

		private float Dequantize(int value)
		{
			return (float)value * 0.01f;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			EyesHorizontalQuantized = _EyesHorizontalQuantized;
			EyesVerticalQuantized = _EyesVerticalQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_EyesHorizontalQuantized = EyesHorizontalQuantized;
			_EyesVerticalQuantized = EyesVerticalQuantized;
		}
	}
}
