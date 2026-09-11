using System;
using System.Collections;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;
using Zorro.Core.Serizalization;

public class ShockStick : ItemInstanceBehaviour
{
	public Light m_light;

	public MeshRenderer rend;

	public ParticleSystem part;

	public GameObject trigger;

	public BatteryDisplay m_batteryDisplay;

	private BatteryEntry m_batteryEntry;

	private OnOffEntry m_onOffEntry;

	private int lastFrame;

	public float shockBatteryCost = 40f;

	public SFX_Instance[] shockImpactSound;

	public Light sparkLight;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<BatteryEntry>(out m_batteryEntry))
		{
			Debug.Log($"Battery entry found, charge: {m_batteryEntry.m_charge}");
		}
		else
		{
			m_batteryEntry = new BatteryEntry
			{
				m_charge = 150f,
				m_maxCharge = 150f
			};
			data.AddDataEntry(m_batteryEntry);
			Debug.Log("Battery entry not found, adding new entry with full charge.");
		}
		if (data.TryGetEntry<OnOffEntry>(out m_onOffEntry))
		{
			Debug.Log($"OnOff entry found, state: {m_onOffEntry.on}");
		}
		else
		{
			m_onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(m_onOffEntry);
			Debug.Log("OnOff entry not found, adding new entry with false.");
		}
		itemInstance.RegisterRPC(ItemRPC.RPC0, RPC_Shock);
	}

	private void RPC_Shock(BinaryDeserializer deserializer)
	{
		Player component = PhotonNetwork.GetPhotonView(deserializer.ReadInt()).GetComponent<Player>();
		m_batteryEntry.m_charge -= shockBatteryCost;
		m_onOffEntry.on = false;
		m_onOffEntry.SetDirty();
		component.refs.ragdoll.TaseShock(5f);
		if (component.refs.view.IsMine)
		{
			GamefeelHandler.instance.perlin.AddShake(4f, 0.3f, 20f);
		}
		StartCoroutine(IShock());
		for (int i = 0; i < shockImpactSound.Length; i++)
		{
			shockImpactSound[i].Play(base.transform.position);
		}
		if (isSimulatedByMe && component.ai && component.name.Contains("BigSlap", StringComparison.InvariantCultureIgnoreCase))
		{
			PlatformManager.UnlockAchievement(Achievements.ACH_ZAP_BIGSLAP);
		}
	}

	public void OnShock(Player playerToShock)
	{
		if (isSimulatedByMe && lastFrame != Time.frameCount)
		{
			lastFrame = Time.frameCount;
			BinarySerializer binarySerializer = new BinarySerializer();
			binarySerializer.WriteInt(playerToShock.refs.view.ViewID);
			itemInstance.CallRPC(ItemRPC.RPC0, binarySerializer);
			for (int i = 0; i < shockImpactSound.Length; i++)
			{
				shockImpactSound[i].Play(base.transform.position);
			}
		}
	}

	private IEnumerator IShock()
	{
		sparkLight.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sparkLight.enabled = false;
	}

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && Player.localPlayer.input.clickWasPressed)
		{
			m_onOffEntry.on = !m_onOffEntry.on;
			m_onOffEntry.SetDirty();
		}
		if (m_onOffEntry.on)
		{
			m_batteryEntry.m_charge -= Time.deltaTime;
		}
		m_batteryDisplay.SetCharge(m_batteryEntry.GetPercentage());
		bool flag = m_onOffEntry.on && m_batteryEntry.m_charge > 0f;
		m_light.enabled = flag;
		rend.enabled = flag;
		if (part.isPlaying && !flag)
		{
			part.Stop();
		}
		if (!part.isPlaying && flag)
		{
			part.Play();
		}
		if (trigger.activeSelf != flag)
		{
			trigger.SetActive(flag);
		}
	}
}
