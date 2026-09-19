using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[NetworkBehaviourWeaved(1)]
	public class SimpleEnemyDeadProcessor : NetworkBehaviour, IEnemyDeadProcessor
	{
		[SerializeField]
		private Transform _overrideSpawnPoint;

		[SerializeField]
		private EnemyLootConfiguration _lootConfiguration;

		[SerializeField]
		private NetworkObject _deathParticle;

		[Tooltip("When enabled, rolls one loot entry per transform in Composite Spawn Points instead of using the configuration drop count around a single point.")]
		[SerializeField]
		private bool _useCompositeSpawnPoints;

		[SerializeField]
		private List<Transform> _compositeSpawnPoints = new List<Transform>();

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsNeedToSpawnItem", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsNeedToSpawnItem = true;

		private IItemSpawnService _itemSpawnService;

		private IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsNeedToSpawnItem
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimpleEnemyDeadProcessor.IsNeedToSpawnItem. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimpleEnemyDeadProcessor.IsNeedToSpawnItem. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Inject]
		public void InjectDependencies(IItemSpawnService itemSpawnService, IPlayerDeadPartSpawnService playerDeadPartSpawnService)
		{
			_itemSpawnService = itemSpawnService;
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
		}

		public void ProcessEnemyDeath()
		{
			if (!(base.Runner == null) && !base.Runner.IsShutdown && base.Runner.IsSharedModeMasterClient && _lootConfiguration != null && _lootConfiguration.HasLoot && IsNeedToSpawnItem)
			{
				SpawnDeadItems();
			}
		}

		private void SpawnDeadItems()
		{
			Vector3 vector = ResolveDefaultSpawnPosition();
			if (_useCompositeSpawnPoints)
			{
				SpawnDeadItemsAtCompositePoints();
			}
			else
			{
				SpawnDeadItemsAtDefaultPoint(vector);
			}
			if (!(_deathParticle == null))
			{
				base.Runner.SpawnAsync(_deathParticle, vector, Quaternion.identity, base.Runner.LocalPlayer);
			}
		}

		private void SpawnDeadItemsAtDefaultPoint(Vector3 spawnPosition)
		{
			int num = _lootConfiguration.RollDropCount();
			for (int i = 0; i < num; i++)
			{
				if (!_lootConfiguration.TryRollEntry(out var rolledEntry))
				{
					break;
				}
				SpawnLootEntry(rolledEntry, spawnPosition, Quaternion.identity, useItemCount: true);
			}
		}

		private void SpawnDeadItemsAtCompositePoints()
		{
			if (_compositeSpawnPoints == null || _compositeSpawnPoints.Count == 0)
			{
				return;
			}
			for (int i = 0; i < _compositeSpawnPoints.Count; i++)
			{
				Transform transform = _compositeSpawnPoints[i];
				if (!(transform == null))
				{
					if (!_lootConfiguration.TryRollEntry(out var rolledEntry))
					{
						break;
					}
					SpawnLootEntry(rolledEntry, transform.position, transform.rotation, useItemCount: false);
				}
			}
		}

		private void SpawnLootEntry(EnemyLootEntry entry, Vector3 position, Quaternion rotation, bool useItemCount)
		{
			switch (entry.Kind)
			{
			case EnemyLootKind.Item:
				SpawnItemLoot(entry, position, rotation, useItemCount);
				break;
			case EnemyLootKind.DeadPart:
				SpawnDeadPartLoot(entry, position).Forget();
				break;
			}
		}

		private void SpawnItemLoot(EnemyLootEntry entry, Vector3 position, Quaternion rotation, bool useItemCount)
		{
			if (!useItemCount)
			{
				_itemSpawnService.SpawnItem(entry.Item, position, rotation, null, entry.UseSpread).Forget();
			}
			else
			{
				_itemSpawnService.SpawnItemsMultiple(entry.Item, position, entry.ItemCount, 0f, entry.UseSpread, null, entry.UseSpread).Forget();
			}
		}

		private async UniTaskVoid SpawnDeadPartLoot(EnemyLootEntry entry, Vector3 position)
		{
			await _playerDeadPartSpawnService.SpawnPlayerDeadPart(entry.DeadPartType, position, entry.DeadPartUsageCount, entry.UseSpread);
		}

		private Vector3 ResolveDefaultSpawnPosition()
		{
			if (!(_overrideSpawnPoint != null))
			{
				return base.transform.position;
			}
			return _overrideSpawnPoint.position;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsNeedToSpawnItem = _IsNeedToSpawnItem;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsNeedToSpawnItem = IsNeedToSpawnItem;
		}
	}
}
