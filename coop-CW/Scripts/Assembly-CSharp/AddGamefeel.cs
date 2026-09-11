using UnityEngine;

public class AddGamefeel : MonoBehaviour
{
	public bool playOnStart;

	public float perlinAmount = 1f;

	public float perlinDuration = 1f;

	public float scale = 15f;

	public float range = 50f;

	public void Start()
	{
		if (playOnStart)
		{
			AddPerlin();
		}
	}

	public void AddPerlin()
	{
		GamefeelHandler.instance.perlin.AddShake(base.transform.position, perlinAmount, perlinDuration, scale, range);
	}
}
