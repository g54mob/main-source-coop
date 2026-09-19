using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Features.LevelLightModule.Scripts.Rendering
{
	public sealed class DepthPrePassFeature : ScriptableRendererFeature
	{
		private sealed class DepthPrePass : ScriptableRenderPass
		{
			public DepthPrePass(RenderPassEvent renderPassEvent)
			{
				base.renderPassEvent = renderPassEvent;
				ConfigureInput(ScriptableRenderPassInput.Depth);
			}

			public void SetRenderPassEvent(RenderPassEvent renderPassEventIn)
			{
				base.renderPassEvent = renderPassEventIn;
			}
		}

		[SerializeField]
		private RenderPassEvent _renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

		private DepthPrePass _pass;

		public override void Create()
		{
			_pass = new DepthPrePass(_renderPassEvent);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView)
			{
				_pass.SetRenderPassEvent(_renderPassEvent);
				renderer.EnqueuePass(_pass);
			}
		}
	}
}
