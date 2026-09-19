using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.LevelTransition
{
	[NetworkBehaviourWeaved(0)]
	public class LevelTransitionArea : NetworkBehaviour
	{
		private static readonly List<LevelTransitionArea> ActiveAreas = new List<LevelTransitionArea>();

		[SerializeField]
		private LayerMask _triggerLayerMask;

		private readonly Collider[] _overlapBuffer = new Collider[64];

		private BeachOccupancyModel _beachOccupancyModel;

		private NetworkRunnerEventBus _eventBus;

		[Inject]
		public void InjectDependencies(BeachOccupancyModel beachOccupancyModel, NetworkRunnerEventBus eventBus)
		{
			_beachOccupancyModel = beachOccupancyModel;
			_eventBus = eventBus;
		}

		public override void Spawned()
		{
			base.Spawned();
			ActiveAreas.Add(this);
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			RescanPlayersInArea();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			ActiveAreas.Remove(this);
			base.Despawned(runner, hasState);
		}

		public static void RescanAllAreas()
		{
			foreach (LevelTransitionArea activeArea in ActiveAreas)
			{
				activeArea.RescanPlayersInArea();
			}
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent _)
		{
			if (base.HasStateAuthority && base.Runner.IsSharedModeMasterClient)
			{
				RescanPlayersInAreaDelayed().Forget();
			}
		}

		private async UniTaskVoid RescanPlayersInAreaDelayed()
		{
			for (int i = 0; i < 5; i++)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (!base.HasStateAuthority || base.Runner == null || !base.Runner.IsRunning)
				{
					return;
				}
			}
			RescanPlayersInArea();
		}

		private void RescanPlayersInArea()
		{
			if (!base.HasStateAuthority || !base.Runner.IsSharedModeMasterClient)
			{
				return;
			}
			Vector3 halfExtents = base.transform.localScale / 2f;
			int num = Physics.OverlapBoxNonAlloc(base.transform.position, halfExtents, _overlapBuffer, base.transform.rotation, _triggerLayerMask);
			for (int i = 0; i < num; i++)
			{
				if (_overlapBuffer[i].transform.TryGetComponent<LevelTransitTrigger>(out var component) && !_beachOccupancyModel.Players.ContainsKey(component.OriginalPlayerRef))
				{
					_beachOccupancyModel.AddPlayer(component.OriginalPlayerRef, component);
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.HasStateAuthority && base.Runner.IsSharedModeMasterClient && other.transform.TryGetComponent<LevelTransitTrigger>(out var component) && !_beachOccupancyModel.Players.ContainsKey(component.OriginalPlayerRef))
			{
				_beachOccupancyModel.AddPlayer(component.OriginalPlayerRef, component);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (base.HasStateAuthority && base.Runner.IsSharedModeMasterClient && other.transform.TryGetComponent<LevelTransitTrigger>(out var component))
			{
				_beachOccupancyModel.Players.Remove(component.OriginalPlayerRef);
			}
		}

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
