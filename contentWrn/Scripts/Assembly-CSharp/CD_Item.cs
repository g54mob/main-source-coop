using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class CD_Item : ItemInstanceBehaviour
{
	private StashAbleEntry stashAbleEntry;

	private float timeSinceHeld;

	public float timeToTeleport = 30f;

	private ExtractVideoMachine machine;

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (!isHeld)
			{
				timeSinceHeld += Time.deltaTime;
			}
			if (timeSinceHeld > timeToTeleport && (double)Vector3.Distance(base.transform.position, machine.m_discRespawnPoint.position) > 0.4)
			{
				base.transform.position = machine.m_discRespawnPoint.position;
				base.transform.rotation = machine.m_discRespawnPoint.rotation;
				Rigidbody component = GetComponent<Rigidbody>();
				component.linearVelocity = 0.ToVec();
				component.AddForce(Vector3.up * 0.01f, ForceMode.Impulse);
			}
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		machine = Object.FindObjectOfType<ExtractVideoMachine>();
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			Debug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
			return;
		}
		stashAbleEntry = new StashAbleEntry
		{
			isStashAble = false
		};
		data.AddDataEntry(stashAbleEntry);
		Debug.Log("stashAbleEntry entry not found, adding new entry with true.");
	}
}
