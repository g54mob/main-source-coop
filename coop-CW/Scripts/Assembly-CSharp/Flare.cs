using Photon.Pun;
using UnityEngine;

public class Flare : ItemInstanceBehaviour
{
	public Light m_light;

	public MeshRenderer lightBeam;

	public MeshRenderer brightPart;

	public GameObject trigger;

	private LifeTimeEntry m_lifeTimeEntry;

	private OnOffEntry m_onOffEntry;

	public float maxLifeTime = 90f;

	private StashAbleEntry stashAbleEntry;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			Debug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
		}
		else
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = true
			};
			data.AddDataEntry(stashAbleEntry);
			Debug.Log("stashAbleEntry entry not found, adding new entry with true.");
		}
		if (data.TryGetEntry<LifeTimeEntry>(out m_lifeTimeEntry))
		{
			Debug.Log($"LifeTime entry found, Lifetime: {m_lifeTimeEntry.m_lifeTimeLeft}");
		}
		else
		{
			m_lifeTimeEntry = new LifeTimeEntry
			{
				m_lifeTimeLeft = maxLifeTime,
				m_maxLifeTime = maxLifeTime
			};
			data.AddDataEntry(m_lifeTimeEntry);
			Debug.Log("LifeTime entry not found, adding new entry with full lifetime.");
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
		if (isHeldByMe)
		{
			lightBeam.material.SetFloat("_Strength", lightBeam.material.GetFloat("_Strength") * 0.35f);
		}
	}

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !m_onOffEntry.on && Player.localPlayer.input.clickWasPressed)
		{
			m_onOffEntry.on = !m_onOffEntry.on;
			stashAbleEntry.isStashAble = false;
			m_onOffEntry.SetDirty();
		}
		if (m_onOffEntry.on)
		{
			m_lifeTimeEntry.m_lifeTimeLeft -= Time.deltaTime;
		}
		bool flag = m_onOffEntry.on && m_lifeTimeEntry.m_lifeTimeLeft > 0f;
		m_light.enabled = flag;
		lightBeam.enabled = flag;
		brightPart.enabled = flag;
		if (trigger.activeSelf != flag)
		{
			trigger.SetActive(flag);
		}
	}
}
