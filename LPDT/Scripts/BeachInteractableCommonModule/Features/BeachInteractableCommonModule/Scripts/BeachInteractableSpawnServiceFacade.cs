using System;
using System.Collections.Generic;
using System.Linq;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class BeachInteractableSpawnServiceFacade : IBeachInteractableSpawnServiceFacade
	{
		private readonly LocalBeachInteractableSpawnService _localBeachInteractableSpawnService;

		private readonly NetworkBeachInteractableSpawnService _networkBeachInteractableSpawnService;

		private readonly BeachInteractableSpawnConfiguration _beachInteractableSpawnConfiguration;

		private readonly IBeachInteractableSpawnAcquireService _beachInteractableSpawnAcquireService;

		private readonly BeachInteractableSpawnModel _beachInteractableSpawnModel;

		private readonly BeachInteractableSpawnEvent _beachInteractableSpawnEvent;

		public BeachInteractableSpawnServiceFacade(LocalBeachInteractableSpawnService localBeachInteractableSpawnService, NetworkBeachInteractableSpawnService networkBeachInteractableSpawnService, BeachInteractableSpawnConfiguration beachInteractableSpawnConfiguration, IBeachInteractableSpawnAcquireService beachInteractableSpawnAcquireService, BeachInteractableSpawnModel beachInteractableSpawnModel, BeachInteractableSpawnEvent beachInteractableSpawnEvent)
		{
			_localBeachInteractableSpawnService = localBeachInteractableSpawnService;
			_networkBeachInteractableSpawnService = networkBeachInteractableSpawnService;
			_beachInteractableSpawnConfiguration = beachInteractableSpawnConfiguration;
			_beachInteractableSpawnAcquireService = beachInteractableSpawnAcquireService;
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
			_beachInteractableSpawnEvent = beachInteractableSpawnEvent;
		}

		public IBeachInteractableController SpawnBeachInteractable(BeachInteractableType type, BeachInteractableLocationType beachInteractableLocationType, BeachInteractableSpawnRules spawnRules, LevelType levelType)
		{
			List<BeachInteractableSpawnData> allowedSpawnPoints = _beachInteractableSpawnAcquireService.GetAllowedSpawnPoints(type);
			if (allowedSpawnPoints.Count == 0)
			{
				Debug.LogWarning($"[HoopSpawn] No allowed spawn points for '{type}' on {levelType} — skipping spawn.");
				return null;
			}
			BeachInteractableSpawnData beachInteractableSpawnData = allowedSpawnPoints[_beachInteractableSpawnModel.Random.Next(0, allowedSpawnPoints.Count)];
			IBeachInteractableController result = SpawnBeachInteractable(type, beachInteractableLocationType, beachInteractableSpawnData.Marker, levelType);
			if ((spawnRules & BeachInteractableSpawnRules.Recorded) != BeachInteractableSpawnRules.None)
			{
				_beachInteractableSpawnEvent.InvokeBeachInteractableSpawned(type);
				_beachInteractableSpawnModel.RecordedBeachInteractables.Add(new RecordedBeachInteractableData(type, beachInteractableSpawnData.Marker));
			}
			return result;
		}

		public IBeachInteractableController SpawnBeachInteractable(BeachInteractableType type, BeachInteractableLocationType beachInteractableLocationType, int spawnPointMarker, LevelType levelType)
		{
			BeachInteractableSpawnData beachInteractableSpawnData = _beachInteractableSpawnModel.BeachInteractablesSpawnData.FirstOrDefault((BeachInteractableSpawnData x) => x.Marker == spawnPointMarker);
			IBeachInteractableController result = GetBeachInteractableSpawnService(type).SpawnBeachInteractable(new BeachInteractableIdentifier(levelType, type, spawnPointMarker), beachInteractableLocationType, beachInteractableSpawnData.Position, beachInteractableSpawnData.Rotation);
			_beachInteractableSpawnModel.RemoveBeachInteractableSpawnData(beachInteractableSpawnData);
			return result;
		}

		private IBeachInteractableSpawnService GetBeachInteractableSpawnService(BeachInteractableType type)
		{
			if (_beachInteractableSpawnConfiguration.LocalBeachInteractablePool.ContainsKey(type))
			{
				return _localBeachInteractableSpawnService;
			}
			if (_beachInteractableSpawnConfiguration.NetworkBeachInteractablePool.ContainsKey(type))
			{
				return _networkBeachInteractableSpawnService;
			}
			throw new ArgumentOutOfRangeException($"{type} was not present in beach interactable spawn pool");
		}
	}
}
