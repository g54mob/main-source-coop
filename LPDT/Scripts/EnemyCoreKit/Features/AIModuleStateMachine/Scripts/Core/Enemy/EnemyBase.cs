using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Fusion;
using UnityEngine;
using UnityHFSM;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Enemy
{
	[NetworkBehaviourWeaved(0)]
	public abstract class EnemyBase<TStateId, TEvent> : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, ICurrentStateProvider<TStateId>, IStateAuthorityChanged, IPublicFacingInterface
	{
		protected IEnemyContext Context;

		protected IInstantiator Instantiator;

		private StateMachine<TStateId, TStateId, TEvent> _fsm;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		public bool IsOccupySpawnPoint { get; private set; }

		public TStateId CurrentStateId { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		public abstract EnemyType EnemyType { get; }

		protected StateMachine<TStateId, TStateId, TEvent> Fsm => _fsm;

		protected virtual bool UsesNavMeshAgent => true;

		public event Action<IEnemyBehaviour> OnDeath;

		protected void InjectBaseDependencies(IEnemyContext context, IInstantiator instantiator)
		{
			Context = context;
			Instantiator = instantiator;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (UsesNavMeshAgent)
			{
				Context.NavMeshAgent.enabled = base.HasStateAuthority;
				if (base.HasStateAuthority)
				{
					Context.NavMeshAgent.Warp(base.transform.position);
				}
			}
			Context.Initialize();
			EnsureFsmBuilt();
		}

		public virtual void StateAuthorityChanged()
		{
			if (base.HasStateAuthority)
			{
				if (UsesNavMeshAgent)
				{
					Context.NavMeshAgent.enabled = true;
					Context.NavMeshAgent.Warp(base.transform.position);
				}
				EnsureFsmBuilt();
			}
		}

		private void EnsureFsmBuilt()
		{
			if (base.HasStateAuthority && _fsm == null)
			{
				_fsm = BuildFsm();
				_fsm.Init();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			if (base.HasStateAuthority)
			{
				this.OnDeath?.Invoke(this);
			}
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _fsm != null)
			{
				_fsm.OnLogic();
			}
		}

		public void SetAreaPosition(Vector3 areaPosition)
		{
		}

		public void TriggerEvent(TEvent ev)
		{
			_fsm?.Trigger(ev);
		}

		public void SetCurrentStateId(TStateId stateId)
		{
			if (base.HasStateAuthority)
			{
				CurrentStateId = stateId;
			}
		}

		protected abstract StateMachine<TStateId, TStateId, TEvent> BuildFsm();

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
