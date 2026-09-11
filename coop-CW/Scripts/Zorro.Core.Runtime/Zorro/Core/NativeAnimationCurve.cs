using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public struct NativeAnimationCurve : IDisposable
	{
		private int m_resolution;

		private float m_startTime;

		private float m_endTime;

		[ReadOnly]
		public NativeArray<float> m_curveData;

		public NativeAnimationCurve(AnimationCurve curve, int resolution, Allocator allocator)
		{
			m_resolution = resolution;
			m_curveData = new NativeArray<float>(resolution, allocator);
			m_startTime = curve.GetStartTime();
			m_endTime = curve.GetEndTime();
			float num = math.abs(m_endTime - m_startTime) / (float)(resolution - 1);
			for (int i = 0; i < resolution; i++)
			{
				m_curveData[i] = curve.Evaluate(m_startTime + (float)i * num);
			}
		}

		public float EvaluateCurve(float time)
		{
			float num = math.saturate(Mathf.InverseLerp(m_startTime, m_endTime, time)) * (float)m_resolution;
			int num2 = Mathf.FloorToInt(num);
			if (num2 >= m_resolution - 2)
			{
				return m_curveData[m_resolution - 1];
			}
			float t = num - (float)num2;
			return math.lerp(m_curveData[num2], m_curveData[num2 + 1], t);
		}

		public void Dispose()
		{
			m_curveData.Dispose();
		}

		public JobHandle Dispose(JobHandle jobHandle)
		{
			return m_curveData.Dispose(jobHandle);
		}

		public float Length()
		{
			return m_endTime - m_startTime;
		}
	}
}
