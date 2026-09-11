using System.Collections;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;
using pworld.Scripts.Extensions;

public class Defib : ItemInstanceBehaviour
{
	private static readonly int Strength = Shader.PropertyToID("_Strength");

	public Light m_light;

	public MeshRenderer lightBeam;

	public MeshRenderer brightPart;

	public GameObject trigger;

	public BatteryDisplay m_batteryDisplay;

	public float maxCharge = 150f;

	public int maxCharges = 3;

	public float attackForce = 10f;

	public float onTime = 0.5f;

	public float elapsedOn;

	public float moveTime = 1f;

	private BatteryEntry m_batteryEntry;

	private OnOffEntry m_onOffEntry;

	private Player playerHoldingItem;

	public AnimationCurve forceCurve;

	private bool startedDefibMove;

	public float activationPoint = 0.2f;

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !m_onOffEntry.on && Player.localPlayer.input.clickIsPressed && m_batteryEntry.m_charge > 0f)
		{
			m_onOffEntry.on = !m_onOffEntry.on;
			m_onOffEntry.SetDirty();
		}
		if (m_onOffEntry.on && m_batteryEntry.m_charge <= 0f)
		{
			m_onOffEntry.on = !m_onOffEntry.on;
			m_onOffEntry.SetDirty();
		}
		int num = maxCharges / m_batteryDisplay.m_renderer.Length;
		m_batteryDisplay.SetCharge(m_batteryEntry.GetPercentage() * (float)num - 0.01f);
		if (m_onOffEntry.on)
		{
			if (!startedDefibMove)
			{
				startedDefibMove = true;
				StartCoroutine(DoForce());
			}
		}
		else
		{
			m_light.enabled = false;
			lightBeam.enabled = false;
			brightPart.enabled = false;
			trigger.SetActive(value: false);
		}
		IEnumerator DoForce()
		{
			Rigidbody myHand = playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
			float animLength = forceCurve.GetTimeLength();
			float elapsed = 0f;
			float scaler = animLength / moveTime;
			while (elapsed < animLength)
			{
				elapsed += Time.deltaTime * scaler;
				float num2 = forceCurve.Evaluate(elapsed);
				if (num2 > activationPoint)
				{
					m_light.enabled = true;
					lightBeam.enabled = true;
					brightPart.enabled = true;
					trigger.SetActive(value: true);
				}
				myHand.AddForce(playerHoldingItem.data.lookDirection * (num2 * attackForce * Time.deltaTime), ForceMode.VelocityChange);
				yield return null;
			}
			elapsedOn = 0f;
			m_onOffEntry.on = false;
			m_onOffEntry.SetDirty();
			startedDefibMove = false;
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		playerHoldingItem = base.transform.root.GetComponent<Player>();
		if (data.TryGetEntry<BatteryEntry>(out m_batteryEntry))
		{
			Debug.Log($"Battery entry found, charge: {m_batteryEntry.m_charge}");
		}
		else
		{
			m_batteryEntry = new BatteryEntry
			{
				m_charge = maxCharge,
				m_maxCharge = maxCharge
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
		if (isHeldByMe)
		{
			lightBeam.material.SetFloat(Strength, lightBeam.material.GetFloat(Strength) * 0.35f);
		}
	}

	public void OnDefibTrigger(Collider other, bool debug = false)
	{
		Player component = other.transform.root.GetComponent<Player>();
		if (!component || !component.data.dead)
		{
			return;
		}
		float charge = m_batteryEntry.m_charge;
		bool num = m_batteryEntry.m_charge > 0f;
		m_batteryEntry.m_charge -= m_batteryEntry.m_maxCharge / (float)maxCharges;
		m_batteryEntry.SetDirty();
		if (num)
		{
			Debug.Log($"Defibrilator used! Charge before={charge:R}/{m_batteryEntry.m_maxCharge:R} after={m_batteryEntry.m_charge:R}/{m_batteryEntry.m_maxCharge:R}");
			component.CallRevive();
			if (isHeldByMe)
			{
				PlatformManager.UnlockAchievement(Achievements.ACH_REVIVE);
			}
		}
	}
}
