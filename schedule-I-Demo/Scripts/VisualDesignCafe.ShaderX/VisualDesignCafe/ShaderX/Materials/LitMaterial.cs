using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace VisualDesignCafe.ShaderX.Materials
{
	public class LitMaterial : BaseMaterial
	{
		public enum SurfaceMapMethod
		{
			None = 0,
			MetallicGloss = 1,
			Packed = 2
		}

		public enum DoubleSidedNormals
		{
			Same = 0,
			Flip = 1
		}

		[ShaderProperty("_AlphaTest")]
		public bool AlphaTest
		{
			get
			{
				return TryGetFloat("_AlphaTest") == 1f;
			}
			set
			{
				TrySetFloat("_AlphaTest", value ? 1 : 0);
			}
		}

		[ShaderProperty("_AlphaTestThreshold")]
		public float AlphaTestThreshold
		{
			get
			{
				return TryGetFloat("_AlphaTestThreshold");
			}
			set
			{
				TrySetFloat("_AlphaTestThreshold", value);
			}
		}

		[ShaderProperty("_AlphaToMask")]
		public bool AlphaToMask
		{
			get
			{
				return TryGetFloat("_AlphaToMask") > 0f;
			}
			set
			{
				TrySetFloat("_AlphaToMask", value ? 1 : 0);
			}
		}

		[ShaderProperty("_DoubleSidedMode")]
		public bool DoubleSided
		{
			get
			{
				return TryGetFloat("_DoubleSidedMode") == 0f;
			}
			set
			{
				TrySetFloat("_DoubleSidedMode", (!value) ? 2 : 0);
			}
		}

		[ShaderProperty("_DoubleSidedNormalMode")]
		public DoubleSidedNormals DoubleSidedNormalMode
		{
			get
			{
				return (DoubleSidedNormals)TryGetFloat("_DoubleSidedNormalMode");
			}
			set
			{
				TrySetFloat("_DoubleSidedNormalMode", (float)value);
			}
		}

		[ShaderProperty("_Tint")]
		public Color Tint
		{
			get
			{
				return TryGetColor("_Tint");
			}
			set
			{
				TrySetColor("_Tint", value);
			}
		}

		[ShaderProperty("_Albedo")]
		public Texture AlbedoMap
		{
			get
			{
				return TryGetTexture("_Albedo", "_MainTex");
			}
			set
			{
				TrySetTexture(value, "_Albedo", "_MainTex");
			}
		}

		[ShaderProperty("_NormalMap")]
		public Texture NormalMap
		{
			get
			{
				return TryGetTexture("_NormalMap");
			}
			set
			{
				TrySetTexture("_NormalMap", value);
			}
		}

		[ShaderProperty("_NormalMapScale")]
		public float NormalMapScale
		{
			get
			{
				return TryGetFloat("_NormalMapScale");
			}
			set
			{
				TrySetFloat("_NormalMapScale", value);
			}
		}

		[ShaderProperty("_SurfaceMapMethod")]
		public SurfaceMapMethod SurfaceMap
		{
			get
			{
				return (SurfaceMapMethod)TryGetFloat("_SurfaceMapMethod");
			}
			set
			{
				TrySetFloat("_SurfaceMapMethod", (float)value);
			}
		}

		[ShaderProperty("_PackedMap")]
		public Texture PackedMap
		{
			get
			{
				return TryGetTexture("_PackedMap");
			}
			set
			{
				TrySetTexture(value, "_PackedMap");
			}
		}

		[ShaderProperty("_MetallicGlossMap")]
		public Texture MetallicGlossMap
		{
			get
			{
				return TryGetTexture("_MetallicGlossMap");
			}
			set
			{
				TrySetTexture("_MetallicGlossMap", value);
			}
		}

		[ShaderProperty("_OcclusionMap")]
		public Texture OcclusionMap
		{
			get
			{
				return TryGetTexture("_OcclusionMap");
			}
			set
			{
				TrySetTexture("_OcclusionMap", value);
			}
		}

		[ShaderProperty("_GlossRemap")]
		public Vector2 GlossRemap
		{
			get
			{
				return TryGetVector("_GlossRemap");
			}
			set
			{
				TrySetVector("_GlossRemap", value);
			}
		}

		[ShaderProperty("_OcclusionRemap")]
		public Vector2 OcclusionRemap
		{
			get
			{
				return TryGetVector("_OcclusionRemap");
			}
			set
			{
				TrySetVector("_OcclusionRemap", value);
			}
		}

		[ShaderProperty("_Glossiness")]
		public float Glossiness
		{
			get
			{
				return TryGetFloat("_Glossiness");
			}
			set
			{
				TrySetFloat("_Glossiness", value);
			}
		}

		[ShaderProperty("_Metallic")]
		public float Metallic
		{
			get
			{
				return TryGetFloat("_Metallic");
			}
			set
			{
				TrySetFloat("_Metallic", value);
			}
		}

		[ShaderProperty("_EmissionColor")]
		public Color EmissionColor
		{
			get
			{
				return TryGetColor("_EmissionColor");
			}
			set
			{
				TrySetColor("_EmissionColor", value);
			}
		}

		[ShaderProperty("_EmissionMap")]
		public Texture EmissionMap
		{
			get
			{
				return TryGetTexture("_EmissionMap");
			}
			set
			{
				TrySetTexture("_EmissionMap", value);
			}
		}

		[ShaderProperty("_EmissionIntensity")]
		public float EmissionIntensity
		{
			get
			{
				return TryGetFloat("_EmissionIntensity");
			}
			set
			{
				TrySetFloat("_EmissionIntensity", value);
			}
		}

		[ShaderProperty("_EmissionExposureWeight")]
		public float EmissionExposureWeight
		{
			get
			{
				return TryGetFloat("_EmissionExposureWeight");
			}
			set
			{
				TrySetFloat("_EmissionExposureWeight", value);
			}
		}

		[ShaderProperty("_EmissionAffectedByAlbedo")]
		public bool EmissionAffectedByAlbedo
		{
			get
			{
				return TryGetFloat("_EmissionAffectedByAlbedo") == 1f;
			}
			set
			{
				TrySetFloat("_EmissionAffectedByAlbedo", value ? 1f : 0f);
			}
		}

		[ShaderProperty("_Translucency")]
		public bool Translucency
		{
			get
			{
				return TryGetFloat("_Translucency") > 0f;
			}
			set
			{
				TrySetFloat("_Translucency", value ? 1f : 0f);
			}
		}

		[ShaderProperty("_DiffusionProfileAsset")]
		public string DiffusionProfile
		{
			get
			{
				return ConvertVector4ToGUID(TryGetVector("_DiffusionProfileAsset"));
			}
			set
			{
				TrySetVector("_DiffusionProfileAsset", ConvertGUIDToVector4(value));
				DiffusionProfileHash = GetDiffusionProfileHash(value);
			}
		}

		[ShaderProperty("_DiffusionProfileHash")]
		public uint DiffusionProfileHash
		{
			get
			{
				return Asuint(TryGetFloat("_DiffusionProfileHash"));
			}
			set
			{
				TrySetFloat("_DiffusionProfileHash", Asfloat(value));
			}
		}

		[ShaderProperty("_SpecularHighlights")]
		public bool SpecularHighlights
		{
			get
			{
				return TryGetFloat("_SpecularHighlights") == 1f;
			}
			set
			{
				TrySetFloat("_SpecularHighlights", value ? 1 : 0);
			}
		}

		[ShaderProperty("_MotionVectors")]
		public bool MotionVectors
		{
			get
			{
				return TryGetFloat("_MotionVectors") == 1f;
			}
			set
			{
				TrySetFloat("_MotionVectors", value ? 1 : 0);
			}
		}

		[ShaderProperty("_Decals")]
		public bool Decals
		{
			get
			{
				return TryGetFloat("_Decals") == 1f;
			}
			set
			{
				TrySetFloat("_Decals", value ? 1 : 0);
			}
		}

		[ShaderProperty("_TemporalAntiAliasing")]
		public bool TemporalAntiAliasing
		{
			get
			{
				return TryGetFloat("_TemporalAntiAliasing") == 1f;
			}
			set
			{
				TrySetFloat("_TemporalAntiAliasing", value ? 1 : 0);
			}
		}

		public static implicit operator Material(LitMaterial material)
		{
			return material.Material;
		}

		public static implicit operator LitMaterial(Material material)
		{
			return new LitMaterial(material);
		}

		public LitMaterial(Material material)
			: base(material)
		{
		}

		public override void Validate(bool clean)
		{
			if (clean)
			{
				ClearKeywords();
			}
			ValidateAlphaTest();
			ValidateAlphaToMask();
			ValidateNormalMap();
			ValidateSurfaceMap();
			ValidateEmission();
			ValidateSpecularHighlights();
			ValidateTranslucency();
			ValidateDecals();
			ValidatePasses(Translucency, Translucency);
			ValidateRenderQueue();
		}

		private void ClearKeywords()
		{
			Material.shaderKeywords = null;
		}

		private void ValidateSpecularHighlights()
		{
			SetKeyword("_SPECULARHIGHLIGHTS_OFF", !SpecularHighlights);
		}

		private void ValidateAlphaTest()
		{
			SetKeyword("_ALPHATEST", AlphaTest && AlphaTestThreshold > 0f);
		}

		private void ValidateNormalMap()
		{
			SetKeyword("_NORMALMAP", NormalMap != null && NormalMapScale != 0f);
		}

		protected virtual void ValidateTranslucency()
		{
			SetKeyword("_TRANSLUCENCY", Translucency);
		}

		private void ValidateSurfaceMap()
		{
			SetKeyword("_SURFACE_MAP_METALLIC_GLOSS", SurfaceMap == SurfaceMapMethod.MetallicGloss && (MetallicGlossMap != null || OcclusionMap != null));
			SetKeyword("_SURFACE_MAP_PACKED", SurfaceMap == SurfaceMapMethod.Packed && PackedMap != null);
		}

		private void ValidateEmission()
		{
			SetKeyword("_EMISSION", EmissionIntensity != 0f && !ColorExtensions.Equals(EmissionColor, Color.black, ColorComparison.IgnoreAlpha));
		}

		private void ValidateDecals()
		{
			SetKeyword("_DISABLE_DECALS", !Decals);
		}

		private void ValidateAlphaToMask()
		{
			SetKeyword("_ALPHATOMASK_ON", AlphaToMask);
		}

		private void ValidateRenderQueue()
		{
			if (ReflectionHelper.Invoke("UnityEngine.Rendering.HighDefinition.HDRenderQueue", "ChangeType", out int returnValue, (object)1, (object)0, (object)GetKeyword("_ALPHATEST"), (object)(!GetKeyword("_DISABLE_DECALS"))))
			{
				Material.renderQueue = returnValue;
			}
			else if (GetKeyword("_ALPHATEST"))
			{
				Material.renderQueue = Mathf.Clamp(Material.renderQueue, 2450, 2999);
			}
			else
			{
				Material.renderQueue = Mathf.Clamp(Material.renderQueue, 2000, 2449);
			}
		}

		protected override bool GetMotionVectorsEnabled()
		{
			return MotionVectors;
		}

		protected override void ValidatePasses(bool receivesSSR, bool useSplitLighting)
		{
			base.ValidatePasses(receivesSSR, useSplitLighting);
			if (GetKeyword("_ALPHATEST"))
			{
				TrySetInt("_ZTestGBuffer", 3);
				TrySetInt("_ZTestForward", 3);
			}
			else
			{
				TrySetInt("_ZTestGBuffer", 4);
				TrySetInt("_ZTestForward", 4);
			}
		}

		private unsafe Vector4 ConvertGUIDToVector4(string guid)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = byte.Parse(guid.Substring(i * 2, 2), NumberStyles.HexNumber);
			}
			Vector4 result;
			fixed (byte* ptr = array)
			{
				result = *(Vector4*)ptr;
			}
			return result;
		}

		private unsafe string ConvertVector4ToGUID(Vector4 vector)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte* ptr = (byte*)(&vector);
			for (int i = 0; i < 16; i++)
			{
				stringBuilder.Append(ptr[i].ToString("x2"));
			}
			byte[] destination = new byte[16];
			Marshal.Copy((IntPtr)ptr, destination, 0, 16);
			return stringBuilder.ToString();
		}

		private unsafe float Asfloat(uint val)
		{
			return *(float*)(&val);
		}

		private unsafe float Asfloat(int val)
		{
			return *(float*)(&val);
		}

		private unsafe int Asint(float val)
		{
			return *(int*)(&val);
		}

		private unsafe uint Asuint(float val)
		{
			return *(uint*)(&val);
		}

		private uint GetDiffusionProfileHash(string assetGuid)
		{
			if (string.IsNullOrEmpty(assetGuid))
			{
				return 0u;
			}
			uint hashCode = (uint)assetGuid.GetHashCode();
			uint num = hashCode & 0x7FFFFF;
			uint num2 = 128u;
			return (num2 << 23) | num;
		}
	}
}
