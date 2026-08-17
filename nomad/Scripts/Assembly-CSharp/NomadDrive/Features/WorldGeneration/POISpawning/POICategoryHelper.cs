using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public static class POICategoryHelper
	{
		public static Color GetCategoryColor(POICategory category)
		{
			return category switch
			{
				POICategory.Minor => new Color(0.4f, 0.7f, 0.4f), 
				POICategory.Major => new Color(0.9f, 0.5f, 0.2f), 
				_ => Color.white, 
			};
		}

		public static string GetIconName(POICategory category)
		{
			return category switch
			{
				POICategory.Minor => "sv_icon_dot3_pix16_gizmo", 
				POICategory.Major => "sv_icon_dot4_pix16_gizmo", 
				_ => "sv_icon_dot0_pix16_gizmo", 
			};
		}
	}
}
