using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public class VoxelOrientCommand : IUndoableCommand
	{
		private readonly struct Matrix
		{
			private readonly Vector3Int x;

			private readonly Vector3Int y;

			private readonly Vector3Int z;

			public Matrix(Vector3Int x, Vector3Int y, Vector3Int z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public static Vector3Int operator *(Matrix m, Vector3Int v)
			{
				return m.x * v.x + m.y * v.y + m.z * v.z;
			}

			public Matrix Transposed()
			{
				return new Matrix(new Vector3Int(x.x, y.x, z.x), new Vector3Int(x.y, y.y, z.y), new Vector3Int(x.z, y.z, z.z));
			}
		}

		private readonly VoxelModel model;

		private readonly VoxelOrientation orientation;

		private readonly Vector3Int offset;

		public string Description => LabelOf(orientation);

		public int RetainedCells => 0;

		private VoxelOrientCommand(VoxelModel model, VoxelOrientation orientation, Vector3Int offset)
		{
			this.model = model;
			this.orientation = orientation;
			this.offset = offset;
		}

		public void Redo()
		{
			Write(model, Map(orientation), offset);
		}

		public void Undo()
		{
			Matrix matrix = Map(orientation).Transposed();
			Write(model, matrix, -(matrix * offset));
		}

		public static bool TryApply(VoxelModel model, VoxelOrientation orientation, out string error)
		{
			error = null;
			if (model == null || model.Grid == null)
			{
				error = "Model yok.";
				return false;
			}
			if (model.Grid.Count == 0)
			{
				error = "Model boş.";
				return false;
			}
			Matrix map = Map(orientation);
			if (!TryFindOffset(model, map, out var vector3Int))
			{
				error = "Bu dönüş modeli düzenleme kutusunun dışına taşırdı.";
				return false;
			}
			VoxelOrientCommand command = new VoxelOrientCommand(model, orientation, vector3Int);
			Write(model, map, vector3Int);
			UndoManager.Push(command);
			return true;
		}

		private static bool TryFindOffset(VoxelModel model, Matrix map, out Vector3Int offset)
		{
			offset = Vector3Int.zero;
			if (!model.Grid.TryGetBounds(out var min, out var max))
			{
				return false;
			}
			Vector3Int vector3Int = map * min;
			Vector3Int vector3Int2 = map * max;
			Vector3Int vector3Int3 = new Vector3Int(Mathf.Min(vector3Int.x, vector3Int2.x), Mathf.Min(vector3Int.y, vector3Int2.y), Mathf.Min(vector3Int.z, vector3Int2.z));
			Vector3Int vector3Int4 = new Vector3Int(Mathf.Max(vector3Int.x, vector3Int2.x), Mathf.Max(vector3Int.y, vector3Int2.y), Mathf.Max(vector3Int.z, vector3Int2.z));
			offset = new Vector3Int(Mathf.RoundToInt((float)(min.x + max.x - vector3Int3.x - vector3Int4.x) * 0.5f), Mathf.RoundToInt((float)(min.y + max.y - vector3Int3.y - vector3Int4.y) * 0.5f), Mathf.RoundToInt((float)(min.z + max.z - vector3Int3.z - vector3Int4.z) * 0.5f));
			if (model.EditBounds == null)
			{
				return true;
			}
			foreach (Vector3Int position in model.Grid.Positions)
			{
				if (!model.CanAdd(map * position + offset))
				{
					return false;
				}
			}
			return true;
		}

		private static void Write(VoxelModel model, Matrix map, Vector3Int offset)
		{
			VoxelGrid grid = model.Grid;
			List<KeyValuePair<Vector3Int, VoxelData>> list = new List<KeyValuePair<Vector3Int, VoxelData>>(grid.Count);
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in grid.Voxels)
			{
				list.Add(voxel);
			}
			List<KeyValuePair<(Vector3Int, int), Color32>> list2 = new List<KeyValuePair<(Vector3Int, int), Color32>>(grid.FaceColorCount);
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in grid.FaceColors)
			{
				list2.Add(faceColor);
			}
			grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> item in list)
			{
				grid.Set(map * item.Key + offset, item.Value);
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> item2 in list2)
			{
				if (item2.Key.Item2 >= 0 && item2.Key.Item2 < 6)
				{
					int num = FaceAxes.IndexOf(map * FaceAxes.Normals[item2.Key.Item2]);
					if (num >= 0)
					{
						grid.SetFaceColor(map * item2.Key.Item1 + offset, num, item2.Value);
					}
				}
			}
			model.RebuildMesh();
		}

		public static string LabelOf(VoxelOrientation orientation)
		{
			return orientation switch
			{
				VoxelOrientation.FlipX => "Flip X", 
				VoxelOrientation.FlipY => "Flip Y", 
				VoxelOrientation.FlipZ => "Flip Z", 
				VoxelOrientation.RotateX90 => "Rotate X 90°", 
				VoxelOrientation.RotateY90 => "Rotate Y 90°", 
				VoxelOrientation.RotateZ90 => "Rotate Z 90°", 
				VoxelOrientation.RotateX180 => "Rotate X 180°", 
				VoxelOrientation.RotateY180 => "Rotate Y 180°", 
				_ => "Rotate Z 180°", 
			};
		}

		private static Matrix Map(VoxelOrientation orientation)
		{
			Vector3Int vector3Int = new Vector3Int(1, 0, 0);
			Vector3Int vector3Int2 = new Vector3Int(-1, 0, 0);
			Vector3Int vector3Int3 = new Vector3Int(0, 1, 0);
			Vector3Int vector3Int4 = new Vector3Int(0, -1, 0);
			Vector3Int vector3Int5 = new Vector3Int(0, 0, 1);
			Vector3Int vector3Int6 = new Vector3Int(0, 0, -1);
			return orientation switch
			{
				VoxelOrientation.FlipX => new Matrix(vector3Int2, vector3Int3, vector3Int5), 
				VoxelOrientation.FlipY => new Matrix(vector3Int, vector3Int4, vector3Int5), 
				VoxelOrientation.FlipZ => new Matrix(vector3Int, vector3Int3, vector3Int6), 
				VoxelOrientation.RotateX90 => new Matrix(vector3Int, vector3Int5, vector3Int4), 
				VoxelOrientation.RotateY90 => new Matrix(vector3Int6, vector3Int3, vector3Int), 
				VoxelOrientation.RotateZ90 => new Matrix(vector3Int3, vector3Int2, vector3Int5), 
				VoxelOrientation.RotateX180 => new Matrix(vector3Int, vector3Int4, vector3Int6), 
				VoxelOrientation.RotateY180 => new Matrix(vector3Int2, vector3Int3, vector3Int6), 
				_ => new Matrix(vector3Int2, vector3Int4, vector3Int5), 
			};
		}
	}
}
