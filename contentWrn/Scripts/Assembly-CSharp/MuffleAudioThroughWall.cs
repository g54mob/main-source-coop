using UnityEngine;

public class MuffleAudioThroughWall : MonoBehaviour
{
	public LayerMask worldLayer;

	public float lerp = 20f;

	private AudioLowPassFilter lowPass;

	private RaycastHit hit;

	private void Start()
	{
		lowPass = GetComponent<AudioLowPassFilter>();
		lowPass.cutoffFrequency = 20000f;
	}

	private void Update()
	{
		if (!Camera.main)
		{
			lowPass.cutoffFrequency = Mathf.Lerp(lowPass.cutoffFrequency, 20000f, lerp * Time.deltaTime);
		}
		if ((bool)Camera.main)
		{
			if (Physics.Linecast(base.transform.position, Camera.main.transform.position, out hit, worldLayer))
			{
				lowPass.cutoffFrequency = Mathf.Lerp(lowPass.cutoffFrequency, 2500f, lerp * Time.deltaTime / (Vector3.Distance(base.transform.position, Camera.main.transform.position) + 1f));
			}
			else
			{
				lowPass.cutoffFrequency = Mathf.Lerp(lowPass.cutoffFrequency, 15000f, lerp * Time.deltaTime / (Vector3.Distance(base.transform.position, Camera.main.transform.position) + 1f));
			}
		}
	}
}
