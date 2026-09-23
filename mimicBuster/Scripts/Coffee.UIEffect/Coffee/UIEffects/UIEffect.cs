using System;
using System.Collections.Generic;
using Coffee.UIEffectInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace Coffee.UIEffects
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class UIEffect : UIEffectBase
	{
		[SerializeField]
		protected ToneFilter m_ToneFilter;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_ToneIntensity = 1f;

		[SerializeField]
		protected ColorFilter m_ColorFilter;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_ColorIntensity = 1f;

		[SerializeField]
		protected Color m_Color = Color.white;

		[SerializeField]
		protected bool m_ColorGlow;

		[SerializeField]
		protected SamplingFilter m_SamplingFilter;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_SamplingIntensity = 0.5f;

		[Range(0.5f, 10f)]
		[SerializeField]
		protected float m_SamplingWidth = 1f;

		[SerializeField]
		protected float m_SamplingScale = 1f;

		[SerializeField]
		protected TransitionFilter m_TransitionFilter;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_TransitionRate = 0.5f;

		[SerializeField]
		protected bool m_TransitionReverse;

		[SerializeField]
		protected Texture m_TransitionTex;

		[SerializeField]
		protected Vector2 m_TransitionTexScale = new Vector2(1f, 1f);

		[SerializeField]
		protected Vector2 m_TransitionTexOffset = new Vector2(0f, 0f);

		[SerializeField]
		protected Vector2 m_TransitionTexSpeed = new Vector2(0f, 0f);

		[Tooltip("Effect rotation (0–360).\nNOTE: This property is shared between `Transition Filter` and `Detail Filter`.")]
		[SerializeField]
		[Range(0f, 360f)]
		private float m_TransitionRotation;

		[Tooltip("The effect maintains its aspect ratio.\nNOTE: This property is shared between `Transition Filter`, `Gradation Mode`, and `Detail Filter`.")]
		[SerializeField]
		protected bool m_TransitionKeepAspectRatio = true;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_TransitionWidth = 0.2f;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_TransitionSoftness = 0.2f;

		[SerializeField]
		protected MinMax01 m_TransitionRange = new MinMax01(0f, 1f);

		[SerializeField]
		protected ColorFilter m_TransitionColorFilter = ColorFilter.MultiplyAdditive;

		[SerializeField]
		protected Color m_TransitionColor = new Color(0f, 0.5f, 1f, 1f);

		[SerializeField]
		protected bool m_TransitionColorGlow;

		[SerializeField]
		protected bool m_TransitionPatternReverse;

		[Range(-5f, 5f)]
		[SerializeField]
		protected float m_TransitionAutoPlaySpeed;

		[SerializeField]
		private Gradient m_TransitionGradient = new Gradient();

		[SerializeField]
		protected TargetMode m_TargetMode;

		[SerializeField]
		protected Color m_TargetColor = Color.white;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_TargetRange = 0.1f;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_TargetSoftness = 0.5f;

		[SerializeField]
		protected BlendType m_BlendType = BlendType.AlphaBlend;

		[SerializeField]
		protected BlendMode m_SrcBlendMode = BlendMode.One;

		[SerializeField]
		protected BlendMode m_DstBlendMode = BlendMode.OneMinusSrcAlpha;

		[SerializeField]
		protected ShadowMode m_ShadowMode;

		[SerializeField]
		protected Vector2 m_ShadowDistance = new Vector2(1f, -1f);

		[Range(1f, 5f)]
		[SerializeField]
		protected int m_ShadowIteration = 1;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_ShadowFade = 0.9f;

		[Range(0f, 2f)]
		[SerializeField]
		protected float m_ShadowMirrorScale = 0.5f;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_ShadowBlurIntensity = 1f;

		[SerializeField]
		protected ColorFilter m_ShadowColorFilter = ColorFilter.Replace;

		[SerializeField]
		protected Color m_ShadowColor = Color.white;

		[SerializeField]
		protected bool m_ShadowColorGlow;

		[SerializeField]
		protected GradationMode m_GradationMode;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_GradationIntensity = 1f;

		[SerializeField]
		protected GradationColorFilter m_GradationColorFilter = GradationColorFilter.Multiply;

		[SerializeField]
		protected Color m_GradationColor1 = Color.white;

		[SerializeField]
		protected Color m_GradationColor2 = Color.white;

		[SerializeField]
		protected Color m_GradationColor3 = Color.white;

		[SerializeField]
		protected Color m_GradationColor4 = Color.white;

		[SerializeField]
		private Gradient m_GradationGradient = new Gradient();

		[Range(-1f, 1f)]
		[SerializeField]
		protected float m_GradationOffset;

		[SerializeField]
		protected float m_GradationScale = 1f;

		[SerializeField]
		[Range(0f, 360f)]
		private float m_GradationRotation;

		[SerializeField]
		protected TextureWrapMode m_GradationWrapMode;

		[SerializeField]
		protected bool m_GradationReverse;

		[SerializeField]
		protected bool m_AllowToModifyMeshShape = true;

		[SerializeField]
		protected EdgeMode m_EdgeMode;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_EdgeWidth = 0.5f;

		[SerializeField]
		protected ColorFilter m_EdgeColorFilter = ColorFilter.Replace;

		[SerializeField]
		protected Color m_EdgeColor = Color.white;

		[SerializeField]
		protected bool m_EdgeColorGlow;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_EdgeShinyRate = 0.5f;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_EdgeShinyWidth = 0.5f;

		[Range(-5f, 5f)]
		[SerializeField]
		protected float m_EdgeShinyAutoPlaySpeed = 1f;

		[SerializeField]
		protected PatternArea m_PatternArea = PatternArea.Inner;

		[SerializeField]
		protected DetailFilter m_DetailFilter;

		[Range(0f, 1f)]
		[SerializeField]
		protected float m_DetailIntensity = 1f;

		[SerializeField]
		protected MinMax01 m_DetailThreshold = new MinMax01(0f, 1f);

		[SerializeField]
		protected Color m_DetailColor = Color.white;

		[SerializeField]
		protected Texture m_DetailTex;

		[SerializeField]
		protected Vector2 m_DetailTexScale = new Vector2(1f, 1f);

		[SerializeField]
		protected Vector2 m_DetailTexOffset = new Vector2(0f, 0f);

		[SerializeField]
		protected Vector2 m_DetailTexSpeed = new Vector2(0f, 0f);

		[SerializeField]
		protected RectTransform m_CustomRoot;

		[SerializeField]
		protected Flip m_Flip;

		private List<UIEffectReplica> _replicas;

		public ToneFilter toneFilter
		{
			get
			{
				return m_ToneFilter;
			}
			set
			{
				if (m_ToneFilter != value)
				{
					context.m_ToneFilter = (m_ToneFilter = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float toneIntensity
		{
			get
			{
				return m_ToneIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_ToneIntensity, value))
				{
					context.m_ToneIntensity = (m_ToneIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public ColorFilter colorFilter
		{
			get
			{
				return m_ColorFilter;
			}
			set
			{
				if (m_ColorFilter != value)
				{
					context.m_ColorFilter = (m_ColorFilter = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float colorIntensity
		{
			get
			{
				return m_ColorIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_ColorIntensity, value))
				{
					context.m_ColorIntensity = (m_ColorIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Color color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color.a = 1f;
				if (!(m_Color == value))
				{
					context.m_Color = (m_Color = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float colorHueShift
		{
			get
			{
				return color.r;
			}
			set
			{
				Color color = this.color;
				color.r = Mathf.Clamp(value, -0.5f, 0.5f);
				this.color = color;
			}
		}

		public float colorSaturationShift
		{
			get
			{
				return color.g;
			}
			set
			{
				Color color = this.color;
				color.g = Mathf.Clamp(value, -1f, 1f);
				this.color = color;
			}
		}

		public float colorValueShift
		{
			get
			{
				return color.b;
			}
			set
			{
				Color color = this.color;
				color.b = Mathf.Clamp(value, -1f, 1f);
				this.color = color;
			}
		}

		public float colorContrastShift
		{
			get
			{
				return color.r;
			}
			set
			{
				Color color = this.color;
				color.r = Mathf.Clamp(value, -1f, 1f);
				this.color = color;
			}
		}

		public float colorBrightnessShift
		{
			get
			{
				return color.g;
			}
			set
			{
				Color color = this.color;
				color.g = Mathf.Clamp(value, -1f, 1f);
				this.color = color;
			}
		}

		public float colorAlpha
		{
			get
			{
				return color.a;
			}
			set
			{
				Color color = this.color;
				color.a = value;
				this.color = color;
			}
		}

		public bool colorGlow
		{
			get
			{
				return m_ColorGlow;
			}
			set
			{
				if (m_ColorGlow != value)
				{
					context.m_ColorGlow = (m_ColorGlow = value);
					SetMaterialContextDirty();
				}
			}
		}

		public SamplingFilter samplingFilter
		{
			get
			{
				return m_SamplingFilter;
			}
			set
			{
				if (m_SamplingFilter != value)
				{
					context.m_SamplingFilter = (m_SamplingFilter = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float samplingIntensity
		{
			get
			{
				return m_SamplingIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_SamplingIntensity, value))
				{
					context.m_SamplingIntensity = (m_SamplingIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float samplingWidth
		{
			get
			{
				return m_SamplingWidth;
			}
			set
			{
				value = Mathf.Clamp(value, 0.5f, 10f);
				if (!Mathf.Approximately(m_SamplingWidth, value))
				{
					context.m_SamplingWidth = (m_SamplingWidth = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float samplingScale
		{
			get
			{
				return m_SamplingScale;
			}
			set
			{
				value = Mathf.Clamp(value, 0.01f, 100f);
				if (!Mathf.Approximately(m_SamplingScale, value))
				{
					m_SamplingScale = value;
					SetMaterialDirty();
				}
			}
		}

		public override float actualSamplingScale => Mathf.Clamp(m_SamplingScale, 0.01f, 100f);

		public override bool canModifyShape => m_AllowToModifyMeshShape;

		public TransitionFilter transitionFilter
		{
			get
			{
				return m_TransitionFilter;
			}
			set
			{
				if (m_TransitionFilter != value)
				{
					context.m_TransitionFilter = (m_TransitionFilter = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float transitionRate
		{
			get
			{
				return m_TransitionRate;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_TransitionRate, value))
				{
					context.m_TransitionRate = (m_TransitionRate = value);
					SetMaterialContextDirty();
				}
			}
		}

		public bool transitionReverse
		{
			get
			{
				return m_TransitionReverse;
			}
			set
			{
				if (m_TransitionReverse != value)
				{
					context.m_TransitionReverse = (m_TransitionReverse = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Texture transitionTexture
		{
			get
			{
				return m_TransitionTex;
			}
			set
			{
				if (!(m_TransitionTex == value))
				{
					context.m_TransitionTex = (m_TransitionTex = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 transitionTextureScale
		{
			get
			{
				return m_TransitionTexScale;
			}
			set
			{
				if (!(m_TransitionTexScale == value))
				{
					context.m_TransitionTexScale = (m_TransitionTexScale = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 transitionTextureOffset
		{
			get
			{
				return m_TransitionTexOffset;
			}
			set
			{
				if (!(m_TransitionTexOffset == value))
				{
					context.m_TransitionTexOffset = (m_TransitionTexOffset = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 transitionTextureSpeed
		{
			get
			{
				return m_TransitionTexSpeed;
			}
			set
			{
				if (!(m_TransitionTexSpeed == value))
				{
					context.m_TransitionTexSpeed = (m_TransitionTexSpeed = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float transitionRotation
		{
			get
			{
				return m_TransitionRotation;
			}
			set
			{
				if (!Mathf.Approximately(m_TransitionRotation, value))
				{
					context.m_TransitionRotation = (m_TransitionRotation = value);
				}
			}
		}

		public bool transitionKeepAspectRatio
		{
			get
			{
				return m_TransitionKeepAspectRatio;
			}
			set
			{
				if (m_TransitionKeepAspectRatio != value)
				{
					context.m_TransitionKeepAspectRatio = (m_TransitionKeepAspectRatio = value);
				}
			}
		}

		public float transitionWidth
		{
			get
			{
				return m_TransitionWidth;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_TransitionWidth, value))
				{
					context.m_TransitionWidth = (m_TransitionWidth = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float transitionSoftness
		{
			get
			{
				return m_TransitionSoftness;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_TransitionSoftness, value))
				{
					context.m_TransitionSoftness = (m_TransitionSoftness = value);
					SetMaterialContextDirty();
				}
			}
		}

		public MinMax01 transitionRange
		{
			get
			{
				return m_TransitionRange;
			}
			set
			{
				if (!m_TransitionRange.Approximately(value))
				{
					context.m_TransitionRange = (m_TransitionRange = value);
					SetMaterialContextDirty();
				}
			}
		}

		public ColorFilter transitionColorFilter
		{
			get
			{
				return m_TransitionColorFilter;
			}
			set
			{
				if (m_TransitionColorFilter != value)
				{
					context.m_TransitionColorFilter = (m_TransitionColorFilter = value);
					SetMaterialDirty();
				}
			}
		}

		public Color transitionColor
		{
			get
			{
				return m_TransitionColor;
			}
			set
			{
				if (!(m_TransitionColor == value))
				{
					context.m_TransitionColor = (m_TransitionColor = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float transitionColorHueShift
		{
			get
			{
				return transitionColor.r;
			}
			set
			{
				Color color = transitionColor;
				color.r = Mathf.Clamp(value, -0.5f, 0.5f);
				transitionColor = color;
			}
		}

		public float transitionColorSaturationShift
		{
			get
			{
				return transitionColor.g;
			}
			set
			{
				Color color = transitionColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				transitionColor = color;
			}
		}

		public float transitionColorValueShift
		{
			get
			{
				return transitionColor.b;
			}
			set
			{
				Color color = transitionColor;
				color.b = Mathf.Clamp(value, -1f, 1f);
				transitionColor = color;
			}
		}

		public float transitionColorContrastShift
		{
			get
			{
				return transitionColor.r;
			}
			set
			{
				Color color = transitionColor;
				color.r = Mathf.Clamp(value, -1f, 1f);
				transitionColor = color;
			}
		}

		public float transitionColorBrightnessShift
		{
			get
			{
				return transitionColor.g;
			}
			set
			{
				Color color = transitionColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				transitionColor = color;
			}
		}

		public float transitionColorAlpha
		{
			get
			{
				return transitionColor.a;
			}
			set
			{
				Color color = transitionColor;
				color.a = value;
				transitionColor = color;
			}
		}

		public bool transitionColorGlow
		{
			get
			{
				return m_TransitionColorGlow;
			}
			set
			{
				if (m_TransitionColorGlow != value)
				{
					context.m_TransitionColorGlow = (m_TransitionColorGlow = value);
					SetMaterialContextDirty();
				}
			}
		}

		public bool transitionPatternReverse
		{
			get
			{
				return m_TransitionPatternReverse;
			}
			set
			{
				if (m_TransitionPatternReverse != value)
				{
					context.m_TransitionPatternReverse = (m_TransitionPatternReverse = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float transitionAutoPlaySpeed
		{
			get
			{
				return m_TransitionAutoPlaySpeed;
			}
			set
			{
				value = Mathf.Clamp(value, -5f, 5f);
				if (!Mathf.Approximately(m_TransitionAutoPlaySpeed, value))
				{
					context.m_TransitionAutoPlaySpeed = (m_TransitionAutoPlaySpeed = value);
					SetMaterialContextDirty();
				}
			}
		}

		public TargetMode targetMode
		{
			get
			{
				return m_TargetMode;
			}
			set
			{
				if (m_TargetMode != value)
				{
					context.m_TargetMode = (m_TargetMode = value);
					SetMaterialDirty();
				}
			}
		}

		public Color targetColor
		{
			get
			{
				return m_TargetColor;
			}
			set
			{
				if (!(m_TargetColor == value))
				{
					context.m_TargetColor = (m_TargetColor = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float targetRange
		{
			get
			{
				return m_TargetRange;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_TargetRange, value))
				{
					context.m_TargetRange = (m_TargetRange = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float targetSoftness
		{
			get
			{
				return m_TargetSoftness;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_TargetSoftness, value))
				{
					context.m_TargetSoftness = (m_TargetSoftness = value);
					SetMaterialContextDirty();
				}
			}
		}

		public BlendType blendType
		{
			get
			{
				return m_BlendType;
			}
			set
			{
				if (m_BlendType != value)
				{
					(m_SrcBlendMode, m_DstBlendMode) = (type: m_BlendType, src: m_SrcBlendMode, dst: m_DstBlendMode).Convert();
					context.m_SrcBlendMode = m_SrcBlendMode;
					context.m_DstBlendMode = m_DstBlendMode;
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public BlendMode srcBlendMode
		{
			get
			{
				return m_SrcBlendMode;
			}
			set
			{
				if (m_SrcBlendMode != value)
				{
					context.m_SrcBlendMode = (m_SrcBlendMode = value);
					m_BlendType = (src: m_SrcBlendMode, dst: m_DstBlendMode).Convert();
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public BlendMode dstBlendMode
		{
			get
			{
				return m_DstBlendMode;
			}
			set
			{
				if (m_DstBlendMode != value)
				{
					context.m_DstBlendMode = (m_DstBlendMode = value);
					m_BlendType = (src: m_SrcBlendMode, dst: m_DstBlendMode).Convert();
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public ShadowMode shadowMode
		{
			get
			{
				return m_ShadowMode;
			}
			set
			{
				if (m_ShadowMode != value)
				{
					context.m_ShadowMode = (m_ShadowMode = value);
					SetVerticesDirty();
					SetVerticesDirty();
				}
			}
		}

		public Vector2 shadowDistance
		{
			get
			{
				return m_ShadowDistance;
			}
			set
			{
				if (!(m_ShadowDistance == value))
				{
					context.m_ShadowDistance = (m_ShadowDistance = value);
					SetVerticesDirty();
				}
			}
		}

		public float shadowFade
		{
			get
			{
				return m_ShadowFade;
			}
			set
			{
				value = Mathf.Clamp01(value);
				if (!Mathf.Approximately(m_ShadowFade, value))
				{
					context.m_ShadowFade = (m_ShadowFade = value);
					SetVerticesDirty();
				}
			}
		}

		public int shadowIteration
		{
			get
			{
				return m_ShadowIteration;
			}
			set
			{
				value = Mathf.Clamp(value, 1, 5);
				if (m_ShadowIteration != value)
				{
					context.m_ShadowIteration = (m_ShadowIteration = value);
					SetVerticesDirty();
				}
			}
		}

		public float shadowMirrorScale
		{
			get
			{
				return m_ShadowMirrorScale;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 2f);
				if (!Mathf.Approximately(m_ShadowMirrorScale, value))
				{
					context.m_ShadowMirrorScale = (m_ShadowMirrorScale = value);
					SetVerticesDirty();
				}
			}
		}

		public float shadowBlurIntensity
		{
			get
			{
				return m_ShadowBlurIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_ShadowBlurIntensity, value))
				{
					context.m_ShadowBlurIntensity = (m_ShadowBlurIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public ColorFilter shadowColorFilter
		{
			get
			{
				return m_ShadowColorFilter;
			}
			set
			{
				if (m_ShadowColorFilter != value)
				{
					context.m_ShadowColorFilter = (m_ShadowColorFilter = value);
					SetMaterialDirty();
				}
			}
		}

		public Color shadowColor
		{
			get
			{
				return m_ShadowColor;
			}
			set
			{
				if (!(m_ShadowColor == value))
				{
					context.m_ShadowColor = (m_ShadowColor = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float shadowColorHueShift
		{
			get
			{
				return shadowColor.r;
			}
			set
			{
				Color color = shadowColor;
				color.r = Mathf.Clamp(value, -0.5f, 0.5f);
				shadowColor = color;
			}
		}

		public float shadowColorSaturationShift
		{
			get
			{
				return shadowColor.g;
			}
			set
			{
				Color color = shadowColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				shadowColor = color;
			}
		}

		public float shadowColorValueShift
		{
			get
			{
				return shadowColor.b;
			}
			set
			{
				Color color = shadowColor;
				color.b = Mathf.Clamp(value, -1f, 1f);
				shadowColor = color;
			}
		}

		public float shadowColorContrastShift
		{
			get
			{
				return shadowColor.r;
			}
			set
			{
				Color color = shadowColor;
				color.r = Mathf.Clamp(value, -1f, 1f);
				shadowColor = color;
			}
		}

		public float shadowColorBrightnessShift
		{
			get
			{
				return shadowColor.g;
			}
			set
			{
				Color color = shadowColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				shadowColor = color;
			}
		}

		public float shadowColorAlpha
		{
			get
			{
				return shadowColor.a;
			}
			set
			{
				Color color = shadowColor;
				color.a = value;
				shadowColor = color;
			}
		}

		[Obsolete("shadowGlow is deprecated. Use shadowColorGlow instead.", false)]
		public bool shadowGlow
		{
			get
			{
				return m_ShadowColorGlow;
			}
			set
			{
				shadowColorGlow = value;
			}
		}

		public bool shadowColorGlow
		{
			get
			{
				return m_ShadowColorGlow;
			}
			set
			{
				if (m_ShadowColorGlow != value)
				{
					context.m_ShadowColorGlow = (m_ShadowColorGlow = value);
					SetMaterialContextDirty();
				}
			}
		}

		public EdgeMode edgeMode
		{
			get
			{
				return m_EdgeMode;
			}
			set
			{
				if (m_EdgeMode != value)
				{
					context.m_EdgeMode = (m_EdgeMode = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float edgeShinyRate
		{
			get
			{
				return m_EdgeShinyRate;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_EdgeShinyRate, value))
				{
					context.m_EdgeShinyRate = (m_EdgeShinyRate = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float edgeWidth
		{
			get
			{
				return m_EdgeWidth;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_EdgeWidth, value))
				{
					context.m_EdgeWidth = (m_EdgeWidth = value);
					SetMaterialContextDirty();
				}
			}
		}

		public ColorFilter edgeColorFilter
		{
			get
			{
				return m_EdgeColorFilter;
			}
			set
			{
				if (m_EdgeColorFilter != value)
				{
					context.m_EdgeColorFilter = (m_EdgeColorFilter = value);
					SetMaterialDirty();
				}
			}
		}

		public Color edgeColor
		{
			get
			{
				return m_EdgeColor;
			}
			set
			{
				if (!(m_EdgeColor == value))
				{
					context.m_EdgeColor = (m_EdgeColor = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float edgeColorHueShift
		{
			get
			{
				return edgeColor.r;
			}
			set
			{
				Color color = edgeColor;
				color.r = Mathf.Clamp(value, -0.5f, 0.5f);
				edgeColor = color;
			}
		}

		public float edgeColorSaturationShift
		{
			get
			{
				return edgeColor.g;
			}
			set
			{
				Color color = edgeColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				edgeColor = color;
			}
		}

		public float edgeColorValueShift
		{
			get
			{
				return edgeColor.b;
			}
			set
			{
				Color color = edgeColor;
				color.b = Mathf.Clamp(value, -1f, 1f);
				edgeColor = color;
			}
		}

		public float edgeColorContrastShift
		{
			get
			{
				return edgeColor.r;
			}
			set
			{
				Color color = edgeColor;
				color.r = Mathf.Clamp(value, -1f, 1f);
				edgeColor = color;
			}
		}

		public float edgeColorBrightnessShift
		{
			get
			{
				return edgeColor.g;
			}
			set
			{
				Color color = edgeColor;
				color.g = Mathf.Clamp(value, -1f, 1f);
				edgeColor = color;
			}
		}

		public float edgeColorAlpha
		{
			get
			{
				return edgeColor.a;
			}
			set
			{
				Color color = edgeColor;
				color.a = value;
				edgeColor = color;
			}
		}

		public bool edgeColorGlow
		{
			get
			{
				return m_EdgeColorGlow;
			}
			set
			{
				if (m_EdgeColorGlow != value)
				{
					context.m_EdgeColorGlow = (m_EdgeColorGlow = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float edgeShinyWidth
		{
			get
			{
				return m_EdgeShinyWidth;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_EdgeShinyWidth, value))
				{
					context.m_EdgeShinyWidth = (m_EdgeShinyWidth = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float edgeShinyAutoPlaySpeed
		{
			get
			{
				return m_EdgeShinyAutoPlaySpeed;
			}
			set
			{
				value = Mathf.Clamp(value, -5f, 5f);
				if (!Mathf.Approximately(m_EdgeShinyAutoPlaySpeed, value))
				{
					context.m_EdgeShinyAutoPlaySpeed = (m_EdgeShinyAutoPlaySpeed = value);
					SetMaterialContextDirty();
				}
			}
		}

		public PatternArea patternArea
		{
			get
			{
				return m_PatternArea;
			}
			set
			{
				if (m_PatternArea != value)
				{
					context.m_PatternArea = (m_PatternArea = value);
					SetMaterialDirty();
				}
			}
		}

		public GradationMode gradationMode
		{
			get
			{
				return m_GradationMode;
			}
			set
			{
				if (m_GradationMode != value)
				{
					context.m_GradationMode = (m_GradationMode = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float gradationIntensity
		{
			get
			{
				return m_GradationIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_GradationIntensity, value))
				{
					context.m_GradationIntensity = (m_GradationIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public GradationColorFilter gradationColorFilter
		{
			get
			{
				return m_GradationColorFilter;
			}
			set
			{
				if (m_GradationColorFilter != value)
				{
					context.m_GradationColorFilter = (m_GradationColorFilter = value);
					SetMaterialDirty();
				}
			}
		}

		public Color gradationColor1
		{
			get
			{
				return m_GradationColor1;
			}
			set
			{
				if (!(m_GradationColor1 == value))
				{
					context.m_GradationColor1 = (m_GradationColor1 = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Color gradationColor2
		{
			get
			{
				return m_GradationColor2;
			}
			set
			{
				if (!(m_GradationColor2 == value))
				{
					context.m_GradationColor2 = (m_GradationColor2 = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Color gradationColor3
		{
			get
			{
				return m_GradationColor3;
			}
			set
			{
				if (!(m_GradationColor3 == value))
				{
					context.m_GradationColor3 = (m_GradationColor3 = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Color gradationColor4
		{
			get
			{
				return m_GradationColor4;
			}
			set
			{
				if (!(m_GradationColor4 == value))
				{
					context.m_GradationColor4 = (m_GradationColor4 = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float gradationOffset
		{
			get
			{
				return m_GradationOffset;
			}
			set
			{
				if (!Mathf.Approximately(m_GradationOffset, value))
				{
					context.m_GradationOffset = (m_GradationOffset = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float gradationScale
		{
			get
			{
				return m_GradationScale;
			}
			set
			{
				value = Mathf.Clamp(value, 0.01f, 10f);
				if (!Mathf.Approximately(m_GradationScale, value))
				{
					context.m_GradationScale = (m_GradationScale = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float gradationRotation
		{
			get
			{
				return m_GradationRotation;
			}
			set
			{
				value = Mathf.Repeat(value, 360f);
				if (!Mathf.Approximately(m_GradationRotation, value))
				{
					context.m_GradationRotation = (m_GradationRotation = value);
				}
			}
		}

		public TextureWrapMode gradationWrapMode
		{
			get
			{
				return m_GradationWrapMode;
			}
			set
			{
				if (m_GradationWrapMode != value)
				{
					context.m_GradationWrapMode = (m_GradationWrapMode = value);
					context.SetGradationDirty();
					SetMaterialContextDirty();
				}
			}
		}

		public bool gradationReverse
		{
			get
			{
				return m_GradationReverse;
			}
			set
			{
				if (m_GradationReverse != value)
				{
					context.m_GradationReverse = (m_GradationReverse = value);
					context.SetGradationDirty();
					SetMaterialContextDirty();
				}
			}
		}

		public DetailFilter detailFilter
		{
			get
			{
				return m_DetailFilter;
			}
			set
			{
				if (m_DetailFilter != value)
				{
					context.m_DetailFilter = (m_DetailFilter = value);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public float detailIntensity
		{
			get
			{
				return m_DetailIntensity;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(m_DetailIntensity, value))
				{
					context.m_DetailIntensity = (m_DetailIntensity = value);
					SetMaterialContextDirty();
				}
			}
		}

		public MinMax01 detailThreshold
		{
			get
			{
				return m_DetailThreshold;
			}
			set
			{
				if (!m_DetailThreshold.Approximately(value))
				{
					context.m_DetailThreshold = (m_DetailThreshold = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Color detailColor
		{
			get
			{
				return m_DetailColor;
			}
			set
			{
				if (!(m_DetailColor == value))
				{
					context.m_DetailColor = (m_DetailColor = value);
					SetMaterialContextDirty();
				}
			}
		}

		public float detailColorAlpha
		{
			get
			{
				return detailColor.a;
			}
			set
			{
				Color color = detailColor;
				color.a = value;
				detailColor = color;
			}
		}

		public Texture detailTexture
		{
			get
			{
				return m_DetailTex;
			}
			set
			{
				if (!(m_DetailTex == value))
				{
					context.m_DetailTex = (m_DetailTex = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 detailTextureScale
		{
			get
			{
				return m_DetailTexScale;
			}
			set
			{
				if (!(m_DetailTexScale == value))
				{
					context.m_DetailTexScale = (m_DetailTexScale = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 detailTextureOffset
		{
			get
			{
				return m_DetailTexOffset;
			}
			set
			{
				if (!(m_DetailTexOffset == value))
				{
					context.m_DetailTexOffset = (m_DetailTexOffset = value);
					SetMaterialContextDirty();
				}
			}
		}

		public Vector2 detailTextureSpeed
		{
			get
			{
				return m_DetailTexSpeed;
			}
			set
			{
				if (!(m_DetailTexSpeed == value))
				{
					context.m_DetailTexSpeed = (m_DetailTexSpeed = value);
					SetMaterialContextDirty();
				}
			}
		}

		public bool allowToModifyMeshShape
		{
			get
			{
				return m_AllowToModifyMeshShape;
			}
			set
			{
				if (m_AllowToModifyMeshShape != value)
				{
					m_AllowToModifyMeshShape = value;
					SetVerticesDirty();
				}
			}
		}

		public RectTransform customRoot
		{
			get
			{
				return m_CustomRoot;
			}
			set
			{
				m_CustomRoot = value;
			}
		}

		public Flip flip
		{
			get
			{
				return m_Flip;
			}
			set
			{
				if (m_Flip != value)
				{
					context.m_Flip = (m_Flip = value);
					SetVerticesDirty();
				}
			}
		}

		public override RectTransform transitionRoot
		{
			get
			{
				if (!(m_CustomRoot != null))
				{
					return base.transform as RectTransform;
				}
				return m_CustomRoot;
			}
		}

		public List<UIEffectReplica> replicas => _replicas ?? (_replicas = InternalListPool<UIEffectReplica>.Rent());

		protected override void OnEnable()
		{
			(m_SrcBlendMode, m_DstBlendMode) = (type: m_BlendType, src: m_SrcBlendMode, dst: m_DstBlendMode).Convert();
			base.OnEnable();
			SetMaterialDirty();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			SetMaterialDirty();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			InternalListPool<UIEffectReplica>.Return(ref _replicas);
		}

		public override void SetVerticesDirty()
		{
			base.SetVerticesDirty();
			int count = replicas.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(replicas[i] == null))
				{
					replicas[i].SetVerticesDirty();
				}
			}
		}

		public override void SetMaterialDirty()
		{
			base.SetMaterialDirty();
			int count = replicas.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(replicas[i] == null))
				{
					replicas[i].SetMaterialDirty();
				}
			}
		}

		public override void SetMaterialContextDirty()
		{
			base.SetMaterialContextDirty();
			int count = replicas.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(replicas[i] == null))
				{
					replicas[i].SetMaterialContextDirty();
				}
			}
		}

		public void SetGradientKeys(Gradient gradient)
		{
			SetGradientKeys(gradient.colorKeys, gradient.alphaKeys, gradient.mode);
		}

		public void GetGradientKeys(out GradientColorKey[] colorKeys, out GradientAlphaKey[] alphaKeys, out GradientMode mode)
		{
			colorKeys = m_GradationGradient?.colorKeys ?? Array.Empty<GradientColorKey>();
			alphaKeys = m_GradationGradient?.alphaKeys ?? Array.Empty<GradientAlphaKey>();
			mode = m_GradationGradient?.mode ?? GradientMode.Blend;
		}

		public void SetTransitionGradientKeys(Gradient gradient)
		{
			SetTransitionGradientKeys(gradient.colorKeys, gradient.alphaKeys, gradient.mode);
		}

		public void GetTransitionGradientKeys(out GradientColorKey[] colorKeys, out GradientAlphaKey[] alphaKeys, out GradientMode mode)
		{
			colorKeys = m_TransitionGradient?.colorKeys ?? Array.Empty<GradientColorKey>();
			alphaKeys = m_TransitionGradient?.alphaKeys ?? Array.Empty<GradientAlphaKey>();
			mode = m_TransitionGradient?.mode ?? GradientMode.Blend;
		}

		public void SetGradientKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys, GradientMode mode = GradientMode.Blend)
		{
			if (m_GradationGradient == null)
			{
				m_GradationGradient = new Gradient();
			}
			m_GradationGradient.SetKeys(colorKeys, alphaKeys);
			m_GradationGradient.mode = mode;
			context?.SetGradationDirty();
			SetMaterialContextDirty();
		}

		public void SetTransitionGradientKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys, GradientMode mode = GradientMode.Blend)
		{
			if (m_TransitionGradient == null)
			{
				m_TransitionGradient = new Gradient();
			}
			m_TransitionGradient.SetKeys(colorKeys, alphaKeys);
			m_TransitionGradient.mode = mode;
			context?.SetTransitionGradationDirty();
			SetMaterialContextDirty();
		}

		public override void ApplyContextToMaterial(Material material)
		{
			base.ApplyContextToMaterial(material);
			int count = replicas.Count;
			for (int i = 0; i < count; i++)
			{
				if ((bool)replicas[i])
				{
					replicas[i].ApplyContextToMaterial(material);
				}
			}
		}

		public override void SetRate(float rate, UIEffectTweener.CullingMask mask)
		{
			if (toneFilter != ToneFilter.None && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.Tone))
			{
				toneIntensity = rate;
			}
			if (colorFilter != ColorFilter.None && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.Color))
			{
				colorIntensity = rate;
			}
			if (samplingFilter != SamplingFilter.None && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.Sampling))
			{
				samplingIntensity = rate;
			}
			if (transitionFilter != TransitionFilter.None && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.Transition))
			{
				transitionRate = rate;
			}
			if (gradationMode != GradationMode.None && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.GradiationOffset))
			{
				gradationOffset = Mathf.Lerp(-1f, 1f, rate);
			}
			if ((gradationMode == GradationMode.Angle || gradationMode == GradationMode.AngleGradient) && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.GradiationRotation))
			{
				gradationRotation = Mathf.Lerp(0f, 360f, rate);
			}
			if (edgeMode == EdgeMode.Shiny && (UIEffectTweener.CullingMask)0 < (mask & UIEffectTweener.CullingMask.EdgeShiny))
			{
				edgeShinyRate = rate;
			}
		}

		public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			TransitionFilter transitionFilter = this.transitionFilter;
			if (transitionFilter == TransitionFilter.None || (uint)(transitionFilter - 4) <= 1u || transitionFilter == TransitionFilter.Pattern)
			{
				return true;
			}
			return transitionRate < 1f;
		}

		public void Clear()
		{
			m_SamplingScale = 1f;
			m_AllowToModifyMeshShape = true;
			m_CustomRoot = null;
			LoadPreset(UIEffectPreset.GetDefaultPreset());
		}

		public void LoadPreset(string presetName)
		{
			LoadPreset(presetName, append: false);
		}

		public void LoadPreset(string presetName, bool append)
		{
			UnityEngine.Object obj = UIEffectProjectSettings.LoadPreset(presetName);
			if (obj is UIEffect src)
			{
				LoadPreset(src, append);
			}
			else if (obj is UIEffectPreset src2)
			{
				LoadPreset(src2, append);
			}
		}

		public void LoadPreset(UIEffect src)
		{
			LoadPreset(src, append: false);
		}

		public void LoadPreset(UIEffectPreset preset)
		{
			LoadPreset(preset, append: false);
		}

		public void LoadPreset(UIEffect src, bool append)
		{
			if (!(src == null))
			{
				if (!append || src.m_ToneFilter != ToneFilter.None)
				{
					m_ToneFilter = src.m_ToneFilter;
					m_ToneIntensity = src.m_ToneIntensity;
				}
				if (!append || src.m_ColorFilter != ColorFilter.None)
				{
					m_ColorFilter = src.m_ColorFilter;
					m_Color = src.m_Color;
					m_ColorIntensity = src.m_ColorIntensity;
					m_ColorGlow = src.m_ColorGlow;
				}
				if (!append || src.m_SamplingFilter != SamplingFilter.None)
				{
					m_SamplingFilter = src.m_SamplingFilter;
					m_SamplingIntensity = src.m_SamplingIntensity;
					m_SamplingWidth = src.m_SamplingWidth;
				}
				if (!append || src.m_TransitionFilter != TransitionFilter.None)
				{
					m_TransitionFilter = src.m_TransitionFilter;
					m_TransitionRate = src.m_TransitionRate;
					m_TransitionReverse = src.m_TransitionReverse;
					m_TransitionTex = src.m_TransitionTex;
					m_TransitionTexScale = src.m_TransitionTexScale;
					m_TransitionTexOffset = src.m_TransitionTexOffset;
					m_TransitionTexSpeed = src.m_TransitionTexSpeed;
					m_TransitionRotation = src.m_TransitionRotation;
					m_TransitionKeepAspectRatio = src.m_TransitionKeepAspectRatio;
					m_TransitionWidth = src.m_TransitionWidth;
					m_TransitionSoftness = src.m_TransitionSoftness;
					m_TransitionRange = src.m_TransitionRange;
					m_TransitionColorFilter = src.m_TransitionColorFilter;
					m_TransitionColor = src.m_TransitionColor;
					m_TransitionColorGlow = src.m_TransitionColorGlow;
					m_TransitionPatternReverse = src.m_TransitionPatternReverse;
					m_TransitionAutoPlaySpeed = src.m_TransitionAutoPlaySpeed;
					SetTransitionGradientKeys(src.m_TransitionGradient);
				}
				if (!append || src.m_TargetMode != TargetMode.None)
				{
					m_TargetMode = src.m_TargetMode;
					m_TargetColor = src.m_TargetColor;
					m_TargetRange = src.m_TargetRange;
					m_TargetSoftness = src.m_TargetSoftness;
				}
				if (!append || src.m_BlendType != BlendType.AlphaBlend)
				{
					m_BlendType = src.m_BlendType;
					(m_SrcBlendMode, m_DstBlendMode) = (type: m_BlendType, src: src.m_SrcBlendMode, dst: src.m_DstBlendMode).Convert();
				}
				if (!append || src.m_ShadowMode != ShadowMode.None)
				{
					m_ShadowMode = src.m_ShadowMode;
					m_ShadowDistance = src.m_ShadowDistance;
					m_ShadowIteration = src.m_ShadowIteration;
					m_ShadowFade = src.m_ShadowFade;
					m_ShadowMirrorScale = src.m_ShadowMirrorScale;
					m_ShadowBlurIntensity = src.m_ShadowBlurIntensity;
					m_ShadowColorFilter = src.m_ShadowColorFilter;
					m_ShadowColor = src.m_ShadowColor;
					m_ShadowColorGlow = src.m_ShadowColorGlow;
				}
				if (!append || src.m_EdgeMode != EdgeMode.None)
				{
					m_EdgeMode = src.m_EdgeMode;
					m_EdgeShinyRate = src.m_EdgeShinyRate;
					m_EdgeWidth = src.m_EdgeWidth;
					m_EdgeColorFilter = src.m_EdgeColorFilter;
					m_EdgeColor = src.m_EdgeColor;
					m_EdgeColorGlow = src.m_EdgeColorGlow;
					m_EdgeShinyWidth = src.m_EdgeShinyWidth;
					m_EdgeShinyAutoPlaySpeed = src.m_EdgeShinyAutoPlaySpeed;
					m_PatternArea = src.m_PatternArea;
				}
				if (!append || src.m_GradationMode != GradationMode.None)
				{
					m_GradationMode = src.m_GradationMode;
					m_GradationIntensity = src.m_GradationIntensity;
					m_GradationColorFilter = src.m_GradationColorFilter;
					m_GradationColor1 = src.m_GradationColor1;
					m_GradationColor2 = src.m_GradationColor2;
					m_GradationColor3 = src.m_GradationColor3;
					m_GradationColor4 = src.m_GradationColor4;
					SetGradientKeys(src.m_GradationGradient);
					m_GradationOffset = src.m_GradationOffset;
					m_GradationScale = src.m_GradationScale;
					m_GradationRotation = src.m_GradationRotation;
					m_GradationWrapMode = src.m_GradationWrapMode;
					m_GradationReverse = src.m_GradationReverse;
				}
				if (!append || src.m_DetailFilter != DetailFilter.None)
				{
					m_DetailColor = src.m_DetailColor;
					m_DetailFilter = src.m_DetailFilter;
					m_DetailIntensity = src.m_DetailIntensity;
					m_DetailThreshold = src.m_DetailThreshold;
					m_DetailTex = src.m_DetailTex;
					m_DetailTexScale = src.m_DetailTexScale;
					m_DetailTexOffset = src.m_DetailTexOffset;
					m_DetailTexSpeed = src.m_DetailTexSpeed;
				}
				if (!append || src.m_Flip != 0)
				{
					m_Flip = (append ? (m_Flip | src.m_Flip) : src.m_Flip);
				}
				UpdateContext(context);
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public void LoadPreset(UIEffectPreset src, bool append)
		{
			if (!(src == null))
			{
				if (!append || src.m_ToneFilter != ToneFilter.None)
				{
					m_ToneFilter = src.m_ToneFilter;
					m_ToneIntensity = src.m_ToneIntensity;
				}
				if (!append || src.m_ColorFilter != ColorFilter.None)
				{
					m_ColorFilter = src.m_ColorFilter;
					m_Color = src.m_Color;
					m_ColorIntensity = src.m_ColorIntensity;
					m_ColorGlow = src.m_ColorGlow;
				}
				if (!append || src.m_SamplingFilter != SamplingFilter.None)
				{
					m_SamplingFilter = src.m_SamplingFilter;
					m_SamplingIntensity = src.m_SamplingIntensity;
					m_SamplingWidth = src.m_SamplingWidth;
				}
				if (!append || src.m_TransitionFilter != TransitionFilter.None)
				{
					m_TransitionFilter = src.m_TransitionFilter;
					m_TransitionRate = src.m_TransitionRate;
					m_TransitionReverse = src.m_TransitionReverse;
					m_TransitionTex = src.m_TransitionTex;
					m_TransitionTexScale = src.m_TransitionTexScale;
					m_TransitionTexOffset = src.m_TransitionTexOffset;
					m_TransitionTexSpeed = src.m_TransitionTexSpeed;
					m_TransitionRotation = src.m_TransitionRotation;
					m_TransitionKeepAspectRatio = src.m_TransitionKeepAspectRatio;
					m_TransitionWidth = src.m_TransitionWidth;
					m_TransitionSoftness = src.m_TransitionSoftness;
					m_TransitionRange = src.m_TransitionRange;
					m_TransitionColorFilter = src.m_TransitionColorFilter;
					m_TransitionColor = src.m_TransitionColor;
					m_TransitionColorGlow = src.m_TransitionColorGlow;
					m_TransitionPatternReverse = src.m_TransitionPatternReverse;
					m_TransitionAutoPlaySpeed = src.m_TransitionAutoPlaySpeed;
					SetTransitionGradientKeys(src.m_TransitionGradient);
				}
				if (!append || src.m_TargetMode != TargetMode.None)
				{
					m_TargetMode = src.m_TargetMode;
					m_TargetColor = src.m_TargetColor;
					m_TargetRange = src.m_TargetRange;
					m_TargetSoftness = src.m_TargetSoftness;
				}
				if (!append || src.m_BlendType != BlendType.AlphaBlend)
				{
					m_BlendType = src.m_BlendType;
					(m_SrcBlendMode, m_DstBlendMode) = (type: m_BlendType, src: src.m_SrcBlendMode, dst: src.m_DstBlendMode).Convert();
				}
				if (!append || src.m_ShadowMode != ShadowMode.None)
				{
					m_ShadowMode = src.m_ShadowMode;
					m_ShadowDistance = src.m_ShadowDistance;
					m_ShadowIteration = src.m_ShadowIteration;
					m_ShadowFade = src.m_ShadowFade;
					m_ShadowMirrorScale = src.m_ShadowMirrorScale;
					m_ShadowBlurIntensity = src.m_ShadowBlurIntensity;
					m_ShadowColorFilter = src.m_ShadowColorFilter;
					m_ShadowColor = src.m_ShadowColor;
					m_ShadowColorGlow = src.m_ShadowColorGlow;
				}
				if (!append || src.m_EdgeMode != EdgeMode.None)
				{
					m_EdgeMode = src.m_EdgeMode;
					m_EdgeShinyRate = src.m_EdgeShinyRate;
					m_EdgeWidth = src.m_EdgeWidth;
					m_EdgeColorFilter = src.m_EdgeColorFilter;
					m_EdgeColor = src.m_EdgeColor;
					m_EdgeColorGlow = src.m_EdgeColorGlow;
					m_EdgeShinyWidth = src.m_EdgeShinyWidth;
					m_EdgeShinyAutoPlaySpeed = src.m_EdgeShinyAutoPlaySpeed;
					m_PatternArea = src.m_PatternArea;
				}
				if (!append || src.m_GradationMode != GradationMode.None)
				{
					m_GradationMode = src.m_GradationMode;
					m_GradationIntensity = src.m_GradationIntensity;
					m_GradationColorFilter = src.m_GradationColorFilter;
					m_GradationColor1 = src.m_GradationColor1;
					m_GradationColor2 = src.m_GradationColor2;
					m_GradationColor3 = src.m_GradationColor3;
					m_GradationColor4 = src.m_GradationColor4;
					SetGradientKeys(src.m_GradationGradient);
					m_GradationOffset = src.m_GradationOffset;
					m_GradationScale = src.m_GradationScale;
					m_GradationRotation = src.m_GradationRotation;
					m_GradationWrapMode = src.m_GradationWrapMode;
					m_GradationReverse = src.m_GradationReverse;
				}
				if (!append || src.m_DetailFilter != DetailFilter.None)
				{
					m_DetailFilter = src.m_DetailFilter;
					m_DetailIntensity = src.m_DetailIntensity;
					m_DetailColor = src.m_DetailColor;
					m_DetailThreshold = src.m_DetailThreshold;
					m_DetailTex = src.m_DetailTex;
					m_DetailTexScale = src.m_DetailTexScale;
					m_DetailTexOffset = src.m_DetailTexOffset;
					m_DetailTexSpeed = src.m_DetailTexSpeed;
				}
				if (!append || src.m_Flip != 0)
				{
					m_Flip = (append ? (m_Flip | src.m_Flip) : src.m_Flip);
				}
				UpdateContext(context);
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public void SavePreset(UIEffectPreset dst, bool append)
		{
			if (!(dst == null))
			{
				if (!append || m_ToneFilter != ToneFilter.None)
				{
					dst.m_ToneFilter = m_ToneFilter;
					dst.m_ToneIntensity = m_ToneIntensity;
				}
				if (!append || m_ColorFilter != ColorFilter.None)
				{
					dst.m_ColorFilter = m_ColorFilter;
					dst.m_Color = m_Color;
					dst.m_ColorIntensity = m_ColorIntensity;
					dst.m_ColorGlow = m_ColorGlow;
				}
				if (!append || m_SamplingFilter != SamplingFilter.None)
				{
					dst.m_SamplingFilter = m_SamplingFilter;
					dst.m_SamplingIntensity = m_SamplingIntensity;
					dst.m_SamplingWidth = m_SamplingWidth;
				}
				if (!append || m_TransitionFilter != TransitionFilter.None)
				{
					dst.m_TransitionFilter = m_TransitionFilter;
					dst.m_TransitionRate = m_TransitionRate;
					dst.m_TransitionReverse = m_TransitionReverse;
					dst.m_TransitionTex = m_TransitionTex;
					dst.m_TransitionTexScale = m_TransitionTexScale;
					dst.m_TransitionTexOffset = m_TransitionTexOffset;
					dst.m_TransitionTexSpeed = m_TransitionTexSpeed;
					dst.m_TransitionRotation = m_TransitionRotation;
					dst.m_TransitionKeepAspectRatio = m_TransitionKeepAspectRatio;
					dst.m_TransitionWidth = m_TransitionWidth;
					dst.m_TransitionSoftness = m_TransitionSoftness;
					dst.m_TransitionRange = m_TransitionRange;
					dst.m_TransitionColorFilter = m_TransitionColorFilter;
					dst.m_TransitionColor = m_TransitionColor;
					dst.m_TransitionColorGlow = m_TransitionColorGlow;
					dst.m_TransitionPatternReverse = m_TransitionPatternReverse;
					dst.m_TransitionAutoPlaySpeed = m_TransitionAutoPlaySpeed;
					dst.m_TransitionGradient.SetKeys(m_TransitionGradient.colorKeys, m_TransitionGradient.alphaKeys);
					dst.m_TransitionGradient.mode = m_TransitionGradient.mode;
				}
				if (!append || m_TargetMode != TargetMode.None)
				{
					dst.m_TargetMode = m_TargetMode;
					dst.m_TargetColor = m_TargetColor;
					dst.m_TargetRange = m_TargetRange;
					dst.m_TargetSoftness = m_TargetSoftness;
				}
				if (!append || m_BlendType != BlendType.AlphaBlend)
				{
					dst.m_BlendType = m_BlendType;
					(dst.m_SrcBlendMode, dst.m_DstBlendMode) = (type: dst.m_BlendType, src: m_SrcBlendMode, dst: m_DstBlendMode).Convert();
				}
				if (!append || m_ShadowMode != ShadowMode.None)
				{
					dst.m_ShadowMode = m_ShadowMode;
					dst.m_ShadowDistance = m_ShadowDistance;
					dst.m_ShadowIteration = m_ShadowIteration;
					dst.m_ShadowFade = m_ShadowFade;
					dst.m_ShadowMirrorScale = m_ShadowMirrorScale;
					dst.m_ShadowBlurIntensity = m_ShadowBlurIntensity;
					dst.m_ShadowColorFilter = m_ShadowColorFilter;
					dst.m_ShadowColor = m_ShadowColor;
					dst.m_ShadowColorGlow = m_ShadowColorGlow;
				}
				if (!append || m_EdgeMode != EdgeMode.None)
				{
					dst.m_EdgeMode = m_EdgeMode;
					dst.m_EdgeShinyRate = m_EdgeShinyRate;
					dst.m_EdgeWidth = m_EdgeWidth;
					dst.m_EdgeColorFilter = m_EdgeColorFilter;
					dst.m_EdgeColor = m_EdgeColor;
					dst.m_EdgeColorGlow = m_EdgeColorGlow;
					dst.m_EdgeShinyWidth = m_EdgeShinyWidth;
					dst.m_EdgeShinyAutoPlaySpeed = m_EdgeShinyAutoPlaySpeed;
					dst.m_PatternArea = m_PatternArea;
				}
				if (!append || m_GradationMode != GradationMode.None)
				{
					dst.m_GradationMode = m_GradationMode;
					dst.m_GradationIntensity = m_GradationIntensity;
					dst.m_GradationColorFilter = m_GradationColorFilter;
					dst.m_GradationColor1 = m_GradationColor1;
					dst.m_GradationColor2 = m_GradationColor2;
					dst.m_GradationColor3 = m_GradationColor3;
					dst.m_GradationColor4 = m_GradationColor4;
					dst.m_GradationGradient.SetKeys(m_GradationGradient.colorKeys, m_GradationGradient.alphaKeys);
					dst.m_GradationGradient.mode = m_GradationGradient.mode;
					dst.m_GradationOffset = m_GradationOffset;
					dst.m_GradationScale = m_GradationScale;
					dst.m_GradationRotation = m_GradationRotation;
					dst.m_GradationWrapMode = m_GradationWrapMode;
					dst.m_GradationReverse = m_GradationReverse;
				}
				if (!append || m_DetailFilter != DetailFilter.None)
				{
					dst.m_DetailFilter = m_DetailFilter;
					dst.m_DetailIntensity = m_DetailIntensity;
					dst.m_DetailColor = m_DetailColor;
					dst.m_DetailThreshold = m_DetailThreshold;
					dst.m_DetailTex = m_DetailTex;
					dst.m_DetailTexScale = m_DetailTexScale;
					dst.m_DetailTexOffset = m_DetailTexOffset;
					dst.m_DetailTexSpeed = m_DetailTexSpeed;
				}
				if (!append || m_Flip != 0)
				{
					dst.m_Flip = (append ? (dst.m_Flip | m_Flip) : m_Flip);
				}
			}
		}

		internal override void UpdateContext(UIEffectContext dst)
		{
			dst.m_ToneFilter = m_ToneFilter;
			dst.m_ToneIntensity = m_ToneIntensity;
			dst.m_ColorFilter = m_ColorFilter;
			dst.m_Color = m_Color;
			dst.m_ColorIntensity = m_ColorIntensity;
			dst.m_ColorGlow = m_ColorGlow;
			dst.m_SamplingFilter = m_SamplingFilter;
			dst.m_SamplingIntensity = m_SamplingIntensity;
			dst.m_SamplingWidth = m_SamplingWidth;
			dst.m_TransitionFilter = m_TransitionFilter;
			dst.m_TransitionRate = m_TransitionRate;
			dst.m_TransitionReverse = m_TransitionReverse;
			dst.m_TransitionTex = m_TransitionTex;
			dst.m_TransitionTexScale = m_TransitionTexScale;
			dst.m_TransitionTexOffset = m_TransitionTexOffset;
			dst.m_TransitionTexSpeed = m_TransitionTexSpeed;
			dst.m_TransitionRotation = m_TransitionRotation;
			dst.m_TransitionKeepAspectRatio = m_TransitionKeepAspectRatio;
			dst.m_TransitionWidth = m_TransitionWidth;
			dst.m_TransitionSoftness = m_TransitionSoftness;
			dst.m_TransitionRange = m_TransitionRange;
			dst.m_TransitionColorFilter = m_TransitionColorFilter;
			dst.m_TransitionColor = m_TransitionColor;
			dst.m_TransitionColorGlow = m_TransitionColorGlow;
			dst.m_TransitionPatternReverse = m_TransitionPatternReverse;
			dst.m_TransitionAutoPlaySpeed = m_TransitionAutoPlaySpeed;
			dst.m_TransitionGradient = m_TransitionGradient;
			dst.m_TargetMode = m_TargetMode;
			dst.m_TargetColor = m_TargetColor;
			dst.m_TargetRange = m_TargetRange;
			dst.m_TargetSoftness = m_TargetSoftness;
			dst.m_SrcBlendMode = m_SrcBlendMode;
			dst.m_DstBlendMode = m_DstBlendMode;
			dst.m_ShadowMode = m_ShadowMode;
			dst.m_ShadowDistance = m_ShadowDistance;
			dst.m_ShadowIteration = m_ShadowIteration;
			dst.m_ShadowFade = m_ShadowFade;
			dst.m_ShadowMirrorScale = m_ShadowMirrorScale;
			dst.m_ShadowBlurIntensity = m_ShadowBlurIntensity;
			dst.m_ShadowColorFilter = m_ShadowColorFilter;
			dst.m_ShadowColor = m_ShadowColor;
			dst.m_ShadowColorGlow = m_ShadowColorGlow;
			dst.m_EdgeMode = m_EdgeMode;
			dst.m_EdgeShinyRate = m_EdgeShinyRate;
			dst.m_EdgeWidth = m_EdgeWidth;
			dst.m_EdgeColorFilter = m_EdgeColorFilter;
			dst.m_EdgeColor = m_EdgeColor;
			dst.m_EdgeColorGlow = m_EdgeColorGlow;
			dst.m_EdgeShinyWidth = m_EdgeShinyWidth;
			dst.m_EdgeShinyAutoPlaySpeed = m_EdgeShinyAutoPlaySpeed;
			dst.m_PatternArea = m_PatternArea;
			dst.m_GradationMode = m_GradationMode;
			dst.m_GradationIntensity = m_GradationIntensity;
			dst.m_GradationColorFilter = m_GradationColorFilter;
			dst.m_GradationColor1 = m_GradationColor1;
			dst.m_GradationColor2 = m_GradationColor2;
			dst.m_GradationColor3 = m_GradationColor3;
			dst.m_GradationColor4 = m_GradationColor4;
			dst.m_GradationGradient = m_GradationGradient;
			dst.m_GradationOffset = m_GradationOffset;
			dst.m_GradationScale = m_GradationScale;
			dst.m_GradationRotation = m_GradationRotation;
			dst.m_GradationWrapMode = m_GradationWrapMode;
			dst.m_GradationReverse = m_GradationReverse;
			dst.m_DetailFilter = m_DetailFilter;
			dst.m_DetailIntensity = m_DetailIntensity;
			dst.m_DetailColor = m_DetailColor;
			dst.m_DetailThreshold = m_DetailThreshold;
			dst.m_DetailTex = m_DetailTex;
			dst.m_DetailTexScale = m_DetailTexScale;
			dst.m_DetailTexOffset = m_DetailTexOffset;
			dst.m_DetailTexSpeed = m_DetailTexSpeed;
			dst.m_Flip = m_Flip;
		}
	}
}
