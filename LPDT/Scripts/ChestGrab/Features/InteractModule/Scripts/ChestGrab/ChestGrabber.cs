using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.InteractModule.Scripts.ChestGrab
{
	[NetworkBehaviourWeaved(0)]
	public class ChestGrabber : NetworkBehaviour
	{
		[SerializeField]
		private ChestInteractable _chestInteractable;

		[SerializeField]
		private Transform _grabablesMagnet;

		private List<IPointGrabable> _grabables = new List<IPointGrabable>();

		private List<IPointGrabable> _grabablesRemoved = new List<IPointGrabable>();

		private bool _isPositionsStored;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.TryGetComponent<IPointGrabable>(out var component) && !_grabables.Contains(component))
			{
				_grabables.Add(component);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.TryGetComponent<IPointGrabable>(out var component) && _grabablesRemoved.Contains(component))
			{
				_grabablesRemoved.Remove(component);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (_grabables.Count == 0)
			{
				return;
			}
			List<IPointGrabable> list = new List<IPointGrabable>();
			foreach (IPointGrabable grabable in _grabables)
			{
				if (grabable.GrabbedByPlayers.Count > 0)
				{
					list.Add(grabable);
				}
				if (grabable.NetworkObject == null)
				{
					list.Add(grabable);
				}
			}
			foreach (IPointGrabable item in list)
			{
				if (_grabablesRemoved.Contains(item) || item.NetworkObject == null)
				{
					_grabables.Remove(item);
					item.Rigidbody.isKinematic = false;
				}
			}
			if (!_chestInteractable.IsOpen)
			{
				foreach (IPointGrabable grabable2 in _grabables)
				{
					if (grabable2.NetworkObject.StateAuthority != base.Runner.LocalPlayer)
					{
						grabable2.NetworkObject.RequestStateAuthority();
					}
					grabable2.Rigidbody.linearVelocity = Vector3.zero;
					grabable2.Rigidbody.angularVelocity = Vector3.zero;
					grabable2.Rigidbody.MovePosition(_grabablesMagnet.position);
					grabable2.Rigidbody.isKinematic = true;
				}
				return;
			}
			foreach (IPointGrabable grabable3 in _grabables)
			{
				grabable3.Rigidbody.isKinematic = false;
			}
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
