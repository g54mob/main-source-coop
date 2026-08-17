using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking.Parenting;
using Mirror;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle
{
	[RequireComponent(typeof(NetworkIdentity))]
	public class SnappingPlanesManager : MonoBehaviour, ISnappingPlaneContainer
	{
		private struct PlacementListenerData
		{
			public SnappingPlane Plane;

			public UnityAction<GameObject> PlacedAction;

			public UnityAction<GameObject> RemovedAction;
		}

		private NetworkedTransform _networkedTransform;

		private SnappingPlaneContainerHelper _containerHelper;

		private readonly List<PlacementListenerData> _listeners = new List<PlacementListenerData>();

		private bool IsInitialized => _containerHelper != null;

		private NetworkedTransform NetworkedTransformRef => _networkedTransform;

		private int SnappingPlaneCount
		{
			get
			{
				SnappingPlane[] snappingPlanes = SnappingPlanes;
				if (snappingPlanes == null)
				{
					return 0;
				}
				return snappingPlanes.Length;
			}
		}

		private int ActiveListenerCount => _listeners.Count;

		private SnappingPlane[] SnappingPlanesDebug => SnappingPlanes;

		private int OccupiedPlaneCount
		{
			get
			{
				if (SnappingPlanes == null)
				{
					return 0;
				}
				int num = 0;
				SnappingPlane[] snappingPlanes = SnappingPlanes;
				foreach (SnappingPlane snappingPlane in snappingPlanes)
				{
					if (snappingPlane != null && snappingPlane.IsAnyObjectPlaced)
					{
						num++;
					}
				}
				return num;
			}
		}

		public SnappingPlane[] SnappingPlanes
		{
			get
			{
				if (!Application.isPlaying)
				{
					return GetComponentsInChildren<SnappingPlane>();
				}
				if (_containerHelper == null)
				{
					Initialize();
				}
				return _containerHelper?.SnappingPlanes;
			}
		}

		protected void Awake()
		{
			if (TryGetComponent<NetworkedTransform>(out var component))
			{
				_networkedTransform = component;
			}
			else
			{
				EvilLogger.LogError("[SnappingPlanesManager] NetworkedTransform component missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\SnappingPlanesManager.cs", 83);
			}
		}

		protected void Start()
		{
			Initialize();
		}

		protected virtual void OnDestroy()
		{
			CleanupListeners();
		}

		public void Initialize()
		{
			if (_containerHelper == null)
			{
				if (_networkedTransform == null)
				{
					EvilLogger.LogError("[SnappingPlanesManager] Cannot initialize - NetworkedTransform is null on " + base.gameObject.name, "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\SnappingPlanesManager.cs", 106);
					return;
				}
				_containerHelper = new SnappingPlaneContainerHelper(this);
				_containerHelper.Initialize();
				SetupPlacementListeners();
			}
		}

		private void SetupPlacementListeners()
		{
			if (SnappingPlanes == null)
			{
				return;
			}
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			foreach (SnappingPlane snappingPlane in snappingPlanes)
			{
				if (!(snappingPlane == null))
				{
					SnappingPlane capturedPlane = snappingPlane;
					UnityAction<GameObject> unityAction = delegate(GameObject obj)
					{
						HandleObjectPlaced(capturedPlane, obj);
					};
					UnityAction<GameObject> unityAction2 = delegate(GameObject obj)
					{
						HandleObjectRemoved(obj);
					};
					snappingPlane.OnObjectPlaced.AddListener(unityAction);
					snappingPlane.OnObjectRemoved.AddListener(unityAction2);
					_listeners.Add(new PlacementListenerData
					{
						Plane = snappingPlane,
						PlacedAction = unityAction,
						RemovedAction = unityAction2
					});
				}
			}
		}

		private void CleanupListeners()
		{
			foreach (PlacementListenerData listener in _listeners)
			{
				if (listener.Plane != null)
				{
					listener.Plane.OnObjectPlaced.RemoveListener(listener.PlacedAction);
					listener.Plane.OnObjectRemoved.RemoveListener(listener.RemovedAction);
				}
			}
			_listeners.Clear();
		}

		private void HandleObjectPlaced(SnappingPlane plane, GameObject obj)
		{
		}

		private void HandleObjectRemoved(GameObject obj)
		{
			NetworkedTransform component = obj.GetComponent<NetworkedTransform>();
			if (component != null)
			{
				component.SetParent(null, default(NetworkedTransformParentingConfig), 0);
			}
		}

		public SnappingPlane GetSnappingPlaneByIndex(ushort index)
		{
			if (_containerHelper == null)
			{
				Initialize();
			}
			return _containerHelper.GetSnappingPlaneByIndex(index);
		}

		public ushort GetSnappingPlaneIDBySnappingPlaneReference(SnappingPlane snappingPlane)
		{
			return _containerHelper.GetSnappingPlaneIDBySnappingPlaneReference(snappingPlane);
		}

		public bool IsAnySnappingPlaneOneShotSlotFull()
		{
			return _containerHelper?.IsAnySnappingPlaneOneShotSlotFull() ?? false;
		}

		public bool IsSnappingPlaneOccupied(int snappingPlaneID)
		{
			return _containerHelper?.IsSnappingPlaneOccupied(snappingPlaneID) ?? false;
		}

		public bool IsSnappingPlaneOccupied(SnappingPlane snappingPlane)
		{
			return _containerHelper?.IsSnappingPlaneOccupied(snappingPlane) ?? false;
		}

		public void EnableAllSnappingPlanes()
		{
			_containerHelper?.EnableAllSnappingPlanes();
		}

		public void DisableAllSnappingPlanes()
		{
			_containerHelper?.DisableAllSnappingPlanes();
		}

		public void EnableSnappingPlane(int snappingPlaneID)
		{
			_containerHelper?.EnableSnappingPlane(snappingPlaneID);
		}

		public void DisableSnappingPlane(int snappingPlaneID)
		{
			_containerHelper?.DisableSnappingPlane(snappingPlaneID);
		}
	}
}
