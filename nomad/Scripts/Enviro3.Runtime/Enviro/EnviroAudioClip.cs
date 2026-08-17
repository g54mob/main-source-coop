using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Enviro
{
	[Serializable]
	public class EnviroAudioClip
	{
		public enum PlayBackType
		{
			Always = 0,
			BasedOnSun = 1,
			BasedOnMoon = 2
		}

		public bool showEditor;

		public string name;

		public AudioClip audioClip;

		public AudioMixerGroup audioMixerGroup;

		public PlayBackType playBackType;

		public AudioSource myAudioSource;

		public bool loop;

		public float volume;

		public AnimationCurve volumeCurve = new AnimationCurve();

		public float maxVolume = 1f;
	}
}
