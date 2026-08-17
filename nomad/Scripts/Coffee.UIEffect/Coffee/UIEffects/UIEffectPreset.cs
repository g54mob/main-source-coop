using Coffee.UIEffectInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace Coffee.UIEffects
{
	[CreateAssetMenu]
	[ExecuteAlways]
	public class UIEffectPreset : ScriptableObject
	{
		internal static UIEffectPreset s_DefaultPreset;

		public ToneFilter m_ToneFilter;

		[Range(0f, 1f)]
		public float m_ToneIntensity = 1f;

		public ColorFilter m_ColorFilter;

		[Range(0f, 1f)]
		public float m_ColorIntensity = 1f;

		public Color m_Color = Color.white;

		public bool m_ColorGlow;

		public SamplingFilter m_SamplingFilter;

		[Range(0f, 1f)]
		public float m_SamplingIntensity = 0.5f;

		[Range(0.5f, 10f)]
		public float m_SamplingWidth = 1f;

		public TransitionFilter m_TransitionFilter;

		[Range(0f, 1f)]
		public float m_TransitionRate = 0.5f;

		public bool m_TransitionReverse;

		public Texture m_TransitionTex;

		public Vector2 m_TransitionTexScale = new Vector2(1f, 1f);

		public Vector2 m_TransitionTexOffset = new Vector2(0f, 0f);

		public Vector2 m_TransitionTexSpeed = new Vector2(0f, 0f);

		[Range(0f, 360f)]
		public float m_TransitionRotation;

		public bool m_TransitionKeepAspectRatio = true;

		[Range(0f, 1f)]
		public float m_TransitionWidth = 0.2f;

		[Range(0f, 1f)]
		public float m_TransitionSoftness = 0.2f;

		public MinMax01 m_TransitionRange = new MinMax01(0f, 1f);

		public ColorFilter m_TransitionColorFilter = ColorFilter.MultiplyAdditive;

		public Color m_TransitionColor = new Color(0f, 0.5f, 1f, 1f);

		public bool m_TransitionColorGlow;

		public bool m_TransitionPatternReverse;

		[Range(-5f, 5f)]
		public float m_TransitionAutoPlaySpeed;

		public Gradient m_TransitionGradient = new Gradient();

		public TargetMode m_TargetMode;

		public Color m_TargetColor = Color.white;

		[Range(0f, 1f)]
		public float m_TargetRange = 0.1f;

		[Range(0f, 1f)]
		public float m_TargetSoftness = 0.5f;

		public BlendType m_BlendType = BlendType.AlphaBlend;

		public BlendMode m_SrcBlendMode = BlendMode.One;

		public BlendMode m_DstBlendMode = BlendMode.OneMinusSrcAlpha;

		public ShadowMode m_ShadowMode;

		public Vector2 m_ShadowDistance = new Vector2(1f, -1f);

		[Range(1f, 5f)]
		public int m_ShadowIteration = 1;

		[Range(0f, 1f)]
		public float m_ShadowFade = 0.9f;

		[Range(0f, 2f)]
		public float m_ShadowMirrorScale = 0.5f;

		[Range(0f, 1f)]
		public float m_ShadowBlurIntensity = 1f;

		public ColorFilter m_ShadowColorFilter = ColorFilter.Replace;

		public Color m_ShadowColor = Color.white;

		public bool m_ShadowColorGlow;

		public GradationMode m_GradationMode;

		[Range(0f, 1f)]
		public float m_GradationIntensity = 1f;

		public GradationColorFilter m_GradationColorFilter = GradationColorFilter.Multiply;

		public Color m_GradationColor1 = Color.white;

		public Color m_GradationColor2 = Color.white;

		public Color m_GradationColor3 = Color.white;

		public Color m_GradationColor4 = Color.white;

		public Gradient m_GradationGradient = new Gradient();

		[Range(-1f, 1f)]
		public float m_GradationOffset;

		public float m_GradationScale = 1f;

		[Range(0f, 360f)]
		public float m_GradationRotation;

		public TextureWrapMode m_GradationWrapMode;

		public bool m_GradationReverse;

		public EdgeMode m_EdgeMode;

		[Range(0f, 1f)]
		public float m_EdgeWidth = 0.5f;

		public ColorFilter m_EdgeColorFilter = ColorFilter.Replace;

		public Color m_EdgeColor = Color.white;

		public bool m_EdgeColorGlow;

		[Range(0f, 1f)]
		public float m_EdgeShinyRate = 0.5f;

		[Range(0f, 1f)]
		public float m_EdgeShinyWidth = 0.5f;

		[Range(-5f, 5f)]
		public float m_EdgeShinyAutoPlaySpeed = 1f;

		public PatternArea m_PatternArea = PatternArea.Inner;

		public DetailFilter m_DetailFilter;

		[Range(0f, 1f)]
		public float m_DetailIntensity = 1f;

		public Color m_DetailColor = Color.white;

		public MinMax01 m_DetailThreshold = new MinMax01(0f, 1f);

		public Texture m_DetailTex;

		public Vector2 m_DetailTexScale = new Vector2(1f, 1f);

		public Vector2 m_DetailTexOffset = new Vector2(0f, 0f);

		public Vector2 m_DetailTexSpeed = new Vector2(0f, 0f);

		public Flip m_Flip;

		internal static UIEffectPreset GetDefaultPreset()
		{
			if (!s_DefaultPreset)
			{
				s_DefaultPreset = ScriptableObject.CreateInstance<UIEffectPreset>();
				s_DefaultPreset.hideFlags = HideFlags.HideAndDontSave;
			}
			return s_DefaultPreset;
		}

		public void UpdateContext(UIEffectContext dst)
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
