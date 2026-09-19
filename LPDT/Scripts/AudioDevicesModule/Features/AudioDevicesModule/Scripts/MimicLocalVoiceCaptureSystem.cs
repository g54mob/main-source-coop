using System;
using System.Collections.Concurrent;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts.MimicVoice;
using NetworkServices.NetworkEvents;
using Photon.Voice.Unity;
using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public sealed class MimicLocalVoiceCaptureSystem : IInitializable, IDisposable, ITickable, ISessionCleanup
	{
		private readonly struct MicFrame
		{
			public readonly short[] Samples;

			public readonly int SamplingRate;

			public readonly int Channels;

			public MicFrame(short[] samples, int samplingRate, int channels)
			{
				Samples = samples;
				SamplingRate = samplingRate;
				Channels = channels;
			}
		}

		private readonly FmodSharedMicCapture _sharedMicCapture;

		private readonly MicrophoneModel _microphoneModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly IMimicVoiceFrameSink _frameSink;

		private bool _isLocalPlayerJoined;

		private int _lastLocalPlayerId;

		private readonly ConcurrentQueue<MicFrame> _micFrames = new ConcurrentQueue<MicFrame>();

		private const int MAX_QUEUED_FRAMES = 256;

		private volatile bool _hasSignalledGap;

		public MimicLocalVoiceCaptureSystem(FmodSharedMicCapture sharedMicCapture, MicrophoneModel microphoneModel, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus, IMimicVoiceFrameSink frameSink)
		{
			_sharedMicCapture = sharedMicCapture;
			_microphoneModel = microphoneModel;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
			_frameSink = frameSink;
		}

		public void Initialize()
		{
			_sharedMicCapture.OnSamplesRead += CaptureLocalFrame;
			_eventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Subscribe<OnShutdownEvent>(OnShutdown);
		}

		public void Dispose()
		{
			_sharedMicCapture.OnSamplesRead -= CaptureLocalFrame;
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_eventBus.Unsubscribe<OnShutdownEvent>(OnShutdown);
			ClearQueuedFrames();
		}

		public void Cleanup()
		{
			ClearQueuedFrames();
		}

		public void Tick()
		{
			MicFrame result;
			while (_micFrames.TryDequeue(out result))
			{
				if (result.Samples == null)
				{
					if (_lastLocalPlayerId > 0)
					{
						_frameSink.EndCapture(_lastLocalPlayerId, "capture overflow");
					}
				}
				else
				{
					ProcessLocalFrame(result.Samples, result.SamplingRate, result.Channels);
				}
			}
		}

		private void ClearQueuedFrames()
		{
			MicFrame result;
			while (_micFrames.TryDequeue(out result))
			{
			}
			_hasSignalledGap = false;
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent eventData)
		{
			if (!(eventData.Player != eventData.Runner.LocalPlayer))
			{
				_isLocalPlayerJoined = true;
				_lastLocalPlayerId = eventData.Player.PlayerId;
			}
		}

		private void OnShutdown(OnShutdownEvent eventData)
		{
			ClearQueuedFrames();
			if (_lastLocalPlayerId > 0)
			{
				_frameSink.EndCapture(_lastLocalPlayerId, "shutdown");
			}
			_isLocalPlayerJoined = false;
			_lastLocalPlayerId = 0;
		}

		private void CaptureLocalFrame(short[] samples, int samplingRate, int channels)
		{
			if (samples == null || samples.Length == 0)
			{
				return;
			}
			if (_micFrames.Count >= 256)
			{
				if (!_hasSignalledGap)
				{
					_hasSignalledGap = true;
					_micFrames.Enqueue(new MicFrame(null, 0, 0));
				}
			}
			else
			{
				_hasSignalledGap = false;
				short[] array = new short[samples.Length];
				Array.Copy(samples, array, samples.Length);
				_micFrames.Enqueue(new MicFrame(array, samplingRate, channels));
			}
		}

		private void ProcessLocalFrame(short[] samples, int samplingRate, int channels)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (playerId > 0)
			{
				if (!_isLocalPlayerJoined)
				{
					_isLocalPlayerJoined = true;
					_lastLocalPlayerId = playerId;
				}
				Recorder playerRecorder = _microphoneModel.PlayerRecorder;
				if (playerRecorder == null || !playerRecorder.RecordingEnabled || !playerRecorder.TransmitEnabled)
				{
					_frameSink.EndCapture(playerId, "local transmit disabled");
					return;
				}
				_lastLocalPlayerId = playerId;
				_frameSink.CaptureShortFrame(playerId, samples, samplingRate, channels);
			}
		}
	}
}
