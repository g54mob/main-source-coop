using UnityEngine;

public class AngularVelocitySound : MonoBehaviour
{
	private Rigidbody rb;

	private AudioSource audio;

	public float vel;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if ((bool)rb && (bool)audio)
		{
			vel = Mathf.Abs(rb.angularVelocity.x) + Mathf.Abs(rb.angularVelocity.y) + Mathf.Abs(rb.angularVelocity.z);
			if (vel > 0.5f)
			{
				audio.enabled = true;
				audio.pitch = Mathf.Lerp(audio.pitch, vel, 5f * Time.deltaTime);
			}
			if (vel <= 0.5f)
			{
				audio.enabled = false;
			}
		}
	}
}
