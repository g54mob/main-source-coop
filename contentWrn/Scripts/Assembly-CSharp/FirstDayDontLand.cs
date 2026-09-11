using UnityEngine;

public class FirstDayDontLand : MonoBehaviour
{
	public GameObject evening;

	private void Update()
	{
		if ((bool)evening && !evening.activeSelf && (bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().enabled = false;
		}
	}
}
