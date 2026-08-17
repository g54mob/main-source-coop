using UnityEngine;

namespace Cinemachine.Utility
{
	public class PositionPredictor
	{
		private Vector3 m_Position;

		private GaussianWindow1D_Vector3 m_Velocity = new GaussianWindow1D_Vector3(10f);

		private GaussianWindow1D_Vector3 m_Accel = new GaussianWindow1D_Vector3(10f);

		private float mLastVelAddedTime;

		private const float kSmoothingDefault = 10f;

		private float mSmoothing = 10f;

		public float Smoothing
		{
			get
			{
				return mSmoothing;
			}
			set
			{
				if (value != mSmoothing)
				{
					mSmoothing = value;
					int maxKernelRadius = Mathf.Max(10, Mathf.FloorToInt(value * 1.5f));
					m_Velocity = new GaussianWindow1D_Vector3(mSmoothing, maxKernelRadius);
					m_Accel = new GaussianWindow1D_Vector3(mSmoothing, maxKernelRadius);
				}
			}
		}

		public bool IsEmpty => m_Velocity.IsEmpty();

		public void ApplyTransformDelta(Vector3 positionDelta)
		{
			m_Position += positionDelta;
		}

		public void Reset()
		{
			m_Velocity.Reset();
			m_Accel.Reset();
		}

		public void AddPosition(Vector3 pos, float deltaTime, float lookaheadTime)
		{
			if (deltaTime < 0.0001f)
			{
				Reset();
			}
			else if (IsEmpty)
			{
				m_Velocity.AddValue(Vector3.zero);
			}
			else
			{
				Vector3 vector = (pos - m_Position) / deltaTime;
				if (vector.sqrMagnitude > 0.0001f)
				{
					Vector3 vector2 = m_Velocity.Value();
					float time = Time.time;
					if (vector.sqrMagnitude >= vector2.sqrMagnitude || Vector3.Angle(vector, vector2) > 10f || time > mLastVelAddedTime + lookaheadTime)
					{
						m_Velocity.AddValue(vector);
						m_Accel.AddValue(vector - vector2);
						mLastVelAddedTime = time;
					}
				}
			}
			m_Position = pos;
		}

		public Vector3 PredictPositionDelta(float lookaheadTime)
		{
			Vector3 obj = (m_Velocity.IsEmpty() ? Vector3.zero : m_Velocity.Value());
			Vector3 vector = (m_Accel.IsEmpty() ? Vector3.zero : m_Accel.Value());
			return obj * lookaheadTime + vector * lookaheadTime * lookaheadTime * 0.5f;
		}

		public Vector3 PredictPosition(float lookaheadTime)
		{
			return m_Position + PredictPositionDelta(lookaheadTime);
		}
	}
}
