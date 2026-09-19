using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public class SoundOcclusionInstanceSignalSystem : IInitializable, IDisposable
	{
		private const int FRAMES_BETWEEN_TRIGGERS = 10;

		private const float MIN_AUDIBLE_DB = -40f;

		private readonly IGameUpdater _gameUpdater;

		private readonly EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		private readonly IEntitiesSoundOcclusionService _entitiesSoundOcclusionService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly EntitiesSoundOcclusionConfiguration _entitiesSoundOcclusionConfiguration;

		private readonly List<EventInstance> _stoppedInstances = new List<EventInstance>();

		private readonly Dictionary<IntPtr, float> _peakRmsByHandle = new Dictionary<IntPtr, float>();

		private readonly Dictionary<IntPtr, Vector3> _lastPositionByHandle = new Dictionary<IntPtr, Vector3>();

		private readonly Dictionary<IntPtr, GUID> _guidByHandle = new Dictionary<IntPtr, GUID>();

		private readonly Dictionary<IntPtr, string> _pathByHandle = new Dictionary<IntPtr, string>();

		private readonly Dictionary<IntPtr, ISoundSource> _sourceByHandle = new Dictionary<IntPtr, ISoundSource>();

		private int _framesSinceLastTrigger;

		public SoundOcclusionInstanceSignalSystem(IGameUpdater gameUpdater, EntitiesSoundOcclusionModel entitiesSoundOcclusionModel, IEntitiesSoundOcclusionService entitiesSoundOcclusionService, MultiplayerModel multiplayerModel, EntitiesSoundOcclusionConfiguration entitiesSoundOcclusionConfiguration)
		{
			_gameUpdater = gameUpdater;
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
			_entitiesSoundOcclusionService = entitiesSoundOcclusionService;
			_multiplayerModel = multiplayerModel;
			_entitiesSoundOcclusionConfiguration = entitiesSoundOcclusionConfiguration;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += CheckInstanceForSignaling;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= CheckInstanceForSignaling;
			_peakRmsByHandle.Clear();
			_lastPositionByHandle.Clear();
			_guidByHandle.Clear();
			_pathByHandle.Clear();
			_sourceByHandle.Clear();
		}

		private void CheckInstanceForSignaling()
		{
			_framesSinceLastTrigger++;
			bool flag = _framesSinceLastTrigger >= 10;
			if (flag)
			{
				_framesSinceLastTrigger = 0;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			bool flag2 = networkRunner != null && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient;
			_stoppedInstances.Clear();
			foreach (KeyValuePair<EventInstance, ISoundSource> trackedSound in _entitiesSoundOcclusionModel.TrackedSounds)
			{
				EventInstance key = trackedSound.Key;
				if (!key.isValid())
				{
					_stoppedInstances.Add(key);
					continue;
				}
				key.getPlaybackState(out var state);
				if (state == PLAYBACK_STATE.STOPPED)
				{
					_stoppedInstances.Add(key);
				}
				else
				{
					if (!flag2)
					{
						continue;
					}
					IntPtr handle = key.handle;
					if (TryGetPlayedLoudness(key, out var loudness))
					{
						_peakRmsByHandle.TryGetValue(handle, out var value);
						if (loudness > value)
						{
							_peakRmsByHandle[handle] = loudness;
						}
					}
					if (key.get3DAttributes(out var attributes) == RESULT.OK)
					{
						_lastPositionByHandle[handle] = new Vector3(attributes.position.x, attributes.position.y, attributes.position.z);
					}
					if ((!_guidByHandle.ContainsKey(handle) || !_pathByHandle.ContainsKey(handle)) && key.getDescription(out var description) == RESULT.OK)
					{
						if (!_guidByHandle.ContainsKey(handle) && description.getID(out var id) == RESULT.OK)
						{
							_guidByHandle[handle] = id;
						}
						if (!_pathByHandle.ContainsKey(handle) && description.getPath(out var path) == RESULT.OK)
						{
							_pathByHandle[handle] = path;
						}
					}
					_sourceByHandle[handle] = trackedSound.Value;
					if (flag)
					{
						EmitOcclusion(handle, resetPeak: true);
					}
				}
			}
			for (int i = 0; i < _stoppedInstances.Count; i++)
			{
				IntPtr handle2 = _stoppedInstances[i].handle;
				if (flag2)
				{
					EmitOcclusion(handle2, resetPeak: false);
				}
				_peakRmsByHandle.Remove(handle2);
				_lastPositionByHandle.Remove(handle2);
				_guidByHandle.Remove(handle2);
				_pathByHandle.Remove(handle2);
				_sourceByHandle.Remove(handle2);
				_entitiesSoundOcclusionService.StopTrackingSound(_stoppedInstances[i]);
			}
		}

		private void EmitOcclusion(IntPtr handle, bool resetPeak)
		{
			_peakRmsByHandle.TryGetValue(handle, out var value);
			if (resetPeak)
			{
				_peakRmsByHandle[handle] = 0f;
			}
			if (!_lastPositionByHandle.TryGetValue(handle, out var value2))
			{
				return;
			}
			_sourceByHandle.TryGetValue(handle, out var value3);
			_pathByHandle.TryGetValue(handle, out var value4);
			if (_guidByHandle.TryGetValue(handle, out var value5) && _entitiesSoundOcclusionConfiguration.TryGetAudibleDistance(value5, out var audibleDistance))
			{
				_entitiesSoundOcclusionService.TriggerOcclusionForEntities(value2, audibleDistance, value3, value4);
				return;
			}
			float num = RmsToDistance(value);
			if (!(num <= 0f))
			{
				_entitiesSoundOcclusionService.TriggerOcclusionForEntities(value2, num, value3, value4);
			}
		}

		private float RmsToDistance(float rms)
		{
			if (rms <= 0f)
			{
				return 0f;
			}
			return Mathf.Clamp01((20f * Mathf.Log10(rms) - -40f) / 40f) * _entitiesSoundOcclusionConfiguration.MaxHearingDistance;
		}

		private bool TryGetPlayedLoudness(EventInstance eventInstance, out float loudness)
		{
			loudness = 0f;
			if (eventInstance.getChannelGroup(out var group) != RESULT.OK)
			{
				return false;
			}
			if (group.getDSP(-3, out var dsp) != RESULT.OK)
			{
				return false;
			}
			dsp.setMeteringEnabled(inputEnabled: false, outputEnabled: true);
			if (dsp.getMeteringInfo(IntPtr.Zero, out var outputInfo) != RESULT.OK)
			{
				return false;
			}
			if (outputInfo.numchannels == 0)
			{
				return false;
			}
			float num = 0f;
			for (int i = 0; i < outputInfo.numchannels; i++)
			{
				num += outputInfo.rmslevel[i] * outputInfo.rmslevel[i];
			}
			float num2 = Mathf.Sqrt(num / (float)outputInfo.numchannels);
			eventInstance.getVolume(out var _, out var finalvolume);
			loudness = num2 * finalvolume;
			return true;
		}
	}
}
