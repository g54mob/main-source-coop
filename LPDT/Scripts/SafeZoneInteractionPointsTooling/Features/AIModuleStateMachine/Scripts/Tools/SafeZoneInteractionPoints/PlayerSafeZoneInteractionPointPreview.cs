using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Tools.SafeZoneInteractionPoints
{
	public static class PlayerSafeZoneInteractionPointPreview
	{
		public readonly struct Settings
		{
			public readonly PlayerSafeZoneInteractionPointPreviewShape PreviewShape;

			public readonly float CandidateHeightOffset;

			public readonly float CandidateSphereRadius;

			public readonly float CandidateBoxRotationY;

			public readonly Vector3 CandidateBoxSize;

			public readonly float CandidatePointSpacing;

			public readonly float CandidatePointRadius;

			public readonly bool CandidateGroupPoints;

			public readonly bool CandidateSeparateSurfaceGroups;

			public readonly int CandidateMaxGroupedPointsCount;

			public readonly float CandidateVerticalCoverage;

			public readonly LayerMask CandidateObstacleMask;

			public readonly float CandidateFloorRayMaxDistance;

			public readonly float CandidateObstacleProbeRadius;

			public readonly bool CandidateIgnoreTriggers;

			public readonly bool CandidateIgnoreOwnSafeZoneColliders;

			public PlayerSafeZoneInteractionPointValidationSettings ValidationSettings => new PlayerSafeZoneInteractionPointValidationSettings(CandidateObstacleMask, CandidateFloorRayMaxDistance, CandidateObstacleProbeRadius, CandidateIgnoreTriggers, CandidateIgnoreOwnSafeZoneColliders);

			public Settings(PlayerSafeZoneInteractionPointPreviewShape previewShape, float candidateHeightOffset, float candidateSphereRadius, float candidateBoxRotationY, Vector3 candidateBoxSize, float candidatePointSpacing, float candidatePointRadius, bool candidateGroupPoints, bool candidateSeparateSurfaceGroups, int candidateMaxGroupedPointsCount, float candidateVerticalCoverage, LayerMask candidateObstacleMask, float candidateFloorRayMaxDistance, float candidateObstacleProbeRadius, bool candidateIgnoreTriggers, bool candidateIgnoreOwnSafeZoneColliders)
			{
				PreviewShape = previewShape;
				CandidateHeightOffset = candidateHeightOffset;
				CandidateSphereRadius = candidateSphereRadius;
				CandidateBoxRotationY = candidateBoxRotationY;
				CandidateBoxSize = candidateBoxSize;
				CandidatePointSpacing = candidatePointSpacing;
				CandidatePointRadius = candidatePointRadius;
				CandidateGroupPoints = candidateGroupPoints;
				CandidateSeparateSurfaceGroups = candidateSeparateSurfaceGroups;
				CandidateMaxGroupedPointsCount = candidateMaxGroupedPointsCount;
				CandidateVerticalCoverage = candidateVerticalCoverage;
				CandidateObstacleMask = candidateObstacleMask;
				CandidateFloorRayMaxDistance = candidateFloorRayMaxDistance;
				CandidateObstacleProbeRadius = candidateObstacleProbeRadius;
				CandidateIgnoreTriggers = candidateIgnoreTriggers;
				CandidateIgnoreOwnSafeZoneColliders = candidateIgnoreOwnSafeZoneColliders;
			}
		}

		private readonly struct CandidatePointGizmo
		{
			public readonly Vector3 Position;

			public readonly bool IsValid;

			public CandidatePointGizmo(Vector3 position, bool isValid)
			{
				Position = position;
				IsValid = isValid;
			}
		}

		private readonly struct CandidatePointGizmoCache
		{
			private const float BOUNDS_EPSILON_SQR = 1E-06f;

			private const float RADIUS_EPSILON = 0.0001f;

			public readonly Bounds Bounds;

			public readonly float Radius;

			public readonly int SettingsHash;

			public readonly List<CandidatePointGizmo> Points;

			public CandidatePointGizmoCache(Bounds bounds, float radius, int settingsHash, List<CandidatePointGizmo> points)
			{
				Bounds = bounds;
				Radius = radius;
				SettingsHash = settingsHash;
				Points = points;
			}

			public bool IsValid(Bounds bounds, float radius, int settingsHash)
			{
				if (SettingsHash == settingsHash && Mathf.Abs(Radius - radius) <= 0.0001f && (Bounds.center - bounds.center).sqrMagnitude <= 1E-06f)
				{
					return (Bounds.size - bounds.size).sqrMagnitude <= 1E-06f;
				}
				return false;
			}
		}

		private readonly struct OrientedBox
		{
			public readonly Vector3 Center;

			public readonly Vector3 Size;

			public readonly Quaternion Rotation;

			public OrientedBox(Vector3 center, Vector3 size, Quaternion rotation)
			{
				Center = center;
				Size = size;
				Rotation = rotation;
			}

			public Vector3 TransformPoint(Vector3 localPosition)
			{
				return Center + Rotation * localPosition;
			}
		}

		private struct CandidatePointGroup
		{
			private Vector3 _sum;

			public int Count { get; private set; }

			public Vector3 Center
			{
				get
				{
					if (Count <= 0)
					{
						return Vector3.zero;
					}
					return _sum / Count;
				}
			}

			public void Add(Vector3 position)
			{
				_sum += position;
				Count++;
			}
		}

		private static readonly Color _selectedGizmoFillColor = new Color(0.1f, 0.8f, 1f, 0.08f);

		private static readonly Color _selectedGizmoWireColor = new Color(0.1f, 0.8f, 1f, 0.15f);

		private static readonly Color _selectedGizmoFullBoundsWireColor = new Color(0.1f, 0.8f, 1f, 0.1f);

		private static readonly Color _candidateInvalidColor = new Color(1f, 0.12f, 0.08f, 0.95f);

		private static readonly Color _candidateValidColor = new Color(0.12f, 1f, 0.32f, 0.95f);

		private const float MIN_GIZMO_RADIUS = 0.1f;

		private const float MIN_CANDIDATE_POINT_SPACING = 0.1f;

		private const float MIN_CANDIDATE_POINT_RADIUS = 0.01f;

		private const float GROUP_DISTANCE_MULTIPLIER = 1.5f;

		private const int MAX_CANDIDATE_POINT_COUNT = 256;

		private const int PREVIEW_SPHERE_LATITUDE_SEGMENTS = 12;

		private const int PREVIEW_SPHERE_LONGITUDE_SEGMENTS = 32;

		private const float GOLDEN_ANGLE = 2.3999631f;

		private static Mesh _previewSphereMesh;

		private static float _previewSphereMeshCoverage = -1f;

		private static readonly Dictionary<int, CandidatePointGizmoCache> _candidatePointGizmoCaches = new Dictionary<int, CandidatePointGizmoCache>();

		public static void DrawSelected(PlayerSafeZone safeZone, Settings settings)
		{
			if (!(safeZone == null))
			{
				GetGizmoSourceBounds(safeZone, out var bounds, out var center, out var radius);
				center += Vector3.up * settings.CandidateHeightOffset;
				bounds.center += Vector3.up * settings.CandidateHeightOffset;
				radius = Mathf.Max(0.1f, settings.CandidateSphereRadius);
				float verticalCoverage = Mathf.Clamp01(settings.CandidateVerticalCoverage);
				DrawPreviewShape(bounds, center, radius, verticalCoverage, settings);
				DrawCandidatePointGizmos(safeZone, bounds, center, radius, verticalCoverage, settings);
			}
		}

		public static List<Vector3> GenerateGroupedCandidateCenters(PlayerSafeZone safeZone, Settings settings)
		{
			List<Vector3> list = new List<Vector3>();
			if (safeZone == null)
			{
				return list;
			}
			GetGizmoSourceBounds(safeZone, out var bounds, out var center, out var radius);
			center += Vector3.up * settings.CandidateHeightOffset;
			bounds.center += Vector3.up * settings.CandidateHeightOffset;
			radius = Mathf.Max(0.1f, settings.CandidateSphereRadius);
			float verticalCoverage = Mathf.Clamp01(settings.CandidateVerticalCoverage);
			float spacing = Mathf.Max(0.1f, settings.CandidatePointSpacing);
			List<Vector3> list2 = new List<Vector3>();
			float spacing2 = CollectValidCandidatePositions(safeZone, bounds, center, radius, verticalCoverage, spacing, settings, list2);
			if (list2.Count == 0)
			{
				return list;
			}
			List<CandidatePointGroup> list3 = BuildCandidatePointGroups(list2, spacing2, bounds, center, settings);
			list3.Sort((CandidatePointGroup left, CandidatePointGroup right) => right.Count.CompareTo(left.Count));
			int num = Mathf.Min(Mathf.Max(0, settings.CandidateMaxGroupedPointsCount), list3.Count);
			for (int num2 = 0; num2 < num; num2++)
			{
				list.Add(list3[num2].Center);
			}
			return list;
		}

		public static bool IsCandidatePositionValid(Vector3 pointPosition, PlayerSafeZone safeZone, Settings settings)
		{
			return PlayerSafeZoneInteractionPointValidator.IsValid(pointPosition, safeZone, settings.ValidationSettings);
		}

		private static void GetGizmoSourceBounds(PlayerSafeZone safeZone, out Bounds bounds, out Vector3 center, out float radius)
		{
			Collider[] componentsInChildren = safeZone.GetComponentsInChildren<Collider>(includeInactive: true);
			bool flag = false;
			bounds = default(Bounds);
			foreach (Collider collider in componentsInChildren)
			{
				if (!(collider == null) && collider.enabled)
				{
					if (!flag)
					{
						bounds = collider.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
			if (!flag)
			{
				center = safeZone.transform.position;
				Vector3 lossyScale = safeZone.transform.lossyScale;
				radius = Mathf.Max(0.1f, Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z)) * 0.5f);
				bounds = new Bounds(center, Vector3.one * radius * 2f);
			}
			else
			{
				center = bounds.center;
				Vector3 extents = bounds.extents;
				radius = Mathf.Max(0.1f, Mathf.Max(extents.x, Mathf.Max(extents.y, extents.z)));
			}
		}

		private static void DrawPreviewShape(Bounds bounds, Vector3 center, float radius, float verticalCoverage, Settings settings)
		{
			if (settings.PreviewShape == PlayerSafeZoneInteractionPointPreviewShape.Box)
			{
				OrientedBox previewBox = GetPreviewBox(bounds, settings);
				OrientedBox verticalCoverageClippedBox = GetVerticalCoverageClippedBox(previewBox, verticalCoverage);
				Matrix4x4 matrix = Gizmos.matrix;
				Gizmos.matrix = Matrix4x4.TRS(verticalCoverageClippedBox.Center, verticalCoverageClippedBox.Rotation, Vector3.one);
				Gizmos.color = _selectedGizmoFillColor;
				Gizmos.DrawCube(Vector3.zero, verticalCoverageClippedBox.Size);
				if (verticalCoverage < 0.999f)
				{
					Gizmos.matrix = Matrix4x4.TRS(previewBox.Center, previewBox.Rotation, Vector3.one);
					Gizmos.color = _selectedGizmoFullBoundsWireColor;
					Gizmos.DrawWireCube(Vector3.zero, previewBox.Size);
				}
				Gizmos.matrix = Matrix4x4.TRS(verticalCoverageClippedBox.Center, verticalCoverageClippedBox.Rotation, Vector3.one);
				Gizmos.color = _selectedGizmoWireColor;
				Gizmos.DrawWireCube(Vector3.zero, verticalCoverageClippedBox.Size);
				Gizmos.matrix = matrix;
			}
			else
			{
				Gizmos.color = _selectedGizmoFillColor;
				Gizmos.DrawMesh(GetPreviewSphereMesh(verticalCoverage), center, Quaternion.identity, Vector3.one * radius);
				Gizmos.color = _selectedGizmoWireColor;
				DrawPreviewSphereWireGizmos(center, radius, verticalCoverage);
			}
		}

		private static void DrawCandidatePointGizmos(PlayerSafeZone safeZone, Bounds bounds, Vector3 center, float radius, float verticalCoverage, Settings settings)
		{
			float spacing = Mathf.Max(0.1f, settings.CandidatePointSpacing);
			float radius2 = Mathf.Max(0.01f, settings.CandidatePointRadius);
			CandidatePointGizmoCache candidatePointGizmoCache = GetCandidatePointGizmoCache(safeZone, bounds, center, radius, verticalCoverage, spacing, settings);
			for (int i = 0; i < candidatePointGizmoCache.Points.Count; i++)
			{
				CandidatePointGizmo candidatePointGizmo = candidatePointGizmoCache.Points[i];
				Gizmos.color = (candidatePointGizmo.IsValid ? _candidateValidColor : _candidateInvalidColor);
				Gizmos.DrawSphere(candidatePointGizmo.Position, radius2);
			}
		}

		private static CandidatePointGizmoCache GetCandidatePointGizmoCache(PlayerSafeZone safeZone, Bounds bounds, Vector3 center, float radius, float verticalCoverage, float spacing, Settings settings)
		{
			int instanceID = safeZone.GetInstanceID();
			int settingsHash = GetSettingsHash(settings);
			if (_candidatePointGizmoCaches.TryGetValue(instanceID, out var value) && value.IsValid(bounds, radius, settingsHash))
			{
				return value;
			}
			value = BuildCandidatePointGizmoCache(safeZone, bounds, center, radius, verticalCoverage, spacing, settings, settingsHash);
			_candidatePointGizmoCaches[instanceID] = value;
			return value;
		}

		private static CandidatePointGizmoCache BuildCandidatePointGizmoCache(PlayerSafeZone safeZone, Bounds bounds, Vector3 center, float radius, float verticalCoverage, float spacing, Settings settings, int settingsHash)
		{
			List<CandidatePointGizmo> list = new List<CandidatePointGizmo>();
			if (settings.CandidateGroupPoints)
			{
				CollectGroupedCandidatePointGizmos(safeZone, bounds, center, radius, verticalCoverage, spacing, settings, list);
				return new CandidatePointGizmoCache(bounds, radius, settingsHash, list);
			}
			if (settings.PreviewShape == PlayerSafeZoneInteractionPointPreviewShape.Box)
			{
				OrientedBox previewBox = GetPreviewBox(bounds, settings);
				OrientedBox verticalCoverageClippedBox = GetVerticalCoverageClippedBox(previewBox, verticalCoverage);
				int boxCandidatePointCount = GetBoxCandidatePointCount(previewBox.Size, spacing);
				if (boxCandidatePointCount >= 256)
				{
					spacing *= Mathf.Sqrt((float)boxCandidatePointCount / 256f);
				}
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 0, list);
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 1, list);
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 2, list);
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 3, list);
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 4, list);
				CollectBoxFaceCandidatePointGizmos(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 5, list);
				return new CandidatePointGizmoCache(bounds, radius, settingsHash, list);
			}
			int sphereCandidatePointCount = GetSphereCandidatePointCount(radius, spacing);
			for (int i = 0; i < sphereCandidatePointCount; i++)
			{
				Vector3 fibonacciSphereDirection = GetFibonacciSphereDirection(i, sphereCandidatePointCount);
				if (!(Mathf.Abs(fibonacciSphereDirection.y) > verticalCoverage))
				{
					Vector3 vector = center + fibonacciSphereDirection * radius;
					bool isValid = !IsSphereSurfaceGroupBoundary(vector, center, radius, spacing, settings) && PlayerSafeZoneInteractionPointValidator.IsValid(vector, safeZone, settings.ValidationSettings);
					list.Add(new CandidatePointGizmo(vector, isValid));
				}
			}
			return new CandidatePointGizmoCache(bounds, radius, settingsHash, list);
		}

		private static void CollectGroupedCandidatePointGizmos(PlayerSafeZone safeZone, Bounds bounds, Vector3 center, float radius, float verticalCoverage, float spacing, Settings settings, List<CandidatePointGizmo> points)
		{
			List<Vector3> list = new List<Vector3>();
			float spacing2 = CollectValidCandidatePositions(safeZone, bounds, center, radius, verticalCoverage, spacing, settings, list);
			if (list.Count == 0)
			{
				return;
			}
			int num = Mathf.Max(0, settings.CandidateMaxGroupedPointsCount);
			if (num != 0)
			{
				List<CandidatePointGroup> list2 = BuildCandidatePointGroups(list, spacing2, bounds, center, settings);
				list2.Sort((CandidatePointGroup left, CandidatePointGroup right) => right.Count.CompareTo(left.Count));
				int num2 = Mathf.Min(num, list2.Count);
				for (int num3 = 0; num3 < num2; num3++)
				{
					points.Add(new CandidatePointGizmo(list2[num3].Center, isValid: true));
				}
			}
		}

		private static void CollectBoxFaceCandidatePointGizmos(PlayerSafeZone safeZone, OrientedBox box, OrientedBox clipBox, float spacing, Settings settings, int faceIndex, List<CandidatePointGizmo> points)
		{
			Vector2 boxFaceSize = GetBoxFaceSize(box.Size, faceIndex);
			int num = Mathf.Max(2, Mathf.CeilToInt(boxFaceSize.x / spacing) + 1);
			int num2 = Mathf.Max(2, Mathf.CeilToInt(boxFaceSize.y / spacing) + 1);
			for (int i = 0; i < num; i++)
			{
				float u = ((num <= 1) ? 0.5f : ((float)i / ((float)num - 1f)));
				for (int j = 0; j < num2; j++)
				{
					float v = ((num2 <= 1) ? 0.5f : ((float)j / ((float)num2 - 1f)));
					Vector3 boxFaceLocalPoint = GetBoxFaceLocalPoint(box.Size, u, v, faceIndex);
					if (IsWithinVerticalClip(boxFaceLocalPoint, clipBox))
					{
						Vector3 vector = box.TransformPoint(boxFaceLocalPoint);
						bool isValid = !IsBoxSurfaceGroupBoundary(box, vector, settings) && PlayerSafeZoneInteractionPointValidator.IsValid(vector, safeZone, settings.ValidationSettings);
						points.Add(new CandidatePointGizmo(vector, isValid));
					}
				}
			}
		}

		private static float CollectValidCandidatePositions(PlayerSafeZone safeZone, Bounds bounds, Vector3 center, float radius, float verticalCoverage, float spacing, Settings settings, List<Vector3> validPositions)
		{
			if (settings.PreviewShape == PlayerSafeZoneInteractionPointPreviewShape.Box)
			{
				OrientedBox previewBox = GetPreviewBox(bounds, settings);
				OrientedBox verticalCoverageClippedBox = GetVerticalCoverageClippedBox(previewBox, verticalCoverage);
				int boxCandidatePointCount = GetBoxCandidatePointCount(previewBox.Size, spacing);
				if (boxCandidatePointCount >= 256)
				{
					spacing *= Mathf.Sqrt((float)boxCandidatePointCount / 256f);
				}
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 0, validPositions);
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 1, validPositions);
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 2, validPositions);
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 3, validPositions);
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 4, validPositions);
				CollectValidBoxFaceCandidates(safeZone, previewBox, verticalCoverageClippedBox, spacing, settings, 5, validPositions);
				return spacing;
			}
			int sphereCandidatePointCount = GetSphereCandidatePointCount(radius, spacing);
			for (int i = 0; i < sphereCandidatePointCount; i++)
			{
				Vector3 fibonacciSphereDirection = GetFibonacciSphereDirection(i, sphereCandidatePointCount);
				if (!(Mathf.Abs(fibonacciSphereDirection.y) > verticalCoverage))
				{
					Vector3 vector = center + fibonacciSphereDirection * radius;
					if (!IsSphereSurfaceGroupBoundary(vector, center, radius, spacing, settings) && PlayerSafeZoneInteractionPointValidator.IsValid(vector, safeZone, settings.ValidationSettings))
					{
						validPositions.Add(vector);
					}
				}
			}
			return GetSphereEffectiveCandidateSpacing(radius, verticalCoverage, sphereCandidatePointCount, spacing);
		}

		private static List<CandidatePointGroup> BuildCandidatePointGroups(List<Vector3> positions, float spacing, Bounds bounds, Vector3 sphereCenter, Settings settings)
		{
			if (!settings.CandidateSeparateSurfaceGroups)
			{
				if (settings.PreviewShape != PlayerSafeZoneInteractionPointPreviewShape.Sphere)
				{
					return BuildCandidatePointGroups(positions, spacing);
				}
				return BuildAdaptiveCandidatePointGroups(positions, spacing);
			}
			if (settings.PreviewShape != PlayerSafeZoneInteractionPointPreviewShape.Sphere)
			{
				return BuildBoxFaceCandidatePointGroups(positions, spacing, bounds, settings);
			}
			return BuildSphereSectorCandidatePointGroups(positions, spacing, sphereCenter);
		}

		private static void CollectValidBoxFaceCandidates(PlayerSafeZone safeZone, OrientedBox box, OrientedBox clipBox, float spacing, Settings settings, int faceIndex, List<Vector3> validPositions)
		{
			Vector2 boxFaceSize = GetBoxFaceSize(box.Size, faceIndex);
			int num = Mathf.Max(2, Mathf.CeilToInt(boxFaceSize.x / spacing) + 1);
			int num2 = Mathf.Max(2, Mathf.CeilToInt(boxFaceSize.y / spacing) + 1);
			for (int i = 0; i < num; i++)
			{
				float u = ((num <= 1) ? 0.5f : ((float)i / ((float)num - 1f)));
				for (int j = 0; j < num2; j++)
				{
					float v = ((num2 <= 1) ? 0.5f : ((float)j / ((float)num2 - 1f)));
					Vector3 boxFaceLocalPoint = GetBoxFaceLocalPoint(box.Size, u, v, faceIndex);
					if (IsWithinVerticalClip(boxFaceLocalPoint, clipBox))
					{
						Vector3 vector = box.TransformPoint(boxFaceLocalPoint);
						if (!IsBoxSurfaceGroupBoundary(box, vector, settings) && PlayerSafeZoneInteractionPointValidator.IsValid(vector, safeZone, settings.ValidationSettings))
						{
							validPositions.Add(vector);
						}
					}
				}
			}
		}

		private static List<CandidatePointGroup> BuildBoxFaceCandidatePointGroups(List<Vector3> positions, float spacing, Bounds bounds, Settings settings)
		{
			OrientedBox previewBox = GetPreviewBox(bounds, settings);
			List<CandidatePointGroup> list = new List<CandidatePointGroup>();
			List<Vector3>[] array = CreatePositionBuckets(6);
			for (int i = 0; i < positions.Count; i++)
			{
				array[GetNearestBoxFaceIndex(previewBox, positions[i])].Add(positions[i]);
			}
			for (int j = 0; j < array.Length; j++)
			{
				list.AddRange(BuildCandidatePointGroups(array[j], spacing));
			}
			return list;
		}

		private static List<CandidatePointGroup> BuildSphereSectorCandidatePointGroups(List<Vector3> positions, float spacing, Vector3 center)
		{
			List<CandidatePointGroup> list = new List<CandidatePointGroup>();
			List<Vector3>[] array = CreatePositionBuckets(4);
			for (int i = 0; i < positions.Count; i++)
			{
				array[GetSphereVerticalSectorIndex(positions[i], center)].Add(positions[i]);
			}
			for (int j = 0; j < array.Length; j++)
			{
				list.AddRange(BuildAdaptiveCandidatePointGroups(array[j], spacing));
			}
			return list;
		}

		private static List<Vector3>[] CreatePositionBuckets(int count)
		{
			List<Vector3>[] array = new List<Vector3>[count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<Vector3>();
			}
			return array;
		}

		private static int GetNearestBoxFaceIndex(OrientedBox box, Vector3 position)
		{
			Vector3 vector = Quaternion.Inverse(box.Rotation) * (position - box.Center);
			Vector3 vector2 = box.Size * 0.5f;
			float num = ((vector2.x > 0f) ? Mathf.Abs(Mathf.Abs(vector.x) - vector2.x) : float.MaxValue);
			float num2 = ((vector2.y > 0f) ? Mathf.Abs(Mathf.Abs(vector.y) - vector2.y) : float.MaxValue);
			float num3 = ((vector2.z > 0f) ? Mathf.Abs(Mathf.Abs(vector.z) - vector2.z) : float.MaxValue);
			if (num3 <= num && num3 <= num2)
			{
				if (!(vector.z >= 0f))
				{
					return 1;
				}
				return 0;
			}
			if (num2 <= num)
			{
				if (!(vector.y >= 0f))
				{
					return 3;
				}
				return 2;
			}
			if (!(vector.x >= 0f))
			{
				return 5;
			}
			return 4;
		}

		private static int GetSphereVerticalSectorIndex(Vector3 position, Vector3 center)
		{
			Vector3 vector = position - center;
			if (vector.x * vector.x + vector.z * vector.z < 0.0001f)
			{
				return 0;
			}
			float num = Mathf.Atan2(vector.z, vector.x);
			if (num < 0f)
			{
				num += MathF.PI * 2f;
			}
			return Mathf.Clamp(Mathf.FloorToInt(num / (MathF.PI / 2f)), 0, 3);
		}

		private static List<CandidatePointGroup> BuildCandidatePointGroups(List<Vector3> positions, float spacing)
		{
			List<CandidatePointGroup> list = new List<CandidatePointGroup>();
			bool[] array = new bool[positions.Count];
			Queue<int> queue = new Queue<int>();
			float num = Mathf.Max(0.1f, spacing) * 1.5f;
			float num2 = num * num;
			for (int i = 0; i < positions.Count; i++)
			{
				if (array[i])
				{
					continue;
				}
				CandidatePointGroup item = default(CandidatePointGroup);
				array[i] = true;
				queue.Enqueue(i);
				while (queue.Count > 0)
				{
					int index = queue.Dequeue();
					Vector3 vector = positions[index];
					item.Add(vector);
					for (int j = 0; j < positions.Count; j++)
					{
						if (!array[j] && !((positions[j] - vector).sqrMagnitude > num2))
						{
							array[j] = true;
							queue.Enqueue(j);
						}
					}
				}
				list.Add(item);
			}
			return list;
		}

		private static List<CandidatePointGroup> BuildAdaptiveCandidatePointGroups(List<Vector3> positions, float spacing)
		{
			return BuildCandidatePointGroupsByDistance(positions, GetAdaptiveGroupDistance(positions, spacing));
		}

		private static List<CandidatePointGroup> BuildCandidatePointGroupsByDistance(List<Vector3> positions, float groupDistance)
		{
			List<CandidatePointGroup> list = new List<CandidatePointGroup>();
			bool[] array = new bool[positions.Count];
			Queue<int> queue = new Queue<int>();
			float num = groupDistance * groupDistance;
			for (int i = 0; i < positions.Count; i++)
			{
				if (array[i])
				{
					continue;
				}
				CandidatePointGroup item = default(CandidatePointGroup);
				array[i] = true;
				queue.Enqueue(i);
				while (queue.Count > 0)
				{
					int index = queue.Dequeue();
					Vector3 vector = positions[index];
					item.Add(vector);
					for (int j = 0; j < positions.Count; j++)
					{
						if (!array[j] && !((positions[j] - vector).sqrMagnitude > num))
						{
							array[j] = true;
							queue.Enqueue(j);
						}
					}
				}
				list.Add(item);
			}
			return list;
		}

		private static float GetAdaptiveGroupDistance(List<Vector3> positions, float spacing)
		{
			if (positions.Count <= 1)
			{
				return Mathf.Max(0.1f, spacing) * 1.5f;
			}
			List<float> list = new List<float>(positions.Count);
			for (int i = 0; i < positions.Count; i++)
			{
				float num = float.MaxValue;
				for (int j = 0; j < positions.Count; j++)
				{
					if (i != j)
					{
						float sqrMagnitude = (positions[i] - positions[j]).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
						}
					}
				}
				if (num < float.MaxValue)
				{
					list.Add(Mathf.Sqrt(num));
				}
			}
			if (list.Count == 0)
			{
				return Mathf.Max(0.1f, spacing) * 1.5f;
			}
			list.Sort();
			float num2 = list[list.Count / 2];
			return Mathf.Max(Mathf.Max(0.1f, spacing) * 1.5f, num2 * 2.75f);
		}

		private static bool IsBoxSurfaceGroupBoundary(OrientedBox box, Vector3 position, Settings settings)
		{
			if (!settings.CandidateSeparateSurfaceGroups || settings.PreviewShape != PlayerSafeZoneInteractionPointPreviewShape.Box)
			{
				return false;
			}
			Vector3 vector = Quaternion.Inverse(box.Rotation) * (position - box.Center);
			Vector3 vector2 = box.Size * 0.5f;
			int num = 0;
			if (Mathf.Abs(Mathf.Abs(vector.x) - vector2.x) <= 0.001f)
			{
				num++;
			}
			if (Mathf.Abs(Mathf.Abs(vector.y) - vector2.y) <= 0.001f)
			{
				num++;
			}
			if (Mathf.Abs(Mathf.Abs(vector.z) - vector2.z) <= 0.001f)
			{
				num++;
			}
			return num >= 2;
		}

		private static bool IsSphereSurfaceGroupBoundary(Vector3 position, Vector3 center, float radius, float spacing, Settings settings)
		{
			if (!settings.CandidateSeparateSurfaceGroups || settings.PreviewShape != PlayerSafeZoneInteractionPointPreviewShape.Sphere)
			{
				return false;
			}
			Vector3 vector = position - center;
			if (vector.x * vector.x + vector.z * vector.z < 0.0001f)
			{
				return false;
			}
			float num = Mathf.Atan2(vector.z, vector.x);
			if (num < 0f)
			{
				num += MathF.PI * 2f;
			}
			float num2 = MathF.PI / 2f;
			float num3 = Mathf.Min(num % num2, num2 - num % num2);
			float num4 = Mathf.Max(MathF.PI / 90f, spacing / Mathf.Max(0.1f, radius) * 0.5f);
			return num3 <= num4;
		}

		private static bool IsWithinVerticalClip(Vector3 localPosition, OrientedBox clipBox)
		{
			if (localPosition.y >= (0f - clipBox.Size.y) * 0.5f - 0.001f)
			{
				return localPosition.y <= clipBox.Size.y * 0.5f + 0.001f;
			}
			return false;
		}

		private static Vector2 GetBoxFaceSize(Vector3 size, int faceIndex)
		{
			switch (faceIndex)
			{
			case 0:
			case 1:
				return new Vector2(size.x, size.y);
			case 2:
			case 3:
				return new Vector2(size.x, size.z);
			default:
				return new Vector2(size.z, size.y);
			}
		}

		private static Vector3 GetBoxFaceLocalPoint(Vector3 size, float u, float v, int faceIndex)
		{
			Vector3 vector = size * 0.5f;
			float x = Mathf.Lerp(0f - vector.x, vector.x, u);
			float y = Mathf.Lerp(0f - vector.y, vector.y, v);
			float z = Mathf.Lerp(0f - vector.z, vector.z, v);
			float z2 = Mathf.Lerp(0f - vector.z, vector.z, u);
			return faceIndex switch
			{
				0 => new Vector3(x, y, vector.z), 
				1 => new Vector3(x, y, 0f - vector.z), 
				2 => new Vector3(x, vector.y, z), 
				3 => new Vector3(x, 0f - vector.y, z), 
				4 => new Vector3(vector.x, y, z2), 
				_ => new Vector3(0f - vector.x, y, z2), 
			};
		}

		private static OrientedBox GetPreviewBox(Bounds bounds, Settings settings)
		{
			Vector3 candidateBoxSize = settings.CandidateBoxSize;
			candidateBoxSize.x = Mathf.Max(0.1f, candidateBoxSize.x);
			candidateBoxSize.y = Mathf.Max(0.1f, candidateBoxSize.y);
			candidateBoxSize.z = Mathf.Max(0.1f, candidateBoxSize.z);
			return new OrientedBox(bounds.center, candidateBoxSize, Quaternion.Euler(0f, settings.CandidateBoxRotationY, 0f));
		}

		private static OrientedBox GetVerticalCoverageClippedBox(OrientedBox box, float verticalCoverage)
		{
			float num = Mathf.Clamp01(verticalCoverage);
			if (num >= 0.999f)
			{
				return box;
			}
			Vector3 size = box.Size;
			float num2 = Mathf.Max(0.05f, box.Size.y * 0.5f * num);
			size.y = num2 * 2f;
			return new OrientedBox(box.Center, size, box.Rotation);
		}

		private static void DrawPreviewSphereWireGizmos(Vector3 center, float radius, float verticalCoverage)
		{
			float num = Mathf.Clamp01(verticalCoverage);
			if (num >= 0.999f)
			{
				Gizmos.DrawWireSphere(center, radius);
				return;
			}
			DrawLatitudeWireCircle(center, radius, num);
			DrawLatitudeWireCircle(center, radius, 0f - num);
		}

		private static int GetSphereCandidatePointCount(float radius, float spacing)
		{
			float num = MathF.PI * 4f * radius * radius;
			float num2 = spacing * spacing;
			return Mathf.Clamp(Mathf.CeilToInt(num / num2), 1, 256);
		}

		private static float GetSphereEffectiveCandidateSpacing(float radius, float verticalCoverage, int pointCount, float fallbackSpacing)
		{
			if (pointCount <= 0)
			{
				return fallbackSpacing;
			}
			float num = MathF.PI * 4f * radius * radius * Mathf.Max(0.001f, verticalCoverage);
			return Mathf.Max(fallbackSpacing, Mathf.Sqrt(num / (float)pointCount));
		}

		private static int GetBoxCandidatePointCount(Vector3 size, float spacing)
		{
			float num = 2f * (size.x * size.y + size.x * size.z + size.y * size.z);
			float num2 = spacing * spacing;
			return Mathf.Clamp(Mathf.CeilToInt(num / num2), 1, 256);
		}

		private static Mesh GetPreviewSphereMesh(float verticalCoverage)
		{
			float num = Mathf.Clamp01(verticalCoverage);
			if (_previewSphereMesh != null && Mathf.Approximately(_previewSphereMeshCoverage, num))
			{
				return _previewSphereMesh;
			}
			if (_previewSphereMesh == null)
			{
				_previewSphereMesh = new Mesh
				{
					name = "PlayerSafeZone Preview Sphere",
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			else
			{
				_previewSphereMesh.Clear();
			}
			BuildPreviewSphereMesh(_previewSphereMesh, num);
			_previewSphereMeshCoverage = num;
			return _previewSphereMesh;
		}

		private static void BuildPreviewSphereMesh(Mesh mesh, float verticalCoverage)
		{
			float num = Mathf.Max(0.001f, verticalCoverage);
			int num2 = 13;
			int num3 = 33;
			Vector3[] array = new Vector3[num2 * num3];
			int[] array2 = new int[2304];
			for (int i = 0; i < num2; i++)
			{
				float t = (float)i / 12f;
				float num4 = Mathf.Lerp(0f - num, num, t);
				float num5 = Mathf.Sqrt(Mathf.Max(0f, 1f - num4 * num4));
				for (int j = 0; j < num3; j++)
				{
					float f = MathF.PI * 2f * (float)j / 32f;
					array[i * num3 + j] = new Vector3(Mathf.Cos(f) * num5, num4, Mathf.Sin(f) * num5);
				}
			}
			int num6 = 0;
			for (int k = 0; k < 12; k++)
			{
				for (int l = 0; l < 32; l++)
				{
					int num7 = k * num3 + l;
					int num8 = num7 + num3;
					array2[num6++] = num7;
					array2[num6++] = num8;
					array2[num6++] = num7 + 1;
					array2[num6++] = num7 + 1;
					array2[num6++] = num8;
					array2[num6++] = num8 + 1;
				}
			}
			mesh.vertices = array;
			mesh.triangles = array2;
			mesh.RecalculateNormals();
		}

		private static void DrawLatitudeWireCircle(Vector3 center, float radius, float normalizedY)
		{
			float y = normalizedY * radius;
			float num = Mathf.Sqrt(Mathf.Max(0f, 1f - normalizedY * normalizedY)) * radius;
			Vector3 vector = center + new Vector3(num, y, 0f);
			for (int i = 1; i <= 32; i++)
			{
				float f = MathF.PI * 2f * (float)i / 32f;
				Vector3 vector2 = center + new Vector3(Mathf.Cos(f) * num, y, Mathf.Sin(f) * num);
				Gizmos.DrawLine(vector, vector2);
				vector = vector2;
			}
		}

		private static Vector3 GetFibonacciSphereDirection(int index, int count)
		{
			if (count <= 1)
			{
				return Vector3.up;
			}
			float num = (float)index / ((float)count - 1f);
			float num2 = 1f - 2f * num;
			float num3 = Mathf.Sqrt(Mathf.Max(0f, 1f - num2 * num2));
			float f = 2.3999631f * (float)index;
			return new Vector3(Mathf.Cos(f) * num3, num2, Mathf.Sin(f) * num3);
		}

		private static int GetSettingsHash(Settings settings)
		{
			return (((((((((((((((int)(17 * 31 + settings.PreviewShape) * 31 + settings.CandidateHeightOffset.GetHashCode()) * 31 + settings.CandidateSphereRadius.GetHashCode()) * 31 + settings.CandidateBoxRotationY.GetHashCode()) * 31 + settings.CandidateBoxSize.GetHashCode()) * 31 + settings.CandidatePointSpacing.GetHashCode()) * 31 + settings.CandidatePointRadius.GetHashCode()) * 31 + settings.CandidateGroupPoints.GetHashCode()) * 31 + settings.CandidateSeparateSurfaceGroups.GetHashCode()) * 31 + settings.CandidateMaxGroupedPointsCount) * 31 + settings.CandidateVerticalCoverage.GetHashCode()) * 31 + settings.CandidateObstacleMask.value) * 31 + settings.CandidateFloorRayMaxDistance.GetHashCode()) * 31 + settings.CandidateObstacleProbeRadius.GetHashCode()) * 31 + settings.CandidateIgnoreTriggers.GetHashCode()) * 31 + settings.CandidateIgnoreOwnSafeZoneColliders.GetHashCode();
		}
	}
}
