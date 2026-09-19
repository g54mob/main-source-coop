using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.LevelModule.Scripts.RoomVariations;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Behaviours
{
	[Serializable]
	public class WeatherBehaviour : BeachBehaviour
	{
		[Serializable]
		private class WeatherPrefabEntry
		{
			[Tooltip("Local visual effect prefab, such as rain, wind, leaves, or mist particles.")]
			[SerializeField]
			private GameObject _prefab;

			[Tooltip("Spawn this effect relative to the beach entry point instead of the beach origin. Ignored when a room override is set.")]
			[SerializeField]
			private bool _relativeToEntryPoint;

			[Tooltip("Optional: when set, the effect is spawned once per registered room of this type, relative to that room's transform instead of the beach reference point.")]
			[SerializeField]
			private RoomType _roomOverride;

			[Tooltip("Local position offset from the selected beach reference point.")]
			[SerializeField]
			private Vector3 _localOffset;

			[Tooltip("Local rotation in Euler angles.")]
			[SerializeField]
			private Vector3 _localRotation;

			[Tooltip("Local scale applied to the spawned effect.")]
			[SerializeField]
			private Vector3 _localScale = Vector3.one;

			[Tooltip("Parent the spawned effect under the runtime beach anchor when one is supplied.")]
			[SerializeField]
			private bool _parentToRuntimeAnchor = true;

			public void Apply(BeachPresetRuntimeContext context, IBeachPrefabSpawnService prefabSpawnService, BeachPrefabSpawnHandle spawnHandle, LevelRoomsModel levelRoomsModel)
			{
				if (_roomOverride != RoomType.None)
				{
					ApplyToRooms(prefabSpawnService, spawnHandle, levelRoomsModel);
					return;
				}
				Vector3 position = context.ToWorldPosition(_localOffset, _relativeToEntryPoint);
				Quaternion rotation = context.ToWorldRotation(_localRotation);
				Transform parent = (_parentToRuntimeAnchor ? context.RuntimeAnchor : null);
				prefabSpawnService.Spawn(spawnHandle, _prefab, position, rotation, _localScale, parent);
			}

			public void Validate(BeachValidationResult result)
			{
				if (_prefab == null)
				{
					result.AddError("Weather visual effect entry has no prefab assigned.");
				}
				if (_roomOverride != RoomType.None && _relativeToEntryPoint)
				{
					result.AddWarning($"Weather visual effect entry overrides room {_roomOverride}, so the beach entry point offset is ignored.");
				}
			}

			private void ApplyToRooms(IBeachPrefabSpawnService prefabSpawnService, BeachPrefabSpawnHandle spawnHandle, LevelRoomsModel levelRoomsModel)
			{
				foreach (LevelRoomEntity room in levelRoomsModel.GetRooms(_roomOverride))
				{
					Transform roomTransform = room.RoomTransform;
					Vector3 position = roomTransform.TransformPoint(_localOffset);
					Quaternion rotation = roomTransform.rotation * Quaternion.Euler(_localRotation);
					Transform parent = (_parentToRuntimeAnchor ? roomTransform : null);
					prefabSpawnService.Spawn(spawnHandle, _prefab, position, rotation, _localScale, parent);
				}
			}
		}

		[Tooltip("Local non-item visual effects spawned by this weather behaviour.")]
		[SerializeField]
		private List<WeatherPrefabEntry> _visualEffectPrefabs = new List<WeatherPrefabEntry>();

		private IBeachPrefabSpawnService _prefabSpawnService;

		private BeachPrefabSpawnHandle _spawnHandle;

		private LevelRoomsModel _levelRoomsModel;

		public override int Order => 100;

		[Inject]
		public void InjectDependencies(IBeachPrefabSpawnService prefabSpawnService, LevelRoomsModel levelRoomsModel)
		{
			_prefabSpawnService = prefabSpawnService;
			_spawnHandle = prefabSpawnService.CreateHandle();
			_levelRoomsModel = levelRoomsModel;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			foreach (WeatherPrefabEntry visualEffectPrefab in _visualEffectPrefabs)
			{
				visualEffectPrefab?.Apply(context, _prefabSpawnService, _spawnHandle, _levelRoomsModel);
			}
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			_prefabSpawnService.Clear(_spawnHandle);
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_visualEffectPrefabs.Count == 0)
			{
				beachValidationResult.AddWarning("WeatherBehaviour has no visual effect prefabs.");
			}
			foreach (WeatherPrefabEntry visualEffectPrefab in _visualEffectPrefabs)
			{
				visualEffectPrefab?.Validate(beachValidationResult);
			}
			return beachValidationResult;
		}
	}
}
