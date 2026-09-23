using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class LoopCutHighlight
	{
		private const float LineWidth = 0.03f;

		private static readonly Color LineColor = new Color(0.9f, 0.15f, 0.85f, 0.9f);

		private readonly Transform target;

		private readonly List<LineHighlight> lines = new List<LineHighlight>();

		private readonly Dictionary<Vector2Int, List<Vector2Int>> outgoing = new Dictionary<Vector2Int, List<Vector2Int>>();

		private readonly List<Vector2Int> loop = new List<Vector2Int>();

		private readonly List<Vector3> worldPoints = new List<Vector3>();

		public static LoopCutHighlight Create(Transform target)
		{
			return new LoopCutHighlight(target);
		}

		private LoopCutHighlight(Transform target)
		{
			this.target = target;
		}

		public void Show(HashSet<Vector2Int> cells, int axis, int coord)
		{
			BuildBoundaryEdges(cells);
			int index = 0;
			while (TryTakeLoop())
			{
				DrawLoop(index++, axis, coord);
			}
			HideFrom(index);
		}

		public void Hide()
		{
			HideFrom(0);
		}

		private void HideFrom(int index)
		{
			for (int i = index; i < lines.Count; i++)
			{
				lines[i].Hide();
			}
		}

		private void BuildBoundaryEdges(HashSet<Vector2Int> cells)
		{
			outgoing.Clear();
			foreach (Vector2Int cell in cells)
			{
				Vector2Int vector2Int = new Vector2Int(cell.x, cell.y);
				Vector2Int vector2Int2 = new Vector2Int(cell.x + 1, cell.y + 1);
				if (!cells.Contains(new Vector2Int(cell.x, cell.y - 1)))
				{
					AddEdge(vector2Int, new Vector2Int(vector2Int2.x, vector2Int.y));
				}
				if (!cells.Contains(new Vector2Int(cell.x + 1, cell.y)))
				{
					AddEdge(new Vector2Int(vector2Int2.x, vector2Int.y), vector2Int2);
				}
				if (!cells.Contains(new Vector2Int(cell.x, cell.y + 1)))
				{
					AddEdge(vector2Int2, new Vector2Int(vector2Int.x, vector2Int2.y));
				}
				if (!cells.Contains(new Vector2Int(cell.x - 1, cell.y)))
				{
					AddEdge(new Vector2Int(vector2Int.x, vector2Int2.y), vector2Int);
				}
			}
		}

		private void AddEdge(Vector2Int from, Vector2Int to)
		{
			if (!outgoing.TryGetValue(from, out var value))
			{
				value = new List<Vector2Int>(1);
				outgoing[from] = value;
			}
			value.Add(to);
		}

		private bool TryTakeLoop()
		{
			loop.Clear();
			Vector2Int vector2Int = default(Vector2Int);
			bool flag = false;
			foreach (KeyValuePair<Vector2Int, List<Vector2Int>> item in outgoing)
			{
				if (item.Value.Count != 0)
				{
					vector2Int = item.Key;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			Vector2Int vector2Int2 = vector2Int;
			for (int i = 0; i < 100000; i++)
			{
				if (!outgoing.TryGetValue(vector2Int2, out var value))
				{
					break;
				}
				if (value.Count == 0)
				{
					break;
				}
				Vector2Int vector2Int3 = value[value.Count - 1];
				value.RemoveAt(value.Count - 1);
				loop.Add(vector2Int2);
				vector2Int2 = vector2Int3;
				if (vector2Int2 == vector2Int)
				{
					break;
				}
			}
			return loop.Count >= 3;
		}

		private void DrawLoop(int index, int axis, int coord)
		{
			int index2 = (axis + 1) % 3;
			int index3 = (axis + 2) % 3;
			worldPoints.Clear();
			foreach (Vector2Int item in loop)
			{
				Vector3 zero = Vector3.zero;
				zero[axis] = coord;
				zero[index2] = item.x;
				zero[index3] = item.y;
				worldPoints.Add(target.TransformPoint(zero));
			}
			LineAt(index).Show(worldPoints.ToArray(), LineColor);
		}

		private LineHighlight LineAt(int index)
		{
			while (lines.Count <= index)
			{
				lines.Add(LineHighlight.Create(target, loop: true, 0.03f));
			}
			return lines[index];
		}
	}
}
