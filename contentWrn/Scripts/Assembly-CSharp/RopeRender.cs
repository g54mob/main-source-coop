using System;
using UnityEngine;

[Serializable]
public class RopeRender
{
	public float wobble = 1f;

	public float scrollSpeed = 1f;

	public float scale = 0.3f;

	public AnimationCurve wobbleCurve;

	public AnimationCurve wobbleOverLineCurve;

	public void DisplayRope(Vector3 from, Vector3 to, float time, LineRenderer line)
	{
		line.enabled = true;
		float num = Mathf.Lerp(1f, 0f, time);
		for (int i = 0; i < line.positionCount; i++)
		{
			float num2 = (float)i / ((float)line.positionCount - 1f);
			Vector3 position = Vector3.Lerp(from, to, num2);
			float num3 = (float)i * scale;
			float num4 = time * scrollSpeed;
			float num5 = Mathf.Cos(num3 + num4);
			position += num5 * Vector3.up * num * wobbleCurve.Evaluate(time) * wobbleOverLineCurve.Evaluate(num2);
			line.SetPosition(i, position);
		}
	}

	internal void StopRend(LineRenderer line)
	{
		line.enabled = false;
	}
}
