using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Fusion;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AI;

namespace Features.SnakeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeController : NetworkBehaviour
	{
		private const float MIN_SEGMENT_LENGTH_SQR = 1E-08f;

		private const float MIN_MOVE_DIRECTION_SQR = 0.0001f;

		public const float HistoryInsertDistance = 0.15f;

		private static readonly ProfilerMarker _tickMarker = new ProfilerMarker("Snake.Controller.TickLate");

		private static readonly ProfilerMarker _crawlOverMarker = new ProfilerMarker("Snake.Controller.CrawlOver");

		private static readonly ProfilerMarker _applyBodyMarker = new ProfilerMarker("Snake.Controller.ApplyBody");

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[Tooltip("Faces NavMesh velocity so SnakeVisualSlither can use its right. Defaults to parent.")]
		[SerializeField]
		private Transform _visual;

		[SerializeField]
		private float _visualRotationSpeed = 8f;

		[SerializeField]
		private float _firstSegmentRotationSpeed = 8f;

		[Tooltip("Yaw slerp rate while SetForcedLookTarget is active (SafeZoneApproach). Independent of _visualRotationSpeed.")]
		[SerializeField]
		private float _forcedLookRotationSpeed = 6f;

		[Tooltip("How fast the forced look point follows the player. Damps network/position jitter. 0 = snap target each call.")]
		[SerializeField]
		private float _forcedLookTargetSmoothSpeed = 10f;

		[SerializeField]
		private float _bodySpacing = 0.5f;

		[SerializeField]
		private float _crawlOverRadius = 1f;

		[SerializeField]
		private float _crawlOverHeight = 0.4f;

		[SerializeField]
		private float _crawlOverMinArcDistance;

		[Tooltip("Arc step for gap probes. 0 = bodySpacing / 4.")]
		[SerializeField]
		private float _crawlOverProbeStep;

		[Tooltip("How far along the body a gap lift can reach neighboring cubes. 0 = bodySpacing.")]
		[SerializeField]
		private float _crawlOverGapSpan;

		[Tooltip("0 = neighbors only partially follow a lifting gap (falloff). 1 = neighbors fully match gap height within span.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _crawlOverNeighborInfluence = 1f;

		[SerializeField]
		private float _crawlOverSmoothSpeed = 8f;

		[SerializeField]
		private List<Transform> _bodyParts = new List<Transform>();

		[Tooltip("Parallel to _bodyParts. When a slot is grabbed, trail stops writing that segment.")]
		[SerializeField]
		private List<SimplePointGrabable> _bodyPartGrabables = new List<SimplePointGrabable>();

		[Tooltip("How strongly free neighbors follow a grabbed segment (0 = off, 1 = full spacing chain).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _grabNeighborInfluence = 0.65f;

		[Tooltip("Free segments on each side of a grab that are pulled (1 = immediate neighbors only). Farther segments stay on normal trail.")]
		[SerializeField]
		private int _grabInfluenceHops = 2;

		[Tooltip("Allowed chain length vs bodySpacing before hard clamp (1 = exact spacing).")]
		[SerializeField]
		private float _grabMaxStretch = 1.15f;

		[Tooltip("Seconds to soft-latch a grabbed segment trail → phys after latch. 0 = snap. Free segments ignore this.")]
		[SerializeField]
		private float _grabEnterBlendDuration = 1f;

		[Tooltip("Exponential soft-latch speed for grabbed segments (and optional influenced neighbors).")]
		[SerializeField]
		private float _grabPoseSmoothSpeed = 4f;

		[Tooltip("Seconds to ease only the previously grabbed segments back onto the trail. 0 = snap.")]
		[SerializeField]
		private float _grabReleaseBlendDuration = 0.8f;

		[Tooltip("Max distance from trail rest allowed as the release-blend start. Farther poses (stale RB / network coil) are clamped so UnJoin cannot teleport the tip to an unknown far point.")]
		[SerializeField]
		private float _grabReleaseMaxStartDistance = 8f;

		private readonly List<Vector3> _positionsHistory = new List<Vector3>();

		private readonly List<Vector3> _samplePoints = new List<Vector3>();

		private readonly List<Vector3> _resolvedPositions = new List<Vector3>();

		private readonly List<Vector3> _releaseBlendFromPositions = new List<Vector3>();

		private readonly List<bool> _grabParticipationMask = new List<bool>();

		private readonly List<bool> _releaseBlendMask = new List<bool>();

		private readonly List<float> _sampleHeights = new List<float>();

		private readonly List<float> _smoothedHeights = new List<float>();

		private readonly List<float> _probeArcs = new List<float>();

		private readonly List<float> _probeHeights = new List<float>();

		private readonly List<float> _historyArcs = new List<float>();

		private bool _wasAnyBodyPartGrabbed;

		private bool _isEnterBlendActive;

		private float _enterBlendElapsed;

		private bool _isReleaseBlendActive;

		private float _releaseBlendElapsed;

		private bool _hasForcedLookTarget;

		private Vector3 _forcedLookTarget;

		private bool _hasVisualRotationSpeedOverride;

		private float _visualRotationSpeedOverride;

		private bool _suppressVelocityVisualRotation;

		public IReadOnlyList<Transform> BodyParts => _bodyParts;

		public IReadOnlyList<SimplePointGrabable> BodyPartGrabables => _bodyPartGrabables;

		public float BodySpacing => _bodySpacing;

		public float CrawlOverRadius => _crawlOverRadius;

		public bool IsBodyPartGrabable(IPointGrabable grabable)
		{
			if (grabable == null)
			{
				return false;
			}
			for (int i = 0; i < _bodyPartGrabables.Count; i++)
			{
				if (_bodyPartGrabables[i] == grabable)
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetMaxGrabbedTrailStretch(out float stretch)
		{
			stretch = 0f;
			if (_bodyParts.Count == 0 || _samplePoints.Count != _bodyParts.Count)
			{
				return false;
			}
			bool flag = false;
			int num = Mathf.Min(_bodyParts.Count, _bodyPartGrabables.Count);
			for (int i = 0; i < num; i++)
			{
				if (IsBodyPartGrabbed(i) && TryGetBodyPartTrailStretch(i, out var stretch2) && (!flag || stretch2 > stretch))
				{
					stretch = stretch2;
					flag = true;
				}
			}
			return flag;
		}

		public void SetForcedLookTarget(Vector3 worldPosition)
		{
			if (!_hasForcedLookTarget)
			{
				_forcedLookTarget = worldPosition;
				_hasForcedLookTarget = true;
				return;
			}
			float num = Mathf.Max(0f, _forcedLookTargetSmoothSpeed);
			if (num <= 0f)
			{
				_forcedLookTarget = worldPosition;
				return;
			}
			float t = 1f - Mathf.Exp((0f - num) * Time.deltaTime);
			_forcedLookTarget = Vector3.Lerp(_forcedLookTarget, worldPosition, t);
		}

		public void ClearForcedLookTarget()
		{
			_hasForcedLookTarget = false;
			_forcedLookTarget = Vector3.zero;
		}

		public void SetVisualRotationSpeedOverride(float speed, bool suppressVelocityYaw = false)
		{
			_hasVisualRotationSpeedOverride = true;
			_visualRotationSpeedOverride = Mathf.Max(0f, speed);
			_suppressVelocityVisualRotation = suppressVelocityYaw;
		}

		public void ClearVisualRotationSpeedOverride()
		{
			_hasVisualRotationSpeedOverride = false;
			_visualRotationSpeedOverride = 0f;
			_suppressVelocityVisualRotation = false;
		}

		public void FaceVisualToward(Vector3 worldDirection)
		{
			Transform visual = GetVisual();
			if (!(visual == null))
			{
				worldDirection.y = 0f;
				if (!(worldDirection.sqrMagnitude < 0.0001f))
				{
					Quaternion b = Quaternion.LookRotation(worldDirection.normalized);
					float t = 1f - Mathf.Exp((0f - GetEffectiveVisualRotationSpeed()) * Time.deltaTime);
					visual.rotation = Quaternion.Slerp(visual.rotation, b, t);
				}
			}
		}

		public bool IsPointNearBodyXZ(Vector3 worldPoint, float radius, int ignoredHeadBodySegments = 2)
		{
			if (_bodyParts == null || _bodyParts.Count == 0 || radius <= 0f)
			{
				return false;
			}
			float num = radius * radius;
			for (int i = Mathf.Clamp(ignoredHeadBodySegments, 0, _bodyParts.Count); i < _bodyParts.Count; i++)
			{
				Transform transform = _bodyParts[i];
				if (!(transform == null))
				{
					float num2 = transform.position.x - worldPoint.x;
					float num3 = transform.position.z - worldPoint.z;
					if (num2 * num2 + num3 * num3 <= num)
					{
						return true;
					}
				}
			}
			return false;
		}

		public float GetMinDistanceToBodyXZ(Vector3 worldPoint, int ignoredHeadBodySegments = 0)
		{
			if (_bodyParts == null || _bodyParts.Count == 0)
			{
				return float.PositiveInfinity;
			}
			int num = Mathf.Clamp(ignoredHeadBodySegments, 0, _bodyParts.Count);
			float num2 = float.PositiveInfinity;
			bool flag = false;
			for (int i = num; i < _bodyParts.Count; i++)
			{
				Transform transform = _bodyParts[i];
				if (!(transform == null))
				{
					float num3 = transform.position.x - worldPoint.x;
					float num4 = transform.position.z - worldPoint.z;
					float num5 = num3 * num3 + num4 * num4;
					if (!(num5 >= num2))
					{
						num2 = num5;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return float.PositiveInfinity;
			}
			return Mathf.Sqrt(num2);
		}

		private void OnValidate()
		{
			SyncSegmentHitSurfaces();
		}

		private void Start()
		{
			SyncSegmentHitSurfaces();
			SeedHistoryBehindHead();
			ApplyBodyParts();
		}

		private void SyncSegmentHitSurfaces()
		{
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				Transform transform = _bodyParts[i];
				if (!(transform == null))
				{
					SnakeSegmentHitSurface component = transform.GetComponent<SnakeSegmentHitSurface>();
					if (!(component == null))
					{
						component.SetSegmentIndex(i);
					}
				}
			}
		}

		public void TickUpdate()
		{
			ApplyVisualRotation();
		}

		public void TickLate()
		{
			using (_tickMarker.Auto())
			{
				Vector3 trailHeadPosition = GetTrailHeadPosition();
				if (_positionsHistory.Count == 0)
				{
					_positionsHistory.Insert(0, trailHeadPosition);
				}
				else if (ShouldCommitHistorySample(trailHeadPosition))
				{
					_positionsHistory.Insert(0, trailHeadPosition);
				}
				else
				{
					_positionsHistory[0] = trailHeadPosition;
				}
				TrimHistory();
				using (_applyBodyMarker.Auto())
				{
					ApplyBodyParts();
				}
			}
		}

		private bool ShouldCommitHistorySample(Vector3 headPosition)
		{
			int index = ((_positionsHistory.Count > 1) ? 1 : 0);
			return Vector3.Distance(_positionsHistory[index], headPosition) > 0.15f;
		}

		public void ReseedBodyTrailFromHead()
		{
			SeedHistoryBehindHead();
			ApplyBodyParts();
		}

		private void SeedHistoryBehindHead()
		{
			Vector3 trailHeadPosition = GetTrailHeadPosition();
			Vector3 trailHeadForward = GetTrailHeadForward();
			_positionsHistory.Clear();
			_positionsHistory.Add(trailHeadPosition);
			for (int i = 1; i < _bodyParts.Count; i++)
			{
				_positionsHistory.Add(trailHeadPosition - trailHeadForward * _bodySpacing * i);
			}
		}

		private Vector3 GetTrailHeadPosition()
		{
			Transform visual = GetVisual();
			if (!(visual != null))
			{
				return base.transform.position;
			}
			return visual.position;
		}

		private Vector3 GetTrailHeadForward()
		{
			Transform visual = GetVisual();
			if (!(visual != null))
			{
				return base.transform.forward;
			}
			return visual.forward;
		}

		private Transform GetVisual()
		{
			if (_visual != null)
			{
				return _visual;
			}
			return base.transform.parent;
		}

		private void TrimHistory()
		{
			float num = Mathf.Max(_bodySpacing, (float)(_bodyParts.Count - 1) * _bodySpacing + _bodySpacing);
			float num2 = 0f;
			for (int i = 0; i < _positionsHistory.Count - 1; i++)
			{
				num2 += Vector3.Distance(_positionsHistory[i], _positionsHistory[i + 1]);
				if (!(num2 <= num))
				{
					_positionsHistory.RemoveRange(i + 2, _positionsHistory.Count - (i + 2));
					break;
				}
			}
		}

		private void ApplyBodyParts()
		{
			if (_positionsHistory.Count == 0)
			{
				return;
			}
			BuildHistoryArcs();
			BuildSamplePoints();
			using (_crawlOverMarker.Auto())
			{
				ComputeCrawlOverHeights();
			}
			if (HasAnyBodyPartGrabbed())
			{
				RefreshGrabParticipationMask();
				SetGrabbedSegmentsPhysicsBlocked(blocked: false);
				_isEnterBlendActive = false;
				_enterBlendElapsed = 0f;
				_wasAnyBodyPartGrabbed = true;
				_isReleaseBlendActive = false;
				_releaseBlendElapsed = 0f;
				for (int i = 0; i < _bodyParts.Count; i++)
				{
					if (!IsBodyPartGrabbed(i))
					{
						Transform transform = _bodyParts[i];
						if (transform != null)
						{
							ApplyTrailOnlyBodyPart(transform, i);
						}
					}
				}
			}
			else
			{
				if (_wasAnyBodyPartGrabbed)
				{
					SetGrabbedSegmentsPhysicsBlocked(blocked: false);
					_isEnterBlendActive = false;
					_enterBlendElapsed = 0f;
					BeginGrabReleaseBlend();
					_wasAnyBodyPartGrabbed = false;
				}
				if (_isReleaseBlendActive)
				{
					ApplyBodyPartsReleaseBlend();
				}
				else
				{
					ApplyBodyPartsFromTrailOnly();
				}
			}
		}

		private void ApplyBodyPartsWithGrabInfluence()
		{
			float num = AdvanceGrabEnterBlend();
			SetGrabbedSegmentsPhysicsBlocked(num < 1f);
			BuildResolvedGrabPins(num);
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				if (!IsBodyPartGrabbed(i))
				{
					continue;
				}
				Transform transform = _bodyParts[i];
				if (!(transform == null))
				{
					if (num < 1f)
					{
						ApplyGrabbedSegmentSoftLatch(transform, i, _resolvedPositions[i]);
					}
					else
					{
						_resolvedPositions[i] = transform.position;
					}
				}
			}
			if (_grabNeighborInfluence > 0f && _grabInfluenceHops > 0)
			{
				ApplyGrabNeighborConstraints();
			}
			for (int j = 0; j < _bodyParts.Count; j++)
			{
				Transform transform2 = _bodyParts[j];
				if (transform2 == null || IsBodyPartGrabbed(j))
				{
					continue;
				}
				if (_grabNeighborInfluence > 0f && IsWithinGrabInfluence(j))
				{
					Vector3 vector = _resolvedPositions[j];
					Vector3 target = vector;
					if (num < 1f)
					{
						Vector3 a = _samplePoints[j];
						a.y += _smoothedHeights[j];
						target = Vector3.Lerp(a, vector, num);
					}
					Vector3 vector2 = (transform2.position = SmoothToward(transform2.position, target, _grabPoseSmoothSpeed));
					if (j == 0)
					{
						ApplyFirstSegmentRotation(transform2);
						continue;
					}
					Vector3 vector4 = ResolveGrabInfluencedLookAt(j, num);
					if ((vector4 - vector2).sqrMagnitude > 0.0001f)
					{
						transform2.LookAt(vector4);
					}
				}
				else
				{
					ApplyTrailOnlyBodyPart(transform2, j);
				}
			}
		}

		private void RefreshGrabParticipationMask()
		{
			EnsureBoolListSize(_grabParticipationMask, _bodyParts.Count);
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				_grabParticipationMask[i] = IsBodyPartGrabbed(i);
			}
		}

		private bool IsWithinGrabInfluence(int index)
		{
			if (_grabInfluenceHops <= 0)
			{
				return false;
			}
			for (int i = 0; i < _bodyPartGrabables.Count; i++)
			{
				if (IsBodyPartGrabbed(i))
				{
					int num = Mathf.Abs(index - i);
					if (num > 0 && num <= _grabInfluenceHops)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void ApplyTrailOnlyBodyPart(Transform bodyPart, int index)
		{
			Vector3 vector = _samplePoints[index];
			vector.y += _smoothedHeights[index];
			bodyPart.position = vector;
			if (index == 0)
			{
				ApplyFirstSegmentRotation(bodyPart);
				SyncBodyPartRigidbody(index, bodyPart);
				return;
			}
			Vector3 vector2 = _samplePoints[index - 1];
			vector2.y += _smoothedHeights[index - 1];
			if ((vector2 - vector).sqrMagnitude > 0.0001f)
			{
				bodyPart.LookAt(vector2);
			}
			SyncBodyPartRigidbody(index, bodyPart);
		}

		private void SyncBodyPartRigidbody(int index, Transform bodyPart)
		{
			if (index < 0 || index >= _bodyPartGrabables.Count)
			{
				return;
			}
			SimplePointGrabable simplePointGrabable = _bodyPartGrabables[index];
			Rigidbody rigidbody = ((simplePointGrabable != null) ? simplePointGrabable.Rigidbody : null);
			if (!(rigidbody == null))
			{
				rigidbody.position = bodyPart.position;
				rigidbody.rotation = bodyPart.rotation;
				if (!rigidbody.isKinematic)
				{
					rigidbody.linearVelocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
				}
				rigidbody.PublishTransform();
			}
		}

		private void ApplyGrabbedSegmentSoftLatch(Transform bodyPart, int index, Vector3 latchedTarget)
		{
			Vector3 vector = SmoothToward(bodyPart.position, latchedTarget, _grabPoseSmoothSpeed);
			SimplePointGrabable simplePointGrabable = ((index < _bodyPartGrabables.Count) ? _bodyPartGrabables[index] : null);
			Rigidbody rigidbody = ((simplePointGrabable != null) ? simplePointGrabable.Rigidbody : null);
			if (rigidbody != null)
			{
				rigidbody.position = vector;
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}
			else
			{
				bodyPart.position = vector;
			}
			if (index >= 0 && index < _resolvedPositions.Count)
			{
				_resolvedPositions[index] = vector;
			}
		}

		private void BeginGrabEnterBlend()
		{
			_enterBlendElapsed = 0f;
			_isEnterBlendActive = _grabEnterBlendDuration > 0f;
			if (_isEnterBlendActive)
			{
				SetGrabbedSegmentsPhysicsBlocked(blocked: true);
			}
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				if (!IsBodyPartGrabbed(i))
				{
					continue;
				}
				Transform transform = _bodyParts[i];
				if (!(transform == null) && i < _samplePoints.Count)
				{
					Vector3 position = _samplePoints[i];
					if (i < _smoothedHeights.Count)
					{
						position.y += _smoothedHeights[i];
					}
					transform.position = position;
					SyncBodyPartRigidbody(i, transform);
				}
			}
		}

		private float AdvanceGrabEnterBlend()
		{
			if (!_isEnterBlendActive)
			{
				return 1f;
			}
			_enterBlendElapsed += Time.deltaTime;
			float num = Mathf.Max(0.0001f, _grabEnterBlendDuration);
			float num2 = Mathf.Clamp01(_enterBlendElapsed / num);
			float result = num2 * num2 * num2 * (num2 * (num2 * 6f - 15f) + 10f);
			if (num2 >= 1f)
			{
				_isEnterBlendActive = false;
				SetGrabbedSegmentsPhysicsBlocked(blocked: false);
			}
			return result;
		}

		private void SetGrabbedSegmentsPhysicsBlocked(bool blocked)
		{
			for (int i = 0; i < _bodyPartGrabables.Count; i++)
			{
				if (IsBodyPartGrabbed(i))
				{
					SimplePointGrabable simplePointGrabable = _bodyPartGrabables[i];
					if (!(simplePointGrabable?.GrabObject == null))
					{
						simplePointGrabable.GrabObject.GrabbingPhysicsBlocked = blocked;
					}
				}
			}
		}

		private static Vector3 SmoothToward(Vector3 current, Vector3 target, float speed)
		{
			if (speed <= 0f)
			{
				return target;
			}
			float t = 1f - Mathf.Exp((0f - speed) * Time.deltaTime);
			return Vector3.Lerp(current, target, t);
		}

		private Vector3 ResolveGrabInfluencedLookAt(int index, float enterBlend)
		{
			int index2 = index - 1;
			Vector3 vector = _resolvedPositions[index2];
			if (enterBlend >= 1f || IsBodyPartGrabbed(index2))
			{
				return vector;
			}
			Vector3 a = _samplePoints[index2];
			a.y += _smoothedHeights[index2];
			return Vector3.Lerp(a, vector, enterBlend);
		}

		private void BeginGrabReleaseBlend()
		{
			int count = _bodyParts.Count;
			EnsureListSize(_releaseBlendFromPositions, count);
			EnsureBoolListSize(_releaseBlendMask, count);
			EnsureBoolListSize(_grabParticipationMask, count);
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				bool flag2 = _grabParticipationMask[i];
				if (!flag2 && _grabNeighborInfluence > 0f && IsIndexWithinParticipationInfluence(i))
				{
					flag2 = true;
				}
				_releaseBlendMask[i] = flag2;
				if (flag2)
				{
					flag = true;
				}
				Vector3 trailRestPoint = GetTrailRestPoint(i);
				Vector3 vector = ResolveReleaseBlendStartPose(i, trailRestPoint);
				_releaseBlendFromPositions[i] = vector;
				Transform transform = _bodyParts[i];
				if (flag2 && transform != null)
				{
					transform.position = vector;
					SyncBodyPartRigidbody(i, transform);
				}
			}
			_releaseBlendElapsed = 0f;
			_isReleaseBlendActive = flag && _grabReleaseBlendDuration > 0f;
			if (!flag || _isReleaseBlendActive)
			{
				return;
			}
			for (int j = 0; j < count; j++)
			{
				if (j < _releaseBlendMask.Count && _releaseBlendMask[j])
				{
					Transform transform2 = _bodyParts[j];
					if (!(transform2 == null))
					{
						transform2.position = GetTrailRestPoint(j);
						SyncBodyPartRigidbody(j, transform2);
					}
				}
			}
		}

		private Vector3 GetTrailRestPoint(int index)
		{
			if (index < 0 || index >= _samplePoints.Count)
			{
				return Vector3.zero;
			}
			Vector3 result = _samplePoints[index];
			if (index < _smoothedHeights.Count)
			{
				result.y += _smoothedHeights[index];
			}
			return result;
		}

		private Vector3 ResolveReleaseBlendStartPose(int index, Vector3 trailPoint)
		{
			Transform transform = ((index >= 0 && index < _bodyParts.Count) ? _bodyParts[index] : null);
			Vector3 vector = ((transform != null) ? transform.position : trailPoint);
			Vector3 vector2 = vector;
			if (index >= 0 && index < _bodyPartGrabables.Count)
			{
				SimplePointGrabable simplePointGrabable = _bodyPartGrabables[index];
				Rigidbody rigidbody = ((simplePointGrabable != null) ? simplePointGrabable.Rigidbody : null);
				if (rigidbody != null)
				{
					vector2 = rigidbody.position;
				}
			}
			float num = Vector3.Distance(vector, trailPoint);
			float num2 = Vector3.Distance(vector2, trailPoint);
			Vector3 vector3 = ((num <= num2) ? vector : vector2);
			float num3 = Mathf.Max(0.5f, _grabReleaseMaxStartDistance);
			if (Vector3.Distance(vector3, trailPoint) > num3)
			{
				vector3 = Vector3.MoveTowards(trailPoint, vector3, num3);
			}
			return vector3;
		}

		private bool IsIndexWithinParticipationInfluence(int index)
		{
			if (_grabInfluenceHops <= 0)
			{
				return false;
			}
			for (int i = 0; i < _grabParticipationMask.Count; i++)
			{
				if (_grabParticipationMask[i])
				{
					int num = Mathf.Abs(index - i);
					if (num > 0 && num <= _grabInfluenceHops)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void ApplyBodyPartsReleaseBlend()
		{
			_releaseBlendElapsed += Time.deltaTime;
			float num = Mathf.Max(0.0001f, _grabReleaseBlendDuration);
			float num2 = Mathf.Clamp01(_releaseBlendElapsed / num);
			float t = num2 * num2 * (3f - 2f * num2);
			EnsureListSize(_releaseBlendFromPositions, _bodyParts.Count);
			EnsureBoolListSize(_releaseBlendMask, _bodyParts.Count);
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				Transform transform = _bodyParts[i];
				if (transform == null)
				{
					continue;
				}
				if (i >= _releaseBlendMask.Count || !_releaseBlendMask[i])
				{
					ApplyTrailOnlyBodyPart(transform, i);
					continue;
				}
				Vector3 trailRestPoint = GetTrailRestPoint(i);
				Vector3 vector = (transform.position = Vector3.Lerp(_releaseBlendFromPositions[i], trailRestPoint, t));
				if (i == 0)
				{
					ApplyFirstSegmentRotation(transform);
					SyncBodyPartRigidbody(i, transform);
					continue;
				}
				Vector3 trailRestPoint2 = GetTrailRestPoint(i - 1);
				Vector3 vector3 = trailRestPoint2;
				if (i - 1 < _releaseBlendMask.Count && _releaseBlendMask[i - 1])
				{
					vector3 = Vector3.Lerp(_releaseBlendFromPositions[i - 1], trailRestPoint2, t);
				}
				if ((vector3 - vector).sqrMagnitude > 0.0001f)
				{
					transform.LookAt(vector3);
				}
				SyncBodyPartRigidbody(i, transform);
			}
			if (num2 >= 1f)
			{
				_isReleaseBlendActive = false;
			}
		}

		private void ApplyBodyPartsFromTrailOnly()
		{
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				Transform transform = _bodyParts[i];
				if (!(transform == null))
				{
					ApplyTrailOnlyBodyPart(transform, i);
				}
			}
		}

		private void BuildResolvedGrabPins(float enterBlend)
		{
			int count = _bodyParts.Count;
			EnsureListSize(_resolvedPositions, count);
			for (int i = 0; i < count; i++)
			{
				Transform transform = _bodyParts[i];
				if (transform == null)
				{
					_resolvedPositions[i] = ((_samplePoints.Count > i) ? _samplePoints[i] : Vector3.zero);
				}
				else if (IsBodyPartGrabbed(i))
				{
					Vector3 position = transform.position;
					if (enterBlend < 1f && i < _samplePoints.Count)
					{
						Vector3 a = _samplePoints[i];
						if (i < _smoothedHeights.Count)
						{
							a.y += _smoothedHeights[i];
						}
						_resolvedPositions[i] = Vector3.Lerp(a, position, enterBlend);
					}
					else
					{
						_resolvedPositions[i] = position;
					}
				}
				else
				{
					Vector3 value = _samplePoints[i];
					value.y += _smoothedHeights[i];
					_resolvedPositions[i] = value;
				}
			}
		}

		private void ApplyGrabNeighborConstraints()
		{
			if (_grabNeighborInfluence <= 0f || _grabInfluenceHops <= 0)
			{
				return;
			}
			int count = _bodyParts.Count;
			float maxSegmentLength = Mathf.Max(0.01f, _bodySpacing * Mathf.Max(1f, _grabMaxStretch));
			for (int i = 0; i < count; i++)
			{
				if (!IsBodyPartGrabbed(i))
				{
					continue;
				}
				int num = i - 1;
				while (num >= 0 && !IsBodyPartGrabbed(num))
				{
					int num2 = i - num;
					if (num2 > _grabInfluenceHops)
					{
						break;
					}
					PullResolvedTowardNeighbor(num, num + 1, num2, maxSegmentLength);
					num--;
				}
				for (int j = i + 1; j < count && !IsBodyPartGrabbed(j); j++)
				{
					int num3 = j - i;
					if (num3 > _grabInfluenceHops)
					{
						break;
					}
					PullResolvedTowardNeighbor(j, j - 1, num3, maxSegmentLength);
				}
			}
		}

		private void PullResolvedTowardNeighbor(int index, int anchorIndex, int hops, float maxSegmentLength)
		{
			Vector3 vector = _resolvedPositions[index];
			Vector3 vector2 = _resolvedPositions[anchorIndex];
			Vector3 vector3 = vector - vector2;
			float magnitude = vector3.magnitude;
			Vector3 b;
			if (magnitude * magnitude < 1E-08f)
			{
				Vector3 vector4 = _samplePoints[index] - _samplePoints[anchorIndex];
				if (vector4.sqrMagnitude < 1E-08f)
				{
					return;
				}
				b = vector2 + vector4.normalized * _bodySpacing;
			}
			else
			{
				float num;
				if (magnitude <= maxSegmentLength)
				{
					num = magnitude;
				}
				else
				{
					float t = Mathf.Clamp01((magnitude - maxSegmentLength) / maxSegmentLength);
					num = Mathf.Lerp(maxSegmentLength, _bodySpacing, t);
				}
				b = vector2 + vector3 / magnitude * num;
			}
			float value = 1f - (float)(hops - 1) / (float)_grabInfluenceHops;
			float t2 = _grabNeighborInfluence * Mathf.Clamp01(value);
			_resolvedPositions[index] = Vector3.Lerp(vector, b, t2);
		}

		private bool HasAnyBodyPartGrabbed()
		{
			for (int i = 0; i < _bodyPartGrabables.Count; i++)
			{
				if (IsBodyPartGrabbed(i))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBodyPartGrabbed(int index)
		{
			if (index < 0 || index >= _bodyPartGrabables.Count)
			{
				return false;
			}
			SimplePointGrabable simplePointGrabable = _bodyPartGrabables[index];
			if (simplePointGrabable != null && simplePointGrabable.Initialized)
			{
				return simplePointGrabable.GrabbedByPlayers.Count > 0;
			}
			return false;
		}

		private bool TryGetBodyPartTrailStretch(int index, out float stretch)
		{
			stretch = 0f;
			if (index < 0 || index >= _bodyParts.Count || index >= _samplePoints.Count)
			{
				return false;
			}
			Transform transform = _bodyParts[index];
			if (transform == null)
			{
				return false;
			}
			Vector3 b = _samplePoints[index];
			if (index < _smoothedHeights.Count)
			{
				b.y += _smoothedHeights[index];
			}
			stretch = Vector3.Distance(transform.position, b);
			return true;
		}

		private static void EnsureListSize(List<Vector3> list, int size)
		{
			while (list.Count < size)
			{
				list.Add(Vector3.zero);
			}
			if (list.Count > size)
			{
				list.RemoveRange(size, list.Count - size);
			}
		}

		private static void EnsureBoolListSize(List<bool> list, int size)
		{
			while (list.Count < size)
			{
				list.Add(item: false);
			}
			if (list.Count > size)
			{
				list.RemoveRange(size, list.Count - size);
			}
		}

		private void ApplyFirstSegmentRotation(Transform bodyPart)
		{
			Vector3 direction;
			if (_hasForcedLookTarget)
			{
				if (!TryGetForcedLookDirectionFrom(bodyPart.position, out direction))
				{
					return;
				}
			}
			else if (!TryGetFirstSegmentDirection(out direction))
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(direction);
			if (!base.HasStateAuthority)
			{
				bodyPart.rotation = quaternion;
				return;
			}
			float num = (_hasForcedLookTarget ? _forcedLookRotationSpeed : _firstSegmentRotationSpeed);
			float t = 1f - Mathf.Exp((0f - num) * Time.deltaTime);
			bodyPart.rotation = Quaternion.Slerp(bodyPart.rotation, quaternion, t);
		}

		private bool TryGetFirstSegmentDirection(out Vector3 direction)
		{
			direction = Vector3.zero;
			if (_samplePoints.Count >= 2)
			{
				Vector3 vector = _samplePoints[0] - _samplePoints[1];
				vector.y = 0f;
				if (vector.sqrMagnitude >= 0.0001f)
				{
					direction = vector.normalized;
					return true;
				}
			}
			return TryGetWorldMoveDirection(out direction);
		}

		private bool TryGetWorldMoveDirection(out Vector3 direction)
		{
			direction = Vector3.zero;
			if (_positionsHistory.Count < 2)
			{
				return false;
			}
			Vector3 vector = _positionsHistory[0] - _positionsHistory[1];
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			direction = vector.normalized;
			return true;
		}

		private void ApplyVisualRotation()
		{
			if (!base.HasStateAuthority || _suppressVelocityVisualRotation)
			{
				return;
			}
			Transform visual = GetVisual();
			if (visual == null)
			{
				return;
			}
			Vector3 direction;
			if (_hasForcedLookTarget)
			{
				if (TryGetForcedLookDirectionFrom(visual.position, out direction))
				{
					Quaternion b = Quaternion.LookRotation(direction);
					float t = 1f - Mathf.Exp((0f - _forcedLookRotationSpeed) * Time.deltaTime);
					visual.rotation = Quaternion.Slerp(visual.rotation, b, t);
				}
			}
			else if (TryGetAgentVelocityDirection(out direction))
			{
				Quaternion b2 = Quaternion.LookRotation(direction);
				float t2 = 1f - Mathf.Exp((0f - GetEffectiveVisualRotationSpeed()) * Time.deltaTime);
				visual.rotation = Quaternion.Slerp(visual.rotation, b2, t2);
			}
		}

		private float GetEffectiveVisualRotationSpeed()
		{
			if (!_hasVisualRotationSpeedOverride)
			{
				return _visualRotationSpeed;
			}
			return _visualRotationSpeedOverride;
		}

		private bool TryGetForcedLookDirectionFrom(Vector3 fromPosition, out Vector3 direction)
		{
			direction = Vector3.zero;
			if (!_hasForcedLookTarget)
			{
				return false;
			}
			Vector3 vector = _forcedLookTarget - fromPosition;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			direction = vector.normalized;
			return true;
		}

		private bool TryGetAgentVelocityDirection(out Vector3 direction)
		{
			direction = Vector3.zero;
			if (_navMeshAgent == null || !_navMeshAgent.isActiveAndEnabled)
			{
				return false;
			}
			Vector3 velocity = _navMeshAgent.velocity;
			velocity.y = 0f;
			if (velocity.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			direction = velocity.normalized;
			return true;
		}

		private void BuildSamplePoints()
		{
			_samplePoints.Clear();
			for (int i = 0; i < _bodyParts.Count; i++)
			{
				_samplePoints.Add(GetPointAtDistance(_bodySpacing * (float)i));
			}
		}

		private void ComputeCrawlOverHeights()
		{
			int count = _samplePoints.Count;
			EnsureListSize(_sampleHeights, count);
			EnsureListSize(_smoothedHeights, count);
			float minArcDistance = ((_crawlOverMinArcDistance > 0f) ? _crawlOverMinArcDistance : (_bodySpacing * 2f));
			float probeStep = ((_crawlOverProbeStep > 0f) ? _crawlOverProbeStep : (_bodySpacing * 0.25f));
			float gapSpan = ((_crawlOverGapSpan > 0f) ? _crawlOverGapSpan : _bodySpacing);
			float radiusSqr = _crawlOverRadius * _crawlOverRadius;
			float totalArc = Mathf.Max(0f, (float)(_bodyParts.Count - 1) * _bodySpacing);
			BuildProbeArcs(totalArc, probeStep);
			ComputeProbeHeights(minArcDistance, radiusSqr);
			FoldProbeHeightsOntoSamples(count, gapSpan);
			float maxDelta = _crawlOverSmoothSpeed * Time.deltaTime;
			for (int i = 0; i < count; i++)
			{
				_smoothedHeights[i] = Mathf.MoveTowards(_smoothedHeights[i], _sampleHeights[i], maxDelta);
			}
		}

		private void BuildProbeArcs(float totalArc, float probeStep)
		{
			_probeArcs.Clear();
			if (totalArc <= 0f || probeStep <= 0f)
			{
				_probeArcs.Add(0f);
				return;
			}
			for (float num = 0f; num < totalArc; num += probeStep)
			{
				_probeArcs.Add(num);
			}
			if (_probeArcs[_probeArcs.Count - 1] < totalArc - 0.0001f)
			{
				_probeArcs.Add(totalArc);
			}
		}

		private void ComputeProbeHeights(float minArcDistance, float radiusSqr)
		{
			EnsureListSize(_probeHeights, _probeArcs.Count);
			for (int i = 0; i < _probeArcs.Count; i++)
			{
				_probeHeights[i] = 0f;
			}
			for (int num = _probeArcs.Count - 1; num >= 0; num--)
			{
				float num2 = _probeArcs[num];
				Vector3 pointAtDistance = GetPointAtDistance(num2);
				_probeHeights[num] = GetCrawlHeightAgainstPath(pointAtDistance, num2 + minArcDistance, radiusSqr);
			}
		}

		private void FoldProbeHeightsOntoSamples(int sampleCount, float gapSpan)
		{
			for (int i = 0; i < sampleCount; i++)
			{
				_sampleHeights[i] = 0f;
			}
			if (gapSpan <= 0f)
			{
				gapSpan = _bodySpacing;
			}
			for (int j = 0; j < _probeArcs.Count; j++)
			{
				float num = _probeHeights[j];
				if (num <= 0f)
				{
					continue;
				}
				float num2 = _probeArcs[j];
				for (int k = 0; k < sampleCount; k++)
				{
					float num3 = Mathf.Abs(_bodySpacing * (float)k - num2);
					if (!(num3 > gapSpan))
					{
						float num4 = Mathf.Lerp(1f - num3 / gapSpan, 1f, _crawlOverNeighborInfluence);
						_sampleHeights[k] = Mathf.Max(_sampleHeights[k], num * num4);
					}
				}
			}
		}

		private float GetCrawlHeightAgainstPath(Vector3 point, float minArcAllowed, float radiusSqr)
		{
			float num = 0f;
			int num2 = _positionsHistory.Count - 1;
			if (num2 < 1)
			{
				return 0f;
			}
			int i;
			for (i = FindArcSegment(_historyArcs, num2, minArcAllowed); i < num2 && _historyArcs[i + 1] <= minArcAllowed; i++)
			{
			}
			for (int j = i; j < num2; j++)
			{
				Vector3 a = _positionsHistory[j];
				Vector3 b = _positionsHistory[j + 1];
				float num3 = _historyArcs[j];
				float num4 = _historyArcs[j + 1] - num3;
				if (!(num4 * num4 < 1E-08f))
				{
					float num5 = 0f;
					if (num3 < minArcAllowed)
					{
						num5 = (minArcAllowed - num3) / num4;
					}
					Vector3 a2 = Vector3.Lerp(a, b, num5);
					float closestTOnSegmentXZ = GetClosestTOnSegmentXZ(point, a2, b);
					if (!(SqrDistancePointToSegmentXZ(point, a2, b, closestTOnSegmentXZ) > radiusSqr))
					{
						float arc = num3 + Mathf.Lerp(num5, 1f, closestTOnSegmentXZ) * num4;
						float probeHeightAtArc = GetProbeHeightAtArc(arc);
						num = Mathf.Max(num, probeHeightAtArc + _crawlOverHeight);
					}
				}
			}
			return num;
		}

		private float GetProbeHeightAtArc(float arc)
		{
			int num = _probeArcs.Count - 1;
			if (num < 0)
			{
				return 0f;
			}
			if (num == 0)
			{
				return _probeHeights[0];
			}
			if (arc <= _probeArcs[0])
			{
				return _probeHeights[0];
			}
			if (arc >= _probeArcs[num])
			{
				return _probeHeights[num];
			}
			int num2 = FindArcSegment(_probeArcs, num, arc);
			float num3 = _probeArcs[num2];
			float num4 = _probeArcs[num2 + 1];
			float t = ((num4 > num3) ? ((arc - num3) / (num4 - num3)) : 0f);
			return Mathf.Lerp(_probeHeights[num2], _probeHeights[num2 + 1], t);
		}

		private static float GetClosestTOnSegmentXZ(Vector3 point, Vector3 a, Vector3 b)
		{
			float num = b.x - a.x;
			float num2 = b.z - a.z;
			float num3 = num * num + num2 * num2;
			if (num3 < 1E-08f)
			{
				return 0f;
			}
			float num4 = point.x - a.x;
			float num5 = point.z - a.z;
			return Mathf.Clamp01((num4 * num + num5 * num2) / num3);
		}

		private static float SqrDistancePointToSegmentXZ(Vector3 point, Vector3 a, Vector3 b, float t)
		{
			float num = a.x + (b.x - a.x) * t - point.x;
			float num2 = a.z + (b.z - a.z) * t - point.z;
			return num * num + num2 * num2;
		}

		private static void EnsureListSize(List<float> list, int size)
		{
			while (list.Count < size)
			{
				list.Add(0f);
			}
			if (list.Count > size)
			{
				list.RemoveRange(size, list.Count - size);
			}
		}

		private void BuildHistoryArcs()
		{
			int count = _positionsHistory.Count;
			EnsureListSize(_historyArcs, count);
			if (count != 0)
			{
				_historyArcs[0] = 0f;
				for (int i = 1; i < count; i++)
				{
					_historyArcs[i] = _historyArcs[i - 1] + Vector3.Distance(_positionsHistory[i - 1], _positionsHistory[i]);
				}
			}
		}

		private static int FindArcSegment(List<float> ascendingArcs, int lastIndex, float distance)
		{
			int num = 0;
			int num2 = lastIndex - 1;
			while (num < num2)
			{
				int num3 = num + num2 >> 1;
				if (ascendingArcs[num3 + 1] >= distance)
				{
					num2 = num3;
				}
				else
				{
					num = num3 + 1;
				}
			}
			return num;
		}

		private Vector3 GetPointAtDistance(float targetDistance)
		{
			if (targetDistance <= 0f || _positionsHistory.Count == 1)
			{
				return _positionsHistory[0];
			}
			int num = _positionsHistory.Count - 1;
			if (targetDistance > _historyArcs[num])
			{
				return _positionsHistory[num];
			}
			int num2 = FindArcSegment(_historyArcs, num, targetDistance);
			float num3 = _historyArcs[num2];
			float num4 = _historyArcs[num2 + 1] - num3;
			if (num4 <= 0f)
			{
				return _positionsHistory[num2];
			}
			float t = (targetDistance - num3) / num4;
			return Vector3.Lerp(_positionsHistory[num2], _positionsHistory[num2 + 1], t);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
