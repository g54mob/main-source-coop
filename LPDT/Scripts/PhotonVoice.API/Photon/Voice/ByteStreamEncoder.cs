using System;

namespace Photon.Voice
{
	public class ByteStreamEncoder : IEncoderDirect<byte[]>, IEncoder, IDisposable
	{
		private static readonly ArraySegment<byte> EmptyBuffer = new ArraySegment<byte>(Array.Empty<byte>());

		public string Error => "";

		public Action<ArraySegment<byte>, FrameFlags> Output { private get; set; }

		public void Input(byte[] buf)
		{
			Output(new ArraySegment<byte>(buf), (FrameFlags)0);
		}

		public ArraySegment<byte> DequeueOutput(out FrameFlags flags)
		{
			flags = (FrameFlags)0;
			return EmptyBuffer;
		}

		public void EndOfStream()
		{
		}

		public I GetPlatformAPI<I>() where I : class
		{
			return null;
		}

		public void Dispose()
		{
		}
	}
}
