using UnityEngine;

public class AggroParticle : MonoBehaviour
{
	private Bot_Chaser bot;

	private ParticleSystem part;

	private void Start()
	{
		bot = base.transform.root.GetComponentInChildren<Bot_Chaser>();
		part = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (bot.aggroState)
		{
			if (!part.isPlaying)
			{
				part.Play();
			}
		}
		else if (part.isPlaying)
		{
			part.Stop();
		}
	}
}
