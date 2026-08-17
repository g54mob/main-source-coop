using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public class PlacementCollisionForwarder : MonoBehaviour
	{
		private HeldItem _parentHeldItem;

		private LayerMask _collisionLayers;

		public void Initialize(HeldItem parentHeldItem, LayerMask collisionLayers)
		{
			_parentHeldItem = parentHeldItem;
			_collisionLayers = collisionLayers;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsValidForwardedCollision(other))
			{
				_parentHeldItem.AddForwardedCollision(other);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			_parentHeldItem?.RemoveForwardedCollision(other);
		}

		private void OnDestroy()
		{
			_parentHeldItem = null;
		}

		private bool IsValidForwardedCollision(Collider other)
		{
			if (_parentHeldItem == null)
			{
				return false;
			}
			if (other == null || !other.gameObject.activeSelf)
			{
				return false;
			}
			if (other.isTrigger)
			{
				return false;
			}
			return (_collisionLayers.value & (1 << other.gameObject.layer)) != 0;
		}
	}
}
