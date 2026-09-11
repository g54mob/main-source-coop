using Unity.Mathematics;
using UnityEngine;

public class AudioToggler : MonoBehaviour
{
	private const float distance = 30f;

	public GameObject sfxHolder;

	private Bot bot;

	private void Start()
	{
		bot = GetComponentInChildren<Bot>();
	}

	private void Update()
	{
		if (!(bot == null))
		{
			Vector3 position = MainCamera.instance.transform.position;
			if (math.distancesq(bot.Center(), position) < 900f)
			{
				sfxHolder.SetActive(value: true);
			}
			else
			{
				sfxHolder.SetActive(value: false);
			}
		}
	}
}
