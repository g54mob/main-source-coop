using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts
{
	public class SlotMachineItemSensor : MonoBehaviour
	{
		[SerializeField]
		private SlotMachineBehaviour _slotMachineBehaviour;

		private void OnTriggerEnter(Collider other)
		{
			if (!(_slotMachineBehaviour == null) && TryResolveItem(other, out var item, out var itemId))
			{
				_slotMachineBehaviour.HandleZoneItemEnter(item, itemId);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(_slotMachineBehaviour == null) && TryResolveItem(other, out var item, out var itemId))
			{
				_slotMachineBehaviour.HandleZoneItemExit(item, itemId);
			}
		}

		private static bool TryResolveItem(Collider other, out IItem item, out NetworkId itemId)
		{
			item = other.GetComponentInParent<IItem>();
			itemId = default(NetworkId);
			if (item == null || !item.IsSpawned || item.IsConsumed || item.NetworkObject == null || !item.NetworkObject.IsValid)
			{
				return false;
			}
			itemId = item.NetworkObject.Id;
			return true;
		}
	}
}
