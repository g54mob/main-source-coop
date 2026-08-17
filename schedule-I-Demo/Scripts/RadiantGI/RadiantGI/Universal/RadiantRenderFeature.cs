using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RadiantGI.Universal
{
	public class RadiantRenderFeature : ScriptableRendererFeature
	{
		public enum RenderingPath
		{
			Forward = 0,
			Deferred = 1,
			Both = 2
		}

		private enum Pass
		{
			CopyExact = 0,
			Raycast = 1,
			BlurHorizontal = 2,
			BlurVertical = 3,
			Upscale = 4,
			TemporalAccum = 5,
			Albedo = 6,
			Normals = 7,
			Compose = 8,
			Compare = 9,
			FinalGIDebug = 10,
			Specular = 11,
			Copy = 12,
			WideFilter = 13,
			Depth = 14,
			CopyDepth = 15,
			RSM_Debug = 16,
			RSM = 17,
			NFO = 18,
			NFOBlur = 19
		}

		private static class ShaderParams
		{
			public static int MainTex = Shader.PropertyToID("_MainTex");

			public static int DownscaledColorAndDepthRT = Shader.PropertyToID("_DownscaledColorAndDepthRT");

			public static int ResolveRT = Shader.PropertyToID("_ResolveRT");

			public static int SourceSize = Shader.PropertyToID("_SourceSize");

			public static int NoiseTex = Shader.PropertyToID("_NoiseTex");

			public static int Downscaled1RT = Shader.PropertyToID("_Downscaled1RT");

			public static int Downscaled1RTA = Shader.PropertyToID("_Downscaled1RTA");

			public static int Downscaled2RT = Shader.PropertyToID("_Downscaled2RT");

			public static int Downscaled2RTA = Shader.PropertyToID("_Downscaled2RTA");

			public static int InputRT = Shader.PropertyToID("_InputRTGI");

			public static int CompareTex = Shader.PropertyToID("_CompareTexGI");

			public static int TempAcum = Shader.PropertyToID("_TempAcum");

			public static int PrevResolve = Shader.PropertyToID("_PrevResolve");

			public static int DownscaledDepthRT = Shader.PropertyToID("_DownscaledDepthRT");

			public static int Probe1Cube = Shader.PropertyToID("_Probe1Cube");

			public static int Probe2Cube = Shader.PropertyToID("_Probe2Cube");

			public static int NFO_RT = Shader.PropertyToID("_NFO_RT");

			public static int NFOBlurRT = Shader.PropertyToID("_NFOBlurRT");

			public static int IndirectData = Shader.PropertyToID("_IndirectData");

			public static int RayData = Shader.PropertyToID("_RayData");

			public static int TemporalData = Shader.PropertyToID("_TemporalData");

			public static int WorldToViewDir = Shader.PropertyToID("_WorldToViewDir");

			public static int CompareParams = Shader.PropertyToID("_CompareParams");

			public static int ExtraData = Shader.PropertyToID("_ExtraData");

			public static int ExtraData2 = Shader.PropertyToID("_ExtraData2");

			public static int ExtraData3 = Shader.PropertyToID("_ExtraData3");

			public static int ExtraData4 = Shader.PropertyToID("_ExtraData4");

			public static int ExtraData5 = Shader.PropertyToID("_ExtraData5");

			public static int EmittersPositions = Shader.PropertyToID("_EmittersPositions");

			public static int EmittersBoxMin = Shader.PropertyToID("_EmittersBoxMin");

			public static int EmittersBoxMax = Shader.PropertyToID("_EmittersBoxMax");

			public static int EmittersColors = Shader.PropertyToID("_EmittersColors");

			public static int EmittersCount = Shader.PropertyToID("_EmittersCount");

			public static int RSMIntensity = Shader.PropertyToID("_RadiantShadowMapIntensity");

			public static int StencilValue = Shader.PropertyToID("_StencilValue");

			public static int StencilCompareFunction = Shader.PropertyToID("_StencilCompareFunction");

			public static int SubstractLightingMultiplier = Shader.PropertyToID("_ExtraData4");

			public static int ProbeData = Shader.PropertyToID("_ProbeData");

			public static int Probe1HDR = Shader.PropertyToID("_Probe1HDR");

			public static int Probe2HDR = Shader.PropertyToID("_Probe2HDR");

			public static int BoundsXZ = Shader.PropertyToID("_BoundsXZ");

			public static int DebugDepthMultiplier = Shader.PropertyToID("_DebugDepthMultiplier");

			public static int NFOTint = Shader.PropertyToID("_NFOTint");

			public const string SKW_FORWARD = "_FORWARD";

			public const string SKW_FORWARD_AND_DEFERRED = "_FORWARD_AND_DEFERRED";

			public const string SKW_COMPARE_MODE = "_COMPARE_MODE";

			public const string SKW_USES_BINARY_SEARCH = "_USES_BINARY_SEARCH";

			public const string SKW_USES_MULTIPLE_RAYS = "_USES_MULTIPLE_RAYS";

			public const string SKW_REUSE_RAYS = "_REUSE_RAYS";

			public const string SKW_FALLBACK_1_PROBE = "_FALLBACK_1_PROBE";

			public const string SKW_FALLBACK_2_PROBES = "_FALLBACK_2_PROBES";

			public const string SKW_VIRTUAL_EMITTERS = "_VIRTUAL_EMITTERS";

			public const string SKW_USES_NEAR_FIELD_OBSCURANCE = "_USES_NEAR_FIELD_OBSCURANCE";

			public const string SKW_ORTHO_SUPPORT = "_ORTHO_SUPPORT";
		}

		private class RadiantPass : ScriptableRenderPass
		{
			private class PerCameraData
			{
				public Vector3 lastCameraPosition;

				public RenderTexture rtAcum;

				public int rtAcumCreationFrame;

				public RenderTexture rtBounce;

				public int rtBounceCreationFrame;

				public float emittersSortTime;

				public Vector3 emittersLastCameraPosition;

				public readonly List<RadiantVirtualEmitter> emittersSorted = new List<RadiantVirtualEmitter>();
			}

			public int computedGIRT;

			private const string RGI_CBUF_NAME = "RadiantGI";

			private const float GOLDEN_RATIO = 0.618034f;

			private const int MAX_EMITTERS = 32;

			private ScriptableRenderer renderer;

			private RadiantRenderFeature settings;

			private RenderTextureDescriptor sourceDesc;

			private RenderTextureDescriptor cameraTargetDesc;

			private readonly Dictionary<Camera, PerCameraData> prevs = new Dictionary<Camera, PerCameraData>();

			private RadiantGlobalIllumination radiant;

			private float goldenRatioAcum;

			private bool usesReprojection;

			private bool usesCompareMode;

			private Vector3 camPos;

			private Volume[] volumes;

			private Material mat;

			private static readonly Vector4 unlimitedBounds = new Vector4(-100000000f, -100000000f, 100000000f, 100000000f);

			private Vector4[] emittersBoxMin;

			private Vector4[] emittersBoxMax;

			private Vector4[] emittersColors;

			private Vector4[] emittersPositions;

			private readonly Plane[] cameraPlanes = new Plane[6];

			public bool Setup(ScriptableRenderer renderer, RadiantRenderFeature settings, bool isSceneView)
			{
				radiant = VolumeManager.instance.stack.GetComponent<RadiantGlobalIllumination>();
				if (radiant == null || !radiant.IsActive())
				{
					return false;
				}
				usesReprojection = radiant.temporalReprojection.value && (!isSceneView || Application.isPlaying);
				usesCompareMode = radiant.compareMode.value && !isSceneView;
				base.renderPassEvent = settings.renderPassEvent;
				this.renderer = renderer;
				this.settings = settings;
				if (mat == null)
				{
					mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Kronnect/RadiantGI_URP"));
					mat.SetTexture(ShaderParams.NoiseTex, Resources.Load<Texture>("RadiantGI/blueNoiseGI128RA"));
				}
				mat.SetInt(ShaderParams.StencilValue, radiant.stencilValue.value);
				mat.SetInt(ShaderParams.StencilCompareFunction, (int)(radiant.stencilCheck.value ? radiant.stencilCompareFunction.value : CompareFunction.Always));
				return true;
			}

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Color;
				if (settings.renderingPath == RenderingPath.Forward)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal;
				}
				if (usesReprojection)
				{
					scriptableRenderPassInput |= ScriptableRenderPassInput.Motion;
				}
				ConfigureInput(scriptableRenderPassInput);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				sourceDesc = renderingData.cameraData.cameraTargetDescriptor;
				sourceDesc.colorFormat = RenderTextureFormat.ARGBHalf;
				sourceDesc.useMipMap = false;
				sourceDesc.msaaSamples = 1;
				sourceDesc.depthBufferBits = 0;
				cameraTargetDesc = sourceDesc;
				float value = radiant.downsampling.value;
				sourceDesc.width = (int)((float)sourceDesc.width / value);
				sourceDesc.height = (int)((float)sourceDesc.height / value);
				Camera camera = renderingData.cameraData.camera;
				camPos = camera.transform.position;
				CommandBuffer commandBuffer = CommandBufferPool.Get("RadiantGI");
				commandBuffer.Clear();
				RenderGI(commandBuffer, camera);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			private void RenderGI(CommandBuffer cmd, Camera cam)
			{
				RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
				int value = radiant.smoothing.value;
				RadiantGlobalIllumination.DebugView value2 = radiant.debugView.value;
				bool value3 = radiant.rayBounce.value;
				int num = (Application.isPlaying ? Time.frameCount : 0);
				bool num2 = settings.renderingPath != RenderingPath.Deferred;
				float value4 = radiant.normalMapInfluence.value;
				float w = ((radiant.lumaInfluence.value > 0f) ? (radiant.lumaInfluence.value * 100f) : 20000f);
				float value5 = radiant.downsampling.value;
				int frameCount = Time.frameCount;
				bool flag = RadiantShadowMap.installed && radiant.fallbackReflectiveShadowMap.value;
				bool flag2 = radiant.virtualEmitters.value;
				mat.SetVector(ShaderParams.IndirectData, new Vector4(radiant.indirectIntensity.value, radiant.indirectMaxSourceBrightness.value, radiant.indirectDistanceAttenuation.value, radiant.rayReuse.value));
				mat.SetVector(ShaderParams.RayData, new Vector4(radiant.rayCount.value, radiant.rayMaxLength.value, radiant.rayMaxSamples.value, radiant.thickness.value));
				cmd.SetGlobalVector(ShaderParams.ExtraData2, new Vector4(radiant.brightnessThreshold.value, radiant.brightnessMax.value, radiant.saturation.value, radiant.reflectiveShadowMapIntensity.value));
				mat.DisableKeyword("_FORWARD_AND_DEFERRED");
				mat.DisableKeyword("_FORWARD");
				if (num2)
				{
					if (settings.renderingPath == RenderingPath.Both)
					{
						mat.EnableKeyword("_FORWARD_AND_DEFERRED");
					}
					else
					{
						mat.EnableKeyword("_FORWARD");
					}
				}
				if (radiant.rayBinarySearch.value)
				{
					mat.EnableKeyword("_USES_BINARY_SEARCH");
				}
				else
				{
					mat.DisableKeyword("_USES_BINARY_SEARCH");
				}
				if (radiant.rayCount.value > 1)
				{
					mat.EnableKeyword("_USES_MULTIPLE_RAYS");
				}
				else
				{
					mat.DisableKeyword("_USES_MULTIPLE_RAYS");
				}
				float value6 = radiant.nearFieldObscurance.value;
				bool flag3 = value6 > 0f;
				if (flag3)
				{
					cmd.SetGlobalVector(ShaderParams.ExtraData4, new Vector4(radiant.nearFieldObscuranceMaxCameraDistance.value, (1f - radiant.nearFieldObscuranceOccluderDistance.value) * 10f, 0f, 0f));
					cmd.SetGlobalColor(ShaderParams.NFOTint, radiant.nearFieldObscuranceTintColor.value);
					mat.EnableKeyword("_USES_NEAR_FIELD_OBSCURANCE");
				}
				else
				{
					mat.DisableKeyword("_USES_NEAR_FIELD_OBSCURANCE");
				}
				if (cam.orthographic)
				{
					mat.EnableKeyword("_ORTHO_SUPPORT");
				}
				else
				{
					mat.DisableKeyword("_ORTHO_SUPPORT");
				}
				cmd.SetGlobalVector(ShaderParams.ExtraData3, new Vector4(radiant.aoInfluence.value, radiant.nearFieldObscuranceSpread.value * 0.5f, 1f / (radiant.nearCameraAttenuation.value + 0.0001f), value6));
				SetupVolumeBounds(cmd, cam);
				if (usesReprojection)
				{
					goldenRatioAcum += 0.618034f * (float)radiant.rayCount.value;
					goldenRatioAcum %= 5000f;
				}
				cmd.SetGlobalVector(ShaderParams.SourceSize, new Vector4(cameraTargetDesc.width, cameraTargetDesc.height, goldenRatioAcum, num));
				cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1f, value4, w));
				cmd.SetGlobalFloat(ShaderParams.ExtraData5, radiant.specularContribution.value);
				cmd.SetGlobalMatrix(ShaderParams.WorldToViewDir, cam.worldToCameraMatrix);
				RenderTextureDescriptor renderTextureDescriptor = cameraTargetDesc;
				renderTextureDescriptor.width = Mathf.Min(sourceDesc.width, renderTextureDescriptor.width / 2);
				renderTextureDescriptor.height = Mathf.Min(sourceDesc.height, renderTextureDescriptor.height / 2);
				int width = renderTextureDescriptor.width;
				int height = renderTextureDescriptor.height;
				int num3 = 9 - radiant.raytracerAccuracy.value;
				RenderTextureDescriptor desc = sourceDesc;
				desc.width = Mathf.CeilToInt((float)desc.width / (float)num3);
				desc.height = Mathf.CeilToInt((float)desc.height / (float)num3);
				desc.colorFormat = RenderTextureFormat.RHalf;
				desc.sRGB = false;
				cmd.GetTemporaryRT(ShaderParams.DownscaledDepthRT, desc, FilterMode.Point);
				FullScreenBlit(cmd, ShaderParams.DownscaledDepthRT, Pass.CopyDepth);
				switch (value2)
				{
				case RadiantGlobalIllumination.DebugView.Albedo:
					FullScreenBlit(cmd, cameraColorTargetHandle, Pass.Albedo);
					return;
				case RadiantGlobalIllumination.DebugView.Normals:
					FullScreenBlit(cmd, cameraColorTargetHandle, Pass.Normals);
					return;
				case RadiantGlobalIllumination.DebugView.Specular:
					FullScreenBlit(cmd, cameraColorTargetHandle, Pass.Specular);
					return;
				case RadiantGlobalIllumination.DebugView.Depth:
					mat.SetFloat(ShaderParams.DebugDepthMultiplier, radiant.debugDepthMultiplier.value);
					FullScreenBlit(cmd, cameraColorTargetHandle, Pass.Depth);
					return;
				}
				if (!prevs.TryGetValue(cam, out var value7))
				{
					value7 = (prevs[cam] = new PerCameraData());
				}
				RenderTexture renderTexture = value7.rtBounce;
				RenderTargetIdentifier source = cameraColorTargetHandle;
				if (value3)
				{
					if (renderTexture != null && (renderTexture.width != cameraTargetDesc.width || renderTexture.height != cameraTargetDesc.height))
					{
						renderTexture.Release();
						renderTexture = null;
					}
					if (renderTexture == null)
					{
						renderTexture = new RenderTexture(cameraTargetDesc);
						renderTexture.Create();
						value7.rtBounce = renderTexture;
						value7.rtBounceCreationFrame = frameCount;
					}
					else if (frameCount - value7.rtBounceCreationFrame > 2)
					{
						source = renderTexture;
					}
				}
				else if (renderTexture != null)
				{
					renderTexture.Release();
					UnityEngine.Object.DestroyImmediate(renderTexture);
				}
				if (flag2)
				{
					float time = Time.time;
					if (emittersForceRefresh || time - value7.emittersSortTime > 5f || (value7.emittersLastCameraPosition - camPos).sqrMagnitude > 25f)
					{
						if (emittersForceRefresh)
						{
							emittersForceRefresh = false;
							foreach (PerCameraData value8 in prevs.Values)
							{
								value8.emittersSortTime = float.MinValue;
							}
						}
						value7.emittersSortTime = time;
						value7.emittersLastCameraPosition = camPos;
						SortEmitters(cam);
						value7.emittersSorted.Clear();
						value7.emittersSorted.AddRange(emitters);
					}
					flag2 = SetupEmitters(cam, value7.emittersSorted);
				}
				if (flag2)
				{
					mat.EnableKeyword("_VIRTUAL_EMITTERS");
				}
				else
				{
					mat.DisableKeyword("_VIRTUAL_EMITTERS");
				}
				mat.DisableKeyword("_REUSE_RAYS");
				mat.DisableKeyword("_FALLBACK_1_PROBE");
				mat.DisableKeyword("_FALLBACK_2_PROBES");
				bool flag4 = false;
				if (radiant.fallbackReflectionProbes.value && SetupProbes(cmd, cam, out var numProbes))
				{
					mat.EnableKeyword((numProbes == 1) ? "_FALLBACK_1_PROBE" : "_FALLBACK_2_PROBES");
					flag4 = true;
				}
				if (!flag4 && radiant.fallbackReuseRays.value && frameCount - value7.rtAcumCreationFrame > 2 && radiant.rayReuse.value > 0f && value7.rtAcum != null)
				{
					cmd.SetGlobalTexture(value: new RenderTargetIdentifier(value7.rtAcum, 0, CubemapFace.Unknown, -1), nameID: ShaderParams.PrevResolve);
					mat.EnableKeyword("_REUSE_RAYS");
				}
				RenderTextureDescriptor desc2 = sourceDesc;
				cmd.GetTemporaryRT(ShaderParams.DownscaledColorAndDepthRT, desc2, FilterMode.Bilinear);
				cmd.GetTemporaryRT(ShaderParams.ResolveRT, sourceDesc, FilterMode.Bilinear);
				FullScreenBlit(cmd, source, ShaderParams.ResolveRT, Pass.Raycast);
				cmd.GetTemporaryRT(ShaderParams.Downscaled1RT, renderTextureDescriptor, FilterMode.Bilinear);
				cmd.GetTemporaryRT(ShaderParams.Downscaled1RTA, renderTextureDescriptor, FilterMode.Bilinear);
				if (flag3)
				{
					RenderTextureDescriptor desc3 = renderTextureDescriptor;
					desc3.colorFormat = RenderTextureFormat.RHalf;
					cmd.GetTemporaryRT(ShaderParams.NFO_RT, desc3, FilterMode.Bilinear);
					cmd.GetTemporaryRT(ShaderParams.NFOBlurRT, desc3, FilterMode.Bilinear);
					FullScreenBlit(cmd, ShaderParams.NFOBlurRT, Pass.NFO);
					FullScreenBlit(cmd, ShaderParams.NFOBlurRT, ShaderParams.NFO_RT, Pass.NFOBlur);
				}
				renderTextureDescriptor.width /= 2;
				renderTextureDescriptor.height /= 2;
				cmd.GetTemporaryRT(ShaderParams.Downscaled2RT, renderTextureDescriptor, FilterMode.Bilinear);
				int num4 = ShaderParams.Downscaled2RT;
				switch (value)
				{
				case 0:
					if (value5 <= 1f)
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
						FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.WideFilter);
					}
					else
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RT, Pass.WideFilter);
					}
					break;
				case 1:
					cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, renderTextureDescriptor, FilterMode.Bilinear);
					if (value5 <= 1f)
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
						FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RTA, Pass.Copy);
					}
					else
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RTA, Pass.Copy);
					}
					if (flag)
					{
						FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
					}
					FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
					break;
				case 2:
					cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, renderTextureDescriptor, FilterMode.Bilinear);
					if (value5 <= 1f)
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
						FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
						FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
						if (flag)
						{
							FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
						}
						FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
					}
					else
					{
						FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
						FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
						if (flag)
						{
							FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
						}
						FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
					}
					break;
				case 4:
					cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, renderTextureDescriptor, FilterMode.Bilinear);
					FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RTA, Pass.BlurHorizontal);
					FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, ShaderParams.Downscaled1RT, Pass.BlurVertical);
					FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
					FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
					if (flag)
					{
						FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
					}
					FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
					cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1.25f, value4, w));
					FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.WideFilter);
					num4 = ShaderParams.Downscaled2RTA;
					break;
				default:
					cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, renderTextureDescriptor, FilterMode.Bilinear);
					FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RTA, Pass.BlurHorizontal);
					FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, ShaderParams.Downscaled1RT, Pass.BlurVertical);
					FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
					FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
					if (flag)
					{
						FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
					}
					FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
					break;
				}
				FullScreenBlit(cmd, num4, ShaderParams.Downscaled1RTA, Pass.Upscale);
				computedGIRT = ShaderParams.Downscaled1RTA;
				RenderTexture renderTexture2 = value7?.rtAcum;
				if (usesReprojection)
				{
					if (renderTexture2 != null && (renderTexture2.width != width || renderTexture2.height != height))
					{
						renderTexture2.Release();
						renderTexture2 = null;
					}
					RenderTextureDescriptor desc4 = sourceDesc;
					desc4.width = width;
					desc4.height = height;
					float num5 = radiant.temporalResponseSpeed.value;
					Pass pass = Pass.TemporalAccum;
					if (renderTexture2 == null)
					{
						renderTexture2 = new RenderTexture(desc4);
						renderTexture2.Create();
						value7.rtAcum = renderTexture2;
						value7.lastCameraPosition = camPos;
						value7.rtAcumCreationFrame = frameCount;
						pass = Pass.Copy;
					}
					else
					{
						float num6 = Vector3.Distance(camPos, value7.lastCameraPosition);
						value7.lastCameraPosition = camPos;
						num5 += num6 * radiant.temporalCameraTranslationResponse.value;
					}
					mat.SetVector(ShaderParams.TemporalData, new Vector4(num5, radiant.temporalDepthRejection.value, radiant.temporalChromaThreshold.value, 0f));
					RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(renderTexture2, 0, CubemapFace.Unknown, -1);
					cmd.SetGlobalTexture(ShaderParams.PrevResolve, renderTargetIdentifier);
					cmd.GetTemporaryRT(ShaderParams.TempAcum, desc4, FilterMode.Bilinear);
					FullScreenBlit(cmd, computedGIRT, ShaderParams.TempAcum, pass);
					FullScreenBlit(cmd, ShaderParams.TempAcum, renderTargetIdentifier, Pass.CopyExact);
					computedGIRT = ShaderParams.TempAcum;
				}
				else if (renderTexture2 != null)
				{
					renderTexture2.Release();
					UnityEngine.Object.DestroyImmediate(renderTexture2);
				}
				cmd.GetTemporaryRT(ShaderParams.InputRT, (usesCompareMode || flag3) ? cameraTargetDesc : sourceDesc, FilterMode.Point);
				FullScreenBlit(cmd, cameraColorTargetHandle, ShaderParams.InputRT, Pass.CopyExact);
				if (usesCompareMode)
				{
					cmd.GetTemporaryRT(ShaderParams.CompareTex, cameraTargetDesc, FilterMode.Point);
					if (value3)
					{
						FullScreenBlit(cmd, computedGIRT, ShaderParams.CompareTex, Pass.Compose);
						FullScreenBlit(cmd, ShaderParams.CompareTex, renderTexture, Pass.CopyExact);
					}
				}
				else if (value3)
				{
					FullScreenBlit(cmd, computedGIRT, renderTexture, Pass.Compose);
					FullScreenBlitToCamera(cmd, renderTexture, Pass.CopyExact);
				}
				else
				{
					FullScreenBlitToCamera(cmd, computedGIRT, Pass.Compose);
				}
				switch (value2)
				{
				case RadiantGlobalIllumination.DebugView.DownscaledHalf:
					FullScreenBlit(cmd, ShaderParams.Downscaled1RT, cameraColorTargetHandle, Pass.CopyExact);
					break;
				case RadiantGlobalIllumination.DebugView.DownscaledQuarter:
					FullScreenBlit(cmd, num4, cameraColorTargetHandle, Pass.CopyExact);
					break;
				case RadiantGlobalIllumination.DebugView.UpscaleToHalf:
					FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, cameraColorTargetHandle, Pass.CopyExact);
					break;
				case RadiantGlobalIllumination.DebugView.Raycast:
					FullScreenBlit(cmd, ShaderParams.ResolveRT, cameraColorTargetHandle, Pass.CopyExact);
					break;
				case RadiantGlobalIllumination.DebugView.ReflectiveShadowMap:
					if (flag)
					{
						FullScreenBlit(cmd, cameraColorTargetHandle, Pass.RSM_Debug);
					}
					break;
				case RadiantGlobalIllumination.DebugView.TemporalAccumulationBuffer:
					if (usesReprojection)
					{
						FullScreenBlit(cmd, ShaderParams.TempAcum, cameraColorTargetHandle, Pass.CopyExact);
					}
					break;
				case RadiantGlobalIllumination.DebugView.FinalGI:
					FullScreenBlit(cmd, computedGIRT, cameraColorTargetHandle, Pass.FinalGIDebug);
					break;
				}
			}

			private void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier destination, Pass pass)
			{
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
			}

			private void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Pass pass)
			{
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.SetGlobalTexture(ShaderParams.MainTex, source);
				cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
			}

			private void FullScreenBlitToCamera(CommandBuffer cmd, RenderTargetIdentifier source, Pass pass)
			{
				RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
				cmd.SetRenderTarget(cameraColorTargetHandle, 0, CubemapFace.Unknown, -1);
				cmd.SetGlobalTexture(ShaderParams.MainTex, source);
				cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
			}

			private float CalculateProbeWeight(Vector3 wpos, Vector3 probeBoxMin, Vector3 probeBoxMax, float blendDistance)
			{
				Vector3 vector = Vector3.Min(wpos - probeBoxMin, probeBoxMax - wpos) / blendDistance;
				return Mathf.Clamp01(Mathf.Min(vector.x, Mathf.Min(vector.y, vector.z)));
			}

			private bool SetupProbes(CommandBuffer cmd, Camera cam, out int numProbes)
			{
				numProbes = PickNearProbes(cam, out var probe, out var probe2);
				if (numProbes == 0)
				{
					return false;
				}
				if (!probe.bounds.Contains(camPos))
				{
					return false;
				}
				if (numProbes >= 2 && !probe2.bounds.Contains(camPos))
				{
					numProbes = 1;
				}
				float num = 0f;
				float num2 = 0f;
				if (numProbes >= 1)
				{
					Shader.SetGlobalTexture(ShaderParams.Probe1Cube, probe.texture);
					Shader.SetGlobalVector(ShaderParams.Probe1HDR, probe.textureHDRDecodeValues);
					Bounds bounds = probe.bounds;
					num = CalculateProbeWeight(camPos, bounds.min, bounds.max, probe.blendDistance);
				}
				if (numProbes >= 2)
				{
					Shader.SetGlobalTexture(ShaderParams.Probe2Cube, probe2.texture);
					Shader.SetGlobalVector(ShaderParams.Probe2HDR, probe.textureHDRDecodeValues);
					Bounds bounds2 = probe2.bounds;
					num2 = CalculateProbeWeight(camPos, bounds2.min, bounds2.max, probe2.blendDistance);
				}
				float value = radiant.probesIntensity.value;
				cmd.SetGlobalVector(ShaderParams.ProbeData, new Vector4(num * value, num2 * value, 0f, 0f));
				return true;
			}

			private int PickNearProbes(Camera cam, out ReflectionProbe probe1, out ReflectionProbe probe2)
			{
				int count = probes.Count;
				probe1 = (probe2 = null);
				switch (count)
				{
				case 0:
					return 0;
				case 1:
					probe1 = probes[0];
					return 1;
				default:
				{
					float num = float.MaxValue;
					float num2 = float.MaxValue;
					for (int i = 0; i < count; i++)
					{
						ReflectionProbe reflectionProbe = probes[i];
						float num3 = ComputeProbeValue(camPos, reflectionProbe);
						if (num3 < num2)
						{
							probe2 = reflectionProbe;
							num2 = num3;
							if (num2 < num)
							{
								num3 = num;
								reflectionProbe = probe1;
								probe1 = probe2;
								num = num2;
								probe2 = reflectionProbe;
								num2 = num3;
							}
						}
					}
					return 2;
				}
				}
			}

			private float ComputeProbeValue(Vector3 camPos, ReflectionProbe probe)
			{
				float num = (probe.transform.position - camPos).sqrMagnitude * (float)(probe.importance + 1) * 1000f;
				if (!probe.bounds.Contains(camPos))
				{
					num += 100000f;
				}
				return num;
			}

			private void SetupVolumeBounds(CommandBuffer cmd, Camera cam)
			{
				if (!radiant.limitToVolumeBounds.value)
				{
					cmd.SetGlobalVector(ShaderParams.BoundsXZ, unlimitedBounds);
					return;
				}
				if (volumes == null)
				{
					volumes = VolumeManager.instance.GetVolumes(-1);
				}
				int num = volumes.Length;
				for (int i = 0; i < num; i++)
				{
					List<Collider> colliders = volumes[i].colliders;
					Volume volume = volumes[i];
					int count = colliders.Count;
					for (int j = 0; j < count; j++)
					{
						Bounds bounds = colliders[i].bounds;
						if (colliders[j].bounds.Contains(camPos) && volume.sharedProfile.Has<RadiantGlobalIllumination>())
						{
							cmd.SetGlobalVector(value: new Vector4(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z), nameID: ShaderParams.BoundsXZ);
							return;
						}
					}
				}
			}

			private bool SetupEmitters(Camera cam, List<RadiantVirtualEmitter> emitters)
			{
				if (emittersBoxMax == null || emittersBoxMax.Length != 32)
				{
					emittersBoxMax = new Vector4[32];
					emittersBoxMin = new Vector4[32];
					emittersColors = new Vector4[32];
					emittersPositions = new Vector4[32];
				}
				int num = 0;
				int num2 = Mathf.Min(150, emitters.Count);
				GeometryUtility.CalculateFrustumPlanes(cam, cameraPlanes);
				for (int i = 0; i < num2; i++)
				{
					RadiantVirtualEmitter radiantVirtualEmitter = emitters[i];
					if (radiantVirtualEmitter == null || !radiantVirtualEmitter.isActiveAndEnabled || radiantVirtualEmitter.intensity <= 0f || radiantVirtualEmitter.range <= 0f)
					{
						continue;
					}
					Vector4 gIColorAndRange = radiantVirtualEmitter.GetGIColorAndRange();
					if (gIColorAndRange.x == 0f && gIColorAndRange.y == 0f && gIColorAndRange.z == 0f)
					{
						continue;
					}
					Bounds bounds = radiantVirtualEmitter.GetBounds();
					if (GeometryUtility.TestPlanesAABB(cameraPlanes, bounds))
					{
						Vector3 position = radiantVirtualEmitter.transform.position;
						emittersPositions[num] = position;
						emittersColors[num] = gIColorAndRange;
						Vector3 min = bounds.min;
						Vector3 max = bounds.max;
						float b = gIColorAndRange.w * gIColorAndRange.w;
						float w = 1f / Mathf.Max(0.0001f, b);
						emittersBoxMin[num] = new Vector4(min.x, min.y, min.z, w);
						emittersBoxMax[num] = new Vector4(max.x, max.y, max.z, 0f);
						num++;
						if (num >= 32)
						{
							break;
						}
					}
				}
				if (num == 0)
				{
					return false;
				}
				Shader.SetGlobalVectorArray(ShaderParams.EmittersPositions, emittersPositions);
				Shader.SetGlobalVectorArray(ShaderParams.EmittersBoxMin, emittersBoxMin);
				Shader.SetGlobalVectorArray(ShaderParams.EmittersBoxMax, emittersBoxMax);
				Shader.SetGlobalVectorArray(ShaderParams.EmittersColors, emittersColors);
				Shader.SetGlobalInt(ShaderParams.EmittersCount, num);
				return true;
			}

			private void SortEmitters(Camera cam)
			{
				emitters.Sort(EmittersDistanceComparer);
			}

			private int EmittersDistanceComparer(RadiantVirtualEmitter p1, RadiantVirtualEmitter p2)
			{
				Vector3 position = p1.transform.position;
				Vector3 position2 = p2.transform.position;
				float num = (position - camPos).sqrMagnitude;
				float num2 = (position2 - camPos).sqrMagnitude;
				Bounds bounds = p1.GetBounds();
				Bounds bounds2 = p2.GetBounds();
				if (!bounds.Contains(camPos))
				{
					num += 100000f;
				}
				if (!bounds2.Contains(camPos))
				{
					num2 += 100000f;
				}
				if (num < num2)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
				return 0;
			}

			public void Cleanup()
			{
				CoreUtils.Destroy(mat);
				if (prevs == null)
				{
					return;
				}
				foreach (PerCameraData value in prevs.Values)
				{
					if (value.rtAcum != null)
					{
						value.rtAcum.Release();
						UnityEngine.Object.DestroyImmediate(value.rtAcum);
					}
					if (value.rtBounce != null)
					{
						value.rtBounce.Release();
						UnityEngine.Object.DestroyImmediate(value.rtBounce);
					}
				}
				prevs.Clear();
			}
		}

		private class RadiantComparePass : ScriptableRenderPass
		{
			private const string RGI_CBUF_NAME = "RadiantGICompare";

			private Material mat;

			private RadiantGlobalIllumination radiant;

			private ScriptableRenderer renderer;

			private RadiantPass radiantPass;

			private RadiantRenderFeature settings;

			public bool Setup(ScriptableRenderer renderer, RadiantRenderFeature settings, RadiantPass radiantPass)
			{
				radiant = VolumeManager.instance.stack.GetComponent<RadiantGlobalIllumination>();
				if (radiant == null || !radiant.IsActive() || radiant.debugView.value != RadiantGlobalIllumination.DebugView.None)
				{
					return false;
				}
				if (!radiant.compareMode.value)
				{
					return false;
				}
				base.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				this.settings = settings;
				this.renderer = renderer;
				this.radiantPass = radiantPass;
				if (mat == null)
				{
					mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Kronnect/RadiantGI_URP"));
				}
				return true;
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get("RadiantGICompare");
				commandBuffer.Clear();
				mat.DisableKeyword("_FORWARD_AND_DEFERRED");
				mat.DisableKeyword("_FORWARD");
				if (settings.renderingPath == RenderingPath.Both)
				{
					mat.EnableKeyword("_FORWARD_AND_DEFERRED");
				}
				else if (settings.renderingPath == RenderingPath.Forward)
				{
					mat.EnableKeyword("_FORWARD");
				}
				if (radiant.virtualEmitters.value)
				{
					mat.EnableKeyword("_VIRTUAL_EMITTERS");
				}
				else
				{
					mat.DisableKeyword("_VIRTUAL_EMITTERS");
				}
				if (radiant.nearFieldObscurance.value > 0f)
				{
					mat.EnableKeyword("_USES_NEAR_FIELD_OBSCURANCE");
				}
				else
				{
					mat.DisableKeyword("_USES_NEAR_FIELD_OBSCURANCE");
				}
				float f = (radiant.compareSameSide.value ? (MathF.PI / 2f) : radiant.compareLineAngle.value);
				mat.SetVector(ShaderParams.CompareParams, new Vector4(Mathf.Cos(f), Mathf.Sin(f), radiant.compareSameSide.value ? radiant.comparePanning.value : (-10f), radiant.compareLineWidth.value));
				mat.SetInt(ShaderParams.StencilValue, radiant.stencilValue.value);
				mat.SetInt(ShaderParams.StencilCompareFunction, (int)(radiant.stencilCheck.value ? radiant.stencilCompareFunction.value : CompareFunction.Always));
				RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
				FullScreenBlit(commandBuffer, cameraColorTargetHandle, ShaderParams.InputRT, Pass.CopyExact);
				FullScreenBlit(commandBuffer, radiantPass.computedGIRT, ShaderParams.CompareTex, Pass.Compose);
				FullScreenBlitToCamera(commandBuffer, ShaderParams.InputRT, Pass.Compare);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			private void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Pass pass)
			{
				cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
				cmd.SetGlobalTexture(ShaderParams.MainTex, source);
				cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
			}

			private void FullScreenBlitToCamera(CommandBuffer cmd, RenderTargetIdentifier source, Pass pass)
			{
				RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
				RTHandle cameraDepthTargetHandle = renderer.cameraDepthTargetHandle;
				cmd.SetRenderTarget(cameraColorTargetHandle, cameraDepthTargetHandle, 0, CubemapFace.Unknown, -1);
				cmd.SetGlobalTexture(ShaderParams.MainTex, source);
				cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
			}

			public void Cleanup()
			{
				CoreUtils.Destroy(mat);
			}
		}

		private static readonly List<ReflectionProbe> probes = new List<ReflectionProbe>();

		private static readonly List<RadiantVirtualEmitter> emitters = new List<RadiantVirtualEmitter>();

		private static bool emittersForceRefresh;

		private static Mesh _fullScreenMesh;

		public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

		[Tooltip("Select the rendering mode according to the URP asset.")]
		public RenderingPath renderingPath = RenderingPath.Deferred;

		[Tooltip("Allows Radiant to be executed even if camera has Post Processing option disabled.")]
		public bool ignorePostProcessingOption = true;

		private RadiantPass radiantPass;

		private RadiantComparePass comparePass;

		public static bool needRTRefresh;

		private static Mesh fullscreenMesh
		{
			get
			{
				if (_fullScreenMesh != null)
				{
					return _fullScreenMesh;
				}
				float y = 1f;
				float y2 = 0f;
				_fullScreenMesh = new Mesh();
				_fullScreenMesh.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(1f, 1f, 0f)
				});
				_fullScreenMesh.SetUVs(0, new List<Vector2>
				{
					new Vector2(0f, y2),
					new Vector2(0f, y),
					new Vector2(1f, y2),
					new Vector2(1f, y)
				});
				_fullScreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, calculateBounds: false);
				_fullScreenMesh.UploadMeshData(markNoLongerReadable: true);
				return _fullScreenMesh;
			}
		}

		private void OnDisable()
		{
			if (radiantPass != null)
			{
				radiantPass.Cleanup();
			}
			if (comparePass != null)
			{
				comparePass.Cleanup();
			}
		}

		public override void Create()
		{
			radiantPass = new RadiantPass();
			comparePass = new RadiantComparePass();
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!renderingData.cameraData.postProcessEnabled && !ignorePostProcessingOption)
			{
				return;
			}
			Camera camera = renderingData.cameraData.camera;
			bool flag = camera.cameraType == CameraType.SceneView;
			if ((camera.cameraType == CameraType.Game || flag) && renderingData.cameraData.renderType == CameraRenderType.Base && radiantPass.Setup(renderer, this, flag))
			{
				renderer.EnqueuePass(radiantPass);
				if (!flag && comparePass.Setup(renderer, this, radiantPass))
				{
					renderer.EnqueuePass(comparePass);
				}
			}
		}

		public static void RegisterReflectionProbe(ReflectionProbe probe)
		{
			if (!(probe == null) && !probes.Contains(probe))
			{
				probes.Add(probe);
			}
		}

		public static void UnregisterReflectionProbe(ReflectionProbe probe)
		{
			if (!(probe == null) && probes.Contains(probe))
			{
				probes.Remove(probe);
			}
		}

		public static void RegisterVirtualEmitter(RadiantVirtualEmitter emitter)
		{
			if (!(emitter == null) && !emitters.Contains(emitter))
			{
				emitters.Add(emitter);
				emittersForceRefresh = true;
			}
		}

		public static void UnregisterVirtualEmitter(RadiantVirtualEmitter emitter)
		{
			if (!(emitter == null) && emitters.Contains(emitter))
			{
				emitters.Remove(emitter);
				emittersForceRefresh = true;
			}
		}
	}
}
