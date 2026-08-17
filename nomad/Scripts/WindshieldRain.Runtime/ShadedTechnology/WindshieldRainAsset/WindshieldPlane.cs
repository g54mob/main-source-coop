using System;
using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[Serializable]
	public class WindshieldPlane
	{
		[SerializeField]
		public float width = 1f;

		[SerializeField]
		public float height = 1f;

		[SerializeField]
		public WindshieldRain m_WindshieldRain;

		public WindshieldPlane(WindshieldRain rainScript)
		{
			m_WindshieldRain = rainScript;
		}

		public Vector2 WorldPosToWindshieldUV(Vector3 worldPos)
		{
			Vector3 vector = m_WindshieldRain.transform.InverseTransformPoint(worldPos);
			float x = (vector.x + width * 0.5f) / width;
			float y = (vector.y + height * 0.5f) / height;
			return new Vector2(x, y);
		}

		public Vector2 WorldPosToWindshieldPos(Vector3 worldPos)
		{
			Vector3 vector = m_WindshieldRain.transform.InverseTransformPoint(worldPos);
			float x = vector.x + width * 0.5f;
			float y = vector.y + height * 0.5f;
			return new Vector2(x, y);
		}
	}
}
