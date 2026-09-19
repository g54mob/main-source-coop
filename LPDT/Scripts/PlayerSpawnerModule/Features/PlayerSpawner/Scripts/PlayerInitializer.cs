using System;
using Cysharp.Threading.Tasks;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayerSpawner.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class PlayerInitializer : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OriginalOwner", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerRef _OriginalOwner;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("InitialHealth", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _InitialHealth;

		private IPlayerStatsInitializeService _playerStatsInitializeService;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe PlayerRef OriginalOwner
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerInitializer.OriginalOwner. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerRef*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerInitializer.OriginalOwner. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerRef*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe float InitialHealth
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerInitializer.InitialHealth. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerInitializer.InitialHealth. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		[Inject]
		public void InjectDependencies(IPlayerStatsInitializeService playerStatsInitializeService, SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_playerStatsInitializeService = playerStatsInitializeService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.Object.HasStateAuthority && OriginalOwner == PlayerRef.None)
			{
				OriginalOwner = base.Object.InputAuthority;
			}
			int playerId = base.Object.StateAuthority.PlayerId;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += InitializePlayerStats;
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value) && value.Object != null)
			{
				InitializePlayerStats(playerId);
			}
			EnsureOwnStatsInitializedAsync().Forget();
			PlayerAvatarSpawnReadyRegistry.MarkReady(base.Object.Id);
		}

		private async UniTaskVoid EnsureOwnStatsInitializedAsync()
		{
			if (!base.Object.HasStateAuthority)
			{
				return;
			}
			EntityStatEntityNetworkedBase ownEntity = GetComponentInChildren<EntityStatEntityNetworkedBase>(includeInactive: true);
			if (!(ownEntity == null))
			{
				int ownerPlayerId = base.Object.StateAuthority.PlayerId;
				await UniTask.WaitUntil(() => base.Object == null || !base.Object.IsValid || !base.Object.HasStateAuthority || (_spawnedEntityStatsModel.PlayerStats.TryGetValue(ownerPlayerId, out var value) && value == ownEntity && ownEntity.Object != null && ownEntity.Object.IsValid));
				if (!(base.Object == null) && base.Object.IsValid && base.Object.HasStateAuthority)
				{
					InitializePlayerStats(ownerPlayerId);
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= InitializePlayerStats;
			PlayerAvatarSpawnReadyRegistry.Clear(base.Object.Id);
		}

		private void InitializePlayerStats(int playerId)
		{
			if (base.Object.HasStateAuthority && base.Object.StateAuthority.PlayerId == playerId)
			{
				_playerStatsInitializeService.InitializeStats(playerId, InitialHealth);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			OriginalOwner = _OriginalOwner;
			InitialHealth = _InitialHealth;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_OriginalOwner = OriginalOwner;
			_InitialHealth = InitialHealth;
		}
	}
}
