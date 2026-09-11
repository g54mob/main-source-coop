using UnityEngine;

public class ShakeSound : MonoBehaviour
{
	public float minDist = 0.15f;

	public SFX_Instance shakeSound;

	private bool shake;

	private Vector3 prevPos;

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, prevPos) > minDist)
		{
			if (!shake)
			{
				shakeSound.Play(base.transform.position);
			}
			shake = true;
		}
		if (Vector3.Distance(base.transform.position, prevPos) < minDist)
		{
			if (shake)
			{
				shakeSound.Play(base.transform.position);
			}
			shake = false;
		}
		prevPos = base.transform.position;
	}
}
