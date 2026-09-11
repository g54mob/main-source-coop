using System;
using System.Runtime.InteropServices;

namespace PlayEveryWare.VideoEncoding
{
	public static class NativeVeMethods
	{
		private const string DllName = "libve";

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool initialize(IntPtr mount_point_buffer, uint buffer_size_bytes);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool cpu_has_required_simd_extensions();

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool initialize_encoding(ref ve_config config, out IntPtr handle);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool encode_video_frame_from_memory(IntPtr handle, IntPtr buffer, uint buffer_size_dim_0_elems, uint buffer_size_dim_1_bytes, ve_encode_video_flags flags);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool encode_mono_audio_sample(IntPtr handle, IntPtr mono_channel_buffer, uint num_samples);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool encode_stereo_audio_sample(IntPtr handle, IntPtr interleaved_channel_buffer, uint num_samples);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool finalize(IntPtr handle);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool try_force_release(IntPtr handle);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool initialize_decoding(ref ve_config config, IntPtr handle);

		[DllImport("libve", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool combine_webm_files(IntPtr handle, IntPtr array, int length);
	}
}
