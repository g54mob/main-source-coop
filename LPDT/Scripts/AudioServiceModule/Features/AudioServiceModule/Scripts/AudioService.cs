using FMOD.Studio;
using FMODUnity;

namespace Features.AudioServiceModule.Scripts
{
	public class AudioService : IAudioService
	{
		private readonly AudioModel _audioModel;

		public AudioService(AudioModel audioModel)
		{
			_audioModel = audioModel;
		}

		public void PlayOneShot(EventReference eventReference, ISoundSource soundSource)
		{
			EventInstance soundInstance = RuntimeManager.CreateInstance(eventReference);
			if (soundSource != null)
			{
				soundInstance.set3DAttributes(soundSource.SourcePosition.To3DAttributes());
			}
			StartOneShot(soundInstance, soundSource);
		}

		public void PlayOneShotAttached(EventReference eventReference, ITransformBasedSoundSource transformBasedSoundSource)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
			RuntimeManager.AttachInstanceToGameObject(eventInstance, transformBasedSoundSource.SoundSourceTransform.gameObject);
			StartOneShot(eventInstance, transformBasedSoundSource);
		}

		public EventInstance CreateInstance(EventReference eventReference)
		{
			return RuntimeManager.CreateInstance(eventReference);
		}

		public void StartInstanceWith3DAttributes(EventInstance soundInstance, ISoundSource soundSource)
		{
			soundInstance.set3DAttributes(soundSource.SourcePosition.To3DAttributes());
			soundInstance.start();
			_audioModel.RaiseInstanceStarted(soundInstance, soundSource);
		}

		public void StartSnapshotInstance(EventInstance snapshotInstance)
		{
			snapshotInstance.start();
			_audioModel.RaiseSnapshotInstanceStarted(snapshotInstance);
		}

		public void StopInstance(EventInstance soundInstance, FMOD.Studio.STOP_MODE stopMode)
		{
			soundInstance.stop(stopMode);
			_audioModel.RaiseInstanceStopped(soundInstance);
		}

		public void ReleaseInstance(EventInstance soundInstance)
		{
			soundInstance.release();
			_audioModel.RaiseInstanceStopped(soundInstance);
		}

		private void StartOneShot(EventInstance soundInstance, ISoundSource soundSource)
		{
			soundInstance.start();
			_audioModel.RaiseInstanceStarted(soundInstance, soundSource);
			_audioModel.RaiseOneShotStarted(soundInstance, soundSource);
		}
	}
}
