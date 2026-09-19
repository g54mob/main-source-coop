using UnityEngine;
using VolumetricLightBeam.Scripts.HD;

namespace VLB
{
	public static class UtilsBeamProps
	{
		public static bool CanChangeDuringPlaytime(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.trackChangesDuringPlaytime;
			}
			return true;
		}

		public static Quaternion GetInternalLocalRotation(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.beamInternalLocalRotation;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.beamInternalLocalRotation;
			}
			return Quaternion.identity;
		}

		public static void SetIntensityFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				volumetricLightBeamSD.intensityFromLight = fromLight;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				volumetricLightBeamHd.useIntensityFromAttachedLightSpot = fromLight;
			}
		}

		public static float GetThickness(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamSD.fresnelPow / 10f);
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamHd.sideSoftness / 10f);
			}
			return 0f;
		}

		public static void SetThickness(VolumetricLightBeamAbstractBase self, float value)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				volumetricLightBeamSD.fresnelPow = (1f - value) * 10f;
				return;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				volumetricLightBeamHd.sideSoftness = (1f - value) * 10f;
			}
		}

		public static float GetFallOffEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fallOffEnd;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.fallOffEnd;
			}
			return 0f;
		}

		public static ColorMode GetColorMode(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.usedColorMode;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.colorMode;
			}
			return ColorMode.Flat;
		}

		public static Color GetColorFlat(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.color;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.colorFlat;
			}
			return Color.white;
		}

		public static Gradient GetColorGradient(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.colorGradient;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.colorGradient;
			}
			return null;
		}

		public static void SetColorFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				volumetricLightBeamSD.colorFromLight = fromLight;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				volumetricLightBeamHd.colorFromLight = fromLight;
			}
		}

		public static float GetConeAngle(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneAngle;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.coneAngle;
			}
			return 0f;
		}

		public static void SetSpotAngleFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				volumetricLightBeamSD.spotAngleFromLight = fromLight;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				volumetricLightBeamHd.useSpotAngleFromAttachedLightSpot = fromLight;
			}
		}

		public static float GetConeRadiusStart(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusStart;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.coneRadiusStart;
			}
			return 0f;
		}

		public static float GetConeRadiusEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusEnd;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.coneRadiusEnd;
			}
			return 0f;
		}

		public static int GetSortingLayerID(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingLayerID;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.GetSortingLayerID();
			}
			return 0;
		}

		public static int GetSortingOrder(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingOrder;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.GetSortingOrder();
			}
			return 0;
		}

		public static bool GetFadeOutEnabled(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.isFadeOutEnabled;
			}
			return false;
		}

		public static float GetFadeOutEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fadeOutEnd;
			}
			return 0f;
		}

		public static void SetFallOffEndFromLight(VolumetricLightBeamAbstractBase self, bool fromLight)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				volumetricLightBeamSD.fallOffEndFromLight = fromLight;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				volumetricLightBeamHd.useFallOffEndFromAttachedLightSpot = fromLight;
			}
		}

		public static Dimensions GetDimensions(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.dimensions;
			}
			VolumetricLightBeamHd volumetricLightBeamHd = self as VolumetricLightBeamHd;
			if ((bool)volumetricLightBeamHd)
			{
				return volumetricLightBeamHd.GetDimensions();
			}
			return Dimensions.Dim3D;
		}

		public static int GetGeomSides(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if ((bool)volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.geomSides;
			}
			return Config.Instance.sharedMeshSides;
		}

		public static AttenuationEquation ConvertAttenuation(AttenuationEquationHD value)
		{
			return (AttenuationEquation)value;
		}

		public static AttenuationEquationHD ConvertAttenuation(AttenuationEquation value)
		{
			if (value == AttenuationEquation.Blend)
			{
				return AttenuationEquationHD.Linear;
			}
			return (AttenuationEquationHD)value;
		}
	}
}
