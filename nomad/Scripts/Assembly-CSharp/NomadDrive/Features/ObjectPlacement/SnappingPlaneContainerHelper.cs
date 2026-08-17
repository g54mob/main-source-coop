using System.Linq;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking.Parenting;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public class SnappingPlaneContainerHelper
	{
		private readonly MonoBehaviour _owner;

		private SnappingPlane[] _snappingPlanes;

		public SnappingPlane[] SnappingPlanes => _snappingPlanes;

		public SnappingPlaneContainerHelper(MonoBehaviour owner)
		{
			_owner = owner;
		}

		public void Initialize()
		{
			SnappingPlane[] componentsInChildren = _owner.GetComponentsInChildren<SnappingPlane>();
			if (componentsInChildren == null)
			{
				EvilLogger.LogError("[" + _owner.gameObject.name + "] GetComponentsInChildren<SnappingPlane>() returned NULL!", "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlaneContainerHelper.cs", 35);
			}
			else
			{
				if (componentsInChildren.Length == 0)
				{
					return;
				}
				_snappingPlanes = componentsInChildren.OrderBy(delegate(SnappingPlane p)
				{
					NetworkedTransform component = p.GetComponent<NetworkedTransform>();
					return (byte)((!(component != null)) ? 255 : component.NetworkedTransformIndex);
				}).ToArray();
				for (byte b = 0; b < _snappingPlanes.Length; b++)
				{
					if (_snappingPlanes[b] == null)
					{
						EvilLogger.LogError($"[{_owner.gameObject.name}] SnappingPlane[{b}] is NULL!", "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlaneContainerHelper.cs", 60);
					}
					else
					{
						_snappingPlanes[b].SnappingPlaneIndex = b;
					}
				}
			}
		}

		public SnappingPlane GetSnappingPlaneByIndex(ushort index)
		{
			if (_snappingPlanes == null)
			{
				EvilLogger.LogError("[" + _owner.gameObject.name + "] SnappingPlanes array is NULL! Initializing now...", "GetSnappingPlaneByIndex", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlaneContainerHelper.cs", 71);
				Initialize();
				if (_snappingPlanes == null)
				{
					EvilLogger.LogError("[" + _owner.gameObject.name + "] SnappingPlanes array is STILL NULL after initialization!", "GetSnappingPlaneByIndex", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlaneContainerHelper.cs", 76);
					return null;
				}
			}
			if (index >= _snappingPlanes.Length)
			{
				EvilLogger.LogError($"[{_owner.gameObject.name}] Index {index} is out of range! SnappingPlanes length: {_snappingPlanes.Length}", "GetSnappingPlaneByIndex", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\SnappingPlaneContainerHelper.cs", 83);
				return null;
			}
			return _snappingPlanes[index];
		}

		public ushort GetSnappingPlaneIDBySnappingPlaneReference(SnappingPlane snappingPlane)
		{
			return snappingPlane.SnappingPlaneIndex;
		}

		public bool IsAnySnappingPlaneOneShotSlotFull()
		{
			if (_snappingPlanes == null)
			{
				return false;
			}
			SnappingPlane[] snappingPlanes = _snappingPlanes;
			foreach (SnappingPlane snappingPlane in snappingPlanes)
			{
				if (snappingPlane != null && snappingPlane.IsAnyObjectPlaced)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsSnappingPlaneOccupied(int snappingPlaneID)
		{
			if (_snappingPlanes == null || snappingPlaneID >= _snappingPlanes.Length)
			{
				return false;
			}
			return _snappingPlanes[snappingPlaneID].IsAnyObjectPlaced;
		}

		public bool IsSnappingPlaneOccupied(SnappingPlane snappingPlane)
		{
			if (snappingPlane != null)
			{
				return snappingPlane.IsAnyObjectPlaced;
			}
			return false;
		}

		public void EnableAllSnappingPlanes()
		{
			if (_snappingPlanes != null)
			{
				SnappingPlane[] snappingPlanes = _snappingPlanes;
				for (int i = 0; i < snappingPlanes.Length; i++)
				{
					snappingPlanes[i]?.Enable();
				}
			}
		}

		public void DisableAllSnappingPlanes()
		{
			if (_snappingPlanes != null)
			{
				SnappingPlane[] snappingPlanes = _snappingPlanes;
				for (int i = 0; i < snappingPlanes.Length; i++)
				{
					snappingPlanes[i]?.Disable();
				}
			}
		}

		public void EnableSnappingPlane(int snappingPlaneID)
		{
			if (_snappingPlanes != null && snappingPlaneID < _snappingPlanes.Length)
			{
				_snappingPlanes[snappingPlaneID]?.Enable();
			}
		}

		public void DisableSnappingPlane(int snappingPlaneID)
		{
			if (_snappingPlanes != null && snappingPlaneID < _snappingPlanes.Length)
			{
				_snappingPlanes[snappingPlaneID]?.Disable();
			}
		}
	}
}
