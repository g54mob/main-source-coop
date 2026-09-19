using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class RequestStateAuthorityOnCollide : MonoBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private LayerMask _layerMask;

		private bool _enabled;

		private void Awake()
		{
			Enable();
		}

		private void OnCollisionEnter(Collision other)
		{
			if (_enabled && GetComponentInChildren<ICartItemsContainer>() == null && _grabable.GrabbedByPlayersCount <= 0 && _grabable.GrabbedByExternalsCount <= 0 && !_grabable.InCart && (_layerMask.value & (1 << other.gameObject.layer)) != 0 && other.gameObject.TryGetComponent<NetworkObject>(out var component) && !(_grabable.Object == null) && !(component == null) && _grabable.Object.StateAuthority.PlayerId != component.StateAuthority.PlayerId)
			{
				_grabable.RequestStateAuthorityRPC(component.StateAuthority.PlayerId);
			}
		}

		public void Enable()
		{
			_enabled = true;
		}

		public void Disable()
		{
			_enabled = false;
		}
	}
}
