using System.Runtime.InteropServices;

namespace PlayEveryWare.VideoEncoding
{
	public class VeConfigFactory
	{
		public static ve_config Create()
		{
			return new ve_config
			{
				size_of = Marshal.SizeOf<ve_config>(),
				video_codec = ve_video_codec_type.VP8,
				video_buffer_prealloc_bytes = 1048576u,
				target_bitrate = 0u,
				min_quantizer = -1,
				max_quantizer = -1,
				lag_in_frames = -1,
				kf_min_dist = 0,
				kf_max_dist = 0,
				buffer_format = ve_buffer_format.RGBA,
				frame_format = ve_frame_format.VPX_IMG_FMT_I420,
				video_quality = ve_video_codec_quality.GOOD,
				num_threads = 0,
				cpu_used = sbyte.MinValue
			};
		}
	}
}
