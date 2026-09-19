using UnityEngine;

namespace Features.AudioServiceModule.Scripts
{
	public class GenericTransformBasedSoundSource : ITransformBasedSoundSource, ISoundSource
	{
		public Transform SoundSourceTransform { get; }

		public Vector3 SourcePosition
		{
			get
			{
				if (!(SoundSourceTransform != null))
				{
					return Vector3.zero;
				}
				return SoundSourceTransform.position;
			}
		}

		public SoundSourceKind Kind => SoundSourceKind.Unknown;

		public int AttributedPlayerId => -1;

		public GenericTransformBasedSoundSource(Transform soundSourceTransform)
		{
			SoundSourceTransform = soundSourceTransform;
		}
	}
}
