using UnityEngine;
using UnityEngine.Audio;

namespace Ami.Extension
{
	public interface IAudioSourceProxy
	{
		float volume { get; set; }

		float pitch { get; set; }

		float time { get; set; }

		int timeSamples { get; set; }

		AudioMixerGroup outputAudioMixerGroup { get; set; }

		bool loop { get; set; }

		bool ignoreListenerVolume { get; set; }

		bool playOnAwake { get; set; }

		bool ignoreListenerPause { get; set; }

		AudioVelocityUpdateMode velocityUpdateMode { get; set; }

		float panStereo { get; set; }

		float spatialBlend { get; set; }

		bool spatialize { get; set; }

		bool spatializePostEffects { get; set; }

		float reverbZoneMix { get; set; }

		bool bypassEffects { get; set; }

		bool bypassListenerEffects { get; set; }

		bool bypassReverbZones { get; set; }

		float dopplerLevel { get; set; }

		float spread { get; set; }

		int priority { get; set; }

		bool mute { get; set; }

		float minDistance { get; set; }

		float maxDistance { get; set; }

		AudioRolloffMode rolloffMode { get; set; }

		AudioClip clip { get; set; }

		AnimationCurve GetCustomCurve(AudioSourceCurveType type);

		void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve);

		bool GetAmbisonicDecoderFloat(int index, out float value);

		bool SetAmbisonicDecoderFloat(int index, float value);

		bool GetSpatializerFloat(int index, out float value);

		bool SetSpatializerFloat(int index, float value);
	}
}
