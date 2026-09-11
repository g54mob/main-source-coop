using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

public class ArtifactRadio : ItemInstanceBehaviour, IArtifactContent
{
	private IntRangeEntry chargesEntry;

	private OnOffEntry onOffEntry;

	private StashAbleEntry stashAbleEntry;

	private TimeEntry radioTimeEntry;

	public float maxBatteryCharge = 45f;

	private BatteryEntry batteryEntry;

	public MeshRenderer bright;

	public AudioLoop music;

	private float timeOnTheGround;

	public float maxTimeOnGround;

	private float timeToAlert;

	public float alertIntervall = 0.15f;

	public bool IsHeld => isHeld;

	public bool IsActive => onOffEntry.on;

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && Player.localPlayer.input.clickWasPressed)
		{
			onOffEntry.on = !onOffEntry.on;
			if (onOffEntry.on)
			{
				timeToAlert = alertIntervall;
			}
			onOffEntry.SetDirty();
		}
		if (batteryEntry.m_charge < 0f)
		{
			onOffEntry.on = false;
			onOffEntry.SetDirty();
		}
		bool flag = onOffEntry.on;
		if (flag != music.enabled)
		{
			if (flag)
			{
				music.SetTime(radioTimeEntry.currentTime);
			}
			music.enabled = flag;
			if (flag && music.clip.name.Contains("Phonk"))
			{
				PlatformManager.UnlockAchievement(Achievements.ACH_PHONK);
			}
		}
		bright.enabled = flag;
		if (flag)
		{
			batteryEntry.m_charge -= Time.deltaTime;
			radioTimeEntry.currentTime += Time.deltaTime;
		}
		if (!isHeld)
		{
			timeOnTheGround += Time.deltaTime;
		}
		else
		{
			timeOnTheGround = 0f;
		}
		if (timeOnTheGround > maxTimeOnGround && onOffEntry.on)
		{
			onOffEntry.on = false;
			Debug.Log("Radio turned off because it was on the ground for too long");
			onOffEntry.SetDirty();
		}
		if (flag)
		{
			timeToAlert -= Time.deltaTime;
		}
		if (flag && timeToAlert < 0f)
		{
			SFX_Player.instance.PlayNoise(base.transform.position, 30f);
			timeToAlert = alertIntervall;
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		Random.State state = Random.state;
		Random.InitState(GameAPI.seed);
		AudioLoop[] components = GetComponents<AudioLoop>();
		music = components[Random.Range(0, components.Length)];
		Random.state = state;
		if (!data.TryGetEntry<BatteryEntry>(out batteryEntry))
		{
			batteryEntry = new BatteryEntry
			{
				m_charge = maxBatteryCharge,
				m_maxCharge = maxBatteryCharge
			};
			data.AddDataEntry(batteryEntry);
		}
		if (data.TryGetEntry<OnOffEntry>(out onOffEntry))
		{
			Debug.Log($"OnOff entry found, state: {onOffEntry.on}");
		}
		else
		{
			onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(onOffEntry);
			Debug.Log("OnOff entry not found, adding new entry with false.");
		}
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			Debug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
		}
		else
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = false
			};
			data.AddDataEntry(stashAbleEntry);
			Debug.Log("stashAbleEntry entry not found, adding new entry with false.");
		}
		if (!data.TryGetEntry<TimeEntry>(out radioTimeEntry))
		{
			radioTimeEntry = new TimeEntry
			{
				currentTime = 0f
			};
			data.AddDataEntry(radioTimeEntry);
		}
	}
}
