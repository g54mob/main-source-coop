using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintDebug")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Debug")]
	public class CwPaintDebug : MonoBehaviour, IHitPoint, IHit, IHitLine, IHitTriangle, IHitQuad
	{
		[SerializeField]
		private Color color = Color.white;

		[SerializeField]
		private float duration = 0.05f;

		[SerializeField]
		private float size = 0.05f;

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
			}
		}

		public float Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			Color tint = GetColor(preview, pressure, color);
			Vector3 end = position + rotation * new Vector3(0f, 0f, 0f - size);
			DrawArrow(position, rotation, tint);
			Debug.DrawLine(position, end, tint, duration);
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			Color tint = GetColor(preview, pressure, color);
			DrawArrow(endPosition, rotation, tint);
			Debug.DrawLine(position, endPosition, tint, duration);
		}

		public void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			Color tint = GetColor(preview, pressure, color);
			DrawArrow(positionA, rotation, tint);
			DrawArrow(positionB, rotation, tint);
			DrawArrow(positionC, rotation, tint);
			Debug.DrawLine(positionA, positionB, tint, duration);
			Debug.DrawLine(positionB, positionC, tint, duration);
			Debug.DrawLine(positionC, positionA, tint, duration);
		}

		public void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			Color tint = GetColor(preview, pressure, color);
			DrawArrow(endPosition, rotation, tint);
			DrawArrow(endPosition2, rotation, tint);
			Debug.DrawLine(position, endPosition, tint, duration);
			Debug.DrawLine(position2, endPosition2, tint, duration);
			Debug.DrawLine(position, position2, tint, duration);
			Debug.DrawLine(endPosition, endPosition2, tint, duration);
		}

		private Color GetColor(bool preview, float pressure, Color color)
		{
			if (preview)
			{
				color.a *= 0.5f;
			}
			color.a *= pressure * 0.75f + 0.25f;
			return color;
		}

		private void DrawArrow(Vector3 position, Quaternion rotation, Color tint)
		{
			Vector3 vector = position + rotation * new Vector3(0f - size, 0f - size);
			Vector3 vector2 = position + rotation * new Vector3(0f - size, size);
			Vector3 vector3 = position + rotation * new Vector3(size, size);
			Vector3 vector4 = position + rotation * new Vector3(size, 0f - size);
			Debug.DrawLine(vector, vector2, tint, duration);
			Debug.DrawLine(vector2, vector3, tint, duration);
			Debug.DrawLine(vector3, vector4, tint, duration);
			Debug.DrawLine(vector4, vector, tint, duration);
			Vector3 start = position + rotation * new Vector3(0f, 0f, size);
			Debug.DrawLine(start, vector, tint, duration);
			Debug.DrawLine(start, vector2, tint, duration);
			Debug.DrawLine(start, vector3, tint, duration);
			Debug.DrawLine(start, vector4, tint, duration);
		}
	}
}
