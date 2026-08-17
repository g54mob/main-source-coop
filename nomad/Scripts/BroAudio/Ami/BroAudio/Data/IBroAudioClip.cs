using UnityEngine;

namespace Ami.BroAudio.Data
{
	public interface IBroAudioClip
	{
		bool IsSet { get; }

		float Volume { get; }

		float Delay { get; }

		float StartPosition { get; }

		float EndPosition { get; }

		float FadeIn { get; }

		float FadeOut { get; }

		AudioClip GetAudioClip();

		bool IsValid();
	}
}
