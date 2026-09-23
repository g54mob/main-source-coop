using Mimicraft.Settings;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class EditorPalette
	{
		public static readonly Color DefaultExtrudeGizmo = new Color(1f, 0.55f, 0.1f, 0.95f);

		public static readonly Color DefaultTransformGizmo = new Color(1f, 0.95f, 0.3f, 1f);

		public static readonly Color DefaultGrid = new Color(0f, 0f, 0f, 0.9f);

		public static Color ExtrudeGizmo => GameSettings.ExtrudeGizmoColor;

		public static Color TransformGizmo => GameSettings.TransformGizmoColor;

		public static Color Grid => GameSettings.EditorGridColor;

		public static Color GridSoft => WithAlpha(Grid, Grid.a * 0.4f);

		public static Color WithAlpha(Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}
