using UnityEngine;

namespace DefaultNamespace
{
	public class BatteryRechargeStation : MonoBehaviour
	{
		public float rechargeRate = 10f;

		public float range = 9f;

		public ParticleSystem part;

		public Transform field;

		public void LateUpdate()
		{
			if (DontGo(out var inv))
			{
				if (part.isPlaying)
				{
					part.Stop();
				}
				return;
			}
			bool flag = false;
			InventorySlot[] slots = inv.slots;
			foreach (InventorySlot inventorySlot in slots)
			{
				if (inventorySlot.ItemInSlot.item != null && inventorySlot.ItemInSlot.data.TryGetEntry<BatteryEntry>(out var t) && !(t.m_maxCharge <= t.m_charge))
				{
					float chargeToAdd = rechargeRate * Time.deltaTime;
					t.AddCharge(chargeToAdd);
					flag = true;
				}
			}
			if (flag)
			{
				if ((bool)Player.localPlayer.data.currentItem)
				{
					field.transform.position = Player.localPlayer.data.currentItem.transform.position;
				}
				else
				{
					field.transform.position = Player.localPlayer.Center();
				}
				if (!part.isPlaying)
				{
					part.Play();
				}
			}
			else if (part.isPlaying)
			{
				part.Stop();
			}
		}

		private bool DontGo(out PlayerInventory inv)
		{
			inv = null;
			if (Player.localPlayer == null)
			{
				return true;
			}
			if (Vector3.Distance(Player.localPlayer.Center(), base.transform.position) > range)
			{
				return true;
			}
			if (!Player.localPlayer.TryGetInventory(out inv))
			{
				return true;
			}
			return false;
		}
	}
}
