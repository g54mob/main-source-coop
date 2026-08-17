using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public AudioObjectManager[] audioObjects;

	public AudioMixerGroup fxGroup;

	private AudioSource source;

	private void Awake()
	{
		for (int i = 0; i < audioObjects.Length; i++)
		{
			Sound[] variables = audioObjects[i].variables;
			foreach (Sound sound in variables)
			{
				sound.source = base.gameObject.AddComponent<AudioSource>();
				sound.source.playOnAwake = false;
				sound.source.outputAudioMixerGroup = fxGroup;
				sound.source.clip = sound.clips[0];
				sound.source.pitch = sound.pitch;
				sound.source.volume = sound.volume;
			}
		}
	}

	public void PlaySound(Sound s)
	{
		if (s != null)
		{
			if (s.clips.Length > 1)
			{
				AudioClip clip = s.clips[Random.Range(0, s.clips.Length)];
				s.source.PlayOneShot(clip, 1f);
			}
			else if (s.source.loop)
			{
				s.source.Play();
			}
			else
			{
				s.source.PlayOneShot(s.source.clip, 1f);
			}
		}
	}
}
