using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class DualRingReportBuffer : IDisposable
	{
		private readonly int qjsQbJmfxxcBfQqYjyRjfoSmXkFy;

		private readonly int scVmzlioQnSrTjLjHBhQtMcpCfSh;

		private readonly int vCAoGebhdoyMGQXAKQFUIDnWJuoX;

		private NativeRingBuffer jnWgiUSndTqvXcnVtFcajjpLbNYSA;

		private NativeRingBuffer IaxIPKjOsEcplbAIhIFxgGyErLjnd;

		private byte[] URwphKyYrcrEbEJSUuMGsxQjTmaH;

		private byte[] JxJFlPgugeLBtXhBKeFpHlUgjHWbc;

		private int wTWtPXmkEIasvDkUynqmdvrdcVjMc;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public int BufferLength => qjsQbJmfxxcBfQqYjyRjfoSmXkFy;

		public int BytesInBuffer => IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer;

		public int EntriesInBuffer => IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer / scVmzlioQnSrTjLjHBhQtMcpCfSh;

		public byte[] ReadBuffer => JxJFlPgugeLBtXhBKeFpHlUgjHWbc;

		public int LastNumBytesRead => wTWtPXmkEIasvDkUynqmdvrdcVjMc;

		public DualRingReportBuffer(int P_0, int P_1)
		{
			if (P_0 <= 0)
			{
				throw new ArgumentOutOfRangeException("entryByteLength must be > 0.");
			}
			if (P_1 < 1)
			{
				throw new ArgumentOutOfRangeException("entryCapacity must be >= 1.");
			}
			scVmzlioQnSrTjLjHBhQtMcpCfSh = P_0;
			vCAoGebhdoyMGQXAKQFUIDnWJuoX = P_1;
			qjsQbJmfxxcBfQqYjyRjfoSmXkFy = P_0 * P_1;
			jnWgiUSndTqvXcnVtFcajjpLbNYSA = new NativeRingBuffer(qjsQbJmfxxcBfQqYjyRjfoSmXkFy);
			IaxIPKjOsEcplbAIhIFxgGyErLjnd = new NativeRingBuffer(qjsQbJmfxxcBfQqYjyRjfoSmXkFy);
			URwphKyYrcrEbEJSUuMGsxQjTmaH = new byte[P_0];
			JxJFlPgugeLBtXhBKeFpHlUgjHWbc = new byte[P_0];
		}

		public int StartRead()
		{
			dymWjsjBfCEIrESusydeCnGdzfEW();
			return IaxIPKjOsEcplbAIhIFxgGyErLjnd.BytesInBuffer;
		}

		public int Read()
		{
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(JxJFlPgugeLBtXhBKeFpHlUgjHWbc, scVmzlioQnSrTjLjHBhQtMcpCfSh);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int Read(byte[] buffer, int numBytesToRead)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (numBytesToRead < 0 || numBytesToRead > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite");
			}
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(buffer, numBytesToRead);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int Read(IntPtr buffer, int bufferLength, int numBytesToRead)
		{
			if (buffer == IntPtr.Zero)
			{
				throw new ArgumentNullException("buffer");
			}
			if (bufferLength <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferLength");
			}
			if (numBytesToRead < 0 || numBytesToRead > bufferLength)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite");
			}
			int result = 0;
			lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
			{
				result = IaxIPKjOsEcplbAIhIFxgGyErLjnd.Read(buffer, bufferLength, bufferLength);
			}
			wTWtPXmkEIasvDkUynqmdvrdcVjMc = result;
			return result;
		}

		public int Write(byte[] buffer, int numBytesToWrite)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (numBytesToWrite < 0 || numBytesToWrite > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite");
			}
			int num = 0;
			lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
			{
				return jnWgiUSndTqvXcnVtFcajjpLbNYSA.Write(buffer, numBytesToWrite);
			}
		}

		public int Write(IntPtr buffer, int bufferLength, int numBytesToWrite)
		{
			if (buffer == IntPtr.Zero)
			{
				throw new ArgumentNullException("buffer");
			}
			if (bufferLength <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferLength");
			}
			if (numBytesToWrite < 0 || numBytesToWrite > bufferLength)
			{
				throw new ArgumentOutOfRangeException("numBytesToWrite");
			}
			int num = 0;
			lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
			{
				return jnWgiUSndTqvXcnVtFcajjpLbNYSA.Write(buffer, bufferLength, numBytesToWrite);
			}
		}

		public void Clear()
		{
			lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
			{
				lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
				{
					IaxIPKjOsEcplbAIhIFxgGyErLjnd.Reset();
					jnWgiUSndTqvXcnVtFcajjpLbNYSA.Reset();
				}
			}
		}

		private void dymWjsjBfCEIrESusydeCnGdzfEW()
		{
			lock (jnWgiUSndTqvXcnVtFcajjpLbNYSA)
			{
				lock (IaxIPKjOsEcplbAIhIFxgGyErLjnd)
				{
					MiscTools.Swap(ref jnWgiUSndTqvXcnVtFcajjpLbNYSA, ref IaxIPKjOsEcplbAIhIFxgGyErLjnd);
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~DualRingReportBuffer()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
