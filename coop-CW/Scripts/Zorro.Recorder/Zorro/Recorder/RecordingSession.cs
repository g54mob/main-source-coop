using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Zorro.Core;

namespace Zorro.Recorder
{
	public class RecordingSession : IDisposable
	{
		private RecordingPipe m_pipe;

		private bool m_debugDumpToPng;

		private bool m_isFirstFrame;

		private bool m_isRecording;

		private float m_timer;

		private float m_frameTime;

		private NativeList<float> m_audioData;

		private NativeList<float> m_micAudioData;

		private Queue<GpuDataRequest> _readbackQueue = new Queue<GpuDataRequest>(4);

		private EncodingStream m_encodingStream;

		private ProfilerMarker m_processQueueMarker = new ProfilerMarker("RecordingSession.ProcessQueue");

		private ProfilerMarker m_queueFrameMarker = new ProfilerMarker("RecordingSession.QueueFrame");

		private float amplitude = 1.5f;

		public int Frames { get; set; }

		public int AudioChannels { get; private set; }

		public int AudioFrames { get; private set; }

		public int MicAudioFrames { get; private set; }

		public Optionable<int> MicSampleRate { get; set; }

		public int MicChannels { get; set; }

		public RecordingPipe Pipe => m_pipe;

		public RecordingSession(int width, int height, string directory, float frameTime)
		{
			m_pipe = new RecordingPipe(width, height, directory);
			m_frameTime = frameTime;
			AudioChannels = LibVEAudioMixer.RemixToChannels;
			MicChannels = LibVEAudioMixer.RemixToChannels;
			AudioFrames = 0;
			int num = 5;
			m_audioData = new NativeList<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond * num * AudioChannels, Allocator.Persistent);
			m_micAudioData = new NativeList<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond * num * AudioChannels, Allocator.Persistent);
			m_encodingStream = new EncodingStream(this);
		}

		public void SetRecording(bool recording)
		{
			m_isRecording = recording;
			m_isFirstFrame = true;
		}

		public void QueueFrame(YuvTextureTriplet frame)
		{
			if (m_pipe == null)
			{
				return;
			}
			using (m_queueFrameMarker.Auto())
			{
				if (!m_isRecording)
				{
					return;
				}
				m_timer += Time.unscaledDeltaTime;
				if (m_timer > m_frameTime)
				{
					if (_readbackQueue.Count > 6)
					{
						Debug.LogWarning("Too many GPU readback requests.");
						return;
					}
					AsyncGPUReadbackRequest item = AsyncGPUReadback.Request(frame.Y, 0, GraphicsFormat.R8_UNorm);
					AsyncGPUReadbackRequest item2 = AsyncGPUReadback.Request(frame.U, 0, GraphicsFormat.R8_UNorm);
					AsyncGPUReadbackRequest item3 = AsyncGPUReadback.Request(frame.V, 0, GraphicsFormat.R8_UNorm);
					GpuDataRequest gpuDataRequest = new GpuDataRequest();
					gpuDataRequest.gpuRequests.Add(item);
					gpuDataRequest.gpuRequests.Add(item2);
					gpuDataRequest.gpuRequests.Add(item3);
					gpuDataRequest.isKeyFrame = m_isFirstFrame;
					m_isFirstFrame = false;
					_readbackQueue.Enqueue(gpuDataRequest);
					m_timer -= m_frameTime;
				}
			}
		}

		private bool AllRequestsDone(GpuDataRequest req)
		{
			foreach (AsyncGPUReadbackRequest gpuRequest in req.gpuRequests)
			{
				if (!gpuRequest.done)
				{
					return false;
				}
			}
			return true;
		}

		private void WaitForAllRequestsToBeDone(GpuDataRequest req)
		{
			foreach (AsyncGPUReadbackRequest gpuRequest in req.gpuRequests)
			{
				gpuRequest.WaitForCompletion();
			}
		}

		private bool AnyRequestHasError(GpuDataRequest req)
		{
			foreach (AsyncGPUReadbackRequest gpuRequest in req.gpuRequests)
			{
				if (gpuRequest.hasError)
				{
					return true;
				}
			}
			return false;
		}

		public void ProcessQueue()
		{
			using (m_processQueueMarker.Auto())
			{
				GpuDataRequest result;
				while (_readbackQueue.TryPeek(out result))
				{
					if (!AllRequestsDone(result))
					{
						bool flag = false;
						foreach (GpuDataRequest item in _readbackQueue)
						{
							flag |= AllRequestsDone(item);
						}
						if (!flag)
						{
							break;
						}
						WaitForAllRequestsToBeDone(result);
					}
					_readbackQueue.Dequeue();
					if (AnyRequestHasError(result))
					{
						Debug.LogError("GPU readback error was detected.");
						continue;
					}
					new List<char> { 'y', 'u', 'v' };
					List<NativeArray<byte>> list = new List<NativeArray<byte>>();
					for (int i = 0; i < result.gpuRequests.Count; i++)
					{
						NativeArray<byte> data = result.gpuRequests[i].GetData<byte>();
						NativeArray<byte> nativeArray = new NativeArray<byte>(data.Length, Allocator.Persistent);
						data.CopyTo(nativeArray);
						list.Add(nativeArray);
					}
					m_pipe.PushFrameData(m_encodingStream, result, list);
					Frames++;
				}
			}
		}

		public static void DumpNativeArrayToPng(NativeArray<byte> pixelData, int width, int height, string filename)
		{
			NativeArray<byte> data = new NativeArray<byte>(width * height * 4, Allocator.Temp);
			for (int i = 0; i < height; i++)
			{
				int num = height - 1 - i;
				for (int j = 0; j < width; j++)
				{
					int index = num * width + j;
					int num2 = (i * width + j) * 4;
					byte value = (data[num2] = pixelData[index]);
					data[num2 + 1] = value;
					data[num2 + 2] = value;
					data[num2 + 3] = byte.MaxValue;
				}
			}
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false, linear: false);
			texture2D.LoadRawTextureData(data);
			texture2D.Apply();
			byte[] bytes = texture2D.EncodeToPNG();
			UnityEngine.Object.Destroy(texture2D);
			data.Dispose();
			File.WriteAllBytes(filename, bytes);
		}

		public void Dispose()
		{
			m_pipe?.Dispose();
			m_audioData.Dispose();
			m_micAudioData.Dispose();
		}

		public void PushAudio(float[] data, int channels)
		{
			LibVEAudioMixer.RemixToStereo(data, 1f, channels, ref m_audioData);
			AudioFrames++;
		}

		public byte[] GetAudioBlob()
		{
			NativeSlice<byte> nativeSlice = new NativeSlice<float>(m_audioData).SliceConvert<byte>();
			byte[] array = new byte[nativeSlice.Length];
			nativeSlice.CopyTo(array);
			return array;
		}

		public NativeList<float> GetAudioFloats()
		{
			return m_audioData;
		}

		public byte[] GetMicAudioBlob()
		{
			NativeSlice<byte> nativeSlice = new NativeSlice<float>(m_micAudioData).SliceConvert<byte>();
			byte[] array = new byte[nativeSlice.Length];
			nativeSlice.CopyTo(array);
			return array;
		}

		public NativeList<float> GetMicAudioFloats()
		{
			return m_micAudioData;
		}

		public string GetAudioPath()
		{
			return Pipe.GetDirectory()?.ToString() + "/audio.raw";
		}

		public string GetMicAudioPath()
		{
			return Pipe.GetDirectory()?.ToString() + "/mic.raw";
		}

		public void PushMicAudio(float[] data, int sampleRate, int channels)
		{
			if (MicSampleRate.IsNone)
			{
				MicSampleRate = Optionable<int>.Some(sampleRate);
				Debug.Log("Setting sample rate to: " + MicSampleRate.Value);
			}
			LibVEAudioMixer.RemixToStereo(data, amplitude, channels, ref m_micAudioData);
			MicAudioFrames++;
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

		public IEnumerator FinalizeEncoder()
		{
			m_encodingStream.PopulateMainThreadParameters();
			m_encodingStream.StopAcceptingVideoData();
			m_encodingStream.QueueAudioData(m_audioData, m_micAudioData);
			m_encodingStream.StopAcceptingAudioData();
			m_encodingStream.QueueFinalize();
			yield return m_encodingStream.WaitForEncodingToFinishAsync();
		}

		public Tuple<string, string> GetEncoderErrorInfo()
		{
			return m_encodingStream.GetEncoderErrorInfo();
		}
	}
}
