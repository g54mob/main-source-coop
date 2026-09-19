using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.VoiceControlModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.VoiceOcclusionModule.Scripts
{
	public class VoiceOcclusionSystem : IInitializable, IDisposable
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly IVoiceService _voiceService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly IPlayerStateService _playerStateService;

		private readonly CameraModel _cameraModel;

		private readonly VoiceDistanceModel _voiceDistanceModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private bool _isLocalPlayerInStore;

		private const int OCCLUSION_RAY_COUNT = 3;

		public VoiceOcclusionSystem(IGameUpdater gameUpdater, IVoiceService voiceService, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration, IPlayerStateService playerStateService, CameraModel cameraModel, VoiceDistanceModel voiceDistanceModel, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_gameUpdater = gameUpdater;
			_voiceService = voiceService;
			_playerMovableModel = playerMovableModel;
			_multiplayerModel = multiplayerModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
			_playerStateService = playerStateService;
			_cameraModel = cameraModel;
			_voiceDistanceModel = voiceDistanceModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += ProcessVoice;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= ProcessVoice;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
		}

		private void OnPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (playerStateData.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				if (playerStateData.PlayerState == PlayerState.Store)
				{
					_isLocalPlayerInStore = true;
				}
				else
				{
					_isLocalPlayerInStore = false;
				}
			}
		}

		private void ProcessVoice()
		{
			ProcessDistanceBasedAttenuation();
			ProcessDistanceBasedEffects();
			ProcessSoundOcclusion();
		}

		private void ProcessSoundOcclusion()
		{
			if (_playerMovableModel.LocalMovable == null || _playerMovableModel.LocalMovable.CameraPositionTransform == null)
			{
				return;
			}
			Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
			Transform transform = _playerMovableModel.LocalMovable.transform;
			bool flag = _playerStateService.IsPlayerDead(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer == allCharacterMovable.Key || allCharacterMovable.Value == null || allCharacterMovable.Value.CameraPositionTransform == null)
				{
					continue;
				}
				Vector3 position2 = allCharacterMovable.Value.CameraPositionTransform.position;
				Vector3 offset = position2 - position;
				float occlusionAmount = GetOcclusionAmount(position, position2, transform, allCharacterMovable.Value.transform);
				if (occlusionAmount > 0f && !flag)
				{
					float normalizedWeightedDistance = GetNormalizedWeightedDistance(offset, _voiceOcclusionConfiguration.MaxDistance);
					float b = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, occlusionAmount * normalizedWeightedDistance);
					if (_voiceService.TryGetEffectValueForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.LowPassParameterName, out var value))
					{
						float targetValue = Mathf.Lerp(value, b, Time.deltaTime * 5f);
						_voiceService.ProcessEffectForSpeaker(allCharacterMovable.Key.PlayerId, targetValue, _voiceOcclusionConfiguration.LowPassParameterName);
					}
				}
				else
				{
					ResetSpeakerEffects(allCharacterMovable.Key.PlayerId);
				}
			}
		}

		private float GetOcclusionAmount(Vector3 localPos, Vector3 targetPos, Transform localRoot, Transform speakerRoot)
		{
			Vector3 vector = Vector3.Cross(Vector3.up, targetPos - localPos);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.right;
			}
			else
			{
				vector.Normalize();
			}
			float occlusionRaySpread = _voiceOcclusionConfiguration.OcclusionRaySpread;
			int num = 0;
			if (IsRayOccluded(localPos, targetPos, localRoot, speakerRoot))
			{
				num++;
			}
			if (IsRayOccluded(localPos + vector * occlusionRaySpread, targetPos + vector * occlusionRaySpread, localRoot, speakerRoot))
			{
				num++;
			}
			if (IsRayOccluded(localPos - vector * occlusionRaySpread, targetPos - vector * occlusionRaySpread, localRoot, speakerRoot))
			{
				num++;
			}
			return (float)num / 3f;
		}

		private bool IsRayOccluded(Vector3 from, Vector3 to, Transform localRoot, Transform speakerRoot)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude <= Mathf.Epsilon)
			{
				return false;
			}
			if (!Physics.Raycast(from, vector / magnitude, out var hitInfo, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask, QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			Transform transform = hitInfo.collider.transform;
			if (!transform.IsChildOf(speakerRoot))
			{
				return !transform.IsChildOf(localRoot);
			}
			return false;
		}

		private float GetWeightedDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedWeightedDistance(Vector3 offset, float maxDistance)
		{
			return ApplyVerticalFalloff(Mathf.Clamp01(GetWeightedDistance(offset) / Mathf.Max(0.01f, maxDistance)), offset);
		}

		private float ApplyVerticalFalloff(float normalizedDistance, Vector3 offset)
		{
			float weightedDistance = GetWeightedDistance(offset);
			if (weightedDistance <= Mathf.Epsilon)
			{
				return normalizedDistance;
			}
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(normalizedDistance, p);
		}

		private void ResetSpeakerEffects(int playerId)
		{
			if (_voiceService.TryGetEffectValueForSpeaker(playerId, _voiceOcclusionConfiguration.LowPassParameterName, out var value))
			{
				float targetValue = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
				_voiceService.ProcessEffectForSpeaker(playerId, targetValue, _voiceOcclusionConfiguration.LowPassParameterName);
			}
		}

		private void ProcessDistanceBasedAttenuation()
		{
			if (_playerMovableModel.LocalMovable == null || _cameraModel.CameraObject == null)
			{
				return;
			}
			Vector3 position = _cameraModel.CameraObject.transform.position;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer == allCharacterMovable.Key || allCharacterMovable.Value == null || allCharacterMovable.Value.CameraPositionTransform == null)
				{
					continue;
				}
				if (_voiceService.TryGetEffectValueForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.DistanceParameterName, out var _) && _isLocalPlayerInStore)
				{
					_voiceService.ProcessEffectForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.StoreVoiceDistanceParameterValue, _voiceOcclusionConfiguration.DistanceParameterName);
					continue;
				}
				Vector3 offset = allCharacterMovable.Value.CameraPositionTransform.position - position;
				float magnitude = offset.magnitude;
				_voiceDistanceModel.Distances[allCharacterMovable.Key.PlayerId] = magnitude;
				float value2;
				float num = (_voiceDistanceModel.MaxDistanceMultiplier.TryGetValue(allCharacterMovable.Key.PlayerId, out value2) ? value2 : 1f);
				float normalizedDistance = Mathf.Clamp01(GetWeightedDistance(offset) * num / _voiceOcclusionConfiguration.MaxAttenuationDistance);
				normalizedDistance = ApplyVerticalFalloff(normalizedDistance, offset);
				float b = _voiceOcclusionConfiguration.AttenuationCurve.Evaluate(normalizedDistance);
				if (_voiceService.TryGetEffectValueForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.DistanceParameterName, out var value3))
				{
					float targetValue = Mathf.Lerp(value3, b, Time.deltaTime * 5f);
					_voiceService.ProcessEffectForSpeaker(allCharacterMovable.Key.PlayerId, targetValue, _voiceOcclusionConfiguration.DistanceParameterName);
				}
			}
		}

		private void ProcessDistanceBasedEffects()
		{
			if (_playerMovableModel.LocalMovable == null || _playerMovableModel.LocalMovable.CameraPositionTransform == null)
			{
				return;
			}
			Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (_multiplayerModel.NetworkRunner.LocalPlayer == allCharacterMovable.Key)
				{
					continue;
				}
				Vector3 offset = allCharacterMovable.Value.CameraPositionTransform.position - position;
				float weightedDistance = GetWeightedDistance(offset);
				if (_playerStateService.IsPlayerDead(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
				{
					continue;
				}
				if (_voiceOcclusionConfiguration.MinDistanceForEffects < weightedDistance)
				{
					float normalizedDistance = Mathf.Clamp01((weightedDistance - _voiceOcclusionConfiguration.MinDistanceForEffects) / (_voiceOcclusionConfiguration.MaxDistanceForEffects - _voiceOcclusionConfiguration.MinDistanceForEffects));
					normalizedDistance = ApplyVerticalFalloff(normalizedDistance, offset);
					if (_voiceService.TryGetEffectValueForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.DelayParameterName, out var value))
					{
						float b = Mathf.Lerp(_voiceOcclusionConfiguration.MinDelay, _voiceOcclusionConfiguration.MaxDelay, normalizedDistance);
						float targetValue = Mathf.Lerp(value, b, Time.deltaTime * 5f);
						_voiceService.ProcessEffectForSpeaker(allCharacterMovable.Key.PlayerId, targetValue, _voiceOcclusionConfiguration.DelayParameterName);
					}
					if (_voiceService.TryGetEffectValueForSpeaker(allCharacterMovable.Key.PlayerId, _voiceOcclusionConfiguration.ReverbParameterName, out var value2))
					{
						float b2 = Mathf.Lerp(_voiceOcclusionConfiguration.MinReverb, _voiceOcclusionConfiguration.MaxReverb, normalizedDistance);
						float targetValue2 = Mathf.Lerp(value2, b2, Time.deltaTime * 5f);
						_voiceService.ProcessEffectForSpeaker(allCharacterMovable.Key.PlayerId, targetValue2, _voiceOcclusionConfiguration.ReverbParameterName);
					}
				}
				else
				{
					ResetDistanceBasedEffects(allCharacterMovable.Key.PlayerId);
				}
			}
		}

		private void ResetDistanceBasedEffects(int playerId)
		{
			if (_voiceService.TryGetEffectValueForSpeaker(playerId, _voiceOcclusionConfiguration.DelayParameterName, out var value))
			{
				float targetValue = Mathf.Lerp(value, _voiceOcclusionConfiguration.MinDelay, Time.deltaTime * 5f);
				_voiceService.ProcessEffectForSpeaker(playerId, targetValue, _voiceOcclusionConfiguration.DelayParameterName);
			}
			if (_voiceService.TryGetEffectValueForSpeaker(playerId, _voiceOcclusionConfiguration.ReverbParameterName, out var value2))
			{
				float targetValue2 = Mathf.Lerp(value2, _voiceOcclusionConfiguration.MinReverb, Time.deltaTime * 5f);
				_voiceService.ProcessEffectForSpeaker(playerId, targetValue2, _voiceOcclusionConfiguration.ReverbParameterName);
			}
		}
	}
}
