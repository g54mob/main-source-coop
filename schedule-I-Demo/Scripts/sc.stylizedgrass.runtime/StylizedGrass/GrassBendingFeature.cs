using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace StylizedGrass
{
	public class GrassBendingFeature : ScriptableRendererFeature
	{
		public class RenderBendVectors : ScriptableRenderPass
		{
			private const string profilerTag = "Render Grass Bending Vectors";

			private static ProfilingSampler profilerSampler = new ProfilingSampler("Render Grass Bending Vectors");

			private const string profilerTagPass = "Geometry to vectors";

			private static ProfilingSampler profilerSamplerRendering = new ProfilingSampler("Geometry to vectors");

			private Settings settings;

			public const int TexelsPerMeter = 1;

			private const float FRUSTUM_MULTIPLIER = 2f;

			private const string LightModeTag = "GrassBender";

			private static RenderTexture vectorMap;

			private static readonly int vectorMapID = Shader.PropertyToID("_BendMap");

			private static readonly int vectorUVID = Shader.PropertyToID("_BendMapUV");

			private static readonly int _CameraForwardVector = Shader.PropertyToID("_CameraForwardVector");

			private static Vector4 rendererCoords;

			private static Vector4 cameraForwardvector;

			private static Vector3 centerPosition;

			private static int resolution;

			private static int m_resolution;

			public static int CurrentResolution;

			private static float orthoSize;

			private static Bounds bounds;

			private static readonly Quaternion viewRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));

			private static readonly Vector3 viewScale = new Vector3(1f, 1f, -1f);

			private static readonly Color neutralVector = new Color(0.5f, 0f, 0.5f, 0f);

			private static Rect viewportRect;

			private FilteringSettings m_FilteringSettings;

			private RenderStateBlock m_RenderStateBlock;

			private readonly List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>
			{
				new ShaderTagId("GrassBender")
			};

			private static readonly Plane[] frustrumPlanes = new Plane[6];

			private static Matrix4x4 projection { get; set; }

			private static Matrix4x4 view { get; set; }

			public RenderBendVectors(ref Settings settings)
			{
				this.settings = settings;
				m_FilteringSettings = new FilteringSettings(RenderQueueRange.all);
				m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
			}

			public static int CalculateResolution(float size)
			{
				return Mathf.Clamp(Mathf.NextPowerOfTwo(Mathf.RoundToInt(size * 1f)), 256, 4096);
			}

			public void Setup(RenderingData renderingData)
			{
				orthoSize = Mathf.Max(5f, settings.renderRange) * 0.5f;
				resolution = CalculateResolution(orthoSize);
				CurrentResolution = resolution;
				centerPosition = renderingData.cameraData.camera.transform.position + renderingData.cameraData.camera.transform.forward * orthoSize;
			}

			private static float SnapToTexel(float pos, float texelSize)
			{
				return (float)Mathf.FloorToInt(pos / texelSize) * texelSize + texelSize * 0.5f;
			}

			private static Vector3 SnapToTexel(Vector3 pos, float texelSizeX, float texelSizeZ)
			{
				return new Vector3(SnapToTexel(pos.x, texelSizeX), pos.y, SnapToTexel(pos.z, texelSizeZ));
			}

			public void SetupProjection(CommandBuffer cmd, ref RenderingData renderingData)
			{
				Setup(renderingData);
				centerPosition = StabilizeProjection(centerPosition, orthoSize * 2f / (float)resolution);
				bounds = new Bounds(centerPosition, Vector3.one * orthoSize);
				centerPosition -= Vector3.up * orthoSize * 2f;
				projection = Matrix4x4.Ortho(0f - orthoSize, orthoSize, 0f - orthoSize, orthoSize, 0.03f, orthoSize * 2f * 2f);
				view = Matrix4x4.TRS(centerPosition, viewRotation, viewScale).inverse;
				cmd.SetViewProjectionMatrices(view, projection);
				viewportRect.width = resolution;
				viewportRect.height = resolution;
				cmd.SetViewport(new Rect(0f, 0f, resolution, resolution));
				GeometryUtility.CalculateFrustumPlanes(projection * view, frustrumPlanes);
				rendererCoords.x = 1f - bounds.center.x - 1f + orthoSize;
				rendererCoords.y = 1f - bounds.center.z - 1f + orthoSize;
				rendererCoords.z = orthoSize * 2f;
				rendererCoords.w = 1f;
				cmd.SetGlobalVector(vectorUVID, rendererCoords);
			}

			private static Vector3 StabilizeProjection(Vector3 pos, float texelSize)
			{
				return new Vector3(Snap(pos.x, texelSize), Snap(pos.y, texelSize), Snap(pos.z, texelSize));
				static float Snap(float coord, float cellSize)
				{
					return (float)Mathf.FloorToInt(coord / cellSize) * cellSize + cellSize * 0.5f;
				}
			}

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				if (resolution != m_resolution)
				{
					m_resolution = resolution;
					if ((bool)vectorMap)
					{
						RenderTexture.ReleaseTemporary(vectorMap);
					}
					vectorMap = RenderTexture.GetTemporary(new RenderTextureDescriptor(resolution, resolution, GraphicsFormat.R16G16B16A16_SFloat, 0, 0));
					vectorMap.name = "_BendMap";
					cmd.SetGlobalTexture(vectorMapID, vectorMap);
				}
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get();
				DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, SortingCriteria.SortingLayer | SortingCriteria.RenderQueue | SortingCriteria.QuantizedFrontToBack);
				drawingSettings.enableInstancing = !UniversalRenderPipeline.asset.useSRPBatcher;
				drawingSettings.perObjectData = PerObjectData.None;
				using (new ProfilingScope(commandBuffer, profilerSampler))
				{
					SetupProjection(commandBuffer, ref renderingData);
					commandBuffer.SetRenderTarget(vectorMap);
					commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, neutralVector);
					cameraForwardvector = renderingData.cameraData.camera.transform.forward;
					cameraForwardvector.w = 1f;
					commandBuffer.SetGlobalVector(_CameraForwardVector, cameraForwardvector);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
					using (new ProfilingScope(commandBuffer, profilerSamplerRendering))
					{
						context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);
					}
					ref CameraData cameraData = ref renderingData.cameraData;
					RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), setInverseMatrices: false);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				CommandBufferPool.Release(commandBuffer);
			}

			public override void FrameCleanup(CommandBuffer cmd)
			{
				cameraForwardvector.w = 0f;
				cmd.SetGlobalVector(_CameraForwardVector, cameraForwardvector);
				rendererCoords.w = 0f;
				cmd.SetGlobalVector(vectorUVID, rendererCoords);
			}

			public static void DrawOrthographicViewGizmo()
			{
				Gizmos.matrix = Matrix4x4.identity;
				float distance = frustrumPlanes[4].distance;
				float distance2 = frustrumPlanes[5].distance;
				float num = distance + distance2;
				Vector3 center = new Vector3(view.inverse.m03, view.inverse.m13 + num * 0.5f, view.inverse.m23);
				Vector3 size = new Vector3(frustrumPlanes[0].distance + frustrumPlanes[1].distance, num, frustrumPlanes[2].distance + frustrumPlanes[3].distance);
				Gizmos.DrawWireCube(center, size);
				Gizmos.color = Color.white * 0.25f;
				Gizmos.DrawCube(center, size);
			}
		}

		[Serializable]
		public class Settings
		{
			[Min(10f)]
			public float renderRange = 50f;

			public bool ignoreSceneView;
		}

		private RenderBendVectors m_ScriptablePass;

		public Settings settings = new Settings();

		public static bool SRPBatcherEnabled()
		{
			if ((bool)UniversalRenderPipeline.asset)
			{
				return UniversalRenderPipeline.asset.useSRPBatcher;
			}
			return false;
		}

		public override void Create()
		{
			if (m_ScriptablePass == null)
			{
				m_ScriptablePass = new RenderBendVectors(ref settings);
			}
			m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRendering;
		}

		private void OnDisable()
		{
			Shader.SetGlobalVector("_BendMapUV", Vector4.zero);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			Camera camera = renderingData.cameraData.camera;
			if ((camera.cameraType == CameraType.SceneView || (camera.cameraType != CameraType.Reflection && camera.cameraType != CameraType.Preview && camera.hideFlags == HideFlags.None)) && (!settings.ignoreSceneView || camera.cameraType != CameraType.SceneView))
			{
				renderer.EnqueuePass(m_ScriptablePass);
			}
		}
	}
}
