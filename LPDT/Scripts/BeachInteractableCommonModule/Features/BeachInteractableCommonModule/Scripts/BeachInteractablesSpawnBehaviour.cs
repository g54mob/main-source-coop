using System;
using System.Collections.Generic;
using System.Linq;
using Features.BeachPresetModule.Scripts.Core;
using Global.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[Serializable]
	public class BeachInteractablesSpawnBehaviour : BeachBehaviour
	{
		[Tooltip("Random beach interactables available for this preset. Randomized spawning chooses only from this list and applies the rules configured for the selected interactable.")]
		[SerializeField]
		private SerializableDictionary<BeachInteractableType, BeachInteractableSpawnRules> _beachInteractablesRandomPool = new SerializableDictionary<BeachInteractableType, BeachInteractableSpawnRules>();

		[Tooltip("Use randomized spawn count instead of a static count.")]
		[SerializeField]
		private bool _randomizeCount;

		[Tooltip("Static number of random beach interactables to spawn when count randomization is disabled.")]
		[Min(0f)]
		[SerializeField]
		private int _randomPoolCount = 1;

		[Tooltip("Static beach interactables available for this preset. Static beach interactables are spawned with 100% chance.")]
		[SerializeField]
		private SerializableDictionary<BeachInteractableType, BeachInteractableSpawnRules> _beachInteractablesStaticPool = new SerializableDictionary<BeachInteractableType, BeachInteractableSpawnRules>();

		[Tooltip("Location type for beach interactables. You may want to have different overrides for different locations.")]
		[SerializeField]
		private BeachInteractableLocationType _locationType;

		private IBeachInteractableSpawnServiceFacade _beachInteractableSpawnServiceFacade;

		private BeachInteractableSpawnModel _beachInteractableSpawnModel;

		private readonly List<IBeachInteractableController> _beachInteractableControllers = new List<IBeachInteractableController>();

		[Inject]
		public void InjectDependencies(IBeachInteractableSpawnServiceFacade beachInteractableSpawnServiceFacade, BeachInteractableSpawnModel beachInteractableSpawnModel)
		{
			_beachInteractableSpawnServiceFacade = beachInteractableSpawnServiceFacade;
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			SpawnRecordedInteractables(context);
			SpawnStaticInteractables(context);
			SpawnRandomInteractables(context);
		}

		public bool TryGetNewlyRecordedPreviewType(IReadOnlyList<RecordedBeachInteractableData> recorded, out BeachInteractableType type)
		{
			type = BeachInteractableType.None;
			bool result = false;
			foreach (KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules> item in _beachInteractablesStaticPool)
			{
				if ((item.Value & BeachInteractableSpawnRules.Recorded) != BeachInteractableSpawnRules.None)
				{
					BeachInteractableType key = item.Key;
					if (key != BeachInteractableType.BigButt && key != BeachInteractableType.Kraken && !IsAlreadyRecorded(item.Key, recorded))
					{
						type = item.Key;
						result = true;
					}
				}
			}
			return result;
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			foreach (IBeachInteractableController beachInteractableController in _beachInteractableControllers)
			{
				beachInteractableController.DespawnInteractable();
			}
			_beachInteractableControllers.Clear();
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_beachInteractablesRandomPool.Count == 0 && _beachInteractablesStaticPool.Count == 0)
			{
				beachValidationResult.AddError("Beach interactables pool cannot be empty.");
			}
			if (!_randomizeCount && _randomPoolCount < 0)
			{
				beachValidationResult.AddError("Beach interactable spawn count cannot be negative.");
			}
			if (_beachInteractablesRandomPool.ContainsKey(BeachInteractableType.None) || _beachInteractablesStaticPool.ContainsKey(BeachInteractableType.None))
			{
				beachValidationResult.AddError("Beach interactables pool cannot contain None.");
			}
			return beachValidationResult;
		}

		private void SpawnRecordedInteractables(BeachPresetRuntimeContext context)
		{
			foreach (RecordedBeachInteractableData recordedBeachInteractable in _beachInteractableSpawnModel.RecordedBeachInteractables)
			{
				try
				{
					IBeachInteractableController beachInteractableController = _beachInteractableSpawnServiceFacade.SpawnBeachInteractable(recordedBeachInteractable.RecordedBeachInteractableType, _locationType, recordedBeachInteractable.SpawnPointMarker, context.LevelType);
					if (beachInteractableController != null)
					{
						_beachInteractableControllers.Add(beachInteractableController);
					}
				}
				catch (Exception arg)
				{
					Debug.LogError($"[BeachInteractablesSpawnBehaviour] failed to spawn recorded interactable '{recordedBeachInteractable.RecordedBeachInteractableType}' — continuing with the rest of the beach. {arg}");
				}
			}
		}

		private void SpawnRandomInteractables(BeachPresetRuntimeContext context)
		{
			List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>> availableBeachInteractables = GetAvailableBeachInteractables(_beachInteractablesRandomPool);
			if (availableBeachInteractables.Count != 0)
			{
				int a = _randomPoolCount;
				if (_randomizeCount)
				{
					a = _beachInteractableSpawnModel.Random.Next(1, availableBeachInteractables.Count + 1);
				}
				List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>> list = new List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>>();
				a = Mathf.Min(a, availableBeachInteractables.Count);
				for (int i = 0; i < a; i++)
				{
					int index = _beachInteractableSpawnModel.Random.Next(0, availableBeachInteractables.Count);
					list.Add(availableBeachInteractables[index]);
					availableBeachInteractables.RemoveAt(index);
				}
				SpawnBeachInteractables(list, context);
			}
		}

		private void SpawnStaticInteractables(BeachPresetRuntimeContext context)
		{
			List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>> availableBeachInteractables = GetAvailableBeachInteractables(_beachInteractablesStaticPool);
			if (availableBeachInteractables.Count != 0)
			{
				SpawnBeachInteractables(availableBeachInteractables, context);
			}
		}

		private List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>> GetAvailableBeachInteractables(SerializableDictionary<BeachInteractableType, BeachInteractableSpawnRules> beachInteractablesPool)
		{
			return beachInteractablesPool.Where((KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules> x) => _beachInteractableSpawnModel.RecordedBeachInteractables.All((RecordedBeachInteractableData y) => y.RecordedBeachInteractableType != x.Key)).ToList();
		}

		private void SpawnBeachInteractables(List<KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules>> beachInteractablesToSpawn, BeachPresetRuntimeContext context)
		{
			foreach (KeyValuePair<BeachInteractableType, BeachInteractableSpawnRules> item in beachInteractablesToSpawn)
			{
				try
				{
					IBeachInteractableController beachInteractableController = _beachInteractableSpawnServiceFacade.SpawnBeachInteractable(item.Key, _locationType, item.Value, context.LevelType);
					if (beachInteractableController != null)
					{
						_beachInteractableControllers.Add(beachInteractableController);
					}
				}
				catch (Exception arg)
				{
					Debug.LogError($"[BeachInteractablesSpawnBehaviour] failed to spawn interactable '{item.Key}' — continuing with the rest of the beach. {arg}");
				}
			}
		}

		private static bool IsAlreadyRecorded(BeachInteractableType type, IReadOnlyList<RecordedBeachInteractableData> recorded)
		{
			if (recorded == null)
			{
				return false;
			}
			for (int i = 0; i < recorded.Count; i++)
			{
				if (recorded[i].RecordedBeachInteractableType == type)
				{
					return true;
				}
			}
			return false;
		}
	}
}
