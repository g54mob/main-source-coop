using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class SnappingPlaneDetector
	{
		private static int _snappingPlaneLayerMask = -1;

		private static int SnappingPlaneLayerMask
		{
			get
			{
				if (_snappingPlaneLayerMask == -1)
				{
					_snappingPlaneLayerMask = LayerMask.GetMask("SnappingPlane");
				}
				return _snappingPlaneLayerMask;
			}
		}

		public static bool TryDetectSnappingPlane(Ray ray, float maxDistance, out SnappingPlane snappingPlane, out RaycastHit hitInfo)
		{
			snappingPlane = null;
			if (!Physics.Raycast(ray, out hitInfo, maxDistance, SnappingPlaneLayerMask))
			{
				return false;
			}
			return hitInfo.collider.TryGetComponent<SnappingPlane>(out snappingPlane);
		}

		public static bool ValidateSnappingPlane(SnappingPlane snappingPlane, IPlaceable placeable, Transform currentPlacementObject)
		{
			if (snappingPlane == null)
			{
				return false;
			}
			if (!snappingPlane.IsEnabled)
			{
				return false;
			}
			if (!snappingPlane.IsPlacementAllowed(currentPlacementObject.gameObject))
			{
				return false;
			}
			if (snappingPlane.transform.IsChildOf(currentPlacementObject))
			{
				return false;
			}
			return true;
		}
	}
}
