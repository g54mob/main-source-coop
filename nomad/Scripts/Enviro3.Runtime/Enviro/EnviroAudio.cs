using System;
using System.Collections.Generic;

namespace Enviro
{
	[Serializable]
	public class EnviroAudio
	{
		public List<EnviroAudioClip> ambientClips = new List<EnviroAudioClip>();

		public List<EnviroAudioClip> weatherClips = new List<EnviroAudioClip>();

		public List<EnviroAudioClip> thunderClips = new List<EnviroAudioClip>();

		public float ambientMasterVolume = 1f;

		public float weatherMasterVolume = 1f;

		public float thunderMasterVolume = 1f;
	}
}
