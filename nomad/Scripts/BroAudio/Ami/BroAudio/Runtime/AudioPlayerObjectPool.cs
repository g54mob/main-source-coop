using System.Collections.Generic;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public class AudioPlayerObjectPool : ObjectPool<AudioPlayer>
	{
		private readonly Transform _parent;

		private readonly List<AudioPlayer> _currentPlayers = new List<AudioPlayer>();

		public AudioPlayerObjectPool(AudioPlayer baseObject, Transform parent, int maxInternalPoolSize)
			: base(baseObject, maxInternalPoolSize)
		{
			_parent = parent;
		}

		public override AudioPlayer Extract()
		{
			AudioPlayer audioPlayer = base.Extract();
			audioPlayer.gameObject.SetActive(value: true);
			_currentPlayers.Add(audioPlayer);
			return audioPlayer;
		}

		public override void Recycle(AudioPlayer player)
		{
			RemoveFromCurrent(player);
			player.gameObject.SetActive(value: false);
			base.Recycle(player);
		}

		protected override AudioPlayer CreateObject()
		{
			return Object.Instantiate(BaseObject, _parent);
		}

		protected override void DestroyObject(AudioPlayer player)
		{
			Object.Destroy(player.gameObject);
		}

		private void RemoveFromCurrent(AudioPlayer player)
		{
			for (int num = _currentPlayers.Count - 1; num >= 0; num--)
			{
				if (_currentPlayers[num] == player)
				{
					_currentPlayers.RemoveAt(num);
				}
			}
		}

		public IReadOnlyList<AudioPlayer> GetCurrentAudioPlayers()
		{
			return _currentPlayers;
		}
	}
}
