using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;

public class WalkieTalkie : ItemInstanceBehaviour
{
	private static List<WalkieTalkiePlayer> m_PlayersWithWalkieTalkies = new List<WalkieTalkiePlayer>();

	private OnOffEntry m_onOffEntry;

	public BatteryDisplay m_batteryDisplay;

	private PhotonView m_Owner;

	private AudioSource m_RemoteSpeaker;

	private PlayerVoiceHandler _mHandler;

	private static bool m_LocalWalkieOn;

	private bool m_PlayingStatic;

	public SFX_Instance sfxOn;

	public SFX_Instance sfxOff;

	private void Update()
	{
		if (isHeldByMe)
		{
			m_LocalWalkieOn = true;
		}
		if (isHeldByMe && !Player.localPlayer.HasLockedInput())
		{
			if (Player.localPlayer.input.clickWasPressed)
			{
				StartSendingWalkieTalkieVoice();
			}
			if (Player.localPlayer.input.clickWasReleased)
			{
				StopSendingWalkieTalkieVoice();
			}
		}
		else if (!isHeldByMe)
		{
			if (m_onOffEntry.on)
			{
				SendRemoteWalkieTalkieVoice();
			}
			else if (m_Owner != null)
			{
				m_RemoteSpeaker.spatialBlend = 1f;
				StopStaticOnTalkies();
			}
		}
		if (m_onOffEntry.on)
		{
			m_batteryDisplay.SetCharge(Player.localPlayer.data.microphoneValue);
		}
	}

	private void SendRemoteWalkieTalkieVoice()
	{
		if (m_LocalWalkieOn)
		{
			_mHandler.Ignore();
			m_RemoteSpeaker.spatialBlend = 0f;
		}
		else
		{
			PlayStaticOnTalkies();
		}
	}

	private void PlayStaticOnTalkies()
	{
		if (m_PlayingStatic)
		{
			return;
		}
		m_PlayingStatic = true;
		foreach (WalkieTalkiePlayer playersWithWalkieTalky in m_PlayersWithWalkieTalkies)
		{
			playersWithWalkieTalky.PlayStatic();
		}
	}

	private void StopStaticOnTalkies()
	{
		if (!m_PlayingStatic)
		{
			return;
		}
		m_PlayingStatic = false;
		foreach (WalkieTalkiePlayer playersWithWalkieTalky in m_PlayersWithWalkieTalkies)
		{
			playersWithWalkieTalky.KillStatic();
		}
	}

	private void StartSendingWalkieTalkieVoice()
	{
		Player.localPlayer.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig.AddForce(Player.localPlayer.data.lookDirection * 2f, ForceMode.VelocityChange);
		if (!m_onOffEntry.on)
		{
			sfxOn.Play(base.transform.position);
		}
		m_onOffEntry.on = true;
		m_onOffEntry.SetDirty();
		Debug.Log("Walkie start!");
	}

	private void StopSendingWalkieTalkieVoice()
	{
		Debug.Log("Walkie stop!");
		Rigidbody rig = Player.localPlayer.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
		if (rig != null)
		{
			rig.AddForce(Player.localPlayer.data.lookDirection * 2f, ForceMode.VelocityChange);
		}
		m_batteryDisplay.SetCharge(0f);
		if (m_onOffEntry != null)
		{
			if (m_onOffEntry.on)
			{
				sfxOff.Play(base.transform.position);
			}
			m_onOffEntry.on = false;
			m_onOffEntry.SetDirty();
		}
		StopStaticOnTalkies();
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		Debug.Log("Config WalkieTalkie!");
		m_Owner = playerView;
		if (!isHeldByMe && m_Owner != null)
		{
			m_RemoteSpeaker = m_Owner.GetComponentInChildren<PhotonVoiceView>().GetComponent<AudioSource>();
			_mHandler = m_RemoteSpeaker.GetComponent<PlayerVoiceHandler>();
		}
		if (m_Owner != null)
		{
			AddOwnerToTalkieList();
		}
		if (isHeldByMe)
		{
			Debug.Log("Local Player has Walkie!");
		}
		if (data.TryGetEntry<OnOffEntry>(out m_onOffEntry))
		{
			m_onOffEntry.on = false;
			Debug.Log($"OnOff entry found, state: {m_onOffEntry.on}");
			return;
		}
		m_onOffEntry = new OnOffEntry
		{
			on = false
		};
		data.AddDataEntry(m_onOffEntry);
		Debug.Log("OnOff entry not found, adding new entry with false.");
	}

	private void AddOwnerToTalkieList()
	{
		WalkieTalkiePlayer walkieTalkiePlayer = m_Owner.GetComponent<Player>().refs.walkieTalkiePlayer;
		if (!m_PlayersWithWalkieTalkies.Contains(walkieTalkiePlayer))
		{
			m_PlayersWithWalkieTalkies.Add(walkieTalkiePlayer);
			Debug.Log("Adding walkieTalkie Player: " + m_PlayersWithWalkieTalkies.Count + " total now!");
		}
	}

	private void RemoveOwnerFromTalkieList()
	{
		WalkieTalkiePlayer walkieTalkiePlayer = m_Owner.GetComponent<Player>().refs.walkieTalkiePlayer;
		if (m_PlayersWithWalkieTalkies.Contains(walkieTalkiePlayer))
		{
			m_PlayersWithWalkieTalkies.Remove(walkieTalkiePlayer);
			Debug.Log("Removing walkieTalkie Player: " + m_PlayersWithWalkieTalkies.Count + " total now!");
		}
	}

	private void OnDestroy()
	{
		StopSendingWalkieTalkieVoice();
		if (!(m_Owner != null))
		{
			return;
		}
		bool flag = false;
		if (isHeldByMe)
		{
			m_LocalWalkieOn = false;
		}
		if (GlobalPlayerData.TryGetPlayerData(m_Owner.Controller, out var globalPlayerData))
		{
			Debug.Log("Checking for walkie in bag?");
			InventorySlot[] slots = globalPlayerData.inventory.slots;
			foreach (InventorySlot inventorySlot in slots)
			{
				if (!(inventorySlot.ItemInSlot.item == null) && inventorySlot.ItemInSlot.item.name.ToLower().Contains("walkietalkie"))
				{
					Debug.Log("Found Walkie in slot: " + inventorySlot.SlotID);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			RemoveOwnerFromTalkieList();
			Debug.Log("Player has no WalkieTalkie anymore!");
		}
	}
}
