using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class TemporaryBlitPass : ScriptableRenderPass
	{
		private bool m_UseSMSS;

		public void Setup(bool useSMSS)
		{
			m_UseSMSS = useSMSS;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordTemporaryBlit(renderGraph, frameData, m_UseSMSS);
		}
	}
}
