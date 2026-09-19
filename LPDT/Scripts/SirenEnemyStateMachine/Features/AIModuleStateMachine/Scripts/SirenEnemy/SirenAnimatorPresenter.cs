using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy
{
	public class SirenAnimatorPresenter : MonoBehaviour
	{
		[SerializeField]
		private SirenEnemy _enemy;

		[SerializeField]
		private SirenEnemyContext _context;

		[SerializeField]
		private SirenEnemySettings _enemySettings;

		[SerializeField]
		private string _headRotationLayerName = "HeadRotationLayer";

		private readonly int _isStunHash = Animator.StringToHash("IsStun");

		private readonly int _isScreamHash = Animator.StringToHash("IsScream");

		private readonly int _isHypnoHash = Animator.StringToHash("IsHypno");

		private readonly int _rotationHash = Animator.StringToHash("Rotation");

		private readonly int _headRotationHash = Animator.StringToHash("HeadRotation");

		private SpawnedPlayersModel _spawnedPlayers;

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayers)
		{
			_spawnedPlayers = spawnedPlayers;
		}

		private void Update()
		{
			if (_enemy.Object.IsValid)
			{
				_context.UpdateFmod3DAttributes();
				ApplyAnimatorState(_enemy.VisualState);
			}
		}

		private void LateUpdate()
		{
			if (_enemy.Object.IsValid && ShouldApplyLookRotation())
			{
				ApplyLookAndRotation();
			}
		}

		private bool ShouldApplyLookRotation()
		{
			if (_enemy.CurrentTargetPlayerId < 0)
			{
				return false;
			}
			if (_enemy.IsLookAtPresentationActive)
			{
				return true;
			}
			SirenVisualState visualState = _enemy.VisualState;
			if (visualState != SirenVisualState.Chasing)
			{
				return visualState == SirenVisualState.Attacking;
			}
			return true;
		}

		private void ApplyAnimatorState(SirenVisualState state)
		{
			Animator animator = _context.Animator;
			int layerIndex = animator.GetLayerIndex(_headRotationLayerName);
			switch (state)
			{
			case SirenVisualState.Idle:
				animator.SetBool(_isStunHash, value: false);
				animator.SetBool(_isScreamHash, value: false);
				animator.SetBool(_isHypnoHash, value: false);
				if (layerIndex >= 0)
				{
					animator.SetLayerWeight(layerIndex, 0f);
				}
				break;
			case SirenVisualState.Chasing:
				animator.SetBool(_isStunHash, value: false);
				animator.SetBool(_isScreamHash, value: false);
				animator.SetBool(_isHypnoHash, value: true);
				if (layerIndex >= 0)
				{
					animator.SetLayerWeight(layerIndex, 1f);
				}
				break;
			case SirenVisualState.Attacking:
				animator.SetBool(_isStunHash, value: false);
				animator.SetBool(_isScreamHash, value: true);
				animator.SetBool(_isHypnoHash, value: false);
				if (layerIndex >= 0)
				{
					animator.SetLayerWeight(layerIndex, 1f);
				}
				break;
			case SirenVisualState.Stun:
				animator.SetBool(_isStunHash, value: true);
				animator.SetBool(_isScreamHash, value: false);
				animator.SetBool(_isHypnoHash, value: false);
				if (layerIndex >= 0)
				{
					animator.SetLayerWeight(layerIndex, 0f);
				}
				break;
			case SirenVisualState.Fear:
				animator.SetBool(_isStunHash, value: false);
				animator.SetBool(_isScreamHash, value: false);
				animator.SetBool(_isHypnoHash, value: false);
				if (layerIndex >= 0)
				{
					animator.SetLayerWeight(layerIndex, 0f);
				}
				break;
			}
		}

		private void ApplyLookAndRotation()
		{
			int currentTargetPlayerId = _enemy.CurrentTargetPlayerId;
			if (currentTargetPlayerId >= 0 && TryGetTargetTransform(currentTargetPlayerId, out var targetTransform))
			{
				Vector3 vector = ((_enemySettings != null) ? _enemySettings.LookOffset : Vector3.zero);
				Vector3 vector2 = targetTransform.position + vector - _context.LookAtTarget.position;
				float num = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
				num -= base.transform.rotation.eulerAngles.y;
				if (num < 0f)
				{
					num += 360f;
				}
				_context.Animator.SetFloat(_rotationHash, num / 360f);
				float num2 = Mathf.Asin(vector2.normalized.y) * 57.29578f;
				_context.Animator.SetFloat(_headRotationHash, (num2 + 180f) / 180f);
			}
		}

		private bool TryGetTargetTransform(int playerId, out Transform targetTransform)
		{
			targetTransform = null;
			if (_spawnedPlayers == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayers.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					targetTransform = player.Value.NetworkObject.transform;
					return true;
				}
			}
			return false;
		}
	}
}
