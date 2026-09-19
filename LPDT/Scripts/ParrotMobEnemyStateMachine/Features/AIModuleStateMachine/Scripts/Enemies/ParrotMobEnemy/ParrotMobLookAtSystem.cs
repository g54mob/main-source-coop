using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobLookAtSystem : MonoSystem
	{
		private const float PathSampleStepDegrees = 5f;

		[SerializeField]
		private ParrotMobEnemy _enemy;

		[SerializeField]
		private ParrotMobEnemyContext _context;

		[SerializeField]
		private Transform _bodyTransform;

		[SerializeField]
		private Transform _yawReference;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private ParrotMobEnemySettings _enemySettings;

		private ParrotMobAlertSettings _alertSettings;

		private ParrotMobScreamSettings _screamSettings;

		public override bool IsEnabled => true;

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, ParrotMobEnemySettings enemySettings, ParrotMobAlertSettings alertSettings, ParrotMobScreamSettings screamSettings)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_enemySettings = enemySettings;
			_alertSettings = alertSettings;
			_screamSettings = screamSettings;
		}

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
		}

		private void LateUpdate()
		{
			if (!base.Initialized || _enemy.Object == null || !_enemy.Object.IsValid || !base.HasStateAuthority || !_context.IsLookAtPresentationActive || _context.CurrentTargetPlayerId < 0 || !TryGetTargetPosition(_context.CurrentTargetPlayerId, out var position))
			{
				return;
			}
			Vector3 vector = position - _bodyTransform.position;
			vector.y = 0f;
			if (!(vector.sqrMagnitude < 0.0001f))
			{
				Vector3 vector2 = Quaternion.Inverse(_yawReference.rotation) * vector;
				vector2.y = 0f;
				if (!(vector2.sqrMagnitude < 0.0001f))
				{
					float num = ClampToAllowedYaw(NormalizeAngle360(Mathf.Atan2(vector2.x, vector2.z) * 57.29578f), _enemySettings.AllowedLookYawRanges);
					float num2 = NormalizeAngle360((Quaternion.Inverse(_yawReference.rotation) * _bodyTransform.rotation).eulerAngles.y);
					float num3 = ((_enemy.VisualState == ParrotMobVisualState.Scream) ? _screamSettings.LookRotationSpeed : _alertSettings.LookRotationSpeed);
					float y = (IsShortestPathFullyAllowed(num2, num, _enemySettings.AllowedLookYawRanges) ? Mathf.LerpAngle(num2, num, Time.deltaTime * num3) : num);
					_bodyTransform.rotation = _yawReference.rotation * Quaternion.Euler(0f, y, 0f);
				}
			}
		}

		private bool TryGetTargetPosition(int playerId, out Vector3 position)
		{
			position = default(Vector3);
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					if (player.Value?.NetworkObject == null)
					{
						return false;
					}
					position = player.Value.NetworkObject.transform.position;
					return true;
				}
			}
			return false;
		}

		private static float ClampToAllowedYaw(float yaw, ParrotMobLookYawRange[] ranges)
		{
			yaw = NormalizeAngle360(yaw);
			if (ranges == null || ranges.Length == 0)
			{
				return yaw;
			}
			if (IsYawAllowed(yaw, ranges))
			{
				return yaw;
			}
			float result = yaw;
			float num = float.MaxValue;
			for (int i = 0; i < ranges.Length; i++)
			{
				ParrotMobLookYawRange parrotMobLookYawRange = ranges[i];
				float num2 = Mathf.Abs(Mathf.DeltaAngle(yaw, parrotMobLookYawRange.Min));
				if (num2 < num)
				{
					num = num2;
					result = NormalizeAngle360(parrotMobLookYawRange.Min);
				}
				float num3 = Mathf.Abs(Mathf.DeltaAngle(yaw, parrotMobLookYawRange.Max));
				if (num3 < num)
				{
					num = num3;
					result = NormalizeAngle360(parrotMobLookYawRange.Max);
				}
			}
			return result;
		}

		private static bool IsShortestPathFullyAllowed(float fromYaw, float toYaw, ParrotMobLookYawRange[] ranges)
		{
			if (ranges == null || ranges.Length == 0)
			{
				return true;
			}
			float num = Mathf.DeltaAngle(fromYaw, toYaw);
			float num2 = Mathf.Abs(num);
			if (num2 <= 0.01f)
			{
				return IsYawAllowed(fromYaw, ranges);
			}
			int num3 = Mathf.Max(1, Mathf.CeilToInt(num2 / 5f));
			for (int i = 0; i <= num3; i++)
			{
				float num4 = (float)i / (float)num3;
				if (!IsYawAllowed(NormalizeAngle360(fromYaw + num * num4), ranges))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsYawAllowed(float yaw, ParrotMobLookYawRange[] ranges)
		{
			yaw = NormalizeAngle360(yaw);
			for (int i = 0; i < ranges.Length; i++)
			{
				ParrotMobLookYawRange parrotMobLookYawRange = ranges[i];
				float num = NormalizeAngle360(parrotMobLookYawRange.Min);
				float num2 = NormalizeAngle360(parrotMobLookYawRange.Max);
				if (num <= num2)
				{
					if (yaw >= num && yaw <= num2)
					{
						return true;
					}
				}
				else if (yaw >= num || yaw <= num2)
				{
					return true;
				}
			}
			return false;
		}

		private static float NormalizeAngle360(float angle)
		{
			angle %= 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
