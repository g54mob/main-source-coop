using UnityEngine;

namespace Features.AudioServiceModule.Scripts
{
	public class GenericSoundSource : ISoundSource
	{
		public Vector3 SourcePosition { get; }

		public int ID { get; }

		public SoundSourceKind Kind { get; }

		public int AttributedPlayerId { get; }

		public GenericSoundSource(Vector3 sourcePosition, int id, SoundSourceKind kind = SoundSourceKind.Unknown, int attributedPlayerId = -1)
		{
			SourcePosition = sourcePosition;
			ID = id;
			Kind = kind;
			AttributedPlayerId = ((kind == SoundSourceKind.Player) ? attributedPlayerId : (-1));
		}
	}
}
