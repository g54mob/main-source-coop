using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Zorro.Core.Serizalization;

public class EmoteItem : ItemInstanceBehaviour
{
	public TextMeshPro emoteText;

	private OnOffEntry onOffEntry;

	private bool invalid;

	public GameObject useEffectPref;

	private void Start()
	{
	}

	private void Update()
	{
		if (!invalid && isHeldByMe && !Player.localPlayer.HasLockedInput() && Player.localPlayer.input.clickWasPressed && Player.localPlayer.TryGetInventory(out var o) && o.TryGetSlot(Player.localPlayer.data.selectedItemSlot, out var slot))
		{
			BinarySerializer binarySerializer = new BinarySerializer();
			binarySerializer.WriteInt(Player.localPlayer.refs.view.ViewID);
			binarySerializer.WriteByte(itemInstance.item.id);
			Debug.Log("Use book from book");
			Player.localPlayer.refs.emotes.DoBookEquipEffect(Player.localPlayer.refs.view.ViewID, itemInstance.item.id, base.transform.position, base.transform.rotation);
			slot.Clear();
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (itemInstance.item == null)
		{
			Debug.LogError("item is null");
			emoteText.text = "ITEM IS NULL";
			return;
		}
		if (itemInstance.item.emoteInfo == null)
		{
			invalid = true;
			emoteText.text = "INVALID EMOTE";
			return;
		}
		string text = itemInstance.item.emoteInfo.displayName;
		if (Enum.TryParse<LocalizationKeys.Keys>(itemInstance.item.name.Trim().Replace(" ", "") + "_Text", out var result))
		{
			text = LocalizationKeys.GetLocalizedString(result);
		}
		emoteText.text = text;
		Debug.Log("Lets go rpc");
	}
}
