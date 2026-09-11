using System;
using UnityEngine;

namespace Tayx.Graphy.Fps
{
	public class G_FpsMonitor : MonoBehaviour
	{
		private short[] m_fpsSamples;

		private short[] m_fpsSamplesSorted;

		private short m_fpsSamplesCapacity = 1024;

		private short m_onePercentSamples = 10;

		private short m_zero1PercentSamples = 1;

		private short m_fpsSamplesCount;

		private short m_indexSample;

		private float m_unscaledDeltaTime;

		public short CurrentFPS { get; private set; }

		public short AverageFPS { get; private set; }

		public short OnePercentFPS { get; private set; }

		public short Zero1PercentFps { get; private set; }

		private void Awake()
		{
			Init();
		}

		private void Update()
		{
			m_unscaledDeltaTime = Time.unscaledDeltaTime;
			CurrentFPS = (short)Mathf.RoundToInt(1f / m_unscaledDeltaTime);
			uint num = 0u;
			m_indexSample++;
			if (m_indexSample >= m_fpsSamplesCapacity)
			{
				m_indexSample = 0;
			}
			m_fpsSamples[m_indexSample] = CurrentFPS;
			if (m_fpsSamplesCount < m_fpsSamplesCapacity)
			{
				m_fpsSamplesCount++;
			}
			for (int i = 0; i < m_fpsSamplesCount; i++)
			{
				num += (uint)m_fpsSamples[i];
			}
			AverageFPS = (short)((float)num / (float)m_fpsSamplesCount);
			m_fpsSamples.CopyTo(m_fpsSamplesSorted, 0);
			Array.Sort(m_fpsSamplesSorted, (short x, short y) => x.CompareTo(y));
			bool flag = false;
			uint num2 = 0u;
			short num3 = ((m_fpsSamplesCount < m_onePercentSamples) ? m_fpsSamplesCount : m_onePercentSamples);
			short num4 = ((m_fpsSamplesCount < m_zero1PercentSamples) ? m_fpsSamplesCount : m_zero1PercentSamples);
			short num5 = (short)(m_fpsSamplesCapacity - m_fpsSamplesCount);
			for (short num6 = num5; num6 < num5 + num3; num6++)
			{
				num2 += (ushort)m_fpsSamplesSorted[num6];
				if (!flag && num6 >= num4 - 1)
				{
					flag = true;
					Zero1PercentFps = (short)((float)num2 / (float)m_zero1PercentSamples);
				}
			}
			OnePercentFPS = (short)((float)num2 / (float)m_onePercentSamples);
		}

		public void UpdateParameters()
		{
			m_onePercentSamples = (short)(m_fpsSamplesCapacity / 100);
			m_zero1PercentSamples = (short)(m_fpsSamplesCapacity / 1000);
		}

		private void Init()
		{
			m_fpsSamples = new short[m_fpsSamplesCapacity];
			m_fpsSamplesSorted = new short[m_fpsSamplesCapacity];
			UpdateParameters();
		}
	}
}
