using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.QuotaModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SwimmingModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class SwimmingComponent : NetworkBehaviour
	{
		private const int DefaultObstacleMaskBits = 1358954497;

		[SerializeField]
		private LayerMask _waterLayerMask;

		[SerializeField]
		private LayerMask _obstacleLayerMask = 1358954497;

		[SerializeField]
		private float _verticalPoitFinderOffset = 1f;

		[SerializeField]
		private float _verticalOffset = 0.67f;

		[SerializeField]
		private float _findDistance = 10f;

		[SerializeField]
		private Transform _findPoint;

		[SerializeField]
		private GrabObjectBase _grabObject;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private GameObject _grabberPrefab;

		[SerializeField]
		private float _amplitude = 0.11f;

		[SerializeField]
		private float _frequency = 2.5f;

		[SerializeField]
		private float _flowSpeed = 1f;

		[SerializeField]
		private float _pointReachDistance = 0.3f;

		[SerializeField]
		private float _cellSize = 0.35f;

		[SerializeField]
		private float _agentRadius = 0.35f;

		[SerializeField]
		private float _stuckTime = 0.75f;

		[SerializeField]
		private float _maxGrabberLeadDistance = 0.6f;

		[SerializeField]
		private float _unstickOffset = 2.25f;

		[SerializeField]
		private float _unstickDuration = 0.85f;

		[SerializeField]
		private float _unstickProgressEpsilon = 0.12f;

		[SerializeField]
		private float _grabberDisableDelay = 1f;

		[SerializeField]
		private List<SwimmingPointType> _swimmingPointsSequense;

		private GameObject _source;

		[SerializeField]
		private Transform _customHandle;

		[SerializeField]
		private float _minRotationSpeed = 0.1f;

		[SerializeField]
		private float _maxRotationSpeed = 0.4f;

		[SerializeField]
		private bool _isRotate;

		private float _rotationSpeed;

		private Vector3 _rotationDirection;

		private PhysGrabber _physGrabber;

		private SwimmingFlowPointModel _swimmingFlowPointModel;

		private SwimGridPathfinder _swimGridPathfinder;

		private static readonly Vector2[] UnstickDirectionOffsets = new Vector2[11]
		{
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(1f, 0.5f),
			new Vector2(-1f, 0.5f),
			new Vector2(1f, -0.5f),
			new Vector2(-1f, -0.5f),
			new Vector2(0.5f, -1f),
			new Vector2(-0.5f, -1f),
			new Vector2(0f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, 1f)
		};

		private int _currentPointIndex;

		private bool _shouldReset;

		private QuotaContainerModel _quotaContainerModel;

		private bool _isInitialized;

		private readonly List<Vector3> _path = new List<Vector3>();

		private int _pathIndex;

		private int _pathWaypointIndex = -1;

		private float _stuckTimer;

		private float _bestProgressDistance = float.MaxValue;

		private bool _isUnsticking;

		private float _unstickTimer;

		private Vector3 _unstickTarget;

		private int _unstickDirectionIndex = -1;

		private float _waypointDistanceAtUnstickStart;

		private float _grabberDisableTimer;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsSwimmingDisabled", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsSwimmingDisabled;

		public bool ShouldReset => _shouldReset;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsSwimmingDisabled
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SwimmingComponent.IsSwimmingDisabled. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SwimmingComponent.IsSwimmingDisabled. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Inject]
		private void InjectDependencies(SwimmingFlowPointModel swimmingFlowPointModel, QuotaContainerModel quotaContainerModel, SwimGridPathfinder swimGridPathfinder)
		{
			_swimmingFlowPointModel = swimmingFlowPointModel;
			_quotaContainerModel = quotaContainerModel;
			_swimGridPathfinder = swimGridPathfinder;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		public bool GetWaterPosition(out Vector3 position)
		{
			if (Physics.Raycast(_findPoint.position + Vector3.up * _verticalPoitFinderOffset, Vector3.down, out var hitInfo, _findDistance, _waterLayerMask))
			{
				_source = hitInfo.collider.gameObject;
				position = hitInfo.point;
				return true;
			}
			position = Vector3.zero;
			return false;
		}

		private void Update()
		{
			if (base.Object == null || !base.Object.IsValid || !_isInitialized || !base.HasStateAuthority)
			{
				return;
			}
			bool flag = false;
			foreach (SimplePointGrabable connectedGrabable in _simplePointGrabable.ConnectedGrabables)
			{
				if (connectedGrabable.GrabbedByPlayers.Count > 0)
				{
					flag = true;
					break;
				}
			}
			Vector3 position;
			bool flag2 = !GetWaterPosition(out position) || IsSwimmingDisabled || _simplePointGrabable.GrabbedByPlayersCount > 0 || flag || _source == null || _swimmingPointsSequense == null || _swimmingPointsSequense.Count == 0;
			foreach (QuotaContainerItemData freeContainerItem in _quotaContainerModel.FreeContainerItems)
			{
				if (freeContainerItem.PointGrabable == _simplePointGrabable)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				_shouldReset = true;
				if (!(_physGrabber == null))
				{
					_grabberDisableTimer += Time.deltaTime;
					if (_grabberDisableTimer >= _grabberDisableDelay)
					{
						ResetSwimming();
					}
				}
				return;
			}
			_shouldReset = false;
			_grabberDisableTimer = 0f;
			if (base.Runner == null)
			{
				return;
			}
			if (_physGrabber == null)
			{
				Vector3 position2 = _grabObject.transform.position;
				position2.y = position.y + _verticalOffset;
				_rotationDirection = UnityEngine.Random.onUnitSphere;
				_rotationSpeed = UnityEngine.Random.Range(_minRotationSpeed, _maxRotationSpeed);
				if (base.Runner.TrySpawn(_grabberPrefab, out NetworkObject obj, (Vector3?)position2, (Quaternion?)null, (PlayerRef?)null, (NetworkRunner.OnBeforeSpawned)null, (NetworkSpawnFlags)0) != NetworkSpawnStatus.Spawned)
				{
					return;
				}
				_physGrabber = obj.GetComponent<PhysGrabber>();
				if (!_grabObject.Grabbers.Contains(_physGrabber))
				{
					if (_customHandle != null)
					{
						_physGrabber.physGrabPoints.Add(_grabObject, _customHandle);
					}
					else
					{
						_physGrabber.physGrabPoints.Add(_grabObject, _simplePointGrabable.GetNearestHandle(position2));
					}
					_grabObject.Grabbers.Add(_physGrabber);
				}
			}
			MoveByPoints(position);
		}

		private void MoveByPoints(Vector3 waterPosition)
		{
			if (_physGrabber == null)
			{
				return;
			}
			if (_currentPointIndex < 0 || _currentPointIndex >= _swimmingPointsSequense.Count)
			{
				_currentPointIndex = 0;
			}
			SwimmingPointType swimmingPointType = _swimmingPointsSequense[_currentPointIndex];
			if (!_swimmingFlowPointModel.TryGetPoint(_source, swimmingPointType, out var transform))
			{
				return;
			}
			Vector3 position = transform.position;
			Vector3 followWorldPosition = GetFollowWorldPosition();
			Vector3 currentPos = _physGrabber.transform.position;
			if (_isUnsticking)
			{
				TickUnstick(followWorldPosition, position);
			}
			if (!_isUnsticking && (_path.Count == 0 || _pathIndex >= _path.Count || _pathWaypointIndex != _currentPointIndex))
			{
				RebuildPath(followWorldPosition, position);
			}
			Vector3 vector = ResolveSteerTarget(position);
			Vector3 vector2 = new Vector3(vector.x, currentPos.y, vector.z) - currentPos;
			vector2.y = 0f;
			float num = HorizontalDistance(followWorldPosition, _isUnsticking ? _unstickTarget : vector);
			float progressDistance = (_isUnsticking ? HorizontalDistance(followWorldPosition, position) : num);
			UpdateStuckState(progressDistance, followWorldPosition, position);
			float num2 = (_isUnsticking ? (_maxGrabberLeadDistance * 2f) : _maxGrabberLeadDistance);
			float num3 = HorizontalDistance(currentPos, followWorldPosition);
			if (!_isUnsticking && num3 > num2)
			{
				Vector3 vector3 = new Vector3(followWorldPosition.x - currentPos.x, 0f, followWorldPosition.z - currentPos.z);
				float a = num3 - num2;
				currentPos += vector3.normalized * Mathf.Min(a, _flowSpeed * Time.deltaTime * 2f);
			}
			else if (vector2.sqrMagnitude > 0.0001f)
			{
				float b = (_isUnsticking ? float.MaxValue : Mathf.Max(0f, num2 - num3));
				float num4 = Mathf.Min(_flowSpeed * Time.deltaTime * (_isUnsticking ? 2.25f : 1f), b);
				if (num4 > 0f && !float.IsInfinity(num4))
				{
					Vector3 vector4 = vector2.normalized * num4;
					if (vector4.sqrMagnitude > vector2.sqrMagnitude)
					{
						vector4 = vector2;
					}
					currentPos += vector4;
				}
			}
			ApplyVerticalBobbing(ref currentPos, waterPosition);
			_physGrabber.transform.position = currentPos;
			if (_isRotate)
			{
				_grabObject.Rigidbody.AddTorque(_rotationDirection * _rotationSpeed, ForceMode.VelocityChange);
			}
			if (_isUnsticking)
			{
				return;
			}
			float num5 = HorizontalDistance(currentPos, vector);
			if (_path.Count > 0 && num5 <= _pointReachDistance && _pathIndex < _path.Count - 1)
			{
				_pathIndex++;
				_bestProgressDistance = float.MaxValue;
				_stuckTimer = 0f;
			}
			if (HorizontalDistance(followWorldPosition, position) <= _pointReachDistance)
			{
				int currentPointIndex = _currentPointIndex;
				TryMoveToNextPoint();
				if (_currentPointIndex != currentPointIndex)
				{
					InvalidatePath();
				}
			}
		}

		private Vector3 ResolveSteerTarget(Vector3 waypointTarget)
		{
			if (_isUnsticking)
			{
				return _unstickTarget;
			}
			if (_path.Count > 0)
			{
				if (_pathIndex >= _path.Count)
				{
					_pathIndex = _path.Count - 1;
				}
				return _path[_pathIndex];
			}
			return waypointTarget;
		}

		private void TickUnstick(Vector3 followPos, Vector3 waypointTarget)
		{
			_unstickTimer -= Time.deltaTime;
			bool num = HorizontalDistance(followPos, _unstickTarget) <= _pointReachDistance;
			bool flag = _unstickTimer <= 0f;
			if (num || flag)
			{
				bool num2 = HorizontalDistance(followPos, waypointTarget) < _waypointDistanceAtUnstickStart - _unstickProgressEpsilon;
				EndUnstick();
				if (num2)
				{
					RebuildPath(followPos, waypointTarget);
					return;
				}
				RebuildPath(followPos, waypointTarget);
				_stuckTimer = 0f;
				_bestProgressDistance = float.MaxValue;
			}
		}

		private Vector3 GetFollowWorldPosition()
		{
			if (_physGrabber != null && _physGrabber.physGrabPoints.TryGetValue(_grabObject, out var value) && value != null)
			{
				return value.position;
			}
			if (_grabObject != null)
			{
				return _grabObject.transform.position;
			}
			if (!(_physGrabber != null))
			{
				return Vector3.zero;
			}
			return _physGrabber.transform.position;
		}

		private static float HorizontalDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		private void UpdateStuckState(float progressDistance, Vector3 followPos, Vector3 targetPoint)
		{
			if (_isUnsticking)
			{
				return;
			}
			if (progressDistance <= _pointReachDistance)
			{
				_stuckTimer = 0f;
				return;
			}
			if (progressDistance < _bestProgressDistance - 0.01f)
			{
				_bestProgressDistance = progressDistance;
				_stuckTimer = 0f;
				return;
			}
			_stuckTimer += Time.deltaTime;
			if (!(_stuckTimer < _stuckTime))
			{
				_stuckTimer = 0f;
				_bestProgressDistance = float.MaxValue;
				BeginUnstick(followPos, targetPoint);
			}
		}

		private void BeginUnstick(Vector3 followPos, Vector3 waypointTarget)
		{
			Vector3 vector = new Vector3(waypointTarget.x - followPos.x, 0f, waypointTarget.z - followPos.z);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.forward;
			}
			else
			{
				vector.Normalize();
			}
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			int num = UnstickDirectionOffsets.Length;
			for (int i = 0; i < num; i++)
			{
				_unstickDirectionIndex = (_unstickDirectionIndex + 1) % num;
				Vector2 vector2 = UnstickDirectionOffsets[_unstickDirectionIndex];
				Vector3 vector3 = followPos + (normalized * vector2.x + vector * vector2.y) * _unstickOffset;
				vector3.y = followPos.y;
				if (IsUnstickDirectionClear(followPos, vector3))
				{
					StartUnstick(vector3, followPos, waypointTarget);
					return;
				}
			}
			_unstickDirectionIndex = (_unstickDirectionIndex + 1) % num;
			Vector2 vector4 = UnstickDirectionOffsets[_unstickDirectionIndex];
			Vector3 sideTarget = followPos + (normalized * vector4.x + vector * vector4.y) * _unstickOffset;
			sideTarget.y = followPos.y;
			StartUnstick(sideTarget, followPos, waypointTarget);
		}

		private void StartUnstick(Vector3 sideTarget, Vector3 followPos, Vector3 waypointTarget)
		{
			_unstickTarget = sideTarget;
			_isUnsticking = true;
			_unstickTimer = _unstickDuration;
			_waypointDistanceAtUnstickStart = HorizontalDistance(followPos, waypointTarget);
			InvalidatePath();
		}

		private bool IsUnstickDirectionClear(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.0001f)
			{
				return false;
			}
			RaycastHit hitInfo;
			return !Physics.SphereCast(new Vector3(from.x, from.y + _agentRadius + 0.05f, from.z), Mathf.Max(0.05f, _agentRadius * 0.85f), vector / magnitude, out hitInfo, magnitude, _obstacleLayerMask, QueryTriggerInteraction.Ignore);
		}

		private void EndUnstick()
		{
			_isUnsticking = false;
			_unstickTimer = 0f;
		}

		private void RebuildPath(Vector3 from, Vector3 to)
		{
			if (!_isUnsticking)
			{
				InvalidatePath();
				_pathWaypointIndex = _currentPointIndex;
				SwimWaterGrid orBuildGrid = _swimGridPathfinder.GetOrBuildGrid(_source, _waterLayerMask, _obstacleLayerMask, _cellSize, _agentRadius);
				if (!_swimGridPathfinder.TryFindPath(from, to, orBuildGrid, _obstacleLayerMask, _agentRadius, _path))
				{
					_path.Clear();
					_pathIndex = 0;
				}
				else
				{
					_pathIndex = 0;
					_bestProgressDistance = float.MaxValue;
					_stuckTimer = 0f;
				}
			}
		}

		private void InvalidatePath()
		{
			_path.Clear();
			_pathIndex = 0;
			_pathWaypointIndex = -1;
			_bestProgressDistance = float.MaxValue;
		}

		private void ApplyVerticalBobbing(ref Vector3 currentPos, Vector3 waterPosition)
		{
			float y = waterPosition.y + _verticalOffset + Mathf.Sin(Time.time * _frequency) * _amplitude;
			currentPos.y = y;
		}

		private void TryMoveToNextPoint()
		{
			if (_swimmingPointsSequense != null && _swimmingPointsSequense.Count != 0 && _currentPointIndex < _swimmingPointsSequense.Count - 1)
			{
				_currentPointIndex++;
			}
		}

		private void ResetSwimming()
		{
			_currentPointIndex = 0;
			InvalidatePath();
			_stuckTimer = 0f;
			EndUnstick();
			_unstickDirectionIndex = -1;
			_grabberDisableTimer = 0f;
			if (_physGrabber != null)
			{
				if (_grabObject.Grabbers.Contains(_physGrabber))
				{
					_grabObject.Grabbers.Remove(_physGrabber);
				}
				if (base.Runner != null)
				{
					base.Runner.Despawn(_physGrabber.Object);
				}
				_physGrabber = null;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (base.HasStateAuthority)
			{
				ResetSwimming();
			}
			_isInitialized = false;
			base.Despawned(runner, hasState);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsSwimmingDisabled = _IsSwimmingDisabled;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsSwimmingDisabled = IsSwimmingDisabled;
		}
	}
}
