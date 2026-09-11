using UnityEngine;

namespace Zorro.Recorder
{
	public class VideoRecorder : MonoBehaviour
	{
		public string OutputPath;

		public bool m_record;

		public bool m_recordMicrophone;

		public Camera m_targetUnityCamera;

		public RecorderAudioListener m_audioListener;

		private RecordingSession m_session;

		private bool m_recordLastFrame;

		private float frameTime;

		public RecordingSession Session => m_session;

		public bool HasRecorded
		{
			get
			{
				if (m_session != null && m_session.Frames > 0)
				{
					return true;
				}
				return false;
			}
		}

		public void RecordFrame()
		{
			if (string.IsNullOrEmpty(OutputPath))
			{
				Debug.LogError("Output path is not set");
			}
		}

		private void Start()
		{
			frameTime = 1f / (float)(int)RecordingConstants.VIDEO_FRAME_RATE;
		}

		private void LateUpdate()
		{
			if (m_record != m_recordLastFrame)
			{
				if (m_record)
				{
					if (m_session == null)
					{
						m_session = new RecordingSession(RecordingConstants.VIDEO_RENDER_TEXTURE_WIDTH, RecordingConstants.VIDEO_RENDER_TEXTURE_HEIGHT, OutputPath, frameTime);
					}
					VideoCaptureSRPFeature.Data.SetRecordingSessionForCamera(m_targetUnityCamera, m_session);
					m_session.SetRecording(recording: true);
				}
				else
				{
					VideoCaptureSRPFeature.Data.ClearRegistrationEntryByRecordingSession(m_session);
					m_session.SetRecording(recording: false);
				}
			}
			m_recordLastFrame = m_record;
			if (m_session != null)
			{
				m_session.ProcessQueue();
			}
		}

		private void OnDestroy()
		{
			if (m_session != null)
			{
				m_session.SetRecording(recording: false);
			}
			VideoCaptureSRPFeature.Data.ClearRegistrationEntryByRecordingSession(m_session);
			m_session?.Dispose();
		}

		public void AudioRead(float[] data, int channels)
		{
			if (m_record)
			{
				m_session?.PushAudio(data, channels);
			}
		}

		public void PushMicData(float[] buffer, int sampleRate, int channels)
		{
			if (m_record && m_recordMicrophone)
			{
				m_session?.PushMicAudio(buffer, sampleRate, channels);
			}
		}
	}
}
