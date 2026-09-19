using System;
using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	public class BoatHingeControllerData : MonoBehaviour
	{
		[SerializeField]
		private CartGrabObject _grabObject;

		[SerializeField]
		private Rigidbody _connectedBody;

		public Vector3 ConnectedAnchor => base.transform.localPosition;

		public Vector3 ConnectedAnchorWorldPosition => base.transform.position;

		public Rigidbody ConnectedBody => _connectedBody;

		public CartGrabObject CartGrabObject => _grabObject;

		public bool IsHingeConnected { get; set; }

		public event Action<bool> OnHingeConnectedChanged;

		public void InvokeOnHingeConnectedChanged(bool isConnected)
		{
			this.OnHingeConnectedChanged?.Invoke(isConnected);
		}
	}
}
