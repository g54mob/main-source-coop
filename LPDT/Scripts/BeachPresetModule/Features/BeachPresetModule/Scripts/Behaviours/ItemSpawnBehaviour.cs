using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Behaviours
{
	[Serializable]
	public class ItemSpawnBehaviour : BeachBehaviour
	{
		[Serializable]
		private class ItemSpawnEntry
		{
			[Tooltip("Gameplay item prefab spawned through IItemSpawnService.")]
			[SerializeField]
			private NetworkBehaviour _itemPrefab;

			[Tooltip("Spawn this item relative to the beach entry point instead of the beach origin.")]
			[SerializeField]
			private bool _relativeToEntryPoint = true;

			[Tooltip("Local spawn offset from the selected beach reference point.")]
			[SerializeField]
			private Vector3 _localOffset;

			[Tooltip("Static local rotation used when random rotation is disabled.")]
			[SerializeField]
			private Vector3 _localRotation;

			[Tooltip("Use randomized spawn count instead of a static count.")]
			[SerializeField]
			private bool _randomizeCount;

			[Tooltip("Static number of items to spawn.")]
			[Min(0f)]
			[SerializeField]
			private int _count = 1;

			[Tooltip("Random item count range used when count randomization is enabled.")]
			[SerializeField]
			private BeachIntRange _countRange = new BeachIntRange(1, 3);

			[Tooltip("Random radius around the local offset for each spawned item.")]
			[Min(0f)]
			[SerializeField]
			private float _randomOffsetRadius;

			[Tooltip("Randomize yaw rotation for each spawned item.")]
			[SerializeField]
			private bool _randomizeYaw;

			[Tooltip("Pass custom item data to the spawned item instead of using its prefab defaults.")]
			[SerializeField]
			private bool _overrideItemData;

			[Tooltip("Custom item data used when Override Item Data is enabled.")]
			[SerializeField]
			private ItemData _itemData;

			[Tooltip("Add random physics force after spawning, using the existing item spawn service option.")]
			[SerializeField]
			private bool _addRandomForce;

			[Tooltip("Random physics force amount used when Add Random Force is enabled.")]
			[Min(0f)]
			[SerializeField]
			private float _randomForce;

			public int EvaluateCount()
			{
				if (!_randomizeCount)
				{
					return _count;
				}
				return _countRange.Evaluate();
			}

			public void Spawn(BeachPresetRuntimeContext context, IBeachItemSpawnService itemSpawnService, List<NetworkBehaviour> spawnedItems, int version, Func<int> getCurrentVersion)
			{
				int num = EvaluateCount();
				for (int i = 0; i < num; i++)
				{
					Vector3 localPosition = _localOffset + EvaluateRandomOffset();
					Vector3 position = context.ToWorldPosition(localPosition, _relativeToEntryPoint);
					Quaternion rotation = context.ToWorldRotation(EvaluateRotation());
					ItemData itemData = (_overrideItemData ? _itemData : null);
					SpawnAsync(itemSpawnService, spawnedItems, version, getCurrentVersion, position, rotation, itemData).Forget();
				}
			}

			public void Validate(BeachValidationResult result)
			{
				if (_itemPrefab == null)
				{
					result.AddError("Item spawn entry has no item prefab assigned.");
				}
				if (!_randomizeCount && _count < 0)
				{
					result.AddError("Item spawn count cannot be negative.");
				}
				if (_randomizeCount)
				{
					_countRange.Validate(result, "Item spawn count range");
					if (_countRange.Min < 0)
					{
						result.AddError("Item spawn count range cannot start below zero.");
					}
				}
				if (_randomOffsetRadius < 0f)
				{
					result.AddError("Item random offset radius cannot be negative.");
				}
				if (_addRandomForce && _randomForce < 0f)
				{
					result.AddError("Item random force cannot be negative.");
				}
			}

			private async UniTaskVoid SpawnAsync(IBeachItemSpawnService itemSpawnService, List<NetworkBehaviour> spawnedItems, int version, Func<int> getCurrentVersion, Vector3 position, Quaternion rotation, ItemData itemData)
			{
				NetworkBehaviour networkBehaviour = await itemSpawnService.SpawnItem(_itemPrefab, position, rotation, itemData, _addRandomForce, _randomForce);
				if (!(networkBehaviour == null))
				{
					if (version != getCurrentVersion())
					{
						itemSpawnService.Despawn(networkBehaviour);
					}
					else
					{
						spawnedItems.Add(networkBehaviour);
					}
				}
			}

			private Vector3 EvaluateRandomOffset()
			{
				if (_randomOffsetRadius <= 0f)
				{
					return Vector3.zero;
				}
				Vector2 vector = UnityEngine.Random.insideUnitCircle * _randomOffsetRadius;
				return new Vector3(vector.x, 0f, vector.y);
			}

			private Vector3 EvaluateRotation()
			{
				if (!_randomizeYaw)
				{
					return _localRotation;
				}
				return new Vector3(_localRotation.x, UnityEngine.Random.Range(0f, 360f), _localRotation.z);
			}
		}

		[Tooltip("Gameplay item spawn entries owned by this behaviour.")]
		[SerializeField]
		private List<ItemSpawnEntry> _spawnEntries = new List<ItemSpawnEntry>();

		private readonly List<NetworkBehaviour> _spawnedItems = new List<NetworkBehaviour>();

		private IBeachItemSpawnService _itemSpawnService;

		private int _version;

		public override int Order => 200;

		[Inject]
		public void InjectDependencies(IBeachItemSpawnService itemSpawnService)
		{
			_itemSpawnService = itemSpawnService;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			_version++;
			foreach (ItemSpawnEntry spawnEntry in _spawnEntries)
			{
				spawnEntry?.Spawn(context, _itemSpawnService, _spawnedItems, _version, () => _version);
			}
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			_version++;
			for (int num = _spawnedItems.Count - 1; num >= 0; num--)
			{
				_itemSpawnService.Despawn(_spawnedItems[num]);
			}
			_spawnedItems.Clear();
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_spawnEntries.Count == 0)
			{
				beachValidationResult.AddWarning("ItemSpawnBehaviour has no item spawn entries.");
			}
			foreach (ItemSpawnEntry spawnEntry in _spawnEntries)
			{
				spawnEntry?.Validate(beachValidationResult);
			}
			return beachValidationResult;
		}
	}
}
