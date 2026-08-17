using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public static class LootCategoryHelper
	{
		public static Color GetCategoryColor(LootCategory category)
		{
			return category switch
			{
				LootCategory.Food => new Color(0.2f, 0.8f, 0.2f), 
				LootCategory.Drink => new Color(0.2f, 0.6f, 1f), 
				LootCategory.VehiclePart => new Color(1f, 0.5f, 0f), 
				LootCategory.Tool => new Color(0.8f, 0.8f, 0.2f), 
				LootCategory.Kitchen => new Color(0.9f, 0.4f, 0.7f), 
				LootCategory.Furniture => new Color(0.6f, 0.4f, 0.2f), 
				LootCategory.Container => new Color(0.5f, 0.5f, 0.5f), 
				LootCategory.Electronics => new Color(0.4f, 0.8f, 0.9f), 
				LootCategory.Seeds => new Color(0.5f, 0.9f, 0.5f), 
				LootCategory.Vehicle => new Color(0.8f, 0.2f, 0.2f), 
				LootCategory.Misc => new Color(0.7f, 0.7f, 0.7f), 
				_ => Color.white, 
			};
		}

		public static string GetIconName(LootCategory category)
		{
			return category switch
			{
				LootCategory.Food => "sv_icon_dot3_pix16_gizmo", 
				LootCategory.Drink => "sv_icon_dot1_pix16_gizmo", 
				LootCategory.VehiclePart => "sv_icon_dot4_pix16_gizmo", 
				LootCategory.Tool => "sv_icon_dot7_pix16_gizmo", 
				LootCategory.Kitchen => "sv_icon_dot6_pix16_gizmo", 
				LootCategory.Furniture => "sv_icon_dot5_pix16_gizmo", 
				LootCategory.Container => "sv_icon_dot0_pix16_gizmo", 
				LootCategory.Electronics => "sv_icon_dot2_pix16_gizmo", 
				LootCategory.Seeds => "sv_icon_dot11_pix16_gizmo", 
				LootCategory.Vehicle => "d_Prefab Icon", 
				LootCategory.Misc => "sv_icon_dot8_pix16_gizmo", 
				_ => "sv_icon_dot0_pix16_gizmo", 
			};
		}
	}
}
