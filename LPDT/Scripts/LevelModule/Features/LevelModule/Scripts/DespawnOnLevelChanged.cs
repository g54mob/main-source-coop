using System;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class DespawnOnLevelChanged : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private LevelModel _levelModel;

		private bool _isDespawnRequested;

		private bool _isSpawned;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SpawnedOnLevel", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LevelType _SpawnedOnLevel;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe LevelType SpawnedOnLevel
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing DespawnOnLevelChanged.SpawnedOnLevel. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(LevelType*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing DespawnOnLevelChanged.SpawnedOnLevel. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(LevelType*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, LevelModel levelModel)
		{
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_levelModel = levelModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isDespawnRequested = false;
			if (base.HasStateAuthority)
			{
				SpawnedOnLevel = _levelModel.CurrentLevel;
			}
			_isSpawned = true;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += OnBeforeLevelChangeEvent;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= OnBeforeLevelChangeEvent;
			base.Despawned(runner, hasState);
		}

		private void OnBeforeLevelChangeEvent(BeforeLevelChangeNetworkEvent evt)
		{
			if (_isSpawned)
			{
				TryDespawn(evt);
			}
		}

		protected virtual void TryDespawn(BeforeLevelChangeNetworkEvent evt)
		{
			if (!ShouldIgnoreLevelChangeEvent(SpawnedOnLevel, evt.PastLevelType, evt.NewLevelType))
			{
				_isDespawnRequested = true;
				if (base.HasStateAuthority)
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		public static bool ShouldIgnoreLevelChangeEvent(LevelType spawnedOnLevel, LevelType pastLevelType, LevelType newLevelType)
		{
			if (spawnedOnLevel != LevelType.None && newLevelType == spawnedOnLevel)
			{
				return true;
			}
			if (newLevelType == LevelType.None && spawnedOnLevel != LevelType.None && pastLevelType != spawnedOnLevel)
			{
				return true;
			}
			return false;
		}

		public virtual void StateAuthorityChanged()
		{
			if (_isDespawnRequested && base.HasStateAuthority && base.Object != null && base.Object.IsValid)
			{
				base.Object.DespawnHierarchy();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SpawnedOnLevel = _SpawnedOnLevel;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SpawnedOnLevel = SpawnedOnLevel;
		}
	}
}
