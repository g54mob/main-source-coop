using System.Collections.Generic;
using System.IO;
using Boxophobic.Utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheVisualEngine
{
	public class TVEUtils
	{
		public static void SetMaterialSettings(Material material)
		{
			if (material.HasProperty("_IsTVEShader"))
			{
				SetMaterialUpgrade(material);
				SetMaterialRuntime(material);
				SetMaterialInternal(material);
			}
		}

		public static void SetMaterialLegacy(Material material)
		{
			_ = material.shader.name;
			if (!material.HasProperty("_IsVersion"))
			{
				return;
			}
			int num = material.GetInt("_IsVersion");
			if (num == 1200)
			{
				num = 1050;
			}
			if (num < 900)
			{
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Cross Vertex Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Vertex Lit (Mobile)");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
					material.SetFloat("_MotionValue_20", 0f);
					material.SetFloat("_MotionValue_30", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Cross Simple Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Simple Lit (Mobile)");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
					material.SetFloat("_MotionValue_20", 0f);
					material.SetFloat("_MotionValue_30", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Grass Vertex Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Vertex Lit (Mobile)");
					material.SetFloat("_MotionValue_20", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Grass Simple Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Simple Lit (Mobile)");
					material.SetFloat("_MotionValue_20", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Uber Vertex Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Vertex Lit (Mobile)");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Uber Simple Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Simple Lit (Mobile)");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Cross Standard Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Standard Lit");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
					material.SetFloat("_MotionValue_20", 0f);
					material.SetFloat("_MotionValue_30", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Cross Subsurface Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Subsurface Lit");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
					material.SetFloat("_MotionValue_20", 0f);
					material.SetFloat("_MotionValue_30", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Grass Standard Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Standard Lit");
					material.SetFloat("_MotionValue_20", 0f);
					material.SetFloat("_MotionValue_30", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Grass Subsurface Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Subsurface Lit");
					material.SetFloat("_MotionValue_20", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Uber Standard Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Standard Lit");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Uber Subsurface Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Subsurface Lit");
					material.SetFloat("_SizeFadeStartValue", 0f);
					material.SetFloat("_SizeFadeEndValue", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Prop Standard Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry Standard Lit");
					material.SetFloat("_TintingIntensityValue", 0f);
					material.SetFloat("_DrynessIntensityValue", 0f);
					material.SetFloat("_ScaleIntensityValue", 0f);
					material.SetFloat("_MotionIntenityValue", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Prop Subsurface Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry Subsurface Lit");
					material.SetFloat("_TintingIntensityValue", 0f);
					material.SetFloat("_DrynessIntensityValue", 0f);
					material.SetFloat("_ScaleIntensityValue", 0f);
					material.SetFloat("_MotionIntenityValue", 0f);
				}
			}
			if (num < 1100)
			{
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Bark Standard Lit")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Standard Lit");
					material.SetFloat("_GlobalColors", 0f);
					material.SetFloat("_GlobalAlpha", 0f);
					material.SetFloat("_SubsurfaceValue", 0f);
					material.SetFloat("_MotionAmplitude_32", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Bark Vertex Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Vertex Lit (Mobile)");
					material.SetFloat("_GlobalColors", 0f);
					material.SetFloat("_GlobalAlpha", 0f);
					material.SetFloat("_SubsurfaceValue", 0f);
					material.SetFloat("_MotionAmplitude_32", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Bark Simple Lit (Mobile)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Simple Lit (Mobile)");
					material.SetFloat("_GlobalColors", 0f);
					material.SetFloat("_GlobalAlpha", 0f);
					material.SetFloat("_SubsurfaceValue", 0f);
					material.SetFloat("_MotionAmplitude_32", 0f);
				}
				if (material.shader.name == "Hidden/BOXOPHOBIC/The Visual Engine/Geometry/Bark Standard Lit (Blanket)")
				{
					material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Geometry/Plant Standard Lit (Blanket)");
					material.SetFloat("_GlobalColors", 0f);
					material.SetFloat("_GlobalAlpha", 0f);
					material.SetFloat("_SubsurfaceValue", 0f);
					material.SetFloat("_MotionAmplitude_32", 0f);
				}
				if (material.shader.name.Contains("Prop"))
				{
					material.SetFloat("_GlobalColors", 0f);
					material.SetFloat("_GlobalAlpha", 0f);
				}
			}
			if (num < 500)
			{
				if (material.HasProperty("_RenderPriority") && material.GetInt("_RenderPriority") != 0)
				{
					material.SetInt("_RenderQueue", 1);
				}
				material.SetInt("_IsVersion", 500);
			}
			if (num < 600)
			{
				if (material.HasProperty("_LayerReactValue"))
				{
					material.SetInt("_LayerVertexValue", material.GetInt("_LayerReactValue"));
				}
				material.SetInt("_IsVersion", 600);
			}
			if (num < 620)
			{
				if (material.HasProperty("_VertexRollingMode"))
				{
					material.SetInt("_MotionValue_20", material.GetInt("_VertexRollingMode"));
				}
				material.SetInt("_IsVersion", 620);
			}
			if (num < 630)
			{
				material.DisableKeyword("TVE_DETAIL_BLEND_OVERLAY");
				material.DisableKeyword("TVE_DETAIL_BLEND_REPLACE");
				material.SetInt("_IsVersion", 630);
			}
			if (num < 640)
			{
				if (material.HasProperty("_Cutoff"))
				{
					material.SetFloat("_AlphaCutoffValue", material.GetFloat("_Cutoff"));
				}
				material.SetInt("_IsVersion", 640);
			}
			if (num < 650)
			{
				if (material.HasProperty("_Cutoff"))
				{
					material.SetFloat("_AlphaClipValue", material.GetFloat("_Cutoff"));
				}
				if (material.HasProperty("_MotionValue_20"))
				{
					material.SetFloat("_MotionValue_20", 1f);
				}
				if (material.HasProperty("_MotionScale_20") && material.HasProperty("_MaxBoundsInfo"))
				{
					float num2 = Mathf.Round(1f / material.GetVector("_MaxBoundsInfo").y * 10f * 0.5f * 10f) / 10f;
					if (num2 > 1f)
					{
						num2 = Mathf.Clamp(Mathf.FloorToInt(num2), 0, 20);
					}
					material.SetFloat("_MotionScale_20", num2);
				}
				if (material.shader.name.Contains("Bark"))
				{
					material.SetFloat("_DetailCoordMode", 1f);
				}
				material.DisableKeyword("TVE_ALPHA_CLIP");
				material.DisableKeyword("TVE_DETAIL_MODE_ON");
				material.DisableKeyword("TVE_DETAIL_MODE_OFF");
				material.DisableKeyword("TVE_DETAIL_TYPE_VERTEX_BLUE");
				material.DisableKeyword("TVE_DETAIL_TYPE_PROJECTION");
				material.DisableKeyword("TVE_IS_VEGETATION_SHADER");
				material.DisableKeyword("TVE_IS_GRASS_SHADER");
				material.SetInt("_IsVersion", 650);
			}
			if (num < 710)
			{
				if (material.HasProperty("_MotionScale_20"))
				{
					float num3 = material.GetFloat("_MotionScale_20");
					material.SetFloat("_MotionScale_20", num3 * 10f);
				}
				material.SetInt("_IsVersion", 710);
			}
			if (num < 800)
			{
				if (material.HasProperty("_ColorsMaskMinValue") && material.HasProperty("_ColorsMaskMaxValue"))
				{
					float value = material.GetFloat("_ColorsMaskMinValue");
					float value2 = material.GetFloat("_ColorsMaskMaxValue");
					material.SetFloat("_MainMaskMinValue", value);
					material.SetFloat("_MainMaskMaxValue", value2);
				}
				if (material.HasProperty("_LeavesFilterMode") && material.HasProperty("_LeavesFilterColor"))
				{
					int num4 = material.GetInt("_LeavesFilterMode");
					Color color = material.GetColor("_LeavesFilterColor");
					if (num4 == 1 && color.r < 0.1f && color.g < 0.1f && color.b < 0.1f)
					{
						material.SetFloat("_GlobalColors", 0f);
						material.SetFloat("_MotionValue_30", 0f);
					}
				}
				if (material.HasProperty("_DetailMeshValue"))
				{
					material.SetFloat("_DetailMeshValue", 0f);
					material.SetFloat("_DetailBlendMinValue", 0.4f);
					material.SetFloat("_DetailBlendMaxValue", 0.6f);
				}
				material.SetInt("_IsVersion", 800);
			}
			if (num < 810)
			{
				if (material.HasProperty("_GlobalColors"))
				{
					float num5 = material.GetFloat("_GlobalColors");
					material.SetFloat("_GlobalColors", Mathf.Clamp01(num5 * 2f));
				}
				if (material.HasProperty("_VertexOcclusionColor"))
				{
					Color color2 = material.GetColor("_VertexOcclusionColor");
					float value3 = (color2.r + color2.g + color2.b + 0.001f) / 3f;
					color2.a = Mathf.Clamp01(value3);
					material.SetColor("_VertexOcclusionColor", color2);
				}
				material.SetInt("_IsIdentifier", 0);
				material.SetInt("_IsVersion", 810);
			}
			if (num < 830)
			{
				material.SetFloat("_OverlayProjectionValue", 0.6f);
				material.SetInt("_IsVersion", 830);
			}
			if (num < 850)
			{
				if (material.HasProperty("_DetailOpaqueMode"))
				{
					int num6 = material.GetInt("_DetailOpaqueMode");
					material.SetInt("_DetailFadeMode", 1 - num6);
				}
				if (material.HasProperty("_DetailTypeMode"))
				{
					if (material.GetInt("_DetailTypeMode") == 1)
					{
						material.SetInt("_DetailCoordMode", 2);
					}
					material.SetInt("_DetailMeshMode", material.GetInt("_DetailTypeMode"));
				}
				if (material.HasProperty("_DetailCoordMode"))
				{
					material.SetInt("_SecondUVsMode", material.GetInt("_DetailCoordMode"));
				}
				if (material.HasProperty("_EmissiveFlagMode"))
				{
					switch (material.GetInt("_EmissiveFlagMode"))
					{
					case 0:
						material.SetInt("_EmissiveFlagMode", 0);
						break;
					case 10:
						material.SetInt("_EmissiveFlagMode", 1);
						break;
					case 20:
						material.SetInt("_EmissiveFlagMode", 2);
						break;
					case 30:
						material.SetInt("_EmissiveFlagMode", 3);
						break;
					}
				}
				if (material.HasProperty("_EmissiveIntensityParams"))
				{
					float num7 = 1f;
					Vector4 vector = material.GetVector("_EmissiveIntensityParams");
					if (vector.w == 0f)
					{
						num7 = vector.y;
						material.SetInt("_EmissiveIntensityMode", 0);
					}
					else
					{
						num7 = vector.z;
						material.SetInt("_EmissiveIntensityMode", 1);
					}
					material.SetFloat("_EmissiveIntensityValue", num7);
				}
				material.SetInt("_IsVersion", 850);
			}
			if (num < 900)
			{
				material.SetFloat("_DetailMeshValue", 1f);
				material.SetFloat("_DetailMaskValue", 1f);
				material.SetInt("_IsVersion", 900);
			}
			if (num < 1000)
			{
				material.SetInt("_IsIdentifier", Random.Range(1, 100));
				if (material.HasProperty("_DetailMeshInvertMode"))
				{
					if (material.GetInt("_DetailMeshInvertMode") == 0)
					{
						material.SetFloat("_DetailMeshMinValue", 0f);
						material.SetFloat("_DetailMeshMaxValue", 1f);
					}
					else
					{
						material.SetFloat("_DetailMeshMinValue", 1f);
						material.SetFloat("_DetailMeshMaxValue", 0f);
					}
				}
				if (material.HasProperty("_DetailMaskInvertMode"))
				{
					if (material.GetInt("_DetailMaskInvertMode") == 0)
					{
						material.SetFloat("_DetailMaskMinValue", 0f);
						material.SetFloat("_DetailMaskMaxValue", 1f);
					}
					else
					{
						material.SetFloat("_DetailMaskMinValue", 1f);
						material.SetFloat("_DetailMaskMaxValue", 0f);
					}
				}
				material.SetInt("_IsVersion", 1000);
			}
			if (num < 1100)
			{
				if (material.HasProperty("_MotionValue_20") && material.GetInt("_MotionValue_20") == 0)
				{
					material.SetFloat("_MotionAmplitude_20", 0f);
					material.SetFloat("_MotionAmplitude_22", 0f);
				}
				if (material.HasProperty("_MotionValue_30") && material.GetInt("_MotionValue_30") == 0)
				{
					material.SetFloat("_MotionAmplitude_32", 0f);
				}
				material.SetInt("_IsVersion", 1100);
			}
			if (num < 1201)
			{
				if (material.HasProperty("_EmissiveColor"))
				{
					material.GetColor("_EmissiveColor");
					if (material.GetColor("_EmissiveColor").r > 0f || material.GetColor("_EmissiveColor").g > 0f || material.GetColor("_EmissiveColor").b > 0f)
					{
						material.SetInt("_EmissiveMode", 1);
					}
				}
				material.SetInt("_IsVersion", 1201);
			}
			if (num < 1230)
			{
				material.SetInt("_IsVersion", 1230);
			}
		}

		public static void SetMaterialUpgrade(Material material)
		{
			_ = material.shader.name;
			if (material.HasProperty("_IsVersion"))
			{
				material.GetInt("_IsVersion");
			}
		}

		public static void SetMaterialRuntime(Material material)
		{
			string projectPipeline = BoxoUtils.GetProjectPipeline();
			string name = material.shader.name;
			BoxoUtils.SetMaterialCoords(material, "_MainCoordMode", "_MainCoordValue", "_main_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_SecondCoordMode", "_SecondCoordValue", "_second_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_SecondMaskCoordMode", "_SecondMaskCoordValue", "_second_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_ThirdCoordMode", "_ThirdCoordValue", "_third_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_ThirdMaskCoordMode", "_ThirdMaskCoordValue", "_third_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_TerrainMaskCoordMode", "_TerrainMaskCoordValue", "_terrain_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_TintingMaskCoordMode", "_TintngMaskCoordValue", "_tinting_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_DrynessMaskCoordMode", "_DrynessMaskCoordValue", "_dryness_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_OverlayCoordMode", "_OverlayCoordValue", "_overlay_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_OverlayMaskCoordMode", "_OverlayMaskCoordValue", "_overlay_mask_coord_value");
			BoxoUtils.SetMaterialCoords(material, "_EmissiveCoordMode", "_EmissiveCoordValue", "_emissive_coord_value");
			BoxoUtils.SetMaterialOptions(material, "_ObjectPhaseMode", "_object_phase_mode");
			BoxoUtils.SetMaterialOptions(material, "_SecondMaskMode", "_second_mask_mode");
			BoxoUtils.SetMaterialOptions(material, "_SecondMeshMode", "_second_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_ThirdMeshMode", "_third_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_ThirdMaskMode", "_third_mask_mode");
			BoxoUtils.SetMaterialOptions(material, "_FourthMeshMode", "_fourth_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_FourthMaskMode", "_fourth_mask_mode");
			BoxoUtils.SetMaterialOptions(material, "_TerrainMeshMode", "_terrain_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_OcclusionColorMeshMode", "_occlusion_color_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_GradientColorMeshMode", "_gradient_color_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_TintingMeshMode", "_tinting_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_CutoutMeshMode", "_cutout_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_DrynessMeshMode", "_dryness_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_OverlayMeshMode", "_overlay_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_WetnessMeshMode", "_wetness_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_WetnessWaterMeshMode", "_wetness_water_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_WetnessRainMeshMode", "_wetness_rain_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_EmissiveMeshMode", "_emissive_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionBaseMeshMode", "_motion_base_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionSmallMeshMode", "_motion_small_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionTinyMeshMode", "_motion_tiny_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionBaseMaskMode", "_motion_base_vert_mode", "_motion_base_proc_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionSmallMaskMode", "_motion_small_vert_mode", "_motion_small_proc_mode");
			BoxoUtils.SetMaterialOptions(material, "_MotionTinyMaskMode", "_motion_tiny_vert_mode", "_motion_tiny_proc_mode");
			BoxoUtils.SetMaterialOptions(material, "_FlattenMeshMode", "_flatten_vert_mode");
			BoxoUtils.SetMaterialOptions(material, "_ConformMeshMode", "_conform_vert_mode");
			BoxoUtils.SetMaterialReciprocal(material, "_MainMultiRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MainOcclusionRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MainSmoothnessRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondMultiRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondOcclusionRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondSmoothnessRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondBaseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SecondBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdMultiRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdOcclusionRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdSmoothnessRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdBaseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ThirdBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthMultiRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthOcclusionRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthSmoothnessRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthBaseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FourthBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TerrainBaseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TerrainMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TerrainProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TerrainMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TerrainBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OcclusionColorMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_GradientColorMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_GradientColorNoiseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_GradientColorBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_VariationColorNoiseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TintingMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TintingLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TintingMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TintingBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_CutoutMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_DrynessMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_DrynessLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_DrynessMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_DrynessBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OverlayMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OverlayLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OverlayProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OverlayMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_OverlayBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SeasonLumaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_WetnessWaterMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_WetnessWaterBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_WetnessRainMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_DitherGlancingRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_RimLightRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_EmissiveMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_EmissiveMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_EmissiveBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SizeFadeBlendRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MotionBaseMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MotionSmallMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MotionTinyMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_FlattenMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ConformMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TransferMeshRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_TransferProjRemap");
			BoxoUtils.SetMaterialFloat(material, "_ImpostorAlphaClipValue", "_AI_Clip");
			BoxoUtils.SetMaterialBool(material, "_MotionSmallIntensityValue", "_motion_small_mode");
			BoxoUtils.SetMaterialVector(material, "_MotionHighlightColor", "_motion_highlight_color");
			BoxoUtils.SetMaterialKeywordInverted(material, "_ObjectModelMode", "TVE_LEGACY");
			BoxoUtils.SetMaterialKeyword(material, "_ObjectCoordMode", "TVE_COORD_ZUP");
			BoxoUtils.SetMaterialKeyword(material, "_ObjectPivotMode", new string[4] { "TVE_PIVOT_SINGLE", "TVE_PIVOT_BAKED", "TVE_PIVOT_PROC", "TVE_PIVOT_PHASE" });
			BoxoUtils.SetMaterialKeyword(material, "_RenderClip", "TVE_CLIPPING");
			BoxoUtils.SetMaterialKeyword(material, "_RenderFilter", new string[5] { "TVE_FILTER_DEFAULT", "TVE_FILTER_POINT", "TVE_FILTER_LOW", "TVE_FILTER_MEDIUM", "TVE_FILTER_HIGH" });
			BoxoUtils.SetMaterialKeyword(material, "_ImpostorMaskMode", new string[4] { "TVE_IMPOSTOR_MASK_OFF", "TVE_IMPOSTOR_MASK_DEFAULT", "TVE_IMPOSTOR_MASK_PACKED", "TVE_IMPOSTOR_MASK_SHADING" });
			BoxoUtils.SetMaterialKeyword(material, "_MainSampleMode", new string[6] { "TVE_MAIN_SAMPLE_MAIN_UV", "TVE_MAIN_SAMPLE_EXTRA_UV", "TVE_MAIN_SAMPLE_PLANAR_2D", "TVE_MAIN_SAMPLE_PLANAR_3D", "TVE_MAIN_SAMPLE_STOCHASTIC_2D", "TVE_MAIN_SAMPLE_STOCHASTIC_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_SecondIntensityValue", "TVE_SECOND");
			BoxoUtils.SetMaterialKeyword(material, "_SecondIntensityValue", "_SecondMaskValue", "TVE_SECOND_MASK");
			BoxoUtils.SetMaterialKeyword(material, "_SecondIntensityValue", "_SecondSampleMode", new string[6] { "TVE_SECOND_SAMPLE_MAIN_UV", "TVE_SECOND_SAMPLE_EXTRA_UV", "TVE_SECOND_SAMPLE_PLANAR_2D", "TVE_SECOND_SAMPLE_PLANAR_3D", "TVE_SECOND_SAMPLE_STOCHASTIC_2D", "TVE_SECOND_SAMPLE_STOCHASTIC_3D" });
			BoxoUtils.SetMaterialKeyword(material, allParentsOn: true, new string[2] { "_SecondIntensityValue", "_SecondMaskValue" }, "_SecondMaskSampleMode", new string[4] { "TVE_SECOND_MASK_SAMPLE_MAIN_UV", "TVE_SECOND_MASK_SAMPLE_EXTRA_UV", "TVE_SECOND_MASK_SAMPLE_PLANAR_2D", "TVE_SECOND_MASK_SAMPLE_PLANAR_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_SecondIntensityValue", "_SecondElementMode", "TVE_SECOND_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_ThirdIntensityValue", "TVE_THIRD");
			BoxoUtils.SetMaterialKeyword(material, "_ThirdIntensityValue", "_ThirdMaskValue", "TVE_THIRD_MASK");
			BoxoUtils.SetMaterialKeyword(material, "_ThirdIntensityValue", "_ThirdSampleMode", new string[6] { "TVE_THIRD_SAMPLE_MAIN_UV", "TVE_THIRD_SAMPLE_EXTRA_UV", "TVE_THIRD_SAMPLE_PLANAR_2D", "TVE_THIRD_SAMPLE_PLANAR_3D", "TVE_THIRD_SAMPLE_STOCHASTIC_2D", "TVE_THIRD_SAMPLE_STOCHASTIC_3D" });
			BoxoUtils.SetMaterialKeyword(material, allParentsOn: true, new string[2] { "_ThirdIntensityValue", "_ThirdMaskValue" }, "_ThirdMaskSampleMode", new string[4] { "TVE_THIRD_MASK_SAMPLE_MAIN_UV", "TVE_THIRD_MASK_SAMPLE_EXTRA_UV", "TVE_THIRD_MASK_SAMPLE_PLANAR_2D", "TVE_THIRD_MASK_SAMPLE_PLANAR_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_ThirdIntensityValue", "_ThirdElementMode", "TVE_THIRD_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_FourthIntensityValue", "TVE_FOURTH");
			BoxoUtils.SetMaterialKeyword(material, "_FourthIntensityValue", "_FourthMaskValue", "TVE_FOURTH_MASK");
			BoxoUtils.SetMaterialKeyword(material, "_FourthIntensityValue", "_FourthSampleMode", new string[6] { "TVE_FOURTH_SAMPLE_MAIN_UV", "TVE_FOURTH_SAMPLE_EXTRA_UV", "TVE_FOURTH_SAMPLE_PLANAR_2D", "TVE_FOURTH_SAMPLE_PLANAR_3D", "TVE_FOURTH_SAMPLE_STOCHASTIC_2D", "TVE_FOURTH_SAMPLE_STOCHASTIC_3D" });
			BoxoUtils.SetMaterialKeyword(material, allParentsOn: true, new string[2] { "_FourthIntensityValue", "_FourthMaskValue" }, "_FourthMaskSampleMode", new string[4] { "TVE_FOURTH_MASK_SAMPLE_MAIN_UV", "TVE_FOURTH_MASK_SAMPLE_EXTRA_UV", "TVE_FOURTH_MASK_SAMPLE_PLANAR_2D", "TVE_FOURTH_MASK_SAMPLE_PLANAR_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_FourthIntensityValue", "_FourthElementMode", "TVE_FOURTH_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainIntensityValue", "TVE_TERRAIN");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainIntensityValue", "_TerrainMaskSampleMode", new string[4] { "TVE_TERRAIN_MASK_SAMPLE_MAIN_UV", "TVE_TERRAIN_MASK_SAMPLE_EXTRA_UV", "TVE_TERRAIN_MASK_SAMPLE_PLANAR_2D", "TVE_TERRAIN_MASK_SAMPLE_PLANAR_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_OcclusionIntensityValue", "TVE_OCCLUSION");
			BoxoUtils.SetMaterialKeyword(material, "_GradientIntensityValue", "TVE_GRADIENT");
			BoxoUtils.SetMaterialKeyword(material, "_VariationIntensityValue", "TVE_VARIATION");
			BoxoUtils.SetMaterialKeyword(material, "_TintingIntensityValue", "TVE_TINTING");
			BoxoUtils.SetMaterialKeyword(material, "_TintingIntensityValue", "_TintingElementMode", "TVE_TINTING_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_CutoutIntensityValue", "TVE_CUTOUT");
			BoxoUtils.SetMaterialKeyword(material, "_CutoutIntensityValue", "_CutoutShadowMode", "TVE_CUTOUT_SHADOW");
			BoxoUtils.SetMaterialKeyword(material, "_CutoutIntensityValue", "_CutoutElementMode", "TVE_CUTOUT_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_DrynessIntensityValue", "TVE_DRYNESS");
			BoxoUtils.SetMaterialKeyword(material, "_DrynessIntensityValue", "_DrynessShiftValue", "TVE_DRYNESS_SHIFT");
			BoxoUtils.SetMaterialKeyword(material, "_DrynessIntensityValue", "_DrynessElementMode", "TVE_DRYNESS_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "TVE_OVERLAY");
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "_OverlayTextureMode", "TVE_OVERLAY_TEX");
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "_OverlaySampleMode", new string[4] { "TVE_OVERLAY_SAMPLE_PLANAR_2D", "TVE_OVERLAY_SAMPLE_PLANAR_3D", "TVE_OVERLAY_SAMPLE_STOCHASTIC_2D", "TVE_OVERLAY_SAMPLE_STOCHASTIC_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "_OverlayMaskSampleMode", new string[2] { "TVE_OVERLAY_MASK_SAMPLE_MAIN_UV", "TVE_OVERLAY_MASK_SAMPLE_EXTRA_UV" });
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "_OverlayGlitterIntensityValue", "TVE_OVERLAY_GLITTER");
			BoxoUtils.SetMaterialKeyword(material, "_OverlayIntensityValue", "_OverlayElementMode", "TVE_OVERLAY_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_WetnessIntensityValue", "TVE_WETNESS");
			BoxoUtils.SetMaterialKeyword(material, "_WetnessIntensityValue", "_WetnessWaterIntensityValue", "TVE_WETNESS_WATER");
			BoxoUtils.SetMaterialKeyword(material, allParentsOn: true, new string[2] { "_WetnessIntensityValue", "_WetnessRainIntensityValue" }, "_WetnessDropsIntensityValue", "TVE_WETNESS_DROPS");
			BoxoUtils.SetMaterialKeyword(material, allParentsOn: true, new string[2] { "_WetnessIntensityValue", "_WetnessRainIntensityValue" }, "_WetnessDripsIntensityValue", "TVE_WETNESS_DRIPS");
			BoxoUtils.SetMaterialKeyword(material, "_WetnessIntensityValue", "_WetnessElementMode", "TVE_WETNESS_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_SeasonIntensityValue", "TVE_SEAOSON");
			BoxoUtils.SetMaterialKeyword(material, "_DitherIntensityValue", "TVE_DITHER");
			BoxoUtils.SetMaterialKeyword(material, "_DitherIntensityValue", "_DitherShadowMode", "TVE_DITHER_SHADOW");
			BoxoUtils.SetMaterialKeyword(material, "_DitherIntensityValue", "_DitherNoiseMode", new string[2] { "TVE_DITHER_NOISE_BAYER", "TVE_DITHER_NOISE_OPTIMAL_3D" });
			BoxoUtils.SetMaterialKeyword(material, "_RimLightIntensityValue", "TVE_RIMLIGHT");
			BoxoUtils.SetMaterialKeyword(material, "_EmissiveIntensityValue", "TVE_EMISSIVE");
			BoxoUtils.SetMaterialKeyword(material, "_EmissiveIntensityValue", "_EmissiveSampleMode", new string[2] { "TVE_EMISSIVE_SAMPLE_MAIN_UV", "TVE_EMISSIVE_SAMPLE_EXTRA_UV" });
			BoxoUtils.SetMaterialKeyword(material, "_EmissiveIntensityValue", "_EmissiveElementMode", "TVE_EMISSIVE_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_SubsurfaceIntensityValue", "TVE_SUBSURFACE");
			BoxoUtils.SetMaterialKeyword(material, "_SubsurfaceIntensityValue", "_SubsurfaceElementMode", "TVE_SUBSURFACE_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_PerspectiveIntensityValue", "TVE_PERSPECTIVE");
			BoxoUtils.SetMaterialKeyword(material, "_SizeFadeIntensityValue", "TVE_SIZEFADE");
			BoxoUtils.SetMaterialKeyword(material, "_SizeFadeIntensityValue", "_SizeFadeElementMode", "TVE_SIZEFADE_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_MotionIntensityValue", "TVE_MOTION");
			BoxoUtils.SetMaterialKeyword(material, "_MotionIntensityValue", "_MotionElementMode", "TVE_MOTION_ELEMENT");
			BoxoUtils.SetMaterialKeyword(material, "_FlattenIntensityValue", "TVE_FLATTEN");
			BoxoUtils.SetMaterialKeyword(material, "_ReshadeIntensityValue", "TVE_RESHADE");
			BoxoUtils.SetMaterialKeyword(material, "_ConformIntensityValue", "TVE_CONFORM");
			BoxoUtils.SetMaterialKeyword(material, "_RotationIntensityValue", "TVE_ROTATION");
			BoxoUtils.SetMaterialKeyword(material, "_TransferIntensityValue", "TVE_TRANSFER");
			BoxoUtils.SetMaterialKeyword(material, "_TransferIntensityValue", "_TransferPerPixelMode", "TVE_TRANSFER_PER_PIXEL");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainTextureMode", new string[2] { "TVE_TERRAIN_TEX_DEFAULT", "TVE_TERRAIN_TEX_PACKED" });
			BoxoUtils.SetMaterialKeyword(material, "_TerrainRemapMode", "TVE_TERRAIN_REMAP");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainColorMode", "TVE_TERRAIN_COLOR");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainHeightBlendValue", "TVE_TERRAIN_BLEND");
			BoxoUtils.SetMaterialKeyword(material, "_TerrainNormalPerPixelMode", "TVE_TERRAIN_NORMAL_PER_PIXEL");
			if (material.HasProperty("_TerrainLayersMode"))
			{
				int num = material.GetInt("_TerrainLayersMode");
				if (num == 4)
				{
					material.EnableKeyword("TVE_TERRAIN_04");
					material.DisableKeyword("TVE_TERRAIN_08");
					material.DisableKeyword("TVE_TERRAIN_12");
					material.DisableKeyword("TVE_TERRAIN_16");
				}
				if (num == 8)
				{
					material.DisableKeyword("TVE_TERRAIN_04");
					material.EnableKeyword("TVE_TERRAIN_08");
					material.DisableKeyword("TVE_TERRAIN_12");
					material.DisableKeyword("TVE_TERRAIN_16");
				}
				if (num == 12)
				{
					material.DisableKeyword("TVE_TERRAIN_04");
					material.DisableKeyword("TVE_TERRAIN_08");
					material.EnableKeyword("TVE_TERRAIN_12");
					material.DisableKeyword("TVE_TERRAIN_16");
				}
				if (num == 16)
				{
					material.DisableKeyword("TVE_TERRAIN_04");
					material.DisableKeyword("TVE_TERRAIN_08");
					material.DisableKeyword("TVE_TERRAIN_12");
					material.EnableKeyword("TVE_TERRAIN_16");
				}
			}
			if (material.HasProperty("_TerrainSampleMode1"))
			{
				for (int i = 1; i < 17; i++)
				{
					string name2 = "_TerrainSampleMode" + i;
					if (material.HasProperty(name2))
					{
						string text = i.ToString("00");
						int num2 = material.GetInt(name2);
						if (num2 == 0)
						{
							material.EnableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_3D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_3D");
						}
						if (num2 == 1)
						{
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_2D");
							material.EnableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_3D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_3D");
						}
						if (num2 == 2)
						{
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_3D");
							material.EnableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_3D");
						}
						if (num2 == 3)
						{
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_2D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_PLANAR_3D");
							material.DisableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_2D");
							material.EnableKeyword("TVE_TERRAIN_SAMPLE_" + text + "_STOCHASTIC_3D");
						}
					}
				}
			}
			if (material.HasProperty("_EmissiveIntensityValue"))
			{
				if (material.HasProperty("_EmissivePowerMode") && material.HasProperty("_EmissivePowerValue"))
				{
					float num3 = material.GetInt("_EmissivePowerMode");
					float num4 = material.GetFloat("_EmissivePowerValue");
					if (num3 == 0f)
					{
						material.SetFloat("_emissive_power_value", num4);
					}
					else if (num3 == 1f)
					{
						material.SetFloat("_emissive_power_value", 0.125f * Mathf.Pow(2f, num4));
					}
				}
				if (material.HasProperty("_EmissiveFlagMode"))
				{
					switch (material.GetInt("_EmissiveFlagMode"))
					{
					case 0:
						material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
						break;
					case 1:
						material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
						break;
					case 2:
						material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
						break;
					case 3:
						material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
						break;
					}
				}
			}
			if (material.HasProperty("_SubsurfaceIntensityValue"))
			{
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceScatteringValue", "_Translucency");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceNormalValue", "_TransNormalDistortion");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceScatteringValue", "_TransStrength");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceNormalValue", "_TransNormal");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceAngleValue", "_TransScattering");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceDirectValue", "_TransDirect");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceAmbientValue", "_TransAmbient");
				BoxoUtils.SetMaterialFloat(material, "_SubsurfaceShadowValue", "_TransShadow");
			}
			if (material.HasProperty("_MetaEnabledMode") && BoxoUtils.GetMaterialInt(material, "_MetaEnabledMode") == 1)
			{
				if (BoxoUtils.GetMaterialInt(material, "_MetaSurfaceMode") == 0)
				{
					BoxoUtils.SetMaterialTexture(material, "_MainAlbedoTex", "_MetaAlbedoTex");
					BoxoUtils.SetMaterialVector(material, "_MainColor", "_MetaAlbedoColor");
					BoxoUtils.SetMaterialFloat(material, "_MainAlbedoValue", "_MetaAlbedoValue");
					BoxoUtils.SetMaterialVector(material, "_main_coord_value", "_MetaCoordValue");
					if (BoxoUtils.GetMaterialFloat(material, "_SecondIntensityValue") > 0f)
					{
						BoxoUtils.SetMaterialTexture(material, "_SecondAlbedoTex", "_MetaAlbedoex");
						BoxoUtils.SetMaterialVector(material, "_SecondColor", "_MetaAlbedoColor");
						BoxoUtils.SetMaterialFloat(material, "_SecondAlbedoValue", "_MetaAlbedoValue");
						BoxoUtils.SetMaterialVector(material, "_second_coord_value", "_MetaCoordValue");
					}
					if (BoxoUtils.GetMaterialFloat(material, "_ThirdIntensityValue") > 0f)
					{
						BoxoUtils.SetMaterialTexture(material, "_ThirdAlbedoTex", "_MetaAlbedoTex");
						BoxoUtils.SetMaterialVector(material, "_ThirdColor", "_MetaAlbedoColor");
						BoxoUtils.SetMaterialFloat(material, "_ThirdAlbedoValue", "_MetaAlbedoalue");
						BoxoUtils.SetMaterialVector(material, "_third_coord_value", "_MetaCoordValue");
					}
					if (BoxoUtils.GetMaterialFloat(material, "_FourthIntensityValue") > 0f)
					{
						BoxoUtils.SetMaterialTexture(material, "_FourthAlbedoTex", "_MetaAlbedoTex");
						BoxoUtils.SetMaterialVector(material, "_FourthColor", "_MetaAlbedoColor");
						BoxoUtils.SetMaterialFloat(material, "_FourthAlbedoValue", "_MetaAlbedoalue");
						BoxoUtils.SetMaterialVector(material, "_fourth_coord_value", "_MetaCoordValue");
					}
				}
				if (BoxoUtils.GetMaterialInt(material, "_MetaSampleMode") == 1)
				{
					material.SetVector("_MetaCoordValue", new Vector4(1f, 1f, 0f, 0f));
				}
			}
			int materialInt = BoxoUtils.GetMaterialInt(material, "_RenderMode");
			float num5 = BoxoUtils.GetMaterialFloat(material, "_RenderClip") + BoxoUtils.GetMaterialFloat(material, "_CutoutIntensityValue") + BoxoUtils.GetMaterialFloat(material, "_DitherIntensityValue");
			int materialInt2 = BoxoUtils.GetMaterialInt(material, "_RenderDecals");
			int materialInt3 = BoxoUtils.GetMaterialInt(material, "_RenderSSR");
			int num6 = 0;
			int num7 = 0;
			if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
			{
				num6 = material.GetInt("_RenderQueue");
				num7 = material.GetInt("_RenderPriority");
			}
			if (num6 == 2)
			{
				if (material.renderQueue == 2000)
				{
					material.SetOverrideTag("RenderType", "Opaque");
				}
				if (material.renderQueue > 2449 && material.renderQueue < 3000)
				{
					material.SetOverrideTag("RenderType", "AlphaTest");
				}
				if (material.renderQueue > 2999)
				{
					material.SetOverrideTag("RenderType", "Transparent");
				}
			}
			if (materialInt == 0)
			{
				if (num6 != 2)
				{
					material.SetOverrideTag("RenderType", "AlphaTest");
					if (num5 == 0f)
					{
						if (materialInt2 == 0)
						{
							material.renderQueue = 2000 + num7;
						}
						else
						{
							material.renderQueue = 2225 + num7;
						}
					}
					else if (materialInt2 == 0)
					{
						material.renderQueue = 2450 + num7;
					}
					else
					{
						material.renderQueue = 2475 + num7;
					}
				}
				material.SetInt("_render_src", 1);
				material.SetInt("_render_dst", 0);
				material.SetInt("_render_zw", 1);
				if (material.HasProperty("_MainColor"))
				{
					Color color = material.GetColor("_MainColor");
					material.SetColor("_MainColor", new Color(color.r, color.g, color.b, 1f));
				}
				if (material.HasProperty("_MainColorTwo"))
				{
					Color color2 = material.GetColor("_MainColorTwo");
					material.SetColor("_MainColorTwo", new Color(color2.r, color2.g, color2.b, 1f));
				}
				material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
				material.DisableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
				material.DisableKeyword("_BLENDMODE_ALPHA");
				material.DisableKeyword("_BLENDMODE_ADD");
				material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");
				material.SetInt("_RenderQueueType", 1);
				material.SetInt("_SurfaceType", 0);
				material.SetInt("_BlendMode", 0);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_AlphaSrcBlend", 1);
				material.SetInt("_AlphaDstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.SetInt("_TransparentZWrite", 1);
				material.SetInt("_ZTestDepthEqualForOpaque", 3);
				if (num5 == 0f)
				{
					material.SetInt("_ZTestGBuffer", 4);
				}
				else
				{
					material.SetInt("_ZTestGBuffer", 3);
				}
				material.SetInt("_ZTestTransparent", 4);
				material.SetShaderPassEnabled("TransparentBackface", enabled: false);
				material.SetShaderPassEnabled("TransparentBackfaceDebugDisplay", enabled: false);
				material.SetShaderPassEnabled("TransparentDepthPrepass", enabled: false);
				material.SetShaderPassEnabled("TransparentDepthPostpass", enabled: false);
			}
			else
			{
				if (num6 != 2)
				{
					material.SetOverrideTag("RenderType", "Transparent");
					material.renderQueue = 3000 + num7;
				}
				int value = 1;
				if (material.HasProperty("_RenderZWrite"))
				{
					value = material.GetInt("_RenderZWrite");
				}
				material.SetInt("_render_src", 5);
				material.SetInt("_render_dst", 10);
				material.SetInt("_render_zw", value);
				material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
				material.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
				material.EnableKeyword("_BLENDMODE_ALPHA");
				material.DisableKeyword("_BLENDMODE_ADD");
				material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");
				material.SetInt("_RenderQueueType", 5);
				material.SetInt("_SurfaceType", 1);
				material.SetInt("_BlendMode", 0);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_AlphaSrcBlend", 1);
				material.SetInt("_AlphaDstBlend", 10);
				material.SetInt("_ZWrite", value);
				material.SetInt("_TransparentZWrite", value);
				material.SetInt("_ZTestDepthEqualForOpaque", 4);
				material.SetInt("_ZTestGBuffer", 4);
				material.SetInt("_ZTestTransparent", 4);
				material.SetShaderPassEnabled("TransparentBackface", enabled: true);
				material.SetShaderPassEnabled("TransparentBackfaceDebugDisplay", enabled: true);
				material.SetShaderPassEnabled("TransparentDepthPrepass", enabled: true);
				material.SetShaderPassEnabled("TransparentDepthPostpass", enabled: true);
			}
			if (projectPipeline == "High Definition")
			{
				BoxoUtils.SetMaterialKeywordInverted(material, "_RenderDecals", "_DISABLE_DECALS");
				if (material.HasProperty("_RenderSSR"))
				{
					if (materialInt3 == 0)
					{
						material.EnableKeyword("_DISABLE_SSR");
						material.SetInt("_StencilRef", 0);
						material.SetInt("_StencilRefDepth", 0);
						material.SetInt("_StencilRefDistortionVec", 4);
						material.SetInt("_StencilRefGBuffer", 2);
						material.SetInt("_StencilRefMV", 32);
						material.SetInt("_StencilWriteMask", 6);
						material.SetInt("_StencilWriteMaskDepth", 8);
						material.SetInt("_StencilWriteMaskDistortionVec", 4);
						material.SetInt("_StencilWriteMaskGBuffer", 14);
						material.SetInt("_StencilWriteMaskMV", 40);
					}
					else
					{
						material.DisableKeyword("_DISABLE_SSR");
						material.SetInt("_StencilRef", 0);
						material.SetInt("_StencilRefDepth", 8);
						material.SetInt("_StencilRefDistortionVec", 4);
						material.SetInt("_StencilRefGBuffer", 10);
						material.SetInt("_StencilRefMV", 40);
						material.SetInt("_StencilWriteMask", 6);
						material.SetInt("_StencilWriteMaskDepth", 8);
						material.SetInt("_StencilWriteMaskDistortionVec", 4);
						material.SetInt("_StencilWriteMaskGBuffer", 14);
						material.SetInt("_StencilWriteMaskMV", 40);
					}
				}
			}
			if (material.HasProperty("_RenderCull"))
			{
				int value2 = material.GetInt("_RenderCull");
				material.SetInt("_render_cull", value2);
				material.SetInt("_CullMode", value2);
				material.SetInt("_TransparentCullMode", value2);
				material.SetInt("_CullModeForward", value2);
				material.DisableKeyword("_DOUBLESIDED_ON");
			}
			if (material.HasProperty("_RenderNormal"))
			{
				switch (material.GetInt("_RenderNormal"))
				{
				case 0:
					material.SetVector("_render_normal", new Vector4(-1f, -1f, -1f, 0f));
					material.SetVector("_DoubleSidedConstants", new Vector4(-1f, -1f, -1f, 0f));
					break;
				case 1:
					material.SetVector("_render_normal", new Vector4(1f, 1f, -1f, 0f));
					material.SetVector("_DoubleSidedConstants", new Vector4(1f, 1f, -1f, 0f));
					break;
				case 2:
					material.SetVector("_render_normal", new Vector4(1f, 1f, 1f, 0f));
					material.SetVector("_DoubleSidedConstants", new Vector4(1f, 1f, 1f, 0f));
					break;
				}
			}
			BoxoUtils.SetMaterialKeywordInverted(material, "_RenderShadow", "_RECEIVE_SHADOWS_OFF");
			if (projectPipeline == "Universal")
			{
				if (name.Contains("Subsurface Lit"))
				{
					if (material.HasProperty("_RenderGBuffer"))
					{
						if (material.GetInt("_RenderGBuffer") == 0)
						{
							material.SetShaderPassEnabled("UniversalGBuffer", enabled: false);
						}
						else
						{
							material.SetShaderPassEnabled("UniversalGBuffer", enabled: true);
						}
					}
				}
				else
				{
					material.SetShaderPassEnabled("UniversalGBuffer", enabled: true);
				}
			}
			if (material.HasProperty("_RenderCoverage"))
			{
				if (num5 == 0f)
				{
					material.SetInt("_render_coverage", 0);
				}
				else
				{
					BoxoUtils.SetMaterialFloat(material, "_RenderCoverage", "_render_coverage");
				}
			}
			else
			{
				material.SetInt("_render_coverage", 0);
			}
			float materialFloat = BoxoUtils.GetMaterialFloat(material, "_MotionIntensityValue");
			if (material.HasProperty("_RenderMotion"))
			{
				float num8 = material.GetFloat("_RenderMotion");
				if (num8 == 0f)
				{
					if (materialFloat > 0f)
					{
						material.SetShaderPassEnabled("MotionVectors", enabled: true);
					}
					else
					{
						material.SetShaderPassEnabled("MotionVectors", enabled: false);
					}
				}
				else if (num8 == 1f)
				{
					material.SetShaderPassEnabled("MotionVectors", enabled: false);
				}
				else if (num8 == 2f)
				{
					material.SetShaderPassEnabled("MotionVectors", enabled: false);
				}
			}
			else
			{
				material.SetShaderPassEnabled("MotionVectors", enabled: false);
			}
		}

		public static void SetMaterialInternal(Material material)
		{
			_ = material.shader.name;
			if (material.HasTexture("_NoiseTex3D") && material.GetTexture("_NoiseTex3D") == null)
			{
				material.SetTexture("_NoiseTex3D", Resources.Load<Texture3D>("Internal NoiseTex3D"));
			}
			if (material.HasTexture("_NoiseTexSS") && material.GetTexture("_NoiseTexSS") == null)
			{
				material.SetTexture("_NoiseTexSS", Resources.Load<Texture2D>("Internal NoiseTexSS"));
			}
			if (material.HasTexture("_OverlayNormalTex") && material.GetTexture("_OverlayNormalTex") == null)
			{
				material.SetTexture("_OverlayNormalTex", Resources.Load<Texture2D>("Internal SnowTex"));
			}
			if (material.HasTexture("_OverlayGlitterTexRT") && material.GetTexture("_OverlayGlitterTexRT") == null)
			{
				material.SetTexture("_OverlayGlitterTexRT", Resources.Load<CustomRenderTexture>("Internal GlitterTexRT"));
			}
			if (material.HasTexture("_WetnessDropsTexRT") && material.GetTexture("_WetnessDropsTexRT") == null)
			{
				material.SetTexture("_WetnessDropsTexRT", Resources.Load<CustomRenderTexture>("Internal DropsTexRT"));
			}
			if (material.HasTexture("_WetnessDripsTexRT") && material.GetTexture("_WetnessDripsTexRT") == null)
			{
				material.SetTexture("_WetnessDripsTexRT", Resources.Load<CustomRenderTexture>("Internal DripsTexRT"));
			}
			if (material.HasTexture("_MotionNoiseTex") && material.GetTexture("_MotionNoiseTex") == null)
			{
				material.SetTexture("_MotionNoiseTex", Resources.Load<Texture2D>("Internal MotionTex"));
			}
			if (material.HasProperty("_MainAlphaClipValue"))
			{
				material.SetFloat("_Cutoff", material.GetFloat("_MainAlphaClipValue"));
			}
			if (material.HasProperty("_MainColor"))
			{
				material.SetColor("_Color", material.GetColor("_MainColor"));
			}
			if (material.HasProperty("_SpecColor"))
			{
				material.SetColor("_SpecColor", Color.white);
			}
			if (material.HasTexture("_MainAlbedoTex") && material.HasTexture("_MainTex"))
			{
				material.SetTexture("_MainTex", material.GetTexture("_MainAlbedoTex"));
			}
			if (material.HasTexture("_MainNormalTex") && material.HasTexture("_BumpMap"))
			{
				material.SetTexture("_BumpMap", material.GetTexture("_MainNormalTex"));
			}
			if (material.HasProperty("_MainCoordValue"))
			{
				if (material.HasTexture("_MainTex"))
				{
					material.SetTextureScale("_MainTex", new Vector2(material.GetVector("_MainCoordValue").x, material.GetVector("_MainCoordValue").y));
					material.SetTextureOffset("_MainTex", new Vector2(material.GetVector("_MainCoordValue").z, material.GetVector("_MainCoordValue").w));
				}
				if (material.HasTexture("_BumpMap"))
				{
					material.SetTextureScale("_BumpMap", new Vector2(material.GetVector("_MainCoordValue").x, material.GetVector("_MainCoordValue").y));
					material.SetTextureOffset("_BumpMap", new Vector2(material.GetVector("_MainCoordValue").z, material.GetVector("_MainCoordValue").w));
				}
			}
		}

		public static void SetImpostorSettings(Material oldMaterial, Material material)
		{
			material.SetFloat("_IsInitialized", 1f);
			SetMaterialRuntime(material);
			SetMaterialInternal(material);
		}

		public static void SetElementSettings(Material material)
		{
			if (!material.HasProperty("_IsElementShader"))
			{
				return;
			}
			string name = material.shader.name;
			material.SetShaderPassEnabled("VolumePass", enabled: false);
			if (material.HasProperty("_IsVersion"))
			{
				int num = material.GetInt("_IsVersion");
				if (num < 600)
				{
					if (material.HasProperty("_ElementLayerValue"))
					{
						int num2 = material.GetInt("_ElementLayerValue");
						if (material.GetInt("_ElementLayerValue") > 0)
						{
							material.SetInt("_ElementLayerMask", (int)Mathf.Pow(2f, num2));
							material.SetInt("_ElementLayerValue", -1);
						}
					}
					if (material.HasProperty("_InvertX"))
					{
						material.SetInt("_ElementInvertMode", material.GetInt("_InvertX"));
					}
					if (material.HasProperty("_ElementFadeSupport"))
					{
						material.SetInt("_ElementVolumeFadeMode", material.GetInt("_ElementFadeSupport"));
					}
					material.SetInt("_IsVersion", 600);
				}
				if (num < 700)
				{
					material.SetInt("_IsVersion", 700);
				}
				if (num < 800)
				{
					if (material.shader.name.Contains("Interaction") && material.HasProperty("_ElementDirectionMode") && material.GetInt("_ElementDirectionMode") == 1)
					{
						material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Elements/Default/Motion Advanced");
						material.SetInt("_ElementDirectionMode", 30);
					}
					if (name.Contains("Orientation"))
					{
						material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Elements/Default/Motion Interaction");
					}
					if (name.Contains("Turbulence"))
					{
						material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Elements/Default/Motion Advanced");
						material.SetInt("_ElementDirectionMode", 10);
					}
					if (name.Contains("Wind Control"))
					{
						material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Elements/Default/Wind Power");
					}
					if (name.Contains("Wind Direction"))
					{
						material.shader = Shader.Find("BOXOPHOBIC/The Visual Engine/Elements/Default/Motion Advanced");
						material.SetInt("_ElementDirectionMode", 10);
					}
					material.SetInt("_IsVersion", 800);
				}
				if (num < 810)
				{
					if (material.HasProperty("_MainTexMinValue") && material.HasProperty("_MainTexMaxValue"))
					{
						float value = material.GetFloat("_MainTexMinValue");
						float value2 = material.GetFloat("_MainTexMaxValue");
						material.SetFloat("_MainMaskAlphaMinValue", value);
						material.SetFloat("_MainMaskAlphaMaxValue", value2);
					}
					material.SetInt("_IsVersion", 810);
				}
			}
			string val = Path.GetFileName(name).Split(" ")[0];
			material.SetOverrideTag("ElementType", val);
			if (material.HasProperty("_ElementColorsMode"))
			{
				int value3 = material.GetInt("_ElementColorsMode");
				material.SetInt("_render_colormask", value3);
			}
			if (material.HasProperty("_ElementBlendRGB"))
			{
				switch (material.GetInt("_ElementBlendRGB"))
				{
				case 0:
					material.SetInt("_render_src", 5);
					material.SetInt("_render_dst", 10);
					break;
				case 1:
					material.SetInt("_render_src", 2);
					material.SetInt("_render_dst", 0);
					break;
				case 2:
					material.SetInt("_render_src", 1);
					material.SetInt("_render_dst", 1);
					break;
				}
				BoxoUtils.SetMaterialOptions(material, "_ElementBlendRGB", "_element_blend_rgb");
			}
			if (material.HasProperty("_ElementBlendA"))
			{
				if (material.GetInt("_ElementBlendA") == 0)
				{
					material.SetInt("_render_src", 7);
					material.SetInt("_render_dst", 0);
				}
				else
				{
					material.SetInt("_render_src", 1);
					material.SetInt("_render_dst", 1);
				}
			}
			BoxoUtils.SetMaterialReciprocal(material, "_MainTexColorRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MainTexAlphaRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_MainTexFalloffRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_SeasonRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_NoiseRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ElementMaskRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ElementProjRemap");
			BoxoUtils.SetMaterialReciprocal(material, "_ControlMaskRemap");
			BoxoUtils.SetMaterialOptions(material, "_MotionDirectionMode", "_motion_direction_mode");
		}

		public static GameObject CreateElement(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Transform parent, Material material, bool customMaterial)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("Internal Element"));
			gameObject.name = "Element " + Path.GetFileNameWithoutExtension(material.shader.name);
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localRotation = localRotation;
			gameObject.transform.localScale = localScale;
			gameObject.AddComponent<TVEElement>();
			if (customMaterial)
			{
				gameObject.GetComponent<TVEElement>().customMaterial = material;
			}
			else
			{
				gameObject.GetComponent<Renderer>().sharedMaterial = material;
			}
			gameObject.transform.parent = parent;
			return gameObject;
		}

		public static GameObject CreateElement(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Transform parent, Material material)
		{
			return CreateElement(localPosition, localRotation, localScale, parent, material, customMaterial: false);
		}

		public static GameObject CreateElement(Terrain terrain, Material material, bool customMaterial)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("Internal Element"));
			gameObject.name = "Element " + terrain.name;
			CopyTerrainDataToElement(terrain, TVETerrainTexture.HeightTexture, material);
			gameObject.AddComponent<TVEElement>();
			if (customMaterial)
			{
				gameObject.GetComponent<TVEElement>().customMaterial = material;
			}
			else
			{
				gameObject.GetComponent<Renderer>().sharedMaterial = material;
			}
			Vector3 position = terrain.transform.position;
			Bounds bounds = terrain.terrainData.bounds;
			gameObject.transform.localPosition = new Vector3(bounds.center.x + position.x, bounds.min.y + position.y, bounds.center.z + position.z);
			gameObject.transform.localScale = new Vector3(terrain.terrainData.size.x, 1f, terrain.terrainData.size.z);
			gameObject.GetComponent<TVEElement>().terrainData = terrain;
			return gameObject;
		}

		public static GameObject CreateElement(Terrain terrain, Material material)
		{
			return CreateElement(terrain, material, customMaterial: false);
		}

		public static GameObject CreateElement(GameObject gameObject, Material material, bool customMaterial)
		{
			gameObject.AddComponent<TVEElement>();
			if (customMaterial)
			{
				gameObject.GetComponent<TVEElement>().customMaterial = material;
			}
			else
			{
				gameObject.GetComponent<Renderer>().sharedMaterial = material;
			}
			return gameObject;
		}

		public static GameObject CreateElement(GameObject gameObject, Material material)
		{
			return CreateElement(gameObject, material, customMaterial: false);
		}

		public static void CopyTerrainDataToElement(Terrain terrain, TVETerrainTexture terrainMask, Material material)
		{
			if (terrain == null || terrain.terrainData == null || terrainMask == TVETerrainTexture.None)
			{
				material.SetTexture("_TerrainHeightTex", null);
				material.SetTexture("_TerrainNormalTex", null);
				material.SetTexture("_TerrainHolesTex", null);
				material.SetInt("_TerrainInputMode", 0);
				return;
			}
			material.SetInt("_TerrainInputMode", 1);
			if (terrain.terrainData.heightmapTexture != null)
			{
				material.SetTexture("_TerrainHeightTex", terrain.terrainData.heightmapTexture);
				if (material.HasProperty("_MainTex") && terrainMask == TVETerrainTexture.HeightTexture)
				{
					material.SetTexture("_MainTex", terrain.terrainData.heightmapTexture);
				}
			}
			if (terrain.normalmapTexture != null)
			{
				material.SetTexture("_TerrainNormalTex", terrain.normalmapTexture);
				if (material.HasProperty("_MainTex") && terrainMask == TVETerrainTexture.NormalTexture)
				{
					material.SetTexture("_MainTex", terrain.normalmapTexture);
				}
			}
			if (terrain.terrainData.holesTexture != null)
			{
				material.SetTexture("_TerrainHolesTex", terrain.terrainData.holesTexture);
				if (material.HasProperty("_MainTex") && terrainMask == TVETerrainTexture.HolesTexture)
				{
					material.SetTexture("_MainTex", terrain.terrainData.holesTexture);
				}
			}
			material.SetVector("_TerrainPosition", terrain.transform.position);
			material.SetVector("_TerrainSize", terrain.terrainData.size);
			if (terrain.terrainData.alphamapTextureCount == 1)
			{
				material.SetTexture("_ControlTex1", terrain.terrainData.alphamapTextures[0]);
			}
			if (terrain.terrainData.alphamapTextureCount == 2)
			{
				material.SetTexture("_ControlTex2", terrain.terrainData.alphamapTextures[1]);
			}
			if (terrain.terrainData.alphamapTextureCount == 3)
			{
				material.SetTexture("_ControlTex3", terrain.terrainData.alphamapTextures[2]);
			}
			if (terrain.terrainData.alphamapTextureCount == 4)
			{
				material.SetTexture("_ControlTex4", terrain.terrainData.alphamapTextures[3]);
			}
		}

		public static void CopyTerrainDataToMaterial(Terrain terrain, Material material)
		{
			if (terrain == null || terrain.terrainData == null || material == null)
			{
				return;
			}
			material.SetVector("_TerrainPosition", terrain.transform.position);
			material.SetVector("_TerrainSize", terrain.terrainData.size);
			if (terrain.terrainData.holesTexture != null)
			{
				material.SetTexture("_TerrainHolesTex", terrain.terrainData.holesTexture);
			}
			for (int i = 0; i < terrain.terrainData.alphamapTextures.Length; i++)
			{
				Texture2D texture2D = terrain.terrainData.alphamapTextures[i];
				int num = i + 1;
				if (texture2D != null)
				{
					material.SetTexture("_TerrainControlTex" + num, texture2D);
				}
			}
			for (int j = 0; j < terrain.terrainData.terrainLayers.Length; j++)
			{
				TerrainLayer terrainLayer = terrain.terrainData.terrainLayers[j];
				int num2 = j + 1;
				if (!(terrainLayer == null))
				{
					if (terrainLayer.diffuseTexture != null)
					{
						material.SetTexture("_TerrainAlbedoTex" + num2, terrainLayer.diffuseTexture);
					}
					else
					{
						material.SetTexture("_TerrainAlbedoTex" + num2, Texture2D.whiteTexture);
					}
					if (terrainLayer.normalMapTexture != null)
					{
						material.SetTexture("_TerrainNormalTex" + num2, terrainLayer.normalMapTexture);
					}
					else
					{
						material.SetTexture("_TerrainNormalTex" + num2, Texture2D.normalTexture);
					}
					if (terrainLayer.maskMapTexture != null)
					{
						material.SetTexture("_TerrainShaderTex" + num2, terrainLayer.maskMapTexture);
					}
					else
					{
						material.SetTexture("_TerrainShaderTex" + num2, Texture2D.whiteTexture);
					}
					float x = 1f / (terrainLayer.maskMapRemapMax.x - terrainLayer.maskMapRemapMin.x);
					float y = 1f / (terrainLayer.maskMapRemapMax.y - terrainLayer.maskMapRemapMin.y);
					float z = 1f / (terrainLayer.maskMapRemapMax.z - terrainLayer.maskMapRemapMin.z);
					float w = 1f / (terrainLayer.maskMapRemapMax.w - terrainLayer.maskMapRemapMin.w);
					material.SetVector("_TerrainShaderMin" + num2, terrainLayer.maskMapRemapMin);
					material.SetVector("_TerrainShaderRcp" + num2, new Vector4(x, y, z, w));
					material.SetVector("_TerrainParams" + num2, new Vector4(terrainLayer.metallic, 0f, terrainLayer.normalScale, terrainLayer.smoothness));
					material.SetVector("_TerrainSpecular" + num2, terrainLayer.specular);
					material.SetVector("_TerrainCoord" + num2, new Vector4(1f / terrainLayer.tileSize.x, 1f / terrainLayer.tileSize.y, terrainLayer.tileOffset.x, terrainLayer.tileOffset.y));
				}
			}
		}

		public static void CopyTerrainDataToMaterial(TVETerrain tveTerrain, Material material)
		{
			if (tveTerrain == null || tveTerrain.terrain == null || tveTerrain.terrainPropertyBlock == null || material == null)
			{
				return;
			}
			Material terrainMaterial = tveTerrain.terrainMaterial;
			material.SetVector("_TerrainPosition", tveTerrain.terrainPropertyBlock.GetVector("_TerrainPosition"));
			material.SetVector("_TerrainSize", tveTerrain.terrainPropertyBlock.GetVector("_TerrainSize"));
			material.SetColor("_TerrainColor", terrainMaterial.GetColor("_TerrainColor"));
			material.SetFloat("_TerrainNormalValue", terrainMaterial.GetFloat("_TerrainNormalValue"));
			material.SetFloat("_TerrainMetallicValue", terrainMaterial.GetFloat("_TerrainMetallicValue"));
			material.SetFloat("_TerrainOcclusionValue", terrainMaterial.GetFloat("_TerrainOcclusionValue"));
			material.SetFloat("_TerrainSmoothnessValue", terrainMaterial.GetFloat("_TerrainSmoothnessValue"));
			material.SetFloat("_TerrainHeightBlendValue", terrainMaterial.GetFloat("_TerrainHeightBlendValue"));
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainHolesTex"))
			{
				Texture texture = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainHolesTex");
				if (texture != null)
				{
					material.SetTexture("_TerrainHolesTex", texture);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex1"))
			{
				Texture texture2 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex1");
				if (texture2 != null)
				{
					material.SetTexture("_TerrainControlTex1", texture2);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex2"))
			{
				Texture texture3 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex2");
				if (texture3 != null)
				{
					material.SetTexture("_TerrainControlTex2", texture3);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex3"))
			{
				Texture texture4 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex3");
				if (texture4 != null)
				{
					material.SetTexture("_TerrainControlTex3", texture4);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex4"))
			{
				Texture texture5 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex4");
				if (texture5 != null)
				{
					material.SetTexture("_TerrainControlTex4", texture5);
				}
			}
			for (int i = 0; i < tveTerrain.terrain.terrainData.terrainLayers.Length; i++)
			{
				int num = i + 1;
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainColor" + num))
				{
					material.SetVector("_TerrainColor" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainColor" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainAlbedoTex" + num))
				{
					material.SetTexture("_TerrainAlbedoTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainAlbedoTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainNormalTex" + num))
				{
					material.SetTexture("_TerrainNormalTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainNormalTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderTex" + num))
				{
					material.SetTexture("_TerrainShaderTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainShaderTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderMin" + num))
				{
					material.SetVector("_TerrainShaderMin" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainShaderMin" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderRcp" + num))
				{
					material.SetVector("_TerrainShaderRcp" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainShaderRcp" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainParams" + num))
				{
					material.SetVector("_TerrainParams" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainParams" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainSpecular" + num))
				{
					material.SetVector("_TerrainSpecular" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainSpecular" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainCoord" + num))
				{
					material.SetVector("_TerrainCoord" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainCoord" + num));
				}
			}
		}

		public static void CopyTerrainDataToRenderer(TVETerrain tveTerrain, Renderer renderer)
		{
			if (tveTerrain == null || tveTerrain.terrain == null || tveTerrain.terrainPropertyBlock == null || renderer == null)
			{
				return;
			}
			Material terrainMaterial = tveTerrain.terrainMaterial;
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetVector("_TerrainPosition", tveTerrain.terrainPropertyBlock.GetVector("_TerrainPosition"));
			materialPropertyBlock.SetVector("_TerrainSize", tveTerrain.terrainPropertyBlock.GetVector("_TerrainSize"));
			materialPropertyBlock.SetColor("_TerrainColor", terrainMaterial.GetColor("_TerrainColor"));
			materialPropertyBlock.SetFloat("_TerrainNormalValue", terrainMaterial.GetFloat("_TerrainNormalValue"));
			materialPropertyBlock.SetFloat("_TerrainMetallicValue", terrainMaterial.GetFloat("_TerrainMetallicValue"));
			materialPropertyBlock.SetFloat("_TerrainOcclusionValue", terrainMaterial.GetFloat("_TerrainOcclusionValue"));
			materialPropertyBlock.SetFloat("_TerrainSmoothnessValue", terrainMaterial.GetFloat("_TerrainSmoothnessValue"));
			materialPropertyBlock.SetFloat("_TerrainHeightBlendValue", terrainMaterial.GetFloat("_TerrainHeightBlendValue"));
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainHolesTex"))
			{
				Texture texture = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainHolesTex");
				if (texture != null)
				{
					materialPropertyBlock.SetTexture("_TerrainHolesTex", texture);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex1"))
			{
				Texture texture2 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex1");
				if (texture2 != null)
				{
					materialPropertyBlock.SetTexture("_TerrainControlTex1", texture2);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex2"))
			{
				Texture texture3 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex2");
				if (texture3 != null)
				{
					materialPropertyBlock.SetTexture("_TerrainControlTex2", texture3);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex3"))
			{
				Texture texture4 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex3");
				if (texture4 != null)
				{
					materialPropertyBlock.SetTexture("_TerrainControlTex3", texture4);
				}
			}
			if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainControlTex4"))
			{
				Texture texture5 = tveTerrain.terrainPropertyBlock.GetTexture("_TerrainControlTex4");
				if (texture5 != null)
				{
					materialPropertyBlock.SetTexture("_TerrainControlTex4", texture5);
				}
			}
			for (int i = 0; i < tveTerrain.terrain.terrainData.terrainLayers.Length; i++)
			{
				int num = i + 1;
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainColor" + num))
				{
					materialPropertyBlock.SetVector("_TerrainColor" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainColor" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainAlbedoTex" + num))
				{
					materialPropertyBlock.SetTexture("_TerrainAlbedoTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainAlbedoTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainNormalTex" + num))
				{
					materialPropertyBlock.SetTexture("_TerrainNormalTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainNormalTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderTex" + num))
				{
					materialPropertyBlock.SetTexture("_TerrainShaderTex" + num, tveTerrain.terrainPropertyBlock.GetTexture("_TerrainShaderTex" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderMin" + num))
				{
					materialPropertyBlock.SetVector("_TerrainShaderMin" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainShaderMin" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainShaderRcp" + num))
				{
					materialPropertyBlock.SetVector("_TerrainShaderRcp" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainShaderRcp" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainParams" + num))
				{
					materialPropertyBlock.SetVector("_TerrainParams" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainParams" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainSpecular" + num))
				{
					materialPropertyBlock.SetVector("_TerrainSpecular" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainSpecular" + num));
				}
				if (tveTerrain.terrainPropertyBlock.HasProperty("_TerrainCoord" + num))
				{
					materialPropertyBlock.SetVector("_TerrainCoord" + num, tveTerrain.terrainPropertyBlock.GetVector("_TerrainCoord" + num));
				}
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}

		public static Mesh CreatePackedMesh(TVEModelData meshData)
		{
			Mesh mesh = Object.Instantiate(meshData.mesh);
			int vertexCount = mesh.vertexCount;
			Bounds bounds = mesh.bounds;
			float a = Mathf.Max(Mathf.Abs(bounds.min.x), Mathf.Abs(bounds.max.x));
			float b = Mathf.Max(Mathf.Abs(bounds.min.z), Mathf.Abs(bounds.max.z));
			float radius = Mathf.Max(a, b) / 100f;
			float height = Mathf.Max(Mathf.Abs(bounds.min.y), Mathf.Abs(bounds.max.y)) / 100f;
			if (meshData.height == 0f)
			{
				meshData.height = height;
			}
			if (meshData.radius == 0f)
			{
				meshData.radius = radius;
			}
			List<float> list = new List<float>(vertexCount);
			List<Vector2> list2 = new List<Vector2>(vertexCount);
			List<Vector3> list3 = new List<Vector3>(vertexCount);
			List<Vector4> list4 = new List<Vector4>(vertexCount);
			List<Color> list5 = new List<Color>(vertexCount);
			List<Vector4> list6 = new List<Vector4>(vertexCount);
			List<Vector4> list7 = new List<Vector4>(vertexCount);
			List<Vector4> list8 = new List<Vector4>(vertexCount);
			for (int i = 0; i < vertexCount; i++)
			{
				list.Add(1f);
				list2.Add(Vector2.zero);
				list3.Add(Vector3.zero);
				list4.Add(Vector4.zero);
			}
			mesh.GetColors(list5);
			mesh.GetUVs(0, list6);
			mesh.GetUVs(1, list7);
			mesh.GetUVs(3, list8);
			if (list7.Count == 0)
			{
				list7 = list4;
			}
			if (list8.Count == 0)
			{
				list8 = list4;
			}
			if (meshData.variationMask == null)
			{
				meshData.variationMask = list;
			}
			if (meshData.occlusionMask == null)
			{
				meshData.occlusionMask = list;
			}
			if (meshData.detailMask == null)
			{
				meshData.detailMask = list;
			}
			if (meshData.heightMask == null)
			{
				meshData.heightMask = list;
			}
			if (meshData.motion2Mask == null)
			{
				meshData.motion2Mask = list;
			}
			if (meshData.motion3Mask == null)
			{
				meshData.motion3Mask = list;
			}
			if (meshData.detailCoord == null)
			{
				meshData.detailCoord = list2;
			}
			if (meshData.detailCoord == null)
			{
				meshData.pivotPositions = list3;
			}
			for (int j = 0; j < vertexCount; j++)
			{
				list5[j] = new Color(meshData.variationMask[j], meshData.occlusionMask[j], meshData.detailMask[j], meshData.heightMask[j]);
				list6[j] = new Vector4(list6[j].x, list6[j].y, BoxoUtils.MathVector2ToFloat(meshData.motion2Mask[j], meshData.motion3Mask[j]), BoxoUtils.MathVector2ToFloat(meshData.height / 100f, meshData.radius / 100f));
				list7[j] = new Vector4(list7[j].x, list7[j].y, meshData.detailCoord[j].x, meshData.detailCoord[j].y);
				list8[j] = new Vector4(meshData.pivotPositions[j].x, meshData.pivotPositions[j].z, meshData.pivotPositions[j].y, 0f);
			}
			mesh.SetColors(list5);
			mesh.SetUVs(0, list6);
			mesh.SetUVs(1, list7);
			mesh.SetUVs(3, list8);
			list.Clear();
			list2.Clear();
			list3.Clear();
			list4.Clear();
			return mesh;
		}

		public static Mesh CombinePackedMeshes(List<GameObject> gameObjects, bool mergeSubMeshes, bool usePrebakedPivots)
		{
			Mesh mesh = new Mesh();
			mesh.indexFormat = IndexFormat.UInt32;
			CombineInstance[] array = new CombineInstance[gameObjects.Count];
			for (int i = 0; i < gameObjects.Count; i++)
			{
				Mesh mesh2 = Object.Instantiate(gameObjects[i].GetComponent<MeshFilter>().sharedMesh);
				MeshRenderer component = gameObjects[i].GetComponent<MeshRenderer>();
				int vertexCount = mesh2.vertexCount;
				List<Vector3> list = new List<Vector3>(vertexCount);
				List<Vector4> list2 = new List<Vector4>(vertexCount);
				mesh2.GetUVs(3, list);
				if (usePrebakedPivots)
				{
					for (int j = 0; j < vertexCount; j++)
					{
						Vector3 position = new Vector3(list[j].x, list[j].z, list[j].y);
						Vector3 vector = gameObjects[i].transform.TransformPoint(position);
						Vector4 item = new Vector4(vector.x, vector.z, vector.y, 0f);
						list2.Add(item);
					}
				}
				else
				{
					for (int k = 0; k < vertexCount; k++)
					{
						Vector3 position2 = gameObjects[i].transform.position;
						Vector4 item2 = new Vector4(position2.x, position2.z, position2.y, 0f);
						list2.Add(item2);
					}
				}
				mesh2.SetUVs(3, list2);
				array[i].mesh = mesh2;
				array[i].transform = component.transform.localToWorldMatrix;
				array[i].lightmapScaleOffset = component.lightmapScaleOffset;
				array[i].realtimeLightmapScaleOffset = component.realtimeLightmapScaleOffset;
			}
			mesh.CombineMeshes(array, mergeSubMeshes, useMatrices: true, hasLightmapData: true);
			return mesh;
		}

		public static Mesh CombinePackedMeshes(List<GameObject> gameObjects, bool mergeSubMeshes)
		{
			return CombinePackedMeshes(gameObjects, mergeSubMeshes, usePrebakedPivots: true);
		}

		public static Mesh CombineColliderMeshes(List<GameObject> gameObjects)
		{
			Mesh mesh = new Mesh();
			CombineInstance[] array = new CombineInstance[gameObjects.Count];
			for (int i = 0; i < gameObjects.Count; i++)
			{
				Mesh mesh2 = Object.Instantiate(gameObjects[i].GetComponent<MeshFilter>().sharedMesh);
				MeshRenderer component = gameObjects[i].GetComponent<MeshRenderer>();
				Matrix4x4 localToWorldMatrix = component.transform.localToWorldMatrix;
				array[i].mesh = mesh2;
				array[i].transform = localToWorldMatrix;
				array[i].lightmapScaleOffset = component.lightmapScaleOffset;
				array[i].realtimeLightmapScaleOffset = component.realtimeLightmapScaleOffset;
			}
			mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true, hasLightmapData: false);
			return mesh;
		}

		public static List<Mesh> SplitPackedMesh(Mesh mesh)
		{
			List<Mesh> list = new List<Mesh>();
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Mesh submesh = GetSubmesh(mesh, i);
				list.Add(submesh);
			}
			return list;
		}

		public static Mesh GetSubmesh(Mesh mesh, int submeshIndex)
		{
			int[] triangles = mesh.GetTriangles(submeshIndex, applyBaseVertex: true);
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector4[] tangents = mesh.tangents;
			Color[] colors = mesh.colors;
			List<Vector4> list = new List<Vector4>();
			List<Vector4> list2 = new List<Vector4>();
			List<Vector4> list3 = new List<Vector4>();
			mesh.GetUVs(0, list);
			mesh.GetUVs(1, list2);
			mesh.GetUVs(3, list3);
			HashSet<Vector3> hashSet = new HashSet<Vector3>();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<Vector3, int> dictionary2 = new Dictionary<Vector3, int>();
			int[] array = triangles;
			foreach (int num in array)
			{
				Vector3 vector = vertices[num];
				if (!hashSet.Contains(vector))
				{
					hashSet.Add(vector);
					int value = (dictionary[num] = hashSet.Count - 1);
					dictionary2[vector] = value;
				}
			}
			Vector3[] array2 = new Vector3[hashSet.Count];
			Vector3[] array3 = new Vector3[hashSet.Count];
			Vector4[] array4 = new Vector4[hashSet.Count];
			Color[] array5 = new Color[hashSet.Count];
			List<Vector4> list4 = new List<Vector4>();
			List<Vector4> list5 = new List<Vector4>();
			List<Vector4> list6 = new List<Vector4>();
			hashSet.CopyTo(array2);
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				int key = item.Key;
				int value2 = item.Value;
				array3[value2] = normals[key];
				array4[value2] = tangents[key];
				array5[value2] = colors[key];
				list4.Add(list[key]);
				list5.Add(list2[key]);
				list6.Add(list3[key]);
			}
			int[] array6 = new int[triangles.Length];
			for (int j = 0; j < triangles.Length; j += 3)
			{
				int num3 = dictionary2[vertices[triangles[j]]];
				int num4 = dictionary2[vertices[triangles[j + 1]]];
				int num5 = dictionary2[vertices[triangles[j + 2]]];
				array6[j] = num3;
				array6[j + 1] = num4;
				array6[j + 2] = num5;
			}
			Mesh mesh2 = new Mesh();
			mesh2.vertices = array2;
			mesh2.normals = array3;
			mesh2.tangents = array4;
			mesh2.colors = array5;
			mesh2.SetUVs(0, list4);
			mesh2.SetUVs(1, list5);
			mesh2.SetUVs(3, list6);
			mesh2.triangles = array6;
			return mesh2;
		}

		public static Texture CreateProxyTextureFromTerrain(TVEProxyData proxyData)
		{
			TVETerrain blitTVETerrain = proxyData.blitTVETerrain;
			if (blitTVETerrain == null)
			{
				return null;
			}
			Mesh blitMesh = proxyData.blitMesh;
			Material material = new Material(proxyData.blitMaterial);
			material.shader = proxyData.blitShader;
			material.SetInt("_BakeDataMode", proxyData.bakeData);
			if (proxyData.bakeAlbedoAsSRGB)
			{
				material.SetInt("_BakeAlbedoSRGBMode", 1);
			}
			else
			{
				material.SetInt("_BakeAlbedoSRGBMode", 0);
			}
			MaterialPropertyBlock terrainPropertyBlock = blitTVETerrain.terrainPropertyBlock;
			RenderTexture renderTexture = new RenderTexture(proxyData.saveSize, proxyData.saveSize, 0, RenderTextureFormat.ARGBHalf);
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.SetRenderTarget(renderTexture);
			commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.clear);
			commandBuffer.SetViewport(new Rect(0f, 0f, renderTexture.width, renderTexture.height));
			Vector3 terrainPosition = blitTVETerrain.terrainPosition;
			Vector3 terrainSize = blitTVETerrain.terrainSize;
			float x = terrainPosition.x;
			float right = terrainPosition.x + terrainSize.x;
			float z = terrainPosition.z;
			float top = terrainPosition.z + terrainSize.z;
			Matrix4x4 inverse = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 0f), Vector3.one).inverse;
			Matrix4x4 proj = Matrix4x4.Ortho(x, right, z, top, -10000f, 10000f);
			commandBuffer.SetViewProjectionMatrices(inverse, proj);
			commandBuffer.DrawMesh(blitMesh, Matrix4x4.identity, material, 0, 0, terrainPropertyBlock);
			Graphics.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Release();
			return renderTexture;
		}

		public static Mesh CreateQuadFromTerrain(Vector3 terrainPos, Vector3 terrainSize)
		{
			Mesh mesh = new Mesh();
			Vector3 vector = new Vector3(terrainPos.x, 0f, terrainPos.z);
			Vector3 vector2 = new Vector3(terrainPos.x + terrainSize.x, 0f, terrainPos.z);
			Vector3 vector3 = new Vector3(terrainPos.x, 0f, terrainPos.z + terrainSize.z);
			Vector3 vector4 = new Vector3(terrainPos.x + terrainSize.x, 0f, terrainPos.z + terrainSize.z);
			Vector3[] vertices = new Vector3[4] { vector, vector2, vector3, vector4 };
			Vector2[] uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		public static void GetChildRecursive(GameObject go, List<GameObject> gameObjects)
		{
			foreach (Transform item in go.transform)
			{
				if (!(item == null))
				{
					gameObjects.Add(item.gameObject);
					GetChildRecursive(item.gameObject, gameObjects);
				}
			}
		}

		public static void GetChildRecursive(GameObject go, List<TVEGameObjectData> gameObjectsData)
		{
			foreach (Transform item in go.transform)
			{
				if (!(item == null))
				{
					TVEGameObjectData tVEGameObjectData = new TVEGameObjectData();
					tVEGameObjectData.gameObject = item.gameObject;
					gameObjectsData.Add(tVEGameObjectData);
					GetChildRecursive(item.gameObject, gameObjectsData);
				}
			}
		}

		public static Color GetGlobalTextureData(string globalTexture, Vector3 position, int layer, Texture2DArray texture2DArray)
		{
			if (TVEManager.Instance == null)
			{
				return Color.black;
			}
			RenderTexture renderTexture = Shader.GetGlobalTexture(globalTexture) as RenderTexture;
			if (renderTexture == null)
			{
				return Color.black;
			}
			if (layer > renderTexture.volumeDepth - 1)
			{
				Debug.Log("<b>[The Visual Engine]</b> The requested global texture layer does not exist!");
				return Color.black;
			}
			if (texture2DArray == null || texture2DArray.depth != renderTexture.volumeDepth)
			{
				texture2DArray = new Texture2DArray(1, 1, renderTexture.volumeDepth, TextureFormat.RGBAHalf, mipChain: false);
			}
			Vector4 globalVector = Shader.GetGlobalVector("TVE_RenderBasePositionR");
			if (globalTexture.Contains("Near"))
			{
				globalVector = Shader.GetGlobalVector("TVE_RenderNearPositionR");
			}
			Vector3 vector = new Vector3(globalVector.x, globalVector.y, globalVector.z);
			float w = globalVector.w;
			float num = Mathf.Clamp(BoxoUtils.MathRemap(position.x, vector.x - w, vector.x + w, 0f, 1f), 0.001f, 1f);
			float num2 = Mathf.Clamp(BoxoUtils.MathRemap(position.z, vector.z - w, vector.z + w, 0f, 1f), 0.001f, 1f);
			int x = Mathf.RoundToInt(num * (float)renderTexture.width - 1f);
			int y = Mathf.RoundToInt(num2 * (float)renderTexture.height - 1f);
			AsyncGPUReadbackRequest asyncGPUReadbackRequest = AsyncGPUReadback.Request(renderTexture, 0, x, 1, y, 1, layer, 1);
			asyncGPUReadbackRequest.WaitForCompletion();
			if (!asyncGPUReadbackRequest.hasError)
			{
				texture2DArray.SetPixelData(asyncGPUReadbackRequest.GetData<byte>(), 0, layer);
				texture2DArray.Apply();
				return texture2DArray.GetPixels(layer, 0)[0];
			}
			return Color.black;
		}
	}
}
