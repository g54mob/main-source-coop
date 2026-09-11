using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using PlayEveryWare.VideoEncoding;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Zorro.Recorder
{
	public class LibVEEncoder
	{
		private RecordingSession m_session;

		private IntPtr m_instanceHandle;

		private int m_frameRate;

		private int m_encodedVideoFrames;

		private int m_encodedAudioSamples;

		private int m_audioInputSampleRate;

		private int m_audioInputChannels;

		private int m_outChannels;

		private const int k_defaultMicSampleRate = 24000;

		private int m_micSampleRate;

		private int m_micChannels;

		private bool m_mic;

		private static readonly int MaxVideoLengthSecs = 90;

		private NativeList<float> m_resampledAudioData = new NativeList<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond * MaxVideoLengthSecs * LibVEAudioMixer.RemixToChannels, Allocator.Persistent);

		private NativeList<float> m_resampledMicAudioData = new NativeList<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond * MaxVideoLengthSecs * LibVEAudioMixer.RemixToChannels, Allocator.Persistent);

		private NativeList<float> m_finalMixedAudioData = new NativeList<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond * MaxVideoLengthSecs * LibVEAudioMixer.RemixToChannels, Allocator.Persistent);

		private NativeArray<float> m_leftChannelEncodingBuffer = new NativeArray<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond, Allocator.Persistent);

		private NativeArray<float> m_rightChannelEncodingBuffer = new NativeArray<float>(LibVEAudioMixer.MaxExpectedSamplesPerSecond, Allocator.Persistent);

		public string ErrorTitle;

		public string ErrorBody;

		public LibVEEncoder(RecordingSession session, byte framerate, bool mic)
		{
			m_session = session;
			m_frameRate = framerate;
			m_mic = mic;
			m_outChannels = m_session.AudioChannels;
			if (!LibVEInitializer.InitializeLibVE())
			{
				throw new Exception("Failed to initialize video encoding, won't be able to record or extract clips");
			}
		}

		public void PopulateMainThreadParameters()
		{
			m_audioInputSampleRate = AudioSettings.outputSampleRate;
			m_audioInputChannels = m_session.AudioChannels;
			m_micChannels = m_session.MicChannels;
			if (m_session.MicSampleRate.IsSome)
			{
				m_micSampleRate = m_session.MicSampleRate.Value;
			}
			else
			{
				Debug.LogWarning($"Warning: Cannot read microphone sample rate; defaulting to {24000}");
				m_micSampleRate = 24000;
			}
			Debug.Log($"[LibVeEncoder] Encoding parameters: m_audioInputSampleRate {m_audioInputSampleRate}, " + $"m_audioInputChannels {m_session.AudioChannels}, " + $"m_micChannels {m_session.MicChannels}, " + $"m_micSampleRate {m_micSampleRate}");
		}

		public bool InitializeEncoding()
		{
			string text = Path.Combine(m_session.Pipe.GetDirectory().FullName ?? "", "output.webm");
			Debug.Log("Output file is " + text);
			ve_config config = LibVEInitializer.GetNewVeConfig(text);
			if (!NativeVeMethods.initialize_encoding(ref config, out m_instanceHandle))
			{
				Debug.LogError("[LibVEEncoder] Failed to initialize encoding");
				return false;
			}
			return true;
		}

		public unsafe bool EncodeAudioSamples(NativeList<float> audioData, NativeList<float> micData)
		{
			if (m_audioInputChannels != 2 || m_micChannels != 2)
			{
				throw new Exception("Require stereo audio being input to this function");
			}
			if (m_outChannels != 2)
			{
				throw new Exception("Require stereo audio being output from this function");
			}
			Debug.Log($"[LibVEEncoder] Received audio data, game audio samples: {audioData.Length}, mic samples: {micData.Length}");
			LibVEAudioMixer.Resample(audioData, m_audioInputChannels, m_audioInputSampleRate, RecordingConstants.AUDIO_SAMPLE_RATE, ref m_resampledAudioData);
			if (m_mic)
			{
				LibVEAudioMixer.Resample(micData, m_micChannels, m_micSampleRate, RecordingConstants.AUDIO_SAMPLE_RATE, ref m_resampledMicAudioData);
				LibVEAudioMixer.MixAudio(m_resampledAudioData, m_audioInputChannels, m_resampledMicAudioData, m_micChannels, m_outChannels, ref m_finalMixedAudioData);
				Debug.Log($"[LibVEEncoder] Resampled game audio length: {m_resampledAudioData.Length} " + $"Resampled mic audio length: {m_resampledMicAudioData.Length} " + $"Final mixed data length: {m_finalMixedAudioData.Length}");
			}
			else
			{
				m_finalMixedAudioData.Dispose();
				m_finalMixedAudioData = new NativeList<float>(m_resampledAudioData.Length, Allocator.Persistent);
				m_finalMixedAudioData.CopyFrom(in m_resampledAudioData);
			}
			if (RecordingConstants.AUDIO_SAMPLE_RATE % RecordingConstants.VIDEO_FRAME_RATE != 0)
			{
				Debug.LogError("[LibVEEncoder] Error! Sample sample rate is not divisible by video frame rate, so you may not get perfectly lined up audio");
			}
			int num = 0;
			int num2 = RecordingConstants.AUDIO_SAMPLE_RATE / RecordingConstants.VIDEO_FRAME_RATE * m_audioInputChannels;
			int num3 = 0;
			IntPtr[] array = new IntPtr[2];
			IntPtr intPtr = (IntPtr)m_leftChannelEncodingBuffer.GetUnsafePtr();
			array[0] = intPtr;
			IntPtr intPtr2 = (IntPtr)m_rightChannelEncodingBuffer.GetUnsafePtr();
			array[1] = intPtr2;
			GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr interleaved_channel_buffer = gCHandle.AddrOfPinnedObject();
			do
			{
				num++;
				int num4 = (num - 1) * num2;
				int num5 = 0;
				for (int i = num4; i < num4 + num2 && i < m_finalMixedAudioData.Length; i += m_outChannels)
				{
					m_leftChannelEncodingBuffer[num5] = m_finalMixedAudioData[i];
					m_rightChannelEncodingBuffer[num5] = m_finalMixedAudioData[i + 1];
					num5++;
				}
				if (!NativeVeMethods.encode_stereo_audio_sample(m_instanceHandle, interleaved_channel_buffer, (uint)num5))
				{
					Debug.LogError("[LibVEEncoder] Failed to encode audio");
					break;
				}
				num3 += num5 * 2;
			}
			while (num < m_encodedVideoFrames);
			gCHandle.Free();
			int num6 = m_encodedVideoFrames * num2;
			if (num3 == num6)
			{
				m_encodedAudioSamples = num3;
				Debug.Log($"[LibVEEncoder] Encoded {num3} to match up with {m_encodedVideoFrames} in chunks of {num2}. " + $"Dropping {m_finalMixedAudioData.Length - num3} samples to avoid audio being longer than video");
				return true;
			}
			Debug.LogError($"[LibVEEncoder] Failed to encode audio; {num3}");
			return false;
		}

		public unsafe bool EncodeFrame(List<NativeArray<byte>> yuvImages, bool isKeyFrame)
		{
			if (yuvImages.Count % 3 != 0)
			{
				throw new Exception("Programming error; video encoding in YUV format must always pass frames in tuples of 3");
			}
			int num = 0;
			int num2 = 0;
			int num3 = yuvImages.Count / 3;
			bool flag = false;
			while (num2 < num3)
			{
				NativeArray<byte> nativeArray = yuvImages[num2 * 3];
				NativeArray<byte> nativeArray2 = yuvImages[num2 * 3 + 1];
				NativeArray<byte> nativeArray3 = yuvImages[num2 * 3 + 2];
				num2++;
				IntPtr[] array = new IntPtr[3];
				IntPtr intPtr = (IntPtr)nativeArray.GetUnsafePtr();
				array[0] = intPtr;
				IntPtr intPtr2 = (IntPtr)nativeArray2.GetUnsafePtr();
				array[1] = intPtr2;
				IntPtr intPtr3 = (IntPtr)nativeArray3.GetUnsafePtr();
				array[2] = intPtr3;
				GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				IntPtr buffer = gCHandle.AddrOfPinnedObject();
				uint length = (uint)nativeArray.Length;
				ve_encode_video_flags ve_encode_video_flags2 = RecordingConstants.VIDEO_DEFAULT_ENCODE_FLAGS;
				if (isKeyFrame)
				{
					ve_encode_video_flags2 |= ve_encode_video_flags.FORCE_KEY_FRAME;
				}
				if (!NativeVeMethods.encode_video_frame_from_memory(m_instanceHandle, buffer, (uint)array.Length, length, ve_encode_video_flags2))
				{
					Debug.LogError("Failed to encode frame from memory");
					flag = true;
				}
				else
				{
					num++;
				}
				gCHandle.Free();
				nativeArray.Dispose();
				nativeArray2.Dispose();
				nativeArray3.Dispose();
				m_encodedVideoFrames++;
			}
			if (num2 == num && !flag)
			{
				return true;
			}
			Debug.LogError("[LibVEEncoder] Failed to encode video");
			return false;
		}

		public bool Finalize()
		{
			bool result = false;
			Debug.Log("[LibVEEncoder] Finalizing...");
			if (!NativeVeMethods.finalize(m_instanceHandle))
			{
				Debug.LogError("[LibVEEncoder] Failed to finalize");
			}
			else
			{
				Debug.Log("[LibVEEncoder] Done!");
				result = true;
			}
			double num = (double)m_encodedVideoFrames / (double)m_frameRate;
			Debug.LogError($"[LibVEEncoder] Number of video frames encoded: {m_encodedVideoFrames}, " + $"frame rate: {m_frameRate}, " + $"total seconds of video encoded: {num}");
			double num2 = (double)m_encodedAudioSamples / (double)RecordingConstants.AUDIO_SAMPLE_RATE / 2.0;
			Debug.LogError($"[LibVEEncoder] Number of audio samples encoded: {m_encodedAudioSamples}, " + $"output sample rate: {RecordingConstants.AUDIO_SAMPLE_RATE}, " + $"total seconds of audio encoded: {num2}");
			Dispose();
			return result;
		}

		public void Dispose()
		{
			m_resampledAudioData.Dispose();
			m_resampledMicAudioData.Dispose();
			m_finalMixedAudioData.Dispose();
			m_leftChannelEncodingBuffer.Dispose();
			m_rightChannelEncodingBuffer.Dispose();
		}
	}
}
