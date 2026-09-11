using System;
using UnityEngine;

[Serializable]
public class GF_SpringShake : Gamefeel
{
	public SpringType springType = SpringType.Rotation;

	public float amount = 1f;

	public Vector3 shakeDirection = Vector3.right;

	public float spring = 15f;

	public float damper = 15f;

	public override void Apply(Vector3 position = default(Vector3), Vector3 direction = default(Vector3), float multiplier = 1f)
	{
		multiplier *= HelperFunctions.GetCameraDistanceMultiplier(position, range);
		if (springType == SpringType.Position)
		{
			GamefeelHandler.instance.spring.AddPositionShake(shakeDirection * amount * multiplier, spring, damper);
		}
		else
		{
			GamefeelHandler.instance.spring.AddRotationShake(shakeDirection * amount * multiplier, spring, damper);
		}
	}
}
