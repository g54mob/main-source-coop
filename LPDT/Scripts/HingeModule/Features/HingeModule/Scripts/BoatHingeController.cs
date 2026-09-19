using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	public class BoatHingeController : MonoBehaviour
	{
		[SerializeField]
		private HingeJoint _hingeJoint;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private float _breakDistance;

		[SerializeField]
		private Transform _hullRoot;

		private CartGrabObject _grabObject;

		private BoatHingeControllerData _activeHingeControllerData;

		private Collider[] _hullColliders;

		private Collider[] _cartColliders;

		private void OnTriggerEnter(Collider other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) != 0 && other.gameObject.TryGetComponent<BoatHingeControllerData>(out var component))
			{
				_activeHingeControllerData = component;
				component.ConnectedBody.freezeRotation = false;
				_hingeJoint.connectedBody = component.ConnectedBody;
				_hingeJoint.connectedAnchor = component.ConnectedAnchor;
				_hingeJoint.anchor = base.transform.InverseTransformPoint(component.ConnectedAnchorWorldPosition);
				_grabObject = component.CartGrabObject;
				_grabObject.EnableYGrab(enable: true);
				_grabObject.SetForceInGrabPoint(forceInGrabPoint: true);
				component.IsHingeConnected = true;
				component.InvokeOnHingeConnectedChanged(isConnected: true);
				SetCartHullCollisionIgnored(component, ignored: true);
			}
		}

		private void SetCartHullCollisionIgnored(BoatHingeControllerData data, bool ignored)
		{
			if (data == null || data.ConnectedBody == null)
			{
				return;
			}
			if (ignored)
			{
				Transform transform = ((_hullRoot != null) ? _hullRoot : ((base.transform.parent != null) ? base.transform.parent : base.transform));
				_hullColliders = transform.GetComponentsInChildren<Collider>(includeInactive: true);
				_cartColliders = data.ConnectedBody.transform.root.GetComponentsInChildren<Collider>(includeInactive: true);
			}
			if (_hullColliders == null || _cartColliders == null)
			{
				return;
			}
			Collider[] hullColliders = _hullColliders;
			foreach (Collider collider in hullColliders)
			{
				if (!IsIgnorable(collider))
				{
					continue;
				}
				Collider[] cartColliders = _cartColliders;
				foreach (Collider collider2 in cartColliders)
				{
					if (IsIgnorable(collider2))
					{
						Physics.IgnoreCollision(collider, collider2, ignored);
					}
				}
			}
			if (!ignored)
			{
				_hullColliders = null;
				_cartColliders = null;
			}
		}

		private static bool IsIgnorable(Collider collider)
		{
			if (collider != null && !collider.isTrigger && collider.enabled)
			{
				return collider.gameObject.activeInHierarchy;
			}
			return false;
		}

		private void FixedUpdate()
		{
			if (!(_grabObject == null) && _grabObject.Grabbers.Count != 0 && !(Vector3.Distance(_grabObject.gameObject.transform.position, _grabObject.Grabbers[0].GrabberTransform.position) < _breakDistance))
			{
				SetCartHullCollisionIgnored(_activeHingeControllerData, ignored: false);
				_hingeJoint.connectedBody.constraints = (RigidbodyConstraints)80;
				_hingeJoint.connectedBody = null;
				_grabObject.EnableYGrab(enable: false);
				_grabObject.SetForceInGrabPoint(forceInGrabPoint: false);
				_grabObject = null;
				_activeHingeControllerData.IsHingeConnected = false;
				_activeHingeControllerData.InvokeOnHingeConnectedChanged(isConnected: false);
			}
		}
	}
}
