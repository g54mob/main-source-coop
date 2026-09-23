using System;
using System.Collections.Generic;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class PaintTool
	{
		private const float BrushRadiusScrollStep = 0.25f;

		private static readonly Color PaintPreviewAlpha = new Color(1f, 1f, 1f, 0.55f);

		private static readonly Color EyedropperPreviewColor = new Color(0.3f, 0.85f, 1f, 0.6f);

		private readonly VoxelModel model;

		private readonly PaintBrushPreview brushPreview;

		private readonly VoxelGridOverlay gridOverlay;

		private Vector3Int lastPaintedCenter;

		private bool hasPaintedOnce;

		private Vector3Int strokeStartVoxel;

		private Vector3Int strokeStartNormal;

		private Vector3Int strokeEndVoxel;

		private Dictionary<Vector3Int, VoxelData?> strokeBefore;

		private Dictionary<Vector3Int, VoxelData?> strokeAfter;

		private Dictionary<(Vector3Int Position, int Face), Color32?> strokeFacesBefore;

		private Dictionary<(Vector3Int Position, int Face), Color32?> strokeFacesAfter;

		private Vector3Int lastHoverVoxel;

		private Vector3Int lastHoverNormal;

		private PaintMode mode;

		private readonly List<(Vector3Int Position, int Face)> singleFace = new List<(Vector3Int, int)>();

		private static readonly Vector3Int[] FaceDirections = new Vector3Int[6]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 0, -1)
		};

		private static Color GridColor => EditorPalette.GridSoft;

		public bool ShowGridOverlay { get; set; } = true;

		public bool IsStroking
		{
			get
			{
				if (strokeBefore != null && Mouse.current != null)
				{
					if (!Mouse.current.leftButton.isPressed)
					{
						return Mouse.current.leftButton.wasReleasedThisFrame;
					}
					return true;
				}
				return false;
			}
		}

		public bool IsBoxDragging { get; private set; }

		public PaintMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				if (mode != value)
				{
					EndStroke();
					mode = value;
				}
			}
		}

		public bool IsEyedropperActive
		{
			get
			{
				if (!VoxelEditorSettings.EyedropperArmed)
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
				return true;
			}
		}

		public PaintTool(VoxelModel model, Transform highlightParent)
		{
			this.model = model;
			brushPreview = PaintBrushPreview.Create(highlightParent);
			gridOverlay = VoxelGridOverlay.Create(highlightParent);
		}

		public void ArmEyedropper()
		{
			VoxelEditorSettings.EyedropperArmed = true;
		}

		public void ConsumeEyedropperArm()
		{
			VoxelEditorSettings.EyedropperArmed = false;
		}

		public void Hide()
		{
			brushPreview.Hide();
			gridOverlay.Hide();
		}

		public (Color32? sampledColor, float? newBrushRadius) UpdateInput(bool hasHover, Vector3Int hoveredVoxel, Vector3Int hoveredFaceNormal, Color32 activeColor, float brushRadius, Texture2D patternTexture, float threshold)
		{
			if (Mouse.current == null)
			{
				EndStroke();
				Hide();
				return (sampledColor: null, newBrushRadius: null);
			}
			if (!hasHover)
			{
				bool isPressed = Mouse.current.leftButton.isPressed;
				bool wasReleasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
				if (mode != PaintMode.Brush || strokeBefore == null || !(isPressed || wasReleasedThisFrame))
				{
					EndStroke();
					Hide();
					return (sampledColor: null, newBrushRadius: null);
				}
				hoveredVoxel = lastHoverVoxel;
				hoveredFaceNormal = lastHoverNormal;
			}
			else
			{
				lastHoverVoxel = hoveredVoxel;
				lastHoverNormal = hoveredFaceNormal;
			}
			float? num = ((mode == PaintMode.Brush) ? TryAdjustBrushRadius(brushRadius) : ((float?)null));
			float effectiveRadius = num ?? brushRadius;
			if (mode == PaintMode.Brush)
			{
				return UpdateBrushStroke(hoveredVoxel, hoveredFaceNormal, activeColor, effectiveRadius, num);
			}
			bool isEyedropperActive = IsEyedropperActive;
			if (mode == PaintMode.Bucket)
			{
				List<(Vector3Int, int)> faces = ((Keyboard.current != null && (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)) ? ComputeBucketFacesOnPlane(model.Grid, hoveredVoxel, hoveredFaceNormal, threshold) : ComputeBucketFaces(model.Grid, hoveredVoxel, hoveredFaceNormal, threshold));
				brushPreview.ShowFaces(faces, isEyedropperActive ? EyedropperPreviewColor : PaintPreviewAlpha);
				if (ShowGridOverlay)
				{
					gridOverlay.ShowFaces(faces, GridColor);
				}
				else
				{
					gridOverlay.Hide();
				}
				if (!Mouse.current.leftButton.wasPressedThisFrame)
				{
					return (sampledColor: null, newBrushRadius: num);
				}
				if (isEyedropperActive)
				{
					if (!model.Grid.Contains(hoveredVoxel))
					{
						return (sampledColor: null, newBrushRadius: num);
					}
					ConsumeEyedropperArm();
					return (sampledColor: model.Grid.GetFaceColor(hoveredVoxel, FaceAxes.IndexOf(hoveredFaceNormal)), newBrushRadius: num);
				}
				PaintBucketFaces(faces, activeColor);
				return (sampledColor: null, newBrushRadius: num);
			}
			List<Vector3Int> list = ComputeBucketVoxelsOnPlane(model.Grid, hoveredVoxel, hoveredFaceNormal, threshold);
			brushPreview.Show(model.Grid, list, isEyedropperActive ? EyedropperPreviewColor : PaintPreviewAlpha);
			if (ShowGridOverlay)
			{
				gridOverlay.Show(model.Grid, list, GridColor);
			}
			else
			{
				gridOverlay.Hide();
			}
			if (isEyedropperActive)
			{
				if (Mouse.current.leftButton.wasPressedThisFrame && model.Grid.Contains(hoveredVoxel))
				{
					ConsumeEyedropperArm();
					return (sampledColor: model.Grid.GetFaceColor(hoveredVoxel, FaceAxes.IndexOf(hoveredFaceNormal)), newBrushRadius: num);
				}
				return (sampledColor: null, newBrushRadius: num);
			}
			if (Mouse.current.leftButton.wasPressedThisFrame)
			{
				PaintPattern(list, hoveredVoxel, hoveredFaceNormal, patternTexture, activeColor.a);
			}
			return (sampledColor: null, newBrushRadius: num);
		}

		private (Color32? sampledColor, float? newBrushRadius) UpdateBrushStroke(Vector3Int hoveredVoxel, Vector3Int hoveredFaceNormal, Color32 activeColor, float effectiveRadius, float? newRadius)
		{
			bool isPressed = Mouse.current.leftButton.isPressed;
			bool wasPressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
			bool wasReleasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
			if (IsEyedropperActive)
			{
				IsBoxDragging = false;
				if (strokeBefore != null)
				{
					EndStroke();
				}
				singleFace.Clear();
				int num = FaceAxes.IndexOf(hoveredFaceNormal);
				if (num >= 0 && model.Grid.Contains(hoveredVoxel) && HasExposedFace(model.Grid, hoveredVoxel, hoveredFaceNormal))
				{
					singleFace.Add((hoveredVoxel, num));
				}
				brushPreview.ShowFaces(singleFace, EyedropperPreviewColor);
				if (ShowGridOverlay)
				{
					gridOverlay.ShowFaces(singleFace, GridColor);
				}
				else
				{
					gridOverlay.Hide();
				}
				if (wasPressedThisFrame && model.Grid.Contains(hoveredVoxel))
				{
					ConsumeEyedropperArm();
					return (sampledColor: model.Grid.GetFaceColor(hoveredVoxel, FaceAxes.IndexOf(hoveredFaceNormal)), newBrushRadius: newRadius);
				}
				return (sampledColor: null, newBrushRadius: newRadius);
			}
			if (wasPressedThisFrame)
			{
				BeginStroke(hoveredVoxel, hoveredFaceNormal);
			}
			if (strokeBefore != null && OnStrokeFace(hoveredVoxel, hoveredFaceNormal))
			{
				strokeEndVoxel = hoveredVoxel;
			}
			bool flag = Keyboard.current != null && (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed);
			bool flag2 = Keyboard.current != null && (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed);
			IsBoxDragging = flag && strokeBefore != null;
			List<Vector3Int> voxels;
			Vector3Int faceNormal;
			if (IsBoxDragging)
			{
				voxels = ComputeBoxOutlineVoxels(model.Grid, strokeStartVoxel, strokeEndVoxel);
				faceNormal = strokeStartNormal;
			}
			else if (flag2 && strokeBefore != null)
			{
				voxels = ComputeLineVoxels(model.Grid, strokeStartVoxel, strokeEndVoxel, effectiveRadius);
				faceNormal = strokeStartNormal;
			}
			else
			{
				voxels = ComputeAffectedVoxels(model.Grid, hoveredVoxel, effectiveRadius);
				faceNormal = hoveredFaceNormal;
			}
			List<(Vector3Int, int)> faces = FacesFacing(model.Grid, voxels, faceNormal);
			brushPreview.ShowFaces(faces, PaintPreviewAlpha);
			if (ShowGridOverlay)
			{
				gridOverlay.ShowFaces(faces, GridColor);
			}
			else
			{
				gridOverlay.Hide();
			}
			if (IsBoxDragging || (flag2 && strokeBefore != null))
			{
				if (wasReleasedThisFrame)
				{
					Paint(faces, activeColor);
					EndStroke();
				}
				else if (!isPressed)
				{
					EndStroke();
				}
				return (sampledColor: null, newBrushRadius: newRadius);
			}
			if (!isPressed)
			{
				EndStroke();
				return (sampledColor: null, newBrushRadius: newRadius);
			}
			if (wasPressedThisFrame || !hasPaintedOnce || hoveredVoxel != lastPaintedCenter)
			{
				Paint(faces, activeColor);
				lastPaintedCenter = hoveredVoxel;
				hasPaintedOnce = true;
			}
			return (sampledColor: null, newBrushRadius: newRadius);
		}

		private static float? TryAdjustBrushRadius(float currentRadius)
		{
			if (Mouse.current == null || Keyboard.current == null)
			{
				return null;
			}
			if (!Keyboard.current.leftCtrlKey.isPressed && !Keyboard.current.rightCtrlKey.isPressed)
			{
				return null;
			}
			float y = Mouse.current.scroll.ReadValue().y;
			if (Mathf.Approximately(y, 0f))
			{
				return null;
			}
			return Mathf.Max(0f, currentRadius + Mathf.Sign(y) * 0.25f);
		}

		private bool OnStrokeFace(Vector3Int voxel, Vector3Int normal)
		{
			if (normal == strokeStartNormal)
			{
				return Along(voxel, normal) == Along(strokeStartVoxel, normal);
			}
			return false;
		}

		private static int Along(Vector3Int voxel, Vector3Int normal)
		{
			if (normal.x == 0)
			{
				if (normal.y == 0)
				{
					return voxel.z;
				}
				return voxel.y;
			}
			return voxel.x;
		}

		private void BeginStroke(Vector3Int startVoxel, Vector3Int startNormal)
		{
			strokeBefore = new Dictionary<Vector3Int, VoxelData?>();
			strokeAfter = new Dictionary<Vector3Int, VoxelData?>();
			strokeFacesBefore = new Dictionary<(Vector3Int, int), Color32?>();
			strokeFacesAfter = new Dictionary<(Vector3Int, int), Color32?>();
			strokeStartVoxel = startVoxel;
			strokeStartNormal = startNormal;
			strokeEndVoxel = startVoxel;
		}

		private void EndStroke()
		{
			if ((strokeBefore != null && strokeBefore.Count > 0) || (strokeFacesBefore != null && strokeFacesBefore.Count > 0))
			{
				UndoManager.Push(new VoxelDiffCommand(model, strokeBefore, strokeAfter, null, strokeFacesBefore, strokeFacesAfter));
			}
			strokeBefore = null;
			strokeAfter = null;
			strokeFacesBefore = null;
			strokeFacesAfter = null;
			hasPaintedOnce = false;
			IsBoxDragging = false;
		}

		private static List<(Vector3Int Position, int Face)> FacesFacing(VoxelGrid grid, List<Vector3Int> voxels, Vector3Int faceNormal)
		{
			List<(Vector3Int, int)> list = new List<(Vector3Int, int)>();
			int num = FaceAxes.IndexOf(faceNormal);
			if (num < 0)
			{
				return list;
			}
			Func<Vector3Int, int, bool> paintMask = VoxelEditorSettings.PaintMask;
			foreach (Vector3Int voxel in voxels)
			{
				if (grid.Contains(voxel) && HasExposedFace(grid, voxel, faceNormal) && (paintMask == null || paintMask(voxel, num)))
				{
					list.Add((voxel, num));
				}
			}
			return list;
		}

		private void Paint(List<(Vector3Int Position, int Face)> faces, Color32 color)
		{
			Func<Vector3Int, int, bool> mask = VoxelEditorSettings.PaintMask;
			if (mask != null)
			{
				faces = faces.FindAll(((Vector3Int Position, int Face) face) => mask(face.Position, face.Face));
			}
			if (faces.Count == 0)
			{
				return;
			}
			AudioLibrary.PlayPaintBrush();
			foreach (var (vector3Int, num) in faces)
			{
				if (model.Grid.TryGet(vector3Int, out var data))
				{
					(Vector3Int, int) key = (vector3Int, num);
					Color32? value;
					if (strokeFacesBefore == null)
					{
						value = (model.Grid.TryGetFaceColor(vector3Int, num, out var color2) ? new Color32?(color2) : ((Color32?)null));
					}
					else if (!strokeFacesBefore.TryGetValue(key, out value))
					{
						value = (model.Grid.TryGetFaceColor(vector3Int, num, out var color3) ? new Color32?(color3) : ((Color32?)null));
						strokeFacesBefore[key] = value;
					}
					Color32 existing = value ?? data.Color;
					model.Grid.SetFaceColor(vector3Int, num, BlendPaintColor(existing, color));
					if (strokeFacesAfter != null)
					{
						strokeFacesAfter[key] = (model.Grid.TryGetFaceColor(vector3Int, num, out var color4) ? new Color32?(color4) : ((Color32?)null));
					}
				}
			}
			model.RebuildMesh();
		}

		private void PaintBucketFaces(List<(Vector3Int Position, int Face)> faces, Color32 color)
		{
			if (faces.Count == 0)
			{
				return;
			}
			AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.paintBucketClip);
			Dictionary<(Vector3Int, int), Color32?> dictionary = new Dictionary<(Vector3Int, int), Color32?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary2 = new Dictionary<(Vector3Int, int), Color32?>();
			foreach (var (vector3Int, num) in faces)
			{
				if (model.Grid.TryGet(vector3Int, out var data))
				{
					(Vector3Int, int) key = (vector3Int, num);
					Color32 color4;
					Color32? color2 = (dictionary[key] = (model.Grid.TryGetFaceColor(vector3Int, num, out color4) ? new Color32?(color4) : ((Color32?)null)));
					model.Grid.SetFaceColor(vector3Int, num, BlendPaintColor(color2 ?? data.Color, color));
					dictionary2[key] = (model.Grid.TryGetFaceColor(vector3Int, num, out var color5) ? new Color32?(color5) : ((Color32?)null));
				}
			}
			model.RebuildMesh();
			UndoManager.Push(new VoxelDiffCommand(model, new Dictionary<Vector3Int, VoxelData?>(), new Dictionary<Vector3Int, VoxelData?>(), null, dictionary, dictionary2));
		}

		private void TakeFaceColors(List<Vector3Int> affected, out Dictionary<(Vector3Int Position, int Face), Color32?> facesBefore, out Dictionary<(Vector3Int Position, int Face), Color32?> facesAfter)
		{
			facesBefore = null;
			facesAfter = null;
			if (model.Grid.FaceColorCount == 0)
			{
				return;
			}
			foreach (Vector3Int item in affected)
			{
				for (int i = 0; i < 6; i++)
				{
					if (model.Grid.TryGetFaceColor(item, i, out var color))
					{
						if (facesBefore == null)
						{
							facesBefore = new Dictionary<(Vector3Int, int), Color32?>();
							facesAfter = new Dictionary<(Vector3Int, int), Color32?>();
						}
						facesBefore[(item, i)] = color;
						facesAfter[(item, i)] = null;
					}
				}
				model.Grid.ClearFaceColors(item);
			}
		}

		private static Color32 BlendPaintColor(Color32 existing, Color32 paint)
		{
			float t = (float)(int)paint.a / 255f;
			return new Color32((byte)Mathf.RoundToInt(Mathf.Lerp((int)existing.r, (int)paint.r, t)), (byte)Mathf.RoundToInt(Mathf.Lerp((int)existing.g, (int)paint.g, t)), (byte)Mathf.RoundToInt(Mathf.Lerp((int)existing.b, (int)paint.b, t)), byte.MaxValue);
		}

		private void PaintPattern(List<Vector3Int> affected, Vector3Int origin, Vector3Int faceNormal, Texture2D pattern, byte alpha)
		{
			if (affected.Count == 0 || pattern == null)
			{
				return;
			}
			AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.paintPatternClip);
			FaceAxes.GetBasis(faceNormal, out var right, out var up);
			int width = pattern.width;
			int height = pattern.height;
			Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			foreach (Vector3Int item in affected)
			{
				Vector3Int vector3Int = item - origin;
				int num = vector3Int.x * right.x + vector3Int.y * right.y + vector3Int.z * right.z;
				int num2 = vector3Int.x * up.x + vector3Int.y * up.y + vector3Int.z * up.z;
				int x = (num % width + width) % width;
				int num3 = (-num2 % height + height) % height;
				Color32 paint = pattern.GetPixel(x, height - 1 - num3);
				paint.a = alpha;
				model.Grid.TryGet(item, out var data);
				dictionary[item] = data;
				VoxelData voxelData = new VoxelData(BlendPaintColor(data.Color, paint));
				dictionary2[item] = voxelData;
				model.Grid.Set(item, voxelData);
			}
			TakeFaceColors(affected, out Dictionary<(Vector3Int, int), Color32?> facesBefore, out Dictionary<(Vector3Int, int), Color32?> facesAfter);
			model.RebuildMesh();
			UndoManager.Push(new VoxelDiffCommand(model, dictionary, dictionary2, null, facesBefore, facesAfter));
		}

		public static List<(Vector3Int Position, int Face)> ComputeBucketFaces(VoxelGrid grid, Vector3Int start, Vector3Int startNormal, float threshold)
		{
			List<(Vector3Int, int)> list = new List<(Vector3Int, int)>();
			int num = FaceAxes.IndexOf(startNormal);
			if (num < 0 || !grid.Contains(start) || !HasExposedFace(grid, start, startNormal))
			{
				return list;
			}
			Color32 faceColor = grid.GetFaceColor(start, num);
			HashSet<(Vector3Int, int)> hashSet = new HashSet<(Vector3Int, int)> { (start, num) };
			Queue<(Vector3Int, int)> queue = new Queue<(Vector3Int, int)>();
			queue.Enqueue((start, num));
			while (queue.Count > 0)
			{
				var (vector3Int, num2) = queue.Dequeue();
				list.Add((vector3Int, num2));
				Vector3Int vector3Int2 = FaceAxes.Normals[num2];
				Vector3Int[] faceDirections = FaceDirections;
				foreach (Vector3Int vector3Int3 in faceDirections)
				{
					if (!(vector3Int3 == vector3Int2) && !(vector3Int3 == -vector3Int2))
					{
						Vector3Int vector3Int4;
						Vector3Int normal;
						if (grid.Contains(vector3Int + vector3Int2 + vector3Int3))
						{
							vector3Int4 = vector3Int + vector3Int2 + vector3Int3;
							normal = -vector3Int3;
						}
						else if (grid.Contains(vector3Int + vector3Int3))
						{
							vector3Int4 = vector3Int + vector3Int3;
							normal = vector3Int2;
						}
						else
						{
							vector3Int4 = vector3Int;
							normal = vector3Int3;
						}
						int num3 = FaceAxes.IndexOf(normal);
						if (hashSet.Add((vector3Int4, num3)) && ColorsWithinThreshold(grid.GetFaceColor(vector3Int4, num3), faceColor, threshold))
						{
							queue.Enqueue((vector3Int4, num3));
						}
					}
				}
			}
			return list;
		}

		public static List<(Vector3Int Position, int Face)> ComputeBucketFacesOnPlane(VoxelGrid grid, Vector3Int start, Vector3Int startNormal, float threshold)
		{
			List<(Vector3Int, int)> list = new List<(Vector3Int, int)>();
			int num = FaceAxes.IndexOf(startNormal);
			if (num < 0)
			{
				return list;
			}
			foreach (Vector3Int item in ComputeBucketVoxelsOnPlane(grid, start, startNormal, threshold))
			{
				list.Add((item, num));
			}
			return list;
		}

		public static List<Vector3Int> ComputeBucketVoxelsOnPlane(VoxelGrid grid, Vector3Int start, Vector3Int faceNormal, float threshold)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			int num = FaceAxes.IndexOf(faceNormal);
			if (num < 0 || !grid.Contains(start) || !HasExposedFace(grid, start, faceNormal))
			{
				return list;
			}
			Color32 faceColor = grid.GetFaceColor(start, num);
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int> { start };
			Queue<Vector3Int> queue = new Queue<Vector3Int>();
			queue.Enqueue(start);
			while (queue.Count > 0)
			{
				Vector3Int vector3Int = queue.Dequeue();
				list.Add(vector3Int);
				Vector3Int[] faceDirections = FaceDirections;
				foreach (Vector3Int vector3Int2 in faceDirections)
				{
					if (!(vector3Int2 == faceNormal) && !(vector3Int2 == -faceNormal))
					{
						Vector3Int vector3Int3 = vector3Int + vector3Int2;
						if (!hashSet.Contains(vector3Int3) && grid.Contains(vector3Int3) && HasExposedFace(grid, vector3Int3, faceNormal) && ColorsWithinThreshold(grid.GetFaceColor(vector3Int3, num), faceColor, threshold))
						{
							hashSet.Add(vector3Int3);
							queue.Enqueue(vector3Int3);
						}
					}
				}
			}
			return list;
		}

		private static bool ColorsWithinThreshold(Color32 a, Color32 b, float threshold)
		{
			float num = a.r - b.r;
			float num2 = a.g - b.g;
			float num3 = a.b - b.b;
			return Mathf.Sqrt(num * num + num2 * num2 + num3 * num3) / 441.67294f <= threshold;
		}

		public static List<Vector3Int> ComputeAffectedVoxels(VoxelGrid grid, Vector3Int center, float radius)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			int num = Mathf.CeilToInt(radius);
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					for (int k = -num; k <= num; k++)
					{
						if (!(new Vector3(i, j, k).magnitude > radius + 0.0001f))
						{
							Vector3Int vector3Int = center + new Vector3Int(i, j, k);
							if (grid.Contains(vector3Int) && IsExternallyVisible(grid, vector3Int))
							{
								list.Add(vector3Int);
							}
						}
					}
				}
			}
			return list;
		}

		public static List<Vector3Int> ComputeLineVoxels(VoxelGrid grid, Vector3Int start, Vector3Int end, float radius)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
			foreach (Vector3Int item in ComputeLinePoints(start, end))
			{
				foreach (Vector3Int item2 in ComputeAffectedVoxels(grid, item, radius))
				{
					if (hashSet.Add(item2))
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}

		private static List<Vector3Int> ComputeLinePoints(Vector3Int start, Vector3Int end)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			int num = end.x - start.x;
			int num2 = end.y - start.y;
			int num3 = end.z - start.z;
			int num4 = Mathf.Abs(num);
			int num5 = Mathf.Abs(num2);
			int num6 = Mathf.Abs(num3);
			int num7 = ((num > 0) ? 1 : (-1));
			int num8 = ((num2 > 0) ? 1 : (-1));
			int num9 = ((num3 > 0) ? 1 : (-1));
			int num10 = start.x;
			int num11 = start.y;
			int num12 = start.z;
			if (num4 >= num5 && num4 >= num6)
			{
				int num13 = 2 * num5 - num4;
				int num14 = 2 * num6 - num4;
				for (int i = 0; i <= num4; i++)
				{
					list.Add(new Vector3Int(num10, num11, num12));
					if (num13 >= 0)
					{
						num11 += num8;
						num13 -= 2 * num4;
					}
					if (num14 >= 0)
					{
						num12 += num9;
						num14 -= 2 * num4;
					}
					num10 += num7;
					num13 += 2 * num5;
					num14 += 2 * num6;
				}
			}
			else if (num5 >= num4 && num5 >= num6)
			{
				int num15 = 2 * num4 - num5;
				int num16 = 2 * num6 - num5;
				for (int j = 0; j <= num5; j++)
				{
					list.Add(new Vector3Int(num10, num11, num12));
					if (num15 >= 0)
					{
						num10 += num7;
						num15 -= 2 * num5;
					}
					if (num16 >= 0)
					{
						num12 += num9;
						num16 -= 2 * num5;
					}
					num11 += num8;
					num15 += 2 * num4;
					num16 += 2 * num6;
				}
			}
			else
			{
				int num17 = 2 * num4 - num6;
				int num18 = 2 * num5 - num6;
				for (int k = 0; k <= num6; k++)
				{
					list.Add(new Vector3Int(num10, num11, num12));
					if (num17 >= 0)
					{
						num10 += num7;
						num17 -= 2 * num6;
					}
					if (num18 >= 0)
					{
						num11 += num8;
						num18 -= 2 * num6;
					}
					num12 += num9;
					num17 += 2 * num4;
					num18 += 2 * num5;
				}
			}
			return list;
		}

		public static List<Vector3Int> ComputeBoxOutlineVoxels(VoxelGrid grid, Vector3Int cornerA, Vector3Int cornerB)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			int num = Mathf.Min(cornerA.x, cornerB.x);
			int num2 = Mathf.Max(cornerA.x, cornerB.x);
			int num3 = Mathf.Min(cornerA.y, cornerB.y);
			int num4 = Mathf.Max(cornerA.y, cornerB.y);
			int num5 = Mathf.Min(cornerA.z, cornerB.z);
			int num6 = Mathf.Max(cornerA.z, cornerB.z);
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					for (int k = num5; k <= num6; k++)
					{
						if (i == num || i == num2 || j == num3 || j == num4 || k == num5 || k == num6)
						{
							Vector3Int vector3Int = new Vector3Int(i, j, k);
							if (grid.Contains(vector3Int) && IsExternallyVisible(grid, vector3Int))
							{
								list.Add(vector3Int);
							}
						}
					}
				}
			}
			return list;
		}

		private static bool HasExposedFace(VoxelGrid grid, Vector3Int pos, Vector3Int faceNormal)
		{
			return !grid.Contains(pos + faceNormal);
		}

		private static bool IsExternallyVisible(VoxelGrid grid, Vector3Int pos)
		{
			if (grid.Contains(pos + new Vector3Int(1, 0, 0)) && grid.Contains(pos + new Vector3Int(-1, 0, 0)) && grid.Contains(pos + new Vector3Int(0, 1, 0)) && grid.Contains(pos + new Vector3Int(0, -1, 0)) && grid.Contains(pos + new Vector3Int(0, 0, 1)))
			{
				return !grid.Contains(pos + new Vector3Int(0, 0, -1));
			}
			return true;
		}
	}
}
