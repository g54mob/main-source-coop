using System;
using UnityEngine;

public static class Vector2Extension
{
	public static Vector2 Rotate(this Vector2 v, float degrees)
	{
		float f = degrees * ((float)Math.PI / 180f);
		float num = Mathf.Sin(f);
		float num2 = Mathf.Cos(f);
		float x = v.x;
		float y = v.y;
		return new Vector2(num2 * x - num * y, num * x + num2 * y);
	}
}
