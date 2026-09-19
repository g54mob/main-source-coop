using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AudioDevicesModule.Scripts;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.LevelModule.Scripts;
using Photon.Voice;
using Zenject;

namespace Features.SelfMicrophonePlayerModule
{
	public class FmodMicSelfMonitorService : IFmodMicSelfMonitor, IInitializable, IDisposable, ITickable, ILateTickable, ILevelCleanup, ISessionCleanup
	{
		private readonly FmodSharedMicCapture _sharedMicCapture;

		private readonly MicrophoneModel _microphoneModel;

		private readonly SelfMicrophoneMonitorConfiguration _configuration;

		private FMOD.System _coreSystem;

		private Channel _monitorChannel;

		private Bus _monitorBus;

		private bool _monitorBusLocked;

		private int _nativeRate;

		private uint _recordSoundLengthPcm;

		private uint _samplesRecorded;

		private uint _samplesPlayed;

		private uint _lastPlayPos;

		private uint _lastRecordPos;

		private uint _driftThreshold;

		private uint _desiredLatency;

		private int _actualLatency;

		private bool _monitoringEnabled;

		public bool IsMonitoringEnabled => _monitoringEnabled;

		public FmodMicSelfMonitorService(FmodSharedMicCapture sharedMicCapture, MicrophoneModel microphoneModel, SelfMicrophoneMonitorConfiguration configuration)
		{
			_sharedMicCapture = sharedMicCapture;
			_microphoneModel = microphoneModel;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_coreSystem = RuntimeManager.CoreSystem;
		}

		public void Dispose()
		{
			StopMonitoringChannel();
		}

		public void Tick()
		{
			UpdateMonitoringPlayback();
		}

		public void LateTick()
		{
			if (_monitoringEnabled && _sharedMicCapture != null && !_monitorChannel.hasHandle())
			{
				UpdateMonitoringPlayback();
			}
		}

		public void SetMonitoring(bool enabled)
		{
			if (_monitoringEnabled != enabled)
			{
				_monitoringEnabled = enabled;
				if (enabled)
				{
					ResetPlaybackState();
					TryStartSharedCapture();
				}
				else
				{
					StopMonitoringChannel();
				}
			}
		}

		private void TryStartSharedCapture()
		{
			int device = ResolveDriverId();
			int suggestedFrequency = 48000;
			ILogger logger = null;
			if (_microphoneModel?.PlayerRecorder != null)
			{
				suggestedFrequency = (int)_microphoneModel.PlayerRecorder.SamplingRate;
				logger = _microphoneModel.PlayerRecorder.Logger;
			}
			_sharedMicCapture.EnsureCapture(device, suggestedFrequency, logger);
		}

		private int ResolveDriverId()
		{
			if (_microphoneModel != null && _microphoneModel.CurrentMicrophoneId >= 0)
			{
				return _microphoneModel.CurrentMicrophoneId;
			}
			return _configuration.FallbackDriverId;
		}

		private void SyncCaptureFromShared()
		{
			_nativeRate = _sharedMicCapture.SamplingRate;
			_recordSoundLengthPcm = _sharedMicCapture.RecordSoundLengthPcm;
			if (_nativeRate > 0 && _recordSoundLengthPcm != 0 && _driftThreshold == 0)
			{
				uint num = (uint)(_nativeRate * _configuration.DriftMs) / 1000;
				_driftThreshold = ((num == 0) ? 1u : num);
				_desiredLatency = (uint)(_nativeRate * _configuration.LatencyMs) / 1000;
				if (_desiredLatency == 0)
				{
					_desiredLatency = 1u;
				}
				_actualLatency = (int)_desiredLatency;
			}
		}

		private void ResetPlaybackState()
		{
			_samplesRecorded = 0u;
			_samplesPlayed = 0u;
			_lastPlayPos = 0u;
			_lastRecordPos = 0u;
			_driftThreshold = 0u;
			_desiredLatency = 0u;
			_actualLatency = 0;
		}

		private void UpdateMonitoringPlayback()
		{
			if (!_monitoringEnabled || _sharedMicCapture == null)
			{
				return;
			}
			if (!_sharedMicCapture.IsActive)
			{
				TryStartSharedCapture();
				if (!_sharedMicCapture.IsActive)
				{
					return;
				}
			}
			SyncCaptureFromShared();
			Sound recordSound = _sharedMicCapture.RecordSound;
			if (!recordSound.hasHandle() || _coreSystem.getRecordPosition(ResolveDriverId(), out var position) != RESULT.OK)
			{
				return;
			}
			if (!_monitorChannel.hasHandle())
			{
				if (TryStartMonitoringPlayback(recordSound))
				{
					InitializePlayheadSync(position);
				}
				return;
			}
			uint num = ((position >= _lastRecordPos) ? (position - _lastRecordPos) : (position + _recordSoundLengthPcm - _lastRecordPos));
			_lastRecordPos = position;
			_samplesRecorded += num;
			_monitorChannel.getPosition(out var position2, TIMEUNIT.PCM);
			uint num2 = ((position2 >= _lastPlayPos) ? (position2 - _lastPlayPos) : (position2 + _recordSoundLengthPcm - _lastPlayPos));
			_lastPlayPos = position2;
			_samplesPlayed += num2;
			int num3 = (int)(_samplesRecorded - _samplesPlayed);
			_actualLatency = (int)(0.97f * (float)_actualLatency + 0.03f * (float)num3);
			int num4 = _nativeRate;
			if (_actualLatency < (int)(_desiredLatency - _driftThreshold))
			{
				num4 = _nativeRate - _nativeRate / 50;
			}
			else if (_actualLatency > (int)(_desiredLatency + _driftThreshold))
			{
				num4 = _nativeRate + _nativeRate / 50;
			}
			_monitorChannel.setFrequency(num4);
		}

		private void InitializePlayheadSync(uint recordPos)
		{
			uint num = _desiredLatency;
			if (num >= _recordSoundLengthPcm)
			{
				num = _recordSoundLengthPcm / 4;
			}
			uint position = ((recordPos >= num) ? (recordPos - num) : (_recordSoundLengthPcm - (num - recordPos)));
			_monitorChannel.setPosition(position, TIMEUNIT.PCM);
			_monitorChannel.getPosition(out _lastPlayPos, TIMEUNIT.PCM);
			_lastRecordPos = recordPos;
			_samplesRecorded = num;
			_samplesPlayed = 0u;
			_actualLatency = (int)num;
		}

		private bool TryStartMonitoringPlayback(Sound recSound)
		{
			if (!_monitorBus.isValid() && RuntimeManager.StudioSystem.getBus(_configuration.MonitorBusPath, out _monitorBus) != RESULT.OK)
			{
				return false;
			}
			if (!_monitorBusLocked)
			{
				if (_monitorBus.lockChannelGroup() != RESULT.OK)
				{
					return false;
				}
				_monitorBusLocked = true;
			}
			RuntimeManager.StudioSystem.flushCommands();
			if (_monitorBus.getChannelGroup(out var group) != RESULT.OK)
			{
				ReleaseMonitorBus();
				return false;
			}
			if (_coreSystem.playSound(recSound, group, paused: false, out _monitorChannel) != RESULT.OK)
			{
				ReleaseMonitorBus();
				return false;
			}
			return true;
		}

		private void ReleaseMonitorBus()
		{
			if (_monitorBusLocked && _monitorBus.isValid())
			{
				_monitorBus.unlockChannelGroup();
				_monitorBusLocked = false;
			}
		}

		private void StopMonitoringChannel()
		{
			if (_monitorChannel.hasHandle())
			{
				_monitorChannel.stop();
				_monitorChannel.clearHandle();
			}
			ReleaseMonitorBus();
		}

		public void Cleanup()
		{
			SetMonitoring(enabled: false);
		}
	}
}
