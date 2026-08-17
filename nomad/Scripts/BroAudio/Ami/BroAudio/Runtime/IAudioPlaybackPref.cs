namespace Ami.BroAudio.Runtime
{
	public interface IAudioPlaybackPref
	{
		float Volume { get; }

		float Pitch { get; }

		EffectType EffectType { get; }
	}
}
