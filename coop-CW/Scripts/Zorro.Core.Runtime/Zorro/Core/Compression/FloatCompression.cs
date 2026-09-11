using Unity.Mathematics;

namespace Zorro.Core.Compression
{
	public static class FloatCompression
	{
		public static byte CompressZeroOne(float value)
		{
			value = math.saturate(value);
			return (byte)(value * 255f);
		}

		public static float DecompressZeroOne(byte value)
		{
			return (float)(int)value / 255f;
		}
	}
}
