using System.Collections.Generic;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.InteractModule.Scripts.AreaItemAvailability
{
	public class AreaItemAvailabilityDisablerForEnemy : MonoBehaviour
	{
		public List<IItem> _itemsInArea = new List<IItem>();

		private void OnTriggerEnter(Collider other)
		{
			IItem item = other.gameObject.GetComponent<IItem>();
			if (item == null)
			{
				item = other.gameObject.GetComponentInParent<IItem>();
			}
			if (item != null && !_itemsInArea.Contains(item))
			{
				item.AvailableForEnemy = false;
				_itemsInArea.Add(item);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			IItem item = other.gameObject.GetComponent<IItem>();
			if (item == null)
			{
				item = other.gameObject.GetComponentInParent<IItem>();
			}
			if (item != null && _itemsInArea.Contains(item))
			{
				item.AvailableForEnemy = true;
				_itemsInArea.Remove(item);
			}
		}
	}
}
