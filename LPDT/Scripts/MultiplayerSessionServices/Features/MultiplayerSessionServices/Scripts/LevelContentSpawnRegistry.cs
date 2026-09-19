using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	public sealed class LevelContentSpawnRegistry : ILevelContentSpawnRegistry
	{
		private readonly List<ILevelContentSpawnProvider> _providers = new List<ILevelContentSpawnProvider>();

		private bool _hasGateFired;

		public void Register(ILevelContentSpawnProvider provider)
		{
			if (!_providers.Contains(provider))
			{
				_providers.Add(provider);
				if (_hasGateFired)
				{
					SpawnProviderIsolatedAsync(provider).Forget();
				}
			}
		}

		public void Unregister(ILevelContentSpawnProvider provider)
		{
			_providers.Remove(provider);
		}

		public void BeginNewLevel()
		{
			_hasGateFired = false;
		}

		public UniTask SpawnAllAsync()
		{
			_hasGateFired = true;
			ILevelContentSpawnProvider[] array = _providers.ToArray();
			Debug.Log(string.Format("[ContentSpawn] SpawnAllAsync firing with {0} registered provider(s): [{1}]. A knife/basketball provider MISSING here means its beach interactable had not spawned yet when the content gate ran.", array.Length, string.Join(", ", array.Select((ILevelContentSpawnProvider p) => p.GetType().Name))));
			UniTask[] array2 = new UniTask[array.Length];
			for (int num = 0; num < array.Length; num++)
			{
				array2[num] = SpawnProviderIsolatedAsync(array[num]);
			}
			return UniTask.WhenAll(array2);
		}

		private async UniTask SpawnProviderIsolatedAsync(ILevelContentSpawnProvider provider)
		{
			try
			{
				await provider.SpawnLevelContentAsync();
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				Debug.LogError($"[LevelContentSpawnRegistry] provider '{provider.GetType().Name}' failed to spawn level content — skipping it so the rest of the level still populates. {ex}");
			}
		}
	}
}
