using System;
using System.Collections.Generic;
using Mirror;
using NomadDrive.Features.Rope;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Cable
{
	public class CableManager : NetworkBehaviour, ICableManager
	{
		private class ManagedCable
		{
			public NomadDrive.Features.Rope.Rope Rope;

			public CableConnectionConfig Config;

			public float OriginalSwingAngle;

			public float OriginalSwingFreq;

			public float OriginalLength;
		}

		[Header("Global Multipliers")]
		[Tooltip("Multiplier for swing angle and frequency. 1 = normal, 2 = double swing.")]
		[SerializeField]
		[Range(0f, 3f)]
		private float _swingMultiplier = 1f;

		[Tooltip("Multiplier for slack/sag amount. 1 = normal, 2 = double sag.")]
		[SerializeField]
		[Range(0f, 3f)]
		private float _slackMultiplier = 1f;

		[Header("Cross-Chunk Settings")]
		[Tooltip("Maximum distance between edge objects for cross-chunk cable connections. Set to 0 to disable distance check.")]
		[SerializeField]
		private float _maxCrossChunkDistance;

		[Header("Debug")]
		[SerializeField]
		private int _registeredCableCount;

		[SerializeField]
		private int _pendingExitEdgeCount;

		[SerializeField]
		private int _pendingEntryEdgeCount;

		[SerializeField]
		private int _crossChunkCableCount;

		private readonly List<ManagedCable> _managedCables = new List<ManagedCable>();

		private readonly List<EdgeObjectInfo> _pendingExitEdges = new List<EdgeObjectInfo>();

		private readonly List<EdgeObjectInfo> _pendingEntryEdges = new List<EdgeObjectInfo>();

		private readonly List<CrossChunkCable> _crossChunkCables = new List<CrossChunkCable>();

		private float _lastSwingMultiplier;

		private float _lastSlackMultiplier;

		public int CableCount => _managedCables.Count;

		public float SwingMultiplier
		{
			get
			{
				return _swingMultiplier;
			}
			set
			{
				_swingMultiplier = Mathf.Clamp(value, 0f, 3f);
				ApplyMultipliers();
			}
		}

		public float SlackMultiplier
		{
			get
			{
				return _slackMultiplier;
			}
			set
			{
				_slackMultiplier = Mathf.Clamp(value, 0f, 3f);
				ApplyMultipliers();
			}
		}

		public event Action<CrossChunkCable> OnCrossChunkCableCreated;

		public event Action<CrossChunkCable> OnCrossChunkCableDestroyed;

		public void RegisterCable(NomadDrive.Features.Rope.Rope rope, CableConnectionConfig config)
		{
			if (rope == null)
			{
				return;
			}
			foreach (ManagedCable managedCable2 in _managedCables)
			{
				if (managedCable2.Rope == rope)
				{
					return;
				}
			}
			ManagedCable managedCable = new ManagedCable
			{
				Rope = rope,
				Config = config,
				OriginalSwingAngle = rope.swingAngle,
				OriginalSwingFreq = rope.swingFreq,
				OriginalLength = rope.length
			};
			_managedCables.Add(managedCable);
			ApplySingleCable(managedCable);
		}

		public void UnregisterCable(NomadDrive.Features.Rope.Rope rope)
		{
			if (rope == null)
			{
				return;
			}
			for (int num = _managedCables.Count - 1; num >= 0; num--)
			{
				if (_managedCables[num].Rope == rope)
				{
					_managedCables.RemoveAt(num);
					break;
				}
			}
		}

		public void ApplyMultipliers()
		{
			foreach (ManagedCable managedCable in _managedCables)
			{
				ApplySingleCable(managedCable);
			}
		}

		public void SetMultipliers(float swing, float slack)
		{
			_swingMultiplier = Mathf.Clamp(swing, 0f, 3f);
			_slackMultiplier = Mathf.Clamp(slack, 0f, 3f);
			ApplyMultipliers();
		}

		public void RegisterEdgeObject(EdgeObjectInfo edgeInfo)
		{
			if (!(edgeInfo.Object == null) && !(edgeInfo.Points == null))
			{
				if (edgeInfo.IsExit)
				{
					_pendingExitEdges.Add(edgeInfo);
				}
				else
				{
					_pendingEntryEdges.Add(edgeInfo);
				}
				TryCreateCrossChunkConnections(edgeInfo);
			}
		}

		public void UnregisterEdgeObjects(EvilRoad road)
		{
			if (road == null)
			{
				return;
			}
			_pendingExitEdges.RemoveAll((EdgeObjectInfo e) => e.OwnerRoad == road);
			_pendingEntryEdges.RemoveAll((EdgeObjectInfo e) => e.OwnerRoad == road);
			for (int num = _crossChunkCables.Count - 1; num >= 0; num--)
			{
				CrossChunkCable cable = _crossChunkCables[num];
				if (cable.ExitEdge.OwnerRoad == road || cable.EntryEdge.OwnerRoad == road)
				{
					DestroyCrossChunkCable(cable);
					_crossChunkCables.RemoveAt(num);
				}
			}
		}

		private void TryCreateCrossChunkConnections(EdgeObjectInfo newEdge)
		{
			foreach (EdgeObjectInfo item in newEdge.IsExit ? _pendingEntryEdges : _pendingExitEdges)
			{
				if (!(item.OwnerRoad == newEdge.OwnerRoad) && !(item.Config != newEdge.Config) && item.IsLeftSide == newEdge.IsLeftSide && !(item.Object == null) && !(newEdge.Object == null))
				{
					float num = Vector3.Distance(newEdge.Object.transform.position, item.Object.transform.position);
					if ((!(_maxCrossChunkDistance > 0f) || !(num > _maxCrossChunkDistance)) && !IsAlreadyConnected(newEdge, item))
					{
						EdgeObjectInfo exitEdge = (newEdge.IsExit ? newEdge : item);
						EdgeObjectInfo entryEdge = (newEdge.IsExit ? item : newEdge);
						CreateCrossChunkCable(exitEdge, entryEdge);
					}
				}
			}
		}

		private bool IsAlreadyConnected(EdgeObjectInfo edge1, EdgeObjectInfo edge2)
		{
			foreach (CrossChunkCable crossChunkCable in _crossChunkCables)
			{
				bool num = crossChunkCable.ExitEdge.Object == edge1.Object && crossChunkCable.EntryEdge.Object == edge2.Object;
				bool flag = crossChunkCable.ExitEdge.Object == edge2.Object && crossChunkCable.EntryEdge.Object == edge1.Object;
				if (num || flag)
				{
					return true;
				}
			}
			return false;
		}

		private void CreateCrossChunkCable(EdgeObjectInfo exitEdge, EdgeObjectInfo entryEdge)
		{
			Transform[] exitPoints = exitEdge.Points.ExitPoints;
			Transform[] entryPoints = entryEdge.Points.EntryPoints;
			List<GameObject> list = new List<GameObject>();
			int num = Mathf.Min(exitPoints.Length, entryPoints.Length);
			for (int i = 0; i < num; i++)
			{
				if (!(exitPoints[i] == null) && !(entryPoints[i] == null))
				{
					GameObject gameObject = CableConnectionHandler.CreateSingleRope(exitPoints[i], entryPoints[i], exitEdge.Config, this, base.transform);
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
				}
			}
			if (list.Count > 0)
			{
				CrossChunkCable crossChunkCable = new CrossChunkCable
				{
					RopeObjects = list,
					ExitEdge = exitEdge,
					EntryEdge = entryEdge
				};
				_crossChunkCables.Add(crossChunkCable);
				this.OnCrossChunkCableCreated?.Invoke(crossChunkCable);
			}
		}

		private void DestroyCrossChunkCable(CrossChunkCable cable)
		{
			foreach (GameObject ropeObject in cable.RopeObjects)
			{
				if (ropeObject != null)
				{
					UnityEngine.Object.Destroy(ropeObject);
				}
			}
			this.OnCrossChunkCableDestroyed?.Invoke(cable);
		}

		private void ApplySingleCable(ManagedCable managed)
		{
			if (!(managed.Rope == null))
			{
				CableConnectionConfig config = managed.Config;
				if (config != null && config.AffectedByGlobalSwing)
				{
					managed.Rope.swingAngle = managed.OriginalSwingAngle * _swingMultiplier;
					managed.Rope.swingFreq = managed.OriginalSwingFreq * _swingMultiplier;
				}
				if (config != null && config.AffectedByGlobalSlack)
				{
					float num = managed.OriginalLength - config.Slack;
					float num2 = config.Slack * _slackMultiplier;
					managed.Rope.length = num + num2;
				}
				managed.Rope.SetDirty();
			}
		}

		private void Start()
		{
			_lastSwingMultiplier = _swingMultiplier;
			_lastSlackMultiplier = _slackMultiplier;
		}

		private void Update()
		{
			if (!Mathf.Approximately(_swingMultiplier, _lastSwingMultiplier) || !Mathf.Approximately(_slackMultiplier, _lastSlackMultiplier))
			{
				_lastSwingMultiplier = _swingMultiplier;
				_lastSlackMultiplier = _slackMultiplier;
				ApplyMultipliers();
			}
		}

		private void LateUpdate()
		{
			_registeredCableCount = _managedCables.Count;
			_pendingExitEdgeCount = _pendingExitEdges.Count;
			_pendingEntryEdgeCount = _pendingEntryEdges.Count;
			_crossChunkCableCount = _crossChunkCables.Count;
			_managedCables.RemoveAll((ManagedCable m) => m.Rope == null);
			_pendingExitEdges.RemoveAll((EdgeObjectInfo e) => e.Object == null);
			_pendingEntryEdges.RemoveAll((EdgeObjectInfo e) => e.Object == null);
			for (int num = _crossChunkCables.Count - 1; num >= 0; num--)
			{
				CrossChunkCable cable = _crossChunkCables[num];
				if (cable.ExitEdge.Object == null || cable.EntryEdge.Object == null)
				{
					DestroyCrossChunkCable(cable);
					_crossChunkCables.RemoveAt(num);
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
