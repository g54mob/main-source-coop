using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class ReadyForAttackTrackSystem : MonoSystem
	{
		private const float DEFAULT_GRAB_AGGRO_PIN_DURATION = 6f;

		[SerializeField]
		private List<SimplePointGrabable> _grabables;

		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		[SerializeField]
		private float _grabAggroPinDuration = 6f;

		private MimicEnemyContext _context;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private readonly Dictionary<PlayerDataHolder, float> _detectedPlayerFirstDetectedTime = new Dictionary<PlayerDataHolder, float>();

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_simpleEnemyDamageable.OnDamaged += OnMimicDamaged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_simpleEnemyDamageable.OnDamaged -= OnMimicDamaged;
			base.Despawned(runner, hasState);
		}

		private void OnMimicDamaged(DamageData damageData)
		{
			if (_enabled)
			{
				TryPrepareAttackOnPlayer(damageData.DamageDealerPlayerID, pinPriority: false);
			}
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		private void ProcessGrabbed()
		{
			if (_grabables == null)
			{
				return;
			}
			foreach (SimplePointGrabable grabable in _grabables)
			{
				if (!(grabable == null) && grabable.GrabbedByPlayers.Count != 0)
				{
					TryPrepareAttackOnPlayer(grabable.GrabbedByPlayers[0], pinPriority: true);
				}
			}
		}

		private void TryPrepareAttackOnPlayer(int playerId, bool pinPriority)
		{
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					_context.AttackCooldown = 0f;
					_context.IsReadyForAttack = true;
					if (pinPriority)
					{
						_context.PinPriorityPlayer(player.Value, ResolvePinDuration());
					}
					else
					{
						_context.SetPriorityPlayer(player.Value);
					}
					_context.SetTargetPositionCompleted(isCompleted: true);
					break;
				}
			}
		}

		private float ResolvePinDuration()
		{
			if (!(_grabAggroPinDuration > 0f))
			{
				return 6f;
			}
			return _grabAggroPinDuration;
		}

		private void Update()
		{
			if (!base.Initialized || !_enabled)
			{
				return;
			}
			ProcessGrabbed();
			if (_context.IsAttackOnCooldown || _context.IsReadyForAttack)
			{
				return;
			}
			UpdateDetectedPlayerFirstDetectedTimeAtAttackDistance();
			foreach (PlayerDataHolder detectedPlayer in _context.DetectedPlayers)
			{
				if (_detectedPlayerFirstDetectedTime.TryGetValue(detectedPlayer, out var value) && _context.DetectedPlayersDistance.TryGetValue(detectedPlayer, out var value2))
				{
					float num = Time.time - value;
					if (!(value2 > _context.DistanceToAttack) && num >= _context.TimeToAttack)
					{
						_context.IsReadyForAttack = true;
						_context.SetPriorityPlayer(detectedPlayer);
						break;
					}
				}
			}
		}

		private void UpdateDetectedPlayerFirstDetectedTimeAtAttackDistance()
		{
			foreach (PlayerDataHolder detectedPlayer in _context.DetectedPlayers)
			{
				if (!_context.DetectedPlayersDistance.TryGetValue(detectedPlayer, out var value))
				{
					continue;
				}
				if (value <= _context.DistanceToAttack)
				{
					if (!_detectedPlayerFirstDetectedTime.ContainsKey(detectedPlayer))
					{
						_detectedPlayerFirstDetectedTime[detectedPlayer] = Time.time;
					}
				}
				else
				{
					_detectedPlayerFirstDetectedTime.Remove(detectedPlayer);
				}
			}
			foreach (PlayerDataHolder item in _detectedPlayerFirstDetectedTime.Keys.ToList())
			{
				if (!_context.DetectedPlayers.Contains(item))
				{
					_detectedPlayerFirstDetectedTime.Remove(item);
				}
			}
		}

		public override void Clear()
		{
			_detectedPlayerFirstDetectedTime.Clear();
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
