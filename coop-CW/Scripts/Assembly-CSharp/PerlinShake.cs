using System.Collections.Generic;
using UnityEngine;

public class PerlinShake : MonoBehaviour
{
	public List<PerlinShakeInstance> shakes = new List<PerlinShakeInstance>();

	private void Update()
	{
		Vector2 zero = Vector2.zero;
		for (int num = shakes.Count - 1; num >= 0; num--)
		{
			zero.x += (Mathf.PerlinNoise(Time.time * shakes[num].scale, 0f) - 0.5f) * shakes[num].amount * (shakes[num].duration / shakes[num].startDuration);
			zero.y += (Mathf.PerlinNoise(0f, Time.time * shakes[num].scale) - 0.5f) * shakes[num].amount * (shakes[num].duration / shakes[num].startDuration);
			shakes[num].duration -= Time.deltaTime;
			if (shakes[num].duration < 0f)
			{
				shakes.RemoveAt(num);
			}
		}
		base.transform.localEulerAngles = zero;
	}

	public void AddShake(float amount = 1f, float duration = 0.2f, float scale = 15f)
	{
		PerlinShakeInstance perlinShakeInstance = new PerlinShakeInstance();
		perlinShakeInstance.amount = amount;
		perlinShakeInstance.duration = duration;
		perlinShakeInstance.startDuration = duration;
		perlinShakeInstance.scale = scale;
		shakes.Add(perlinShakeInstance);
	}

	public void AddShake(Vector3 pos, float amount = 1f, float duration = 0.2f, float scale = 15f, float range = 50f)
	{
		float num = Mathf.InverseLerp(range, 0f, Vector3.Distance(MainCamera.instance.transform.position, pos));
		if (!(num <= 0.001f))
		{
			AddShake(amount * num, duration * num, scale);
		}
	}
}
