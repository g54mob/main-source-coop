using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class BevelTool
	{
		private enum Phase
		{
			Idle = 0,
			Dragging = 1
		}

		private readonly struct Handle
		{
			public readonly bool IsCorner;

			public readonly BevelEdge Edge;

			public readonly BevelCorner Corner;

			public readonly Vector3 LocalA;

			public readonly Vector3 LocalB;

			public Vector3Int Signs
			{
				get
				{
					if (!IsCorner)
					{
						return Edge.Signs;
					}
					return Corner.Signs;
				}
			}

			public int EdgeAxis
			{
				get
				{
					if (!IsCorner)
					{
						return Edge.Axis;
					}
					return -1;
				}
			}

			public Vector3 LocalCentre => (LocalA + LocalB) * 0.5f;

			public Handle(in BevelEdge edge)
			{
				IsCorner = false;
				Edge = edge;
				Corner = default(BevelCorner);
				edge.GetLine(out LocalA, out LocalB);
			}

			public Handle(in BevelCorner corner)
			{
				IsCorner = true;
				Edge = default(BevelEdge);
				Corner = corner;
				LocalA = corner.Point;
				LocalB = corner.Point;
			}
		}

		private const float PickPixelThreshold = 14f;

		private const float CornerPickBonus = 6f;

		private const float ArrowLength = 1.6f;

		private readonly VoxelModel model;

		private readonly Transform target;

		private readonly LineHighlight edgeHighlight;

		private readonly ArrowGizmo arrowGizmo;

		private readonly List<Handle> handles = new List<Handle>(64);

		private readonly List<BevelEdge> foundEdges = new List<BevelEdge>(64);

		private readonly List<BevelCorner> foundCorners = new List<BevelCorner>(32);

		private int handlesVersion = -1;

		private readonly Dictionary<Vector3Int, VoxelData?> taken = new Dictionary<Vector3Int, VoxelData?>();

		private Phase phase;

		private int hovered = -1;

		private Handle grabbed;

		private float grabOffset;

		private MeshCollider occluder;

		private bool occluderResolved;

		public BevelProfile Profile { get; set; } = BevelProfile.Rounded;

		public float GizmoSizeMultiplier { get; set; } = 1f;

		public float GizmoThicknessMultiplier { get; set; } = 1f;

		public int CurrentRadius { get; private set; }

		public bool IsActive => phase != Phase.Idle;

		public bool IsDragging => phase == Phase.Dragging;

		public bool IsHoveringHandle => hovered >= 0;

		public BevelTool(VoxelModel model, Transform highlightParent)
		{
			this.model = model;
			target = model.transform;
			edgeHighlight = LineHighlight.Create(highlightParent, loop: false, 0.03f);
			arrowGizmo = ArrowGizmo.Create(highlightParent);
		}

		public void UpdateInput(Camera cam)
		{
			if (!(cam == null) && PointerScreenPosition.TryGet(out var position))
			{
				if (phase == Phase.Dragging)
				{
					UpdateDrag(cam, position);
				}
				else
				{
					UpdateHover(cam, position);
				}
			}
		}

		private void UpdateHover(Camera cam, Vector2 screenPos)
		{
			RebuildHandles();
			hovered = Pick(cam, screenPos);
			if (hovered < 0)
			{
				edgeHighlight.Hide();
				arrowGizmo.Hide();
				return;
			}
			ShowHandle(handles[hovered], EditorPalette.WithAlpha(EditorPalette.TransformGizmo, 0.9f));
			if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
			{
				grabbed = handles[hovered];
				Ray ray = ToLocalRay(cam.ScreenPointToRay(screenPos));
				grabOffset = (AxisProjection.TryGetDistanceAlongAxis(ray, grabbed.LocalCentre, BevelGeometry.OutwardDirection(grabbed.Signs, grabbed.EdgeAxis), out var distanceAlongAxis) ? distanceAlongAxis : 0f);
				CurrentRadius = 0;
				taken.Clear();
				phase = Phase.Dragging;
			}
		}

		private void UpdateDrag(Camera cam, Vector2 screenPos)
		{
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				Cancel();
				return;
			}
			if (AxisProjection.TryGetDistanceAlongAxis(ToLocalRay(cam.ScreenPointToRay(screenPos)), axisDir: BevelGeometry.OutwardDirection(grabbed.Signs, grabbed.EdgeAxis), axisPoint: grabbed.LocalCentre, distanceAlongAxis: out var distanceAlongAxis))
			{
				int num = Mathf.Max(0, Mathf.RoundToInt(distanceAlongAxis - grabOffset));
				if (num != CurrentRadius)
				{
					ApplyRadius(num);
				}
			}
			ShowHandle(in grabbed, EditorPalette.TransformGizmo);
			if ((Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame) || (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame))
			{
				Commit();
			}
		}

		private void ApplyRadius(int radius)
		{
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				Cut(radius);
				return;
			}
			int num = ((radius <= CurrentRadius) ? 1 : (-1));
			for (int i = radius; i != CurrentRadius; i += num)
			{
				Cut(i);
				if (!ExceedsBounds())
				{
					return;
				}
			}
			Cut(CurrentRadius);
		}

		private void Cut(int radius)
		{
			Restore();
			CurrentRadius = radius;
			if (radius > 0)
			{
				foreach (Vector3Int item in grabbed.IsCorner ? BevelGeometry.Compute(model.Grid, in grabbed.Corner, radius, Profile) : BevelGeometry.Compute(model.Grid, in grabbed.Edge, radius, Profile))
				{
					if (model.Grid.TryGet(item, out var data))
					{
						taken[item] = data;
						model.Grid.Remove(item);
					}
				}
			}
			model.RebuildMesh();
		}

		private bool ExceedsBounds()
		{
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				return false;
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(VoxelFocusManager.GatherBody());
			if (voxelBodyReport.HasVoxels && !voxelBodyReport.BelowMin)
			{
				return voxelBodyReport.AboveMax;
			}
			return true;
		}

		private void Restore()
		{
			if (taken.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<Vector3Int, VoxelData?> item in taken)
			{
				if (item.Value.HasValue)
				{
					model.Grid.Set(item.Key, item.Value.Value);
				}
			}
			taken.Clear();
		}

		private void Commit()
		{
			if (taken.Count > 0)
			{
				Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>(taken.Count);
				Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>(taken.Count);
				foreach (KeyValuePair<Vector3Int, VoxelData?> item in taken)
				{
					dictionary[item.Key] = item.Value;
					dictionary2[item.Key] = null;
				}
				string arg = ((Profile == BevelProfile.Rounded) ? "Round" : "Bevel");
				string arg2 = (grabbed.IsCorner ? "corner" : "edge");
				UndoManager.Push(new VoxelDiffCommand(model, dictionary, dictionary2, $"{arg} {arg2} {CurrentRadius}"));
			}
			taken.Clear();
			Finish();
		}

		public void Cancel()
		{
			if (taken.Count > 0)
			{
				Restore();
				model.RebuildMesh();
			}
			Finish();
		}

		private void Finish()
		{
			phase = Phase.Idle;
			hovered = -1;
			CurrentRadius = 0;
			edgeHighlight.Hide();
			arrowGizmo.Hide();
		}

		public void Hide()
		{
			if (phase == Phase.Idle)
			{
				edgeHighlight.Hide();
				arrowGizmo.Hide();
				hovered = -1;
			}
		}

		private void ShowHandle(in Handle handle, Color color)
		{
			if (handle.IsCorner)
			{
				edgeHighlight.Hide();
			}
			else
			{
				edgeHighlight.Show(new Vector3[2]
				{
					target.TransformPoint(handle.LocalA),
					target.TransformPoint(handle.LocalB)
				}, color);
			}
			Vector3 directionLocal = BevelGeometry.OutwardDirection(handle.Signs, handle.EdgeAxis);
			float length = 1.6f * GizmoSizeMultiplier + (float)CurrentRadius;
			arrowGizmo.ShowCustom(handle.LocalCentre, directionLocal, length, color, ArrowScaleCompensation(), GizmoThicknessMultiplier);
		}

		private void RebuildHandles()
		{
			if (handlesVersion == model.Grid.Version)
			{
				return;
			}
			handlesVersion = model.Grid.Version;
			handles.Clear();
			BevelGeometry.FindHandles(model.Grid, foundEdges, foundCorners);
			foreach (BevelCorner foundCorner in foundCorners)
			{
				BevelCorner corner = foundCorner;
				handles.Add(new Handle(in corner));
			}
			foreach (BevelEdge foundEdge in foundEdges)
			{
				BevelEdge edge = foundEdge;
				handles.Add(new Handle(in edge));
			}
		}

		private int Pick(Camera cam, Vector2 screenPos)
		{
			int result = -1;
			float num = 14f;
			for (int i = 0; i < handles.Count; i++)
			{
				Vector3 vector = target.TransformPoint(handles[i].LocalA);
				Vector3 vector2 = target.TransformPoint(handles[i].LocalB);
				Vector3 vector3 = cam.WorldToScreenPoint(vector);
				Vector3 vector4 = cam.WorldToScreenPoint(vector2);
				if (!(vector3.z <= 0f) || !(vector4.z <= 0f))
				{
					float t = ClosestParameter(screenPos, vector3, vector4);
					float num2 = Vector2.Distance(screenPos, Vector2.Lerp(vector3, vector4, t));
					if (handles[i].IsCorner)
					{
						num2 -= 6f;
					}
					if (!(num2 >= num) && !IsOccluded(cam, Vector3.Lerp(vector, vector2, t)))
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		private bool IsOccluded(Camera cam, Vector3 worldPoint)
		{
			if (!occluderResolved)
			{
				occluderResolved = true;
				occluder = model.GetComponent<MeshCollider>();
			}
			if (occluder == null || !occluder.enabled)
			{
				return false;
			}
			Vector3 position = cam.transform.position;
			Vector3 vector = worldPoint - position;
			float magnitude = vector.magnitude;
			float num = model.VoxelSize * 0.6f;
			if (magnitude <= num)
			{
				return false;
			}
			RaycastHit hitInfo;
			return occluder.Raycast(new Ray(position, vector / magnitude), out hitInfo, magnitude - num);
		}

		private float ArrowScaleCompensation()
		{
			return 1f / Mathf.Max(model.VoxelSize, 0.0001f);
		}

		private static float ClosestParameter(Vector2 p, Vector2 a, Vector2 b)
		{
			Vector2 rhs = b - a;
			float sqrMagnitude = rhs.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return 0f;
			}
			return Mathf.Clamp01(Vector2.Dot(p - a, rhs) / sqrMagnitude);
		}

		private Ray ToLocalRay(Ray worldRay)
		{
			return new Ray(target.InverseTransformPoint(worldRay.origin), target.InverseTransformDirection(worldRay.direction));
		}
	}
}
