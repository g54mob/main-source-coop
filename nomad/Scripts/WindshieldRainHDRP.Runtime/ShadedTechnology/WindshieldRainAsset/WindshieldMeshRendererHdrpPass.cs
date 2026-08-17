using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ShadedTechnology.WindshieldRainAsset
{
	public class WindshieldMeshRendererHdrpPass : CustomPass
	{
		protected override bool executeInSceneView => true;

		protected override void Execute(CustomPassContext ctx)
		{
			if (WindshieldMeshRenderer.ActiveRenderers == null || WindshieldMeshRenderer.ActiveRenderers.Count == 0)
			{
				return;
			}
			CommandBuffer cmd = ctx.cmd;
			foreach (WindshieldMeshRenderer activeRenderer in WindshieldMeshRenderer.ActiveRenderers)
			{
				if (!(activeRenderer.mesh == null) && !(activeRenderer.material == null) && !(activeRenderer.transform == null))
				{
					int num = activeRenderer.material.FindPass("ForwardOnly");
					if (num < 0)
					{
						num = activeRenderer.material.FindPass("Forward");
					}
					if (num < 0)
					{
						num = 0;
					}
					cmd.DrawMesh(activeRenderer.mesh, activeRenderer.transform.localToWorldMatrix, activeRenderer.material, 0, num);
				}
			}
		}

		protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
		{
		}

		protected override void Cleanup()
		{
		}
	}
}
