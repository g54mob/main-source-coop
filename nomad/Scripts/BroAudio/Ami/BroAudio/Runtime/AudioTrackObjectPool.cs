using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.BroAudio.Runtime
{
	public class AudioTrackObjectPool : ObjectPool<AudioMixerGroup>
	{
		private AudioMixerGroup[] _audioMixerGroups;

		private int _usedTrackCount;

		private readonly bool _isDominator;

		public AudioTrackObjectPool(AudioMixerGroup[] audioMixerGroups, bool isDominator = false)
			: base((AudioMixerGroup)null, audioMixerGroups.Length)
		{
			_audioMixerGroups = audioMixerGroups;
			_isDominator = isDominator;
		}

		protected override AudioMixerGroup CreateObject()
		{
			if (_usedTrackCount >= _audioMixerGroups.Length)
			{
				if (_isDominator)
				{
					Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>You have used up all the [Dominator] tracks. If you need more tracks, please click the [Add Dominator Track] button in Tool/BroAudio/Preference.");
				}
				else
				{
					Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>You have reached the limit of BroAudio tracks count, which is way beyond the MaxRealVoices count. That means the sound will be inaudible, and also uncontrollable. For more infomation, please check the documentation");
				}
				return null;
			}
			AudioMixerGroup result = _audioMixerGroups[_usedTrackCount];
			_usedTrackCount++;
			return result;
		}

		protected override void DestroyObject(AudioMixerGroup track)
		{
		}
	}
}
