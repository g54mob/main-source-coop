using System;
using UnityEngine;
using Zorro.Core;

namespace Zorro.Recorder
{
	public class RecorderAudioListener : Singleton<RecorderAudioListener>
	{
		private VideoRecorder m_videoRecorder;

		private void OnAudioFilterRead(float[] data, int channels)
		{
			try
			{
				if (m_videoRecorder != null)
				{
					m_videoRecorder.AudioRead(data, channels);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
			}
		}

		public void SendAudioTo(VideoRecorder recorder)
		{
			Debug.Log("Setting camera to send audio to " + ((recorder == null) ? "null" : recorder.name));
			m_videoRecorder = recorder;
		}

		public void RemoveRecorder(VideoRecorder recorder)
		{
			if (m_videoRecorder == recorder)
			{
				m_videoRecorder = null;
			}
		}

		public void SendMic(float[] buffer, int sampleRate, int channels)
		{
			if (m_videoRecorder != null)
			{
				m_videoRecorder.PushMicData(CanRecordMic() ? buffer : new float[buffer.Length], sampleRate, channels);
			}
		}

		public virtual bool CanRecordMic()
		{
			return true;
		}
	}
}
