using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public struct EmitPoint
	{
		public Vector4 position;

		public Vector4 direction;

		public Vector4 velocity;

		public Color color;

		public EmitPoint(Vector3 position, Vector3 direction)
		{
			this.position = position;
			this.direction = direction;
			velocity = Vector4.zero;
			color = Color.white;
		}

		public EmitPoint(Vector3 position, Vector3 direction, Color color)
		{
			this.position = position;
			this.direction = direction;
			velocity = Vector4.zero;
			this.color = color;
		}

		public EmitPoint GetTransformed(Matrix4x4 transform, Matrix4x4 prevTransform, Color multiplyColor, float deltaTime)
		{
			EmitPoint result = new EmitPoint(transform.MultiplyPoint3x4(position), transform.MultiplyVector(direction), color * multiplyColor);
			result.velocity = ((deltaTime > 0f) ? (((Vector3)result.position - prevTransform.MultiplyPoint3x4(position)) / deltaTime) : Vector3.zero);
			return result;
		}
	}
}
