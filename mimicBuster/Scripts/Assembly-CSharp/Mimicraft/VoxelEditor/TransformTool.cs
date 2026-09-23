using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class TransformTool
	{
		private enum Phase
		{
			Idle = 0,
			Moving = 1,
			Rotating = 2
		}

		private struct EdgeInfo
		{
			public Vector3 WorldA;

			public Vector3 WorldB;

			public int Axis;
		}

		private enum Refusal
		{
			None = 0,
			Size = 1,
			Level = 2,
			Map = 3
		}

		private const float ArrowLengthFactor = 0.6f;

		private const float MinArrowLength = 0.6f;

		private const float PickPixelThreshold = 14f;

		private const float RotateRingRadiusFactor = 0.35f;

		private const float MinRotateRingRadius = 0.3f;

		private const int RingSegments = 32;

		private const float MinRotateSensitivityRadiusFactor = 0.3f;

		private const float FadeSpeed = 8f;

		private const float RotatingArrowAlpha = 0.1f;

		private const float EdgeHoverArrowAlpha = 0.4f;

		private static readonly Color AxisXColor = new Color(0.9f, 0.2f, 0.2f, 0.95f);

		private static readonly Color AxisYColor = new Color(0.2f, 0.85f, 0.2f, 0.95f);

		private static readonly Color AxisZColor = new Color(0.2f, 0.45f, 0.95f, 0.95f);

		private const float CenterRingRadiusFactor = 0.75f;

		private const float DeltaFontSize = 1.4f;

		private const float DeltaWorldOffset = 0.9f;

		private static readonly Color DeltaColor = new Color(1f, 0.95f, 0.5f, 1f);

		private readonly VoxelModel model;

		private readonly Transform target;

		private readonly ArrowGizmo[] moveArrows = new ArrowGizmo[3];

		private readonly LineHighlight edgeHighlight;

		private readonly LineHighlight rotateRing;

		private readonly LineHighlight[] centerRings = new LineHighlight[3];

		private readonly TextMeshPro deltaLabel;

		private Phase phase;

		private int hoveredAxis = -1;

		private int hoveredEdge = -1;

		private int hoveredRing = -1;

		private float currentArrowAlpha = 1f;

		private Vector3 moveAxisWorldDir;

		private Vector3 moveStartPosition;

		private float moveGrabOffset;

		private Vector3 rotatePivotWorld;

		private Vector3 rotateAxisWorld;

		private Vector3 rotateStartVector;

		private Quaternion rotateStartRotation;

		private Vector3 rotateStartPosition;

		private float rotateRadius;

		private const int MaxRotationSteps = 48;

		private const float MapPathStep = 0.25f;

		private float acceptedMove;

		private float acceptedAngle;

		private bool startedWithinSize;

		private bool clearanceApplies;

		private ClearanceResult acceptedClearance;

		private float acceptedOverlap;

		private Transform ignoreRoot;

		private static Color AxisHoverColor => EditorPalette.TransformGizmo;

		private static Color EdgeHoverColor => EditorPalette.WithAlpha(EditorPalette.TransformGizmo, 0.9f);

		public bool IsActive => phase != Phase.Idle;

		public bool UseGlobalSpace { get; set; }

		public TransformMode Mode { get; set; }

		public float SnapMoveIncrement { get; set; } = 1f;

		public float SnapRotateIncrement { get; set; } = 15f;

		public float GizmoSizeMultiplier { get; set; } = 1f;

		public float GizmoThicknessMultiplier { get; set; } = 1f;

		public bool IsHoveringAxis => hoveredAxis >= 0;

		public bool IsHoveringEdge
		{
			get
			{
				if (hoveredEdge < 0)
				{
					return hoveredRing >= 0;
				}
				return true;
			}
		}

		public bool IsDragging => phase != Phase.Idle;

		public Color? HoveredAxisTint
		{
			get
			{
				if (hoveredAxis == 0)
				{
					return AxisXColor;
				}
				if (hoveredAxis == 1)
				{
					return AxisYColor;
				}
				if (hoveredAxis == 2)
				{
					return AxisZColor;
				}
				return null;
			}
		}

		public TransformTool(VoxelModel model, Transform highlightParent)
		{
			this.model = model;
			target = model.transform;
			for (int i = 0; i < moveArrows.Length; i++)
			{
				moveArrows[i] = ArrowGizmo.Create(highlightParent);
			}
			edgeHighlight = LineHighlight.Create(highlightParent, loop: false, 0.03f);
			rotateRing = LineHighlight.Create(highlightParent, loop: true, 0.03f);
			for (int j = 0; j < centerRings.Length; j++)
			{
				centerRings[j] = LineHighlight.Create(highlightParent, loop: true, 0.03f);
			}
			deltaLabel = WorldLabel.Create(highlightParent, "TransformDelta", 1.4f, DeltaColor);
			deltaLabel.gameObject.SetActive(value: false);
		}

		public void UpdateInput(Camera cam)
		{
			if (cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				return;
			}
			Vector3 pivotLocal = GetPivotLocal();
			float gizmoLength = GetGizmoLength();
			if (Mode == TransformMode.Move)
			{
				RenderMoveGizmo(pivotLocal, gizmoLength);
				HideCenterRings();
			}
			else
			{
				HideMoveArrows();
				RenderCenterRings(pivotLocal, gizmoLength);
			}
			switch (phase)
			{
			case Phase.Idle:
				if (Mode == TransformMode.Move)
				{
					UpdateIdle(cam, pivotLocal, gizmoLength, position);
				}
				else
				{
					UpdateIdleRotate(cam, pivotLocal, gizmoLength, position);
				}
				break;
			case Phase.Moving:
				UpdateMove(cam, position);
				break;
			case Phase.Rotating:
				UpdateRotate(cam, position);
				break;
			}
			UpdateDeltaLabel(cam, pivotLocal);
		}

		public void RenderWithoutInput()
		{
			if (phase == Phase.Idle)
			{
				hoveredAxis = -1;
				hoveredEdge = -1;
				hoveredRing = -1;
				edgeHighlight.Hide();
			}
			Vector3 pivotLocal = GetPivotLocal();
			float gizmoLength = GetGizmoLength();
			if (Mode == TransformMode.Move)
			{
				RenderMoveGizmo(pivotLocal, gizmoLength);
				HideCenterRings();
			}
			else
			{
				HideMoveArrows();
				RenderCenterRings(pivotLocal, gizmoLength);
			}
		}

		private void UpdateDeltaLabel(Camera cam, Vector3 pivotLocal)
		{
			if (phase == Phase.Idle)
			{
				if (deltaLabel.gameObject.activeSelf)
				{
					deltaLabel.gameObject.SetActive(value: false);
				}
				return;
			}
			string text;
			if (phase == Phase.Moving)
			{
				Vector3 vector = (target.position - moveStartPosition) / Mathf.Max(model.VoxelSize, 0.0001f);
				text = "(" + Signed(vector.x) + ", " + Signed(vector.y) + ", " + Signed(vector.z) + ")";
			}
			else
			{
				text = $"{Quaternion.Angle(rotateStartRotation, target.rotation):0}°";
			}
			deltaLabel.text = text;
			Vector3 vector2 = ((phase == Phase.Rotating) ? rotatePivotWorld : target.TransformPoint(pivotLocal)) + Vector3.up * 0.9f;
			deltaLabel.gameObject.SetActive(value: true);
			WorldLabel.Place(deltaLabel, vector2, WorldLabel.CameraPosition(cam, vector2), WorldLabel.ScaleFor(model));
		}

		private static string Signed(float value)
		{
			int num = Mathf.RoundToInt(value);
			if (num <= 0)
			{
				return num.ToString();
			}
			return $"+{num}";
		}

		public void Hide()
		{
			HideMoveArrows();
			HideCenterRings();
			edgeHighlight.Hide();
			rotateRing.Hide();
			if (deltaLabel != null && deltaLabel.gameObject.activeSelf)
			{
				deltaLabel.gameObject.SetActive(value: false);
			}
			phase = Phase.Idle;
			hoveredAxis = -1;
			hoveredEdge = -1;
			hoveredRing = -1;
		}

		private void HideMoveArrows()
		{
			ArrowGizmo[] array = moveArrows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Hide();
			}
		}

		private void HideCenterRings()
		{
			LineHighlight[] array = centerRings;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Hide();
			}
		}

		private Vector3 GetPivotLocal()
		{
			if (GridBounds.TryCompute(model.Grid, out var min, out var max))
			{
				return ((Vector3)min + (Vector3)max + Vector3.one) * 0.5f;
			}
			return Vector3.zero;
		}

		private float GetGizmoLength()
		{
			Vector3Int min;
			Vector3Int max;
			float num = ((!GridBounds.TryCompute(model.Grid, out min, out max)) ? 0.6f : Mathf.Max((max - min + Vector3.one).magnitude * 0.6f, 0.6f));
			return num * GizmoSizeMultiplier;
		}

		private Vector3[] GetAxisDirectionsLocal()
		{
			if (!UseGlobalSpace)
			{
				return new Vector3[3]
				{
					Vector3.right,
					Vector3.up,
					Vector3.forward
				};
			}
			return new Vector3[3]
			{
				target.InverseTransformDirection(Vector3.right),
				target.InverseTransformDirection(Vector3.up),
				target.InverseTransformDirection(Vector3.forward)
			};
		}

		private Vector3[] GetAxisDirectionsWorld()
		{
			if (!UseGlobalSpace)
			{
				return new Vector3[3] { target.right, target.up, target.forward };
			}
			return new Vector3[3]
			{
				Vector3.right,
				Vector3.up,
				Vector3.forward
			};
		}

		private void RenderMoveGizmo(Vector3 pivotLocal, float length)
		{
			float b = ((phase == Phase.Rotating) ? 0.1f : ((hoveredEdge >= 0) ? 0.4f : 1f));
			currentArrowAlpha = Mathf.Lerp(currentArrowAlpha, b, Time.deltaTime * 8f);
			float arrowScaleCompensation = GetArrowScaleCompensation();
			Vector3[] axisDirectionsLocal = GetAxisDirectionsLocal();
			moveArrows[0].ShowCustom(pivotLocal, axisDirectionsLocal[0], length, WithAlpha((hoveredAxis == 0) ? AxisHoverColor : AxisXColor, currentArrowAlpha), arrowScaleCompensation, GizmoThicknessMultiplier);
			moveArrows[1].ShowCustom(pivotLocal, axisDirectionsLocal[1], length, WithAlpha((hoveredAxis == 1) ? AxisHoverColor : AxisYColor, currentArrowAlpha), arrowScaleCompensation, GizmoThicknessMultiplier);
			moveArrows[2].ShowCustom(pivotLocal, axisDirectionsLocal[2], length, WithAlpha((hoveredAxis == 2) ? AxisHoverColor : AxisZColor, currentArrowAlpha), arrowScaleCompensation, GizmoThicknessMultiplier);
		}

		private void RenderCenterRings(Vector3 pivotLocal, float length)
		{
			if (phase == Phase.Rotating)
			{
				HideCenterRings();
				return;
			}
			Vector3 pivot = target.TransformPoint(pivotLocal);
			float centerRingRadius = GetCenterRingRadius(length);
			Vector3[] ringAxesWorld = GetRingAxesWorld();
			for (int i = 0; i < 3; i++)
			{
				Color color = ((hoveredRing == i) ? AxisHoverColor : AxisColor(i));
				centerRings[i].Show(BuildRingPoints(pivot, ringAxesWorld[i], centerRingRadius), color);
			}
		}

		private float GetCenterRingRadius(float length)
		{
			return Mathf.Max(length * 0.75f, 0.3f);
		}

		private Vector3[] GetRingAxesWorld()
		{
			return new Vector3[3] { target.right, target.up, target.forward };
		}

		private static Color AxisColor(int axis)
		{
			return axis switch
			{
				1 => AxisYColor, 
				0 => AxisXColor, 
				_ => AxisZColor, 
			};
		}

		private void UpdateIdleRotate(Camera cam, Vector3 pivotLocal, float length, Vector2 mouseScreen)
		{
			edgeHighlight.Hide();
			hoveredAxis = -1;
			hoveredEdge = -1;
			Vector3 pivotWorld = target.TransformPoint(pivotLocal);
			float centerRingRadius = GetCenterRingRadius(length);
			Vector3[] ringAxesWorld = GetRingAxesWorld();
			hoveredRing = PickRing(cam, mouseScreen, pivotWorld, ringAxesWorld, centerRingRadius);
			if (hoveredRing >= 0 && Mouse.current.leftButton.wasPressedThisFrame)
			{
				BeginRotateAboutCenter(pivotWorld, ringAxesWorld[hoveredRing], centerRingRadius, cam, mouseScreen);
			}
		}

		private static int PickRing(Camera cam, Vector2 mouseScreen, Vector3 pivotWorld, Vector3[] axes, float radius)
		{
			int result = -1;
			float num = 14f;
			for (int i = 0; i < 3; i++)
			{
				Vector3[] array = BuildRingPoints(pivotWorld, axes[i], radius);
				for (int j = 0; j < array.Length; j++)
				{
					Vector2 a = cam.WorldToScreenPoint(array[j]);
					Vector2 b = cam.WorldToScreenPoint(array[(j + 1) % array.Length]);
					float num2 = DistancePointToSegment(mouseScreen, a, b);
					if (num2 < num)
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		private void BeginRotateAboutCenter(Vector3 pivotWorld, Vector3 axisWorld, float radius, Camera cam, Vector2 mouseScreen)
		{
			rotatePivotWorld = pivotWorld;
			rotateAxisWorld = axisWorld.normalized;
			rotateStartRotation = target.rotation;
			rotateStartPosition = target.position;
			rotateRadius = radius;
			if (!TryGetPlaneVector(cam, mouseScreen, out rotateStartVector))
			{
				rotateStartVector = Vector3.Cross(rotateAxisWorld, Vector3.up).normalized;
			}
			rotateRing.Show(BuildRingPoints(rotatePivotWorld, rotateAxisWorld, rotateRadius), EdgeHoverColor);
			edgeHighlight.Hide();
			BeginGesture();
			phase = Phase.Rotating;
		}

		private static Color WithAlpha(Color color, float alphaMultiplier)
		{
			return new Color(color.r, color.g, color.b, color.a * alphaMultiplier);
		}

		private float GetArrowScaleCompensation()
		{
			return 1f / Mathf.Max(model.VoxelSize, 0.0001f);
		}

		private Vector3 GetArrowTipWorld(Vector3 pivotWorld, Vector3 worldDir, float length)
		{
			return pivotWorld + worldDir * length;
		}

		public float? GetHoverScreenDistance(Camera cam)
		{
			if (cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				return null;
			}
			Vector3 pivotLocal = GetPivotLocal();
			float gizmoLength = GetGizmoLength();
			Vector3 pivotWorld = target.TransformPoint(pivotLocal);
			float num = ((Mode == TransformMode.Rotate) ? NearestRingDistance(cam, position, pivotWorld, gizmoLength) : NearestMoveGizmoDistance(cam, position, pivotWorld, gizmoLength));
			if (!(num <= 14f))
			{
				return null;
			}
			return num;
		}

		private float NearestMoveGizmoDistance(Camera cam, Vector2 mouseScreen, Vector3 pivotWorld, float length)
		{
			Vector2 a = cam.WorldToScreenPoint(pivotWorld);
			float num = float.MaxValue;
			Vector3[] axisDirectionsWorld = GetAxisDirectionsWorld();
			for (int i = 0; i < 3; i++)
			{
				Vector2 b = cam.WorldToScreenPoint(GetArrowTipWorld(pivotWorld, axisDirectionsWorld[i], length));
				num = Mathf.Min(num, DistancePointToSegment(mouseScreen, a, b));
			}
			foreach (EdgeInfo item in ComputeEdges())
			{
				Vector2 a2 = cam.WorldToScreenPoint(item.WorldA);
				Vector2 b2 = cam.WorldToScreenPoint(item.WorldB);
				num = Mathf.Min(num, DistancePointToSegment(mouseScreen, a2, b2));
			}
			return num;
		}

		private float NearestRingDistance(Camera cam, Vector2 mouseScreen, Vector3 pivotWorld, float length)
		{
			float centerRingRadius = GetCenterRingRadius(length);
			Vector3[] ringAxesWorld = GetRingAxesWorld();
			float num = float.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				Vector3[] array = BuildRingPoints(pivotWorld, ringAxesWorld[i], centerRingRadius);
				for (int j = 0; j < array.Length; j++)
				{
					Vector2 a = cam.WorldToScreenPoint(array[j]);
					Vector2 b = cam.WorldToScreenPoint(array[(j + 1) % array.Length]);
					num = Mathf.Min(num, DistancePointToSegment(mouseScreen, a, b));
				}
			}
			return num;
		}

		private void UpdateIdle(Camera cam, Vector3 pivotLocal, float length, Vector2 mouseScreen)
		{
			List<EdgeInfo> list = ComputeEdges();
			hoveredAxis = PickAxis(cam, mouseScreen, pivotLocal, length);
			hoveredEdge = ((hoveredAxis < 0) ? PickEdge(cam, mouseScreen, list) : (-1));
			if (hoveredEdge >= 0)
			{
				edgeHighlight.Show(new Vector3[2]
				{
					list[hoveredEdge].WorldA,
					list[hoveredEdge].WorldB
				}, EdgeHoverColor);
			}
			else
			{
				edgeHighlight.Hide();
			}
			if (Mouse.current.leftButton.wasPressedThisFrame)
			{
				if (hoveredAxis >= 0)
				{
					BeginMove(hoveredAxis, cam, mouseScreen);
				}
				else if (hoveredEdge >= 0)
				{
					BeginRotate(list[hoveredEdge], cam, mouseScreen);
				}
			}
		}

		private List<EdgeInfo> ComputeEdges()
		{
			List<EdgeInfo> list = new List<EdgeInfo>(12);
			if (!GridBounds.TryCompute(model.Grid, out var min, out var max))
			{
				return list;
			}
			Vector3 vector = min;
			Vector3 vector2 = max + Vector3.one;
			Vector3[] array = new Vector3[8];
			for (int i = 0; i < 8; i++)
			{
				array[i] = new Vector3(((i & 1) != 0) ? vector2.x : vector.x, ((i & 2) != 0) ? vector2.y : vector.y, ((i & 4) != 0) ? vector2.z : vector.z);
			}
			for (int j = 0; j < 3; j++)
			{
				int num = 1 << j;
				for (int k = 0; k < 8; k++)
				{
					if ((k & num) == 0)
					{
						int num2 = k | num;
						list.Add(new EdgeInfo
						{
							WorldA = target.TransformPoint(array[k]),
							WorldB = target.TransformPoint(array[num2]),
							Axis = j
						});
					}
				}
			}
			return list;
		}

		private int PickAxis(Camera cam, Vector2 mouseScreen, Vector3 pivotLocal, float length)
		{
			Vector3 vector = target.TransformPoint(pivotLocal);
			Vector2 a = cam.WorldToScreenPoint(vector);
			Vector3[] axisDirectionsWorld = GetAxisDirectionsWorld();
			int result = -1;
			float num = 14f;
			for (int i = 0; i < 3; i++)
			{
				Vector2 b = cam.WorldToScreenPoint(GetArrowTipWorld(vector, axisDirectionsWorld[i], length));
				float num2 = DistancePointToSegment(mouseScreen, a, b);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}

		private static int PickEdge(Camera cam, Vector2 mouseScreen, List<EdgeInfo> edges)
		{
			int result = -1;
			float num = 14f;
			for (int i = 0; i < edges.Count; i++)
			{
				Vector2 a = cam.WorldToScreenPoint(edges[i].WorldA);
				Vector2 b = cam.WorldToScreenPoint(edges[i].WorldB);
				float num2 = DistancePointToSegment(mouseScreen, a, b);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
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

		private void BeginMove(int axis, Camera cam, Vector2 screenPos)
		{
			Vector3[] axisDirectionsWorld = GetAxisDirectionsWorld();
			moveAxisWorldDir = axisDirectionsWorld[axis].normalized;
			moveStartPosition = target.position;
			Ray ray = cam.ScreenPointToRay(screenPos);
			moveGrabOffset = (AxisProjection.TryGetDistanceAlongAxis(ray, moveStartPosition, moveAxisWorldDir, out var distanceAlongAxis) ? distanceAlongAxis : 0f);
			BeginGesture();
			phase = Phase.Moving;
		}

		private void BeginGesture()
		{
			acceptedMove = 0f;
			acceptedAngle = 0f;
			startedWithinSize = BodySizeWithinLimits();
			clearanceApplies = BodyClearance.Applies;
			ignoreRoot = FindPlayerRoot(target);
			if (clearanceApplies)
			{
				Accept(BodyClearance.Measure(model, ignoreRoot));
			}
			else
			{
				Accept(default(ClearanceResult));
			}
		}

		private void Accept(ClearanceResult result)
		{
			acceptedClearance = result;
			acceptedOverlap = ((clearanceApplies && result.Burial > 0f) ? BodyClearance.Measure(model, ignoreRoot, ClearanceParts.Overlap).Burial : 0f);
		}

		internal static Transform FindPlayerRoot(Transform piece)
		{
			PlayerVoxelBody playerVoxelBody = ((piece != null) ? piece.GetComponentInParent<PlayerVoxelBody>() : null);
			if (!(playerVoxelBody != null))
			{
				return null;
			}
			return playerVoxelBody.transform;
		}

		private bool BodySizeWithinLimits()
		{
			if (!VoxelEditorSettings.BodyRulesApply)
			{
				return true;
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(VoxelFocusManager.GatherBody());
			if (!voxelBodyReport.BelowMin)
			{
				return !voxelBodyReport.AboveMax;
			}
			return false;
		}

		private Refusal Judge(ClearanceResult candidate)
		{
			if (VoxelEditorSettings.BodyRulesApply && startedWithinSize && !BodySizeWithinLimits())
			{
				return Refusal.Size;
			}
			if (!clearanceApplies)
			{
				return Refusal.None;
			}
			if (candidate.OutsideCorners > acceptedClearance.OutsideCorners)
			{
				return Refusal.Map;
			}
			if (!candidate.NoWorseThan(acceptedClearance))
			{
				return Refusal.Level;
			}
			return Refusal.None;
		}

		private static void Complain(Refusal refusal)
		{
			switch (refusal)
			{
			case Refusal.Level:
				BodyClearance.Refuse();
				break;
			case Refusal.Map:
				PlayAreaGuard.Refuse();
				break;
			}
		}

		private static float SnapToward(float value, float from, float increment)
		{
			if (increment <= 0f)
			{
				return value;
			}
			if (value >= from)
			{
				return Mathf.Max(from, Mathf.Floor(value / increment + 0.0001f) * increment);
			}
			return Mathf.Min(from, Mathf.Ceil(value / increment - 0.0001f) * increment);
		}

		private static float StepBack(float candidate, float from, bool snapping, float increment)
		{
			if (!snapping || increment <= 0f)
			{
				return from + (candidate - from) * 0.5f;
			}
			float num = candidate - Mathf.Sign(candidate - from) * increment;
			if (Mathf.Sign(num - from) != Mathf.Sign(candidate - from))
			{
				return from;
			}
			return num;
		}

		private void ApplyMove(float distance)
		{
			target.SetPositionAndRotation(moveStartPosition + moveAxisWorldDir * distance, target.rotation);
		}

		private void MoveToward(float desired, bool snapping, float increment)
		{
			float num = acceptedMove;
			if (Mathf.Abs(desired - num) < 1E-05f)
			{
				return;
			}
			float num2 = desired;
			Refusal refusal = Refusal.None;
			if (clearanceApplies)
			{
				PieceShapeData shape = PieceShape.Of(model.Grid);
				Matrix4x4 localToWorldMatrix = target.localToWorldMatrix;
				float num3 = desired - num;
				float num4 = BodyClearance.SweepShare(shape, localToWorldMatrix, moveAxisWorldDir * num3, ignoreRoot);
				if (num4 < 1f)
				{
					float num5 = num3 * num4;
					num5 -= Mathf.Sign(num3) * Mathf.Min(Mathf.Abs(num5), 0.001f);
					num2 = num + num5;
					refusal = Refusal.Level;
				}
				if (MapBounds.AnyBaked)
				{
					float num6 = num2 - num;
					int num7 = Mathf.Max(1, Mathf.CeilToInt(Mathf.Abs(num6) / 0.25f));
					for (int i = 1; i <= num7; i++)
					{
						float num8 = num6 * (float)i / (float)num7;
						Matrix4x4 localToWorld = Matrix4x4.Translate(moveAxisWorldDir * num8) * localToWorldMatrix;
						if (BodyClearance.CountOutsideCorners(shape, localToWorld) > acceptedClearance.OutsideCorners)
						{
							num2 = num + num6 * (float)(i - 1) / (float)num7;
							refusal = Refusal.Map;
							break;
						}
					}
				}
			}
			if (snapping)
			{
				num2 = SnapToward(num2, num, increment);
			}
			float num9 = num2;
			for (int j = 0; j < 24; j++)
			{
				if (!(Mathf.Abs(num9 - num) >= 1E-05f))
				{
					break;
				}
				ApplyMove(num9);
				ClearanceResult clearanceResult = (clearanceApplies ? BodyClearance.Measure(model, ignoreRoot) : default(ClearanceResult));
				Refusal refusal2 = Judge(clearanceResult);
				if (refusal2 == Refusal.None)
				{
					acceptedMove = num9;
					Accept(clearanceResult);
					Complain(refusal);
					return;
				}
				refusal = refusal2;
				num9 = StepBack(num9, num, snapping, increment);
			}
			ApplyMove(num);
			Complain(refusal);
		}

		private void ApplyAngle(float degrees)
		{
			Quaternion quaternion = Quaternion.AngleAxis(degrees, rotateAxisWorld);
			target.SetPositionAndRotation(rotatePivotWorld + quaternion * (rotateStartPosition - rotatePivotWorld), quaternion * rotateStartRotation);
		}

		private void RotateToward(float desired, bool snapping, float increment)
		{
			float num = acceptedAngle;
			float num2 = desired - num;
			if (Mathf.Abs(num2) < 0.0001f)
			{
				return;
			}
			float num3 = desired;
			Refusal refusal = Refusal.None;
			if (clearanceApplies)
			{
				PieceShapeData shape = PieceShape.Of(model.Grid);
				Matrix4x4 localToWorldMatrix = target.localToWorldMatrix;
				float num4 = BodyClearance.Reach(shape, localToWorldMatrix, rotatePivotWorld);
				float num5 = ((num4 > 0.0001f) ? Mathf.Clamp(2.2918313f / num4, 0.25f, 10f) : 10f);
				int num6 = Mathf.CeilToInt(Mathf.Abs(num2) / num5);
				if (num6 > 48)
				{
					num6 = 48;
					num2 = Mathf.Sign(num2) * num5 * (float)num6;
					num3 = num + num2;
				}
				for (int i = 1; i <= num6; i++)
				{
					float angle = num2 * (float)i / (float)num6;
					Matrix4x4 localToWorld = Matrix4x4.Translate(rotatePivotWorld) * Matrix4x4.Rotate(Quaternion.AngleAxis(angle, rotateAxisWorld)) * Matrix4x4.Translate(-rotatePivotWorld) * localToWorldMatrix;
					ClearanceResult clearanceResult = BodyClearance.Measure(shape, localToWorld, ignoreRoot, ClearanceParts.Overlap);
					bool flag = MapBounds.AnyBaked && BodyClearance.CountOutsideCorners(shape, localToWorld) > acceptedClearance.OutsideCorners;
					if (clearanceResult.Burial > acceptedOverlap + 0.0005f || flag)
					{
						num3 = num + num2 * (float)(i - 1) / (float)num6;
						refusal = (flag ? Refusal.Map : Refusal.Level);
						break;
					}
				}
			}
			if (snapping)
			{
				num3 = SnapToward(num3, num, increment);
			}
			float num7 = num3;
			for (int j = 0; j < 24; j++)
			{
				if (!(Mathf.Abs(num7 - num) >= 0.0001f))
				{
					break;
				}
				ApplyAngle(num7);
				ClearanceResult clearanceResult2 = (clearanceApplies ? BodyClearance.Measure(model, ignoreRoot) : default(ClearanceResult));
				Refusal refusal2 = Judge(clearanceResult2);
				if (refusal2 == Refusal.None)
				{
					acceptedAngle = num7;
					Accept(clearanceResult2);
					Complain(refusal);
					return;
				}
				refusal = refusal2;
				num7 = StepBack(num7, num, snapping, increment);
			}
			ApplyAngle(num);
			Complain(refusal);
		}

		private void UpdateMove(Camera cam, Vector2 screenPos)
		{
			if (AxisProjection.TryGetDistanceAlongAxis(cam.ScreenPointToRay(screenPos), moveStartPosition, moveAxisWorldDir, out var distanceAlongAxis))
			{
				float num = distanceAlongAxis - moveGrabOffset;
				float num2 = SnapMoveIncrement * model.VoxelSize;
				bool flag = ShouldSnap() && num2 > 0.0001f;
				if (flag)
				{
					num = Mathf.Round(num / num2) * num2;
				}
				MoveToward(num, flag, num2);
			}
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				target.position = moveStartPosition;
				phase = Phase.Idle;
			}
			else if (Mouse.current.leftButton.wasReleasedThisFrame)
			{
				if (target.position != moveStartPosition)
				{
					Vector3 vector = (target.position - moveStartPosition) / Mathf.Max(model.VoxelSize, 0.0001f);
					string description = $"{Mathf.RoundToInt(vector.x)}x{Mathf.RoundToInt(vector.y)}x{Mathf.RoundToInt(vector.z)} Move";
					UndoManager.Push(new TransformCommand(target, moveStartPosition, target.rotation, target.position, target.rotation, description));
				}
				phase = Phase.Idle;
			}
		}

		private static bool ShouldSnap()
		{
			if (Keyboard.current != null)
			{
				if (!Keyboard.current.leftShiftKey.isPressed)
				{
					return !Keyboard.current.rightShiftKey.isPressed;
				}
				return false;
			}
			return true;
		}

		private void BeginRotate(EdgeInfo edge, Camera cam, Vector2 mouseScreen)
		{
			float num = Vector2.Distance(mouseScreen, cam.WorldToScreenPoint(edge.WorldA));
			float num2 = Vector2.Distance(mouseScreen, cam.WorldToScreenPoint(edge.WorldB));
			rotatePivotWorld = ((num <= num2) ? edge.WorldA : edge.WorldB);
			Vector3[] array = new Vector3[3] { target.right, target.up, target.forward };
			rotateAxisWorld = array[edge.Axis].normalized;
			rotateStartRotation = target.rotation;
			rotateStartPosition = target.position;
			rotateRadius = Mathf.Max(GetGizmoLength() * 0.35f, 0.3f);
			if (!TryGetPlaneVector(cam, mouseScreen, out rotateStartVector))
			{
				rotateStartVector = Vector3.Cross(rotateAxisWorld, Vector3.up).normalized;
			}
			rotateRing.Show(BuildRingPoints(rotatePivotWorld, rotateAxisWorld, rotateRadius), EdgeHoverColor);
			edgeHighlight.Hide();
			BeginGesture();
			phase = Phase.Rotating;
		}

		private bool TryGetPlaneVector(Camera cam, Vector2 mouseScreen, out Vector3 vector)
		{
			Plane plane = new Plane(rotateAxisWorld, rotatePivotWorld);
			Ray ray = cam.ScreenPointToRay(mouseScreen);
			if (plane.Raycast(ray, out var enter))
			{
				Vector3 vector2 = ray.GetPoint(enter) - rotatePivotWorld;
				if (vector2.magnitude < rotateRadius * 0.3f)
				{
					vector = default(Vector3);
					return false;
				}
				vector = vector2.normalized;
				return true;
			}
			vector = default(Vector3);
			return false;
		}

		private void UpdateRotate(Camera cam, Vector2 mouseScreen)
		{
			if (TryGetPlaneVector(cam, mouseScreen, out var vector))
			{
				float num = Vector3.SignedAngle(rotateStartVector, vector, rotateAxisWorld);
				float num2 = Mathf.Max(SnapRotateIncrement, 0.01f);
				bool flag = ShouldSnap();
				if (flag)
				{
					num = Mathf.Round(num / num2) * num2;
				}
				num = acceptedAngle + Mathf.DeltaAngle(acceptedAngle, num);
				RotateToward(num, flag, num2);
			}
			rotateRing.Show(BuildRingPoints(rotatePivotWorld, rotateAxisWorld, rotateRadius), EdgeHoverColor);
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				target.rotation = rotateStartRotation;
				target.position = rotateStartPosition;
				rotateRing.Hide();
				phase = Phase.Idle;
			}
			else if (Mouse.current.leftButton.wasReleasedThisFrame)
			{
				if (target.rotation != rotateStartRotation)
				{
					float num3 = Quaternion.Angle(rotateStartRotation, target.rotation);
					string description = $"Rotate {num3:0}deg";
					UndoManager.Push(new TransformCommand(target, rotateStartPosition, rotateStartRotation, target.position, target.rotation, description));
				}
				rotateRing.Hide();
				phase = Phase.Idle;
			}
		}

		private static Vector3[] BuildRingPoints(Vector3 pivot, Vector3 axis, float radius)
		{
			Vector3 rhs = ((Mathf.Abs(Vector3.Dot(axis, Vector3.up)) < 0.99f) ? Vector3.up : Vector3.right);
			Vector3 normalized = Vector3.Cross(axis, rhs).normalized;
			Vector3 normalized2 = Vector3.Cross(axis, normalized).normalized;
			Vector3[] array = new Vector3[32];
			for (int i = 0; i < 32; i++)
			{
				float f = (float)i * MathF.PI * 2f / 32f;
				array[i] = pivot + (normalized * Mathf.Cos(f) + normalized2 * Mathf.Sin(f)) * radius;
			}
			return array;
		}
	}
}
