using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Photon.Voice.Unity.FMOD;

namespace Features.VoiceSpeakersModule.Scripts.Data
{
	public class SpawnedVoiceModel : ISessionCleanup
	{
		private readonly Dictionary<int, SpeakerFMOD> _speakers = new Dictionary<int, SpeakerFMOD>();

		private readonly Dictionary<int, SpeakerFMOD> _speakersNon3d = new Dictionary<int, SpeakerFMOD>();

		public IReadOnlyDictionary<int, SpeakerFMOD> Speakers => _speakers;

		public IReadOnlyDictionary<int, SpeakerFMOD> SpeakersNon3d => _speakersNon3d;

		public event Action OnSpeakerRegistered;

		public event Action OnSpeakerNon3dRegistered;

		public event Action<SpeakerFMOD> OnSpeakerInstanceRegistered;

		public void RegisterSpeaker(int playerRef, SpeakerFMOD speakerFMOD)
		{
			_speakers[playerRef] = speakerFMOD;
			this.OnSpeakerRegistered?.Invoke();
			this.OnSpeakerInstanceRegistered?.Invoke(speakerFMOD);
		}

		public void RegisterNon3dSpeaker(int playerRef, SpeakerFMOD speakerFMOD)
		{
			_speakersNon3d[playerRef] = speakerFMOD;
			this.OnSpeakerNon3dRegistered?.Invoke();
			this.OnSpeakerInstanceRegistered?.Invoke(speakerFMOD);
		}

		public void UnregisterSpeaker(int statRef, SpeakerFMOD speakerFMOD)
		{
			if (!_speakers.TryGetValue(statRef, out var value) || !(value != speakerFMOD))
			{
				_speakers.Remove(statRef);
			}
		}

		public void UnregisterSpeakerNon3d(int statRef, SpeakerFMOD speakerFMOD)
		{
			if (!_speakersNon3d.TryGetValue(statRef, out var value) || !(value != speakerFMOD))
			{
				_speakersNon3d.Remove(statRef);
			}
		}

		public void Cleanup()
		{
			_speakers.Clear();
			_speakersNon3d.Clear();
			this.OnSpeakerRegistered = null;
			this.OnSpeakerNon3dRegistered = null;
			this.OnSpeakerInstanceRegistered = null;
		}
	}
}
