using System;
using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class LoopCutTool
	{
		private const float MaxPickDistance = 100f;

		private static readonly Color GridColor = new Color(0.75f, 0.4f, 0.95f, 0.3f);

		private readonly VoxelModel model;

		private readonly Collider modelCollider;

		private readonly LoopCutHighlight highlight;

		private readonly VoxelGridOverlay gridOverlay;

		private readonly Action onCutPerformed;

		private bool hasHover;

		private int hoverAxis;

		private int hoverCoord;

		private readonly List<KeyValuePair<Vector3Int, VoxelData>> pendingSplit = new List<KeyValuePair<Vector3Int, VoxelData>>();

		private readonly Queue<Vector3Int> floodQueue = new Queue<Vector3Int>();

		private readonly HashSet<Vector3Int> floodSeen = new HashSet<Vector3Int>();

		private readonly HashSet<Vector2Int> seamCells = new HashSet<Vector2Int>();

		private bool splitAbovePlane;

		private Vector3Int cachedVoxel;

		private int cachedFaceAxis = -1;

		private int cachedGridCount = -1;

		private int cachedAxis;

		private int cachedCoord;

		private bool cachedValid;

		private static readonly Vector3Int[] Neighbours = new Vector3Int[6]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 0, -1)
		};

		public bool ShowGridOverlay { get; set; } = true;

		public LoopCutTool(VoxelModel model, Collider modelCollider, Transform highlightParent, Action onCutPerformed)
		{
			this.model = model;
			this.modelCollider = modelCollider;
			this.onCutPerformed = onCutPerformed;
			highlight = LoopCutHighlight.Create(highlightParent);
			gridOverlay = VoxelGridOverlay.Create(highlightParent);
		}

		public void UpdateInput(Camera cam)
		{
			if (ShowGridOverlay)
			{
				gridOverlay.ShowWholeGrid(model.Grid, GridColor);
			}
			else
			{
				gridOverlay.Hide();
			}
			if (cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				Clear();
				return;
			}
			Ray worldRay = cam.ScreenPointToRay(position);
			if (!FacePicker.TryPick(worldRay, model, modelCollider, 100f, out var voxelPosition, out var faceNormal, out var hitDistance))
			{
				Clear();
				return;
			}
			if (!GridBounds.TryCompute(model.Grid, out var min, out var max))
			{
				Clear();
				return;
			}
			Vector3 point = model.transform.InverseTransformPoint(worldRay.GetPoint(hitDistance));
			int num = ((faceNormal.x == 0) ? ((faceNormal.y != 0) ? 1 : 2) : 0);
			if (!cachedValid || voxelPosition != cachedVoxel || num != cachedFaceAxis || model.Grid.Count != cachedGridCount)
			{
				cachedVoxel = voxelPosition;
				cachedFaceAxis = num;
				cachedGridCount = model.Grid.Count;
				cachedValid = ChooseCut(voxelPosition, num, point, min, max, out cachedAxis, out cachedCoord);
			}
			if (!cachedValid)
			{
				Clear();
				return;
			}
			CollectSplitPiece(voxelPosition, cachedAxis, cachedCoord);
			hasHover = true;
			hoverAxis = cachedAxis;
			hoverCoord = cachedCoord;
			ShowHighlightForPendingSplit();
			if (Mouse.current.leftButton.wasPressedThisFrame)
			{
				PerformCut(cam);
			}
		}

		public void Clear()
		{
			hasHover = false;
			cachedValid = false;
			cachedFaceAxis = -1;
			highlight.Hide();
		}

		public void HideAll()
		{
			Clear();
			gridOverlay.Hide();
		}

		private bool ChooseCut(Vector3Int voxel, int faceAxis, Vector3 point, Vector3Int boundsMin, Vector3Int boundsMax, out int bestAxis, out int bestCoord)
		{
			bestAxis = 0;
			bestCoord = 0;
			int num = int.MaxValue;
			float num2 = float.MaxValue;
			bool result = false;
			for (int i = 0; i < 3; i++)
			{
				if (i == faceAxis)
				{
					continue;
				}
				for (int j = 0; j < 2; j++)
				{
					int num3 = voxel[i] + j;
					if (num3 <= boundsMin[i] || num3 > boundsMax[i])
					{
						continue;
					}
					CollectSplitPiece(voxel, i, num3);
					int count = pendingSplit.Count;
					if (count != 0 && count != model.Grid.Count)
					{
						float num4 = Mathf.Abs(point[i] - (float)num3);
						if (count <= num && (count != num || !(num4 >= num2)))
						{
							num = count;
							num2 = num4;
							bestAxis = i;
							bestCoord = num3;
							result = true;
						}
					}
				}
			}
			return result;
		}

		private void CollectSplitPiece(Vector3Int start, int axis, int coord)
		{
			pendingSplit.Clear();
			floodSeen.Clear();
			floodQueue.Clear();
			bool flag = (splitAbovePlane = start[axis] >= coord);
			floodQueue.Enqueue(start);
			floodSeen.Add(start);
			while (floodQueue.Count > 0)
			{
				Vector3Int vector3Int = floodQueue.Dequeue();
				if (!model.Grid.TryGet(vector3Int, out var data))
				{
					continue;
				}
				pendingSplit.Add(new KeyValuePair<Vector3Int, VoxelData>(vector3Int, data));
				Vector3Int[] neighbours = Neighbours;
				foreach (Vector3Int vector3Int2 in neighbours)
				{
					Vector3Int vector3Int3 = vector3Int + vector3Int2;
					if (vector3Int3[axis] >= coord == flag && floodSeen.Add(vector3Int3) && model.Grid.Contains(vector3Int3))
					{
						floodQueue.Enqueue(vector3Int3);
					}
				}
			}
		}

		private void ShowHighlightForPendingSplit()
		{
			int index = (hoverAxis + 1) % 3;
			int index2 = (hoverAxis + 2) % 3;
			int num = (splitAbovePlane ? hoverCoord : (hoverCoord - 1));
			seamCells.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> item in pendingSplit)
			{
				if (item.Key[hoverAxis] == num)
				{
					seamCells.Add(new Vector2Int(item.Key[index], item.Key[index2]));
				}
			}
			if (seamCells.Count == 0)
			{
				highlight.Hide();
			}
			else
			{
				highlight.Show(seamCells, hoverAxis, hoverCoord);
			}
		}

		private void PerformCut(Camera cam)
		{
			if (!hasHover)
			{
				return;
			}
			List<KeyValuePair<Vector3Int, VoxelData>> list = new List<KeyValuePair<Vector3Int, VoxelData>>(pendingSplit);
			foreach (KeyValuePair<Vector3Int, VoxelData> item in list)
			{
				if (!model.CanRemove(item.Key))
				{
					Clear();
					return;
				}
			}
			if (list.Count == 0 || list.Count == model.Grid.Count)
			{
				Clear();
				return;
			}
			if (VoxelEditorSettings.BodyRulesApply && VoxelFocusManager.GatherBody().Count >= VoxelEditorSettings.MaxPieces)
			{
				PieceCapGuard.Refuse();
				Clear();
				return;
			}
			if (VoxelEditorSettings.BodyRulesApply && !PieceCapGuard.SplitLeavesLargeEnoughPieces(model.Grid, list))
			{
				PieceCapGuard.RefuseTooSmall();
				Clear();
				return;
			}
			List<KeyValuePair<(Vector3Int, int), Color32>> faceColors = null;
			if (model.Grid.FaceColorCount > 0)
			{
				faceColors = new List<KeyValuePair<(Vector3Int, int), Color32>>(model.Grid.FaceColors);
			}
			foreach (KeyValuePair<Vector3Int, VoxelData> item2 in list)
			{
				model.Grid.Remove(item2.Key);
			}
			model.RebuildMesh();
			GameObject splitPiece = VoxelSplitFactory.CreateSplitPiece(model, list, cam, faceColors);
			Dictionary<Vector3Int, VoxelData> dictionary = new Dictionary<Vector3Int, VoxelData>();
			foreach (KeyValuePair<Vector3Int, VoxelData> item3 in list)
			{
				dictionary[item3.Key] = item3.Value;
			}
			Vector3Int extent;
			string description = (GridBounds.TryComputeExtent(dictionary.Keys, out extent) ? $"{extent.x}x{extent.y}x{extent.z} Loop Cut" : "Loop Cut");
			UndoManager.Push(new LoopCutCommand(model, splitPiece, dictionary, description));
			Clear();
			onCutPerformed?.Invoke();
		}
	}
}
