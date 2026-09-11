using UnityEngine;

public static class Vector3Extensions
{
	public static Vector2 XZ(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	public static Vector3 Flat(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}
}
