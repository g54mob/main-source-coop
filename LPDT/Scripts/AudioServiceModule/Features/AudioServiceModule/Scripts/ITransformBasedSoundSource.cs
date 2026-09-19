using UnityEngine;

namespace Features.AudioServiceModule.Scripts
{
	public interface ITransformBasedSoundSource : ISoundSource
	{
		int ISoundSource.ID => SoundSourceTransform.GetInstanceID();

		Transform SoundSourceTransform { get; }
	}
}
