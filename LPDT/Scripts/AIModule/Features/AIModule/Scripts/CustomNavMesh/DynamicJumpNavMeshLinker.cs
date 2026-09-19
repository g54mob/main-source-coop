using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts.CustomNavMesh
{
	public class DynamicJumpNavMeshLinker : MonoBehaviour
	{
		private const float DEFAULT_POLL_INTERVAL = 0.15f;

		private const float DEFAULT_CARVE_SETTLE_DELAY = 0.1f;

		[SerializeField]
		private NavMeshObstacle _obstacle;

		[SerializeField]
		private Collider _boundsCollider;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private NavMeshAreas _area = NavMeshAreas.Jump;

		[SerializeField]
		private List<NavMeshAgentTypes> _agentTypes = new List<NavMeshAgentTypes>
		{
			NavMeshAgentTypes.Universal,
			NavMeshAgentTypes.Humanoid,
			NavMeshAgentTypes.Mimic
		};

		[SerializeField]
		private int _maxLinks = 2;

		[SerializeField]
		private int _sampleDirections = 8;

		[SerializeField]
		private float _sampleDistance = 0.35f;

		[SerializeField]
		private float _sampleRadius = 0.75f;

		[SerializeField]
		private float _linkWidth = 1.5f;

		[SerializeField]
		private float _costModifier = 2f;

		[SerializeField]
		private bool _bidirectional = true;

		[SerializeField]
		private float _stationarySpeedThreshold = 0.05f;

		[SerializeField]
		private float _stationaryTime = 0.5f;

		[SerializeField]
		private float _rebuildMoveThreshold = 0.2f;

		[SerializeField]
		private float _pollInterval = 0.15f;

		[SerializeField]
		private float _carveSettleDelay = 0.1f;

		[SerializeField]
		private bool _drawGizmos = true;

		[SerializeField]
		private bool _drawGizmosInPlayMode = true;

		[SerializeField]
		private bool _drawCandidatesWhenSelected = true;

		private readonly List<NavMeshLinkInstance> _linkInstances = new List<NavMeshLinkInstance>(16);

		private readonly List<JumpLinkPair> _activePairs = new List<JumpLinkPair>(4);

		private readonly List<JumpLinkPair> _sampleBuffer = new List<JumpLinkPair>(4);

		private readonly List<Vector3> _candidateHits = new List<Vector3>(16);

		private readonly List<int> _agentTypeIds = new List<int>(4);

		private int _areaId;

		private float _pollTimer;

		private float _stationaryTimer;

		private float _settleTimer;

		private Vector3 _lastPolledPosition;

		private Vector3 _linksBuiltAtPosition;

		private bool _hasLinks;

		private bool _isMoving = true;

		private bool _awaitingSettle;

		private bool _lastRebuildFailed;

		public int AreaId => _areaId;

		public bool EditorDrawGizmos => _drawGizmos;

		public bool EditorDrawGizmosInPlayMode => _drawGizmosInPlayMode;

		public bool EditorDrawCandidatesWhenSelected => _drawCandidatesWhenSelected;

		public bool EditorIsMoving => _isMoving;

		public bool EditorLastRebuildFailed => _lastRebuildFailed;

		public bool EditorIsActiveAndEnabled => base.isActiveAndEnabled;

		public IReadOnlyList<JumpLinkPair> EditorActivePairs => _activePairs;

		public IReadOnlyList<Vector3> EditorCandidateHits => _candidateHits;

		public int EditorMaxLinks => _maxLinks;

		public int EditorSampleDirections => _sampleDirections;

		public float EditorSampleDistance => _sampleDistance;

		public float EditorSampleRadius => _sampleRadius;

		public bool EditorTryGetSampleCenter(out Vector3 center, out float horizontalRadius)
		{
			return TryGetSampleCenter(out center, out horizontalRadius);
		}

		private void Awake()
		{
			ResolveAgentTypeIds();
			_areaId = NavMesh.GetAreaFromName(_area.ToString());
			_lastPolledPosition = base.transform.position;
			_linksBuiltAtPosition = base.transform.position;
		}

		private void ResolveAgentTypeIds()
		{
			_agentTypeIds.Clear();
			if (_agentTypes == null || _agentTypes.Count == 0)
			{
				_agentTypeIds.Add(NavMesh.GetSettingsByIndex(0).agentTypeID);
				return;
			}
			for (int i = 0; i < _agentTypes.Count; i++)
			{
				int agentTypeIdByName = GetAgentTypeIdByName(_agentTypes[i].ToString());
				if (!_agentTypeIds.Contains(agentTypeIdByName))
				{
					_agentTypeIds.Add(agentTypeIdByName);
				}
			}
			if (_agentTypeIds.Count == 0)
			{
				_agentTypeIds.Add(NavMesh.GetSettingsByIndex(0).agentTypeID);
			}
		}

		private void OnEnable()
		{
			_pollTimer = 0f;
			_stationaryTimer = 0f;
			_settleTimer = 0f;
			_isMoving = true;
			_awaitingSettle = false;
			_lastRebuildFailed = false;
			_lastPolledPosition = base.transform.position;
		}

		private void OnDisable()
		{
			ClearLinks();
		}

		private void OnDestroy()
		{
			ClearLinks();
		}

		private void Update()
		{
			_pollTimer += Time.deltaTime;
			if (!(_pollTimer < _pollInterval))
			{
				float pollTimer = _pollTimer;
				_pollTimer = 0f;
				Poll(pollTimer);
			}
		}

		private void Poll(float deltaTime)
		{
			Vector3 position = base.transform.position;
			float num = Vector3.Distance(position, _lastPolledPosition);
			_lastPolledPosition = position;
			bool flag = false;
			if (_rigidbody != null)
			{
				flag = _rigidbody.linearVelocity.sqrMagnitude > _stationarySpeedThreshold * _stationarySpeedThreshold;
			}
			float num2 = ((deltaTime > 0f) ? (num / deltaTime) : 0f);
			if (flag || num2 > _stationarySpeedThreshold)
			{
				_isMoving = true;
				_stationaryTimer = 0f;
				_awaitingSettle = false;
				_settleTimer = 0f;
				if (_hasLinks)
				{
					ClearLinks();
				}
				return;
			}
			_isMoving = false;
			_stationaryTimer += deltaTime;
			if (_hasLinks)
			{
				if (Vector3.Distance(position, _linksBuiltAtPosition) >= _rebuildMoveThreshold)
				{
					_awaitingSettle = true;
					_settleTimer = 0f;
					ClearLinks();
				}
			}
			else if (!(_stationaryTimer < _stationaryTime))
			{
				if (!_awaitingSettle)
				{
					_awaitingSettle = true;
					_settleTimer = 0f;
				}
				_settleTimer += deltaTime;
				if (!(_settleTimer < _carveSettleDelay))
				{
					RebuildLinks();
					_awaitingSettle = false;
				}
			}
		}

		private void RebuildLinks()
		{
			ClearLinks();
			if (!TryGetSampleCenter(out var center, out var horizontalRadius))
			{
				_lastRebuildFailed = true;
				return;
			}
			DynamicJumpNavMeshLinkSampler.SamplePairs(center, horizontalRadius, _sampleDirections, _sampleDistance, _sampleRadius, -1, _maxLinks, _sampleBuffer, _candidateHits);
			if (_sampleBuffer.Count == 0)
			{
				_lastRebuildFailed = true;
				return;
			}
			for (int i = 0; i < _sampleBuffer.Count; i++)
			{
				JumpLinkPair item = _sampleBuffer[i];
				bool flag = false;
				for (int j = 0; j < _agentTypeIds.Count; j++)
				{
					NavMeshLinkInstance navMeshLinkInstance = NavMesh.AddLink(new NavMeshLinkData
					{
						startPosition = item.Start,
						endPosition = item.End,
						width = _linkWidth,
						costModifier = _costModifier,
						bidirectional = _bidirectional,
						area = _areaId,
						agentTypeID = _agentTypeIds[j]
					});
					if (NavMesh.IsLinkValid(navMeshLinkInstance))
					{
						NavMesh.SetLinkOwner(navMeshLinkInstance, this);
						_linkInstances.Add(navMeshLinkInstance);
						if (!flag)
						{
							_activePairs.Add(item);
							flag = true;
						}
					}
				}
			}
			_hasLinks = _linkInstances.Count > 0;
			_lastRebuildFailed = !_hasLinks;
			if (_hasLinks)
			{
				_linksBuiltAtPosition = base.transform.position;
			}
		}

		private void ClearLinks()
		{
			for (int i = 0; i < _linkInstances.Count; i++)
			{
				NavMeshLinkInstance handle = _linkInstances[i];
				if (NavMesh.IsLinkValid(handle))
				{
					NavMesh.RemoveLink(handle);
				}
			}
			_linkInstances.Clear();
			_activePairs.Clear();
			_hasLinks = false;
		}

		private bool TryGetSampleCenter(out Vector3 center, out float horizontalRadius)
		{
			if (_obstacle != null && _obstacle.enabled)
			{
				Transform transform = _obstacle.transform;
				Vector3 lossyScale = transform.lossyScale;
				center = transform.TransformPoint(_obstacle.center);
				if (_obstacle.shape == NavMeshObstacleShape.Box)
				{
					Vector3 size = _obstacle.size;
					float a = Mathf.Abs(size.x * lossyScale.x) * 0.5f;
					float b = Mathf.Abs(size.z * lossyScale.z) * 0.5f;
					horizontalRadius = Mathf.Max(a, b);
				}
				else
				{
					float num = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
					horizontalRadius = Mathf.Abs(_obstacle.radius) * num;
				}
				return true;
			}
			if (_boundsCollider != null && _boundsCollider.enabled)
			{
				Bounds bounds = _boundsCollider.bounds;
				center = bounds.center;
				horizontalRadius = Mathf.Max(bounds.extents.x, bounds.extents.z);
				return true;
			}
			center = base.transform.position;
			horizontalRadius = 0.5f;
			return true;
		}

		private static int GetAgentTypeIdByName(string agentTypeName)
		{
			for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
			{
				NavMeshBuildSettings settingsByIndex = NavMesh.GetSettingsByIndex(i);
				if (NavMesh.GetSettingsNameFromID(settingsByIndex.agentTypeID) == agentTypeName)
				{
					return settingsByIndex.agentTypeID;
				}
			}
			return NavMesh.GetSettingsByIndex(0).agentTypeID;
		}
	}
}
