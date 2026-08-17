using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace CorgiGodRays
{
	public class GodRaysRenderPass : ScriptableRenderPass
	{
		private struct VisibleLightRemap
		{
			public VisibleLight lightData;

			public int visibleLightIndex;
		}

		private const string _profilerTag = "GodRaysRenderPass";

		private static readonly int _GodRaysTexture = Shader.PropertyToID("_GodRaysTexture");

		private static readonly int _CorgiGrabpass = Shader.PropertyToID("_CorgiGrabpass");

		private static readonly int _CopyBlitTex = Shader.PropertyToID("_CopyBlitTex");

		private static readonly int _TempBlurTex = Shader.PropertyToID("_TempBlurTex");

		private static readonly int _BlurInputTex = Shader.PropertyToID("_BlurInputTex");

		private static readonly int _CorgiDepthGrabpassFullRes = Shader.PropertyToID("_CorgiDepthGrabpassFullRes");

		private static readonly int _CorgiDepthGrabpassNonFullRes = Shader.PropertyToID("_CorgiDepthGrabpassNonFullRes");

		private static readonly int _CorgiInverseProjection = Shader.PropertyToID("_CorgiInverseProjection");

		private static readonly int _CorgiCameraToWorld = Shader.PropertyToID("_CorgiCameraToWorld");

		private static readonly int _CorgiInverseProjectionArray = Shader.PropertyToID("_CorgiInverseProjectionArray");

		private static readonly int _GodRaysParams = Shader.PropertyToID("_GodRaysParams");

		private static readonly int _MainLightScattering = Shader.PropertyToID("_MainLightScattering");

		private static readonly int _AdditionalLightScattering = Shader.PropertyToID("_AdditionalLightScattering");

		private static readonly int _Jitter = Shader.PropertyToID("_Jitter");

		private static readonly int _MaxDistance = Shader.PropertyToID("_MaxDistance");

		private static readonly int _MainLightIntensity = Shader.PropertyToID("_MainLightIntensity");

		private static readonly int _AdditionalLightIntensity = Shader.PropertyToID("_AdditionalLightIntensity");

		private static readonly int _TintColor = Shader.PropertyToID("_TintColor");

		private static readonly int _CorgiVisibleLightCount = Shader.PropertyToID("_CorgiVisibleLightCount");

		private static readonly int _CorgiVisibleLightData = Shader.PropertyToID("_CorgiVisibleLightData");

		private static readonly int _CorgiLightIndexToShadowIndex = Shader.PropertyToID("_CorgiLightIndexToShadowIndex");

		private static readonly int _CorgiGodraysIntensityCurveTexture = Shader.PropertyToID("_CorgiGodraysIntensityCurveTexture");

		[NonSerialized]
		private GodRaysRenderFeature.GodRaysSettings _settings;

		[NonSerialized]
		private ScriptableRenderer _renderer;

		[NonSerialized]
		private Matrix4x4[] _InverseProjectionArray = new Matrix4x4[2];

		[NonSerialized]
		private MaterialPropertyBlock _propertyBlock;

		[NonSerialized]
		private Texture2D _intensityCurveTexture;

		[NonSerialized]
		private PropertyInfo _cacheRenderFeaturesPropertyInfo;

		[NonSerialized]
		private static GraphicsBuffer _additionalLightsBuffer;

		[NonSerialized]
		private static GraphicsBuffer _lightsToShadowIndexBuffer;

		[NonSerialized]
		private const int MaxLightCount = 256;

		private MixedLightingSetup m_MixedLightingSetup;

		private int _temporal_pass_index;

		private bool _curveTextureForceRefreshTrigger;

		private static Mesh s_FullscreenMesh;

		public static Mesh fullscreenMesh
		{
			get
			{
				if (s_FullscreenMesh != null)
				{
					return s_FullscreenMesh;
				}
				float y = 1f;
				float y2 = 0f;
				s_FullscreenMesh = new Mesh
				{
					name = "Fullscreen Quad"
				};
				s_FullscreenMesh.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(1f, 1f, 0f)
				});
				s_FullscreenMesh.SetUVs(0, new List<Vector2>
				{
					new Vector2(0f, y2),
					new Vector2(0f, y),
					new Vector2(1f, y2),
					new Vector2(1f, y)
				});
				s_FullscreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, calculateBounds: false);
				s_FullscreenMesh.UploadMeshData(markNoLongerReadable: true);
				return s_FullscreenMesh;
			}
		}

		public void Setup(GodRaysRenderFeature.GodRaysSettings settings, ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			_settings = settings;
			_renderer = renderer;
			_propertyBlock = new MaterialPropertyBlock();
			switch (settings.renderOrder)
			{
			case GodRaysRenderFeature.GodraysRenderOrder.AfterOpaque:
				base.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
				break;
			case GodRaysRenderFeature.GodraysRenderOrder.AfterTransparent:
				base.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				break;
			case GodRaysRenderFeature.GodraysRenderOrder.Custom:
				base.renderPassEvent = settings.customRenderPassEvent + settings.customRenderPassOffset;
				break;
			}
			ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Depth;
			if (_settings.temporallyRender && _settings.temporalReprojection)
			{
				scriptableRenderPassInput |= ScriptableRenderPassInput.Motion;
			}
			ConfigureInput(scriptableRenderPassInput);
		}

		public void Initialize()
		{
			Dispose();
			_additionalLightsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 256, Marshal.SizeOf<ShaderInput.LightData>());
			_lightsToShadowIndexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 256, Marshal.SizeOf<int>());
		}

		public void Dispose()
		{
			if (_additionalLightsBuffer != null)
			{
				_additionalLightsBuffer.Dispose();
			}
			if (_lightsToShadowIndexBuffer != null)
			{
				_lightsToShadowIndexBuffer.Dispose();
			}
			DisposeCurveTexture();
		}

		private void DisposeCurveTexture()
		{
			if (_intensityCurveTexture != null)
			{
				UnityEngine.Object.Destroy(_intensityCurveTexture);
				_intensityCurveTexture = null;
			}
		}

		private void InitializeLightConstants(NativeArray<VisibleLight> lights, int lightIndex, out Vector4 lightPos, out Vector4 lightColor, out Vector4 lightAttenuation, out Vector4 lightSpotDir, out Vector4 lightOcclusionProbeChannel, out uint lightLayerMask, out bool isSubtractive)
		{
			UniversalRenderPipeline.InitializeLightConstants_Common(lights, lightIndex, out lightPos, out lightColor, out lightAttenuation, out lightSpotDir, out lightOcclusionProbeChannel);
			lightLayerMask = 0u;
			isSubtractive = false;
			if (lightIndex < 0)
			{
				return;
			}
			ref VisibleLight reference = ref UnsafeElementAtMutable(lights, lightIndex);
			Light light = reference.light;
			LightBakingOutput bakingOutput = light.bakingOutput;
			isSubtractive = bakingOutput.isBaked && bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && bakingOutput.mixedLightingMode == MixedLightingMode.Subtractive;
			if (light == null)
			{
				return;
			}
			if (bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && reference.light.shadows != LightShadows.None && m_MixedLightingSetup == MixedLightingSetup.None)
			{
				switch (bakingOutput.mixedLightingMode)
				{
				case MixedLightingMode.Subtractive:
					m_MixedLightingSetup = MixedLightingSetup.Subtractive;
					break;
				case MixedLightingMode.Shadowmask:
					m_MixedLightingSetup = MixedLightingSetup.ShadowMask;
					break;
				}
			}
			UniversalAdditionalLightData universalAdditionalLightData = light.GetUniversalAdditionalLightData();
			lightLayerMask = ToValidRenderingLayers(universalAdditionalLightData.renderingLayers);
		}

		public static uint ToValidRenderingLayers(uint renderingLayers)
		{
			return 2147483647u;
		}

		public unsafe static ref T UnsafeElementAtMutable<T>(NativeArray<T> array, int index) where T : struct
		{
			return ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), index);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			UniversalRenderer universalRenderer = _renderer as UniversalRenderer;
			if (_settings == null || !renderingData.postProcessingEnabled)
			{
				return;
			}
			if (VolumeManager.instance == null)
			{
				Debug.LogWarning("VolumeManager.instance is null.");
				return;
			}
			if (VolumeManager.instance.stack == null)
			{
				Debug.LogWarning("VolumeManager.instance.stack is null.");
				return;
			}
			GodRaysVolume component = VolumeManager.instance.stack.GetComponent<GodRaysVolume>();
			if (component == null || (component.MainLightIntensity.value == 0f && component.AdditionalLightsIntensity.value == 0f) || (!_settings.allowMainLight && !_settings.allowAdditionalLights))
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get("GodRaysRenderPass");
			commandBuffer.Clear();
			if (_settings.allowAdditionalLights && (int)_settings.AdditionalLightLayers != 0)
			{
				AdditionalLightsShadowCasterPass additionalLightsShadowCasterPass = (AdditionalLightsShadowCasterPass)universalRenderer.GetType().GetField("m_AdditionalLightsShadowCasterPass", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(universalRenderer);
				NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
				int length = visibleLights.Length;
				int num = 0;
				NativeArray<ShaderInput.LightData> data = new NativeArray<ShaderInput.LightData>(length, Allocator.Temp);
				NativeArray<int> data2 = new NativeArray<int>(length, Allocator.Temp);
				int maxAdditionalLightCount = _settings.maxAdditionalLightCount;
				List<VisibleLightRemap> list = new List<VisibleLightRemap>(renderingData.lightData.visibleLights.Length);
				for (int i = 0; i < renderingData.lightData.visibleLights.Length; i++)
				{
					list.Add(new VisibleLightRemap
					{
						lightData = renderingData.lightData.visibleLights[i],
						visibleLightIndex = i
					});
				}
				Vector3 cameraPosition = renderingData.cameraData.camera.transform.position;
				list.Sort(delegate(VisibleLightRemap a, VisibleLightRemap b)
				{
					float num8 = Vector3.Distance(a.lightData.light.transform.position, cameraPosition);
					float value2 = Vector3.Distance(b.lightData.light.transform.position, cameraPosition);
					return num8.CompareTo(value2);
				});
				int num2 = 0;
				int num3 = 0;
				ShaderInput.LightData value = default(ShaderInput.LightData);
				for (; num2 < list.Count; num2++)
				{
					if (num3 >= 256)
					{
						break;
					}
					VisibleLightRemap visibleLightRemap = list[num2];
					VisibleLight lightData = visibleLightRemap.lightData;
					int layer = lightData.light.gameObject.layer;
					int num4 = 1 << layer;
					int visibleLightIndex = visibleLightRemap.visibleLightIndex;
					if ((num4 & _settings.AdditionalLightLayers.value) != 0 && renderingData.lightData.mainLightIndex != visibleLightIndex)
					{
						InitializeLightConstants(visibleLights, visibleLightIndex, out value.position, out value.color, out value.attenuation, out value.spotDirection, out value.occlusionProbeChannels, out value.layerMask, out var _);
						data[num3] = value;
						data2[num3] = additionalLightsShadowCasterPass.GetShadowLightIndexFromLightIndex(visibleLightIndex);
						num3++;
						num++;
						if (num >= maxAdditionalLightCount)
						{
							break;
						}
					}
				}
				_additionalLightsBuffer.SetData(data);
				_lightsToShadowIndexBuffer.SetData(data2);
				commandBuffer.SetGlobalBuffer(_CorgiVisibleLightData, _additionalLightsBuffer);
				commandBuffer.SetGlobalBuffer(_CorgiLightIndexToShadowIndex, _lightsToShadowIndexBuffer);
				commandBuffer.SetGlobalFloat(_CorgiVisibleLightCount, num);
				data.Dispose();
				data2.Dispose();
			}
			if (_settings.allowAdditionalLightShadows)
			{
				commandBuffer.EnableShaderKeyword("GODRAYS_ADDITIVE_LIGHT_SHADOWS");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("GODRAYS_ADDITIVE_LIGHT_SHADOWS");
			}
			bool flag = false;
			if (_settings.supportUnityScreenSpaceShadows)
			{
				if (_cacheRenderFeaturesPropertyInfo == null)
				{
					Type type = _renderer.GetType();
					_cacheRenderFeaturesPropertyInfo = type.GetProperty("rendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (_cacheRenderFeaturesPropertyInfo != null && _cacheRenderFeaturesPropertyInfo.GetValue(_renderer, null) is List<ScriptableRendererFeature> list2)
				{
					flag = list2.FindIndex((ScriptableRendererFeature other) => other != null && other.isActive && other.name == "ScreenSpaceShadows") != -1;
				}
			}
			if (flag)
			{
				ShadowData shadowData = renderingData.shadowData;
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", shadowData.mainLightShadowCascadesCount > 0);
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_CASCADE", shadowData.mainLightShadowCascadesCount == 0);
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_SCREEN", state: false);
			}
			if (_settings.useVariableIntensity)
			{
				EnsureCurveTexture();
				commandBuffer.SetGlobalTexture(_CorgiGodraysIntensityCurveTexture, _intensityCurveTexture);
				commandBuffer.EnableShaderKeyword("GODRAYS_VARIABLE_INTENSITY");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("GODRAYS_VARIABLE_INTENSITY");
			}
			if (_settings.allowMainLight)
			{
				commandBuffer.EnableShaderKeyword("GODRAYS_MAIN_LIGHT");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("GODRAYS_MAIN_LIGHT");
			}
			Matrix4x4 inverse = GL.GetGPUProjectionMatrix(renderingData.cameraData.GetProjectionMatrix(), renderIntoTexture: false).inverse;
			commandBuffer.SetGlobalMatrix(_CorgiCameraToWorld, renderingData.cameraData.camera.cameraToWorldMatrix);
			commandBuffer.SetGlobalMatrix(_CorgiInverseProjection, inverse);
			if (renderingData.cameraData.cameraTargetDescriptor.volumeDepth > 1)
			{
				for (int num5 = 0; num5 < 2; num5++)
				{
					Matrix4x4 inverse2 = GL.GetGPUProjectionMatrix(renderingData.cameraData.GetProjectionMatrix(num5), renderIntoTexture: false).inverse;
					_InverseProjectionArray[num5] = inverse2;
				}
				commandBuffer.SetGlobalMatrixArray(_CorgiInverseProjectionArray, _InverseProjectionArray);
			}
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.msaaSamples = 1;
			cameraTargetDescriptor.bindMS = false;
			commandBuffer.GetTemporaryRT(_CorgiGrabpass, cameraTargetDescriptor);
			RTHandle cameraColorTargetHandle = _renderer.cameraColorTargetHandle;
			RTHandle cameraDepthTargetHandle = _renderer.cameraDepthTargetHandle;
			commandBuffer.SetGlobalTexture(_CopyBlitTex, cameraColorTargetHandle);
			commandBuffer.SetRenderTarget(_CorgiGrabpass, 0, CubemapFace.Unknown, -1);
			commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.Grabpass, 0, 0);
			if (_settings.useUnityDepthDirectly)
			{
				commandBuffer.EnableShaderKeyword("_GODRAYS_USE_UNITY_DEPTH");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("_GODRAYS_USE_UNITY_DEPTH");
			}
			RenderTextureDescriptor desc = cameraTargetDescriptor;
			desc.colorFormat = RenderTextureFormat.RFloat;
			desc.depthBufferBits = 0;
			desc.depthStencilFormat = GraphicsFormat.None;
			desc.msaaSamples = 1;
			desc.bindMS = false;
			commandBuffer.SetGlobalTexture(_CopyBlitTex, cameraDepthTargetHandle);
			commandBuffer.GetTemporaryRT(_CorgiDepthGrabpassFullRes, desc);
			commandBuffer.SetRenderTarget(_CorgiDepthGrabpassFullRes, 0, CubemapFace.Unknown, -1);
			commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.DepthGrabpass, 0, 0);
			commandBuffer.SetGlobalTexture(_CorgiDepthGrabpassFullRes, _CorgiDepthGrabpassFullRes);
			RenderTextureDescriptor renderTextureDescriptor = cameraTargetDescriptor;
			if (_settings.temporallyRender && _settings.temporalReprojection)
			{
				renderTextureDescriptor.enableRandomWrite = true;
			}
			if (_settings.encodeLightColor)
			{
				commandBuffer.EnableShaderKeyword("GODRAYS_ENCODE_LIGHT_COLOR");
				renderTextureDescriptor.colorFormat = (_settings.enableHighQualityTextures ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf);
			}
			else
			{
				commandBuffer.DisableShaderKeyword("GODRAYS_ENCODE_LIGHT_COLOR");
				renderTextureDescriptor.colorFormat = (_settings.enableHighQualityTextures ? RenderTextureFormat.RFloat : RenderTextureFormat.RHalf);
			}
			int textureQuality = (int)_settings.textureQuality;
			renderTextureDescriptor.width /= textureQuality;
			renderTextureDescriptor.height /= textureQuality;
			commandBuffer.SetGlobalVector(_GodRaysParams, new Vector4(renderTextureDescriptor.width, renderTextureDescriptor.height, 1f / (float)renderTextureDescriptor.width, 1f / (float)renderTextureDescriptor.height));
			if (_settings.depthAwareUpsampling && _settings.textureQuality != GodRaysRenderFeature.VolumeTextureQuality.High)
			{
				RenderTextureDescriptor desc2 = renderTextureDescriptor;
				desc2.colorFormat = RenderTextureFormat.RFloat;
				desc2.msaaSamples = 1;
				desc2.bindMS = false;
				commandBuffer.SetGlobalTexture(_CopyBlitTex, cameraDepthTargetHandle);
				commandBuffer.GetTemporaryRT(_CorgiDepthGrabpassNonFullRes, desc2);
				commandBuffer.SetRenderTarget(_CorgiDepthGrabpassNonFullRes, 0, CubemapFace.Unknown, -1);
				commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.DepthGrabpass, 0, 0);
			}
			_propertyBlock.Clear();
			_propertyBlock.SetFloat(_MainLightScattering, component.MainLightScattering.value);
			_propertyBlock.SetFloat(_AdditionalLightScattering, component.AdditionalLightsScattering.value);
			_propertyBlock.SetFloat(_MainLightIntensity, component.MainLightIntensity.value);
			_propertyBlock.SetFloat(_AdditionalLightIntensity, component.AdditionalLightsIntensity.value);
			_propertyBlock.SetFloat(_Jitter, _settings.Jitter);
			_propertyBlock.SetFloat(_MaxDistance, _settings.maxDistance);
			if (_settings.stepQuality == GodRaysRenderFeature.VolumeStepQuality.Low)
			{
				commandBuffer.EnableShaderKeyword("VOLUME_STEPS_LOW");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("VOLUME_STEPS_LOW");
			}
			if (_settings.stepQuality == GodRaysRenderFeature.VolumeStepQuality.Med)
			{
				commandBuffer.EnableShaderKeyword("VOLUME_STEPS_MED");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("VOLUME_STEPS_MED");
			}
			if (_settings.stepQuality == GodRaysRenderFeature.VolumeStepQuality.High)
			{
				commandBuffer.EnableShaderKeyword("VOLUME_STEPS_HIGH");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("VOLUME_STEPS_HIGH");
			}
			if (_settings.allowAdditionalLights)
			{
				commandBuffer.EnableShaderKeyword("GODRAYS_ADDITIVE_LIGHTS");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("GODRAYS_ADDITIVE_LIGHTS");
			}
			commandBuffer.GetTemporaryRT(_GodRaysTexture, renderTextureDescriptor);
			if (_settings.temporallyRender)
			{
				if (_settings.temporalUseDiscard)
				{
					commandBuffer.EnableShaderKeyword("GODRAYS_DISCARD_TEMPORAL");
				}
				else
				{
					commandBuffer.DisableShaderKeyword("GODRAYS_DISCARD_TEMPORAL");
				}
				_temporal_pass_index++;
				if (_temporal_pass_index >= _settings.temporalDuration)
				{
					_temporal_pass_index = 0;
				}
				_propertyBlock.SetInt("_CorgiTemporallyRendered", 1);
				_propertyBlock.SetInt("_CorgiTemporalPassIndex", _temporal_pass_index);
				_propertyBlock.SetInt("_CorgiTemporalPassCount", _settings.temporalDuration);
				if (_settings.temporallyRender && _settings.temporalReprojection)
				{
					int num6 = Shader.PropertyToID("_TemporalGodrays");
					commandBuffer.GetTemporaryRT(num6, renderTextureDescriptor);
					commandBuffer.CopyTexture(_GodRaysTexture, 0, 0, num6, 0, 0);
					ComputeShader temporalReprojectionShader = _settings.renderData.TemporalReprojectionShader;
					int kernelIndex = 0;
					commandBuffer.SetComputeTextureParam(temporalReprojectionShader, kernelIndex, "Input", num6);
					commandBuffer.SetComputeTextureParam(temporalReprojectionShader, kernelIndex, "Output", _GodRaysTexture);
					commandBuffer.SetComputeIntParam(temporalReprojectionShader, "texture_width", renderTextureDescriptor.width);
					commandBuffer.SetComputeIntParam(temporalReprojectionShader, "texture_height", renderTextureDescriptor.height);
					commandBuffer.SetComputeIntParam(temporalReprojectionShader, "TemporalPassIndex", _temporal_pass_index);
					commandBuffer.SetComputeIntParam(temporalReprojectionShader, "TemporalPassCount", _settings.temporalDuration);
					commandBuffer.SetComputeTextureParam(temporalReprojectionShader, kernelIndex, "_CameraMotionVectorsTexture", "_MotionVectorTexture");
					commandBuffer.SetComputeVectorParam(temporalReprojectionShader, "_CameraMotionVectorsTexture_Resolution", new Vector4(cameraTargetDescriptor.width, cameraTargetDescriptor.height));
					commandBuffer.DispatchCompute(temporalReprojectionShader, kernelIndex, renderTextureDescriptor.width / 32, renderTextureDescriptor.height / 32, 1);
				}
			}
			else
			{
				_propertyBlock.SetInt("_CorgiTemporallyRendered", 0);
			}
			_propertyBlock.SetVector("_CorgiScreenParams", new Vector4(renderTextureDescriptor.width, renderTextureDescriptor.height, 1f / (float)renderTextureDescriptor.width, 1f / (float)renderTextureDescriptor.height));
			commandBuffer.SetRenderTarget(_GodRaysTexture, 0, CubemapFace.Unknown, -1);
			commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.ScreenSpaceGodRays, 0, 0, _propertyBlock);
			if (_settings.blur)
			{
				commandBuffer.GetTemporaryRT(_TempBlurTex, renderTextureDescriptor);
				if (_settings.blurSamples == GodRaysRenderFeature.BilateralBlurSamples.Low)
				{
					commandBuffer.EnableShaderKeyword("SAMPLE_COUNT_LOW");
				}
				else
				{
					commandBuffer.DisableShaderKeyword("SAMPLE_COUNT_LOW");
				}
				if (_settings.blurSamples == GodRaysRenderFeature.BilateralBlurSamples.Med)
				{
					commandBuffer.EnableShaderKeyword("SAMPLE_COUNT_MED");
				}
				else
				{
					commandBuffer.DisableShaderKeyword("SAMPLE_COUNT_MED");
				}
				if (_settings.blurSamples == GodRaysRenderFeature.BilateralBlurSamples.High)
				{
					commandBuffer.EnableShaderKeyword("SAMPLE_COUNT_HIGH");
				}
				else
				{
					commandBuffer.DisableShaderKeyword("SAMPLE_COUNT_HIGH");
				}
				for (int num7 = 0; num7 < _settings.BlurCount; num7++)
				{
					commandBuffer.SetGlobalTexture(_BlurInputTex, _GodRaysTexture);
					commandBuffer.SetRenderTarget(_TempBlurTex, 0, CubemapFace.Unknown, -1);
					commandBuffer.EnableShaderKeyword("BLUR_X");
					commandBuffer.DisableShaderKeyword("BLUR_Y");
					commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.BilateralBlur, 0, 0);
					commandBuffer.SetGlobalTexture(_BlurInputTex, _TempBlurTex);
					commandBuffer.SetRenderTarget(_GodRaysTexture, 0, CubemapFace.Unknown, -1);
					commandBuffer.DisableShaderKeyword("BLUR_X");
					commandBuffer.EnableShaderKeyword("BLUR_Y");
					commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.BilateralBlur, 0, 0);
				}
				commandBuffer.ReleaseTemporaryRT(_TempBlurTex);
			}
			if (_settings.depthAwareUpsampling && _settings.textureQuality != GodRaysRenderFeature.VolumeTextureQuality.High)
			{
				commandBuffer.SetGlobalTexture(_CorgiDepthGrabpassNonFullRes, _CorgiDepthGrabpassNonFullRes);
			}
			if (_settings.depthAwareUpsampling && _settings.textureQuality != GodRaysRenderFeature.VolumeTextureQuality.High)
			{
				commandBuffer.EnableShaderKeyword("DEPTH_AWARE_UPSAMPLE");
			}
			else
			{
				commandBuffer.DisableShaderKeyword("DEPTH_AWARE_UPSAMPLE");
			}
			_propertyBlock.Clear();
			_propertyBlock.SetColor(_TintColor, component.Tint.value);
			commandBuffer.SetGlobalTexture(_GodRaysTexture, _GodRaysTexture);
			commandBuffer.SetGlobalTexture(_CopyBlitTex, _CorgiGrabpass);
			commandBuffer.SetRenderTarget(cameraColorTargetHandle, 0, CubemapFace.Unknown, -1);
			commandBuffer.DrawMesh(fullscreenMesh, Matrix4x4.identity, _settings.renderData.ApplyGodRays, 0, 0, _propertyBlock);
			commandBuffer.ReleaseTemporaryRT(_CorgiGrabpass);
			commandBuffer.ReleaseTemporaryRT(_CorgiDepthGrabpassFullRes);
			commandBuffer.ReleaseTemporaryRT(_GodRaysTexture);
			if (_settings.depthAwareUpsampling && _settings.textureQuality != GodRaysRenderFeature.VolumeTextureQuality.High)
			{
				commandBuffer.ReleaseTemporaryRT(_CorgiDepthGrabpassNonFullRes);
			}
			if (flag)
			{
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", state: false);
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_CASCADE", state: false);
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_SCREEN", state: true);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		public void TriggerRefreshCurveTexture()
		{
			_curveTextureForceRefreshTrigger = true;
		}

		private void EnsureCurveTexture()
		{
			if (_curveTextureForceRefreshTrigger)
			{
				_curveTextureForceRefreshTrigger = false;
				DisposeCurveTexture();
			}
			if (!(_intensityCurveTexture != null))
			{
				_intensityCurveTexture = new Texture2D(64, 1, TextureFormat.RFloat, mipChain: false, linear: true);
				NativeArray<float> pixelData = _intensityCurveTexture.GetPixelData<float>(0);
				for (int i = 0; i < 64; i++)
				{
					float time = (float)i / 64f;
					pixelData[i] = _settings.variableIntensityCurve.Evaluate(time);
				}
				_intensityCurveTexture.SetPixelData(pixelData, 0);
				_intensityCurveTexture.Apply();
			}
		}
	}
}
