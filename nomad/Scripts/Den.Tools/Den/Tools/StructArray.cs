using System;
using System.Runtime.InteropServices;

namespace Den.Tools
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct StructArray
	{
		[FieldOffset(0)]
		public byte b0;

		[FieldOffset(1)]
		public byte b1;

		[FieldOffset(2)]
		public byte b2;

		[FieldOffset(3)]
		public byte b3;

		[FieldOffset(4)]
		public byte b4;

		[FieldOffset(5)]
		public byte b5;

		[FieldOffset(6)]
		public byte b6;

		[FieldOffset(7)]
		public byte b7;

		[FieldOffset(0)]
		private long l;

		public const int length = 8;

		public byte this[int n]
		{
			get
			{
				return (byte)((l >> n * 8) & 0xFF);
			}
			set
			{
				l &= ~(255L << n * 8);
				l |= (long)((ulong)value << n * 8);
			}
		}

		public float GetFloat(int n)
		{
			return (float)((l >> n * 8) & 0xFF) / 255f;
		}

		public void SetFloat(int n, float f)
		{
			long num = (int)(f * 255f);
			l &= ~(255L << n * 8);
			l |= num << n * 8;
		}
	}
}
