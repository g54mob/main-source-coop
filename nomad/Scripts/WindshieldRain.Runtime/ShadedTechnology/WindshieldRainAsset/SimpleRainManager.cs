using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[AddComponentMenu("Windshield Rain Asset/Simple Rain Manager")]
	public class SimpleRainManager : MonoBehaviour
	{
		public float m_addVelocityFactor = 0.5f;

		public float m_accelerationScale = 1f;

		public Material[] m_rainMaterials;

		public Vector3 m_gravityVector = new Vector3(0f, -9.8f, 0f);

		public float m_timeMod = 4f;

		private const float _accelerationScaleMultiplier = 0.01f;

		private const int rain_layers_count = 4;

		private Matrix4x4 _transformMatrix;

		private Vector3 _currentAcceleration;

		private Vector3 _lastPosition;

		private Vector3 _lastVelocity = Vector3.zero;

		[Header("Triplanar Rotation")]
		public Vector3 m_triplanarFacesRotation = Vector3.zero;

		public bool m_SetAccelerationForcibly;

		public Vector3 m_ForcedAcceleration;

		private int index = -1;

		public void UpdateTriplanarRotation()
		{
			_transformMatrix = Matrix4x4.TRS(base.transform.position, Quaternion.Euler(-m_triplanarFacesRotation), base.transform.lossyScale);
			Material[] rainMaterials = m_rainMaterials;
			for (int i = 0; i < rainMaterials.Length; i++)
			{
				rainMaterials[i].SetMatrix("_TransformMatrix", _transformMatrix);
			}
		}

		private void Start()
		{
			_lastPosition = base.transform.position;
			_currentAcceleration = m_gravityVector;
			Vector3 vector = base.transform.worldToLocalMatrix.MultiplyVector(_currentAcceleration);
			_transformMatrix = Matrix4x4.TRS(base.transform.position, Quaternion.Euler(-m_triplanarFacesRotation), base.transform.lossyScale);
			vector = _transformMatrix.MultiplyVector(vector);
			Material[] rainMaterials = m_rainMaterials;
			foreach (Material obj in rainMaterials)
			{
				obj.SetVector("_RainDirection_0", vector);
				obj.SetVector("_RainDirection_1", vector);
				obj.SetVector("_RainDirection_2", vector);
				obj.SetVector("_RainDirection_3", vector);
				obj.SetMatrix("_TransformMatrix", _transformMatrix);
				obj.SetFloat("_ModTime", m_timeMod);
			}
		}

		private void CalculateCurrentPosVelAcc()
		{
			Vector3 vector = (base.transform.position - _lastPosition) / Time.fixedDeltaTime;
			_lastPosition = base.transform.position;
			Vector3 vector2 = -((vector - _lastVelocity) / Time.fixedDeltaTime);
			_lastVelocity = vector;
			_currentAcceleration = (m_gravityVector + vector2) * m_accelerationScale * 0.01f;
		}

		private void FixedUpdate()
		{
			CalculateCurrentPosVelAcc();
			int num = (int)(Time.timeSinceLevelLoad % m_timeMod / (m_timeMod / 4f));
			if (index < 0)
			{
				index = num;
			}
			if (index == num)
			{
				Vector3 currentAcceleration = _currentAcceleration;
				if (m_SetAccelerationForcibly)
				{
					currentAcceleration = base.transform.worldToLocalMatrix.MultiplyVector(m_ForcedAcceleration);
					currentAcceleration = _transformMatrix.MultiplyVector(currentAcceleration);
				}
				else
				{
					currentAcceleration = base.transform.worldToLocalMatrix.MultiplyVector(currentAcceleration - m_addVelocityFactor * _lastVelocity * m_accelerationScale * 0.01f);
					currentAcceleration = _transformMatrix.MultiplyVector(currentAcceleration);
				}
				Material[] rainMaterials = m_rainMaterials;
				foreach (Material obj in rainMaterials)
				{
					obj.SetVector("_RainDirection_" + index, currentAcceleration);
					obj.SetMatrix("_TransformMatrix", _transformMatrix);
				}
				index = (index + 1) % 4;
			}
		}
	}
}
