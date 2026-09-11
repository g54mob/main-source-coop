using System;

namespace Photon.Voice
{
	public class DecoderConfigFrame : IDisposable
	{
		private ILogger logger;

		private IDecoder decoder;

		private FrameBuffer configFrame;

		private bool configFrameDecoded;

		public DecoderConfigFrame(ILogger logger, IDecoder decoder)
		{
			this.logger = logger;
			this.decoder = decoder;
		}

		public bool TryConfigure(ref FrameBuffer buf, bool decoderReady)
		{
			if (configFrameDecoded)
			{
				return true;
			}
			if (buf.IsConfig)
			{
				configFrame = buf;
				buf.Retain();
				logger.LogInfo("[PV] [VD] storing config frame " + configFrame.Length);
			}
			if (!decoderReady)
			{
				return false;
			}
			if (configFrame.Array != null)
			{
				logger.LogInfo("[PV] [VD] decoding config frame " + configFrame.Length);
				configFrameDecoded = true;
				decoder.Input(ref configFrame);
				configFrame.Release();
			}
			return buf.Array != configFrame.Array;
		}

		public void Dispose()
		{
			configFrame.Release();
		}
	}
}
