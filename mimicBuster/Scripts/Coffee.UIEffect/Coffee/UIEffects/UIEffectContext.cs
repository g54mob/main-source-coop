using System;
using System.Collections.Generic;
using Coffee.UIEffectInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	public sealed class UIEffectContext
	{
		private static readonly UIEffectContext s_DefaultContext = new UIEffectContext();

		private static readonly List<UIVertex> s_WorkingVertices = new List<UIVertex>(8192);

		private static readonly int s_SrcBlend = Shader.PropertyToID("_SrcBlend");

		private static readonly int s_DstBlend = Shader.PropertyToID("_DstBlend");

		private static readonly int s_ToneIntensity = Shader.PropertyToID("_ToneIntensity");

		private static readonly int s_ColorFilter = Shader.PropertyToID("_ColorFilter");

		private static readonly int s_ColorValue = Shader.PropertyToID("_ColorValue");

		private static readonly int s_ColorIntensity = Shader.PropertyToID("_ColorIntensity");

		private static readonly int s_ColorGlow = Shader.PropertyToID("_ColorGlow");

		private static readonly int s_SamplingIntensity = Shader.PropertyToID("_SamplingIntensity");

		private static readonly int s_SamplingWidth = Shader.PropertyToID("_SamplingWidth");

		private static readonly int s_SamplingScale = Shader.PropertyToID("_SamplingScale");

		private static readonly int s_TransitionRate = Shader.PropertyToID("_TransitionRate");

		private static readonly int s_TransitionReverse = Shader.PropertyToID("_TransitionReverse");

		private static readonly int s_TransitionTex = Shader.PropertyToID("_TransitionTex");

		private static readonly int s_TransitionTex_ST = Shader.PropertyToID("_TransitionTex_ST");

		private static readonly int s_TransitionTex_Speed = Shader.PropertyToID("_TransitionTex_Speed");

		private static readonly int s_TransitionWidth = Shader.PropertyToID("_TransitionWidth");

		private static readonly int s_TransitionSoftness = Shader.PropertyToID("_TransitionSoftness");

		private static readonly int s_TransitionRange = Shader.PropertyToID("_TransitionRange");

		private static readonly int s_TransitionColorFilter = Shader.PropertyToID("_TransitionColorFilter");

		private static readonly int s_TransitionColor = Shader.PropertyToID("_TransitionColor");

		private static readonly int s_TransitionColorGlow = Shader.PropertyToID("_TransitionColorGlow");

		private static readonly int s_TransitionPatternReverse = Shader.PropertyToID("_TransitionPatternReverse");

		private static readonly int s_TransitionAutoPlaySpeed = Shader.PropertyToID("_TransitionAutoPlaySpeed");

		private static readonly int s_TransitionGradientTex = Shader.PropertyToID("_TransitionGradientTex");

		private static readonly int s_TargetColor = Shader.PropertyToID("_TargetColor");

		private static readonly int s_TargetRange = Shader.PropertyToID("_TargetRange");

		private static readonly int s_TargetSoftness = Shader.PropertyToID("_TargetSoftness");

		private static readonly int s_ShadowColorFilter = Shader.PropertyToID("_ShadowColorFilter");

		private static readonly int s_ShadowColor = Shader.PropertyToID("_ShadowColor");

		private static readonly int s_ShadowBlurIntensity = Shader.PropertyToID("_ShadowBlurIntensity");

		private static readonly int s_ShadowColorGlow = Shader.PropertyToID("_ShadowColorGlow");

		private static readonly int s_EdgeWidth = Shader.PropertyToID("_EdgeWidth");

		private static readonly int s_EdgeColorFilter = Shader.PropertyToID("_EdgeColorFilter");

		private static readonly int s_EdgeColor = Shader.PropertyToID("_EdgeColor");

		private static readonly int s_EdgeColorGlow = Shader.PropertyToID("_EdgeColorGlow");

		private static readonly int s_EdgeShinyAutoPlaySpeed = Shader.PropertyToID("_EdgeShinyAutoPlaySpeed");

		private static readonly int s_EdgeShinyRate = Shader.PropertyToID("_EdgeShinyRate");

		private static readonly int s_EdgeShinyWidth = Shader.PropertyToID("_EdgeShinyWidth");

		private static readonly int s_PatternArea = Shader.PropertyToID("_PatternArea");

		private static readonly int s_DetailIntensity = Shader.PropertyToID("_DetailIntensity");

		private static readonly int s_DetailThreshold = Shader.PropertyToID("_DetailThreshold");

		private static readonly int s_DetailColor = Shader.PropertyToID("_DetailColor");

		private static readonly int s_DetailTex = Shader.PropertyToID("_DetailTex");

		private static readonly int s_DetailTex_ST = Shader.PropertyToID("_DetailTex_ST");

		private static readonly int s_DetailTex_Speed = Shader.PropertyToID("_DetailTex_Speed");

		private static readonly int s_GradationIntensity = Shader.PropertyToID("_GradationIntensity");

		private static readonly int s_GradationColorFilter = Shader.PropertyToID("_GradationColorFilter");

		private static readonly int s_GradationColor1 = Shader.PropertyToID("_GradationColor1");

		private static readonly int s_GradationColor2 = Shader.PropertyToID("_GradationColor2");

		private static readonly int s_GradationColor3 = Shader.PropertyToID("_GradationColor3");

		private static readonly int s_GradationColor4 = Shader.PropertyToID("_GradationColor4");

		private static readonly int s_GradationTex = Shader.PropertyToID("_GradationTex");

		private static readonly int s_GradationTex_ST = Shader.PropertyToID("_GradationTex_ST");

		private static readonly int s_GradationRadial = Shader.PropertyToID("_GradationRadial");

		private static readonly int s_RootViewMatrix = Shader.PropertyToID("_RootViewMatrix");

		private static readonly int s_GradViewMatrix = Shader.PropertyToID("_GradViewMatrix");

		private static readonly int s_MirrorRootViewMatrix = Shader.PropertyToID("_MirrorRootViewMatrix");

		private static readonly int s_MirrorGradViewMatrix = Shader.PropertyToID("_MirrorGradViewMatrix");

		private static readonly int s_CanvasToWorldMatrix = Shader.PropertyToID("_CanvasToWorldMatrix");

		private static readonly string[] s_ToneKeywords = new string[6] { "", "TONE_GRAYSCALE", "TONE_SEPIA", "TONE_NEGATIVE", "TONE_RETRO", "TONE_POSTERIZE" };

		private static readonly string[] s_ColorKeywords = new string[2] { "", "COLOR_FILTER" };

		private static readonly string[] s_SamplingKeywords = new string[8] { "", "SAMPLING_BLUR_FAST", "SAMPLING_BLUR_MEDIUM", "SAMPLING_BLUR_DETAIL", "SAMPLING_PIXELATION", "SAMPLING_RGB_SHIFT", "SAMPLING_EDGE_LUMINANCE", "SAMPLING_EDGE_ALPHA" };

		private static readonly string[] s_TransitionKeywords = new string[10] { "", "TRANSITION_FADE", "TRANSITION_CUTOFF", "TRANSITION_DISSOLVE", "TRANSITION_SHINY", "TRANSITION_MASK", "TRANSITION_MELT", "TRANSITION_BURN", "TRANSITION_PATTERN", "TRANSITION_BLAZE" };

		private static readonly string[] s_TargetKeywords = new string[3] { "", "TARGET_HUE", "TARGET_LUMINANCE" };

		private static readonly string[] s_EdgeKeywords = new string[3] { "", "EDGE_PLAIN", "EDGE_SHINY" };

		private static readonly string[] s_DetailKeywords = new string[7] { "", "DETAIL_MASKING", "DETAIL_MULTIPLY", "DETAIL_ADDITIVE", "DETAIL_REPLACE", "DETAIL_MULTIPLY_ADDITIVE", "DETAIL_SUBTRACTIVE" };

		private static readonly string[] s_GradationKeywords = new string[4] { "", "GRADATION_GRADIENT", "GRADATION_COLOR2", "GRADATION_COLOR4" };

		private static readonly Vector2[][] s_ShadowVectors = new Vector2[5][]
		{
			Array.Empty<Vector2>(),
			new Vector2[1] { Vector2.one },
			new Vector2[3]
			{
				Vector2.one,
				Vector2.right,
				Vector2.up
			},
			new Vector2[4]
			{
				Vector2.one,
				-Vector2.one,
				new Vector2(1f, -1f),
				new Vector2(-1f, 1f)
			},
			new Vector2[8]
			{
				Vector2.one,
				-Vector2.one,
				new Vector2(1f, -1f),
				new Vector2(-1f, 1f),
				Vector2.right,
				Vector2.up,
				Vector2.left,
				Vector2.down
			}
		};

		public static Func<UIVertex, Rect, UIVertex> onModifyVertex;

		public ToneFilter m_ToneFilter;

		public float m_ToneIntensity;

		public ColorFilter m_ColorFilter;

		public float m_ColorIntensity;

		public Color m_Color;

		public bool m_ColorGlow;

		public SamplingFilter m_SamplingFilter;

		public float m_SamplingIntensity;

		public float m_SamplingWidth;

		public TransitionFilter m_TransitionFilter;

		public float m_TransitionRate;

		public bool m_TransitionReverse;

		public Texture m_TransitionTex;

		public Vector2 m_TransitionTexScale;

		public Vector2 m_TransitionTexOffset;

		public Vector2 m_TransitionTexSpeed;

		public float m_TransitionRotation;

		public bool m_TransitionKeepAspectRatio;

		public float m_TransitionWidth;

		public float m_TransitionSoftness;

		public MinMax01 m_TransitionRange;

		public ColorFilter m_TransitionColorFilter;

		public Color m_TransitionColor;

		public bool m_TransitionColorGlow;

		public bool m_TransitionPatternReverse;

		public float m_TransitionAutoPlaySpeed;

		public Gradient m_TransitionGradient;

		public TargetMode m_TargetMode;

		public Color m_TargetColor;

		public float m_TargetRange;

		public float m_TargetSoftness;

		public BlendMode m_SrcBlendMode;

		public BlendMode m_DstBlendMode;

		public ShadowMode m_ShadowMode;

		public Vector2 m_ShadowDistance;

		public int m_ShadowIteration;

		public float m_ShadowFade;

		public float m_ShadowMirrorScale;

		public float m_ShadowBlurIntensity;

		public ColorFilter m_ShadowColorFilter;

		public Color m_ShadowColor;

		public bool m_ShadowColorGlow;

		public EdgeMode m_EdgeMode;

		public float m_EdgeShinyRate;

		public float m_EdgeWidth;

		public ColorFilter m_EdgeColorFilter;

		public Color m_EdgeColor;

		public bool m_EdgeColorGlow;

		public float m_EdgeShinyWidth;

		public float m_EdgeShinyAutoPlaySpeed;

		public PatternArea m_PatternArea;

		public GradationMode m_GradationMode;

		public float m_GradationIntensity;

		public GradationColorFilter m_GradationColorFilter;

		public Color m_GradationColor1;

		public Color m_GradationColor2;

		public Color m_GradationColor3;

		public Color m_GradationColor4;

		public Gradient m_GradationGradient;

		public float m_GradationOffset;

		public float m_GradationScale;

		public float m_GradationRotation;

		public TextureWrapMode m_GradationWrapMode;

		public bool m_GradationReverse;

		public DetailFilter m_DetailFilter;

		public float m_DetailIntensity;

		public MinMax01 m_DetailThreshold;

		public Color m_DetailColor;

		public Texture m_DetailTex;

		public Vector2 m_DetailTexScale;

		public Vector2 m_DetailTexOffset;

		public Vector2 m_DetailTexSpeed;

		public Flip m_Flip;

		private Texture2D _gradationRampTex;

		private Texture2D _transitionRampTex;

		private bool _isGradientDirty = true;

		private bool _isTransitionGradientDirty = true;

		private static readonly Color[] s_Colors = new Color[256];

		private static readonly InternalObjectPool<Texture2D> s_TexturePool = new InternalObjectPool<Texture2D>(() => new Texture2D(s_Colors.Length, 1, TextureFormat.RGBAFloat, mipChain: false, linear: false)
		{
			name = "GradationRamp",
			hideFlags = HideFlags.DontSave,
			wrapMode = TextureWrapMode.Repeat,
			anisoLevel = 0
		}, (Texture2D texture) => texture, delegate
		{
		});

		public bool willModifyMaterial
		{
			get
			{
				if (m_ToneFilter == ToneFilter.None && m_ColorFilter == ColorFilter.None && m_SamplingFilter == SamplingFilter.None && m_TransitionFilter == TransitionFilter.None && m_SrcBlendMode == BlendMode.One && m_DstBlendMode == BlendMode.OneMinusSrcAlpha && m_ShadowMode == ShadowMode.None && m_EdgeMode == EdgeMode.None && m_DetailFilter == DetailFilter.None)
				{
					return m_GradationMode != GradationMode.None;
				}
				return true;
			}
		}

		public bool willModifyVertex => willModifyMaterial;

		public bool useViewMatrix
		{
			get
			{
				if (m_TransitionFilter == TransitionFilter.None && m_GradationMode == GradationMode.None && m_DetailFilter == DetailFilter.None)
				{
					return m_EdgeMode == EdgeMode.Shiny;
				}
				return true;
			}
		}

		private Texture2D gradationRampTex
		{
			get
			{
				if (m_GradationGradient == null)
				{
					return null;
				}
				if (_gradationRampTex == null)
				{
					_gradationRampTex = s_TexturePool.Rent();
				}
				if (!_isGradientDirty)
				{
					return _gradationRampTex;
				}
				_isGradientDirty = false;
				int num = s_Colors.Length;
				for (int i = 0; i < num; i++)
				{
					int num2 = (m_GradationReverse ? (num - 1 - i) : i);
					float time = (float)i / (float)(num - 1);
					s_Colors[num2] = ApplyColorSpace(m_GradationGradient.Evaluate(time));
				}
				_gradationRampTex.wrapMode = m_GradationWrapMode;
				_gradationRampTex.filterMode = ((m_GradationGradient.mode == GradientMode.Blend) ? FilterMode.Bilinear : FilterMode.Point);
				_gradationRampTex.SetPixels(s_Colors);
				_gradationRampTex.Apply();
				return _gradationRampTex;
			}
		}

		private Texture2D transitionRampTex
		{
			get
			{
				if (m_TransitionGradient == null)
				{
					return null;
				}
				if (_transitionRampTex == null)
				{
					_transitionRampTex = s_TexturePool.Rent();
				}
				if (!_isTransitionGradientDirty)
				{
					return _transitionRampTex;
				}
				_isTransitionGradientDirty = false;
				int num = s_Colors.Length;
				for (int i = 0; i < num; i++)
				{
					float time = (float)i / (float)(num - 1);
					s_Colors[i] = ApplyColorSpace(m_TransitionGradient.Evaluate(time));
				}
				s_Colors[num - 1].a = 0f;
				_transitionRampTex.wrapMode = TextureWrapMode.Clamp;
				_transitionRampTex.filterMode = ((m_TransitionGradient.mode == GradientMode.Blend) ? FilterMode.Bilinear : FilterMode.Point);
				_transitionRampTex.SetPixels(s_Colors);
				_transitionRampTex.Apply();
				return _transitionRampTex;
			}
		}

		public void Reset()
		{
			_isGradientDirty = true;
			_isTransitionGradientDirty = true;
			s_TexturePool.Return(ref _gradationRampTex);
			s_TexturePool.Return(ref _transitionRampTex);
			CopyFrom(s_DefaultContext);
		}

		private void CopyFrom(UIEffectContext src)
		{
			m_ToneFilter = src.m_ToneFilter;
			m_ToneIntensity = src.m_ToneIntensity;
			m_ColorFilter = src.m_ColorFilter;
			m_Color = src.m_Color;
			m_ColorIntensity = src.m_ColorIntensity;
			m_ColorGlow = src.m_ColorGlow;
			m_SamplingFilter = src.m_SamplingFilter;
			m_SamplingIntensity = src.m_SamplingIntensity;
			m_SamplingWidth = src.m_SamplingWidth;
			m_TransitionFilter = src.m_TransitionFilter;
			m_TransitionRate = src.m_TransitionRate;
			m_TransitionReverse = src.m_TransitionReverse;
			m_TransitionTex = src.m_TransitionTex;
			m_TransitionTexScale = src.m_TransitionTexScale;
			m_TransitionTexOffset = src.m_TransitionTexOffset;
			m_TransitionTexSpeed = src.m_TransitionTexSpeed;
			m_TransitionKeepAspectRatio = src.m_TransitionKeepAspectRatio;
			m_TransitionRotation = src.m_TransitionRotation;
			m_TransitionWidth = src.m_TransitionWidth;
			m_TransitionSoftness = src.m_TransitionSoftness;
			m_TransitionRange = src.m_TransitionRange;
			m_TransitionColor = src.m_TransitionColor;
			m_TransitionColorFilter = src.m_TransitionColorFilter;
			m_TransitionColorGlow = src.m_TransitionColorGlow;
			m_TransitionPatternReverse = src.m_TransitionPatternReverse;
			m_TransitionAutoPlaySpeed = src.m_TransitionAutoPlaySpeed;
			m_TransitionGradient = src.m_TransitionGradient;
			m_TargetMode = src.m_TargetMode;
			m_TargetColor = src.m_TargetColor;
			m_TargetRange = src.m_TargetRange;
			m_TargetSoftness = src.m_TargetSoftness;
			m_SrcBlendMode = src.m_SrcBlendMode;
			m_DstBlendMode = src.m_DstBlendMode;
			m_ShadowMode = src.m_ShadowMode;
			m_ShadowDistance = src.m_ShadowDistance;
			m_ShadowIteration = src.m_ShadowIteration;
			m_ShadowFade = src.m_ShadowFade;
			m_ShadowMirrorScale = src.m_ShadowMirrorScale;
			m_ShadowBlurIntensity = src.m_ShadowBlurIntensity;
			m_ShadowColorFilter = src.m_ShadowColorFilter;
			m_ShadowColor = src.m_ShadowColor;
			m_ShadowColorGlow = src.m_ShadowColorGlow;
			m_EdgeMode = src.m_EdgeMode;
			m_EdgeShinyRate = src.m_EdgeShinyRate;
			m_EdgeWidth = src.m_EdgeWidth;
			m_EdgeColorFilter = src.m_EdgeColorFilter;
			m_EdgeColor = src.m_EdgeColor;
			m_EdgeColorGlow = src.m_EdgeColorGlow;
			m_EdgeShinyAutoPlaySpeed = src.m_EdgeShinyAutoPlaySpeed;
			m_EdgeShinyWidth = src.m_EdgeShinyWidth;
			m_PatternArea = src.m_PatternArea;
			m_GradationMode = src.m_GradationMode;
			m_GradationIntensity = src.m_GradationIntensity;
			m_GradationColorFilter = src.m_GradationColorFilter;
			m_GradationColor1 = src.m_GradationColor1;
			m_GradationColor2 = src.m_GradationColor2;
			m_GradationColor3 = src.m_GradationColor3;
			m_GradationColor4 = src.m_GradationColor4;
			m_GradationGradient = src.m_GradationGradient;
			m_GradationScale = src.m_GradationScale;
			m_GradationOffset = src.m_GradationOffset;
			m_GradationRotation = src.m_GradationRotation;
			m_GradationWrapMode = src.m_GradationWrapMode;
			m_GradationReverse = src.m_GradationReverse;
			m_DetailFilter = src.m_DetailFilter;
			m_DetailIntensity = src.m_DetailIntensity;
			m_DetailColor = src.m_DetailColor;
			m_DetailThreshold = src.m_DetailThreshold;
			m_DetailTex = src.m_DetailTex;
			m_DetailTexScale = src.m_DetailTexScale;
			m_DetailTexOffset = src.m_DetailTexOffset;
			m_DetailTexSpeed = src.m_DetailTexSpeed;
			m_Flip = src.m_Flip;
		}

		public void SetGradationDirty()
		{
			_isGradientDirty = true;
		}

		public void SetTransitionGradationDirty()
		{
			_isTransitionGradientDirty = true;
		}

		public void ApplyToMaterial(Material material, float actualSamplingScale = 1f)
		{
			if (!(material == null))
			{
				material.SetInt(s_SrcBlend, (int)m_SrcBlendMode);
				material.SetInt(s_DstBlend, (int)m_DstBlendMode);
				material.SetFloat(s_ToneIntensity, Mathf.Clamp01(m_ToneIntensity));
				material.SetInt(s_ColorFilter, (int)m_ColorFilter);
				material.SetColor(s_ColorValue, ApplyColorSpace(m_Color));
				material.SetFloat(s_ColorIntensity, Mathf.Clamp01(m_ColorIntensity));
				material.SetInt(s_ColorGlow, m_ColorGlow ? 1 : 0);
				material.SetFloat(s_SamplingIntensity, Mathf.Clamp01(m_SamplingIntensity));
				material.SetFloat(s_SamplingWidth, m_SamplingWidth);
				material.SetFloat(s_SamplingScale, actualSamplingScale);
				material.SetFloat(s_TransitionRate, Mathf.Clamp01(m_TransitionRate));
				material.SetInt(s_TransitionReverse, m_TransitionReverse ? 1 : 0);
				material.SetTexture(s_TransitionTex, (m_TransitionFilter != TransitionFilter.None) ? m_TransitionTex : null);
				material.SetVector(s_TransitionTex_ST, new Vector4(m_TransitionTexScale.x, m_TransitionTexScale.y, m_TransitionTexOffset.x, m_TransitionTexOffset.y));
				material.SetVector(s_TransitionTex_Speed, m_TransitionTexSpeed);
				material.SetFloat(s_TransitionWidth, Mathf.Clamp01(m_TransitionWidth));
				material.SetFloat(s_TransitionSoftness, Mathf.Clamp01(m_TransitionSoftness));
				material.SetVector(s_TransitionRange, new Vector2(m_TransitionRange.min, m_TransitionRange.max));
				material.SetInt(s_TransitionColorFilter, (int)m_TransitionColorFilter);
				material.SetColor(s_TransitionColor, ApplyColorSpace(m_TransitionColor));
				material.SetInt(s_TransitionColorGlow, m_TransitionColorGlow ? 1 : 0);
				material.SetInt(s_TransitionPatternReverse, m_TransitionPatternReverse ? 1 : 0);
				material.SetFloat(s_TransitionAutoPlaySpeed, m_TransitionAutoPlaySpeed);
				material.SetTexture(s_TransitionGradientTex, (m_TransitionFilter == TransitionFilter.Blaze) ? transitionRampTex : null);
				material.SetColor(s_TargetColor, ApplyColorSpace(m_TargetColor));
				material.SetFloat(s_TargetRange, Mathf.Clamp01(m_TargetRange));
				material.SetFloat(s_TargetSoftness, Mathf.Clamp01(m_TargetSoftness));
				SamplingFilter samplingFilter = m_SamplingFilter;
				if ((uint)(samplingFilter - 1) <= 2u)
				{
					material.SetFloat(s_ShadowBlurIntensity, Mathf.Clamp01(m_ShadowBlurIntensity));
				}
				else
				{
					material.SetFloat(s_ShadowBlurIntensity, Mathf.Clamp01(m_SamplingIntensity));
				}
				material.SetInt(s_ShadowColorFilter, (int)m_ShadowColorFilter);
				material.SetColor(s_ShadowColor, ApplyColorSpace(m_ShadowColor));
				material.SetInt(s_ShadowColorGlow, m_ShadowColorGlow ? 1 : 0);
				material.SetFloat(s_EdgeWidth, Mathf.Clamp01(m_EdgeWidth));
				material.SetInt(s_EdgeColorFilter, (int)m_EdgeColorFilter);
				material.SetColor(s_EdgeColor, ApplyColorSpace(m_EdgeColor));
				material.SetInt(s_EdgeColorGlow, m_EdgeColorGlow ? 1 : 0);
				material.SetFloat(s_EdgeShinyRate, Mathf.Clamp01(m_EdgeShinyRate));
				material.SetFloat(s_EdgeShinyWidth, Mathf.Clamp01(m_EdgeShinyWidth));
				material.SetFloat(s_EdgeShinyAutoPlaySpeed, m_EdgeShinyAutoPlaySpeed);
				material.SetInt(s_PatternArea, (int)((m_EdgeMode != EdgeMode.None) ? m_PatternArea : PatternArea.All));
				material.SetTexture(s_GradationTex, (m_GradationMode != GradationMode.None) ? gradationRampTex : null);
				material.SetVector(s_GradationTex_ST, GetGradationScaleAndOffset());
				material.SetFloat(s_GradationIntensity, m_GradationIntensity);
				material.SetInt(s_GradationColorFilter, (int)m_GradationColorFilter);
				material.SetColor(s_GradationColor1, ApplyColorSpace(m_GradationColor1));
				material.SetColor(s_GradationColor2, ApplyColorSpace(m_GradationColor2));
				material.SetColor(s_GradationColor3, ApplyColorSpace(m_GradationColor3));
				material.SetColor(s_GradationColor4, ApplyColorSpace(m_GradationColor4));
				material.SetInt(s_GradationRadial, (m_GradationMode == GradationMode.Radial || m_GradationMode == GradationMode.RadialDetail || m_GradationMode == GradationMode.RadialGradient) ? 1 : 0);
				material.SetFloat(s_DetailIntensity, Mathf.Clamp01(m_DetailIntensity));
				material.SetColor(s_DetailColor, ApplyColorSpace(m_DetailColor));
				material.SetVector(s_DetailThreshold, new Vector2(m_DetailThreshold.min, m_DetailThreshold.max));
				material.SetTexture(s_DetailTex, (m_DetailFilter != DetailFilter.None) ? m_DetailTex : null);
				material.SetVector(s_DetailTex_ST, new Vector4(m_DetailTexScale.x, m_DetailTexScale.y, m_DetailTexOffset.x, m_DetailTexOffset.y));
				material.SetVector(s_DetailTex_Speed, m_DetailTexSpeed);
				SetKeyword(material, s_ToneKeywords, (int)m_ToneFilter);
				SetKeyword(material, s_ColorKeywords, (m_ColorFilter != ColorFilter.None || m_ShadowMode != ShadowMode.None) ? 1 : 0);
				SetKeyword(material, s_SamplingKeywords, (int)m_SamplingFilter);
				SetKeyword(material, s_TransitionKeywords, (int)m_TransitionFilter);
				SetKeyword(material, s_EdgeKeywords, (int)m_EdgeMode);
				SetKeyword(material, s_DetailKeywords, (int)m_DetailFilter);
				SetKeyword(material, s_TargetKeywords, (int)m_TargetMode);
				switch (m_GradationMode)
				{
				case GradationMode.None:
					SetKeyword(material, s_GradationKeywords, 0);
					break;
				case GradationMode.HorizontalGradient:
				case GradationMode.VerticalGradient:
				case GradationMode.AngleGradient:
				case GradationMode.RadialGradient:
					SetKeyword(material, s_GradationKeywords, 1);
					break;
				case GradationMode.Horizontal:
				case GradationMode.Vertical:
				case GradationMode.Radial:
				case GradationMode.RadialDetail:
				case GradationMode.DiagonalToRightBottom:
				case GradationMode.DiagonalToLeftBottom:
				case GradationMode.Angle:
					SetKeyword(material, s_GradationKeywords, 2);
					break;
				case GradationMode.Diagonal:
					SetKeyword(material, s_GradationKeywords, 3);
					break;
				}
			}
		}

		public void SetEnablePreview(bool enable, Material material)
		{
			if (!(material == null))
			{
				material.SetVector(s_TransitionTex_Speed, enable ? ((Vector4)m_TransitionTexSpeed) : Vector4.zero);
				material.SetVector(s_DetailTex_Speed, enable ? ((Vector4)m_DetailTexSpeed) : Vector4.zero);
				material.SetFloat(s_TransitionAutoPlaySpeed, enable ? m_TransitionAutoPlaySpeed : 0f);
				material.SetFloat(s_EdgeShinyAutoPlaySpeed, enable ? m_EdgeShinyAutoPlaySpeed : 0f);
			}
		}

		public void UpdateViewMatrix(Material material, RectTransform transitionRoot, Canvas canvas)
		{
			if (!(material == null))
			{
				Vector2 size = transitionRoot.rect.size;
				Vector3 vector = new Vector3(1f / size.x, 1f / size.y, 1f);
				if (m_TransitionKeepAspectRatio && !Mathf.Approximately(vector.x, vector.y))
				{
					vector.x = (vector.y = Mathf.Min(vector.x, vector.y));
				}
				if ((m_Flip & Flip.Effect) != 0)
				{
					vector.x = (((m_Flip & Flip.Horizontal) != 0) ? (0f - vector.x) : vector.x);
					vector.y = (((m_Flip & Flip.Vertical) != 0) ? (0f - vector.y) : vector.y);
				}
				Vector3 vector2 = new Vector3(0.5f, 0.5f);
				Matrix4x4 matrix4x = Matrix4x4.Translate((transitionRoot.pivot - (Vector2)vector2) * size) * transitionRoot.worldToLocalMatrix;
				Quaternion q = Quaternion.Euler(0f, 0f, m_TransitionRotation);
				float num = 1f / GetMultiplier(m_TransitionRotation);
				material.SetMatrix(s_RootViewMatrix, Matrix4x4.TRS(vector2, q, vector * num) * matrix4x);
				float gradationRotation = GetGradationRotation();
				Quaternion q2 = Quaternion.Euler(0f, 0f, gradationRotation);
				float num2 = 1f / GetMultiplier(gradationRotation);
				material.SetMatrix(s_GradViewMatrix, Matrix4x4.TRS(vector2, q2, vector * num2) * matrix4x);
				if (m_ShadowMode == ShadowMode.Mirror)
				{
					float num3 = 1f / m_ShadowMirrorScale;
					Vector3 pos = new Vector3(0f, size.y / 2f * (num3 + 1f) - m_ShadowDistance.y * num3, 0f);
					Vector3 s = new Vector3(1f, num3, 1f);
					Vector3 vector3 = new Vector3(vector.x, 0f - vector.y, vector.z);
					Matrix4x4 matrix4x2 = Matrix4x4.TRS(pos, Quaternion.identity, s) * matrix4x;
					material.SetMatrix(s_MirrorRootViewMatrix, Matrix4x4.TRS(vector2, q, vector3 * num) * matrix4x2);
					material.SetMatrix(s_MirrorGradViewMatrix, Matrix4x4.TRS(vector2, q2, vector3 * num2) * matrix4x2);
				}
				bool flag = canvas.renderMode == RenderMode.WorldSpace || (canvas.renderMode != RenderMode.ScreenSpaceOverlay && (bool)canvas.worldCamera);
				material.SetMatrix(s_CanvasToWorldMatrix, flag ? Matrix4x4.identity : canvas.transform.localToWorldMatrix);
			}
		}

		private Vector4 GetGradationScaleAndOffset()
		{
			float num = 1f / m_GradationScale;
			switch (m_GradationMode)
			{
			case GradationMode.HorizontalGradient:
			case GradationMode.VerticalGradient:
			case GradationMode.AngleGradient:
				return new Vector4(num, 1f, -0.5f * (num + 1f) - m_GradationOffset, 0f);
			case GradationMode.Radial:
			case GradationMode.RadialDetail:
				return new Vector4(num, 1f, m_GradationOffset, 0f);
			default:
				return new Vector4(num, 1f, m_GradationOffset * (num + 1f) / 2f - num * 0.5f + 0.5f, 0f);
			}
		}

		private float GetGradationRotation()
		{
			switch (m_GradationMode)
			{
			case GradationMode.DiagonalToLeftBottom:
				return 135f;
			case GradationMode.DiagonalToRightBottom:
				return 45f;
			case GradationMode.Vertical:
				return 90f;
			case GradationMode.VerticalGradient:
				return -90f;
			case GradationMode.Angle:
			case GradationMode.AngleGradient:
				return m_GradationRotation;
			default:
				return 0f;
			}
		}

		private static float GetMultiplier(float deg)
		{
			float f = MathF.PI / 180f * deg;
			float num = Mathf.Sin(f);
			float num2 = Mathf.Cos(f);
			return Mathf.Max(Mathf.Abs(num2 - num), Mathf.Abs(num2 + num));
		}

		private static void SetKeyword(Material material, Span<string> keywords, int index)
		{
			int length = keywords.Length;
			if (index < length && !string.IsNullOrEmpty(keywords[index]))
			{
				material.EnableKeyword(keywords[index]);
			}
			for (int i = 0; i < length; i++)
			{
				if (i != index)
				{
					material.DisableKeyword(keywords[i]);
				}
			}
		}

		public void ModifyMesh(Graphic graphic, Canvas canvas, RectTransform transitionRoot, VertexHelper vh, bool canModifyShape)
		{
			ApplyFlipWithoutEffect(vh, m_Flip);
			int currentIndexCount = vh.currentIndexCount;
			if (!willModifyVertex || currentIndexCount == 0)
			{
				return;
			}
			GraphicProxy graphicProxy = GraphicProxy.Find(graphic);
			bool flag = graphicProxy.IsText(graphic);
			graphicProxy.OnPreModifyMesh(graphic, canvas);
			List<UIVertex> list = s_WorkingVertices;
			Vector4 expandSize = graphicProxy.ModifyExpandSize(graphic, GetExpandSize(canModifyShape));
			vh.GetUIVertexStream(list);
			int num = (flag ? 6 : currentIndexCount);
			for (int i = 0; i < currentIndexCount; i += num)
			{
				UIVertexUtil.GetBounds(list, i, num, out var posBounds, out var uvBounds);
				UIVertexUtil.Expand(list, i, num, expandSize, posBounds);
				for (int j = 0; j < num; j++)
				{
					UIVertex uIVertex = list[i + j];
					if (onModifyVertex != null)
					{
						uIVertex = onModifyVertex(uIVertex, uvBounds);
					}
					else
					{
						uIVertex.uv1 = new Vector4(uvBounds.xMin, uvBounds.yMin, uvBounds.xMax, uvBounds.yMax);
					}
					list[i + j] = uIVertex;
				}
			}
			ApplyFlipWithEffect(list, m_Flip);
			ApplyShadow(list, transitionRoot, m_Flip);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		private void ApplyShadow(List<UIVertex> verts, RectTransform transitionRoot, Flip flip)
		{
			Vector2 shadowDistance = m_ShadowDistance;
			if (!(shadowDistance == Vector2.zero))
			{
				if ((flip & Flip.Shadow) != 0)
				{
					shadowDistance.x = (((flip & Flip.Horizontal) != 0) ? (0f - shadowDistance.x) : shadowDistance.x);
					shadowDistance.y = (((flip & Flip.Vertical) != 0) ? (0f - shadowDistance.y) : shadowDistance.y);
				}
				switch (m_ShadowMode)
				{
				case ShadowMode.Shadow:
				case ShadowMode.Shadow3:
				case ShadowMode.Outline:
				case ShadowMode.Outline8:
					ShadowUtil.DoShadow(verts, s_ShadowVectors[(int)m_ShadowMode], shadowDistance, m_ShadowIteration, m_ShadowFade);
					break;
				case ShadowMode.Mirror:
					ShadowUtil.DoMirror(verts, shadowDistance, m_ShadowMirrorScale, m_ShadowFade, transitionRoot);
					break;
				}
			}
		}

		private static void ApplyFlipWithoutEffect(VertexHelper vh, Flip flip)
		{
			if ((flip & Flip.Effect) == 0)
			{
				bool flag = (flip & Flip.Horizontal) != 0;
				bool flag2 = (flip & Flip.Vertical) != 0;
				if (flag || flag2)
				{
					UIVertexUtil.Flip(vh, flag, flag2);
				}
			}
		}

		private static void ApplyFlipWithEffect(List<UIVertex> verts, Flip flip)
		{
			if ((flip & Flip.Effect) != 0)
			{
				bool flag = (flip & Flip.Horizontal) != 0;
				bool flag2 = (flip & Flip.Vertical) != 0;
				if (flag || flag2)
				{
					UIVertexUtil.Flip(verts, flag, flag2);
				}
			}
		}

		private Vector4 GetExpandSize(bool canModifyShape)
		{
			if (!canModifyShape)
			{
				return Vector4.zero;
			}
			Vector4 zero = Vector4.zero;
			switch (m_SamplingFilter)
			{
			case SamplingFilter.BlurFast:
				zero += Vector4.one * 10f;
				break;
			case SamplingFilter.BlurMedium:
				zero += Vector4.one * 15f;
				break;
			case SamplingFilter.BlurDetail:
				zero += Vector4.one * 20f;
				break;
			case SamplingFilter.RgbShift:
				zero.x += 40f;
				zero.z += 40f;
				break;
			}
			switch (m_TransitionFilter)
			{
			case TransitionFilter.Melt:
				zero.y += 40f;
				break;
			case TransitionFilter.Burn:
				zero.w += 40f;
				break;
			}
			return zero;
		}

		private static Color ApplyColorSpace(Color color)
		{
			if (QualitySettings.activeColorSpace != ColorSpace.Linear)
			{
				return color;
			}
			return color.linear;
		}
	}
}
