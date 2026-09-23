using System;
using System.Threading;

namespace Mimicraft.Voice
{
	public sealed class VoiceRing
	{
		private readonly float[] buffer;

		private int write;

		private int read;

		private int count;

		public int Capacity => buffer.Length;

		public int TotalRead { get; private set; }

		public int Count => Volatile.Read(ref count);

		public int Free => buffer.Length - Count;

		public VoiceRing(int capacity)
		{
			if (capacity <= 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			buffer = new float[capacity];
		}

		public bool Write(float[] source, int offset, int length)
		{
			if (source == null || length <= 0 || offset < 0 || offset + length > source.Length)
			{
				return false;
			}
			if (length > Free)
			{
				return false;
			}
			int num = Math.Min(length, buffer.Length - write);
			Array.Copy(source, offset, buffer, write, num);
			int num2 = length - num;
			if (num2 > 0)
			{
				Array.Copy(source, offset + num, buffer, 0, num2);
			}
			write = ((num2 > 0) ? num2 : (write + num));
			if (write == buffer.Length)
			{
				write = 0;
			}
			Interlocked.Add(ref count, length);
			return true;
		}

		public int Read(float[] destination, int offset, int length)
		{
			if (destination == null || length <= 0 || offset < 0 || offset + length > destination.Length)
			{
				return 0;
			}
			int num = Math.Min(length, Count);
			if (num <= 0)
			{
				return 0;
			}
			int num2 = Math.Min(num, buffer.Length - read);
			Array.Copy(buffer, read, destination, offset, num2);
			int num3 = num - num2;
			if (num3 > 0)
			{
				Array.Copy(buffer, 0, destination, offset + num2, num3);
			}
			read = ((num3 > 0) ? num3 : (read + num2));
			if (read == buffer.Length)
			{
				read = 0;
			}
			Interlocked.Add(ref count, -num);
			TotalRead += num;
			return num;
		}

		public void Clear()
		{
			read = write;
			Interlocked.Exchange(ref count, 0);
		}
	}
}
