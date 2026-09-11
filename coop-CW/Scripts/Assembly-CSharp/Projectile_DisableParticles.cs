using System;
using UnityEngine;

public class Projectile_DisableParticles : MonoBehaviour
{
	private ParticleSystem[] parts;

	private void Awake()
	{
		Projectile component = GetComponent<Projectile>();
		component.postHitAction = (Action<RaycastHit>)Delegate.Combine(component.postHitAction, new Action<RaycastHit>(PostHit));
		parts = base.transform.GetComponentsInChildren<ParticleSystem>();
	}

	private void PostHit(RaycastHit hit)
	{
		for (int i = 0; i < parts.Length; i++)
		{
			parts[i].Stop();
		}
	}
}
