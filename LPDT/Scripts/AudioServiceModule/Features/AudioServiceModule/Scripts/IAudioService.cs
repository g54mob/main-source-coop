using FMOD.Studio;
using FMODUnity;

namespace Features.AudioServiceModule.Scripts
{
	public interface IAudioService
	{
		void PlayOneShot(EventReference eventReference, ISoundSource soundSource = null);

		void PlayOneShotAttached(EventReference eventReference, ITransformBasedSoundSource transformBasedSoundSource);

		EventInstance CreateInstance(EventReference eventReference);

		void StartInstanceWith3DAttributes(EventInstance soundInstance, ISoundSource soundSource);

		void StartSnapshotInstance(EventInstance snapshotInstance);

		void StopInstance(EventInstance soundInstance, FMOD.Studio.STOP_MODE stopMode);

		void ReleaseInstance(EventInstance soundInstance);
	}
}
