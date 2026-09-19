using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings;
using Features.AnimationModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy
{
	[NetworkBehaviourWeaved(2)]
	public class BartenderEyeRotator : NetworkBehaviour
	{
		private static readonly int EyesHorizontalHash = Animator.StringToHash("EyesHorizontal");

		private static readonly int EyesVerticalHash = Animator.StringToHash("EyesVertical");

		private const float QUANTIZE_STEP = 0.01f;

		[SerializeField]
		private MimicFacialAnimationSettings _settings;

		[SerializeField]
		private CompositeAnimator _animator;

		[SerializeField]
		private Transform _eyesPositionTransform;

		[SerializeField]
		private Transform _headForwardTransform;

		private BartenderEnemyContext _context;

		private BartenderEnemy _enemy;

		private SpawnedPlayersModel _spawnedPlayersModel;

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
					throw new InvalidOperationException("Error when accessing BartenderEyeRotator.EyesHorizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEyeRotator.EyesHorizontalQuantized. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing BartenderEyeRotator.EyesVerticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BartenderEyeRotator.EyesVerticalQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Inject]
		private void InjectDependencies(BartenderEnemyContext context, BartenderEnemy enemy, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			_enemy = enemy;
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			base.Despawned(runner, hasState);
		}

		private void LateUpdate()
		{
			if (!_isSpawned || !_settings.EnableEyeAnimation)
			{
				return;
			}
			if (base.HasStateAuthority)
			{
				Transform transform = null;
				if (_context != null && _enemy != null && _enemy.HasServeStock && _enemy.CounterLookTarget != null)
				{
					transform = _enemy.CounterLookTarget;
				}
				if (transform == null)
				{
					transform = FindBestPlayerLookTarget();
				}
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
			Transform transform = ResolveContextPlayerTarget();
			if (transform != null)
			{
				return transform;
			}
			if (_spawnedPlayersModel.Players.Count == 0)
			{
				return null;
			}
			Vector3 eyesPosition = GetEyesPosition();
			Vector3 headForward = GetHeadForward();
			Vector3 headRight = GetHeadRight();
			Vector3 headUp = GetHeadUp();
			Transform result = null;
			float num = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (value?.NetworkObject == null)
				{
					continue;
				}
				Transform transform2 = ResolvePlayerLookTransform(value.NetworkObject);
				if (!(transform2 == null))
				{
					float num2 = Vector3.Distance(eyesPosition, transform2.position);
					if (!(num2 > _settings.MaxTargetDistance) && IsWithinEyeLimits(eyesPosition, headForward, headRight, headUp, transform2.position) && !(num2 >= num))
					{
						num = num2;
						result = transform2;
					}
				}
			}
			return result;
		}

		private Transform ResolveContextPlayerTarget()
		{
			if (_context.PriorityPlayer?.NetworkObject != null)
			{
				Transform transform = ResolvePlayerLookTransform(_context.PriorityPlayer.NetworkObject);
				if (transform != null)
				{
					return transform;
				}
			}
			foreach (PlayerDataHolder detectedPlayer in _context.DetectedPlayers)
			{
				if (!(detectedPlayer?.NetworkObject == null))
				{
					Transform transform2 = ResolvePlayerLookTransform(detectedPlayer.NetworkObject);
					if (transform2 != null)
					{
						return transform2;
					}
				}
			}
			foreach (PlayerDataHolder visiblePlayer in _context.VisiblePlayers)
			{
				if (!(visiblePlayer?.NetworkObject == null))
				{
					Transform transform3 = ResolvePlayerLookTransform(visiblePlayer.NetworkObject);
					if (transform3 != null)
					{
						return transform3;
					}
				}
			}
			return null;
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
			Vector3 eyesPosition = GetEyesPosition();
			Vector3 headForward = GetHeadForward();
			Vector3 headRight = GetHeadRight();
			Vector3 headUp = GetHeadUp();
			Vector3 normalized = (targetPosition - eyesPosition).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, headUp).normalized;
			float value = Vector3.SignedAngle(headForward, normalized2, headUp);
			value = Mathf.Clamp(value, 0f - _settings.EyesHorizontalLimit, _settings.EyesHorizontalLimit);
			Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, headRight).normalized;
			float value2 = Vector3.SignedAngle(headForward, normalized3, headRight);
			value2 = Mathf.Clamp(value2, _settings.EyesVerticalLimitBottom, _settings.EyesVerticalLimitTop);
			float value3 = Mathf.InverseLerp(0f - _settings.EyesHorizontalLimit, _settings.EyesHorizontalLimit, value) * 2f - 1f;
			float value4 = Mathf.InverseLerp(_settings.EyesVerticalLimitBottom, _settings.EyesVerticalLimitTop, value2) * 2f - 1f;
			EyesHorizontalQuantized = Quantize(value3);
			EyesVerticalQuantized = Quantize(value4);
		}

		private void ReturnToDefault()
		{
			EyesHorizontalQuantized = 0;
			EyesVerticalQuantized = 0;
		}

		private void ApplyLerpedValues()
		{
			_lerpedHorizontalValue = Mathf.Lerp(_lerpedHorizontalValue, Dequantize(EyesHorizontalQuantized), Time.deltaTime * _settings.EyesLerpSpeed);
			_lerpedVerticalValue = Mathf.Lerp(_lerpedVerticalValue, Dequantize(EyesVerticalQuantized), Time.deltaTime * _settings.EyesLerpSpeed);
			_animator.SetFloat(EyesHorizontalHash, _lerpedHorizontalValue);
			_animator.SetFloat(EyesVerticalHash, _lerpedVerticalValue);
		}

		private bool IsWithinEyeLimits(Vector3 eyesPosition, Vector3 headForward, Vector3 headRight, Vector3 headUp, Vector3 targetPosition)
		{
			Vector3 normalized = (targetPosition - eyesPosition).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, headUp).normalized;
			float num = Vector3.SignedAngle(headForward, normalized2, headUp);
			if (num < 0f - _settings.EyesHorizontalLimit || num > _settings.EyesHorizontalLimit)
			{
				return false;
			}
			Vector3 normalized3 = Vector3.ProjectOnPlane(normalized, headRight).normalized;
			float num2 = Vector3.SignedAngle(headForward, normalized3, headRight);
			if (num2 >= _settings.EyesVerticalLimitBottom)
			{
				return num2 <= _settings.EyesVerticalLimitTop;
			}
			return false;
		}

		private Vector3 GetEyesPosition()
		{
			return _eyesPositionTransform.position;
		}

		private Vector3 GetHeadForward()
		{
			return _headForwardTransform.forward;
		}

		private Vector3 GetHeadRight()
		{
			return _headForwardTransform.right;
		}

		private Vector3 GetHeadUp()
		{
			return _headForwardTransform.up;
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
