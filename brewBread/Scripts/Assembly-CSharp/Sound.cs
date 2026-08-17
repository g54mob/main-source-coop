using System;
using UnityEngine;

[Serializable]
public class Sound
{
	public TAG tag;

	public AudioClip[] clips;

	[Range(0f, 1f)]
	public float volume = 1f;

	[Range(0.1f, 3f)]
	public float pitch = 1f;

	[HideInInspector]
	public AudioSource source;
}
