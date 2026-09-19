using System;
using System.Collections.Generic;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.ChestScreamerModule.Scripts.Presets;
using Features.LevelModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.ChestScreamerModule.Scripts.Systems
{
	public class ChestScreamerGameplaySystem : IChestScreamerGameplay, IInitializable, IDisposable
	{
		private readonly IChestScreamerPresetResolver _presetResolver;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly IAudioService _audioService;

		private readonly List<ChestScreamerLifetime> _activeScreamers = new List<ChestScreamerLifetime>();

		public ChestScreamerGameplaySystem(IChestScreamerPresetResolver presetResolver, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, IAudioService audioService)
		{
			_presetResolver = presetResolver;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_audioService = audioService;
		}

		public void Initialize()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend += OnBeforeLevelChange;
		}

		public void Dispose()
		{
			_beforeLevelChangeNetworkEvent.OnNetworkEventSend -= OnBeforeLevelChange;
			Cleanup();
		}

		public bool HasPreset(ChestScreamerType type)
		{
			return GetPreset(type) != null;
		}

		public bool CanTrigger(ChestScreamerType type, float openDeltaDegrees, float grabHoldSeconds, bool isLidGrabbed)
		{
			ChestScreamerPreset preset = GetPreset(type);
			if (preset == null)
			{
				return false;
			}
			if (!(openDeltaDegrees >= preset.MinOpenAngleDegrees))
			{
				return grabHoldSeconds >= preset.MinGrabHoldSeconds && isLidGrabbed;
			}
			return true;
		}

		public float GetUnblockGrabTimer(ChestScreamerType type)
		{
			return GetPreset(type).UnblockGrabTimer;
		}

		public bool IsBlockedGrabbleAfterScreamer(ChestScreamerType type)
		{
			return GetPreset(type).IsBlockedGrabbleAfterScreamer;
		}

		public float GetOpenImpulsePerMass(ChestScreamerType type)
		{
			return GetPreset(type).OpenImpulsePerMass;
		}

		public float GetOpenImpulseForwardBias(ChestScreamerType type)
		{
			return GetPreset(type).OpenImpulseForwardBias;
		}

		public float GetHoldOpenAngleDegrees(ChestScreamerType type)
		{
			return GetPreset(type).HoldOpenAngleDegrees;
		}

		public void PlayFeedback(ChestScreamerType type, Vector3 position, Quaternion rotation, int soundSourceId)
		{
			ChestScreamerPreset preset = GetPreset(type);
			if (preset != null)
			{
				SpawnScreamerVisual(preset, position, rotation);
				PlaySound(preset.SwarmSound, position, soundSourceId);
				PlaySound(preset.LidSlamSound, position, soundSourceId);
			}
		}

		public void RestorePersistedScreamerVisual(ChestScreamerType type, Vector3 position, Quaternion rotation)
		{
			ChestScreamerPreset preset = GetPreset(type);
			if (preset != null && preset.IsBlockedGrabbleAfterScreamer)
			{
				SpawnScreamerVisual(preset, position, rotation);
			}
		}

		private void SpawnScreamerVisual(ChestScreamerPreset preset, Vector3 position, Quaternion rotation)
		{
			_activeScreamers.RemoveAll((ChestScreamerLifetime screamer) => screamer == null);
			ChestScreamerLifetime chestScreamerLifetime = UnityEngine.Object.Instantiate(preset.ScreamerPrefab, position, rotation);
			_activeScreamers.Add(chestScreamerLifetime);
			chestScreamerLifetime.Configure(preset.UnblockGrabTimer, !preset.IsBlockedGrabbleAfterScreamer);
		}

		private ChestScreamerPreset GetPreset(ChestScreamerType type)
		{
			return _presetResolver.GetPreset(type);
		}

		public void Cleanup()
		{
			for (int num = _activeScreamers.Count - 1; num >= 0; num--)
			{
				ChestScreamerLifetime chestScreamerLifetime = _activeScreamers[num];
				if (chestScreamerLifetime != null)
				{
					UnityEngine.Object.Destroy(chestScreamerLifetime.gameObject);
				}
			}
			_activeScreamers.Clear();
		}

		private void OnBeforeLevelChange(BeforeLevelChangeNetworkEvent _)
		{
			Cleanup();
		}

		private void PlaySound(EventReference eventReference, Vector3 position, int soundSourceId)
		{
			if (!eventReference.IsNull)
			{
				_audioService.PlayOneShot(eventReference, new GenericSoundSource(position, soundSourceId));
			}
		}
	}
}
