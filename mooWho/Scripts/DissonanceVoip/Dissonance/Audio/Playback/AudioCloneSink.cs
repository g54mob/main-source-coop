using System;
using UnityEngine;

namespace Dissonance.Audio.Playback
{
	public class AudioCloneSink : MonoBehaviour
	{
		private static readonly Log Log = Logs.Create(LogCategory.Playback, "Audio Clone Sink");

		private AudioSource _audioSource;

		public bool AutoPlay = true;

		public AudioCloneSource Source;

		private AudioCloneSource _subscribedTo;

		private readonly float[] _audioBuffer = new float[48000];

		private bool _complete = true;

		private void Awake()
		{
			if (!_audioSource)
			{
				_audioSource = GetComponent<AudioSource>();
			}
		}

		private void OnEnable()
		{
			if (!_audioSource)
			{
				Log.Error("AudioSource component is missing. AudioCloneSink requires an AudioSource on the same GameObject.");
				return;
			}
			_audioSource.Stop();
			if (_audioSource.spatialize)
			{
				_audioSource.spatialize = false;
			}
			_audioSource.clip = AudioClip.Create("Flatline", 4096, 1, AudioSettings.outputSampleRate, stream: false, delegate(float[] buf)
			{
				for (int i = 0; i < buf.Length; i++)
				{
					buf[i] = 1f;
				}
			});
			_audioSource.loop = true;
			_audioSource.pitch = 1f;
			_audioSource.dopplerLevel = 0f;
			if (AutoPlay)
			{
				_audioSource.Play();
			}
		}

		private void OnDisable()
		{
			Unsubscribe();
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void Update()
		{
			if (_subscribedTo != Source)
			{
				Subscribe(Source);
			}
		}

		private void Subscribe(AudioCloneSource source)
		{
			Unsubscribe();
			if ((bool)source)
			{
				source.Subscribe(this);
				_subscribedTo = source;
			}
		}

		private void Unsubscribe()
		{
			if ((bool)_subscribedTo)
			{
				_subscribedTo.Unsubscribe(this);
				_subscribedTo = null;
			}
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_complete)
			{
				Array.Clear(data, 0, data.Length);
			}
			else
			{
				SamplePlaybackComponent.StretchAudioChannels(data, channels, _audioBuffer);
			}
		}

		internal void OnAudioPlayback(ArraySegment<float> data, bool complete)
		{
			if (data.Count > _audioBuffer.Length)
			{
				Log.Error($"Audio data too large: {data.Count} > {_audioBuffer.Length}");
				Array.Clear(_audioBuffer, 0, _audioBuffer.Length);
			}
			else
			{
				data.CopyTo(_audioBuffer);
				_complete = complete;
			}
		}
	}
}
