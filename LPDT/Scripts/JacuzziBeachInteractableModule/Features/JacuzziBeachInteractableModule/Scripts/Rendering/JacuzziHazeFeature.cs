using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Features.JacuzziBeachInteractableModule.Scripts.Rendering
{
	public class JacuzziHazeFeature : ScriptableRendererFeature
	{
		private class JacuzziHazePass : ScriptableRenderPass
		{
			private class PassData
			{
				public Material Material;

				public TextureHandle Source;

				public float Blend;

				public float DistortionStrength;

				public float ChromaticAberration;

				public Texture NoiseTexture;

				public float NoiseTiling;

				public Vector2 NoiseScrollSpeed;

				public Vector4 TexelSize;

				public float Time;
			}

			private static readonly int BlendId = Shader.PropertyToID("_HazeBlend");

			private static readonly int DistortionStrengthId = Shader.PropertyToID("_HazeDistortionStrength");

			private static readonly int ChromaticAberrationId = Shader.PropertyToID("_HazeChromaticAberration");

			private static readonly int NoiseTexId = Shader.PropertyToID("_HazeNoiseTex");

			private static readonly int NoiseTilingId = Shader.PropertyToID("_HazeNoiseTiling");

			private static readonly int NoiseScrollSpeedId = Shader.PropertyToID("_HazeNoiseScrollSpeed");

			private static readonly int TexelSizeId = Shader.PropertyToID("_HazeTexelSize");

			private static readonly int TimeId = Shader.PropertyToID("_HazeTime");

			private static readonly Vector4 BlitScaleBias = new Vector4(1f, 1f, 0f, 0f);

			private readonly Material _material;

			private readonly Texture _noiseTexture;

			public JacuzziHazePass(Material material, Texture noiseTexture, RenderPassEvent renderPassEvent)
			{
				_material = material;
				_noiseTexture = noiseTexture;
				base.renderPassEvent = renderPassEvent;
			}

			public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
			{
				JacuzziHazeVolume component = VolumeManager.instance.stack.GetComponent<JacuzziHazeVolume>();
				if (component == null || !component.IsActive())
				{
					return;
				}
				UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
				RenderTextureDescriptor cameraTargetDescriptor = frameData.Get<UniversalCameraData>().cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_JacuzziHazeDistorted", clear: false);
				Vector4 texelSize = new Vector4(1f / (float)Mathf.Max(1, cameraTargetDescriptor.width), 1f / (float)Mathf.Max(1, cameraTargetDescriptor.height), cameraTargetDescriptor.width, cameraTargetDescriptor.height);
				PassData passData;
				using (IRasterRenderGraphBuilder rasterRenderGraphBuilder = renderGraph.AddRasterRenderPass<PassData>("JacuzziHaze_Distort", out passData, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\JacuzziBeachInteractableModule\\Scripts\\Rendering\\JacuzziHazeFeature.cs", 68))
				{
					passData.Material = _material;
					passData.Source = universalResourceData.activeColorTexture;
					passData.Blend = component.blend.value;
					passData.DistortionStrength = component.distortionStrength.value;
					passData.ChromaticAberration = component.chromaticAberration.value;
					passData.NoiseTexture = ((_noiseTexture != null) ? _noiseTexture : Texture2D.grayTexture);
					passData.NoiseTiling = component.noiseTiling.value;
					passData.NoiseScrollSpeed = component.noiseScrollSpeed.value;
					passData.TexelSize = texelSize;
					passData.Time = Time.time;
					rasterRenderGraphBuilder.UseTexture(in passData.Source);
					rasterRenderGraphBuilder.SetRenderAttachment(textureHandle, 0);
					rasterRenderGraphBuilder.AllowPassCulling(value: false);
					rasterRenderGraphBuilder.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
					{
						data.Material.SetFloat(BlendId, data.Blend);
						data.Material.SetFloat(DistortionStrengthId, data.DistortionStrength);
						data.Material.SetFloat(ChromaticAberrationId, data.ChromaticAberration);
						data.Material.SetTexture(NoiseTexId, data.NoiseTexture);
						data.Material.SetFloat(NoiseTilingId, data.NoiseTiling);
						data.Material.SetVector(NoiseScrollSpeedId, data.NoiseScrollSpeed);
						data.Material.SetVector(TexelSizeId, data.TexelSize);
						data.Material.SetFloat(TimeId, data.Time);
						Blitter.BlitTexture(context.cmd, data.Source, BlitScaleBias, data.Material, 0);
					});
				}
				PassData passData2;
				using IRasterRenderGraphBuilder rasterRenderGraphBuilder2 = renderGraph.AddRasterRenderPass<PassData>("JacuzziHaze_CopyBack", out passData2, "C:\\Dev\\UnityProjects\\RubberArmsNew\\RubberArmsPrototype\\Assets\\Features\\JacuzziBeachInteractableModule\\Scripts\\Rendering\\JacuzziHazeFeature.cs", 100);
				passData2.Source = textureHandle;
				rasterRenderGraphBuilder2.UseTexture(in passData2.Source);
				rasterRenderGraphBuilder2.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				rasterRenderGraphBuilder2.AllowPassCulling(value: false);
				rasterRenderGraphBuilder2.SetRenderFunc(delegate(PassData data, RasterGraphContext context)
				{
					Blitter.BlitTexture(context.cmd, data.Source, BlitScaleBias, 0f, bilinear: false);
				});
			}
		}

		private const string SHADER_PATH = "Hidden/Custom/JacuzziHaze";

		private const int DISTORT_PASS = 0;

		[SerializeField]
		private Shader _shader;

		[Tooltip("Tiling noise texture. Wrap Mode must be Repeat and sRGB should be off. Only the R channel is read.")]
		[SerializeField]
		private Texture2D _noiseTexture;

		[SerializeField]
		private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

		private Material _material;

		private JacuzziHazePass _pass;

		public override void Create()
		{
			if (_shader == null)
			{
				_shader = Shader.Find("Hidden/Custom/JacuzziHaze");
			}
			if (_shader != null && _material == null)
			{
				_material = CoreUtils.CreateEngineMaterial(_shader);
			}
			_pass = new JacuzziHazePass(_material, _noiseTexture, _renderPassEvent);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!(_material == null) && renderingData.cameraData.postProcessEnabled && (renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView))
			{
				JacuzziHazeVolume jacuzziHazeVolume = VolumeManager.instance.stack?.GetComponent<JacuzziHazeVolume>();
				if (!(jacuzziHazeVolume == null) && jacuzziHazeVolume.IsActive())
				{
					renderer.EnqueuePass(_pass);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			CoreUtils.Destroy(_material);
		}
	}
}
