using System;
using UnityEngine;

namespace VisualDesignCafe.ShaderX.Materials
{
	public class BaseMaterial
	{
		private enum StencilUsage
		{
			Clear = 0,
			IsUnlit = 1,
			RequiresDeferredLighting = 2,
			SubsurfaceScattering = 4,
			TraceReflectionRay = 8,
			Decals = 16,
			ObjectMotionVector = 32,
			ExcludeFromTAA = 2,
			DistortionVectors = 4,
			SMAA = 4,
			WaterSurface = 16,
			AfterOpaqueReservedBits = 56,
			UserBit0 = 64,
			UserBit1 = 128,
			HDRPReservedBits = 63
		}

		public readonly Material Material;

		public BaseMaterial(Material material)
		{
			Material = material ?? throw new ArgumentNullException("material");
		}

		public virtual void Validate(bool clean)
		{
		}

		protected void SetKeyword(string keyword, bool isEnabled)
		{
			if (isEnabled)
			{
				if (!Material.IsKeywordEnabled(keyword))
				{
					Material.EnableKeyword(keyword);
					SetDirty();
				}
			}
			else if (Material.IsKeywordEnabled(keyword))
			{
				Material.DisableKeyword(keyword);
				SetDirty();
			}
		}

		protected bool GetKeyword(string keyword)
		{
			return Material.IsKeywordEnabled(keyword);
		}

		protected bool TrySetInt(string propertyName, int value, Action validate = null)
		{
			if (Material.HasProperty(propertyName))
			{
				if (Material.GetInt(propertyName) != value)
				{
					Material.SetInt(propertyName, value);
					SetDirty();
					validate?.Invoke();
				}
				return true;
			}
			return false;
		}

		protected int TryGetInt(string propertyName)
		{
			return Material.HasProperty(propertyName) ? Material.GetInt(propertyName) : 0;
		}

		protected bool TrySetFloat(string propertyName, float value, Action validate = null)
		{
			if (Material.HasProperty(propertyName))
			{
				if (Material.GetFloat(propertyName) != value)
				{
					Material.SetFloat(propertyName, value);
					SetDirty();
					validate?.Invoke();
				}
				return true;
			}
			return false;
		}

		protected float TryGetFloat(string propertyName)
		{
			return Material.HasProperty(propertyName) ? Material.GetFloat(propertyName) : 0f;
		}

		protected bool TrySetTexture(string propertyName, Texture value, Action validate = null)
		{
			if (Material.HasProperty(propertyName))
			{
				if (Material.GetTexture(propertyName) != value)
				{
					Material.SetTexture(propertyName, value);
					SetDirty();
					validate?.Invoke();
				}
				return true;
			}
			return false;
		}

		protected bool TrySetTexture(Texture value, params string[] propertyName)
		{
			bool result = false;
			for (int i = 0; i < propertyName.Length; i++)
			{
				if (TrySetTexture(propertyName[i], value))
				{
					result = true;
				}
			}
			return result;
		}

		protected Texture TryGetTexture(string propertyName)
		{
			return Material.HasProperty(propertyName) ? Material.GetTexture(propertyName) : null;
		}

		protected Texture TryGetTexture(params string[] propertyName)
		{
			for (int i = 0; i < propertyName.Length; i++)
			{
				if (Material.HasProperty(propertyName[i]))
				{
					return Material.GetTexture(propertyName[i]);
				}
			}
			return null;
		}

		protected bool TrySetColor(string propertyName, Color value, Action validate = null)
		{
			if (Material.HasProperty(propertyName))
			{
				if (Material.GetColor(propertyName) != value)
				{
					Material.SetColor(propertyName, value);
					SetDirty();
					validate?.Invoke();
				}
				return true;
			}
			return false;
		}

		protected bool TrySetColor(Color value, params string[] propertyName)
		{
			bool result = false;
			for (int i = 0; i < propertyName.Length; i++)
			{
				if (TrySetColor(propertyName[i], value))
				{
					result = true;
				}
			}
			return result;
		}

		protected Color TryGetColor(string propertyName)
		{
			return Material.HasProperty(propertyName) ? Material.GetColor(propertyName) : Color.clear;
		}

		protected Color TryGetColor(params string[] propertyName)
		{
			for (int i = 0; i < propertyName.Length; i++)
			{
				if (Material.HasProperty(propertyName[i]))
				{
					return Material.GetColor(propertyName[i]);
				}
			}
			return Color.clear;
		}

		protected bool TrySetVector(string propertyName, Vector4 value, Action validate = null)
		{
			if (Material.HasProperty(propertyName))
			{
				if (Material.GetVector(propertyName) != value)
				{
					Material.SetVector(propertyName, value);
					SetDirty();
					validate?.Invoke();
				}
				return true;
			}
			return false;
		}

		protected Vector4 TryGetVector(string propertyName)
		{
			return Material.HasProperty(propertyName) ? Material.GetVector(propertyName) : Vector4.zero;
		}

		protected void SetDirty()
		{
			if (Application.isEditor && (!Application.isPlaying || (ReflectionHelper.Invoke("UnityEditor.EditorUtility", "IsPersistent", out bool returnValue, (object)Material) && returnValue)))
			{
				ReflectionHelper.Invoke("UnityEditor.EditorUtility", "SetDirty", Material);
			}
		}

		protected virtual bool GetMotionVectorsEnabled()
		{
			return TryGetFloat("_MotionVectors") > 0f;
		}

		protected virtual void ValidatePasses(bool receivesSSR, bool useSplitLighting)
		{
			SetupStencil(receivesLighting: true, receivesSSR, useSplitLighting);
			Material.SetShaderPassEnabled("MotionVectors", GetMotionVectorsEnabled());
			Material.SetShaderPassEnabled("DistortionVectors", enabled: false);
			Material.SetShaderPassEnabled("TransparentDepthPrepass", enabled: false);
			Material.SetShaderPassEnabled("TransparentDepthPostpass", enabled: false);
			Material.SetShaderPassEnabled("TransparentBackface", enabled: false);
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

		private void ComputeStencilProperties(bool receivesLighting, bool forwardOnly, bool receivesSSR, bool useSplitLighting, out int stencilRef, out int stencilWriteMask, out int stencilRefDepth, out int stencilWriteMaskDepth, out int stencilRefGBuffer, out int stencilWriteMaskGBuffer, out int stencilRefMV, out int stencilWriteMaskMV)
		{
			stencilRef = 0;
			stencilWriteMask = 6;
			stencilRefDepth = 0;
			stencilWriteMaskDepth = 0;
			stencilRefGBuffer = 2;
			stencilWriteMaskGBuffer = 6;
			stencilRefMV = 32;
			stencilWriteMaskMV = 32;
			if (forwardOnly)
			{
				stencilWriteMaskMV |= 2;
			}
			if (useSplitLighting)
			{
				stencilRefGBuffer |= 4;
				stencilRef |= 4;
			}
			if (receivesSSR)
			{
				stencilRefDepth |= 8;
				stencilRefGBuffer |= 8;
				stencilRefMV |= 8;
			}
			stencilWriteMaskDepth |= 8;
			stencilWriteMaskGBuffer |= 8;
			stencilWriteMaskMV |= 8;
			if (!receivesLighting)
			{
				stencilRefDepth |= 1;
				stencilWriteMaskDepth |= 1;
				stencilRefMV |= 1;
			}
			stencilWriteMaskDepth |= 1;
			stencilWriteMaskGBuffer |= 1;
			stencilWriteMaskMV |= 1;
		}

		private void SetupStencil(bool receivesLighting, bool receivesSSR, bool useSplitLighting)
		{
			bool forwardOnly = false;
			ComputeStencilProperties(receivesLighting, forwardOnly, receivesSSR, useSplitLighting, out var stencilRef, out var stencilWriteMask, out var stencilRefDepth, out var stencilWriteMaskDepth, out var stencilRefGBuffer, out var stencilWriteMaskGBuffer, out var stencilRefMV, out var stencilWriteMaskMV);
			if (Material.HasProperty("_StencilRef"))
			{
				Material.SetInt("_StencilRef", stencilRef);
				Material.SetInt("_StencilWriteMask", stencilWriteMask);
			}
			if (Material.HasProperty("_StencilRefDepth"))
			{
				Material.SetInt("_StencilRefDepth", stencilRefDepth);
				Material.SetInt("_StencilWriteMaskDepth", stencilWriteMaskDepth);
			}
			if (Material.HasProperty("_StencilRefGBuffer"))
			{
				Material.SetInt("_StencilRefGBuffer", stencilRefGBuffer);
				Material.SetInt("_StencilWriteMaskGBuffer", stencilWriteMaskGBuffer);
			}
			if (Material.HasProperty("_StencilRefDistortionVec"))
			{
				Material.SetInt("_StencilRefDistortionVec", 4);
				Material.SetInt("_StencilWriteMaskDistortionVec", 4);
			}
			if (Material.HasProperty("_StencilRefMV"))
			{
				Material.SetInt("_StencilRefMV", stencilRefMV);
				Material.SetInt("_StencilWriteMaskMV", stencilWriteMaskMV);
			}
		}
	}
}
