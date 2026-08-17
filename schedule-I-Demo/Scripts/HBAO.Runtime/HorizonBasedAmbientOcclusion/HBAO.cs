using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace HorizonBasedAmbientOcclusion
{
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[AddComponentMenu("Image Effects/HBAO")]
	[RequireComponent(typeof(Camera))]
	public class HBAO : MonoBehaviour
	{
		public enum Preset
		{
			FastestPerformance = 0,
			FastPerformance = 1,
			Normal = 2,
			HighQuality = 3,
			HighestQuality = 4,
			Custom = 5
		}

		public enum PipelineStage
		{
			BeforeImageEffectsOpaque = 0,
			AfterLighting = 1,
			BeforeReflections = 2
		}

		public enum Quality
		{
			Lowest = 0,
			Low = 1,
			Medium = 2,
			High = 3,
			Highest = 4
		}

		public enum Resolution
		{
			Full = 0,
			Half = 1
		}

		public enum NoiseType
		{
			Dither = 0,
			InterleavedGradientNoise = 1,
			SpatialDistribution = 2
		}

		public enum Deinterleaving
		{
			Disabled = 0,
			x4 = 1
		}

		public enum DebugMode
		{
			Disabled = 0,
			AOOnly = 1,
			ColorBleedingOnly = 2,
			SplitWithoutAOAndWithAO = 3,
			SplitWithAOAndAOOnly = 4,
			SplitWithoutAOAndAOOnly = 5,
			ViewNormals = 6
		}

		public enum BlurType
		{
			None = 0,
			Narrow = 1,
			Medium = 2,
			Wide = 3,
			ExtraWide = 4
		}

		public enum PerPixelNormals
		{
			GBuffer = 0,
			Camera = 1,
			Reconstruct = 2
		}

		public enum VarianceClipping
		{
			Disabled = 0,
			_4Tap = 1,
			_8Tap = 2
		}

		[Serializable]
		public struct Presets
		{
			public Preset preset;

			[SerializeField]
			public static Presets defaults => new Presets
			{
				preset = Preset.Normal
			};
		}

		[Serializable]
		public struct GeneralSettings
		{
			[Tooltip("The stage the AO is injected into the rendering pipeline.")]
			[Space(6f)]
			public PipelineStage pipelineStage;

			[Tooltip("The quality of the AO.")]
			[Space(10f)]
			public Quality quality;

			[Tooltip("The deinterleaving factor.")]
			public Deinterleaving deinterleaving;

			[Tooltip("The resolution at which the AO is calculated.")]
			public Resolution resolution;

			[Tooltip("The type of noise to use.")]
			[Space(10f)]
			public NoiseType noiseType;

			[Tooltip("The debug mode actually displayed on screen.")]
			[Space(10f)]
			public DebugMode debugMode;

			[SerializeField]
			public static GeneralSettings defaults => new GeneralSettings
			{
				pipelineStage = PipelineStage.BeforeImageEffectsOpaque,
				quality = Quality.Medium,
				deinterleaving = Deinterleaving.Disabled,
				resolution = Resolution.Full,
				noiseType = NoiseType.Dither,
				debugMode = DebugMode.Disabled
			};
		}

		[Serializable]
		public struct AOSettings
		{
			[Tooltip("AO radius: this is the distance outside which occluders are ignored.")]
			[Space(6f)]
			[Range(0.25f, 5f)]
			public float radius;

			[Tooltip("Maximum radius in pixels: this prevents the radius to grow too much with close-up object and impact on performances.")]
			[Range(16f, 256f)]
			public float maxRadiusPixels;

			[Tooltip("For low-tessellated geometry, occlusion variations tend to appear at creases and ridges, which betray the underlying tessellation. To remove these artifacts, we use an angle bias parameter which restricts the hemisphere.")]
			[Range(0f, 0.5f)]
			public float bias;

			[Tooltip("This value allows to scale up the ambient occlusion values.")]
			[Range(0f, 4f)]
			public float intensity;

			[Tooltip("Enable/disable MultiBounce approximation.")]
			public bool useMultiBounce;

			[Tooltip("MultiBounce approximation influence.")]
			[Range(0f, 1f)]
			public float multiBounceInfluence;

			[Tooltip("The amount of AO offscreen samples are contributing.")]
			[Range(0f, 1f)]
			public float offscreenSamplesContribution;

			[Tooltip("The max distance to display AO.")]
			[Space(10f)]
			public float maxDistance;

			[Tooltip("The distance before max distance at which AO start to decrease.")]
			public float distanceFalloff;

			[Tooltip("The type of per pixel normals to use.")]
			[Space(10f)]
			public PerPixelNormals perPixelNormals;

			[Tooltip("This setting allow you to set the base color if the AO, the alpha channel value is unused.")]
			[Space(10f)]
			public Color baseColor;

			[SerializeField]
			public static AOSettings defaults => new AOSettings
			{
				radius = 0.8f,
				maxRadiusPixels = 128f,
				bias = 0.05f,
				intensity = 1f,
				useMultiBounce = false,
				multiBounceInfluence = 1f,
				offscreenSamplesContribution = 0f,
				maxDistance = 150f,
				distanceFalloff = 50f,
				perPixelNormals = PerPixelNormals.GBuffer,
				baseColor = Color.black
			};
		}

		[Serializable]
		public struct TemporalFilterSettings
		{
			[Space(6f)]
			public bool enabled;

			[Tooltip("The type of variance clipping to use.")]
			public VarianceClipping varianceClipping;

			[SerializeField]
			public static TemporalFilterSettings defaults => new TemporalFilterSettings
			{
				enabled = false,
				varianceClipping = VarianceClipping._4Tap
			};
		}

		[Serializable]
		public struct BlurSettings
		{
			[Tooltip("The type of blur to use.")]
			[Space(6f)]
			public BlurType type;

			[Tooltip("This parameter controls the depth-dependent weight of the bilateral filter, to avoid bleeding across edges. A zero sharpness is a pure Gaussian blur. Increasing the blur sharpness removes bleeding by using lower weights for samples with large depth delta from the current pixel.")]
			[Space(10f)]
			[Range(0f, 16f)]
			public float sharpness;

			[SerializeField]
			public static BlurSettings defaults => new BlurSettings
			{
				type = BlurType.Medium,
				sharpness = 8f
			};
		}

		[Serializable]
		public struct ColorBleedingSettings
		{
			[Space(6f)]
			public bool enabled;

			[Tooltip("This value allows to control the saturation of the color bleeding.")]
			[Space(10f)]
			[Range(0f, 4f)]
			public float saturation;

			[Tooltip("This value allows to scale the contribution of the color bleeding samples.")]
			[Range(0f, 32f)]
			public float albedoMultiplier;

			[Tooltip("Use masking on emissive pixels")]
			[Range(0f, 1f)]
			public float brightnessMask;

			[Tooltip("Brightness level where masking starts/ends")]
			[MinMaxSlider(0f, 2f)]
			public Vector2 brightnessMaskRange;

			[SerializeField]
			public static ColorBleedingSettings defaults => new ColorBleedingSettings
			{
				enabled = false,
				saturation = 1f,
				albedoMultiplier = 4f,
				brightnessMask = 1f,
				brightnessMaskRange = new Vector2(0f, 0.5f)
			};
		}

		[AttributeUsage(AttributeTargets.Field)]
		public class SettingsGroup : Attribute
		{
		}

		public class MinMaxSliderAttribute : PropertyAttribute
		{
			public readonly float max;

			public readonly float min;

			public MinMaxSliderAttribute(float min, float max)
			{
				this.min = min;
				this.max = max;
			}
		}

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

			public const int Composite_AfterLighting = 10;

			public const int Composite_BeforeReflections = 11;

			public const int Composite_BlendAO = 12;

			public const int Composite_BlendCB = 13;

			public const int Debug_ViewNormals = 14;
		}

		private static class ShaderProperties
		{
			public static int mainTex;

			public static int hbaoTex;

			public static int tempTex;

			public static int tempTex2;

			public static int noiseTex;

			public static int depthTex;

			public static int normalsTex;

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

			static ShaderProperties()
			{
				mainTex = Shader.PropertyToID("_MainTex");
				hbaoTex = Shader.PropertyToID("_HBAOTex");
				tempTex = Shader.PropertyToID("_TempTex");
				tempTex2 = Shader.PropertyToID("_TempTex2");
				noiseTex = Shader.PropertyToID("_NoiseTex");
				depthTex = Shader.PropertyToID("_DepthTex");
				normalsTex = Shader.PropertyToID("_NormalsTex");
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
			}

			public static string GetOrthographicOrDeferredKeyword(bool orthographic, GeneralSettings settings)
			{
				if (!orthographic)
				{
					if (settings.pipelineStage == PipelineStage.BeforeImageEffectsOpaque)
					{
						return "__";
					}
					return "DEFERRED_SHADING";
				}
				return "ORTHOGRAPHIC_PROJECTION";
			}

			public static string GetQualityKeyword(GeneralSettings settings)
			{
				return settings.quality switch
				{
					Quality.Lowest => "QUALITY_LOWEST", 
					Quality.Low => "QUALITY_LOW", 
					Quality.Medium => "QUALITY_MEDIUM", 
					Quality.High => "QUALITY_HIGH", 
					Quality.Highest => "QUALITY_HIGHEST", 
					_ => "QUALITY_MEDIUM", 
				};
			}

			public static string GetNoiseKeyword(GeneralSettings settings)
			{
				return settings.noiseType switch
				{
					NoiseType.InterleavedGradientNoise => "INTERLEAVED_GRADIENT_NOISE", 
					_ => "__", 
				};
			}

			public static string GetDeinterleavingKeyword(GeneralSettings settings)
			{
				Deinterleaving deinterleaving = settings.deinterleaving;
				if (deinterleaving != Deinterleaving.Disabled && deinterleaving == Deinterleaving.x4)
				{
					return "DEINTERLEAVED";
				}
				return "__";
			}

			public static string GetDebugKeyword(GeneralSettings settings)
			{
				return settings.debugMode switch
				{
					DebugMode.AOOnly => "DEBUG_AO", 
					DebugMode.ColorBleedingOnly => "DEBUG_COLORBLEEDING", 
					DebugMode.SplitWithoutAOAndWithAO => "DEBUG_NOAO_AO", 
					DebugMode.SplitWithAOAndAOOnly => "DEBUG_AO_AOONLY", 
					DebugMode.SplitWithoutAOAndAOOnly => "DEBUG_NOAO_AOONLY", 
					_ => "__", 
				};
			}

			public static string GetMultibounceKeyword(AOSettings settings)
			{
				if (!settings.useMultiBounce)
				{
					return "__";
				}
				return "MULTIBOUNCE";
			}

			public static string GetOffscreenSamplesContributionKeyword(AOSettings settings)
			{
				if (!(settings.offscreenSamplesContribution > 0f))
				{
					return "__";
				}
				return "OFFSCREEN_SAMPLES_CONTRIBUTION";
			}

			public static string GetPerPixelNormalsKeyword(AOSettings settings)
			{
				return settings.perPixelNormals switch
				{
					PerPixelNormals.Camera => "NORMALS_CAMERA", 
					PerPixelNormals.Reconstruct => "NORMALS_RECONSTRUCT", 
					_ => "__", 
				};
			}

			public static string GetBlurRadiusKeyword(BlurSettings settings)
			{
				return settings.type switch
				{
					BlurType.Narrow => "BLUR_RADIUS_2", 
					BlurType.Medium => "BLUR_RADIUS_3", 
					BlurType.Wide => "BLUR_RADIUS_4", 
					BlurType.ExtraWide => "BLUR_RADIUS_5", 
					_ => "BLUR_RADIUS_3", 
				};
			}

			public static string GetVarianceClippingKeyword(TemporalFilterSettings settings)
			{
				return settings.varianceClipping switch
				{
					VarianceClipping._4Tap => "VARIANCE_CLIPPING_4TAP", 
					VarianceClipping._8Tap => "VARIANCE_CLIPPING_8TAP", 
					_ => "__", 
				};
			}

			public static string GetColorBleedingKeyword(ColorBleedingSettings settings)
			{
				if (!settings.enabled)
				{
					return "__";
				}
				return "COLOR_BLEEDING";
			}

			public static string GetLightingLogEncodedKeyword(bool hdr)
			{
				if (!hdr)
				{
					return "LIGHTING_LOG_ENCODED";
				}
				return "__";
			}
		}

		public enum StereoRenderingMode
		{
			MultiPass = 0,
			SinglePassInstanced = 1
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

		public Shader hbaoShader;

		[SerializeField]
		[SettingsGroup]
		private Presets m_Presets = Presets.defaults;

		[SerializeField]
		[SettingsGroup]
		private GeneralSettings m_GeneralSettings = GeneralSettings.defaults;

		[SerializeField]
		[SettingsGroup]
		private AOSettings m_AOSettings = AOSettings.defaults;

		[SerializeField]
		[SettingsGroup]
		private TemporalFilterSettings m_TemporalFilterSettings = TemporalFilterSettings.defaults;

		[SerializeField]
		[SettingsGroup]
		private BlurSettings m_BlurSettings = BlurSettings.defaults;

		[SerializeField]
		[SettingsGroup]
		private ColorBleedingSettings m_ColorBleedingSettings = ColorBleedingSettings.defaults;

		private static readonly Vector2[] s_jitter = new Vector2[16];

		private static readonly float[] s_temporalRotations = new float[6] { 60f, 300f, 180f, 240f, 120f, 0f };

		private static readonly float[] s_temporalOffsets = new float[4] { 0f, 0.5f, 0.25f, 0.75f };

		private RenderTextureDescriptor m_sourceDescriptor;

		private string[] m_ShaderKeywords;

		private Vector4[] m_UVToViewPerEye = new Vector4[2];

		private float[] m_RadiusPerEye = new float[2];

		private bool m_IsCommandBufferDirty;

		private Mesh m_FullscreenTriangle;

		private PipelineStage? m_PreviousPipelineStage;

		private Resolution? m_PreviousResolution;

		private Deinterleaving? m_PreviousDeinterleaving;

		private DebugMode? m_PreviousDebugMode;

		private NoiseType? m_PreviousNoiseType;

		private BlurType? m_PreviousBlurAmount;

		private int m_PreviousWidth;

		private int m_PreviousHeight;

		private bool m_PreviousAllowHDR;

		private bool m_PreviousUseMultibounce;

		private bool m_PreviousColorBleedingEnabled;

		private bool m_PreviousTemporalFilterEnabled;

		private RenderingPath m_PreviousRenderingPath;

		private StereoRenderingMode m_PrevStereoRenderingMode;

		public Presets presets
		{
			get
			{
				return m_Presets;
			}
			set
			{
				m_Presets = value;
			}
		}

		public GeneralSettings generalSettings
		{
			get
			{
				return m_GeneralSettings;
			}
			set
			{
				m_GeneralSettings = value;
			}
		}

		public AOSettings aoSettings
		{
			get
			{
				return m_AOSettings;
			}
			set
			{
				m_AOSettings = value;
			}
		}

		public TemporalFilterSettings temporalFilterSettings
		{
			get
			{
				return m_TemporalFilterSettings;
			}
			set
			{
				m_TemporalFilterSettings = value;
			}
		}

		public BlurSettings blurSettings
		{
			get
			{
				return m_BlurSettings;
			}
			set
			{
				m_BlurSettings = value;
			}
		}

		public ColorBleedingSettings colorBleedingSettings
		{
			get
			{
				return m_ColorBleedingSettings;
			}
			set
			{
				m_ColorBleedingSettings = value;
			}
		}

		private Material material { get; set; }

		private Camera hbaoCamera { get; set; }

		private CommandBuffer cmdBuffer { get; set; }

		private int width { get; set; }

		private int height { get; set; }

		private bool stereoActive { get; set; }

		private int xrActiveEye { get; set; }

		private StereoRenderingMode stereoRenderingMode { get; set; }

		private int screenWidth { get; set; }

		private int screenHeight { get; set; }

		private int aoWidth { get; set; }

		private int aoHeight { get; set; }

		private int reinterleavedAoWidth { get; set; }

		private int reinterleavedAoHeight { get; set; }

		private int deinterleavedAoWidth { get; set; }

		private int deinterleavedAoHeight { get; set; }

		private int frameCount { get; set; }

		private bool motionVectorsSupported { get; set; }

		private RenderTexture aoHistoryBuffer { get; set; }

		private RenderTexture colorBleedingHistoryBuffer { get; set; }

		private Texture2D noiseTex { get; set; }

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

		private CameraEvent cameraEvent
		{
			get
			{
				if (generalSettings.debugMode != DebugMode.Disabled)
				{
					return CameraEvent.BeforeImageEffectsOpaque;
				}
				return generalSettings.pipelineStage switch
				{
					PipelineStage.BeforeReflections => CameraEvent.BeforeReflections, 
					PipelineStage.AfterLighting => CameraEvent.AfterLighting, 
					_ => CameraEvent.BeforeImageEffectsOpaque, 
				};
			}
		}

		private bool isCommandBufferDirty
		{
			get
			{
				if (m_IsCommandBufferDirty || m_PreviousPipelineStage != generalSettings.pipelineStage || m_PreviousResolution != generalSettings.resolution || m_PreviousDebugMode != generalSettings.debugMode || m_PreviousAllowHDR != hbaoCamera.allowHDR || m_PreviousWidth != width || m_PreviousHeight != height || m_PreviousDeinterleaving != generalSettings.deinterleaving || m_PreviousBlurAmount != blurSettings.type || m_PreviousUseMultibounce != aoSettings.useMultiBounce || m_PreviousColorBleedingEnabled != colorBleedingSettings.enabled || m_PreviousTemporalFilterEnabled != temporalFilterSettings.enabled || m_PreviousRenderingPath != hbaoCamera.actualRenderingPath)
				{
					m_PreviousPipelineStage = generalSettings.pipelineStage;
					m_PreviousResolution = generalSettings.resolution;
					m_PreviousDebugMode = generalSettings.debugMode;
					m_PreviousAllowHDR = hbaoCamera.allowHDR;
					m_PreviousWidth = width;
					m_PreviousHeight = height;
					m_PreviousDeinterleaving = generalSettings.deinterleaving;
					m_PreviousBlurAmount = blurSettings.type;
					m_PreviousUseMultibounce = aoSettings.useMultiBounce;
					m_PreviousColorBleedingEnabled = colorBleedingSettings.enabled;
					m_PreviousTemporalFilterEnabled = temporalFilterSettings.enabled;
					m_PreviousRenderingPath = hbaoCamera.actualRenderingPath;
					return true;
				}
				return false;
			}
			set
			{
				m_IsCommandBufferDirty = value;
			}
		}

		private bool isHistoryBufferDirty
		{
			get
			{
				if (aoHistoryBuffer == null || (colorBleedingSettings.enabled && colorBleedingHistoryBuffer == null) || m_PreviousTemporalFilterEnabled != temporalFilterSettings.enabled || m_PreviousResolution != generalSettings.resolution || m_PreviousColorBleedingEnabled != colorBleedingSettings.enabled || m_PrevStereoRenderingMode != stereoRenderingMode)
				{
					m_PreviousTemporalFilterEnabled = temporalFilterSettings.enabled;
					m_PreviousResolution = generalSettings.resolution;
					m_PreviousColorBleedingEnabled = colorBleedingSettings.enabled;
					m_PrevStereoRenderingMode = stereoRenderingMode;
					return true;
				}
				return false;
			}
		}

		private static RenderTextureFormat defaultHDRRenderTextureFormat => RenderTextureFormat.DefaultHDR;

		private RenderTextureFormat sourceFormat
		{
			get
			{
				if (!hbaoCamera.allowHDR)
				{
					return RenderTextureFormat.Default;
				}
				return defaultHDRRenderTextureFormat;
			}
		}

		private static RenderTextureFormat colorFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
				{
					return RenderTextureFormat.Default;
				}
				return RenderTextureFormat.ARGBHalf;
			}
		}

		private static RenderTextureFormat depthFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat))
				{
					return RenderTextureFormat.RHalf;
				}
				return RenderTextureFormat.RFloat;
			}
		}

		private static RenderTextureFormat normalsFormat
		{
			get
			{
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010))
				{
					return RenderTextureFormat.Default;
				}
				return RenderTextureFormat.ARGB2101010;
			}
		}

		private static bool isLinearColorSpace => QualitySettings.activeColorSpace == ColorSpace.Linear;

		private bool renderingInSceneView => hbaoCamera.cameraType == CameraType.SceneView;

		public Preset GetCurrentPreset()
		{
			return m_Presets.preset;
		}

		public void ApplyPreset(Preset preset)
		{
			if (preset == Preset.Custom)
			{
				m_Presets.preset = preset;
				return;
			}
			DebugMode debugMode = generalSettings.debugMode;
			m_GeneralSettings = GeneralSettings.defaults;
			m_AOSettings = AOSettings.defaults;
			m_ColorBleedingSettings = ColorBleedingSettings.defaults;
			m_BlurSettings = BlurSettings.defaults;
			SetDebugMode(debugMode);
			switch (preset)
			{
			case Preset.FastestPerformance:
				SetQuality(Quality.Lowest);
				SetAoRadius(0.5f);
				SetAoMaxRadiusPixels(64f);
				SetBlurType(BlurType.ExtraWide);
				break;
			case Preset.FastPerformance:
				SetQuality(Quality.Low);
				SetAoRadius(0.5f);
				SetAoMaxRadiusPixels(64f);
				SetBlurType(BlurType.Wide);
				break;
			case Preset.HighQuality:
				SetQuality(Quality.High);
				SetAoRadius(1f);
				break;
			case Preset.HighestQuality:
				SetQuality(Quality.Highest);
				SetAoRadius(1.2f);
				SetAoMaxRadiusPixels(256f);
				SetBlurType(BlurType.Narrow);
				break;
			}
			m_Presets.preset = preset;
		}

		public PipelineStage GetPipelineStage()
		{
			return m_GeneralSettings.pipelineStage;
		}

		public void SetPipelineStage(PipelineStage pipelineStage)
		{
			m_GeneralSettings.pipelineStage = pipelineStage;
		}

		public Quality GetQuality()
		{
			return m_GeneralSettings.quality;
		}

		public void SetQuality(Quality quality)
		{
			m_GeneralSettings.quality = quality;
		}

		public Deinterleaving GetDeinterleaving()
		{
			return m_GeneralSettings.deinterleaving;
		}

		public void SetDeinterleaving(Deinterleaving deinterleaving)
		{
			m_GeneralSettings.deinterleaving = deinterleaving;
		}

		public Resolution GetResolution()
		{
			return m_GeneralSettings.resolution;
		}

		public void SetResolution(Resolution resolution)
		{
			m_GeneralSettings.resolution = resolution;
		}

		public NoiseType GetNoiseType()
		{
			return m_GeneralSettings.noiseType;
		}

		public void SetNoiseType(NoiseType noiseType)
		{
			m_GeneralSettings.noiseType = noiseType;
		}

		public DebugMode GetDebugMode()
		{
			return m_GeneralSettings.debugMode;
		}

		public void SetDebugMode(DebugMode debugMode)
		{
			m_GeneralSettings.debugMode = debugMode;
		}

		public float GetAoRadius()
		{
			return m_AOSettings.radius;
		}

		public void SetAoRadius(float radius)
		{
			m_AOSettings.radius = Mathf.Clamp(radius, 0.25f, 5f);
		}

		public float GetAoMaxRadiusPixels()
		{
			return m_AOSettings.maxRadiusPixels;
		}

		public void SetAoMaxRadiusPixels(float maxRadiusPixels)
		{
			m_AOSettings.maxRadiusPixels = Mathf.Clamp(maxRadiusPixels, 16f, 256f);
		}

		public float GetAoBias()
		{
			return m_AOSettings.bias;
		}

		public void SetAoBias(float bias)
		{
			m_AOSettings.bias = Mathf.Clamp(bias, 0f, 0.5f);
		}

		public float GetAoOffscreenSamplesContribution()
		{
			return m_AOSettings.offscreenSamplesContribution;
		}

		public void SetAoOffscreenSamplesContribution(float contribution)
		{
			m_AOSettings.offscreenSamplesContribution = Mathf.Clamp01(contribution);
		}

		public float GetAoMaxDistance()
		{
			return m_AOSettings.maxDistance;
		}

		public void SetAoMaxDistance(float maxDistance)
		{
			m_AOSettings.maxDistance = maxDistance;
		}

		public float GetAoDistanceFalloff()
		{
			return m_AOSettings.distanceFalloff;
		}

		public void SetAoDistanceFalloff(float distanceFalloff)
		{
			m_AOSettings.distanceFalloff = distanceFalloff;
		}

		public PerPixelNormals GetAoPerPixelNormals()
		{
			return m_AOSettings.perPixelNormals;
		}

		public void SetAoPerPixelNormals(PerPixelNormals perPixelNormals)
		{
			m_AOSettings.perPixelNormals = perPixelNormals;
		}

		public Color GetAoColor()
		{
			return m_AOSettings.baseColor;
		}

		public void SetAoColor(Color color)
		{
			m_AOSettings.baseColor = color;
		}

		public float GetAoIntensity()
		{
			return m_AOSettings.intensity;
		}

		public void SetAoIntensity(float intensity)
		{
			m_AOSettings.intensity = Mathf.Clamp(intensity, 0f, 4f);
		}

		public bool UseMultiBounce()
		{
			return m_AOSettings.useMultiBounce;
		}

		public void EnableMultiBounce(bool enabled = true)
		{
			m_AOSettings.useMultiBounce = enabled;
		}

		public float GetAoMultiBounceInfluence()
		{
			return m_AOSettings.multiBounceInfluence;
		}

		public void SetAoMultiBounceInfluence(float multiBounceInfluence)
		{
			m_AOSettings.multiBounceInfluence = Mathf.Clamp01(multiBounceInfluence);
		}

		public bool IsTemporalFilterEnabled()
		{
			return m_TemporalFilterSettings.enabled;
		}

		public void EnableTemporalFilter(bool enabled = true)
		{
			m_TemporalFilterSettings.enabled = enabled;
		}

		public VarianceClipping GetTemporalFilterVarianceClipping()
		{
			return m_TemporalFilterSettings.varianceClipping;
		}

		public void SetTemporalFilterVarianceClipping(VarianceClipping varianceClipping)
		{
			m_TemporalFilterSettings.varianceClipping = varianceClipping;
		}

		public BlurType GetBlurType()
		{
			return m_BlurSettings.type;
		}

		public void SetBlurType(BlurType blurType)
		{
			m_BlurSettings.type = blurType;
		}

		public float GetBlurSharpness()
		{
			return m_BlurSettings.sharpness;
		}

		public void SetBlurSharpness(float sharpness)
		{
			m_BlurSettings.sharpness = Mathf.Clamp(sharpness, 0f, 16f);
		}

		public bool IsColorBleedingEnabled()
		{
			return m_ColorBleedingSettings.enabled;
		}

		public void EnableColorBleeding(bool enabled = true)
		{
			m_ColorBleedingSettings.enabled = enabled;
		}

		public float GetColorBleedingSaturation()
		{
			return m_ColorBleedingSettings.saturation;
		}

		public void SetColorBleedingSaturation(float saturation)
		{
			m_ColorBleedingSettings.saturation = Mathf.Clamp(saturation, 0f, 4f);
		}

		public float GetColorBleedingAlbedoMultiplier()
		{
			return m_ColorBleedingSettings.albedoMultiplier;
		}

		public void SetColorBleedingAlbedoMultiplier(float albedoMultiplier)
		{
			m_ColorBleedingSettings.albedoMultiplier = Mathf.Clamp(albedoMultiplier, 0f, 32f);
		}

		public float GetColorBleedingBrightnessMask()
		{
			return m_ColorBleedingSettings.brightnessMask;
		}

		public void SetColorBleedingBrightnessMask(float brightnessMask)
		{
			m_ColorBleedingSettings.brightnessMask = Mathf.Clamp01(brightnessMask);
		}

		public Vector2 GetColorBleedingBrightnessMaskRange()
		{
			return m_ColorBleedingSettings.brightnessMaskRange;
		}

		public void SetColorBleedingBrightnessMaskRange(Vector2 brightnessMaskRange)
		{
			brightnessMaskRange.x = Mathf.Clamp(brightnessMaskRange.x, 0f, 2f);
			brightnessMaskRange.y = Mathf.Clamp(brightnessMaskRange.y, 0f, 2f);
			brightnessMaskRange.x = Mathf.Min(brightnessMaskRange.x, brightnessMaskRange.y);
			m_ColorBleedingSettings.brightnessMaskRange = brightnessMaskRange;
		}

		private void OnEnable()
		{
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				Debug.LogWarning("HBAO shader is not supported on this platform.");
				base.enabled = false;
				return;
			}
			if (hbaoShader == null)
			{
				hbaoShader = Shader.Find("Hidden/HBAO");
			}
			if (hbaoShader == null)
			{
				Debug.LogError("HBAO shader was not found...");
			}
			else if (!hbaoShader.isSupported)
			{
				Debug.LogWarning("HBAO shader is not supported on this platform.");
				base.enabled = false;
			}
			else
			{
				Initialize();
			}
		}

		private void OnDisable()
		{
			ClearCommandBuffer(cmdBuffer);
			ReleaseHistoryBuffers();
			if (material != null)
			{
				UnityEngine.Object.DestroyImmediate(material);
			}
			if (noiseTex != null)
			{
				UnityEngine.Object.DestroyImmediate(noiseTex);
			}
			if (fullscreenTriangle != null)
			{
				UnityEngine.Object.DestroyImmediate(fullscreenTriangle);
			}
		}

		private void OnPreRender()
		{
			if (!(hbaoShader == null) && !(hbaoCamera == null))
			{
				FetchRenderParameters();
				CheckParameters();
				UpdateMaterialProperties();
				UpdateShaderKeywords();
				if (isCommandBufferDirty)
				{
					ClearCommandBuffer(cmdBuffer);
					BuildCommandBuffer(cmdBuffer, cameraEvent);
					hbaoCamera.AddCommandBuffer(cameraEvent, cmdBuffer);
					isCommandBufferDirty = false;
				}
			}
		}

		private void OnPostRender()
		{
			frameCount++;
		}

		private void OnValidate()
		{
			if (!(hbaoShader == null) && !(hbaoCamera == null))
			{
				CheckParameters();
			}
		}

		private void Initialize()
		{
			m_sourceDescriptor = new RenderTextureDescriptor(0, 0);
			hbaoCamera = GetComponent<Camera>();
			hbaoCamera.forceIntoRenderTexture = true;
			material = new Material(hbaoShader);
			material.hideFlags = HideFlags.HideAndDontSave;
			motionVectorsSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf);
			cmdBuffer = new CommandBuffer
			{
				name = "HBAO"
			};
			isCommandBufferDirty = true;
		}

		private void FetchRenderParameters()
		{
			if (hbaoCamera.stereoEnabled)
			{
				RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
				stereoRenderingMode = StereoRenderingMode.MultiPass;
				if (eyeTextureDesc.dimension == TextureDimension.Tex2DArray)
				{
					stereoRenderingMode = StereoRenderingMode.SinglePassInstanced;
				}
				width = eyeTextureDesc.width;
				height = eyeTextureDesc.height;
				m_sourceDescriptor = eyeTextureDesc;
				xrActiveEye = (int)hbaoCamera.stereoActiveEye;
				screenWidth = XRSettings.eyeTextureWidth;
				screenHeight = XRSettings.eyeTextureHeight;
				stereoActive = true;
			}
			else
			{
				width = hbaoCamera.pixelWidth;
				height = hbaoCamera.pixelHeight;
				m_sourceDescriptor.width = width;
				m_sourceDescriptor.height = height;
				xrActiveEye = 0;
				screenWidth = width;
				screenHeight = height;
				stereoActive = false;
			}
			int num = ((generalSettings.resolution == Resolution.Full) ? 1 : ((generalSettings.deinterleaving != Deinterleaving.Disabled) ? 1 : 2));
			if (num > 1)
			{
				aoWidth = (width + width % 2) / num;
				aoHeight = (height + height % 2) / num;
			}
			else
			{
				aoWidth = width;
				aoHeight = height;
			}
			reinterleavedAoWidth = width + ((width % 4 != 0) ? (4 - width % 4) : 0);
			reinterleavedAoHeight = height + ((height % 4 != 0) ? (4 - height % 4) : 0);
			deinterleavedAoWidth = reinterleavedAoWidth / 4;
			deinterleavedAoHeight = reinterleavedAoHeight / 4;
		}

		private void AllocateHistoryBuffers()
		{
			ReleaseHistoryBuffers();
			aoHistoryBuffer = GetScreenSpaceRT(0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
			if (colorBleedingSettings.enabled)
			{
				colorBleedingHistoryBuffer = GetScreenSpaceRT(0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = aoHistoryBuffer;
			GL.Clear(clearDepth: false, clearColor: true, Color.white);
			if (colorBleedingSettings.enabled)
			{
				RenderTexture.active = colorBleedingHistoryBuffer;
				GL.Clear(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 1f));
			}
			RenderTexture.active = active;
			frameCount = 0;
		}

		private void ReleaseHistoryBuffers()
		{
			if (aoHistoryBuffer != null)
			{
				aoHistoryBuffer.Release();
			}
			if (colorBleedingHistoryBuffer != null)
			{
				colorBleedingHistoryBuffer.Release();
			}
		}

		private void ClearCommandBuffer(CommandBuffer cmd)
		{
			if (cmd != null)
			{
				if (hbaoCamera != null)
				{
					hbaoCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);
					hbaoCamera.RemoveCommandBuffer(CameraEvent.AfterLighting, cmd);
					hbaoCamera.RemoveCommandBuffer(CameraEvent.BeforeReflections, cmd);
				}
				cmd.Clear();
			}
		}

		private void BuildCommandBuffer(CommandBuffer cmd, CameraEvent cameraEvent)
		{
			if (generalSettings.deinterleaving == Deinterleaving.Disabled)
			{
				GetScreenSpaceTemporaryRT(cmd, ShaderProperties.hbaoTex, 0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
				AO(cmd);
			}
			else
			{
				GetScreenSpaceTemporaryRT(cmd, ShaderProperties.hbaoTex, 0, widthOverride: reinterleavedAoWidth, heightOverride: reinterleavedAoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
				DeinterleavedAO(cmd);
			}
			Blur(cmd);
			TemporalFilter(cmd);
			Composite(cmd, cameraEvent);
			ReleaseTemporaryRT(cmd, ShaderProperties.hbaoTex);
		}

		private void AO(CommandBuffer cmd)
		{
			BlitFullscreenTriangleWithClear(cmd, BuiltinRenderTextureType.CameraTarget, ShaderProperties.hbaoTex, material, new Color(0f, 0f, 0f, 1f));
		}

		private void DeinterleavedAO(CommandBuffer cmd)
		{
			for (int i = 0; i < 4; i++)
			{
				RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[4]
				{
					ShaderProperties.depthSliceTex[i << 2],
					ShaderProperties.depthSliceTex[(i << 2) + 1],
					ShaderProperties.depthSliceTex[(i << 2) + 2],
					ShaderProperties.depthSliceTex[(i << 2) + 3]
				};
				RenderTargetIdentifier[] destinations2 = new RenderTargetIdentifier[4]
				{
					ShaderProperties.normalsSliceTex[i << 2],
					ShaderProperties.normalsSliceTex[(i << 2) + 1],
					ShaderProperties.normalsSliceTex[(i << 2) + 2],
					ShaderProperties.normalsSliceTex[(i << 2) + 3]
				};
				int num = (i & 1) << 1;
				int num2 = i >> 1 << 1;
				cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[0], new Vector2(num, num2));
				cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[1], new Vector2(num + 1, num2));
				cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[2], new Vector2(num, num2 + 1));
				cmd.SetGlobalVector(ShaderProperties.deinterleaveOffset[3], new Vector2(num + 1, num2 + 1));
				for (int j = 0; j < 4; j++)
				{
					GetScreenSpaceTemporaryRT(cmd, ShaderProperties.depthSliceTex[j + 4 * i], 0, widthOverride: deinterleavedAoWidth, heightOverride: deinterleavedAoHeight, colorFormat: depthFormat, readWrite: RenderTextureReadWrite.Linear, filter: FilterMode.Point);
					GetScreenSpaceTemporaryRT(cmd, ShaderProperties.normalsSliceTex[j + 4 * i], 0, widthOverride: deinterleavedAoWidth, heightOverride: deinterleavedAoHeight, colorFormat: normalsFormat, readWrite: RenderTextureReadWrite.Linear, filter: FilterMode.Point);
				}
				BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, destinations, material, 2);
				BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, destinations2, material, 3);
			}
			for (int k = 0; k < 16; k++)
			{
				cmd.SetGlobalTexture(ShaderProperties.depthTex, ShaderProperties.depthSliceTex[k]);
				cmd.SetGlobalTexture(ShaderProperties.normalsTex, ShaderProperties.normalsSliceTex[k]);
				cmd.SetGlobalVector(ShaderProperties.jitter, s_jitter[k]);
				GetScreenSpaceTemporaryRT(cmd, ShaderProperties.aoSliceTex[k], 0, widthOverride: deinterleavedAoWidth, heightOverride: deinterleavedAoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear, filter: FilterMode.Point);
				BlitFullscreenTriangleWithClear(cmd, BuiltinRenderTextureType.CameraTarget, ShaderProperties.aoSliceTex[k], material, new Color(0f, 0f, 0f, 1f), 1);
				ReleaseTemporaryRT(cmd, ShaderProperties.depthSliceTex[k]);
				ReleaseTemporaryRT(cmd, ShaderProperties.normalsSliceTex[k]);
			}
			GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, widthOverride: reinterleavedAoWidth, heightOverride: reinterleavedAoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
			for (int l = 0; l < 16; l++)
			{
				cmd.SetGlobalVector(ShaderProperties.atlasOffset, new Vector2(((l & 1) + ((l & 7) >> 2 << 1)) * deinterleavedAoWidth, (((l & 3) >> 1) + (l >> 3 << 1)) * deinterleavedAoHeight));
				BlitFullscreenTriangle(cmd, ShaderProperties.aoSliceTex[l], ShaderProperties.tempTex, material, 4);
				ReleaseTemporaryRT(cmd, ShaderProperties.aoSliceTex[l]);
			}
			ApplyFlip(cmd);
			BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, ShaderProperties.hbaoTex, material, 5);
			ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
		}

		private void Blur(CommandBuffer cmd)
		{
			if (blurSettings.type != BlurType.None)
			{
				float num = aoWidth;
				float num2 = aoHeight;
				if (hbaoCamera.allowDynamicResolution)
				{
					num *= ScalableBufferManager.widthScaleFactor;
					num2 *= ScalableBufferManager.heightScaleFactor;
				}
				GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
				cmd.SetGlobalVector(ShaderProperties.blurDeltaUV, new Vector2(1f / num, 0f));
				BlitFullscreenTriangle(cmd, ShaderProperties.hbaoTex, ShaderProperties.tempTex, material, 6);
				cmd.SetGlobalVector(ShaderProperties.blurDeltaUV, new Vector2(0f, 1f / num2));
				BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, ShaderProperties.hbaoTex, material, 6);
				ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
			}
		}

		private void TemporalFilter(CommandBuffer cmd)
		{
			if (isHistoryBufferDirty && temporalFilterSettings.enabled)
			{
				AllocateHistoryBuffers();
			}
			if (temporalFilterSettings.enabled && !renderingInSceneView)
			{
				if (colorBleedingSettings.enabled)
				{
					RenderTargetIdentifier[] destinations = new RenderTargetIdentifier[2] { aoHistoryBuffer, colorBleedingHistoryBuffer };
					GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
					GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex2, 0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
					BlitFullscreenTriangle(cmd, aoHistoryBuffer, ShaderProperties.tempTex2, material, 8);
					BlitFullscreenTriangle(cmd, colorBleedingHistoryBuffer, ShaderProperties.tempTex, material, 8);
					BlitFullscreenTriangle(cmd, ShaderProperties.tempTex2, destinations, material, 7);
					ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
					ReleaseTemporaryRT(cmd, ShaderProperties.tempTex2);
					cmd.SetGlobalTexture(ShaderProperties.hbaoTex, colorBleedingHistoryBuffer);
				}
				else
				{
					GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, widthOverride: aoWidth, heightOverride: aoHeight, colorFormat: colorFormat, readWrite: RenderTextureReadWrite.Linear);
					BlitFullscreenTriangle(cmd, aoHistoryBuffer, ShaderProperties.tempTex, material, 8);
					BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, aoHistoryBuffer, material, 7);
					ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
					cmd.SetGlobalTexture(ShaderProperties.hbaoTex, aoHistoryBuffer);
				}
			}
		}

		private void Composite(CommandBuffer cmd, CameraEvent cameraEvent)
		{
			if (generalSettings.debugMode == DebugMode.Disabled)
			{
				switch (cameraEvent)
				{
				case CameraEvent.BeforeReflections:
					CompositeBeforeReflections(cmd);
					break;
				case CameraEvent.AfterLighting:
					CompositeAfterLighting(cmd);
					break;
				default:
					CompositeBeforeImageEffectsOpaque(cmd);
					break;
				}
			}
			else
			{
				CompositeDebug(cmd, (generalSettings.debugMode == DebugMode.ViewNormals) ? 14 : 9);
			}
		}

		private void CompositeBeforeReflections(CommandBuffer cmd)
		{
			bool allowHDR = hbaoCamera.allowHDR;
			RenderTargetIdentifier[] array = new RenderTargetIdentifier[2]
			{
				BuiltinRenderTextureType.GBuffer0,
				allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3
			};
			GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, RenderTextureFormat.ARGB32);
			GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex2, 0, allowHDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB2101010);
			BlitFullscreenTriangle(cmd, array[0], ShaderProperties.tempTex, material, 8);
			BlitFullscreenTriangle(cmd, array[1], ShaderProperties.tempTex2, material, 8);
			BlitFullscreenTriangle(cmd, ShaderProperties.tempTex2, array, material, 11);
			ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
			ReleaseTemporaryRT(cmd, ShaderProperties.tempTex2);
		}

		private void CompositeAfterLighting(CommandBuffer cmd)
		{
			bool allowHDR = hbaoCamera.allowHDR;
			BuiltinRenderTextureType builtinRenderTextureType = (allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3);
			GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, allowHDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB2101010);
			BlitFullscreenTriangle(cmd, builtinRenderTextureType, ShaderProperties.tempTex, material, 8);
			BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, builtinRenderTextureType, material, 10);
			ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
		}

		private void CompositeBeforeImageEffectsOpaque(CommandBuffer cmd)
		{
			if (aoSettings.useMultiBounce)
			{
				GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, sourceFormat);
				if (stereoActive && hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
				{
					cmd.Blit(BuiltinRenderTextureType.CameraTarget, ShaderProperties.tempTex);
				}
				else
				{
					BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, ShaderProperties.tempTex, material, 8);
				}
			}
			ApplyFlip(cmd, SystemInfo.graphicsUVStartsAtTop);
			BlitFullscreenTriangle(cmd, aoSettings.useMultiBounce ? ((RenderTargetIdentifier)ShaderProperties.tempTex) : ((RenderTargetIdentifier)BuiltinRenderTextureType.None), BuiltinRenderTextureType.CameraTarget, material, 12);
			if (colorBleedingSettings.enabled)
			{
				BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.None, BuiltinRenderTextureType.CameraTarget, material, 13);
			}
			if (aoSettings.useMultiBounce)
			{
				ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
			}
		}

		private void CompositeDebug(CommandBuffer cmd, int finalPassId = 9)
		{
			GetScreenSpaceTemporaryRT(cmd, ShaderProperties.tempTex, 0, sourceFormat);
			if (stereoActive && hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				cmd.Blit(BuiltinRenderTextureType.CameraTarget, ShaderProperties.tempTex);
			}
			else
			{
				BlitFullscreenTriangle(cmd, BuiltinRenderTextureType.CameraTarget, ShaderProperties.tempTex, material, 8);
			}
			ApplyFlip(cmd, SystemInfo.graphicsUVStartsAtTop);
			BlitFullscreenTriangle(cmd, ShaderProperties.tempTex, BuiltinRenderTextureType.CameraTarget, material, finalPassId);
			ReleaseTemporaryRT(cmd, ShaderProperties.tempTex);
		}

		private void UpdateMaterialProperties()
		{
			int num = ((!stereoActive || stereoRenderingMode != StereoRenderingMode.SinglePassInstanced || renderingInSceneView) ? 1 : 2);
			for (int i = 0; i < num; i++)
			{
				Matrix4x4 obj = ((i == 0) ? hbaoCamera.projectionMatrix : hbaoCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));
				float m = obj.m00;
				float m2 = obj.m11;
				m_UVToViewPerEye[(i == 0) ? xrActiveEye : i] = new Vector4(2f / m, -2f / m2, -1f / m, 1f / m2);
				m_RadiusPerEye[(i == 0) ? xrActiveEye : i] = aoSettings.radius * 0.5f * ((float)(screenHeight / ((generalSettings.deinterleaving != Deinterleaving.x4) ? 1 : 4)) / (2f / m2));
			}
			float num2 = Mathf.Max(16f, aoSettings.maxRadiusPixels * Mathf.Sqrt((float)(screenWidth * screenHeight) / 2073600f));
			num2 /= (float)((generalSettings.deinterleaving != Deinterleaving.x4) ? 1 : 4);
			Vector4 value = ((generalSettings.deinterleaving == Deinterleaving.x4) ? new Vector4((float)reinterleavedAoWidth / (float)width, (float)reinterleavedAoHeight / (float)height, 1f / ((float)reinterleavedAoWidth / (float)width), 1f / ((float)reinterleavedAoHeight / (float)height)) : ((generalSettings.resolution == Resolution.Half) ? new Vector4(((float)width + 0.5f) / (float)width, ((float)height + 0.5f) / (float)height, 1f, 1f) : Vector4.one));
			material.SetTexture(ShaderProperties.noiseTex, noiseTex);
			material.SetVector(ShaderProperties.inputTexelSize, new Vector4(1f / (float)width, 1f / (float)height, width, height));
			if (hbaoCamera.allowDynamicResolution)
			{
				material.SetVector(ShaderProperties.aoTexelSize, new Vector4(1f / ((float)aoWidth * ScalableBufferManager.widthScaleFactor), 1f / ((float)aoHeight * ScalableBufferManager.heightScaleFactor), (float)aoWidth * ScalableBufferManager.widthScaleFactor, (float)aoHeight * ScalableBufferManager.heightScaleFactor));
			}
			else
			{
				material.SetVector(ShaderProperties.aoTexelSize, new Vector4(1f / (float)aoWidth, 1f / (float)aoHeight, aoWidth, aoHeight));
			}
			material.SetVector(ShaderProperties.deinterleavedAOTexelSize, new Vector4(1f / (float)deinterleavedAoWidth, 1f / (float)deinterleavedAoHeight, deinterleavedAoWidth, deinterleavedAoHeight));
			material.SetVector(ShaderProperties.reinterleavedAOTexelSize, new Vector4(1f / (float)reinterleavedAoWidth, 1f / (float)reinterleavedAoHeight, reinterleavedAoWidth, reinterleavedAoHeight));
			material.SetVector(ShaderProperties.targetScale, value);
			material.SetVectorArray(ShaderProperties.uvToView, m_UVToViewPerEye);
			material.SetFloatArray(ShaderProperties.radius, m_RadiusPerEye);
			material.SetFloat(ShaderProperties.maxRadiusPixels, num2);
			material.SetFloat(ShaderProperties.negInvRadius2, -1f / (aoSettings.radius * aoSettings.radius));
			material.SetFloat(ShaderProperties.angleBias, aoSettings.bias);
			material.SetFloat(ShaderProperties.aoMultiplier, 2f * (1f / (1f - aoSettings.bias)));
			material.SetFloat(ShaderProperties.intensity, isLinearColorSpace ? aoSettings.intensity : (aoSettings.intensity * 0.45454547f));
			material.SetColor(ShaderProperties.baseColor, aoSettings.baseColor);
			material.SetFloat(ShaderProperties.multiBounceInfluence, aoSettings.multiBounceInfluence);
			material.SetFloat(ShaderProperties.offscreenSamplesContrib, aoSettings.offscreenSamplesContribution);
			material.SetFloat(ShaderProperties.maxDistance, aoSettings.maxDistance);
			material.SetFloat(ShaderProperties.distanceFalloff, aoSettings.distanceFalloff);
			material.SetFloat(ShaderProperties.blurSharpness, blurSettings.sharpness);
			material.SetFloat(ShaderProperties.colorBleedSaturation, colorBleedingSettings.saturation);
			material.SetFloat(ShaderProperties.albedoMultiplier, colorBleedingSettings.albedoMultiplier);
			material.SetFloat(ShaderProperties.colorBleedBrightnessMask, colorBleedingSettings.brightnessMask);
			material.SetVector(ShaderProperties.colorBleedBrightnessMaskRange, AdjustBrightnessMaskToGammaSpace(new Vector2(Mathf.Pow(colorBleedingSettings.brightnessMaskRange.x, 3f), Mathf.Pow(colorBleedingSettings.brightnessMaskRange.y, 3f))));
			material.SetVector(ShaderProperties.temporalParams, (temporalFilterSettings.enabled && !renderingInSceneView) ? new Vector2(s_temporalRotations[frameCount % 6] / 360f, s_temporalOffsets[frameCount % 4]) : Vector2.zero);
		}

		private void UpdateShaderKeywords()
		{
			if (m_ShaderKeywords == null || m_ShaderKeywords.Length != 12)
			{
				m_ShaderKeywords = new string[12];
			}
			m_ShaderKeywords[0] = ShaderProperties.GetOrthographicOrDeferredKeyword(hbaoCamera.orthographic, generalSettings);
			m_ShaderKeywords[1] = ShaderProperties.GetQualityKeyword(generalSettings);
			m_ShaderKeywords[2] = ShaderProperties.GetNoiseKeyword(generalSettings);
			m_ShaderKeywords[3] = ShaderProperties.GetDeinterleavingKeyword(generalSettings);
			m_ShaderKeywords[4] = ShaderProperties.GetDebugKeyword(generalSettings);
			m_ShaderKeywords[5] = ShaderProperties.GetMultibounceKeyword(aoSettings);
			m_ShaderKeywords[6] = ShaderProperties.GetOffscreenSamplesContributionKeyword(aoSettings);
			m_ShaderKeywords[7] = ShaderProperties.GetPerPixelNormalsKeyword(aoSettings);
			m_ShaderKeywords[8] = ShaderProperties.GetBlurRadiusKeyword(blurSettings);
			m_ShaderKeywords[9] = ShaderProperties.GetVarianceClippingKeyword(temporalFilterSettings);
			m_ShaderKeywords[10] = ShaderProperties.GetColorBleedingKeyword(colorBleedingSettings);
			m_ShaderKeywords[11] = ShaderProperties.GetLightingLogEncodedKeyword(hbaoCamera.allowHDR);
			material.shaderKeywords = m_ShaderKeywords;
		}

		private void CheckParameters()
		{
			hbaoCamera.depthTextureMode |= DepthTextureMode.Depth;
			if (aoSettings.perPixelNormals == PerPixelNormals.Camera)
			{
				hbaoCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			if (temporalFilterSettings.enabled)
			{
				hbaoCamera.depthTextureMode |= DepthTextureMode.MotionVectors;
			}
			if (hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading && aoSettings.perPixelNormals == PerPixelNormals.GBuffer)
			{
				SetAoPerPixelNormals(PerPixelNormals.Camera);
			}
			if (generalSettings.deinterleaving != Deinterleaving.Disabled && SystemInfo.supportedRenderTargetCount < 4)
			{
				SetDeinterleaving(Deinterleaving.Disabled);
			}
			if (generalSettings.pipelineStage != PipelineStage.BeforeImageEffectsOpaque && hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				SetPipelineStage(PipelineStage.BeforeImageEffectsOpaque);
			}
			if (generalSettings.pipelineStage != PipelineStage.BeforeImageEffectsOpaque && aoSettings.perPixelNormals == PerPixelNormals.Camera)
			{
				SetAoPerPixelNormals(PerPixelNormals.GBuffer);
			}
			if (stereoActive && hbaoCamera.actualRenderingPath != RenderingPath.DeferredShading && aoSettings.perPixelNormals != PerPixelNormals.Reconstruct)
			{
				SetAoPerPixelNormals(PerPixelNormals.Reconstruct);
			}
			if (temporalFilterSettings.enabled && !motionVectorsSupported)
			{
				EnableTemporalFilter(enabled: false);
			}
			if (colorBleedingSettings.enabled && temporalFilterSettings.enabled && SystemInfo.supportedRenderTargetCount < 2)
			{
				EnableTemporalFilter(enabled: false);
			}
			if (noiseTex == null || m_PreviousNoiseType != generalSettings.noiseType)
			{
				if (noiseTex != null)
				{
					UnityEngine.Object.DestroyImmediate(noiseTex);
				}
				CreateNoiseTexture();
				m_PreviousNoiseType = generalSettings.noiseType;
			}
		}

		private RenderTextureDescriptor GetDefaultDescriptor(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default)
		{
			RenderTextureDescriptor result = new RenderTextureDescriptor(m_sourceDescriptor.width, m_sourceDescriptor.height, m_sourceDescriptor.colorFormat, depthBufferBits);
			result.dimension = m_sourceDescriptor.dimension;
			result.volumeDepth = m_sourceDescriptor.volumeDepth;
			result.vrUsage = m_sourceDescriptor.vrUsage;
			result.msaaSamples = m_sourceDescriptor.msaaSamples;
			result.memoryless = m_sourceDescriptor.memoryless;
			result.useMipMap = m_sourceDescriptor.useMipMap;
			result.autoGenerateMips = m_sourceDescriptor.autoGenerateMips;
			result.enableRandomWrite = m_sourceDescriptor.enableRandomWrite;
			result.shadowSamplingMode = m_sourceDescriptor.shadowSamplingMode;
			if (hbaoCamera.allowDynamicResolution)
			{
				result.useDynamicScale = true;
			}
			if (colorFormat != RenderTextureFormat.Default)
			{
				result.colorFormat = colorFormat;
			}
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

		private RenderTexture GetScreenSpaceRT(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filter = FilterMode.Bilinear, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor defaultDescriptor = GetDefaultDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				defaultDescriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				defaultDescriptor.height = heightOverride;
			}
			return new RenderTexture(defaultDescriptor)
			{
				filterMode = filter
			};
		}

		private void GetScreenSpaceTemporaryRT(CommandBuffer cmd, int nameID, int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filter = FilterMode.Bilinear, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor defaultDescriptor = GetDefaultDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				defaultDescriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				defaultDescriptor.height = heightOverride;
			}
			cmd.GetTemporaryRT(nameID, defaultDescriptor, filter);
		}

		private void ReleaseTemporaryRT(CommandBuffer cmd, int nameID)
		{
			cmd.ReleaseTemporaryRT(nameID);
		}

		private void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int pass = 0)
		{
			cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
			cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		private void BlitFullscreenTriangle(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier[] destinations, Material material, int pass = 0)
		{
			cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destinations, destinations[0], 0, CubemapFace.Unknown, -1);
			cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		private void BlitFullscreenTriangleWithClear(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Color clearColor, int pass = 0)
		{
			cmd.SetGlobalTexture(ShaderProperties.mainTex, source);
			cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
			cmd.ClearRenderTarget(clearDepth: false, clearColor: true, clearColor);
			cmd.DrawMesh(fullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		private static void ApplyFlip(CommandBuffer cmd, bool flip = true)
		{
			if (flip)
			{
				cmd.SetGlobalVector(ShaderProperties.uvTransform, new Vector4(1f, -1f, 0f, 1f));
			}
			else
			{
				cmd.SetGlobalVector(ShaderProperties.uvTransform, new Vector4(1f, 1f, 0f, 0f));
			}
		}

		private static Vector2 AdjustBrightnessMaskToGammaSpace(Vector2 v)
		{
			if (!isLinearColorSpace)
			{
				return ToGammaSpace(v);
			}
			return v;
		}

		private static float ToGammaSpace(float v)
		{
			return Mathf.Pow(v, 0.45454547f);
		}

		private static Vector2 ToGammaSpace(Vector2 v)
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
					float r = ((generalSettings.noiseType != NoiseType.Dither) ? (0.25f * (0.0625f * (float)(((i + j) & 3) << 2) + (float)(i & 3))) : MersenneTwister.Numbers[num++]);
					float g = ((generalSettings.noiseType != NoiseType.Dither) ? (0.25f * (float)((j - i) & 3)) : MersenneTwister.Numbers[num++]);
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
}
