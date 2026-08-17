using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
	[Serializable]
	public struct ParticleVariable
	{
		public TAG tag;

		public Color StartColor;
	}

	public List<ParticleVariable> particlesVariable = new List<ParticleVariable>();

	private ParticleSystem.MainModule main;

	private EffectPlayer effectPlayer;

	private new string tag;

	private void Start()
	{
		main = GetComponent<ParticleSystem>().main;
		effectPlayer = base.transform.parent.GetComponent<EffectPlayer>();
		tag = effectPlayer.Tag;
	}

	private void Update()
	{
		if (!(tag != effectPlayer.Tag))
		{
			return;
		}
		tag = effectPlayer.Tag;
		for (int i = 0; i < particlesVariable.Count; i++)
		{
			switch (particlesVariable[i].tag)
			{
			case TAG.SNOW:
				if (tag == "Snow")
				{
					main.startColor = particlesVariable[i].StartColor;
				}
				break;
			case TAG.ICE:
				if (tag == "Ice")
				{
					main.startColor = particlesVariable[i].StartColor;
				}
				break;
			case TAG.PLATFORM:
				if (tag == "Platform")
				{
					main.startColor = particlesVariable[i].StartColor;
				}
				break;
			case TAG.DIRT:
				if (tag == "Dirt")
				{
					main.startColor = particlesVariable[i].StartColor;
				}
				break;
			case TAG.MUD:
				if (tag == "Mud")
				{
					main.startColor = particlesVariable[i].StartColor;
				}
				break;
			}
		}
	}
}
