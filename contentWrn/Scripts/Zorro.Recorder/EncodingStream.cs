using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using Zorro.Recorder;

public class EncodingStream
{
	private class EncodingWorkItem
	{
	}

	private class AudioEncodingWorkItem : EncodingWorkItem
	{
		public NativeList<float> m_gameAudio;

		public NativeList<float> m_micAudio;
	}

	private class VideoEncodingWorkItem : EncodingWorkItem
	{
		public List<NativeArray<byte>> m_yuvData;

		public bool m_isKeyFrame;
	}

	private class FinalizeWorkItem : EncodingWorkItem
	{
	}

	private class InitializeWorkItem : EncodingWorkItem
	{
	}

	private LibVEEncoder m_encoder;

	private bool m_allowedToQueueAdditionalVideoData = true;

	private bool m_allowedToQueueAdditionalAudioData = true;

	private BlockingCollection<EncodingWorkItem> m_workerThreadQueue;

	private Thread m_workerThread;

	public EncodingStream(RecordingSession session)
	{
		m_encoder = new LibVEEncoder(session, 24, mic: true);
		m_workerThreadQueue = new BlockingCollection<EncodingWorkItem>
		{
			new InitializeWorkItem()
		};
		m_workerThread = new Thread(WorkerThread);
		m_workerThread.Start();
	}

	private void WorkerThread()
	{
		foreach (EncodingWorkItem item in m_workerThreadQueue.GetConsumingEnumerable())
		{
			try
			{
				if (item is InitializeWorkItem)
				{
					InitializeEncoder();
				}
				else if (item is AudioEncodingWorkItem)
				{
					SendAudioToEncoder(item as AudioEncodingWorkItem);
				}
				else if (item is VideoEncodingWorkItem)
				{
					SendFrameToEncoder(item as VideoEncodingWorkItem);
				}
				else if (item is FinalizeWorkItem)
				{
					FinalizeEncoder();
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Worker thread error: " + ex.Message);
			}
		}
	}

	public void StopAcceptingVideoData()
	{
		m_allowedToQueueAdditionalVideoData = false;
	}

	public void StopAcceptingAudioData()
	{
		m_allowedToQueueAdditionalAudioData = false;
	}

	public bool QueueVideoData(List<NativeArray<byte>> yuvFrame, bool isKeyFrame)
	{
		if (!m_allowedToQueueAdditionalVideoData)
		{
			return false;
		}
		VideoEncodingWorkItem videoEncodingWorkItem = new VideoEncodingWorkItem();
		videoEncodingWorkItem.m_yuvData = yuvFrame;
		videoEncodingWorkItem.m_isKeyFrame = isKeyFrame;
		m_workerThreadQueue.Add(videoEncodingWorkItem);
		return true;
	}

	public bool QueueAudioData(NativeList<float> gameAudio, NativeList<float> micAudio)
	{
		if (!m_allowedToQueueAdditionalAudioData)
		{
			return false;
		}
		AudioEncodingWorkItem audioEncodingWorkItem = new AudioEncodingWorkItem();
		audioEncodingWorkItem.m_gameAudio = gameAudio;
		audioEncodingWorkItem.m_micAudio = micAudio;
		m_workerThreadQueue.Add(audioEncodingWorkItem);
		return true;
	}

	public void QueueFinalize()
	{
		FinalizeWorkItem item = new FinalizeWorkItem();
		m_workerThreadQueue.Add(item);
		m_workerThreadQueue.CompleteAdding();
	}

	public IEnumerator WaitForEncodingToFinishAsync()
	{
		while (m_workerThread.IsAlive)
		{
			yield return null;
		}
		Debug.Log("Encoding stream is done");
	}

	public void PopulateMainThreadParameters()
	{
		m_encoder.PopulateMainThreadParameters();
	}

	public Tuple<string, string> GetEncoderErrorInfo()
	{
		return new Tuple<string, string>(m_encoder.ErrorTitle, m_encoder.ErrorBody);
	}

	private void InitializeEncoder()
	{
		m_encoder.InitializeEncoding();
	}

	private void SendFrameToEncoder(VideoEncodingWorkItem item)
	{
		m_encoder.EncodeFrame(item.m_yuvData, item.m_isKeyFrame);
	}

	private void SendAudioToEncoder(AudioEncodingWorkItem item)
	{
		m_encoder.EncodeAudioSamples(item.m_gameAudio, item.m_micAudio);
	}

	private void FinalizeEncoder()
	{
		Debug.Log("Sending finalize to encoder");
		m_encoder.Finalize();
	}
}
