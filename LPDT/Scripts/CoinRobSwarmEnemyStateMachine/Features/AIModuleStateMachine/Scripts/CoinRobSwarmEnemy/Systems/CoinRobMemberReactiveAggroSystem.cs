using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CoinRobMemberReactiveAggroSystem : MonoSystem
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private CoinRobBehaviour _unit;

		private CoinRobSwarmEnemyContext _swarmContext;

		private MultiplayerModel _multiplayerModel;

		private bool _enabled;

		private bool _wasLocalPlayerGrabbing;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public void Bind(CoinRobSwarmEnemyContext swarmContext)
		{
			swarmContext.RegisterMemberAggroSystem(this);
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
		}

		public override void Clear()
		{
		}

		public override void Spawned()
		{
			base.Spawned();
			_grabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			_unit.OnMemberDamaged += OnMemberDamaged;
			_unit.OnDeactivated += OnUnitDeactivated;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_unit.OnMemberDamaged -= OnMemberDamaged;
			_unit.OnDeactivated -= OnUnitDeactivated;
			_grabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			_swarmContext?.UnregisterMemberAggroSystem(this);
			_swarmContext = null;
			_enabled = false;
			_wasLocalPlayerGrabbing = false;
			base.Despawned(runner, hasState);
		}

		internal void AssignSwarmContext(CoinRobSwarmEnemyContext swarmContext)
		{
			_swarmContext = swarmContext;
		}

		private void OnMemberDamaged(DamageData damageData)
		{
			int num = damageData.DamageDealerPlayerID;
			if (num <= 0)
			{
				num = _unit.LastDamageDealerPlayerId;
			}
			RememberDamageDealer(num);
			RequestMemberAggro(num);
		}

		private void OnUnitDeactivated(CoinRobBehaviour unit)
		{
			int lastDamageDealerPlayerId = unit.LastDamageDealerPlayerId;
			RememberDamageDealer(lastDamageDealerPlayerId);
			RequestMemberAggro(lastDamageDealerPlayerId);
		}

		private void RememberDamageDealer(int playerId)
		{
			if (playerId > 0 && _swarmContext != null)
			{
				_swarmContext.LastMemberDamageDealerPlayerId = playerId;
			}
		}

		private void OnGrabbedPlayersChanged()
		{
			if (base.Initialized && !(base.Runner == null))
			{
				bool flag = _grabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId);
				if (flag && !_wasLocalPlayerGrabbing)
				{
					HandleLocalPlayerGrabbed();
				}
				_wasLocalPlayerGrabbing = flag;
			}
		}

		private void HandleLocalPlayerGrabbed()
		{
			RequestMemberAggro(base.Runner.LocalPlayer.PlayerId);
		}

		private void RequestMemberAggro(int playerId)
		{
			if (playerId > 0)
			{
				if (IsMasterAggroPeer())
				{
					TryQueuePlayerAttack(playerId);
				}
				else
				{
					ForwardAggroToSwarmHost(playerId);
				}
			}
		}

		private void ForwardAggroToSwarmHost(int playerId)
		{
			if (!(base.Runner == null) && !(_unit.SwarmHostNetworkId == default(NetworkId)) && base.Runner.TryFindObject(_unit.SwarmHostNetworkId, out var networkObject) && networkObject.TryGetComponent<CoinRobSwarmEnemy>(out var component))
			{
				component.RequestReactivePlayerAttackFromMember(playerId);
			}
		}

		private void TryQueuePlayerAttack(int playerId)
		{
			if (playerId > 0 && _enabled && !(_swarmContext == null))
			{
				_swarmContext.QueueReactivePlayerAttack(playerId);
			}
		}

		private bool IsMasterAggroPeer()
		{
			if (base.Runner != null)
			{
				return base.Runner.IsSharedModeMasterClient;
			}
			if (_multiplayerModel?.NetworkRunner != null)
			{
				return _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
			}
			return false;
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
