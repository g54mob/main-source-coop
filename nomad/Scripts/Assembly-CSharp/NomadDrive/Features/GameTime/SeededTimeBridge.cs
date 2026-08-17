using System;
using Enviro;
using EvilCore;
using EvilCore.Extensions;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.GameTime
{
	public class SeededTimeBridge : MonoBehaviour
	{
		[Tooltip("Range the new-session starting hour is deterministically drawn from.")]
		[SerializeField]
		private SeededTimeConfig config;

		[Inject]
		private IGameSaveService _gameSaveService;

		private EnviroManager _enviroManager;

		private bool _applied;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		private void Update()
		{
			if (_applied || config == null)
			{
				return;
			}
			if (_enviroManager == null)
			{
				_enviroManager = EnviroManager.instance;
				if (_enviroManager == null || _enviroManager.Time == null)
				{
					return;
				}
			}
			WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
			if (instance == null || !instance.IsWorldFullyReady || instance.SeedManager == null)
			{
				return;
			}
			if (!NetworkServer.active)
			{
				_applied = true;
			}
			else if (_gameSaveService != null)
			{
				_applied = true;
				if (!_gameSaveService.IsLoadedWorld)
				{
					ApplySeededStartTime(instance);
				}
			}
		}

		private void ApplySeededStartTime(WorldGenerator worldGenerator)
		{
			System.Random rng = worldGenerator.SeedManager.CreateRandom(config.seedIdentifier);
			float timeOfDay = config.PickStartHour(rng);
			_enviroManager.Time.SetTimeOfDay(timeOfDay);
		}
	}
}
