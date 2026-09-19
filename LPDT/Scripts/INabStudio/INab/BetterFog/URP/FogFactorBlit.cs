using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class FogFactorBlit : ScriptableRenderPass
	{
		private Material m_Material;

		public void Setup(Material material)
		{
			m_Material = material;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordFogFactorPass(renderGraph, frameData, m_Material);
		}
	}
}
