using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemGooBall : ItemInstanceBehaviour
{
	private StashAbleEntry stashAbleEntry;

	private LifeTimeEntry m_lifeTimeEntry;

	[FormerlySerializedAs("maxLifeTime")]
	public float explodeTime = 90f;

	private OnOffEntry usedEntry;

	public GameObject explodedGoopPref;

	[FormerlySerializedAs("sphereCollider_gc")]
	public SphereCollider sphereCollider;

	public AudioLoop tickingSound;

	public GameObject lightObj;

	private bool exploded;

	private float timeNotHeld;

	private void Update()
	{
		bool flag = usedEntry.on && !exploded;
		if (flag != tickingSound.enabled)
		{
			tickingSound.enabled = flag;
		}
		if (flag)
		{
			tickingSound.pitch = explodeTime / m_lifeTimeEntry.m_lifeTimeLeft + 0.1f;
		}
		if (!isHeld)
		{
			timeNotHeld += Time.deltaTime;
		}
		else
		{
			timeNotHeld = 0f;
		}
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !usedEntry.on && Player.localPlayer.input.clickWasPressed)
		{
			usedEntry.on = true;
			stashAbleEntry.isStashAble = false;
			stashAbleEntry.SetDirty();
			usedEntry.SetDirty();
		}
		if (usedEntry.on)
		{
			if (!lightObj.activeSelf)
			{
				lightObj.SetActive(value: true);
			}
			m_lifeTimeEntry.m_lifeTimeLeft -= Time.deltaTime;
		}
		if (m_lifeTimeEntry.m_lifeTimeLeft <= 0f && isHeldByMe)
		{
			Player.localPlayer.refs.items.DropItem(Player.localPlayer.refs.items.m_displayingSlot);
		}
	}

	private void SpawnExplosive()
	{
		PhotonNetwork.Instantiate(explodedGoopPref.name, base.transform.position, base.transform.rotation, 0);
		PhotonNetwork.Destroy(GetComponentInParent<PhotonView>().gameObject);
		exploded = true;
	}

	private void OnCollisionEnter(Collision other)
	{
		CheckCollision(other);
	}

	private void CheckCollision(Collision other)
	{
		if (PhotonNetwork.IsMasterClient && usedEntry.on && !exploded && timeNotHeld > 0.1f)
		{
			SpawnExplosive();
		}
	}

	private void OnCollisionStay(Collision other)
	{
		CheckCollision(other);
	}

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
				m_lifeTimeLeft = explodeTime,
				m_maxLifeTime = explodeTime
			};
			data.AddDataEntry(m_lifeTimeEntry);
			Debug.Log("LifeTime entry not found, adding new entry with full lifetime.");
		}
		if (data.TryGetEntry<OnOffEntry>(out usedEntry))
		{
			Debug.Log($"OnOff entry found, state: {usedEntry.on}");
			return;
		}
		usedEntry = new OnOffEntry
		{
			on = false
		};
		data.AddDataEntry(usedEntry);
		Debug.Log("OnOff entry not found, adding new entry with false.");
	}
}
