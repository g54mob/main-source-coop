using System;

namespace Photon.Voice
{
	public class ByteStreamDecoder : IDecoder, IDisposable
	{
		public delegate void OutputDelegate(ref FrameBuffer buf);

		private OutputDelegate output;

		private Action onMissingFrame;

		public string Error => "";

		public ByteStreamDecoder(OutputDelegate output, Action onMissingFrame)
		{
			this.output = output;
			this.onMissingFrame = onMissingFrame;
		}

		public void Input(ref FrameBuffer buf)
		{
			if (buf.Array == null)
			{
				onMissingFrame?.Invoke();
			}
			else
			{
				output?.Invoke(ref buf);
			}
		}

		public void Open(VoiceInfo info)
		{
		}

		public void Dispose()
		{
		}
	}
}
