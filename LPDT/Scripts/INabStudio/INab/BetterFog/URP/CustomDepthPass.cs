using System.Collections.Generic;
using INab.BetterFog.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class CustomDepthPass : ScriptableRenderPass
	{
		private Material material;

		private List<CustomRenderer> depthRenderers;

		public void Setup(Material material, List<CustomRenderer> depthRenderers)
		{
			this.material = material;
			this.depthRenderers = depthRenderers;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordCustomDepthBlit(renderGraph, frameData, material, depthRenderers);
		}
	}
}
