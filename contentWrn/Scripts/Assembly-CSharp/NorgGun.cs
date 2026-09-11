using Photon.Pun;
using UnityEngine;
using Zorro.Core.Serizalization;

public class NorgGun : ItemInstanceBehaviour
{
	public BatteryDisplay m_batteryDisplay;

	public float maxCharge = 100f;

	public int maxCharges = 30;

	private BatteryEntry m_batteryEntry;

	public GameObject projectile;

	public Transform firePoint;

	private Player playerHoldingItem;

	private float sinceFire;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		playerHoldingItem = base.transform.root.GetComponent<Player>();
		if (!data.TryGetEntry<BatteryEntry>(out m_batteryEntry))
		{
			m_batteryEntry = new BatteryEntry
			{
				m_charge = maxCharge,
				m_maxCharge = maxCharge
			};
			data.AddDataEntry(m_batteryEntry);
		}
		itemInstance.RegisterRPC(ItemRPC.RPC1, RPCA_FireNorf);
	}

	private void LateUpdate()
	{
		float num = 1f / (float)m_batteryDisplay.m_renderer.Length * (float)maxCharges;
		float num2 = m_batteryEntry.GetPercentage() * num - 0.01f;
		m_batteryDisplay.SetCharge(num2 / (float)m_batteryDisplay.m_renderer.Length);
	}

	private void FixedUpdate()
	{
		sinceFire += Time.fixedDeltaTime;
	}

	private void Update()
	{
		if (!(m_batteryEntry.m_charge <= 0f) && isHeldByMe && Player.localPlayer.input.clickIsPressed && !Player.localPlayer.HasLockedInput() && sinceFire > 0.15f)
		{
			Fire();
		}
	}

	private void Fire()
	{
		m_batteryEntry.m_charge -= m_batteryEntry.m_maxCharge / (float)maxCharges;
		m_batteryEntry.SetDirty();
		sinceFire = 0f;
		BinarySerializer binarySerializer = new BinarySerializer();
		binarySerializer.WriteFloat3(firePoint.position);
		binarySerializer.WriteFloat3(firePoint.forward);
		itemInstance.CallRPC(ItemRPC.RPC1, binarySerializer);
	}

	public void RPCA_FireNorf(BinaryDeserializer deserializer)
	{
		if (isHeld)
		{
			Vector3 position = deserializer.ReadFloat3();
			Vector3 forward = deserializer.ReadFloat3();
			sinceFire = 0f;
			GameObject obj = Object.Instantiate(projectile, position, Quaternion.LookRotation(forward));
			GameAPI.instance.objectSpawnedAction(obj);
			GamefeelHandler.instance.perlin.AddShake(base.transform.position, 2f, 0.15f, 15f, 40f);
			playerHoldingItem.refs.ragdoll.GetBodypart(BodypartType.Item).rig.AddForce(base.transform.forward * -1f, ForceMode.VelocityChange);
		}
	}
}
