using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FMOD;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Fusion;
using NetworkServices.ObjectsProvider;
using Photon.Voice.Unity.FMOD;
using UnityEngine;

namespace Features.VoiceControlModule.Scripts
{
	public class VoiceService : IVoiceService
	{
		private const int SPEAKER_SPAWN_MAX_ATTEMPTS = 10;

		private const float SPEAKER_SPAWN_RETRY_BASE_DELAY_SECONDS = 0.25f;

		private const float SPEAKER_SPAWN_RETRY_MAX_DELAY_SECONDS = 2f;

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly MultiplayerModel _multiplayerModel;

		private Dictionary<int, NetworkObject> _spawnedSpeakerObjectsNon3d = new Dictionary<int, NetworkObject>();

		private Dictionary<int, NetworkObject> _spawnedSpeakerObjects = new Dictionary<int, NetworkObject>();

		private readonly HashSet<int> _spawningNon3d = new HashSet<int>();

		private readonly HashSet<int> _spawning = new HashSet<int>();

		private Dictionary<int, List<VoiceEffectData>> _effectsCoroutine = new Dictionary<int, List<VoiceEffectData>>();

		private Dictionary<int, List<VoiceEffectData>> _effectsCoroutineNon3d = new Dictionary<int, List<VoiceEffectData>>();

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly VoiceConfig _voiceConfig;

		public VoiceService(SpawnedVoiceModel spawnedVoiceModel, MultiplayerModel multiplayerModel, ICoroutineRunner coroutineRunner, VoiceConfig voiceConfig)
		{
			_spawnedVoiceModel = spawnedVoiceModel;
			_multiplayerModel = multiplayerModel;
			_coroutineRunner = coroutineRunner;
			_voiceConfig = voiceConfig;
		}

		public async UniTask SpawnNon3dVoiceSpeaker(int playerId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
		{
			if (!_spawningNon3d.Add(playerId))
			{
				return;
			}
			try
			{
				DespawnOrphanedVoiceSpeakersForPlayer(playerId);
				if (HasValidSpeaker(_spawnedSpeakerObjectsNon3d, playerId))
				{
					return;
				}
				foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
				{
					if (activePlayer.PlayerId == playerId)
					{
						NetworkObject networkObject = await SpawnSpeakerWithRetryAsync(_voiceConfig.GetPlayerSpeaker(PlayerSpeakerType.NonSpatializer), position, rotation, activePlayer);
						if (networkObject != null)
						{
							_spawnedSpeakerObjectsNon3d.Add(playerId, networkObject);
						}
						return;
					}
				}
			}
			finally
			{
				_spawningNon3d.Remove(playerId);
			}
		}

		public async UniTask SpawnVoiceSpeaker(int playerId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
		{
			if (!_spawning.Add(playerId))
			{
				return;
			}
			try
			{
				DespawnOrphanedVoiceSpeakersForPlayer(playerId);
				if (HasValidSpeaker(_spawnedSpeakerObjects, playerId))
				{
					return;
				}
				foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
				{
					if (activePlayer.PlayerId == playerId)
					{
						NetworkObject networkObject = await SpawnSpeakerWithRetryAsync(_voiceConfig.GetPlayerSpeaker(PlayerSpeakerType.Spatializer), position, rotation, activePlayer);
						if (networkObject != null)
						{
							_spawnedSpeakerObjects.Add(playerId, networkObject);
						}
						return;
					}
				}
			}
			finally
			{
				_spawning.Remove(playerId);
			}
		}

		private async UniTask<NetworkObject> SpawnSpeakerWithRetryAsync(GameObject prefab, Vector3 position, Quaternion rotation, PlayerRef playerRef)
		{
			int maxAttempts = Mathf.Max(1, 10);
			float delay = Mathf.Max(0f, 0.25f);
			float maxDelay = Mathf.Max(delay, 2f);
			for (int attempt = 1; attempt <= maxAttempts; attempt++)
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (networkRunner == null || !networkRunner.IsRunning)
				{
					return null;
				}
				if (!networkRunner.ActivePlayers.Any((PlayerRef activePlayer) => activePlayer.PlayerId == playerRef.PlayerId))
				{
					return null;
				}
				try
				{
					NetworkObject networkObject = await networkRunner.SpawnAsync(prefab, position, rotation, playerRef);
					if (networkObject != null && networkObject.IsValid)
					{
						return networkObject;
					}
					if (networkObject != null)
					{
						SafeDespawnSpeaker(networkObject);
					}
				}
				catch (Exception ex)
				{
					if (attempt >= maxAttempts)
					{
						UnityEngine.Debug.LogWarning($"[VoiceService] Voice speaker spawn for player {playerRef.PlayerId} gave up after {attempt} attempt(s) ({ex.GetType().Name}: {ex.Message}).");
						return null;
					}
				}
				await UniTask.Delay(TimeSpan.FromSeconds(delay));
				delay = Mathf.Min(delay * 2f, maxDelay);
			}
			return null;
		}

		private void SafeDespawnSpeaker(NetworkObject speaker)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner != null && networkRunner.IsRunning && speaker != null && speaker.IsValid)
			{
				networkRunner.Despawn(speaker);
			}
		}

		public void DespawnVoiceSpeaker(int playerId)
		{
			if (_spawnedSpeakerObjects.ContainsKey(playerId))
			{
				_spawnedSpeakerObjects[playerId].DespawnHierarchy();
				_spawnedSpeakerObjects.Remove(playerId);
			}
		}

		public void DespawnNon3dVoiceSpeaker(int playerId)
		{
			if (_spawnedSpeakerObjectsNon3d.ContainsKey(playerId))
			{
				_spawnedSpeakerObjectsNon3d[playerId].DespawnHierarchy();
				_spawnedSpeakerObjectsNon3d.Remove(playerId);
			}
		}

		public void ProcessFadeEffectForSpeaker(int playerId, float targetValue, float duration, string effectName)
		{
			if (!_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (!_effectsCoroutine.ContainsKey(playerId))
			{
				VoiceEffectData voiceEffectData = new VoiceEffectData(effectName, null, isRunning: true);
				voiceEffectData.EffectCoroutine = _coroutineRunner.StartCoroutine(FadeCoroutine(value, duration, targetValue, effectName, voiceEffectData));
				_effectsCoroutine.Add(playerId, new List<VoiceEffectData> { voiceEffectData });
				return;
			}
			VoiceEffectData voiceEffectData2 = _effectsCoroutine[playerId].FirstOrDefault((VoiceEffectData e) => e.EffectName == effectName);
			if (voiceEffectData2 != null)
			{
				if (voiceEffectData2.EffectCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(voiceEffectData2.EffectCoroutine);
				}
				_effectsCoroutine[playerId].Remove(voiceEffectData2);
			}
			VoiceEffectData voiceEffectData3 = new VoiceEffectData(effectName, null, isRunning: true);
			voiceEffectData3.EffectCoroutine = _coroutineRunner.StartCoroutine(FadeCoroutine(value, duration, targetValue, effectName, voiceEffectData3));
			_effectsCoroutine[playerId].Add(voiceEffectData3);
		}

		public void ProcessEffectForSpeaker(int playerId, float targetValue, string effectName)
		{
			if (_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value))
			{
				value.EventInstance.setParameterByName(effectName, targetValue);
			}
		}

		public void ProcessEffectForSpeakerNon3d(int playerId, float targetValue, string effectName)
		{
			if (_spawnedVoiceModel.SpeakersNon3d.TryGetValue(playerId, out var value))
			{
				value.EventInstance.setParameterByName(effectName, targetValue);
			}
		}

		public bool TryGetEffectValueForSpeaker(int playerId, string effectName, out float value)
		{
			if (!_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value2))
			{
				value = 0f;
				return false;
			}
			value2.EventInstance.getParameterByName(effectName, out value);
			return true;
		}

		public void ProcessEffectForSpeakerNon3d(int playerId, float targetValue, float duration, string effectName)
		{
			if (!_spawnedVoiceModel.SpeakersNon3d.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (!_effectsCoroutineNon3d.ContainsKey(playerId))
			{
				VoiceEffectData voiceEffectData = new VoiceEffectData(effectName, null, isRunning: true);
				voiceEffectData.EffectCoroutine = _coroutineRunner.StartCoroutine(FadeCoroutine(value, duration, targetValue, effectName, voiceEffectData));
				_effectsCoroutineNon3d.Add(playerId, new List<VoiceEffectData> { voiceEffectData });
				return;
			}
			VoiceEffectData voiceEffectData2 = _effectsCoroutineNon3d[playerId].FirstOrDefault((VoiceEffectData e) => e.EffectName == effectName);
			if (voiceEffectData2 != null)
			{
				if (voiceEffectData2.EffectCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(voiceEffectData2.EffectCoroutine);
				}
				_effectsCoroutineNon3d[playerId].Remove(voiceEffectData2);
			}
			VoiceEffectData voiceEffectData3 = new VoiceEffectData(effectName, null, isRunning: true);
			voiceEffectData3.EffectCoroutine = _coroutineRunner.StartCoroutine(FadeCoroutine(value, duration, targetValue, effectName, voiceEffectData3));
			_effectsCoroutineNon3d[playerId].Add(voiceEffectData3);
		}

		public void ProcessFadeVolumeEffectForSpeaker(int playerId, float targetValue, float duration)
		{
			if (!_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (duration == 0f)
			{
				value.EventInstance.setVolume(targetValue);
				return;
			}
			if (!_effectsCoroutine.ContainsKey(playerId))
			{
				VoiceEffectData voiceEffectData = new VoiceEffectData("Volume", null, isRunning: true);
				voiceEffectData.EffectCoroutine = _coroutineRunner.StartCoroutine(VolumeFadeCoroutine(value, duration, targetValue, voiceEffectData));
				_effectsCoroutine.Add(playerId, new List<VoiceEffectData> { voiceEffectData });
				return;
			}
			VoiceEffectData voiceEffectData2 = _effectsCoroutine[playerId].FirstOrDefault((VoiceEffectData e) => e.EffectName == "Volume");
			if (voiceEffectData2 != null)
			{
				if (voiceEffectData2.EffectCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(voiceEffectData2.EffectCoroutine);
				}
				_effectsCoroutine[playerId].Remove(voiceEffectData2);
			}
			VoiceEffectData voiceEffectData3 = new VoiceEffectData("Volume", null, isRunning: true);
			voiceEffectData3.EffectCoroutine = _coroutineRunner.StartCoroutine(VolumeFadeCoroutine(value, duration, targetValue, voiceEffectData3));
			_effectsCoroutine[playerId].Add(voiceEffectData3);
		}

		public void ProcessFadeVolumeEffectForSpeakerNon3d(int playerId, float targetValue, float duration)
		{
			if (!_spawnedVoiceModel.SpeakersNon3d.TryGetValue(playerId, out var value))
			{
				return;
			}
			if (duration == 0f)
			{
				value.EventInstance.setVolume(targetValue);
				return;
			}
			if (!_effectsCoroutineNon3d.ContainsKey(playerId))
			{
				VoiceEffectData voiceEffectData = new VoiceEffectData("Volume", null, isRunning: true);
				voiceEffectData.EffectCoroutine = _coroutineRunner.StartCoroutine(VolumeFadeCoroutine(value, duration, targetValue, voiceEffectData));
				_effectsCoroutineNon3d.Add(playerId, new List<VoiceEffectData> { voiceEffectData });
				return;
			}
			VoiceEffectData voiceEffectData2 = _effectsCoroutineNon3d[playerId].FirstOrDefault((VoiceEffectData e) => e.EffectName == "Volume");
			if (voiceEffectData2 != null)
			{
				if (voiceEffectData2.EffectCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(voiceEffectData2.EffectCoroutine);
				}
				_effectsCoroutineNon3d[playerId].Remove(voiceEffectData2);
			}
			VoiceEffectData voiceEffectData3 = new VoiceEffectData("Volume", null, isRunning: true);
			voiceEffectData3.EffectCoroutine = _coroutineRunner.StartCoroutine(VolumeFadeCoroutine(value, duration, targetValue, voiceEffectData3));
			_effectsCoroutineNon3d[playerId].Add(voiceEffectData3);
		}

		public void SetVolumeForSpeakerNon3d(int playerId, float targetValue)
		{
			if (_spawnedVoiceModel.SpeakersNon3d.TryGetValue(playerId, out var value))
			{
				value.EventInstance.setVolume(targetValue);
			}
		}

		public void SetVolumeForSpeaker(int playerId, float targetValue)
		{
			if (_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value))
			{
				value.EventInstance.setVolume(targetValue);
			}
		}

		public float GetVolumeForSpeaker(int playerId)
		{
			if (!_spawnedVoiceModel.Speakers.TryGetValue(playerId, out var value))
			{
				return 0f;
			}
			float volume = 0f;
			if (value.EventInstance.getVolume(out volume, out var _) == RESULT.OK)
			{
				return volume;
			}
			return 0f;
		}

		private IEnumerator FadeCoroutine(SpeakerFMOD speaker, float duration, float targetValue, string parameterName, VoiceEffectData data)
		{
			float elapsed = 0f;
			speaker.EventInstance.getParameterByName(parameterName, out var currentPitch);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / duration;
				speaker.EventInstance.setParameterByName(parameterName, Mathf.Lerp(currentPitch, targetValue, t));
				yield return null;
			}
			speaker.EventInstance.setParameterByName(parameterName, targetValue);
			data.IsRunning = false;
		}

		private IEnumerator VolumeFadeCoroutine(SpeakerFMOD speaker, float duration, float targetValue, VoiceEffectData data)
		{
			float elapsed = 0f;
			speaker.EventInstance.getVolume(out var currentVolume, out var _);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / duration;
				float b = Mathf.Lerp(currentVolume, targetValue, t);
				speaker.EventInstance.setVolume(Mathf.Lerp(currentVolume, b, t));
				yield return null;
			}
			speaker.EventInstance.setVolume(targetValue);
			data.IsRunning = false;
		}

		public bool IsVoiceEffectPlaying(int playerId)
		{
			if (!_effectsCoroutine.ContainsKey(playerId))
			{
				return false;
			}
			foreach (VoiceEffectData item in _effectsCoroutine[playerId])
			{
				if (item.IsRunning)
				{
					return true;
				}
			}
			return false;
		}

		private bool HasValidSpeaker(Dictionary<int, NetworkObject> speakers, int playerId)
		{
			if (!speakers.TryGetValue(playerId, out var value))
			{
				return false;
			}
			if (value != null && value.IsValid)
			{
				return true;
			}
			speakers.Remove(playerId);
			return false;
		}

		private void DespawnOrphanedVoiceSpeakersForPlayer(int playerId)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			List<NetworkObject> list = new List<NetworkObject>();
			foreach (NetworkObject allNetworkObject in networkRunner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.InputAuthority.PlayerId == playerId && !(allNetworkObject.GetComponent<PlayerSpeakerAutoRegister>() == null))
				{
					list.Add(allNetworkObject);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			_spawnedSpeakerObjects.Remove(playerId);
			_spawnedSpeakerObjectsNon3d.Remove(playerId);
			foreach (NetworkObject item in list)
			{
				networkRunner.Despawn(item);
			}
		}
	}
}
