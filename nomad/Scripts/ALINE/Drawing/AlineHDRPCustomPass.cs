using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Drawing
{
	internal class AlineHDRPCustomPass : CustomPass
	{
		private bool disabledDepth;

		protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
		{
			targetColorBuffer = TargetBuffer.Camera;
			targetDepthBuffer = TargetBuffer.Camera;
			disabledDepth = false;
		}

		protected override void Execute(CustomPassContext context)
		{
			if (!disabledDepth && context.cameraColorBuffer.isMSAAEnabled != context.cameraDepthBuffer.isMSAAEnabled)
			{
				Debug.LogWarning("ALINE: Cannot draw depth-tested gizmos due to limitations in Unity's high-definition render pipeline combined with MSAA. Typically this is caused by enabling Camera -> Frame Setting Overrides -> MSAA Within Forward.\n\nDepth-testing for gizmos will stay disabled until you disable this type of MSAA and recompile scripts.");
				disabledDepth = true;
				targetDepthBuffer = TargetBuffer.None;
			}
			DrawingManager.instance.SubmitFrame(context.hdCamera.camera, new DrawingData.CommandBufferWrapper
			{
				cmd = context.cmd
			}, usingRenderPipeline: true);
		}

		protected override void Cleanup()
		{
		}
	}
}
