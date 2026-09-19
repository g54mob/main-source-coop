using System;
using FMOD.Studio;

namespace Features.AudioServiceModule.Scripts
{
	public class AudioModel
	{
		public event Action<EventInstance, ISoundSource> OnInstanceStarted;

		public event Action<EventInstance> OnSnapshotInstanceStarted;

		public event Action<EventInstance> OnInstanceStopped;

		internal event Action<EventInstance, ISoundSource> OnOneShotStarted;

		internal void RaiseInstanceStarted(EventInstance soundInstance, ISoundSource soundSource)
		{
			this.OnInstanceStarted?.Invoke(soundInstance, soundSource);
		}

		internal void RaiseSnapshotInstanceStarted(EventInstance soundInstance)
		{
			this.OnSnapshotInstanceStarted?.Invoke(soundInstance);
		}

		internal void RaiseInstanceStopped(EventInstance soundInstance)
		{
			this.OnInstanceStopped?.Invoke(soundInstance);
		}

		internal void RaiseOneShotStarted(EventInstance soundInstance, ISoundSource soundSource)
		{
			this.OnOneShotStarted?.Invoke(soundInstance, soundSource);
		}
	}
}
