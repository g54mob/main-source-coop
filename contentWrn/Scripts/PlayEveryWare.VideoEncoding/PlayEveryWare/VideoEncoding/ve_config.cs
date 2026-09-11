using System.Runtime.InteropServices;

namespace PlayEveryWare.VideoEncoding
{
	public struct ve_config
	{
		public int size_of;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string output_file;

		public ve_video_codec_type video_codec;

		public uint width;

		public uint height;

		public uint fps;

		public uint video_buffer_prealloc_bytes;

		public uint target_bitrate;

		public int min_quantizer;

		public int max_quantizer;

		public int lag_in_frames;

		public int kf_min_dist;

		public int kf_max_dist;

		public ve_buffer_format buffer_format;

		public ve_frame_format frame_format;

		public ve_video_codec_quality video_quality;

		public uint audio_sample_rate;

		public uint audio_channels;

		public float audio_quality;

		[MarshalAs(UnmanagedType.I1)]
		public bool audio_enable;

		[MarshalAs(UnmanagedType.I1)]
		public bool overwrite_existing;

		[MarshalAs(UnmanagedType.U1)]
		public byte num_threads;

		[MarshalAs(UnmanagedType.I1)]
		public sbyte cpu_used;
	}
}
