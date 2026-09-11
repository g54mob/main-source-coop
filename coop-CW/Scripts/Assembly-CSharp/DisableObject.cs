using UnityEngine;

public class DisableObject : MonoBehaviour
{
	public bool on = true;

	public GameObject stop;

	private void Update()
	{
		if ((bool)stop && !on)
		{
			stop.SetActive(value: false);
		}
	}
}
