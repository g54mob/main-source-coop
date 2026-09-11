using System;
using UnityEngine;

[Serializable]
public class SphereSampler
{
	public float radius = 25f;

	public Vector3 GetPosition(Vector3 center)
	{
		Vector3 vector = UnityEngine.Random.insideUnitSphere * radius;
		return center + vector;
	}
}
