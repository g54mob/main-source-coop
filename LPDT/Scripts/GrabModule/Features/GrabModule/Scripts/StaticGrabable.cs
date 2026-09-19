using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class StaticGrabable : NetworkBehaviour, IGrabableBase
	{
		[SerializeField]
		private Transform _parentGameObject;

		public List<NetworkRigidbody> _joints = new List<NetworkRigidbody>();

		public Rigidbody _handRigidbody;

		public bool IsEnableToGrab => true;

		public GrabableType GrabableType => GrabableType.Static;

		public GameObject GameObject => base.gameObject;

		public void Join(Rigidbody handRigidbody)
		{
			_handRigidbody = handRigidbody;
			NetworkRigidbody component = _handRigidbody.GetComponent<NetworkRigidbody>();
			if (!_joints.Contains(component))
			{
				_handRigidbody.transform.SetParent(_parentGameObject, worldPositionStays: true);
				if (component.Object.HasStateAuthority)
				{
					_handRigidbody.isKinematic = true;
				}
				_handRigidbody.linearVelocity = Vector3.zero;
				_handRigidbody.angularVelocity = Vector3.zero;
				_joints.Add(component);
			}
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (_handRigidbody != null && _handRigidbody.transform.parent != _parentGameObject)
			{
				_handRigidbody.transform.SetParent(_parentGameObject);
			}
		}

		public void Unjoin(Rigidbody handRigidbody)
		{
			NetworkRigidbody component = handRigidbody.GetComponent<NetworkRigidbody>();
			if (_joints.Contains(component))
			{
				_handRigidbody = null;
				handRigidbody.transform.SetParent(null, worldPositionStays: true);
				Debug.LogError("false");
				if (component.Object.HasStateAuthority)
				{
					handRigidbody.isKinematic = false;
				}
				_joints.Remove(component);
				handRigidbody.GetComponent<GrabController>().NotifyUnjoin(this);
			}
		}

		public void UnjoinAll()
		{
			GrabController grabController = null;
			foreach (NetworkRigidbody joint in _joints)
			{
				joint.transform.SetParent(null, worldPositionStays: true);
				grabController = joint.GetComponent<GrabController>();
			}
			_joints.Clear();
			grabController?.NotifyUnjoin(this);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
