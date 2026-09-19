using UnityEngine;

namespace Features.AudioServiceModule.Scripts
{
	public interface ISoundSource
	{
		Vector3 SourcePosition { get; }

		int ID { get; }

		SoundSourceKind Kind { get; }

		int AttributedPlayerId { get; }
	}
}
