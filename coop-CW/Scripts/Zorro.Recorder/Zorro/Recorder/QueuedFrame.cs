using UnityEngine;
using UnityEngine.Rendering;

namespace Zorro.Recorder
{
	public class QueuedFrame
	{
		public RenderTexture Frame;

		public AsyncGPUReadbackRequest ReadbackRequest;

		public QueuedFrame(RenderTexture frame, AsyncGPUReadbackRequest readbackRequest)
		{
			Frame = frame;
			ReadbackRequest = readbackRequest;
		}
	}
}
