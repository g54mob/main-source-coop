using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class SMSSPass : ScriptableRenderPass
	{
		private Material m_Material;

		private int m_Iterations;

		public void Setup(Material material, int iterations)
		{
			m_Material = material;
			m_Iterations = iterations;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordSMSSPass(renderGraph, frameData, m_Material, m_Iterations);
		}
	}
}
