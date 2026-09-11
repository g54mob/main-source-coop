using Photon.Pun;
using UnityEngine;

public class Flashlight : ItemInstanceBehaviour
{
	private static readonly int Strength = Shader.PropertyToID("_Strength");

	public Light m_light;

	public MeshRenderer lightBeam;

	public MeshRenderer brightPart;

	public GameObject trigger;

	public BatteryDisplay m_batteryDisplay;

	private BatteryEntry m_batteryEntry;

	private OnOffEntry m_onOffEntry;

	public float maxCharge = 150f;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<BatteryEntry>(out m_batteryEntry))
		{
			VerboseDebug.Log($"Battery entry found, charge: {m_batteryEntry.m_charge}");
		}
		else
		{
			m_batteryEntry = new BatteryEntry
			{
				m_charge = maxCharge,
				m_maxCharge = maxCharge
			};
			data.AddDataEntry(m_batteryEntry);
			VerboseDebug.Log("Battery entry not found, adding new entry with full charge.");
		}
		if (data.TryGetEntry<OnOffEntry>(out m_onOffEntry))
		{
			VerboseDebug.Log($"OnOff entry found, state: {m_onOffEntry.on}");
		}
		else
		{
			m_onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(m_onOffEntry);
			VerboseDebug.Log("OnOff entry not found, adding new entry with false.");
		}
		if (isHeldByMe)
		{
			lightBeam.material.SetFloat(Strength, lightBeam.material.GetFloat(Strength) * 0.35f);
		}
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
		lightBeam.enabled = flag;
		brightPart.enabled = flag;
		if (trigger.activeSelf != flag)
		{
			trigger.SetActive(flag);
		}
	}
}
