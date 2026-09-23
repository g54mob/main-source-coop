using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class BodyClearance
	{
		public const float ContactMargin = 0.03f;

		public const float SendTolerance = 0.35f;

		public const float SendRestore = 0.2f;

		public const float ServerTolerance = 0.6f;

		private const float MaxShrinkShare = 0.6f;

		public const float RotationStepLength = 0.04f;

		private static readonly Collider[] overlaps = new Collider[32];

		private static readonly RaycastHit[] casts = new RaycastHit[32];

		private static readonly List<Collider> candidates = new List<Collider>();

		private static BoxCollider probe;

		private const float WarnCooldownSeconds = 3f;

		private static float lastWarned = float.NegativeInfinity;

		public static int LevelMask => -1957;

		public static bool Applies
		{
			get
			{
				if (!VoxelEditorSettings.BodyRulesApply)
				{
					return false;
				}
				GameModeController current = GameModeController.Current;
				if (!(current == null))
				{
					return current.EnforcesBodyClearance;
				}
				return true;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			probe = null;
		}

		public static ClearanceResult Measure(VoxelModel model, Transform ignoreRoot, ClearanceParts parts = ClearanceParts.All)
		{
			if (!(model == null))
			{
				return Measure(PieceShape.Of(model.Grid), model.transform.localToWorldMatrix, ignoreRoot, parts);
			}
			return default(ClearanceResult);
		}

		public static ClearanceResult Measure(PieceShapeData shape, Matrix4x4 localToWorld, Transform ignoreRoot, ClearanceParts parts = ClearanceParts.All)
		{
			ClearanceResult result = default(ClearanceResult);
			if (shape == null || !shape.HasVoxels)
			{
				return result;
			}
			bool depth = (parts & ClearanceParts.Depth) != 0;
			Pose(localToWorld, out var rotation, out var scale);
			GatherCandidates(shape, localToWorld, rotation, scale, ignoreRoot);
			float num = 0f;
			if (candidates.Count > 0)
			{
				for (int i = 0; i < shape.Boxes.Count; i++)
				{
					VoxelBox box = shape.Boxes[i];
					BoxInWorld(box, localToWorld, scale, out var centre, out var half);
					if (MayTouchCandidate(centre, half, rotation))
					{
						float num2 = BoxBurial(centre, half, rotation, ignoreRoot, depth, ref result.Blocker);
						num += num2 * (float)box.Volume;
					}
				}
			}
			result.Burial = ((shape.Volume > 0) ? Mathf.Clamp01(num / (float)shape.Volume) : 0f);
			if ((parts & ClearanceParts.Containment) != 0 && result.Burial <= 0f && shape.LargestBox >= 0)
			{
				BoxInWorld(shape.Boxes[shape.LargestBox], localToWorld, scale, out var centre2, out var _);
				if (SolidSpaceProbe.IsCenterInsideSolid(centre2, ignoreRoot, out var blocker))
				{
					result.Burial = 1f;
					result.Blocker = blocker;
				}
			}
			if ((parts & ClearanceParts.Outside) != 0)
			{
				CountOutside(shape, localToWorld, scale, ref result);
			}
			return result;
		}

		public static int CountOutsideCorners(PieceShapeData shape, Matrix4x4 localToWorld)
		{
			if (shape == null || !shape.HasVoxels || !MapBounds.AnyBaked)
			{
				return 0;
			}
			Pose(localToWorld, out var _, out var scale);
			ClearanceResult result = default(ClearanceResult);
			CountOutside(shape, localToWorld, scale, ref result);
			return result.OutsideCorners;
		}

		private static void GatherCandidates(PieceShapeData shape, Matrix4x4 localToWorld, Quaternion rotation, Vector3 scale, Transform ignoreRoot)
		{
			candidates.Clear();
			BoxInWorld(new VoxelBox
			{
				MinX = shape.Min.x,
				MinY = shape.Min.y,
				MinZ = shape.Min.z,
				SizeX = shape.Max.x - shape.Min.x + 1,
				SizeY = shape.Max.y - shape.Min.y + 1,
				SizeZ = shape.Max.z - shape.Min.z + 1
			}, localToWorld, scale, out var centre, out var half);
			int num = Physics.OverlapBoxNonAlloc(centre, Shrink(half), overlaps, rotation, LevelMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (Counts(overlaps[i], ignoreRoot))
				{
					candidates.Add(overlaps[i]);
				}
			}
		}

		private static bool MayTouchCandidate(Vector3 centre, Vector3 half, Quaternion rotation)
		{
			Vector3 vector = WorldExtent(half, rotation);
			Bounds bounds = new Bounds(centre, vector * 2f);
			foreach (Collider candidate in candidates)
			{
				if (candidate != null && candidate.bounds.Intersects(bounds))
				{
					return true;
				}
			}
			return false;
		}

		private static float BoxBurial(Vector3 centre, Vector3 half, Quaternion rotation, Transform ignoreRoot, bool depth, ref Collider blocker)
		{
			int num = Physics.OverlapBoxNonAlloc(centre, Shrink(half), overlaps, rotation, LevelMask, QueryTriggerInteraction.Ignore);
			bool flag = false;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				Collider collider = overlaps[i];
				if (Counts(collider, ignoreRoot))
				{
					flag = true;
					if ((object)blocker == null)
					{
						blocker = collider;
					}
					if (!depth)
					{
						return 1f;
					}
					num2 = Mathf.Max(num2, Penetration(centre, half, rotation, collider));
				}
			}
			if (!flag)
			{
				return 0f;
			}
			if (num2 > 0f)
			{
				return num2;
			}
			if (!SolidSpaceProbe.IsCenterInsideSolid(centre, ignoreRoot, out var _))
			{
				return 0.5f;
			}
			return 1f;
		}

		private static float Penetration(Vector3 centre, Vector3 half, Quaternion rotation, Collider other)
		{
			BoxCollider boxCollider = Probe();
			boxCollider.size = half * 2f;
			if (!Physics.ComputePenetration(boxCollider, centre, rotation, other, other.transform.position, other.transform.rotation, out var direction, out var distance))
			{
				return 0f;
			}
			Vector3 vector = Quaternion.Inverse(rotation) * direction;
			float a = 2f * (Mathf.Abs(vector.x) * half.x + Mathf.Abs(vector.y) * half.y + Mathf.Abs(vector.z) * half.z);
			return Mathf.Clamp01((distance - 0.03f) / Mathf.Max(a, 0.0001f));
		}

		private static void CountOutside(PieceShapeData shape, Matrix4x4 localToWorld, Vector3 scale, ref ClearanceResult result)
		{
			if (!MapBounds.AnyBaked)
			{
				return;
			}
			foreach (VoxelBox box in shape.Boxes)
			{
				BoxInWorld(box, localToWorld, scale, out var centre, out var _);
				if (!MapBounds.IsInsidePlayArea(centre))
				{
					result.OutsideCentres++;
				}
				for (int i = 0; i < 8; i++)
				{
					Vector3 point = new Vector3(((i & 1) == 0) ? box.MinX : (box.MinX + box.SizeX), ((i & 2) == 0) ? box.MinY : (box.MinY + box.SizeY), ((i & 4) == 0) ? box.MinZ : (box.MinZ + box.SizeZ));
					if (!MapBounds.IsInsidePlayArea(localToWorld.MultiplyPoint3x4(point)))
					{
						result.OutsideCorners++;
					}
				}
			}
		}

		public static float SweepShare(PieceShapeData shape, Matrix4x4 from, Vector3 worldDelta, Transform ignoreRoot)
		{
			float magnitude = worldDelta.magnitude;
			if (shape == null || !shape.HasVoxels || magnitude < 1E-05f)
			{
				return 1f;
			}
			Vector3 direction = worldDelta / magnitude;
			Pose(from, out var rotation, out var scale);
			BoxInWorld(new VoxelBox
			{
				MinX = shape.Min.x,
				MinY = shape.Min.y,
				MinZ = shape.Min.z,
				SizeX = shape.Max.x - shape.Min.x + 1,
				SizeY = shape.Max.y - shape.Min.y + 1,
				SizeZ = shape.Max.z - shape.Min.z + 1
			}, from, scale, out var centre, out var half);
			if (Nearest(centre, Shrink(half), rotation, direction, magnitude, ignoreRoot) >= magnitude)
			{
				return 1f;
			}
			float num = magnitude;
			foreach (VoxelBox box in shape.Boxes)
			{
				BoxInWorld(box, from, scale, out var centre2, out var half2);
				num = Mathf.Min(num, Nearest(centre2, Shrink(half2), rotation, direction, num, ignoreRoot));
				if (num <= 0f)
				{
					break;
				}
			}
			return Mathf.Clamp01(num / magnitude);
		}

		public static int FittingLayers(VoxelBox firstLayer, Vector3Int normal, int layers, Matrix4x4 localToWorld, Transform ignoreRoot)
		{
			if (layers <= 0)
			{
				return 0;
			}
			Pose(localToWorld, out var rotation, out var scale);
			BoxInWorld(firstLayer, localToWorld, scale, out var centre, out var half);
			Vector3 vector = Shrink(half);
			int num = Physics.OverlapBoxNonAlloc(centre, vector, overlaps, rotation, LevelMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (Counts(overlaps[i], ignoreRoot))
				{
					return 0;
				}
			}
			if (layers == 1)
			{
				return 1;
			}
			Vector3 vector2 = localToWorld.MultiplyVector(normal);
			float magnitude = vector2.magnitude;
			if (magnitude < 1E-05f)
			{
				return layers;
			}
			float distance = magnitude * (float)(layers - 1);
			float num2 = Nearest(centre, vector, rotation, vector2 / magnitude, distance, ignoreRoot);
			return Mathf.Clamp(1 + Mathf.FloorToInt(num2 / magnitude + 0.0001f), 1, layers);
		}

		private static float Nearest(Vector3 centre, Vector3 half, Quaternion rotation, Vector3 direction, float distance, Transform ignoreRoot)
		{
			int num = Physics.BoxCastNonAlloc(centre, half, direction, casts, rotation, distance, LevelMask, QueryTriggerInteraction.Ignore);
			float num2 = distance;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = casts[i];
				if ((!(raycastHit.distance <= 0f) || !(raycastHit.point == Vector3.zero)) && raycastHit.distance < num2 && Counts(raycastHit.collider, ignoreRoot))
				{
					num2 = raycastHit.distance;
				}
			}
			return num2;
		}

		public static void Refuse()
		{
			if (!(Time.unscaledTime - lastWarned < 3f))
			{
				lastWarned = Time.unscaledTime;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Tool.BlockedByGeometry"));
				}
			}
		}

		private static bool Counts(Collider collider, Transform ignoreRoot)
		{
			if (collider == null || collider.isTrigger)
			{
				return false;
			}
			if (collider is CharacterController || collider.GetComponentInParent<VoxelModel>() != null)
			{
				return false;
			}
			if (probe != null && collider == probe)
			{
				return false;
			}
			if (!(ignoreRoot == null))
			{
				if (collider.transform != ignoreRoot)
				{
					return !collider.transform.IsChildOf(ignoreRoot);
				}
				return false;
			}
			return true;
		}

		private static BoxCollider Probe()
		{
			if (probe != null)
			{
				return probe;
			}
			GameObject gameObject = new GameObject("BodyClearanceProbe");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.layer = 2;
			gameObject.transform.position = new Vector3(0f, -100000f, 0f);
			Object.DontDestroyOnLoad(gameObject);
			probe = gameObject.AddComponent<BoxCollider>();
			probe.isTrigger = true;
			return probe;
		}

		private static Vector3 Shrink(Vector3 half)
		{
			return new Vector3(Mathf.Max(half.x - 0.03f, half.x * 0.39999998f), Mathf.Max(half.y - 0.03f, half.y * 0.39999998f), Mathf.Max(half.z - 0.03f, half.z * 0.39999998f));
		}

		private static void Pose(Matrix4x4 m, out Quaternion rotation, out Vector3 scale)
		{
			Vector3 vector = m.GetColumn(0);
			Vector3 vector2 = m.GetColumn(1);
			Vector3 vector3 = m.GetColumn(2);
			scale = new Vector3(vector.magnitude, vector2.magnitude, vector3.magnitude);
			rotation = ((vector3.sqrMagnitude > 0f && vector2.sqrMagnitude > 0f) ? Quaternion.LookRotation(vector3 / scale.z, vector2 / scale.y) : Quaternion.identity);
		}

		private static void BoxInWorld(VoxelBox box, Matrix4x4 localToWorld, Vector3 scale, out Vector3 centre, out Vector3 half)
		{
			Vector3 point = new Vector3((float)box.MinX + (float)box.SizeX * 0.5f, (float)box.MinY + (float)box.SizeY * 0.5f, (float)box.MinZ + (float)box.SizeZ * 0.5f);
			centre = localToWorld.MultiplyPoint3x4(point);
			half = new Vector3((float)box.SizeX * 0.5f * scale.x, (float)box.SizeY * 0.5f * scale.y, (float)box.SizeZ * 0.5f * scale.z);
		}

		private static Vector3 WorldExtent(Vector3 half, Quaternion rotation)
		{
			Vector3 vector = rotation * Vector3.right;
			Vector3 vector2 = rotation * Vector3.up;
			Vector3 vector3 = rotation * Vector3.forward;
			return new Vector3(Mathf.Abs(vector.x) * half.x + Mathf.Abs(vector2.x) * half.y + Mathf.Abs(vector3.x) * half.z, Mathf.Abs(vector.y) * half.x + Mathf.Abs(vector2.y) * half.y + Mathf.Abs(vector3.y) * half.z, Mathf.Abs(vector.z) * half.x + Mathf.Abs(vector2.z) * half.y + Mathf.Abs(vector3.z) * half.z);
		}

		public static float Reach(PieceShapeData shape, Matrix4x4 localToWorld, Vector3 pivot)
		{
			if (shape == null || !shape.HasVoxels)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				Vector3 point = new Vector3(((i & 1) == 0) ? shape.Min.x : (shape.Max.x + 1), ((i & 2) == 0) ? shape.Min.y : (shape.Max.y + 1), ((i & 4) == 0) ? shape.Min.z : (shape.Max.z + 1));
				num = Mathf.Max(num, (localToWorld.MultiplyPoint3x4(point) - pivot).magnitude);
			}
			return num;
		}
	}
}
