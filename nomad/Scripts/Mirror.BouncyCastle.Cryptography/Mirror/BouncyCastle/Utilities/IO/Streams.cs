using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mirror.BouncyCastle.Utilities.IO
{
	public static class Streams
	{
		private static readonly int MaxStackAlloc = (Platform.Is64BitProcess ? 4096 : 1024);

		public static int DefaultBufferSize => MaxStackAlloc;

		public static void CopyTo(Stream source, Stream destination, int bufferSize)
		{
			byte[] array = new byte[bufferSize];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		public static async Task CopyToAsync(Stream source, Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			byte[] buffer = new byte[bufferSize];
			int count;
			while ((count = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) != 0)
			{
				await destination.WriteAsync(buffer, 0, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public static void PipeAll(Stream inStr, Stream outStr)
		{
			PipeAll(inStr, outStr, DefaultBufferSize);
		}

		public static void PipeAll(Stream inStr, Stream outStr, int bufferSize)
		{
			CopyTo(inStr, outStr, bufferSize);
		}

		public static byte[] ReadAll(Stream inStr)
		{
			MemoryStream memoryStream = new MemoryStream();
			PipeAll(inStr, memoryStream);
			return memoryStream.ToArray();
		}

		public static int ReadFully(Stream inStr, byte[] buf, int off, int len)
		{
			int i;
			int num;
			for (i = 0; i < len; i += num)
			{
				num = inStr.Read(buf, off + i, len - i);
				if (num < 1)
				{
					break;
				}
			}
			return i;
		}

		public static void ValidateBufferArguments(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length - offset;
			if ((offset | num) < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num2 = num - count;
			if ((count | num2) < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}
	}
}
