using System;
using UnityEngine;

[Serializable]
public class BoxSampler
{
	public Vector3 size = new Vector3(25f, 25f, 25f);

	public Vector3 GetPosition(Vector3 center)
	{
		Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - size.x, size.x) * 0.5f, UnityEngine.Random.Range(0f - size.y, size.y) * 0.5f, UnityEngine.Random.Range(0f - size.z, size.z) * 0.5f);
		return center + vector;
	}
}
