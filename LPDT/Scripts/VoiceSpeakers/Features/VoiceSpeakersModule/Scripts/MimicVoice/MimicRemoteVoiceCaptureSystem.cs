using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.Unity.FMOD;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public sealed class MimicRemoteVoiceCaptureSystem : IInitializable, IDisposable, ITickable, ISessionCleanup
	{
		private sealed class SpeakerSubscription
		{
			public volatile bool IsPlaying;

			public volatile bool HasSignalledGap;

			public RemoteVoiceLink Link { get; }

			public int PlayerId { get; }

			public Action<FrameOut<float>> Handler { get; set; }

			public SpeakerSubscription(RemoteVoiceLink link, int playerId)
			{
				Link = link;
				PlayerId = playerId;
			}
		}

		private readonly struct DecodedFrame
		{
			public readonly int PlayerId;

			public readonly float[] Samples;

			public readonly int SamplingRate;

			public readonly int Channels;

			public DecodedFrame(int playerId, float[] samples, int samplingRate, int channels)
			{
				PlayerId = playerId;
				Samples = samples;
				SamplingRate = samplingRate;
				Channels = channels;
			}
		}

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMimicVoiceFrameSink _frameSink;

		private readonly Dictionary<Speaker, SpeakerSubscription> _subscriptions = new Dictionary<Speaker, SpeakerSubscription>();

		private readonly ConcurrentQueue<DecodedFrame> _decodedFrames = new ConcurrentQueue<DecodedFrame>();

		private const int MAX_QUEUED_FRAMES = 256;

		public MimicRemoteVoiceCaptureSystem(SpawnedVoiceModel spawnedVoiceModel, MultiplayerModel multiplayerModel, IMimicVoiceFrameSink frameSink)
		{
			_spawnedVoiceModel = spawnedVoiceModel;
			_multiplayerModel = multiplayerModel;
			_frameSink = frameSink;
		}

		public void Initialize()
		{
			RegisterCurrentSpeakers();
		}

		public void Tick()
		{
			RegisterCurrentSpeakers();
			UnregisterStaleSpeakers();
			RefreshPlaybackGates();
			DrainDecodedFrames();
		}

		public void Cleanup()
		{
			UnsubscribeAll();
		}

		public void Dispose()
		{
			UnsubscribeAll();
		}

		private void UnsubscribeAll()
		{
			foreach (KeyValuePair<Speaker, SpeakerSubscription> subscription in _subscriptions)
			{
				Unsubscribe(subscription.Value);
			}
			_subscriptions.Clear();
			DecodedFrame result;
			while (_decodedFrames.TryDequeue(out result))
			{
			}
		}

		private void RegisterCurrentSpeakers()
		{
			RegisterSpeakers(_spawnedVoiceModel.Speakers);
			RegisterSpeakers(_spawnedVoiceModel.SpeakersNon3d);
		}

		private void RegisterSpeakers(IReadOnlyDictionary<int, SpeakerFMOD> speakers)
		{
			foreach (KeyValuePair<int, SpeakerFMOD> speaker in speakers)
			{
				SpeakerFMOD value = speaker.Value;
				int key = speaker.Key;
				if (value == null || key <= 0 || IsLocalPlayer(key) || _subscriptions.ContainsKey(value))
				{
					continue;
				}
				RemoteVoiceLink remoteVoice = value.RemoteVoice;
				if (remoteVoice != null)
				{
					SpeakerSubscription subscription = new SpeakerSubscription(remoteVoice, key);
					subscription.IsPlaying = value.IsPlaying;
					subscription.Handler = delegate(FrameOut<float> frame)
					{
						CaptureRemoteFrame(subscription, frame);
					};
					remoteVoice.FloatFrameDecoded += subscription.Handler;
					_subscriptions[value] = subscription;
				}
			}
		}

		private void UnregisterStaleSpeakers()
		{
			List<Speaker> list = null;
			foreach (KeyValuePair<Speaker, SpeakerSubscription> subscription in _subscriptions)
			{
				SpeakerSubscription value = subscription.Value;
				if (!(subscription.Key != null) || !IsMappedTo(value.PlayerId, subscription.Key) || subscription.Key.RemoteVoice != value.Link)
				{
					if (list == null)
					{
						list = new List<Speaker>();
					}
					list.Add(subscription.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (Speaker item in list)
			{
				Unsubscribe(_subscriptions[item]);
				_subscriptions.Remove(item);
			}
		}

		private void RefreshPlaybackGates()
		{
			foreach (KeyValuePair<Speaker, SpeakerSubscription> subscription in _subscriptions)
			{
				subscription.Value.IsPlaying = subscription.Key != null && subscription.Key.IsPlaying;
			}
		}

		private void Unsubscribe(SpeakerSubscription subscription)
		{
			if (subscription.Link != null && subscription.Handler != null)
			{
				subscription.Link.FloatFrameDecoded -= subscription.Handler;
			}
		}

		private bool IsMappedTo(int playerId, Speaker speaker)
		{
			if (!IsMappedTo(_spawnedVoiceModel.Speakers, playerId, speaker))
			{
				return IsMappedTo(_spawnedVoiceModel.SpeakersNon3d, playerId, speaker);
			}
			return true;
		}

		private bool IsMappedTo(IReadOnlyDictionary<int, SpeakerFMOD> speakers, int playerId, Speaker speaker)
		{
			if (speakers.TryGetValue(playerId, out var value))
			{
				return value == speaker;
			}
			return false;
		}

		private bool IsLocalPlayer(int playerId)
		{
			if (_multiplayerModel.NetworkRunner != null)
			{
				return _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId;
			}
			return false;
		}

		private void CaptureRemoteFrame(SpeakerSubscription subscription, FrameOut<float> frame)
		{
			if (!subscription.IsPlaying)
			{
				return;
			}
			float[] array = frame?.Buf;
			if (array == null || array.Length == 0)
			{
				return;
			}
			if (_decodedFrames.Count >= 256)
			{
				if (!subscription.HasSignalledGap)
				{
					subscription.HasSignalledGap = true;
					_decodedFrames.Enqueue(new DecodedFrame(subscription.PlayerId, null, 0, 0));
				}
			}
			else
			{
				subscription.HasSignalledGap = false;
				float[] array2 = new float[array.Length];
				Array.Copy(array, array2, array.Length);
				VoiceInfo voiceInfo = subscription.Link.VoiceInfo;
				_decodedFrames.Enqueue(new DecodedFrame(subscription.PlayerId, array2, voiceInfo.SamplingRate, voiceInfo.Channels));
			}
		}

		private void DrainDecodedFrames()
		{
			DecodedFrame result;
			while (_decodedFrames.TryDequeue(out result))
			{
				if (result.Samples == null)
				{
					_frameSink.EndCapture(result.PlayerId, "capture overflow");
				}
				else
				{
					_frameSink.CaptureFloatFrame(result.PlayerId, result.Samples, result.SamplingRate, result.Channels);
				}
			}
		}
	}
}
