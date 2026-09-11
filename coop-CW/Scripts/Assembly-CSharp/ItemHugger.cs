using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class ItemHugger : ItemInstanceBehaviour
{
	public Item hugEmote;

	private Player playerHoldingItem;

	public float healInterval = 0.5f;

	public float fullHealTime = 10f;

	private float timeSinceLastHeal;

	private OnOffEntry onOffEntry;

	private bool playedEmote;

	public GameObject trigger;

	[SerializeField]
	public float hugForce;

	public float hugBoxDistance = 0.5f;

	public PlayerEmotes PlayerEmotes => playerHoldingItem.refs.emotes;

	public void TriggerSaysTriggerStay(Collider other)
	{
		if (playerHoldingItem.transform.root == other.transform.root || !other.transform.root.TryGetComponent<Player>(out var component))
		{
			return;
		}
		Rigidbody rig = component.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
		Vector3 vector = trigger.transform.position - rig.position;
		Vector3 vector2 = vector.normalized * Mathf.Clamp(vector.magnitude, 0f, 1f) * hugForce;
		rig.AddForce(vector2, ForceMode.Acceleration);
		playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.AddForce(-vector2, ForceMode.Acceleration);
		if (timeSinceLastHeal > healInterval)
		{
			float healAmount = Player.PlayerData.maxHealth / fullHealTime;
			if (component.CallHeal(healAmount))
			{
				timeSinceLastHeal = 0f;
			}
		}
	}

	private void Update()
	{
		if (isHeld)
		{
			Rigidbody rig = playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			trigger.transform.position = rig.position + playerHoldingItem.data.lookDirection.xoz().normalized * hugBoxDistance;
			trigger.transform.rotation = Quaternion.LookRotation(playerHoldingItem.data.lookDirection.xoz().normalized, Vector3.up);
		}
		timeSinceLastHeal += Time.deltaTime;
		if (isHeldByMe && !onOffEntry.on && Player.localPlayer.input.clickIsPressed && !Player.localPlayer.HasLockedInput())
		{
			onOffEntry.on = true;
			onOffEntry.SetDirty();
		}
		if (PhotonNetwork.IsMasterClient && !isHeld && onOffEntry.on)
		{
			onOffEntry.on = false;
			onOffEntry.SetDirty();
			Debug.LogError("Hugger deactivated!");
		}
		if (onOffEntry.on && !PlayerEmotes.IsPlayingEmote && !playedEmote && isHeldByMe)
		{
			playedEmote = true;
			PlayerEmotes.PlayEmote(hugEmote);
		}
		if (onOffEntry.on && playedEmote && !PlayerEmotes.IsPlayingEmote && isHeldByMe)
		{
			playedEmote = false;
			onOffEntry.on = false;
			onOffEntry.SetDirty();
		}
		if (onOffEntry.on)
		{
			if (!trigger.gameObject.activeSelf)
			{
				trigger.gameObject.SetActive(value: true);
				Debug.LogError("Hugger trigger activated!");
			}
		}
		else if (trigger.gameObject.activeSelf)
		{
			trigger.gameObject.SetActive(value: false);
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		playerHoldingItem = base.transform.root.GetComponent<Player>();
		if (data.TryGetEntry<OnOffEntry>(out onOffEntry))
		{
			Debug.Log($"OnOff entry found, state: {onOffEntry.on}");
			return;
		}
		onOffEntry = new OnOffEntry
		{
			on = false
		};
		data.AddDataEntry(onOffEntry);
		Debug.Log("OnOff entry not found, adding new entry with false.");
	}
}
