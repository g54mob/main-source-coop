using System;
using FMODUnity;
using UnityEngine;

namespace Features.VoiceSpeakersModule.Scripts.MimicVoice
{
	public interface IMimicVoicePlaybackService
	{
		bool PlayRandomForPlayer(int playerId, Transform source, Func<Transform> listenerProvider, int seed, EventReference voiceEvent);

		bool TryGetNormalizedLoudness(Transform source, out float loudness);
	}
}
