using System.Collections.Generic;
using Mimicraft.Settings;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class ExtrudeTool
	{
		private enum Phase
		{
			Idle = 0,
			BoxSelecting = 1,
			ReadyToExtrude = 2,
			Dragging = 3
		}

		private enum SelectMode
		{
			Replace = 0,
			Add = 1,
			Subtract = 2
		}

		private class Column
		{
			public Vector3Int Origin;

			public VoxelData OriginalData;

			public int AppliedSteps;

			public readonly List<VoxelData?> NegativeSnapshots = new List<VoxelData?>();

			public readonly List<VoxelData?> PositiveSnapshots = new List<VoxelData?>();

			public Color32? PulledFaceColor;

			public readonly Dictionary<(Vector3Int Position, int Face), Color32?> PulledSurfaceFaces = new Dictionary<(Vector3Int, int), Color32?>();

			public readonly List<Color32?[]> NegativeFaceSnapshots = new List<Color32?[]>();

			public readonly List<Color32?> PositiveFaceSnapshots = new List<Color32?>();
		}

		private struct LevelRect
		{
			public int W;

			public int U;

			public int V;

			public int SizeU;

			public int SizeV;
		}

		private static readonly Color SubtractGridColor = new Color(0.9f, 0.15f, 0.1f, 0.95f);

		private readonly VoxelModel model;

		private readonly SelectionHighlight selectionHighlight;

		private readonly ArrowGizmo arrowGizmo;

		private readonly VoxelGridOverlay gridOverlay;

		private readonly VoxelGridOverlay hoverOverlay;

		private Phase phase;

		private Vector3Int boxAnchorVoxel;

		private Vector3Int faceNormal;

		private List<Vector3Int> selectedVoxels = new List<Vector3Int>();

		private readonly List<Column> columns = new List<Column>();

		private Vector3 selectionCenterLocal;

		private float dragGrabOffset;

		private SelectMode currentSelectMode;

		private readonly HashSet<Vector3Int> committedVoxels = new HashSet<Vector3Int>();

		private bool hasCommittedNormal;

		private Vector3Int committedFaceNormal;

		private const float ArrowPickPixelThreshold = 14f;

		private bool startedInsidePlayArea = true;

		private List<int> smallAtStart;

		private readonly List<int> weakAtStart = new List<int>();

		private const int MaxLevelRects = 32;

		private readonly List<LevelRect> levelRects = new List<LevelRect>();

		private readonly Dictionary<int, HashSet<Vector2Int>> levelLayers = new Dictionary<int, HashSet<Vector2Int>>();

		private readonly List<Vector2Int> levelOrder = new List<Vector2Int>();

		private static Color GridColor => EditorPalette.Grid;

		public int CurrentSteps { get; private set; }

		public bool IsOverLimit { get; private set; }

		public bool IsActive => phase != Phase.Idle;

		public bool IsDragging
		{
			get
			{
				if (phase != Phase.BoxSelecting)
				{
					return phase == Phase.Dragging;
				}
				return true;
			}
		}

		public bool ShowGridOverlay { get; set; } = true;

		public ExtrudeMode Mode { get; set; }

		public ExtrudeSelectionMode SelectionMode { get; set; }

		public float GizmoSizeMultiplier { get; set; } = 1f;

		public float GizmoThicknessMultiplier { get; set; } = 1f;

		public float? GetArrowHoverScreenDistance(Camera cam)
		{
			if (phase != Phase.ReadyToExtrude || cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				return null;
			}
			Transform transform = model.transform;
			Vector3 vector = transform.TransformPoint(selectionCenterLocal);
			float num = 0.4f * GizmoSizeMultiplier;
			Vector3 position2 = vector + transform.TransformDirection(faceNormal).normalized * num;
			Vector2 a = cam.WorldToScreenPoint(vector);
			Vector2 b = cam.WorldToScreenPoint(position2);
			float num2 = DistancePointToSegment(position, a, b);
			if (!(num2 <= 14f))
			{
				return null;
			}
			return num2;
		}

		private static float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return Vector2.Distance(p, a);
			}
			float num = Mathf.Clamp01(Vector2.Dot(p - a, vector) / sqrMagnitude);
			return Vector2.Distance(p, a + vector * num);
		}

		public ExtrudeTool(VoxelModel model, Transform highlightParent)
		{
			this.model = model;
			selectionHighlight = SelectionHighlight.Create(highlightParent);
			arrowGizmo = ArrowGizmo.Create(highlightParent);
			gridOverlay = VoxelGridOverlay.Create(highlightParent);
			hoverOverlay = VoxelGridOverlay.Create(highlightParent);
		}

		public void UpdateInput(Camera cam, bool hasHover, Vector3Int hoveredVoxel, Vector3Int hoveredNormal)
		{
			if (cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				return;
			}
			UpdateGridOverlay(hasHover, hoveredVoxel, hoveredNormal);
			int num;
			int num2;
			bool flag;
			switch (phase)
			{
			case Phase.Idle:
				if (hasHover && Mouse.current.leftButton.wasPressedThisFrame)
				{
					BeginSelect(hoveredVoxel, hoveredNormal);
				}
				break;
			case Phase.BoxSelecting:
				UpdateBoxSelect(cam, position);
				if (Mouse.current.leftButton.wasReleasedThisFrame)
				{
					EndBoxSelect();
				}
				break;
			case Phase.ReadyToExtrude:
				if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
				{
					CancelSelection();
				}
				else if (Mouse.current.leftButton.wasPressedThisFrame)
				{
					if (hasHover && (IsShiftHeld() || IsAltHeld()))
					{
						BeginSelect(hoveredVoxel, hoveredNormal);
					}
					else
					{
						BeginDrag(cam, position);
					}
				}
				break;
			case Phase.Dragging:
				{
					UpdateDrag(cam, position);
					if (Keyboard.current != null)
					{
						num = (Keyboard.current.escapeKey.wasPressedThisFrame ? 1 : 0);
						if (num != 0)
						{
							num2 = 0;
							goto IL_0141;
						}
					}
					else
					{
						num = 0;
					}
					num2 = ((Mouse.current.leftButton.wasReleasedThisFrame || (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame))) ? 1 : 0);
					goto IL_0141;
				}
				IL_0141:
				flag = (byte)num2 != 0;
				if (num != 0)
				{
					CancelDrag(cam);
				}
				else if (flag)
				{
					CommitDrag(cam);
				}
				break;
			}
		}

		public void Cancel()
		{
			switch (phase)
			{
			case Phase.Dragging:
				CancelDrag();
				break;
			case Phase.BoxSelecting:
			case Phase.ReadyToExtrude:
				CancelSelection();
				break;
			}
			gridOverlay.Hide();
			hoverOverlay.Hide();
		}

		private void UpdateGridOverlay(bool hasHover, Vector3Int hoveredVoxel, Vector3Int hoveredNormal)
		{
			if (!ShowGridOverlay)
			{
				gridOverlay.Hide();
				hoverOverlay.Hide();
			}
			else if (phase == Phase.Idle)
			{
				hoverOverlay.Hide();
				if (!hasHover)
				{
					gridOverlay.Hide();
				}
				else
				{
					gridOverlay.Show(model.Grid, BuildHoverPreview(hoveredVoxel, hoveredNormal), GridColor);
				}
			}
			else if (phase == Phase.BoxSelecting)
			{
				hoverOverlay.Hide();
				Color color = ((currentSelectMode == SelectMode.Subtract) ? SubtractGridColor : GridColor);
				gridOverlay.Show(model.Grid, BuildPreview(), color);
			}
			else
			{
				gridOverlay.Show(model.Grid, committedVoxels, GridColor);
				if (hasHover && (IsShiftHeld() || IsAltHeld()))
				{
					hoverOverlay.Show(model.Grid, BuildHoverPreview(hoveredVoxel, hoveredNormal), IsAltHeld() ? SubtractGridColor : GridColor);
				}
				else
				{
					hoverOverlay.Hide();
				}
			}
		}

		private List<Vector3Int> BuildHoverPreview(Vector3Int hoveredVoxel, Vector3Int hoveredNormal)
		{
			if (SelectionMode != ExtrudeSelectionMode.Face)
			{
				return GetExistingNeighborhood(hoveredVoxel, GameSettings.EditorHoverRadius);
			}
			return PaintTool.ComputeBucketVoxelsOnPlane(model.Grid, hoveredVoxel, hoveredNormal, 1f);
		}

		private List<Vector3Int> GetExistingNeighborhood(Vector3Int center, int radius)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			for (int i = -radius; i <= radius; i++)
			{
				for (int j = -radius; j <= radius; j++)
				{
					for (int k = -radius; k <= radius; k++)
					{
						Vector3Int vector3Int = center + new Vector3Int(i, j, k);
						if (model.Grid.Contains(vector3Int))
						{
							list.Add(vector3Int);
						}
					}
				}
			}
			return list;
		}

		private static bool IsShiftHeld()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftShiftKey.isPressed)
				{
					return Keyboard.current.rightShiftKey.isPressed;
				}
				return true;
			}
			return false;
		}

		private static bool IsAltHeld()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftAltKey.isPressed)
				{
					return Keyboard.current.rightAltKey.isPressed;
				}
				return true;
			}
			return false;
		}

		private bool TryBeginSelectMode(Vector3Int normal)
		{
			bool flag = IsShiftHeld();
			bool flag2 = IsAltHeld();
			if ((flag || flag2) && hasCommittedNormal && normal != committedFaceNormal)
			{
				return false;
			}
			currentSelectMode = (flag ? SelectMode.Add : (flag2 ? SelectMode.Subtract : SelectMode.Replace));
			if (currentSelectMode == SelectMode.Replace)
			{
				committedVoxels.Clear();
				hasCommittedNormal = false;
			}
			return true;
		}

		private void BeginSelect(Vector3Int anchor, Vector3Int normal)
		{
			if (SelectionMode == ExtrudeSelectionMode.Face)
			{
				BeginFaceSelect(anchor, normal);
			}
			else
			{
				BeginBoxSelect(anchor, normal);
			}
		}

		private void BeginBoxSelect(Vector3Int anchor, Vector3Int normal)
		{
			if (TryBeginSelectMode(normal))
			{
				boxAnchorVoxel = anchor;
				faceNormal = normal;
				selectedVoxels = new List<Vector3Int> { anchor };
				selectionHighlight.Show(BuildPreview(), faceNormal);
				arrowGizmo.Hide();
				phase = Phase.BoxSelecting;
			}
		}

		private void BeginFaceSelect(Vector3Int anchor, Vector3Int normal)
		{
			if (TryBeginSelectMode(normal))
			{
				faceNormal = normal;
				selectedVoxels = PaintTool.ComputeBucketVoxelsOnPlane(model.Grid, anchor, normal, 1f);
				if (selectedVoxels.Count == 0)
				{
					selectedVoxels.Add(anchor);
				}
				EndBoxSelect();
			}
		}

		private void UpdateBoxSelect(Camera cam, Vector2 screenPos)
		{
			if (BoxSelector.TryGetPlaneCell(ToLocalRay(cam.ScreenPointToRay(screenPos)), boxAnchorVoxel, faceNormal, out var cell))
			{
				selectedVoxels = BoxSelector.Select(model.Grid, boxAnchorVoxel, faceNormal, cell);
				if (selectedVoxels.Count == 0)
				{
					selectedVoxels.Add(boxAnchorVoxel);
				}
				selectionHighlight.Show(BuildPreview(), faceNormal);
			}
		}

		private List<Vector3Int> BuildPreview()
		{
			if (currentSelectMode == SelectMode.Replace)
			{
				return selectedVoxels;
			}
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>(committedVoxels);
			if (currentSelectMode == SelectMode.Add)
			{
				foreach (Vector3Int selectedVoxel in selectedVoxels)
				{
					hashSet.Add(selectedVoxel);
				}
			}
			else
			{
				foreach (Vector3Int selectedVoxel2 in selectedVoxels)
				{
					hashSet.Remove(selectedVoxel2);
				}
			}
			return new List<Vector3Int>(hashSet);
		}

		private void EndBoxSelect()
		{
			switch (currentSelectMode)
			{
			case SelectMode.Add:
				foreach (Vector3Int selectedVoxel in selectedVoxels)
				{
					committedVoxels.Add(selectedVoxel);
				}
				break;
			case SelectMode.Subtract:
				foreach (Vector3Int selectedVoxel2 in selectedVoxels)
				{
					committedVoxels.Remove(selectedVoxel2);
				}
				break;
			default:
				committedVoxels.Clear();
				foreach (Vector3Int selectedVoxel3 in selectedVoxels)
				{
					committedVoxels.Add(selectedVoxel3);
				}
				break;
			}
			if (committedVoxels.Count == 0)
			{
				CancelSelection();
				return;
			}
			hasCommittedNormal = true;
			committedFaceNormal = faceNormal;
			columns.Clear();
			Vector3 zero = Vector3.zero;
			int num = FaceAxes.IndexOf(faceNormal);
			foreach (Vector3Int committedVoxel in committedVoxels)
			{
				model.Grid.TryGet(committedVoxel, out var data);
				columns.Add(new Column
				{
					Origin = committedVoxel,
					OriginalData = data,
					AppliedSteps = 0,
					PulledFaceColor = ((num >= 0 && model.Grid.TryGetFaceColor(committedVoxel, num, out var color)) ? new Color32?(color) : ((Color32?)null))
				});
				zero += (Vector3)committedVoxel;
			}
			selectionCenterLocal = zero / columns.Count + new Vector3(0.5f, 0.5f, 0.5f) + (Vector3)faceNormal * 0.5f;
			phase = Phase.ReadyToExtrude;
			arrowGizmo.Show(selectionCenterLocal, faceNormal, 0, GetArrowScaleCompensation(), GizmoSizeMultiplier, GizmoThicknessMultiplier);
		}

		private void BeginDrag(Camera cam, Vector2 screenPos)
		{
			CurrentSteps = 0;
			Ray ray = ToLocalRay(cam.ScreenPointToRay(screenPos));
			dragGrabOffset = (AxisProjection.TryGetDistanceAlongAxis(ray, selectionCenterLocal, faceNormal, out var distanceAlongAxis) ? distanceAlongAxis : 0f);
			phase = Phase.Dragging;
			startedInsidePlayArea = PlayAreaGuard.Allows(model.transform, model.Grid);
			smallAtStart = (VoxelEditorSettings.BodyRulesApply ? VoxelBodyRules.Evaluate(VoxelFocusManager.GatherBody()).SmallPieces : null);
			weakAtStart.Clear();
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				return;
			}
			foreach (VoxelBodyPiece item in VoxelFocusManager.GatherBody())
			{
				weakAtStart.Add(item.WeakIslands);
			}
		}

		private void UpdateDrag(Camera cam, Vector2 screenPos)
		{
			if (!AxisProjection.TryGetDistanceAlongAxis(ToLocalRay(cam.ScreenPointToRay(screenPos)), selectionCenterLocal, faceNormal, out var distanceAlongAxis))
			{
				return;
			}
			int num = ClampToLevel(ClampToNeighbourObjects(Mathf.RoundToInt(distanceAlongAxis - dragGrabOffset)));
			if (num != CurrentSteps)
			{
				int num2 = ApplyWithinBodyLimits(num);
				IsOverLimit = num2 != num;
				if (num2 == CurrentSteps)
				{
					arrowGizmo.Show(selectionCenterLocal, faceNormal, CurrentSteps, GetArrowScaleCompensation(), GizmoSizeMultiplier, GizmoThicknessMultiplier, IsOverLimit);
					return;
				}
				AudioLibrary.PlayExtrudeStep(num2 > CurrentSteps);
				CurrentSteps = num2;
				model.RebuildMesh(updateCollider: false);
				arrowGizmo.Show(selectionCenterLocal, faceNormal, CurrentSteps, GetArrowScaleCompensation(), GizmoSizeMultiplier, GizmoThicknessMultiplier, IsOverLimit);
			}
		}

		private int ApplyWithinBodyLimits(int wanted)
		{
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				Apply(wanted);
				return wanted;
			}
			int num = ((wanted <= CurrentSteps) ? 1 : (-1));
			for (int i = wanted; i != CurrentSteps; i += num)
			{
				Apply(i);
				if (!ExceedsBounds())
				{
					return i;
				}
			}
			Apply(CurrentSteps);
			return CurrentSteps;
			void Apply(int steps)
			{
				foreach (Column column in columns)
				{
					ApplyColumnSteps(column, steps);
				}
				PaintPulledSides();
			}
		}

		private int ClampToLevel(int steps)
		{
			if (steps <= 0 || steps <= CurrentSteps || columns.Count == 0 || !BodyClearance.Applies)
			{
				return steps;
			}
			int num = ((faceNormal.x == 0) ? ((faceNormal.y != 0) ? 1 : 2) : 0);
			int num2 = faceNormal[num];
			int num3 = ((num == 0) ? 1 : 0);
			int num4 = ((num == 2) ? 1 : 2);
			CollectLevelRects(num, num3, num4);
			Transform ignoreRoot = TransformTool.FindPlayerRoot(model.transform);
			Matrix4x4 localToWorldMatrix = model.transform.localToWorldMatrix;
			int num5 = steps;
			foreach (LevelRect levelRect in levelRects)
			{
				Vector3Int vector3Int = default(Vector3Int);
				Vector3Int vector3Int2 = default(Vector3Int);
				vector3Int[num] = levelRect.W + num2;
				vector3Int2[num] = 1;
				vector3Int[num3] = levelRect.U;
				vector3Int2[num3] = levelRect.SizeU;
				vector3Int[num4] = levelRect.V;
				vector3Int2[num4] = levelRect.SizeV;
				VoxelBox firstLayer = new VoxelBox
				{
					MinX = vector3Int.x,
					MinY = vector3Int.y,
					MinZ = vector3Int.z,
					SizeX = vector3Int2.x,
					SizeY = vector3Int2.y,
					SizeZ = vector3Int2.z
				};
				num5 = Mathf.Min(num5, BodyClearance.FittingLayers(firstLayer, faceNormal, num5, localToWorldMatrix, ignoreRoot));
				if (num5 <= CurrentSteps)
				{
					break;
				}
			}
			num5 = Mathf.Max(num5, CurrentSteps);
			if (num5 < steps)
			{
				BodyClearance.Refuse();
			}
			return num5;
		}

		private void CollectLevelRects(int axis, int uAxis, int vAxis)
		{
			levelRects.Clear();
			foreach (HashSet<Vector2Int> value3 in levelLayers.Values)
			{
				value3.Clear();
			}
			foreach (Column column in columns)
			{
				int key = column.Origin[axis];
				if (!levelLayers.TryGetValue(key, out var value))
				{
					value = (levelLayers[key] = new HashSet<Vector2Int>());
				}
				value.Add(new Vector2Int(column.Origin[uAxis], column.Origin[vAxis]));
			}
			foreach (KeyValuePair<int, HashSet<Vector2Int>> levelLayer in levelLayers)
			{
				HashSet<Vector2Int> value2 = levelLayer.Value;
				if (value2.Count == 0)
				{
					continue;
				}
				int count = levelRects.Count;
				levelOrder.Clear();
				levelOrder.AddRange(value2);
				levelOrder.Sort((Vector2Int a, Vector2Int b) => (a.y == b.y) ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));
				HashSet<Vector2Int> hashSet2 = new HashSet<Vector2Int>(value2);
				foreach (Vector2Int item in levelOrder)
				{
					if (!hashSet2.Contains(item))
					{
						continue;
					}
					int num;
					for (num = 1; hashSet2.Contains(new Vector2Int(item.x + num, item.y)); num++)
					{
					}
					int num2;
					for (num2 = 1; RowRemaining(hashSet2, item.x, item.y + num2, num); num2++)
					{
					}
					for (int num3 = 0; num3 < num2; num3++)
					{
						for (int num4 = 0; num4 < num; num4++)
						{
							hashSet2.Remove(new Vector2Int(item.x + num4, item.y + num3));
						}
					}
					levelRects.Add(new LevelRect
					{
						W = levelLayer.Key,
						U = item.x,
						V = item.y,
						SizeU = num,
						SizeV = num2
					});
				}
				if (levelRects.Count - count <= 32)
				{
					continue;
				}
				int num5 = int.MaxValue;
				int num6 = int.MaxValue;
				int num7 = int.MinValue;
				int num8 = int.MinValue;
				foreach (Vector2Int item2 in value2)
				{
					num5 = Mathf.Min(num5, item2.x);
					num6 = Mathf.Min(num6, item2.y);
					num7 = Mathf.Max(num7, item2.x);
					num8 = Mathf.Max(num8, item2.y);
				}
				levelRects.RemoveRange(count, levelRects.Count - count);
				levelRects.Add(new LevelRect
				{
					W = levelLayer.Key,
					U = num5,
					V = num6,
					SizeU = num7 - num5 + 1,
					SizeV = num8 - num6 + 1
				});
			}
		}

		private static bool RowRemaining(HashSet<Vector2Int> remaining, int u0, int v, int width)
		{
			for (int i = 0; i < width; i++)
			{
				if (!remaining.Contains(new Vector2Int(u0 + i, v)))
				{
					return false;
				}
			}
			return true;
		}

		private int ClampToNeighbourObjects(int steps)
		{
			if (steps <= 0 || !VoxelFocusManager.HasOtherUsableModel(model))
			{
				return steps;
			}
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				return steps;
			}
			int num = steps;
			foreach (Column column in columns)
			{
				Vector3Int origin = column.Origin;
				for (int i = 0; i < num; i++)
				{
					origin += faceNormal;
					if (VoxelFocusManager.IsCellOccupiedByOther(model, origin))
					{
						num = i;
						break;
					}
				}
				if (num == 0)
				{
					break;
				}
			}
			return num;
		}

		private bool ExceedsBounds()
		{
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				return false;
			}
			if (startedInsidePlayArea && !PlayAreaGuard.Allows(model.transform, model.Grid))
			{
				PlayAreaGuard.Refuse();
				return true;
			}
			List<VoxelBodyPiece> list = VoxelFocusManager.GatherBody();
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(list);
			if (!voxelBodyReport.HasVoxels || voxelBodyReport.BelowMin || voxelBodyReport.AboveMax)
			{
				return true;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = ((i < weakAtStart.Count) ? weakAtStart[i] : 0);
				if (list[i].WeakIslands > num)
				{
					PieceCapGuard.RefuseFragment();
					return true;
				}
			}
			if (voxelBodyReport.SmallPieces != null)
			{
				foreach (int smallPiece in voxelBodyReport.SmallPieces)
				{
					if (smallAtStart == null || !smallAtStart.Contains(smallPiece))
					{
						return true;
					}
				}
			}
			return false;
		}

		private float GetArrowScaleCompensation()
		{
			return 1f / Mathf.Max(model.VoxelSize, 0.0001f);
		}

		private void CommitDrag(Camera cam)
		{
			if (ExceedsBounds())
			{
				RevertToZero();
			}
			else if (Mode == ExtrudeMode.Create && CurrentSteps > 0)
			{
				CommitAsNewObject(cam);
			}
			else
			{
				PushUndoCommand();
				model.RefreshCollider();
			}
			phase = Phase.Idle;
			selectedVoxels.Clear();
			committedVoxels.Clear();
			hasCommittedNormal = false;
			columns.Clear();
			selectionHighlight.Hide();
			arrowGizmo.Hide();
		}

		private void CommitAsNewObject(Camera cam)
		{
			Dictionary<Vector3Int, VoxelData> dictionary = new Dictionary<Vector3Int, VoxelData>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary3 = null;
			int num = FaceAxes.IndexOf(faceNormal);
			foreach (Column column in columns)
			{
				Vector3Int origin = column.Origin;
				for (int i = 0; i < column.AppliedSteps; i++)
				{
					origin += faceNormal;
					dictionary[origin] = new VoxelData(column.OriginalData.Color);
					dictionary2[origin] = ((i < column.PositiveSnapshots.Count) ? column.PositiveSnapshots[i] : ((VoxelData?)null));
					Color32?[] array = CaptureFaceColors(origin);
					Color32? color = ((i < column.PositiveFaceSnapshots.Count) ? column.PositiveFaceSnapshots[i] : ((Color32?)null));
					if (array == null && !color.HasValue)
					{
						continue;
					}
					if (dictionary3 == null)
					{
						dictionary3 = new Dictionary<(Vector3Int, int), Color32?>();
					}
					for (int j = 0; j < 6; j++)
					{
						Color32? value = ((j == num) ? color : ((array != null) ? array[j] : ((Color32?)null)));
						if (value.HasValue)
						{
							dictionary3[(origin, j)] = value;
						}
					}
				}
			}
			if (dictionary.Count == 0)
			{
				PushUndoCommand();
				return;
			}
			if (VoxelEditorSettings.BodyRulesApply && VoxelFocusManager.GatherBody().Count >= VoxelEditorSettings.MaxPieces)
			{
				PieceCapGuard.Refuse();
				PushUndoCommand();
				return;
			}
			if (VoxelEditorSettings.BodyRulesApply && !PieceCapGuard.SplitLeavesLargeEnoughPieces(model.Grid, dictionary))
			{
				PieceCapGuard.RefuseTooSmall();
				PushUndoCommand();
				return;
			}
			List<KeyValuePair<(Vector3Int, int), Color32>> faceColors = null;
			if (model.Grid.FaceColorCount > 0)
			{
				faceColors = new List<KeyValuePair<(Vector3Int, int), Color32>>(model.Grid.FaceColors);
			}
			foreach (Vector3Int key in dictionary.Keys)
			{
				model.Grid.Remove(key);
			}
			model.RebuildMesh();
			GameObject splitPiece = VoxelSplitFactory.CreateSplitPiece(model, dictionary, cam, faceColors);
			Vector3Int extent;
			string description = (GridBounds.TryComputeExtent(dictionary.Keys, out extent) ? $"{extent.x}x{extent.y}x{extent.z} Create" : "Create");
			UndoManager.Push(new ExtrudeCreateCommand(model, splitPiece, dictionary, dictionary2, description, dictionary3));
		}

		private void PushUndoCommand()
		{
			Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<(Vector3Int, int), Color32?> before = null;
			Dictionary<(Vector3Int, int), Color32?> after = null;
			int num = FaceAxes.IndexOf(faceNormal);
			foreach (Column column in columns)
			{
				if (column.AppliedSteps > 0)
				{
					Vector3Int origin = column.Origin;
					for (int i = 0; i < column.AppliedSteps; i++)
					{
						origin += faceNormal;
						dictionary[origin] = ((i < column.PositiveSnapshots.Count) ? column.PositiveSnapshots[i] : ((VoxelData?)null));
						dictionary2[origin] = new VoxelData(column.OriginalData.Color);
						Color32? value = ((i < column.PositiveFaceSnapshots.Count) ? column.PositiveFaceSnapshots[i] : ((Color32?)null));
						bool flag = i == column.AppliedSteps - 1 && column.PulledFaceColor.HasValue;
						if (num >= 0 && (value.HasValue || flag))
						{
							EnsureFaceDiffs(ref before, ref after);
							before[(origin, num)] = value;
							after[(origin, num)] = (flag ? column.PulledFaceColor : ((Color32?)null));
						}
					}
				}
				else if (column.AppliedSteps < 0)
				{
					Vector3Int origin2 = column.Origin;
					int num2 = -column.AppliedSteps;
					for (int j = 0; j < num2; j++)
					{
						dictionary[origin2] = ((j < column.NegativeSnapshots.Count) ? column.NegativeSnapshots[j] : ((VoxelData?)null));
						dictionary2[origin2] = null;
						Color32?[] array = ((j < column.NegativeFaceSnapshots.Count) ? column.NegativeFaceSnapshots[j] : null);
						if (array != null)
						{
							EnsureFaceDiffs(ref before, ref after);
							for (int k = 0; k < 6; k++)
							{
								if (array[k].HasValue)
								{
									before[(origin2, k)] = array[k];
									after[(origin2, k)] = null;
								}
							}
						}
						origin2 -= faceNormal;
					}
				}
				foreach (KeyValuePair<(Vector3Int, int), Color32?> pulledSurfaceFace in column.PulledSurfaceFaces)
				{
					if (model.Grid.Contains(pulledSurfaceFace.Key.Item1))
					{
						EnsureFaceDiffs(ref before, ref after);
						before[pulledSurfaceFace.Key] = pulledSurfaceFace.Value;
						after[pulledSurfaceFace.Key] = column.PulledFaceColor;
					}
				}
			}
			if (dictionary.Count > 0)
			{
				Vector3Int extent;
				string description = (GridBounds.TryComputeExtent(dictionary2.Keys, out extent) ? $"{extent.x}x{extent.y}x{extent.z} Extrude" : "Extrude");
				UndoManager.Push(new VoxelDiffCommand(model, dictionary, dictionary2, description, before, after));
			}
		}

		private static void EnsureFaceDiffs(ref Dictionary<(Vector3Int Position, int Face), Color32?> before, ref Dictionary<(Vector3Int Position, int Face), Color32?> after)
		{
			if (before == null)
			{
				before = new Dictionary<(Vector3Int, int), Color32?>();
				after = new Dictionary<(Vector3Int, int), Color32?>();
			}
		}

		private void CancelDrag(Camera cam = null)
		{
			RevertToZero();
			CommitDrag(cam);
		}

		private void RevertToZero()
		{
			foreach (Column column in columns)
			{
				ApplyColumnSteps(column, 0);
			}
			model.RebuildMesh();
			CurrentSteps = 0;
			IsOverLimit = false;
		}

		private void CancelSelection()
		{
			phase = Phase.Idle;
			selectedVoxels.Clear();
			committedVoxels.Clear();
			hasCommittedNormal = false;
			columns.Clear();
			selectionHighlight.Hide();
			arrowGizmo.Hide();
		}

		private void ApplyColumnSteps(Column column, int targetSteps)
		{
			ResetColumnToZero(column);
			int num = FaceAxes.IndexOf(faceNormal);
			int num2 = 0;
			if (targetSteps > 0)
			{
				Vector3Int origin = column.Origin;
				Vector3Int position = column.Origin;
				for (int i = 0; i < targetSteps; i++)
				{
					origin += faceNormal;
					if (!model.CanAdd(origin))
					{
						break;
					}
					if (i >= column.PositiveSnapshots.Count)
					{
						VoxelData data;
						bool flag = model.Grid.TryGet(origin, out data);
						column.PositiveSnapshots.Add(flag ? new VoxelData?(data) : ((VoxelData?)null));
						column.PositiveFaceSnapshots.Add((flag && num >= 0 && model.Grid.TryGetFaceColor(origin, num, out var color)) ? new Color32?(color) : ((Color32?)null));
					}
					model.Grid.Set(origin, new VoxelData(column.OriginalData.Color));
					position = origin;
					num2 = i + 1;
				}
				if (num2 > 0 && num >= 0 && column.PulledFaceColor.HasValue)
				{
					model.Grid.SetFaceColor(position, num, column.PulledFaceColor.Value);
				}
			}
			else if (targetSteps < 0)
			{
				Vector3Int origin2 = column.Origin;
				int num3 = -targetSteps;
				for (int j = 0; j < num3; j++)
				{
					if (!model.CanRemove(origin2))
					{
						break;
					}
					if (j >= column.NegativeSnapshots.Count)
					{
						VoxelData data2;
						bool flag2 = model.Grid.TryGet(origin2, out data2);
						column.NegativeSnapshots.Add(flag2 ? new VoxelData?(data2) : ((VoxelData?)null));
						column.NegativeFaceSnapshots.Add(flag2 ? CaptureFaceColors(origin2) : null);
					}
					model.Grid.Remove(origin2);
					origin2 -= faceNormal;
					num2 = j + 1;
				}
				if (column.PulledFaceColor.HasValue)
				{
					PaintPulledSurface(column, origin2, num);
					Vector3Int origin3 = column.Origin;
					for (int k = 0; k < num2; k++)
					{
						Vector3Int[] normals = FaceAxes.Normals;
						foreach (Vector3Int vector3Int in normals)
						{
							if (!(vector3Int == faceNormal) && !(vector3Int == -faceNormal))
							{
								Vector3Int vector3Int2 = origin3 + vector3Int;
								if (model.Grid.Contains(vector3Int2))
								{
									PaintPulledSurface(column, vector3Int2, FaceAxes.IndexOf(-vector3Int));
								}
							}
						}
						origin3 -= faceNormal;
					}
				}
				num2 = -num2;
			}
			column.AppliedSteps = num2;
		}

		private void PaintPulledSides()
		{
			if (FaceAxes.IndexOf(faceNormal) < 0)
			{
				return;
			}
			foreach (Column column in columns)
			{
				if (column.AppliedSteps <= 0 || !column.PulledFaceColor.HasValue)
				{
					continue;
				}
				Vector3Int origin = column.Origin;
				for (int i = 0; i < column.AppliedSteps; i++)
				{
					origin += faceNormal;
					Vector3Int[] normals = FaceAxes.Normals;
					foreach (Vector3Int vector3Int in normals)
					{
						if (!(vector3Int == faceNormal) && !(vector3Int == -faceNormal) && !model.Grid.Contains(origin + vector3Int))
						{
							PaintPulledSurface(column, origin, FaceAxes.IndexOf(vector3Int));
						}
					}
				}
			}
		}

		private void PaintPulledSurface(Column column, Vector3Int cell, int face)
		{
			if (face >= 0 && column.PulledFaceColor.HasValue && model.Grid.Contains(cell))
			{
				(Vector3Int, int) key = (cell, face);
				if (!column.PulledSurfaceFaces.ContainsKey(key))
				{
					column.PulledSurfaceFaces[key] = (model.Grid.TryGetFaceColor(cell, face, out var color) ? new Color32?(color) : ((Color32?)null));
				}
				model.Grid.SetFaceColor(cell, face, column.PulledFaceColor.Value);
			}
		}

		private Color32?[] CaptureFaceColors(Vector3Int pos)
		{
			Color32?[] array = null;
			for (int i = 0; i < 6; i++)
			{
				if (model.Grid.TryGetFaceColor(pos, i, out var color))
				{
					if (array == null)
					{
						array = new Color32?[6];
					}
					array[i] = color;
				}
			}
			return array;
		}

		private void RestoreFaceColors(Vector3Int pos, Color32?[] faces)
		{
			if (faces == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				if (faces[i].HasValue)
				{
					model.Grid.SetFaceColor(pos, i, faces[i].Value);
				}
			}
		}

		private void ResetColumnToZero(Column column)
		{
			int num = FaceAxes.IndexOf(faceNormal);
			foreach (KeyValuePair<(Vector3Int, int), Color32?> pulledSurfaceFace in column.PulledSurfaceFaces)
			{
				if (model.Grid.Contains(pulledSurfaceFace.Key.Item1))
				{
					if (pulledSurfaceFace.Value.HasValue)
					{
						model.Grid.SetFaceColor(pulledSurfaceFace.Key.Item1, pulledSurfaceFace.Key.Item2, pulledSurfaceFace.Value.Value);
					}
					else
					{
						model.Grid.ClearFaceColor(pulledSurfaceFace.Key.Item1, pulledSurfaceFace.Key.Item2);
					}
				}
			}
			column.PulledSurfaceFaces.Clear();
			if (column.AppliedSteps > 0)
			{
				Vector3Int origin = column.Origin;
				for (int i = 0; i < column.AppliedSteps; i++)
				{
					origin += faceNormal;
					VoxelData? voxelData = ((i < column.PositiveSnapshots.Count) ? column.PositiveSnapshots[i] : ((VoxelData?)null));
					if (voxelData.HasValue)
					{
						model.Grid.Set(origin, voxelData.Value);
						Color32? color = ((i < column.PositiveFaceSnapshots.Count) ? column.PositiveFaceSnapshots[i] : ((Color32?)null));
						if (num >= 0)
						{
							if (color.HasValue)
							{
								model.Grid.SetFaceColor(origin, num, color.Value);
							}
							else
							{
								model.Grid.ClearFaceColor(origin, num);
							}
						}
					}
					else
					{
						model.Grid.Remove(origin);
					}
				}
			}
			else if (column.AppliedSteps < 0)
			{
				Vector3Int origin2 = column.Origin;
				int num2 = -column.AppliedSteps;
				for (int j = 0; j < num2; j++)
				{
					VoxelData? voxelData2 = ((j < column.NegativeSnapshots.Count) ? column.NegativeSnapshots[j] : ((VoxelData?)null));
					if (voxelData2.HasValue)
					{
						model.Grid.Set(origin2, voxelData2.Value);
						RestoreFaceColors(origin2, (j < column.NegativeFaceSnapshots.Count) ? column.NegativeFaceSnapshots[j] : null);
					}
					else
					{
						model.Grid.Remove(origin2);
					}
					origin2 -= faceNormal;
				}
			}
			column.AppliedSteps = 0;
		}

		private Ray ToLocalRay(Ray worldRay)
		{
			Transform transform = model.transform;
			Vector3 origin = transform.InverseTransformPoint(worldRay.origin);
			Vector3 direction = transform.InverseTransformDirection(worldRay.direction);
			return new Ray(origin, direction);
		}
	}
}
