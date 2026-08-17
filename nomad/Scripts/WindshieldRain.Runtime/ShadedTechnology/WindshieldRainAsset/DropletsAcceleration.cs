using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[AddComponentMenu("Windshield Rain Asset/DropletsAcceleration")]
	[RequireComponent(typeof(WindshieldRain))]
	public class DropletsAcceleration : MonoBehaviour
	{
		[HideInInspector]
		public WindshieldRain m_RainScript;

		public float m_AccelerationScale = 1f;

		public float m_AddVelocityFactor = 0.5f;

		public Vector3 m_GravityForce = new Vector3(0f, -1f, 0f);

		private Vector3 m_LastPosition;

		private Vector3 m_LastVelocity = Vector3.zero;

		private const float _accelerationScaleMultiplier = 0.01f;

		private void Start()
		{
			m_LastPosition = base.transform.position;
		}

		private void FixedUpdate()
		{
			Vector3 vector = (base.transform.position - m_LastPosition) / Time.fixedDeltaTime;
			m_LastPosition = base.transform.position;
			Vector3 vector2 = -((vector - m_LastVelocity) / Time.fixedDeltaTime);
			vector2 = (m_GravityForce + vector2 + m_AddVelocityFactor * -m_LastVelocity) * m_AccelerationScale * 0.01f;
			m_LastVelocity = vector;
			float y = Vector3.Dot(vector2, base.transform.up);
			float x = Vector3.Dot(vector2, base.transform.right);
			m_RainScript.Movement = new Vector2(x, y);
		}
	}
}
