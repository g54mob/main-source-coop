using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class FogBlendBlit : ScriptableRenderPass
	{
		private Material m_Material;

		private bool m_UseSMSS;

		public void Setup(Material material, bool useSMSS)
		{
			m_Material = material;
			m_UseSMSS = useSMSS;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordFogBlendPass(renderGraph, frameData, m_Material, m_UseSMSS);
		}
	}
}
