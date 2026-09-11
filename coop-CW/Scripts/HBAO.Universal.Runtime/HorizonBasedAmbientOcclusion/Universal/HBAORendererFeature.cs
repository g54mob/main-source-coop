using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

namespace HorizonBasedAmbientOcclusion.Universal
{
	public class HBAORendererFeature : ScriptableRendererFeature
	{
		private class HBAORenderPass : ScriptableRenderPass
		{
			private static class Pass
			{
				public const int AO = 0;

				public const int AO_Deinterleaved = 1;

				public const int Deinterleave_Depth = 2;

				public const int Deinterleave_Normals = 3;

				public const int Atlas_AO_Deinterleaved = 4;

				public const int Reinterleave_AO = 5;

				public const int Blur = 6;

				public const int Temporal_Filter = 7;

				public const int Copy = 8;

				public const int Composite = 9;

				public const int Debug_ViewNormals = 10;
			}

			private static class ShaderProperties
			{
				public static int mainTex;

				public static int inputTex;

				public static int hbaoTex;

				public static int tempTex;

				public static int tempTex2;

				public static int noiseTex;

				public static int depthTex;

				public static int normalsTex;

				public static int ssaoTex;

				public static int[] depthSliceTex;

				public static int[] normalsSliceTex;

				public static int[] aoSliceTex;

				public static int[] deinterleaveOffset;

				public static int atlasOffset;

				public static int jitter;

				public static int uvTransform;

				public static int inputTexelSize;

				public static int aoTexelSize;

				public static int deinterleavedAOTexelSize;

				public static int reinterleavedAOTexelSize;

				public static int uvToView;

				public static int targetScale;

				public static int radius;

				public static int maxRadiusPixels;

				public static int negInvRadius2;

				public static int angleBias;

				public static int aoMultiplier;

				public static int intensity;

				public static int multiBounceInfluence;

				public static int offscreenSamplesContrib;

				public static int maxDistance;

				public static int distanceFalloff;

				public static int baseColor;

				public static int colorBleedSaturation;

				public static int albedoMultiplier;

				public static int colorBleedBrightnessMask;

				public static int colorBleedBrightnessMaskRange;

				public static int blurDeltaUV;

				public static int blurSharpness;

				public static int temporalParams;

				public static int historyBufferRTHandleScale;

				static ShaderProperties()
				{
					mainTex = Shader.PropertyToID("_MainTex");
					inputTex = Shader.PropertyToID("_InputTex");
					hbaoTex = Shader.PropertyToID("_HBAOTex");
					tempTex = Shader.PropertyToID("_TempTex");
					tempTex2 = Shader.PropertyToID("_TempTex2");
					noiseTex = Shader.PropertyToID("_NoiseTex");
					depthTex = Shader.PropertyToID("_DepthTex");
					normalsTex = Shader.PropertyToID("_NormalsTex");
					ssaoTex = Shader.PropertyToID("_SSAOTex");
					depthSliceTex = new int[16];
					normalsSliceTex = new int[16];
					aoSliceTex = new int[16];
					for (int i = 0; i < 16; i++)
					{
						depthSliceTex[i] = Shader.PropertyToID("_DepthSliceTex" + i);
						normalsSliceTex[i] = Shader.PropertyToID("_NormalsSliceTex" + i);
						aoSliceTex[i] = Shader.PropertyToID("_AOSliceTex" + i);
					}
					deinterleaveOffset = new int[4]
					{
						Shader.PropertyToID("_Deinterleave_Offset00"),
						Shader.PropertyToID("_Deinterleave_Offset10"),
						Shader.PropertyToID("_Deinterleave_Offset01"),
						Shader.PropertyToID("_Deinterleave_Offset11")
					};
					atlasOffset = Shader.PropertyToID("_AtlasOffset");
					jitter = Shader.PropertyToID("_Jitter");
					uvTransform = Shader.PropertyToID("_UVTransform");
					inputTexelSize = Shader.PropertyToID("_Input_TexelSize");
					aoTexelSize = Shader.PropertyToID("_AO_TexelSize");
					deinterleavedAOTexelSize = Shader.PropertyToID("_DeinterleavedAO_TexelSize");
					reinterleavedAOTexelSize = Shader.PropertyToID("_ReinterleavedAO_TexelSize");
					uvToView = Shader.PropertyToID("_UVToView");
					targetScale = Shader.PropertyToID("_TargetScale");
					radius = Shader.PropertyToID("_Radius");
					maxRadiusPixels = Shader.PropertyToID("_MaxRadiusPixels");
					negInvRadius2 = Shader.PropertyToID("_NegInvRadius2");
					angleBias = Shader.PropertyToID("_AngleBias");
					aoMultiplier = Shader.PropertyToID("_AOmultiplier");
					intensity = Shader.PropertyToID("_Intensity");
					multiBounceInfluence = Shader.PropertyToID("_MultiBounceInfluence");
					offscreenSamplesContrib = Shader.PropertyToID("_OffscreenSamplesContrib");
					maxDistance = Shader.PropertyToID("_MaxDistance");
					distanceFalloff = Shader.PropertyToID("_DistanceFalloff");
					baseColor = Shader.PropertyToID("_BaseColor");
					colorBleedSaturation = Shader.PropertyToID("_ColorBleedSaturation");
					albedoMultiplier = Shader.PropertyToID("_AlbedoMultiplier");
					colorBleedBrightnessMask = Shader.PropertyToID("_ColorBleedBrightnessMask");
					colorBleedBrightnessMaskRange = Shader.PropertyToID("_ColorBleedBrightnessMaskRange");
					blurDeltaUV = Shader.PropertyToID("_BlurDeltaUV");
					blurSharpness = Shader.PropertyToID("_BlurSharpness");
					temporalParams = Shader.PropertyToID("_TemporalParams");
					historyBufferRTHandleScale = Shader.PropertyToID("_HistoryBuffer_RTHandleScale");
				}

				public static string GetOrthographicProjectionKeyword(bool orthographic)
				{
					if (!orthographic)
					{
						return "__";
					}
					return "ORTHOGRAPHIC_PROJECTION";
				}

				public static string GetQualityKeyword(HBAO.Quality quality)
				{
					return quality switch
					{
						HBAO.Quality.Lowest => "QUALITY_LOWEST", 
						HBAO.Quality.Low => "QUALITY_LOW", 
						HBAO.Quality.Medium => "QUALITY_MEDIUM", 
						HBAO.Quality.High => "QUALITY_HIGH", 
						HBAO.Quality.Highest => "QUALITY_HIGHEST", 
						_ => "QUALITY_MEDIUM", 
					};
				}

				public static string GetNoiseKeyword(HBAO.NoiseType noiseType)
				{
					return noiseType switch
					{
						HBAO.NoiseType.InterleavedGradientNoise => "INTERLEAVED_GRADIENT_NOISE", 
						_ => "__", 
					};
				}

				public static string GetDeinterleavingKeyword(HBAO.Deinterleaving deinterleaving)
				{
					if (deinterleaving != HBAO.Deinterleaving.Disabled && deinterleaving == HBAO.Deinterleaving.x4)
					{
						return "DEINTERLEAVED";
					}
					return "__";
				}

				public static string GetDebugKeyword(HBAO.DebugMode debugMode)
				{
					return debugMode switch
					{
						HBAO.DebugMode.AOOnly => "DEBUG_AO", 
						HBAO.DebugMode.ColorBleedingOnly => "DEBUG_COLORBLEEDING", 
						HBAO.DebugMode.SplitWithoutAOAndWithAO => "DEBUG_NOAO_AO", 
						HBAO.DebugMode.SplitWithAOAndAOOnly => "DEBUG_AO_AOONLY", 
						HBAO.DebugMode.SplitWithoutAOAndAOOnly => "DEBUG_NOAO_AOONLY", 
						_ => "__", 
					};
				}

				public static string GetMultibounceKeyword(bool useMultiBounce, bool litAoModeEnabled)
				{
					if (!useMultiBounce || litAoModeEnabled)
					{
						return "__";
					}
					return "MULTIBOUNCE";
				}

				public static string GetOffscreenSamplesContributionKeyword(float offscreenSamplesContribution)
				{
					if (!(offscreenSamplesContribution > 0f))
					{
						return "__";
					}
					return "OFFSCREEN_SAMPLES_CONTRIBUTION";
				}

				public static string GetPerPixelNormalsKeyword(HBAO.PerPixelNormals perPixelNormals)
				{
					return perPixelNormals switch
					{
						HBAO.PerPixelNormals.Reconstruct4Samples => "NORMALS_RECONSTRUCT4", 
						HBAO.PerPixelNormals.Reconstruct2Samples => "NORMALS_RECONSTRUCT2", 
						_ => "__", 
					};
				}

				public static string GetBlurRadiusKeyword(HBAO.BlurType blurType)
				{
					return blurType switch
					{
						HBAO.BlurType.Narrow => "BLUR_RADIUS_2", 
						HBAO.BlurType.Medium => "BLUR_RADIUS_3", 
						HBAO.BlurType.Wide => "BLUR_RADIUS_4", 
						HBAO.BlurType.ExtraWide => "BLUR_RADIUS_5", 
						_ => "BLUR_RADIUS_3", 
					};
				}

				public static string GetVarianceClippingKeyword(HBAO.VarianceClipping varianceClipping)
				{
					return varianceClipping switch
					{
						HBAO.VarianceClipping._4Tap => "VARIANCE_CLIPPING_4TAP", 
						HBAO.VarianceClipping._8Tap => "VARIANCE_CLIPPING_8TAP", 
						_ => "__", 
					};
				}

				public static string GetColorBleedingKeyword(bool colorBleedingEnabled, bool litAoModeEnabled)
				{
					if (!colorBleedingEnabled || litAoModeEnabled)
					{
						return "__";
					}
					return "COLOR_BLEEDING";
				}

				public static string GetModeKeyword(HBAO.Mode mode)
				{
					if (mode != HBAO.Mode.LitAO)
					{
						return "__";
					}
					return "LIT_AO";
				}
			}

			private static class MersenneTwister
			{
				public static float[] Numbers = new float[32]
				{
					0.556725f, 0.00552f, 0.708315f, 0.583199f, 0.236644f, 0.99238f, 0.981091f, 0.119804f, 0.510866f, 0.560499f,
					0.961497f, 0.557862f, 0.539955f, 0.332871f, 0.417807f, 0.920779f, 0.730747f, 0.07669f, 0.008562f, 0.660104f,
					0.428921f, 0.511342f, 0.587871f, 0.906406f, 0.43798f, 0.620309f, 0.062196f, 0.119485f, 0.235646f, 0.795892f,
					0.044437f, 0.617311f
				};
			}

			private class CameraHistoryBuffers
			{
				public CameraData cameraData { get; set; }

				public BufferedRTHandleSystem historyRTSystem { get; set; }

				public int frameCount { get; set; }

				public int lastRenderedFrame { get; set; }
			}

			private enum HistoryBufferType
			{
				AmbientOcclusion = 0,
				ColorBleeding = 1
			}

			public HBAO hbao;

			private static readonly Vector2[] s_jitter = new Vector2[16];

			private static readonly float[] s_temporalRotations = new float[6] { 60f, 300f, 180f, 240f, 120f, 0f };

			private static readonly float[] s_temporalOffsets = new float[4] { 0f, 0.5f, 0.25f, 0.75f };

			private Mesh m_FullscreenTriangle;

			private HBAO.Resolution? m_PreviousResolution;

			private HBAO.NoiseType? m_PreviousNoiseType;

			private bool m_PreviousColorBleedingEnabled;

			private XRSettings.StereoRenderingMode m_PrevStereoRenderingMode;

			private string[] m_ShaderKeywords;

			private RenderTargetIdentifier[] m_RtsDepth = new RenderTargetIdentifier[4];

			private RenderTargetIdentifier[] m_RtsNormals = new RenderTargetIdentifier[4];

			private List<CameraHistoryBuffers> m_CameraHistoryBuffers = new List<CameraHistoryBuffers>();

			private Vector4[] m_UVToViewPerEye = new Vector4[2];

			private float[] m_RadiusPerEye = new float[2];

			private Material material { get; set; }

			private RenderTargetIdentifier source { get; set; }

			private CameraData cameraData { get; set; }

			private RenderTextureDescriptor sourceDesc { get; set; }

			private RenderTextureDescriptor aoDesc { get; set; }

			private RenderTextureDescriptor deinterleavedDepthDesc { get; set; }

			private RenderTextureDescriptor deinterleavedNormalsDesc { get; set; }

			private RenderTextureDescriptor deinterleavedAoDesc { get; set; }

			private RenderTextureDescriptor reinterleavedAoDesc { get; set; }

			private RenderTextureFormat colorFormat { get; set; }

			private GraphicsFormat graphicsColorFormat { get; set; }

			private RenderTextureFormat depthFormat { get; set; }

			private RenderTextureFormat normalsFormat { get; set; }

			private bool motionVectorsSupported { get; set; }

			private Texture2D noiseTex { get; set; }

			private static bool isLinearColorSpace => QualitySettings.activeColorSpace == ColorSpace.Linear;

			private bool renderingInSceneView => cameraData.camera.cameraType == CameraType.SceneView;

			private Mesh fullscreenTriangle
			{
				get
				{
					if (m_FullscreenTriangle != null)
					{
						return m_FullscreenTriangle;
					}
					m_FullscreenTriangle = new Mesh
					{
						name = "Fullscreen Triangle"
					};
					m_FullscreenTriangle.SetVertices(new List<Vector3>
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(-1f, 3f, 0f),
						new Vector3(3f, -1f, 0f)
					});
					m_FullscreenTriangle.SetIndices(new int[3] { 0, 1, 2 }, MeshTopology.Triangles, 0, calculateBounds: false);
					m_FullscreenTriangle.UploadMeshData(markNoLongerReadable: false);
					return m_FullscreenTriangle;
				}
			}

			public void FillSupportedRenderTextureFormats()
			{
				colorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default);
				graphicsColorFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? GraphicsFormat.R16G16B16A16_SFloat : GraphicsFormat.R8G8B8A8_SRGB);
				depthFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat) ? RenderTextureFormat.RFloat : RenderTextureFormat.RHalf);
				normalsFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010) ? RenderTextureFormat.ARGB2101010 : RenderTextureFormat.Default);
				motionVectorsSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf);
			}

			public void Setup(Shader shader, ScriptableRenderer renderer, RenderingData renderingData)
			{
				if (material == null)
				{
					material = CoreUtils.CreateEngineMaterial(shader);
				}
			}

			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				source = renderingData.cameraData.renderer.cameraColorTargetHandle;
				cameraData = renderingData.cameraData;
				FetchVolumeComponent();
				ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Depth;
				if (hbao.perPixelNormals.value == HBAO.PerPixelNormals.Camera)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Normal;
				}
				if (hbao.temporalFilterEnabled.value)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Motion;
				}
				ConfigureInput(scriptableRenderPassInput);
				ConfigureColorStoreAction(RenderBufferStoreAction.DontCare);
				base.renderPassEvent = ((hbao.debugMode.value != HBAO.DebugMode.Disabled) ? RenderPassEvent.AfterRenderingTransparents : ((hbao.mode.value != HBAO.Mode.LitAO) ? RenderPassEvent.BeforeRenderingTransparents : ((hbao.renderingPath.value == HBAO.RenderingPath.Deferred) ? RenderPassEvent.AfterRenderingGbuffer : ((RenderPassEvent)201))));
			}

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				if (!(material == null))
				{
					FetchVolumeComponent();
					if (hbao.IsActive())
					{
						FetchRenderParameters(cameraTextureDescriptor);
						CheckParameters();
						UpdateMaterialProperties();
						UpdateShaderKeywords();
					}
				}
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (material == null)
				{
					Debug.LogError("HBAO material has not been correctly initialized...");
				}
				else if (hbao.IsActive())
				{
					CameraHistoryBuffers currentCameraHistoryBuffers = GetCurrentCameraHistoryBuffers();
					currentCameraHistoryBuffers?.historyRTSystem.SwapAndSetReferenceSize(aoDesc.width, aoDesc.height);
					CommandBuffer commandBuffer = CommandBufferPool.Get("HBAO");
					if (hbao.mode.value == HBAO.Mode.LitAO)
					{
						CoreUtils.SetKeyword(commandBuffer, "_SCREEN_SPACE_OCCLUSION", state: true);
						commandBuffer.GetTemporaryRT(ShaderProperties.ssaoTex, aoDesc, FilterMode.Bilinear);
					}
					else
					{
						commandBuffer.GetTemporaryRT(ShaderProperties.inputTex, sourceDesc, FilterMode.Point);
						CopySource(commandBuffer);
					}
					commandBuffer.SetGlobalVector(ShaderProperties.temporalParams, (currentCameraHistoryBuffers != null) ? new Vector2(s_temporalRotations[currentCameraHistoryBuffers.frameCount % 6] / 360f, s_temporalOffsets[currentCameraHistoryBuffers.frameCount % 4]) : Vector2.zero);
					if (hbao.deinterleaving.value == HBAO.Deinterleaving.Disabled)
					{
						commandBuffer.GetTemporaryRT(ShaderProperties.hbaoTex, aoDesc, FilterMode.Bilinear);
						AO(commandBuffer);
					}
					else
					{
						commandBuffer.GetTemporaryRT(ShaderProperties.hbaoTex, reinterleavedAoDesc, FilterMode.Bilinear);
						DeinterleavedAO(commandBuffer);
					}
					Blur(commandBuffer);
					TemporalFilter(commandBuffer, currentCameraHistoryBuffers);
					Composite(commandBuffer);
					commandBuffer.ReleaseTemporaryRT(ShaderProperties.hbaoTex);
					if (hbao.mode.value != HBAO.Mode.LitAO)
					{
						commandBuffer.ReleaseTemporaryRT(ShaderProperties.inputTex);
					}
					context.ExecuteCommandBuffer(commandBuffer);
					CommandBufferPool.Release(commandBuffer);
				}
			}

			public override void FrameCleanup(CommandBuffer cmd)
			{
				if (hbao.mode.value == HBAO.Mode.LitAO)
				{
					cmd.ReleaseTemporaryRT(ShaderProperties.ssaoTex);
					CoreUtils.SetKeyword(cmd, "_SCREEN_SPACE_OCCLUSION", state: false);
				}
				for (int num = m_CameraHistoryBuffers.Count - 1; num >= 0; num--)
				{
					CameraHistoryBuffers buffers = m_CameraHistoryBuffers[num];
					if (Time.frameCount - buffers.lastRenderedFrame > 1)
					{
						ReleaseCameraHistoryBuffers(ref buffers);
					}
				}
			}

			public void Cleanup()
			{
				for (int num = m_CameraHistoryBuffers.Count - 1; num >= 0; num--)
				{
					CameraHistoryBuffers buffers = m_CameraHistoryBuffers[num];
					ReleaseCameraHistoryBuffers(ref buffers);
				}
				CoreUtils.Destroy(material);
				CoreUtils.Destroy(noiseTex);
			}

			private void FetchVolumeComponent()
			{
				if (hbao == null)
				{
					hbao = VolumeManager.instance.stack.GetComponent<HBAO>();
				}
			}

			private void FetchRenderParameters(RenderTextureDescriptor cameraTextureDesc)
			{
				cameraTextureDesc.msaaSamples = 1;
				cameraTextureDesc.depthBufferBits = 0;
				sourceDesc = cameraTextureDesc;
				int num = cameraTextureDesc.width;
				int num2 = cameraTextureDesc.height;
				int num3 = ((hbao.resolution.value == HBAO.Resolution.Full) ? 1 : ((hbao.deinterleaving.value != HBAO.Deinterleaving.Disabled) ? 1 : 2));
				if (num3 > 1)
				{
					num = (num + num % 2) / num3;
					num2 = (num2 + num2 % 2) / num3;
				}
				aoDesc = GetStereoCompatibleDescriptor(num, num2, colorFormat, 0, RenderTextureReadWrite.Linear);
				if (hbao.deinterleaving.value != HBAO.Deinterleaving.Disabled)
				{
					int num4 = cameraTextureDesc.width + ((cameraTextureDesc.width % 4 != 0) ? (4 - cameraTextureDesc.width % 4) : 0);
					int num5 = cameraTextureDesc.height + ((cameraTextureDesc.height % 4 != 0) ? (4 - cameraTextureDesc.height % 4) : 0);
					int width = num4 / 4;
					int height = num5 / 4;
					deinterleavedDepthDesc = GetStereoCompatibleDescriptor(width, height, depthFormat, 0, RenderTextureReadWrite.Linear);
					deinterleavedNormalsDesc = GetStereoCompatibleDescriptor(width, height, normalsFormat, 0, RenderTextureReadWrite.Linear);
					deinterleavedAoDesc = GetStereoCompatibleDescriptor(width, height, colorFormat, 0, RenderTextureReadWrite.Linear);
					reinterleavedAoDesc = GetStereoCompatibleDescriptor(num4, num5, colorFormat, 0, RenderTextureReadWrite.Linear);
				}
			}

			private RTHandle HistoryBufferAllocator(RTHandleSystem rtHandleSystem, int frameIndex)
			{
				TextureDimension textureDimension = TextureDimension.Tex2D;
				int slices = 1;
				if (XRSettings.enabled && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
				{
					textureDimension = TextureDimension.Tex2DArray;
					slices = 2;
				}
				Vector2 one = Vector2.one;
				GraphicsFormat graphicsFormat = graphicsColorFormat;
				string name = "HBAO_HistoryBuffer_" + frameIndex;
				TextureDimension dimension = textureDimension;
				return rtHandleSystem.Alloc(one, slices, DepthBits.None, graphicsFormat, FilterMode.Point, TextureWrapMode.Repeat, dimension, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: true, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, name);
			}

			private void AllocCameraHistoryBuffers(ref CameraHistoryBuffers buffers)
			{
				buffers = new CameraHistoryBuffers();
				buffers.cameraData = cameraData;
				buffers.frameCount = 0;
				buffers.historyRTSystem = new BufferedRTHandleSystem();
				buffers.historyRTSystem.AllocBuffer(0, HistoryBufferAllocator, 2);
				if (hbao.colorBleedingEnabled.value)
				{
					buffers.historyRTSystem.AllocBuffer(1, HistoryBufferAllocator, 2);
				}
				m_CameraHistoryBuffers.Add(buffers);
			}

			private void ReleaseCameraHistoryBuffers(ref CameraHistoryBuffers buffers)
			{
				buffers.historyRTSystem.ReleaseAll();
				buffers.historyRTSystem.Dispose();
				m_CameraHistoryBuffers.Remove(buffers);
				buffers = null;
			}

			private CameraHistoryBuffers GetCurrentCameraHistoryBuffers()
			{
				CameraHistoryBuffers buffers = null;
				if (hbao.temporalFilterEnabled.value && !renderingInSceneView)
				{
					for (int i = 0; i < m_CameraHistoryBuffers.Count; i++)
					{
						if (m_CameraHistoryBuffers[i].cameraData.camera == cameraData.camera)
						{
							buffers = m_CameraHistoryBuffers[i];
							break;
						}
					}
					if ((m_PreviousColorBleedingEnabled != hbao.colorBleedingEnabled.value || m_PrevStereoRenderingMode != XRSettings.stereoRenderingMode || m_PreviousResolution != hbao.resolution.value) && buffers != null)
					{
						ReleaseCameraHistoryBuffers(ref buffers);
						m_PreviousColorBleedingEnabled = hbao.colorBleedingEnabled.value;
						m_PreviousResolution = hbao.resolution.value;
						m_PrevStereoRenderingMode = XRSettings.stereoRenderingMode;
					}
					if (buffers == null)
					{
						AllocCameraHistoryBuffers(ref buffers);
					}
				}
				return buffers;
			}

			private void CopySource(CommandBuffer cmd)
			{
				BlitFullscreenTriangle(cmd, source, ShaderProperties.inputTex, material, 8);
			}

			private void AO(CommandBuffer cmd)
			{
				BlitFullscreenTriangleWithClear(cmd, (hbao.mode.value == HBAO.Mode.LitAO) ? source : ((RenderTargetIdentifier)ShaderProperties.inputTex), ShaderProperties.hbaoTex, material, new Color(0f, 0f, 0f, 1f));
			}

			private void DeinterleavedAO(CommandBuffer cmd)
			{
				for (int i = 0; i < 4; i++)
				{
					m_RtsDepth[0] = ShaderProperties.depthSliceTex[i << 2];
					m_RtsDepth[1] = ShaderProperties.depthSliceTex[(i << 2) + 1];
					m_RtsDepth[2] = ShaderProperties.depthSliceTex[(i << 2) + 2];
					m_RtsDepth[3] = ShaderProperties.depthSliceTex[(i << 2) + 3];
					m_RtsNormals[0] = ShaderProperties.normalsSliceTex[i << 2];
					m_RtsNormals[1] = ShaderProperties.normalsSliceTex[(i << 2) + 1];
					m_RtsNormals[2] = ShaderProperties.normalsSliceTex[(i << 2) + 2];
					m_RtsNormals[3] = ShaderProperties.normalsSliceTex[(i << 2) + 3];
					int num = (i & 1) << 1;
					int num2 = i >> 1 << 1;
					cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[0], new Vector2(num, num2));
					cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[1], new Vector2(num + 1, num2));
					cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[2], new Vector2(num, num2 + 1));
					cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[3], new Vector2(num + 1, num2 + 1));
					for (int j = 0; j < 4; j++)
					{
						cmd.GetTemporaryRT(ShaderProperties.depthSliceTex[j + 4 * i], deinterleavedDepthDesc, FilterMode.Point);
						cmd.GetTemporaryRT(ShaderProperties.normalsSliceTex[j + 4 * i], deinterleavedNormalsDesc, FilterMode.Point);
					}
					BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, m_RtsDepth, material, 2);
					BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, m_RtsNormals, material, 3);
				}
				for (int k = 0; k < 16; k++)
				{
					cmd.SetGlobalTexture(ShaderProperties.depthTex, ShaderProperties.depthSliceTex[k]);
					cmd.SetGlobalTexture(ShaderProperties.normalsTex, ShaderProperties.normalsSliceTex[k]);
					cmd.SetGlobalVector(ShaderProperties.jitter, s_jitter[k]);
					cmd.GetTemporaryRT(ShaderProperties.aoSliceTex[k], deinterleavedAoDesc, FilterMode.Point);
					BlitFullscreenTriangleWithClear(cmd, (hbao.mode.value == HBAO.Mode.LitAO) ? source : ((RenderTargetIdentifier)ShaderProperties.inputTex), ShaderProperties.aoSliceTex[k], material, new Color(0f, 0f, 0f, 1f), 1);
					cmd.ReleaseTemporaryRT(ShaderProperties.depthSliceTex[k]);
					cmd.ReleaseTemporaryRT(ShaderProperties.normalsSliceTex[k]);
				}
				cmd.GetTemporaryRT(ShaderProperties.tempTex, reinterleavedAoDesc, FilterMode.Point);
				for (int l = 0; l < 16; l++)
				{
					cmd.SetGlobalVector(ShaderProperties.atlasOffset, new Vector2(((l & 1) + ((l & 7) >> 2 << 1)) * deinterleavedAoDesc.width, (((l & 3) >> 1) + (l >> 3 << 1)) * deinterleavedAoDesc.height));
					BlitFullscreenTriangle(cmd, ShaderProperties.aoSliceTex[l], ShaderProperties.tempTex, material, 4);
					cmd.ReleaseTemporaryRT(ShaderProperties.aoSliceTex[l]);
				}
				BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, ShaderProperties.hbaoTex, material, 5);
				cmd.ReleaseTemporaryRT(ShaderProperties.tempTex);
			}

			private void Blur(CommandBuffer cmd)
			{
				if (hbao.blurType.value != HBAO.BlurType.None)
				{
					float num = aoDesc.width;
					float num2 = aoDesc.height;
					if (sourceDesc.useDynamicScale)
					{
						num *= ScalableBufferManager.widthScaleFactor;
						num2 *= ScalableBufferManager.heightScaleFactor;
					}
					cmd.GetTemporaryRT(ShaderProperties.tempTex, aoDesc, FilterMode.Bilinear);
					cmd.SetGlobalVector(ShaderProperties.blurDeltaUV, new Vector2(1f / num, 0f));
					BlitFullscreenTriangle(cmd, ShaderProperties.hbaoTex, ShaderProperties.tempTex, material, 6);
					cmd.SetGlobalVector(ShaderProperties.blurDeltaUV, new Vector2(0f, 1f / num2));
					BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, ShaderProperties.hbaoTex, material, 6);
					cmd.ReleaseTemporaryRT(ShaderProperties.tempTex);
				}
			}

			private void TemporalFilter(CommandBuffer cmd, CameraHistoryBuffers buffers)
			{
				if (hbao.temporalFilterEnabled.value && !renderingInSceneView && buffers != null)
				{
					cmd.SetGlobalVector(ShaderProperties.historyBufferRTHandleScale, buffers.historyRTSystem.rtHandleProperties.rtHandleScale);
					if (buffers.frameCount == 0)
					{
						cmd.SetRenderTarget(buffers.historyRTSystem.GetFrameRT(0, 1), 0, CubemapFace.Unknown, -1);
						cmd.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
						if (hbao.colorBleedingEnabled.value)
						{
							cmd.SetRenderTarget(buffers.historyRTSystem.GetFrameRT(1, 1), 0, CubemapFace.Unknown, -1);
							cmd.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 1f));
						}
					}
					Rect viewportRect = new Rect(Vector2.zero, buffers.historyRTSystem.rtHandleProperties.currentViewportSize);
					if (hbao.colorBleedingEnabled.value)
					{
						RTHandle frameRT = buffers.historyRTSystem.GetFrameRT(0, 0);
						RTHandle frameRT2 = buffers.historyRTSystem.GetFrameRT(1, 0);
						RTHandle frameRT3 = buffers.historyRTSystem.GetFrameRT(0, 1);
						RTHandle frameRT4 = buffers.historyRTSystem.GetFrameRT(1, 1);
						RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[2] { frameRT, frameRT2 };
						cmd.SetGlobalTexture(ShaderProperties.tempTex, frameRT4);
						BlitFullscreenTriangle(cmd, frameRT3, destinations, viewportRect, material, 7);
						cmd.SetGlobalTexture(ShaderProperties.hbaoTex, frameRT2);
					}
					else
					{
						RTHandle frameRT5 = buffers.historyRTSystem.GetFrameRT(0, 0);
						RTHandle frameRT6 = buffers.historyRTSystem.GetFrameRT(0, 1);
						BlitFullscreenTriangle(cmd, frameRT6, frameRT5, viewportRect, material, 7);
						cmd.SetGlobalTexture(ShaderProperties.hbaoTex, frameRT5);
					}
					buffers.frameCount++;
					buffers.lastRenderedFrame = Time.frameCount;
				}
				else
				{
					cmd.SetGlobalVector(ShaderProperties.historyBufferRTHandleScale, Vector4.one);
				}
			}

			private void Composite(CommandBuffer cmd)
			{
				BlitFullscreenTriangle(cmd, (hbao.mode.value == HBAO.Mode.LitAO) ? source : ((RenderTargetIdentifier)ShaderProperties.inputTex), (hbao.mode.value == HBAO.Mode.LitAO && hbao.debugMode.value == HBAO.DebugMode.Disabled) ? ((RenderTargetIdentifier)ShaderProperties.ssaoTex) : source, material, (hbao.debugMode.value == HBAO.DebugMode.ViewNormals) ? 10 : 9);
				if (hbao.mode.value == HBAO.Mode.LitAO)
				{
					cmd.SetGlobalTexture("_ScreenSpaceOcclusionTexture", ShaderProperties.ssaoTex);
					cmd.SetGlobalVector("_AmbientOcclusionParam", new Vector4(0f, 0f, 0f, hbao.directLightingStrength.value));
				}
			}

			private void UpdateMaterialProperties()
			{
				int width = cameraData.cameraTargetDescriptor.width;
				int height = cameraData.cameraTargetDescriptor.height;
				int num = ((!XRSettings.enabled || XRSettings.stereoRenderingMode != XRSettings.StereoRenderingMode.SinglePassInstanced || renderingInSceneView) ? 1 : 2);
				for (int i = 0; i < num; i++)
				{
					Matrix4x4 projectionMatrix = cameraData.GetProjectionMatrix(i);
					float m = projectionMatrix.m00;
					float m2 = projectionMatrix.m11;
					m_UVToViewPerEye[i] = new Vector4(2f / m, -2f / m2, -1f / m, 1f / m2);
					m_RadiusPerEye[i] = hbao.radius.value * 0.5f * ((float)(height / ((hbao.deinterleaving.value != HBAO.Deinterleaving.x4) ? 1 : 4)) / (2f / m2));
				}
				float num2 = Mathf.Max(16f, hbao.maxRadiusPixels.value * Mathf.Sqrt((float)(width * height) / 2073600f));
				num2 /= (float)((hbao.deinterleaving.value != HBAO.Deinterleaving.x4) ? 1 : 4);
				Vector4 value = ((hbao.deinterleaving.value == HBAO.Deinterleaving.x4) ? new Vector4((float)reinterleavedAoDesc.width / (float)width, (float)reinterleavedAoDesc.height / (float)height, 1f / ((float)reinterleavedAoDesc.width / (float)width), 1f / ((float)reinterleavedAoDesc.height / (float)height)) : ((hbao.resolution.value == HBAO.Resolution.Half) ? new Vector4(((float)width + 0.5f) / (float)width, ((float)height + 0.5f) / (float)height, 1f, 1f) : Vector4.one));
				material.SetTexture(ShaderProperties.noiseTex, noiseTex);
				material.SetVector(ShaderProperties.inputTexelSize, new Vector4(1f / (float)width, 1f / (float)height, width, height));
				if (sourceDesc.useDynamicScale)
				{
					material.SetVector(ShaderProperties.aoTexelSize, new Vector4(1f / ((float)aoDesc.width * ScalableBufferManager.widthScaleFactor), 1f / ((float)aoDesc.height * ScalableBufferManager.heightScaleFactor), (float)aoDesc.width * ScalableBufferManager.widthScaleFactor, (float)aoDesc.height * ScalableBufferManager.heightScaleFactor));
				}
				else
				{
					material.SetVector(ShaderProperties.aoTexelSize, new Vector4(1f / (float)aoDesc.width, 1f / (float)aoDesc.height, aoDesc.width, aoDesc.height));
				}
				material.SetVector(ShaderProperties.deinterleavedAOTexelSize, new Vector4(1f / (float)deinterleavedAoDesc.width, 1f / (float)deinterleavedAoDesc.height, deinterleavedAoDesc.width, deinterleavedAoDesc.height));
				material.SetVector(ShaderProperties.reinterleavedAOTexelSize, new Vector4(1f / (float)reinterleavedAoDesc.width, 1f / (float)reinterleavedAoDesc.height, reinterleavedAoDesc.width, reinterleavedAoDesc.height));
				material.SetVector(ShaderProperties.targetScale, value);
				material.SetVectorArray(ShaderProperties.uvToView, m_UVToViewPerEye);
				material.SetFloatArray(ShaderProperties.radius, m_RadiusPerEye);
				material.SetFloat(ShaderProperties.maxRadiusPixels, num2);
				material.SetFloat(ShaderProperties.negInvRadius2, -1f / (hbao.radius.value * hbao.radius.value));
				material.SetFloat(ShaderProperties.angleBias, hbao.bias.value);
				material.SetFloat(ShaderProperties.aoMultiplier, 2f * (1f / (1f - hbao.bias.value)));
				material.SetFloat(ShaderProperties.intensity, isLinearColorSpace ? hbao.intensity.value : (hbao.intensity.value * 0.45454547f));
				material.SetFloat(ShaderProperties.multiBounceInfluence, hbao.multiBounceInfluence.value);
				material.SetFloat(ShaderProperties.offscreenSamplesContrib, hbao.offscreenSamplesContribution.value);
				material.SetFloat(ShaderProperties.maxDistance, hbao.maxDistance.value);
				material.SetFloat(ShaderProperties.distanceFalloff, hbao.distanceFalloff.value);
				material.SetColor(ShaderProperties.baseColor, hbao.baseColor.value);
				material.SetFloat(ShaderProperties.blurSharpness, hbao.sharpness.value);
				material.SetFloat(ShaderProperties.colorBleedSaturation, hbao.saturation.value);
				material.SetFloat(ShaderProperties.colorBleedBrightnessMask, hbao.brightnessMask.value);
				material.SetVector(ShaderProperties.colorBleedBrightnessMaskRange, AdjustBrightnessMaskToGammaSpace(new Vector2(Mathf.Pow(hbao.brightnessMaskRange.value.x, 3f), Mathf.Pow(hbao.brightnessMaskRange.value.y, 3f))));
			}

			private void UpdateShaderKeywords()
			{
				if (m_ShaderKeywords == null || m_ShaderKeywords.Length != 12)
				{
					m_ShaderKeywords = new string[12];
				}
				m_ShaderKeywords[0] = ShaderProperties.GetOrthographicProjectionKeyword(cameraData.camera.orthographic);
				m_ShaderKeywords[1] = ShaderProperties.GetQualityKeyword(hbao.quality.value);
				m_ShaderKeywords[2] = ShaderProperties.GetNoiseKeyword(hbao.noiseType.value);
				m_ShaderKeywords[3] = ShaderProperties.GetDeinterleavingKeyword(hbao.deinterleaving.value);
				m_ShaderKeywords[4] = ShaderProperties.GetDebugKeyword(hbao.debugMode.value);
				m_ShaderKeywords[5] = ShaderProperties.GetMultibounceKeyword(hbao.useMultiBounce.value, hbao.mode.value == HBAO.Mode.LitAO);
				m_ShaderKeywords[6] = ShaderProperties.GetOffscreenSamplesContributionKeyword(hbao.offscreenSamplesContribution.value);
				m_ShaderKeywords[7] = ShaderProperties.GetPerPixelNormalsKeyword(hbao.perPixelNormals.value);
				m_ShaderKeywords[8] = ShaderProperties.GetBlurRadiusKeyword(hbao.blurType.value);
				m_ShaderKeywords[9] = ShaderProperties.GetVarianceClippingKeyword(hbao.varianceClipping.value);
				m_ShaderKeywords[10] = ShaderProperties.GetColorBleedingKeyword(hbao.colorBleedingEnabled.value, hbao.mode.value == HBAO.Mode.LitAO);
				m_ShaderKeywords[11] = ShaderProperties.GetModeKeyword(hbao.mode.value);
				material.shaderKeywords = m_ShaderKeywords;
			}

			private void CheckParameters()
			{
				if (hbao.deinterleaving.value != HBAO.Deinterleaving.Disabled && SystemInfo.supportedRenderTargetCount < 4)
				{
					hbao.SetDeinterleaving(HBAO.Deinterleaving.Disabled);
				}
				if (hbao.temporalFilterEnabled.value && !motionVectorsSupported)
				{
					hbao.EnableTemporalFilter(enabled: false);
				}
				if (hbao.colorBleedingEnabled.value && hbao.temporalFilterEnabled.value && SystemInfo.supportedRenderTargetCount < 2)
				{
					hbao.EnableTemporalFilter(enabled: false);
				}
				if (hbao.colorBleedingEnabled.value && hbao.mode.value == HBAO.Mode.LitAO)
				{
					hbao.EnableColorBleeding(enabled: false);
				}
				if (noiseTex == null || m_PreviousNoiseType != hbao.noiseType.value)
				{
					CoreUtils.Destroy(noiseTex);
					CreateNoiseTexture();
					m_PreviousNoiseType = hbao.noiseType.value;
				}
			}

			private RenderTextureDescriptor GetStereoCompatibleDescriptor(int width, int height, RenderTextureFormat format = RenderTextureFormat.Default, int depthBufferBits = 0, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default)
			{
				RenderTextureDescriptor result = sourceDesc;
				result.depthBufferBits = depthBufferBits;
				result.msaaSamples = 1;
				result.width = width;
				result.height = height;
				result.colorFormat = format;
				switch (readWrite)
				{
				case RenderTextureReadWrite.sRGB:
					result.sRGB = true;
					break;
				case RenderTextureReadWrite.Linear:
					result.sRGB = false;
					break;
				case RenderTextureReadWrite.Default:
					result.sRGB = isLinearColorSpace;
					break;
				}
				return result;
			}

			public void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex = 0)
			{
				cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			public void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Rect viewportRect, Material material, int passIndex = 0)
			{
				cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			public void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Material material, int passIndex = 0)
			{
				cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			public void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Rect viewportRect, Material material, int passIndex = 0)
			{
				cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
				cmd.SetViewport(viewportRect);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			public void BlitFullscreenTriangleWithClear(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Color clearColor, int passIndex = 0)
			{
				cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.ClearRenderTarget(clearDepth: false, clearColor: true, clearColor);
				cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, passIndex);
			}

			private Vector2 AdjustBrightnessMaskToGammaSpace(Vector2 v)
			{
				if (!isLinearColorSpace)
				{
					return ToGammaSpace(v);
				}
				return v;
			}

			private float ToGammaSpace(float v)
			{
				return Mathf.Pow(v, 0.45454547f);
			}

			private Vector2 ToGammaSpace(Vector2 v)
			{
				return new Vector2(ToGammaSpace(v.x), ToGammaSpace(v.y));
			}

			private void CreateNoiseTexture()
			{
				noiseTex = new Texture2D(4, 4, SystemInfo.SupportsTextureFormat(TextureFormat.RGHalf) ? TextureFormat.RGHalf : TextureFormat.RGB24, mipChain: false, linear: true);
				noiseTex.filterMode = FilterMode.Point;
				noiseTex.wrapMode = TextureWrapMode.Repeat;
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						float r = ((hbao.noiseType.value != HBAO.NoiseType.Dither) ? (0.25f * (0.0625f * (float)(((i + j) & 3) << 2) + (float)(i & 3))) : MersenneTwister.Numbers[num++]);
						float g = ((hbao.noiseType.value != HBAO.NoiseType.Dither) ? (0.25f * (float)((j - i) & 3)) : MersenneTwister.Numbers[num++]);
						Color color = new Color(r, g, 0f);
						noiseTex.SetPixel(i, j, color);
					}
				}
				noiseTex.Apply();
				int k = 0;
				int num2 = 0;
				for (; k < s_jitter.Length; k++)
				{
					float x = MersenneTwister.Numbers[num2++];
					float y = MersenneTwister.Numbers[num2++];
					s_jitter[k] = new Vector2(x, y);
				}
			}
		}

		[SerializeField]
		[HideInInspector]
		private Shader shader;

		private HBAORenderPass m_HBAORenderPass;

		private void OnDisable()
		{
			m_HBAORenderPass?.Cleanup();
		}

		public override void Create()
		{
			if (!base.isActive)
			{
				m_HBAORenderPass?.Cleanup();
				m_HBAORenderPass = null;
			}
			else
			{
				base.name = "HBAO";
				m_HBAORenderPass = new HBAORenderPass();
				m_HBAORenderPass.FillSupportedRenderTextureFormats();
			}
		}

		protected override void Dispose(bool disposing)
		{
			m_HBAORenderPass?.Cleanup();
			m_HBAORenderPass = null;
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			shader = Shader.Find("Hidden/Universal Render Pipeline/HBAO");
			if (shader == null)
			{
				Debug.LogWarning("HBAO shader was not found. Please ensure it compiles correctly");
			}
			else if (renderingData.cameraData.postProcessEnabled)
			{
				m_HBAORenderPass.Setup(shader, renderer, renderingData);
				renderer.EnqueuePass(m_HBAORenderPass);
			}
		}
	}
}
