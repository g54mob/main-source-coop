using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public struct NativeAnimationCurveArray : IDisposable
	{
		private int m_resolution;

		private int curveCount;

		[ReadOnly]
		public NativeArray<float> m_curveData;

		[ReadOnly]
		public NativeArray<float2> m_curveInfo;

		public NativeAnimationCurveArray(AnimationCurve[] curves, int resolution, Allocator allocator)
		{
			m_resolution = resolution;
			m_curveData = new NativeArray<float>(resolution * curves.Length, allocator);
			m_curveInfo = new NativeArray<float2>(curves.Length, allocator);
			for (int i = 0; i < curves.Length; i++)
			{
				float startTime = curves[i].GetStartTime();
				float endTime = curves[i].GetEndTime();
				m_curveInfo[i] = new float2(startTime, endTime);
				float num = (endTime - startTime) / (float)(resolution - 1);
				for (int j = 0; j < resolution; j++)
				{
					m_curveData[i * resolution + j] = curves[i].Evaluate(startTime + (float)j * num);
				}
			}
			curveCount = curves.Length;
		}

		public float EvaluateCurve(int curveIndex, float time)
		{
			float x = m_curveInfo[curveIndex].x;
			float y = m_curveInfo[curveIndex].y;
			float num = math.saturate(Mathf.InverseLerp(x, y, time)) * (float)m_resolution;
			int num2 = Mathf.FloorToInt(num);
			if (num2 >= m_resolution - 2)
			{
				return m_curveData[curveIndex * m_resolution + m_resolution - 1];
			}
			float t = num - (float)num2;
			return math.lerp(m_curveData[curveIndex * m_resolution + num2], m_curveData[curveIndex * m_resolution + num2 + 1], t);
		}

		public void Dispose()
		{
			m_curveData.Dispose();
			m_curveInfo.Dispose();
		}
	}
}
