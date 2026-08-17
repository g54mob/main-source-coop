using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public class Wall : MonoBehaviour, ISnappingPlaneContainer
	{
		private SnappingPlaneContainerHelper _containerHelper;

		public SnappingPlane[] SnappingPlanes => _containerHelper?.SnappingPlanes;

		private void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			_containerHelper = new SnappingPlaneContainerHelper(this);
			_containerHelper.Initialize();
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
