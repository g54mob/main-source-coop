using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
	public ParticleSystem[] particles;

	[SerializeField]
	private LayerMask groundlayer;

	private List<int> currentParticlesPlaying = new List<int>();

	private CharacterStateController characterStateController;

	private CharacterStates state;

	private new string tag;

	private AudioManager audioManager;

	public string Tag => tag;

	private void Start()
	{
		TryGetComponent<CharacterStateController>(out characterStateController);
		if (characterStateController != null)
		{
			state = characterStateController.GetCurrentState();
		}
		audioManager = Object.FindObjectOfType<AudioManager>();
	}

	private void Update()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.down, 2f, groundlayer);
		if ((bool)raycastHit2D)
		{
			tag = raycastHit2D.transform.tag;
		}
		if (characterStateController != null && state != characterStateController.GetCurrentState())
		{
			state = characterStateController.GetCurrentState();
			StopAllParticles();
		}
	}

	public void PlayParticle(int ParticleID)
	{
		if (ParticleID >= particles.Length)
		{
			return;
		}
		if (particles[ParticleID].main.loop)
		{
			for (int i = 0; i < currentParticlesPlaying.Count; i++)
			{
				if (currentParticlesPlaying[i] == ParticleID)
				{
					return;
				}
			}
			currentParticlesPlaying.Add(ParticleID);
		}
		particles[ParticleID].Play();
	}

	public void PlayNoVariableSound(AudioClip clip)
	{
		AudioSource component = GetComponent<AudioSource>();
		if (component != null)
		{
			component.PlayOneShot(clip);
		}
	}

	public void PlayTest(Sound t)
	{
	}

	public void PlaySound(AudioObjectManager audioObject)
	{
		Sound[] variables = audioObject.variables;
		foreach (Sound sound in variables)
		{
			if (sound.tag == TAG.NO_VARIABLE || CompareTag(sound.tag))
			{
				audioManager?.PlaySound(sound);
			}
		}
	}

	public void PlaySound(Object obj)
	{
		Sound[] variables = (obj as AudioObjectManager).variables;
		foreach (Sound sound in variables)
		{
			if (sound.tag == TAG.NO_VARIABLE || CompareTag(sound.tag))
			{
				audioManager?.PlaySound(sound);
			}
		}
	}

	private bool CompareTag(TAG t)
	{
		bool result = false;
		switch (t)
		{
		case TAG.SNOW:
			if (tag == "Snow")
			{
				result = true;
			}
			break;
		case TAG.ICE:
			if (tag == "Ice")
			{
				result = true;
			}
			break;
		case TAG.PLATFORM:
			if (tag == "Platform")
			{
				result = true;
			}
			break;
		case TAG.DIRT:
			if (tag == "Dirt")
			{
				result = true;
			}
			break;
		case TAG.MOVABLE:
			if (tag == "Movable")
			{
				result = true;
			}
			break;
		}
		return result;
	}

	private void StopAllParticles()
	{
		for (int i = 0; i < currentParticlesPlaying.Count; i++)
		{
			particles[currentParticlesPlaying[i]].Stop();
		}
		currentParticlesPlaying.Clear();
	}
}
