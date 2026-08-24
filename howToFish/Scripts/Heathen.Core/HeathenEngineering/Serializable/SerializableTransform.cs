using System;
using Unity.Mathematics;
using UnityEngine;

namespace HeathenEngineering.Serializable
{
	[Serializable]
	public class SerializableTransform
	{
		public float3 position;

		public quaternion rotation;

		public float3 localScale;

		public SerializableTransform()
		{
			position = default(float3);
			rotation = default(quaternion);
			localScale = new float3(1f, 1f, 1f);
		}

		public SerializableTransform(float3 position, quaternion rotation, float3 localScale)
		{
			this.position = position;
			this.rotation = rotation;
			this.localScale = localScale;
		}

		public SerializableTransform(Transform transform)
		{
			position = transform.position;
			rotation = transform.rotation;
			localScale = transform.localScale;
		}

		public SerializableTransform(Vector3 position, Quaternion rotation, Vector3 localScale)
		{
			this.position = position;
			this.rotation = rotation;
			this.localScale = localScale;
		}

		public void SetTransform(Transform transform)
		{
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = localScale;
		}

		public static implicit operator SerializableTransform(Transform value)
		{
			return new SerializableTransform
			{
				position = value.position,
				rotation = value.rotation,
				localScale = value.localScale
			};
		}
	}
}
