using Features.GrabModule.Scripts;
using UnityEngine;

namespace Features.InteractModule.Scripts.AreaItemAvailability
{
	public class AreaItemsUnbreakable : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			GetGrabbable(other)?.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
		}

		private void OnTriggerExit(Collider other)
		{
			GetGrabbable(other)?.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
		}

		private IPointGrabable GetGrabbable(Collider other)
		{
			IPointGrabable pointGrabable = other.GetComponent<IPointGrabable>();
			if (pointGrabable == null)
			{
				pointGrabable = other.GetComponentInParent<IPointGrabable>();
			}
			return pointGrabable;
		}
	}
}
