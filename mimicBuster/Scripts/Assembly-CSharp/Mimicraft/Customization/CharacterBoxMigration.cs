using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class CharacterBoxMigration
	{
		public static void Apply(CharacterData data, IReadOnlyDictionary<string, Vector3Int> savedBoxes, CharacterRigDefinition rig)
		{
			if (data == null || rig == null)
			{
				return;
			}
			foreach (CharacterPartData part in data.Parts)
			{
				if (part?.Grid == null || part.Grid.Count == 0)
				{
					continue;
				}
				CharacterPartDefinition characterPartDefinition = rig.Find(part.PartId);
				if (characterPartDefinition == null)
				{
					continue;
				}
				Vector3Int value;
				Vector3Int vector3Int = ((savedBoxes != null && savedBoxes.TryGetValue(part.PartId, out value)) ? value : characterPartDefinition.AuthoredBoxSize);
				Vector3Int boxSize = characterPartDefinition.BoxSize;
				int num = WholeFactor(boxSize, vector3Int);
				if (num > 1)
				{
					part.Grid = Grow(part.Grid, num);
					continue;
				}
				int num2 = WholeFactor(vector3Int, boxSize);
				if (num2 > 1)
				{
					part.Grid = Shrink(part.Grid, num2);
					Debug.Log($"[CharacterBoxMigration] '{part.PartId}' {vector3Int.x}³ boyutundan {boxSize.x}³ " + $"boyutuna indirildi (1/{num2}) - detay kaybı olabilir.");
				}
			}
		}

		private static int WholeFactor(Vector3Int bigger, Vector3Int smaller)
		{
			if (smaller.x <= 0 || smaller.y <= 0 || smaller.z <= 0)
			{
				return 1;
			}
			if (bigger.x % smaller.x != 0 || bigger.y % smaller.y != 0 || bigger.z % smaller.z != 0)
			{
				return 1;
			}
			int num = bigger.x / smaller.x;
			if (num != bigger.y / smaller.y || num != bigger.z / smaller.z)
			{
				return 1;
			}
			return num;
		}

		private static VoxelGrid Grow(VoxelGrid source, int factor)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in source.Voxels)
			{
				Vector3Int vector3Int = voxel.Key * factor;
				for (int i = 0; i < factor; i++)
				{
					for (int j = 0; j < factor; j++)
					{
						for (int k = 0; k < factor; k++)
						{
							voxelGrid.Set(vector3Int + new Vector3Int(k, j, i), voxel.Value);
						}
					}
				}
			}
			GrowFaceColours(source, voxelGrid, factor);
			return voxelGrid;
		}

		private static void GrowFaceColours(VoxelGrid source, VoxelGrid grown, int factor)
		{
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in source.FaceColors)
			{
				int item = faceColor.Key.Item2;
				if (item < 0 || item >= 6)
				{
					continue;
				}
				Vector3Int normal = FaceAxes.Normals[item];
				Vector3Int vector3Int = faceColor.Key.Item1 * factor;
				for (int i = 0; i < factor; i++)
				{
					for (int j = 0; j < factor; j++)
					{
						for (int k = 0; k < factor; k++)
						{
							Vector3Int vector3Int2 = new Vector3Int(k, j, i);
							if (OnOuterLayer(vector3Int2, normal, factor))
							{
								grown.SetFaceColor(vector3Int + vector3Int2, item, faceColor.Value);
							}
						}
					}
				}
			}
		}

		private static VoxelGrid Shrink(VoxelGrid source, int factor)
		{
			Dictionary<Vector3Int, List<Color32>> dictionary = new Dictionary<Vector3Int, List<Color32>>();
			foreach (Vector3Int item in Ordered(source.Positions))
			{
				if (source.TryGet(item, out var data))
				{
					Vector3Int key = FloorDiv(item, factor);
					if (!dictionary.TryGetValue(key, out var value))
					{
						value = (dictionary[key] = new List<Color32>());
					}
					value.Add(data.Color);
				}
			}
			VoxelGrid voxelGrid = new VoxelGrid();
			foreach (KeyValuePair<Vector3Int, List<Color32>> item2 in dictionary)
			{
				voxelGrid.Set(item2.Key, new VoxelData(Commonest(item2.Value)));
			}
			ShrinkFaceColours(source, voxelGrid, factor);
			return voxelGrid;
		}

		private static void ShrinkFaceColours(VoxelGrid source, VoxelGrid shrunk, int factor)
		{
			Dictionary<(Vector3Int, int), List<Color32>> dictionary = new Dictionary<(Vector3Int, int), List<Color32>>();
			foreach (KeyValuePair<(Vector3Int, int), Color32> item3 in Ordered(source.FaceColors))
			{
				int item = item3.Key.Item2;
				if (item < 0 || item >= 6)
				{
					continue;
				}
				Vector3Int item2 = item3.Key.Item1;
				Vector3Int vector3Int = FloorDiv(item2, factor);
				if (OnOuterLayer(item2 - vector3Int * factor, FaceAxes.Normals[item], factor))
				{
					(Vector3Int, int) key = (vector3Int, item);
					if (!dictionary.TryGetValue(key, out var value))
					{
						value = (dictionary[key] = new List<Color32>());
					}
					value.Add(item3.Value);
				}
			}
			foreach (KeyValuePair<(Vector3Int, int), List<Color32>> item4 in dictionary)
			{
				if (shrunk.Contains(item4.Key.Item1))
				{
					shrunk.SetFaceColor(item4.Key.Item1, item4.Key.Item2, Commonest(item4.Value));
				}
			}
		}

		private static bool OnOuterLayer(Vector3Int within, Vector3Int normal, int factor)
		{
			if (normal.x != 0 && within.x != ((normal.x > 0) ? (factor - 1) : 0))
			{
				return false;
			}
			if (normal.y != 0 && within.y != ((normal.y > 0) ? (factor - 1) : 0))
			{
				return false;
			}
			if (normal.z != 0 && within.z != ((normal.z > 0) ? (factor - 1) : 0))
			{
				return false;
			}
			return true;
		}

		private static Vector3Int FloorDiv(Vector3Int cell, int factor)
		{
			return new Vector3Int(Mathf.FloorToInt((float)cell.x / (float)factor), Mathf.FloorToInt((float)cell.y / (float)factor), Mathf.FloorToInt((float)cell.z / (float)factor));
		}

		private static Color32 Commonest(List<Color32> colours)
		{
			Color32 result = colours[0];
			int num = 0;
			for (int i = 0; i < colours.Count; i++)
			{
				int num2 = 0;
				for (int j = 0; j < colours.Count; j++)
				{
					if (Same(colours[i], colours[j]))
					{
						num2++;
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = colours[i];
				}
			}
			return result;
		}

		private static bool Same(Color32 a, Color32 b)
		{
			if (a.r == b.r && a.g == b.g && a.b == b.b)
			{
				return a.a == b.a;
			}
			return false;
		}

		private static List<Vector3Int> Ordered(IEnumerable<Vector3Int> cells)
		{
			List<Vector3Int> list = new List<Vector3Int>(cells);
			list.Sort(Compare);
			return list;
		}

		private static List<KeyValuePair<(Vector3Int Position, int Face), Color32>> Ordered(IEnumerable<KeyValuePair<(Vector3Int Position, int Face), Color32>> painted)
		{
			List<KeyValuePair<(Vector3Int, int), Color32>> list = new List<KeyValuePair<(Vector3Int, int), Color32>>(painted);
			list.Sort(delegate(KeyValuePair<(Vector3Int Position, int Face), Color32> a, KeyValuePair<(Vector3Int Position, int Face), Color32> b)
			{
				int num = Compare(a.Key.Position, b.Key.Position);
				return (num == 0) ? a.Key.Face.CompareTo(b.Key.Face) : num;
			});
			return list;
		}

		private static int Compare(Vector3Int a, Vector3Int b)
		{
			if (a.z != b.z)
			{
				return a.z.CompareTo(b.z);
			}
			if (a.y != b.y)
			{
				return a.y.CompareTo(b.y);
			}
			return a.x.CompareTo(b.x);
		}
	}
}
