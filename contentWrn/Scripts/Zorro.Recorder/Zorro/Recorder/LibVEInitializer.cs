using System;
using System.Runtime.InteropServices;
using PlayEveryWare.VideoEncoding;

namespace Zorro.Recorder
{
	public class LibVEInitializer
	{
		private static bool m_isDllInitialized;

		private static string m_temporaryDatamountPoint;

		public static bool InitializeLibVE()
		{
			if (!m_isDllInitialized)
			{
				byte[] array = new byte[32];
				IntPtr intPtr = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject();
				if (!NativeVeMethods.initialize(intPtr, (uint)array.Length))
				{
					Console.WriteLine("Failed to initialize DLL");
					return false;
				}
				m_temporaryDatamountPoint = Marshal.PtrToStringAnsi(intPtr);
				m_isDllInitialized = true;
			}
			return true;
		}

		public static string GetTemporaryDatamountPoint()
		{
			if (!m_isDllInitialized && !InitializeLibVE())
			{
				throw new Exception("Failed to initialize video encoding, won't be able to record or extract clips");
			}
			return m_temporaryDatamountPoint;
		}

		public static ve_config GetNewVeConfig(string outputFilename)
		{
			ve_config result = VeConfigFactory.Create();
			result.output_file = outputFilename;
			result.video_codec = ve_video_codec_type.VP8;
			result.width = (uint)RecordingConstants.VIDEO_OUTPUT_WIDTH;
			result.height = (uint)RecordingConstants.VIDEO_OUTPUT_HEIGHT;
			result.fps = RecordingConstants.VIDEO_FRAME_RATE;
			result.video_buffer_prealloc_bytes = (uint)RecordingConstants.VIDEO_BUFFER_PREALLOC_BYTES;
			result.buffer_format = ve_buffer_format.YUV_I420;
			result.frame_format = ve_frame_format.VPX_IMG_FMT_I420;
			result.video_quality = ve_video_codec_quality.REALTIME;
			result.audio_sample_rate = (uint)RecordingConstants.AUDIO_SAMPLE_RATE;
			result.audio_channels = RecordingConstants.AUDIO_CHANNELS;
			result.audio_quality = 0.5f;
			result.audio_enable = true;
			result.overwrite_existing = true;
			result.target_bitrate = (uint)RecordingConstants.TARGET_BITRATE;
			result.min_quantizer = RecordingConstants.MIN_QUANTIZER;
			result.max_quantizer = RecordingConstants.MAX_QUANTIZER;
			result.lag_in_frames = 0;
			result.kf_min_dist = 0;
			result.kf_max_dist = RecordingConstants.VIDEO_FRAME_RATE;
			result.num_threads = (byte)Environment.ProcessorCount;
			result.cpu_used = 5;
			return result;
		}
	}
}
