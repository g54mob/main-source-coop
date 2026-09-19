using System.Collections.Generic;
using INab.BetterFog.Core;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace INab.BetterFog.URP
{
	internal class FogOffsetPass : ScriptableRenderPass
	{
		private List<CustomRenderer> fogRenderers;

		public void Setup(List<CustomRenderer> fogRenderers)
		{
			this.fogRenderers = fogRenderers;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			frameData.GetOrCreate<BetterFogPassData>().RecordFogOffsetBlit(renderGraph, frameData, fogRenderers);
		}
	}
}
