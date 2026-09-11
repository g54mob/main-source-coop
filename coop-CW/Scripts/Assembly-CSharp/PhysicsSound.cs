using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
	public SFX_Instance[] impactSounds;

	private float counter;

	private void Update()
	{
		counter += Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!(counter < 0.5f) && !(collision.relativeVelocity.sqrMagnitude < 25f) && !collision.collider.GetComponentInParent<Player>())
		{
			counter = 0f;
			for (int i = 0; i < impactSounds.Length; i++)
			{
				impactSounds[i].Play(base.transform.position);
			}
		}
	}
}
