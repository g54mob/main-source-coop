using System.Collections.Generic;
using Features.GrabModule.Scripts;
using UnityEngine;

namespace Features.InteractModule.Scripts.ChestGrab
{
	public class LidItemsAuthorityFollower : MonoBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _authoritySource;

		[SerializeField]
		private int _requestCountPerTick = 10;

		private HashSet<IPointGrabable> _trackedItems = new HashSet<IPointGrabable>();

		private void FixedUpdate()
		{
			if (_trackedItems.Count == 0 || _authoritySource.Object == null)
			{
				return;
			}
			if (!_authoritySource.Object.HasStateAuthority)
			{
				ReleaseDroppedItemClaims();
				return;
			}
			int playerId = _authoritySource.Object.StateAuthority.PlayerId;
			int num = 0;
			foreach (IPointGrabable trackedItem in _trackedItems)
			{
				if (trackedItem != null && !(trackedItem.NetworkObject == null) && trackedItem.GrabbedByPlayersCount <= 0 && trackedItem.GrabbedBySomethingCount <= 0)
				{
					if (trackedItem.NetworkObject.StateAuthority.PlayerId == playerId)
					{
						trackedItem.IsAuthorityRequested = true;
					}
					else if (!trackedItem.IsAuthorityRequested && num < _requestCountPerTick)
					{
						num++;
						trackedItem.IsAuthorityRequested = true;
						trackedItem.RequestStateAuthorityRPC(playerId);
					}
				}
			}
		}

		private void ReleaseDroppedItemClaims()
		{
			foreach (IPointGrabable trackedItem in _trackedItems)
			{
				if (trackedItem != null && !(trackedItem.NetworkObject == null) && trackedItem.NetworkObject.HasStateAuthority && trackedItem.GrabbedByPlayersCount <= 0 && trackedItem.GrabbedBySomethingCount <= 0)
				{
					trackedItem.IsAuthorityRequested = false;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			IPointGrabable pointGrabable = other.GetComponent<IPointGrabable>() ?? other.GetComponentInParent<IPointGrabable>();
			if (pointGrabable != null && !(pointGrabable.NetworkObject == null) && pointGrabable != _authoritySource)
			{
				_trackedItems.Add(pointGrabable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			IPointGrabable pointGrabable = other.GetComponent<IPointGrabable>() ?? other.GetComponentInParent<IPointGrabable>();
			if (pointGrabable != null && _trackedItems.Remove(pointGrabable) && pointGrabable.GrabbedByPlayersCount == 0 && pointGrabable.GrabbedBySomethingCount == 0)
			{
				pointGrabable.IsAuthorityRequested = false;
			}
		}
	}
}
