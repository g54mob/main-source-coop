using System;
using UnityEngine;
using Zorro.Core;

namespace Photon.Voice.Unity
{
	public class MicrophoneRelay : Singleton<MicrophoneRelay>
	{
		public Action<float> onMicData;

		public float m_lastRead;

		public void SendMic(float[] buffer)
		{
			m_lastRead = Time.time;
			float level = MicrophoneLevelMax(buffer);
			float obj = MicrophoneLevelMaxDecibels(level);
			onMicData?.Invoke(obj);
		}

		private void Update()
		{
			if (Time.time - m_lastRead > 1f)
			{
				onMicData?.Invoke(0f);
				m_lastRead = Time.time;
			}
		}

		public void RegisterMicListener(Action<float> listen)
		{
			onMicData = (Action<float>)Delegate.Combine(onMicData, listen);
		}

		public void UnregisterMicListener(Action<float> listen)
		{
			onMicData = (Action<float>)Delegate.Remove(onMicData, listen);
		}

		private float MicrophoneLevelMax(float[] data)
		{
			int num = 128;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num3 = data[i] * data[i];
				if (num2 < num3)
				{
					num2 = num3;
				}
			}
			return num2;
		}

		private float MicrophoneLevelMaxDecibels(float level)
		{
			return 20f * Mathf.Log10(Mathf.Abs(level));
		}
	}
}
