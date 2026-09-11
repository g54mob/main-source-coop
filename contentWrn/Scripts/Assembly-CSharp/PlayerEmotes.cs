using System.Collections;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

public class PlayerEmotes : MonoBehaviour
{
	public Item chokedEmote;

	public Item childUnsafeEmote;

	public Item childSafeEmote;

	public Item latestEmotePlayed;

	private Player player;

	private PhotonView photonView_g;

	private PlayerVoiceHandler playerVoice;

	public bool IsPlayingEmote
	{
		get
		{
			if (player == null)
			{
				return false;
			}
			if (player.data == null)
			{
				return false;
			}
			return player.data.emoteTime > 0f;
		}
	}

	private IEnumerator Start()
	{
		player = GetComponent<Player>();
		photonView_g = GetComponent<PhotonView>();
		while (PlayerVoiceHandler.Instance == null)
		{
			yield return null;
		}
		playerVoice = PlayerVoiceHandler.Instance;
	}

	private void Update()
	{
		if (player.refs.view.IsMine && !(player.data.emoteTime > 0f) && player.input.emoteIsPressed)
		{
			if (!player.TryGetInventory(out var o))
			{
				Debug.LogError("Failed to get inventory");
			}
			else if (!(o.emote == null))
			{
				photonView_g.RPC("StartEmote", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	private void StartEmote()
	{
		if (!player.TryGetInventory(out var o))
		{
			Debug.LogError("Failed to get inventory");
		}
		else if (!(o.emote == null))
		{
			DoEmote(o.emote);
		}
	}

	public void PlayChokedAnimation()
	{
		PlayEmote(chokedEmote);
	}

	public void PlayEmote(Item emoteItem)
	{
		if (emoteItem.emoteInfo == null)
		{
			Debug.LogError("Tried to play incorrenct emote");
			return;
		}
		photonView_g.RPC("RPC_PlayEmote", RpcTarget.All, emoteItem.id);
	}

	[PunRPC]
	private void RPC_PlayEmote(byte itemId)
	{
		if (!ItemDatabase.TryGetItemFromID(itemId, out var item))
		{
			Debug.LogError("Couldnt get item from id");
		}
		DoEmote(item);
	}

	private void DoEmote(Item emote)
	{
		if (emote.emoteInfo.unequip)
		{
			player.data.selectedItemSlot = 5;
		}
		if (!playerVoice.AllowedToCommunicate && emote.id == childUnsafeEmote.id)
		{
			player.refs.animationHandler.PlayEmote(childSafeEmote.emoteInfo.animationName, childSafeEmote.emoteInfo.emoteLength);
			latestEmotePlayed = childSafeEmote;
		}
		else
		{
			player.refs.animationHandler.PlayEmote(emote.emoteInfo.animationName, emote.emoteInfo.emoteLength);
			latestEmotePlayed = emote;
		}
		OnEmote(emote);
	}

	private void OnEmote(Item emote)
	{
		switch (emote.persistentID)
		{
		case "c48eda8b-cbb0-4798-b94a-b966d5004f5f":
		case "a4da2558-d060-4c0d-bac9-05d7d8ed59c1":
			PlatformManager.UnlockAchievement(Achievements.ACH_EMOTE_ANCIENT_GESTURE);
			break;
		case "ec3acf99-c659-400a-83e4-544deb17eba6":
			PlatformManager.UnlockAchievement(Achievements.ACH_EMOTE_PEACE);
			break;
		}
	}

	public void DoBookEquipEffect(int viewID, byte itemId, Vector3 pos, Quaternion rotation)
	{
		Debug.Log("DoBookEquipEffect");
		photonView_g.RPC("RPC_DoBookEquipEffect", RpcTarget.All, viewID, itemId, pos, rotation);
	}

	[PunRPC]
	private void RPC_DoBookEquipEffect(int viewID, byte itemId, Vector3 pos, Quaternion rotation)
	{
		Debug.Log("RPC_DoBookEquipEffect");
		Object obj = Resources.Load("BoookDissolve");
		if (obj == null)
		{
			Debug.LogError("Couldnt load BoookDissolve");
		}
		if (ItemDatabase.TryGetItemFromID(itemId, out var item))
		{
			Debug.Log("do use effect");
			if (!PlayerHandler.instance.TryGetPlayerFromViewID(viewID).TryGetInventory(out var o))
			{
				Debug.LogError("Failed to get inventory");
			}
			o.emote = item;
			Object.Instantiate(obj, pos, rotation);
		}
		else
		{
			Debug.LogError("Failed to get item from id");
		}
	}
}
