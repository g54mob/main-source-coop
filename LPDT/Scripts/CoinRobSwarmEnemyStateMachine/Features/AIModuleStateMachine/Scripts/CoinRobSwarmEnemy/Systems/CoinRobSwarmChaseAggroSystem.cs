using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CoinRobSwarmChaseAggroSystem : MonoSystem
	{
		private CoinRobSwarmEnemyContext _swarmContext;

		private CoinRobCombatSettings _combatSettings;

		private CoinRobSwarmReactiveAggroService _reactiveAggroService;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private CauldronStealthModel _cauldronStealthModel;

		private readonly Dictionary<int, float> _chaseAggroTimers = new Dictionary<int, float>();

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(CoinRobSwarmReactiveAggroService reactiveAggroService, SpawnedPlayersModel spawnedPlayersModel, CauldronStealthModel cauldronStealthModel)
		{
			_reactiveAggroService = reactiveAggroService;
			_spawnedPlayersModel = spawnedPlayersModel;
			_cauldronStealthModel = cauldronStealthModel;
		}

		public void Bind(CoinRobSwarmEnemyContext swarmContext, CoinRobCombatSettings combatSettings)
		{
			swarmContext.RegisterChaseAggroSystem(this, combatSettings);
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

		public override void Clear()
		{
			_chaseAggroTimers.Clear();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_swarmContext?.UnregisterChaseAggroSystem(this);
			_swarmContext = null;
			_combatSettings = null;
			_enabled = false;
			Clear();
			base.Despawned(runner, hasState);
		}

		internal void AssignSwarmContext(CoinRobSwarmEnemyContext swarmContext, CoinRobCombatSettings combatSettings)
		{
			_swarmContext = swarmContext;
			_combatSettings = combatSettings;
		}

		private void Update()
		{
			if (base.Initialized && IsMasterAggroPeer() && _enabled && !_swarmContext.IsReadyForPlayerAttack && !(_swarmContext.ChasePlayer != PlayerRef.None) && IsChaseProximityAggroAllowed(_swarmContext.CurrentFsmStateId) && _reactiveAggroService.TryResolveChaseAggro(_combatSettings, _swarmContext, _chaseAggroTimers, out var playerId) && !_cauldronStealthModel.IsStealthed(playerId))
			{
				TryPreparePlayerAttack(playerId);
			}
		}

		private void TryPreparePlayerAttack(int playerId)
		{
			if (!IsMasterAggroPeer() || !_enabled || playerId <= 0 || _swarmContext.IsReadyForPlayerAttack)
			{
				return;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					if (!(player.Value.NetworkObject == null))
					{
						_swarmContext.QueueReactivePlayerAttack(playerId);
					}
					break;
				}
			}
		}

		private bool IsMasterAggroPeer()
		{
			if (base.Runner != null)
			{
				return base.Runner.IsSharedModeMasterClient;
			}
			return false;
		}

		private bool IsChaseProximityAggroAllowed(CoinRobSwarmStateId stateId)
		{
			if (stateId != CoinRobSwarmStateId.Wandering && stateId != CoinRobSwarmStateId.Chasing && stateId != CoinRobSwarmStateId.Attacking && stateId != CoinRobSwarmStateId.Stealing)
			{
				return stateId == CoinRobSwarmStateId.RunAway;
			}
			return true;
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
