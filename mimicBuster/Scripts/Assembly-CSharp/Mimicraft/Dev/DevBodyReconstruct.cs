using System.Collections.Generic;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Dev
{
	public static class DevBodyReconstruct
	{
		private sealed class Plan
		{
			public byte[] Source;

			public VoxelBodyData Target;

			public Entry[] Order;

			public int[] StepEnd;

			public VoxelBodyData Scratch;

			public int ShownCount = -1;

			public float NextRebuildTime;
		}

		private readonly struct Entry
		{
			public readonly int Piece;

			public readonly Vector3Int Cell;

			public Entry(int piece, Vector3Int cell)
			{
				Piece = piece;
				Cell = cell;
			}
		}

		private const int CubeSize = 3;

		private const int CubeCells = 27;

		private static readonly Color32 CubeColor = new Color32(139, 90, 43, byte.MaxValue);

		private const float MaxRebuildsPerSecond = 30f;

		private const float CubeShare = 0.12f;

		private const int MinSteps = 5;

		private const int MaxSteps = 24;

		private static readonly Dictionary<ulong, Plan> plans = new Dictionary<ulong, Plan>();

		private static readonly Vector3Int[] Neighbours = new Vector3Int[6]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 0, -1)
		};

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			plans.Clear();
		}

		public static void Forget(ulong actor)
		{
			plans.Remove(actor);
		}

		public static void Rewind(ulong actor)
		{
			if (plans.TryGetValue(actor, out var value))
			{
				value.ShownCount = -1;
			}
		}

		public static void ForgetAll()
		{
			plans.Clear();
		}

		public static byte[] TargetFor(DevRecording recording, DevRecording.Actor actor, int frame)
		{
			byte[] result = actor.BodyBytes;
			int num = int.MinValue;
			foreach (DevRecording.BodyChange bodyChange in recording.BodyChanges)
			{
				if (bodyChange.Actor == actor.Id && bodyChange.Frame <= frame && bodyChange.Bytes.Length != 0 && bodyChange.Frame >= num)
				{
					num = bodyChange.Frame;
					result = bodyChange.Bytes;
				}
			}
			return result;
		}

		public static bool TryProgressAt(DevRecording recording, ulong actor, float seconds, out float progress, out int startFrame)
		{
			progress = 0f;
			startFrame = 0;
			if (!recording.TryGetReconstruct(actor, out var found))
			{
				return false;
			}
			startFrame = found.Frame;
			float num = (float)found.Frame / Mathf.Max(0.001f, recording.TickRate);
			float num2 = seconds - num;
			if (num2 < 0f || num2 > found.Seconds)
			{
				return false;
			}
			progress = ((found.Seconds <= 0f) ? 1f : Mathf.Clamp01(num2 / found.Seconds));
			return true;
		}

		public static bool Apply(PlayerVoxelBody body, ulong actor, byte[] target, float progress)
		{
			if (body == null || target == null || target.Length == 0)
			{
				return false;
			}
			Plan plan = PlanFor(actor, target);
			if (plan == null || plan.Order.Length == 0)
			{
				return false;
			}
			int num = RevealCount(plan, progress);
			if (num == plan.ShownCount)
			{
				return false;
			}
			if (plan.ShownCount >= 0 && num != 0 && num < plan.Order.Length && Time.unscaledTime < plan.NextRebuildTime)
			{
				return false;
			}
			plan.ShownCount = num;
			plan.NextRebuildTime = Time.unscaledTime + 1f / 30f;
			Compose(plan, num);
			body.DevApplyBody(plan.Scratch);
			return true;
		}

		private static int RevealCount(Plan plan, float progress)
		{
			if (progress < 0.12f)
			{
				return 0;
			}
			float num = (progress - 0.12f) / 0.88f;
			int num2 = plan.StepEnd.Length;
			int num3 = Mathf.Clamp(Mathf.CeilToInt(num * (float)num2), 1, num2);
			return plan.StepEnd[num3 - 1];
		}

		private static void Compose(Plan plan, int count)
		{
			for (int i = 0; i < plan.Scratch.pieces.Count; i++)
			{
				plan.Scratch.pieces[i].grid.Clear();
			}
			for (int j = 0; j < count; j++)
			{
				Entry entry = plan.Order[j];
				if (plan.Target.pieces[entry.Piece].grid.TryGet(entry.Cell, out var data))
				{
					plan.Scratch.pieces[entry.Piece].grid.Set(entry.Cell, data);
				}
			}
			if (count < 27)
			{
				AddCube(plan, count);
			}
			for (int k = 0; k < plan.Target.pieces.Count; k++)
			{
				VoxelGrid grid = plan.Scratch.pieces[k].grid;
				foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in plan.Target.pieces[k].grid.FaceColors)
				{
					if (grid.Contains(faceColor.Key.Item1))
					{
						grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
					}
				}
			}
		}

		private static void AddCube(Plan plan, int shown)
		{
			VoxelGrid grid = plan.Scratch.pieces[plan.Order[0].Piece].grid;
			Vector3Int vector3Int = plan.Order[0].Cell - new Vector3Int(1, 1, 1);
			int num = 27 - shown;
			for (int i = 0; i < 3; i++)
			{
				if (num <= 0)
				{
					break;
				}
				for (int j = 0; j < 3; j++)
				{
					if (num <= 0)
					{
						break;
					}
					for (int k = 0; k < 3; k++)
					{
						if (num <= 0)
						{
							break;
						}
						Vector3Int position = new Vector3Int(vector3Int.x + i, vector3Int.y + j, vector3Int.z + k);
						if (!grid.Contains(position))
						{
							grid.Set(position, new VoxelData(CubeColor));
							num--;
						}
					}
				}
			}
		}

		private static Plan PlanFor(ulong actor, byte[] target)
		{
			if (plans.TryGetValue(actor, out var value) && value.Source == target)
			{
				return value;
			}
			if (!VoxelBodyCodec.TryDecode(target, out var body) || body.pieces.Count == 0)
			{
				plans.Remove(actor);
				return null;
			}
			BuildOrder(body, out var order, out var stepEnd);
			Plan plan = new Plan
			{
				Source = target,
				Target = body,
				Order = order,
				StepEnd = stepEnd,
				Scratch = Blank(body)
			};
			plans[actor] = plan;
			return plan;
		}

		private static VoxelBodyData Blank(VoxelBodyData target)
		{
			VoxelBodyData voxelBodyData = new VoxelBodyData
			{
				voxelSize = target.voxelSize
			};
			foreach (VoxelPieceData piece in target.pieces)
			{
				voxelBodyData.pieces.Add(new VoxelPieceData
				{
					grid = new VoxelGrid(),
					localPosition = piece.localPosition,
					localRotation = piece.localRotation
				});
			}
			return voxelBodyData;
		}

		private static void BuildOrder(VoxelBodyData body, out Entry[] order, out int[] stepEnd)
		{
			List<List<Vector3Int>>[] array = new List<List<Vector3Int>>[body.pieces.Count];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < body.pieces.Count; i++)
			{
				array[i] = OrderPiece(body.pieces[i].grid);
				num = Mathf.Max(num, array[i].Count);
				foreach (List<Vector3Int> item in array[i])
				{
					num2 += item.Count;
				}
			}
			int a = Mathf.Clamp(num, 5, 24);
			a = Mathf.Max(1, Mathf.Min(a, num2));
			List<Entry>[] array2 = new List<Entry>[a];
			for (int j = 0; j < a; j++)
			{
				array2[j] = new List<Entry>();
			}
			for (int k = 0; k < array.Length; k++)
			{
				List<List<Vector3Int>> list = array[k];
				if (list.Count == 0)
				{
					continue;
				}
				for (int l = 0; l < list.Count; l++)
				{
					int num3 = Mathf.Min(a - 1, l * a / list.Count);
					foreach (Vector3Int item2 in list[l])
					{
						array2[num3].Add(new Entry(k, item2));
					}
				}
			}
			order = new Entry[num2];
			List<int> list2 = new List<int>(a);
			int num4 = 0;
			List<Entry>[] array3 = array2;
			foreach (List<Entry> list3 in array3)
			{
				if (list3.Count == 0)
				{
					continue;
				}
				foreach (Entry item3 in list3)
				{
					order[num4++] = item3;
				}
				if (num4 >= 27 || num4 >= num2)
				{
					list2.Add(num4);
				}
			}
			if (list2.Count == 0)
			{
				list2.Add(0);
			}
			stepEnd = list2.ToArray();
		}

		private static List<List<Vector3Int>> OrderPiece(VoxelGrid grid)
		{
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
			foreach (Vector3Int position in grid.Positions)
			{
				hashSet.Add(position);
			}
			List<List<Vector3Int>> list = new List<List<Vector3Int>>();
			if (hashSet.Count == 0)
			{
				return list;
			}
			Vector3 to = Centre(grid);
			List<Vector3Int> list2 = new List<Vector3Int>();
			List<Vector3Int> list3 = new List<Vector3Int>();
			while (hashSet.Count > 0)
			{
				Vector3Int item = Nearest(hashSet, to);
				hashSet.Remove(item);
				list2.Clear();
				list2.Add(item);
				int num = 0;
				while (list2.Count > 0)
				{
					list3.Clear();
					foreach (Vector3Int item2 in list2)
					{
						Vector3Int[] neighbours = Neighbours;
						foreach (Vector3Int vector3Int in neighbours)
						{
							if (hashSet.Remove(item2 + vector3Int))
							{
								list3.Add(item2 + vector3Int);
							}
						}
					}
					if (num < list.Count)
					{
						list[num].AddRange(list2);
					}
					else
					{
						list.Add(new List<Vector3Int>(list2));
					}
					num++;
					List<Vector3Int> list4 = list3;
					List<Vector3Int> list5 = list2;
					list2 = list4;
					list3 = list5;
				}
			}
			return list;
		}

		private static Vector3 Centre(VoxelGrid grid)
		{
			if (!grid.TryGetBounds(out var min, out var max))
			{
				return Vector3.zero;
			}
			return new Vector3(min.x + max.x, min.y + max.y, min.z + max.z) * 0.5f;
		}

		private static Vector3Int Nearest(HashSet<Vector3Int> cells, Vector3 to)
		{
			Vector3Int result = default(Vector3Int);
			float num = float.MaxValue;
			foreach (Vector3Int cell in cells)
			{
				float sqrMagnitude = (new Vector3(cell.x, cell.y, cell.z) - to).sqrMagnitude;
				if (!(sqrMagnitude >= num))
				{
					num = sqrMagnitude;
					result = cell;
				}
			}
			return result;
		}
	}
}
