using System;
using UnityEngine;

[Serializable]
public class GF_Perlin : Gamefeel
{
	public float amount = 1f;

	public float duration = 0.2f;

	public float scale = 15f;

	public override void Apply(Vector3 position = default(Vector3), Vector3 direction = default(Vector3), float multiplier = 1f)
	{
		multiplier *= HelperFunctions.GetCameraDistanceMultiplier(position, range);
		GamefeelHandler.instance.perlin.AddShake(amount * multiplier, duration, scale);
	}
}
