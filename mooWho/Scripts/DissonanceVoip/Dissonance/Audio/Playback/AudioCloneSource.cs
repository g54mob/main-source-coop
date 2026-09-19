using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dissonance.Audio.Playback
{
	public class AudioCloneSource : MonoBehaviour, IAudioOutputSubscriber
	{
		private readonly List<AudioCloneSink> _sinks = new List<AudioCloneSink>();

		void IAudioOutputSubscriber.OnAudioPlayback(ArraySegment<float> data, bool complete)
		{
			lock (_sinks)
			{
				foreach (AudioCloneSink sink in _sinks)
				{
					sink.OnAudioPlayback(data, complete);
				}
			}
		}

		public void Subscribe(AudioCloneSink sink)
		{
			lock (_sinks)
			{
				_sinks.Add(sink);
			}
		}

		public void Unsubscribe(AudioCloneSink sink)
		{
			lock (_sinks)
			{
				_sinks.Remove(sink);
			}
		}
	}
}
