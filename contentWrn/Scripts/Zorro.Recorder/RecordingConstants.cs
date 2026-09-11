using PlayEveryWare.VideoEncoding;

public static class RecordingConstants
{
	public static readonly int VIDEO_RENDER_TEXTURE_WIDTH = 420;

	public static readonly int VIDEO_RENDER_TEXTURE_HEIGHT = 420;

	public static readonly int VIDEO_BUFFER_PREALLOC_BYTES = 0;

	public static readonly ve_encode_video_flags VIDEO_DEFAULT_ENCODE_FLAGS = ve_encode_video_flags.NONE;

	public static readonly int VIDEO_OUTPUT_WIDTH = 420;

	public static readonly int VIDEO_OUTPUT_HEIGHT = 420;

	public static readonly byte VIDEO_FRAME_RATE = 24;

	public static readonly int TARGET_BITRATE = 25;

	public static readonly byte MIN_QUANTIZER = 20;

	public static readonly byte MAX_QUANTIZER = 50;

	public static readonly int AUDIO_SAMPLE_RATE = 24000;

	public static readonly byte AUDIO_CHANNELS = 2;

	public static readonly float AUDIO_QUALITY = 0.5f;
}
