using System;
using UnityEngine;
using VisualDesignCafe.ShaderX;
using VisualDesignCafe.ShaderX.Materials;

namespace VisualDesignCafe.Nature
{
	public class NatureMaterial : LitMaterial
	{
		[Obsolete]
		public enum WindMode
		{
			Off = 0,
			Baked = 1,
			Automatic = 2
		}

		public enum ColorCorrectionMode
		{
			Tint = 0,
			HSL = 1
		}

		public enum InteractionMode
		{
			Off = 0,
			Pivot = 1,
			Vertex = 2
		}

		public enum LightingQuality
		{
			High = 0,
			Low = 1
		}

		public enum BlendMode
		{
			Add = 0,
			Overlay = 1
		}

		[ShaderProperty("_ScaleFade")]
		public Vector2 ScaleFade
		{
			get
			{
				return TryGetVector("_ScaleFade");
			}
			set
			{
				TrySetVector("_ScaleFade", value);
			}
		}

		[ShaderProperty("_NatureRendererDistanceControl")]
		public bool NatureRendererControlsDistance
		{
			get
			{
				return TryGetFloat("_NatureRendererDistanceControl") == 1f;
			}
			set
			{
				TrySetFloat("_NatureRendererDistanceControl", value ? 1 : 0);
			}
		}

		[ShaderProperty("_ColorCorrection")]
		public ColorCorrectionMode ColorCorrection
		{
			get
			{
				return (ColorCorrectionMode)TryGetFloat("_ColorCorrection");
			}
			set
			{
				TrySetFloat("_ColorCorrection", (float)value);
			}
		}

		[ShaderProperty("_HSL")]
		public Vector3 HSL
		{
			get
			{
				return TryGetVector("_HSL");
			}
			set
			{
				TrySetVector("_HSL", value);
			}
		}

		[ShaderProperty("_HSLVariation")]
		public Vector3 HSLVariation
		{
			get
			{
				return TryGetVector("_HSLVariation");
			}
			set
			{
				TrySetVector("_HSLVariation", value);
			}
		}

		[ShaderProperty("_TintVariation")]
		public Color TintVariation
		{
			get
			{
				return TryGetColor("_TintVariation");
			}
			set
			{
				TrySetColor("_TintVariation", value);
			}
		}

		[ShaderProperty("_ColorVariationSpread")]
		public float ColorVariationSpread
		{
			get
			{
				return TryGetFloat("_ColorVariationSpread");
			}
			set
			{
				TrySetFloat("_ColorVariationSpread", value);
			}
		}

		[ShaderProperty("_VertexNormalStrength")]
		public float VertexNormalStrength
		{
			get
			{
				return TryGetFloat("_VertexNormalStrength");
			}
			set
			{
				TrySetFloat("_VertexNormalStrength", value);
			}
		}

		[ShaderProperty("_SecondaryMaps")]
		public float SecondaryMaps
		{
			get
			{
				return TryGetFloat("_SecondaryMaps");
			}
			set
			{
				TrySetFloat("_SecondaryMaps", value);
			}
		}

		[ShaderProperty("_SecondaryAlbedo")]
		public Texture SecondaryAlbedoMap
		{
			get
			{
				return TryGetTexture("_SecondaryAlbedo");
			}
			set
			{
				TrySetTexture("_SecondaryAlbedo", value);
			}
		}

		[ShaderProperty("_SecondaryGlossiness")]
		public float SecondaryGlossiness
		{
			get
			{
				return TryGetFloat("_SecondaryGlossiness");
			}
			set
			{
				TrySetFloat("_SecondaryGlossiness", value);
			}
		}

		[ShaderProperty("_SecondaryGlossRemap")]
		public Vector2 SecondaryGlossRemap
		{
			get
			{
				return TryGetVector("_SecondaryGlossRemap");
			}
			set
			{
				TrySetVector("_SecondaryGlossRemap", value);
			}
		}

		[ShaderProperty("_SecondaryMetallic")]
		public float SecondaryMetallic
		{
			get
			{
				return TryGetFloat("_SecondaryMetallic");
			}
			set
			{
				TrySetFloat("_SecondaryMetallic", value);
			}
		}

		[ShaderProperty("_SecondaryMetallicGlossMap")]
		public Texture SecondaryMetallicGlossMap
		{
			get
			{
				return TryGetTexture("_SecondaryMetallicGlossMap");
			}
			set
			{
				TrySetTexture("_SecondaryMetallicGlossMap", value);
			}
		}

		[ShaderProperty("_SecondaryNormalMap")]
		public Texture SecondaryNormalMap
		{
			get
			{
				return TryGetTexture("_SecondaryNormalMap");
			}
			set
			{
				TrySetTexture("_SecondaryNormalMap", value);
			}
		}

		[ShaderProperty("_SecondaryNormalMapScale")]
		public float SecondaryNormalMapScale
		{
			get
			{
				return TryGetFloat("_SecondaryNormalMapScale");
			}
			set
			{
				TrySetFloat("_SecondaryNormalMapScale", value);
			}
		}

		[ShaderProperty("_SecondaryOcclusionMap")]
		public Texture SecondaryOcclusionMap
		{
			get
			{
				return TryGetTexture("_SecondaryOcclusionMap");
			}
			set
			{
				TrySetTexture("_SecondaryOcclusionMap", value);
			}
		}

		[ShaderProperty("_SecondaryOcclusionRemap")]
		public Vector2 SecondaryOcclusionRemap
		{
			get
			{
				return TryGetVector("_SecondaryOcclusionRemap");
			}
			set
			{
				TrySetVector("_SecondaryOcclusionRemap", value);
			}
		}

		[ShaderProperty("_SecondaryPackedMap")]
		public Texture SecondaryPackedMap
		{
			get
			{
				return TryGetTexture("_SecondaryPackedMap");
			}
			set
			{
				TrySetTexture("_SecondaryPackedMap", value);
			}
		}

		public bool BakedMeshData
		{
			get
			{
				return TryGetFloat("_BakedMeshData") > 0f;
			}
			set
			{
				TrySetFloat("_BakedMeshData", value ? 1 : 0);
			}
		}

		public float ObjectHeight
		{
			get
			{
				return TryGetFloat("_ObjectHeight");
			}
			set
			{
				TrySetFloat("_ObjectHeight", value);
			}
		}

		public float ObjectRadius
		{
			get
			{
				return TryGetFloat("_ObjectRadius");
			}
			set
			{
				TrySetFloat("_ObjectRadius", value);
			}
		}

		[ShaderProperty("_Wind")]
		public bool WindEnabled
		{
			get
			{
				return (int)TryGetFloat("_Wind") > 0;
			}
			set
			{
				TrySetFloat("_Wind", value ? 1 : 0);
			}
		}

		[Obsolete("Use 'WindEnabled' and 'BakedMeshData' instead")]
		public WindMode Wind
		{
			get
			{
				return WindEnabled ? (BakedMeshData ? WindMode.Baked : WindMode.Automatic) : WindMode.Off;
			}
			set
			{
				switch (value)
				{
				case WindMode.Automatic:
					WindEnabled = true;
					BakedMeshData = false;
					break;
				case WindMode.Baked:
					WindEnabled = true;
					BakedMeshData = true;
					break;
				case WindMode.Off:
					WindEnabled = false;
					break;
				}
			}
		}

		[ShaderProperty("_WindVariation")]
		public float WindVariation
		{
			get
			{
				return TryGetFloat("_WindVariation");
			}
			set
			{
				TrySetFloat("_WindVariation", value);
			}
		}

		[ShaderProperty("_WindStrength")]
		public float WindStrength
		{
			get
			{
				return TryGetFloat("_WindStrength");
			}
			set
			{
				TrySetFloat("_WindStrength", value);
			}
		}

		[ShaderProperty("_TurbulenceStrength")]
		public float TurbulenceStrength
		{
			get
			{
				return TryGetFloat("_TurbulenceStrength");
			}
			set
			{
				TrySetFloat("_TurbulenceStrength", value);
			}
		}

		[ShaderProperty("_RecalculateWindNormals")]
		public float RecalculateWindNormals
		{
			get
			{
				return TryGetFloat("_RecalculateWindNormals");
			}
			set
			{
				TrySetFloat("_RecalculateWindNormals", value);
			}
		}

		[ShaderProperty("_WindFade")]
		public Vector2 WindFade
		{
			get
			{
				return TryGetVector("_WindFade");
			}
			set
			{
				TrySetVector("_WindFade", value);
			}
		}

		[ShaderProperty("_TrunkBendFactor")]
		public Vector2 TrunkBendFactor
		{
			get
			{
				return TryGetVector("_TrunkBendFactor");
			}
			set
			{
				TrySetVector("_TrunkBendFactor", value);
			}
		}

		[ShaderProperty("_Interaction")]
		public float Interaction
		{
			get
			{
				return TryGetFloat("_Interaction");
			}
			set
			{
				TrySetFloat("_Interaction", value);
			}
		}

		[ShaderProperty("_InteractionDuration")]
		public float InteractionDuration
		{
			get
			{
				return TryGetFloat("_InteractionDuration");
			}
			set
			{
				TrySetFloat("_InteractionDuration", value);
			}
		}

		[ShaderProperty("_InteractionStrength")]
		public float InteractionStrength
		{
			get
			{
				return TryGetFloat("_InteractionStrength");
			}
			set
			{
				TrySetFloat("_InteractionStrength", value);
			}
		}

		[ShaderProperty("_InteractionPushDown")]
		public float InteractionPushDown
		{
			get
			{
				return TryGetFloat("_InteractionPushDown");
			}
			set
			{
				TrySetFloat("_InteractionPushDown", value);
			}
		}

		[ShaderProperty("_TranslucencyBlendMode")]
		public BlendMode TranslucencyBlendMode
		{
			get
			{
				return (BlendMode)TryGetFloat("_TranslucencyBlendMode");
			}
			set
			{
				TrySetFloat("_TranslucencyBlendMode", (float)value);
			}
		}

		[ShaderProperty("_TranslucencyStrength")]
		public float TranslucencyStrength
		{
			get
			{
				return TryGetFloat("_TranslucencyStrength");
			}
			set
			{
				TrySetFloat("_TranslucencyStrength", value);
			}
		}

		[ShaderProperty("_TranslucencyDistortion")]
		public float TranslucencyDistortion
		{
			get
			{
				return TryGetFloat("_TranslucencyDistortion");
			}
			set
			{
				TrySetFloat("_TranslucencyDistortion", value);
			}
		}

		[ShaderProperty("_TranslucencyScattering")]
		public float TranslucencyScattering
		{
			get
			{
				return TryGetFloat("_TranslucencyScattering");
			}
			set
			{
				TrySetFloat("_TranslucencyScattering", value);
			}
		}

		[ShaderProperty("_TranslucencyColor")]
		public float TranslucencyColor
		{
			get
			{
				return TryGetFloat("_TranslucencyColor");
			}
			set
			{
				TrySetFloat("_TranslucencyColor", value);
			}
		}

		[ShaderProperty("_TranslucencyAmbient")]
		public float TranslucencyAmbient
		{
			get
			{
				return TryGetFloat("_TranslucencyAmbient");
			}
			set
			{
				TrySetFloat("_TranslucencyAmbient", value);
			}
		}

		[ShaderProperty("_ThicknessMap")]
		public Texture ThicknessMap
		{
			get
			{
				return TryGetTexture("_ThicknessMap");
			}
			set
			{
				TrySetTexture("_ThicknessMap", value);
			}
		}

		[ShaderProperty("_ThicknessRemap")]
		public Vector2 ThicknessRemap
		{
			get
			{
				return TryGetVector("_ThicknessRemap");
			}
			set
			{
				TrySetVector("_ThicknessRemap", value);
			}
		}

		[ShaderProperty("_Overlay")]
		public bool Overlay
		{
			get
			{
				return TryGetFloat("_Overlay") == 1f;
			}
			set
			{
				TrySetFloat("_Overlay", value ? 1 : 0);
			}
		}

		[ShaderProperty("_SampleAlphaOverlay")]
		public bool SampleAlphaOverlay
		{
			get
			{
				return TryGetFloat("_SampleAlphaOverlay") == 1f;
			}
			set
			{
				TrySetFloat("_SampleAlphaOverlay", value ? 1 : 0);
			}
		}

		[ShaderProperty("_SampleColorOverlay")]
		public bool SampleColorOverlay
		{
			get
			{
				return TryGetFloat("_SampleColorOverlay") == 1f;
			}
			set
			{
				TrySetFloat("_SampleColorOverlay", value ? 1 : 0);
			}
		}

		[ShaderProperty("_PerspectiveCorrection")]
		public float PerspectiveCorrection
		{
			get
			{
				return TryGetFloat("_PerspectiveCorrection");
			}
			set
			{
				TrySetFloat("_PerspectiveCorrection", value);
			}
		}

		[ShaderProperty("_LightingQuality")]
		public LightingQuality Lighting
		{
			get
			{
				return (LightingQuality)TryGetFloat("_LightingQuality");
			}
			set
			{
				TrySetFloat("_LightingQuality", (float)value);
			}
		}

		public NatureMaterial(Material material)
			: base(material)
		{
		}

		public override void Validate(bool clean)
		{
			base.Validate(clean);
			ValidateColorCorrection();
			ValidateMeshData();
			ValidateWind();
			ValidateInteraction();
			ValidateOverlay();
			ValidateTranslucency();
			ValidateLightingQuality();
			ValidateSecondaryMaps();
			ValidatePasses(receivesSSR: false, GetKeyword("_TRANSLUCENCY") || GetKeyword("_TRANSLUCENCY_MAP"));
		}

		private void ValidateMeshData()
		{
			SetKeyword("_BAKED_MESH_DATA", BakedMeshData);
		}

		private void ValidateColorCorrection()
		{
			SetKeyword("_COLOR_TINT", ColorCorrection == ColorCorrectionMode.Tint);
			SetKeyword("_COLOR_HSL", ColorCorrection == ColorCorrectionMode.HSL);
		}

		private void ValidateWind()
		{
			SetKeyword("_WIND_OFF", !WindEnabled);
			SetKeyword("_WIND", WindEnabled);
		}

		private void ValidateInteraction()
		{
			SetKeyword("_INTERACTION_OFF", Interaction < 1f);
			SetKeyword("_INTERACTION", Interaction >= 1f);
		}

		private void ValidateOverlay()
		{
			SetKeyword("_OVERLAY", Overlay);
		}

		protected override void ValidateTranslucency()
		{
			if (Material.HasProperty("_TranslucencyStrength"))
			{
				SetKeyword("_TRANSLUCENCY", base.Translucency && TranslucencyStrength > 0f && ThicknessMap == null);
				SetKeyword("_TRANSLUCENCY_MAP", base.Translucency && TranslucencyStrength > 0f && ThicknessMap != null);
			}
			else
			{
				SetKeyword("_TRANSLUCENCY", base.Translucency && ThicknessMap == null);
				SetKeyword("_TRANSLUCENCY_MAP", base.Translucency && ThicknessMap != null);
			}
		}

		private void ValidateLightingQuality()
		{
			SetKeyword("_LIGHTING_QUALITY_HIGH", Lighting == LightingQuality.High);
			SetKeyword("_LIGHTING_QUALITY_LOW", Lighting == LightingQuality.Low);
		}

		private void ValidateSecondaryMaps()
		{
			bool flag = false;
			switch (base.SurfaceMap)
			{
			case SurfaceMapMethod.MetallicGloss:
				flag = SecondaryMetallicGlossMap != null || SecondaryOcclusionMap != null;
				break;
			case SurfaceMapMethod.Packed:
				flag = SecondaryPackedMap != null;
				break;
			case SurfaceMapMethod.None:
				flag = false;
				break;
			}
			if (SecondaryNormalMap != null)
			{
				flag = true;
			}
			if (SecondaryAlbedoMap != null)
			{
				flag = true;
			}
			SetKeyword("_SECONDARY_MAPS", SecondaryMaps > 0f && flag);
		}
	}
}
