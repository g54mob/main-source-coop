using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Customization
{
	public sealed class WeaponSkinBox : IVoxelBox
	{
		public const int MaxCells = 512;

		public string DisplayName { get; }

		public Vector3Int BoxSize { get; }

		public Vector3Int LimitMin => Vector3Int.zero;

		public Vector3Int LimitSize => BoxSize;

		public float VoxelSize { get; }

		public Vector3 LocalCorner { get; }

		public Vector3 MeasuredCentre { get; }

		public bool HasRequiredCore => false;

		public bool IsRequired(Vector3Int cell)
		{
			return false;
		}

		public bool IsInsideBox(Vector3Int cell)
		{
			if (cell.x >= 0 && cell.y >= 0 && cell.z >= 0 && cell.x < BoxSize.x && cell.y < BoxSize.y)
			{
				return cell.z < BoxSize.z;
			}
			return false;
		}

		private WeaponSkinBox(string displayName, Vector3Int boxSize, float voxelSize, Vector3 localCorner, Vector3 measuredCentre)
		{
			DisplayName = displayName;
			BoxSize = boxSize;
			VoxelSize = voxelSize;
			LocalCorner = localCorner;
			MeasuredCentre = measuredCentre;
		}

		public static bool TryCreate(GameObject measureFrom, string displayName, Vector3Int authoredBox, float voxelSize, Vector3 anchorOffset, out WeaponSkinBox box, bool centreOnZ = false, Transform measureFrame = null)
		{
			string explanation;
			return TryCreate(measureFrom, displayName, authoredBox, voxelSize, anchorOffset, out box, out explanation, centreOnZ, measureFrame);
		}

		public static bool TryCreate(GameObject measureFrom, string displayName, Vector3Int authoredBox, float voxelSize, Vector3 anchorOffset, out WeaponSkinBox box, out string explanation, bool centreOnZ = false, Transform measureFrame = null)
		{
			box = null;
			explanation = "";
			if (voxelSize <= 0.0001f)
			{
				explanation = "Voxel Size sifir";
				return false;
			}
			Bounds bounds;
			bool flag = TryMeasureBounds(measureFrom, measureFrame, out bounds);
			if ((authoredBox.x <= 0 || authoredBox.y <= 0 || authoredBox.z <= 0) && !flag)
			{
				explanation = "'" + displayName + "' icin kutu belirlenemedi: Skin Box Size " + $"{authoredBox.x}x{authoredBox.y}x{authoredBox.z} ve '{measureFrom?.name}' altinda " + "olculecek bir renderer yok. WeaponDefinition'da Skin Box Size'i doldur.";
				return false;
			}
			Vector3Int vector3Int = new Vector3Int(ResolveAxis(authoredBox.x, flag ? bounds.size.x : 0f, voxelSize), ResolveAxis(authoredBox.y, flag ? bounds.size.y : 0f, voxelSize), ResolveAxis(authoredBox.z, flag ? bounds.size.z : 0f, voxelSize));
			Vector3 vector = (Vector3)vector3Int * voxelSize;
			Vector3 measuredCentre = (flag ? bounds.center : Vector3.zero);
			Vector3 vector2 = (centreOnZ ? new Vector3((float)(-(vector3Int.x / 2)) * voxelSize, (float)(-(vector3Int.y / 2)) * voxelSize, (float)(-(vector3Int.z / 2)) * voxelSize) : new Vector3(measuredCentre.x - vector.x * 0.5f, measuredCentre.y - vector.y * 0.5f, 0f));
			box = new WeaponSkinBox(displayName, vector3Int, voxelSize, vector2 + anchorOffset, measuredCentre);
			string text = (flag ? ($"min({bounds.min.x:0.###},{bounds.min.y:0.###},{bounds.min.z:0.###}) " + $"max({bounds.max.x:0.###},{bounds.max.y:0.###},{bounds.max.z:0.###})") : "YOK");
			string text2 = "";
			if (flag)
			{
				Vector3Int vector3Int2 = new Vector3Int(Mathf.CeilToInt(bounds.size.x / voxelSize), Mathf.CeilToInt(bounds.size.y / voxelSize), Mathf.CeilToInt(bounds.size.z / voxelSize));
				if (vector3Int2.x != vector3Int.x || vector3Int2.y != vector3Int.y || vector3Int2.z != vector3Int.z)
				{
					text2 = $" [mesh'e tam oturan kutu: {vector3Int2.x}x{vector3Int2.y}x{vector3Int2.z}]";
				}
			}
			explanation = "olculen " + text + text2 + ", " + $"yazilan kutu {authoredBox.x}x{authoredBox.y}x{authoredBox.z}, " + "olculen mesh " + (flag ? $"{bounds.size.x:0.###}x{bounds.size.y:0.###}x{bounds.size.z:0.###}m" : "YOK") + ", " + $"kullanilan {vector3Int.x}x{vector3Int.y}x{vector3Int.z} voxel = " + $"{vector.x:0.####}x{vector.y:0.####}x{vector.z:0.####}m, " + $"kose {box.LocalCorner.x:0.####},{box.LocalCorner.y:0.####},{box.LocalCorner.z:0.####}" + ((anchorOffset == Vector3.zero) ? "" : $" (Anchor Offset {anchorOffset} dahil)");
			return true;
		}

		private static int ResolveAxis(int authored, float measuredSize, float voxelSize)
		{
			if (authored > 0)
			{
				return Mathf.Min(authored, 512);
			}
			return Mathf.Clamp(Mathf.CeilToInt(measuredSize / voxelSize), 1, 512);
		}

		private static bool TryMeasureBounds(GameObject measureFrom, Transform frame, out Bounds bounds)
		{
			bounds = default(Bounds);
			if (measureFrom == null)
			{
				return false;
			}
			Transform transform = ((frame != null) ? frame : measureFrom.transform);
			bool flag = false;
			Renderer[] componentsInChildren = measureFrom.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer == null || renderer.GetComponentInParent<VoxelModel>() != null)
				{
					continue;
				}
				Bounds localBounds = renderer.localBounds;
				Vector3 center = localBounds.center;
				Vector3 extents = localBounds.extents;
				Transform transform2 = renderer.transform;
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = new Vector3(center.x + (((j & 1) == 0) ? (0f - extents.x) : extents.x), center.y + (((j & 2) == 0) ? (0f - extents.y) : extents.y), center.z + (((j & 4) == 0) ? (0f - extents.z) : extents.z));
					Vector3 vector = transform.InverseTransformPoint(transform2.TransformPoint(position));
					if (flag)
					{
						bounds.Encapsulate(vector);
						continue;
					}
					bounds = new Bounds(vector, Vector3.zero);
					flag = true;
				}
			}
			return flag;
		}
	}
}
