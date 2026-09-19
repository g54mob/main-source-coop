using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.VoiceSpeakersModule.Scripts.Data
{
	public class ActiveSpeakerModel : ISessionCleanup
	{
		private readonly Dictionary<int, VoiceActivityStatus> _speakersActiveStatus = new Dictionary<int, VoiceActivityStatus>();

		public IReadOnlyDictionary<int, VoiceActivityStatus> SpeakersActiveStatus => _speakersActiveStatus;

		public event Action<int, VoiceActivityStatus> OnPlayerSpeakerStateChanged;

		public event Action<int> OnPlayerSpeakerCountChanged;

		public void SetStatus(int speakerRef, VoiceActivityStatus isActive)
		{
			if (_speakersActiveStatus[speakerRef] != isActive)
			{
				_speakersActiveStatus[speakerRef] = isActive;
				this.OnPlayerSpeakerStateChanged?.Invoke(speakerRef, isActive);
			}
		}

		public void RegisterPlayer(int player)
		{
			if (_speakersActiveStatus.TryAdd(player, VoiceActivityStatus.None))
			{
				this.OnPlayerSpeakerCountChanged?.Invoke(_speakersActiveStatus.Count);
			}
		}

		public void UnregisterPlayer(int player)
		{
			_speakersActiveStatus.Remove(player);
			this.OnPlayerSpeakerCountChanged?.Invoke(_speakersActiveStatus.Count);
		}

		public void Cleanup()
		{
			_speakersActiveStatus.Clear();
			this.OnPlayerSpeakerStateChanged = null;
			this.OnPlayerSpeakerCountChanged = null;
		}
	}
}
