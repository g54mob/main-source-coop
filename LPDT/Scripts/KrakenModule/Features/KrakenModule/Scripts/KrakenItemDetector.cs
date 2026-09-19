using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public class KrakenItemDetector : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _ignoreLayerMask;

		private readonly HashSet<IPointGrabable> _itemsInZone = new HashSet<IPointGrabable>();

		public IReadOnlyCollection<IPointGrabable> ItemsInZone => _itemsInZone;

		public event Action<IPointGrabable> OnGrabbableDetected;

		public event Action<IPointGrabable> OnGrabbableExited;

		public void PruneInvalidItems()
		{
			_itemsInZone.RemoveWhere((IPointGrabable item) => !IsItemAlive(item) || KrakenThrowAssignmentService.IsWornHeadwear(item));
		}

		public void ForgetItem(IPointGrabable item)
		{
			if (IsItemAlive(item))
			{
				_itemsInZone.Remove(item);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (((1 << other.gameObject.layer) & _ignoreLayerMask.value) == 0)
			{
				IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
				if (IsItemAlive(pointGrabable) && !KrakenThrowAssignmentService.IsWornHeadwear(pointGrabable) && _itemsInZone.Add(pointGrabable))
				{
					this.OnGrabbableDetected?.Invoke(pointGrabable);
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
			if (IsItemAlive(pointGrabable) && _itemsInZone.Remove(pointGrabable))
			{
				this.OnGrabbableExited?.Invoke(pointGrabable);
			}
		}

		private static T FindComponent<T>(GameObject other)
		{
			T val = other.GetComponent<T>();
			if (val == null)
			{
				val = other.GetComponentInParent<T>();
			}
			if (val == null)
			{
				val = other.GetComponentInChildren<T>();
			}
			return val;
		}

		private static bool IsItemAlive(IPointGrabable item)
		{
			if (item == null)
			{
				return false;
			}
			if (item is UnityEngine.Object obj && obj == null)
			{
				return false;
			}
			if (item.IsMainRagdollGrabable)
			{
				return false;
			}
			return true;
		}
	}
}
