using Photon.Pun;
using UnityEngine;

public class HatItem : ItemInstanceBehaviour
{
	private OnOffEntry onOffEntry;

	private bool invalid;

	public Hat hatprefab;

	private void Start()
	{
	}

	private void Update()
	{
		if (!invalid && isHeldByMe && !Player.localPlayer.HasLockedInput() && Player.localPlayer.input.clickWasPressed && Player.localPlayer.TryGetInventory(out var o) && o.TryGetSlot(Player.localPlayer.data.selectedItemSlot, out var slot))
		{
			Debug.Log("Use book from book");
			int indexOfHat = HatDatabase.instance.GetIndexOfHat(hatprefab);
			Player.localPlayer.Call_EquipHat(indexOfHat);
			MetaProgressionHandler.UnlockHat(indexOfHat);
			slot.Clear();
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (itemInstance.item == null)
		{
			Debug.LogError("item is null");
		}
		else if (itemInstance.item.emoteInfo == null)
		{
			invalid = true;
		}
	}
}
