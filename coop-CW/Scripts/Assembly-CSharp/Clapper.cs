using Photon.Pun;
using UnityEngine;

public class Clapper : ItemInstanceBehaviour, IArtifactContent
{
	public Transform top;

	private OnOffEntry onOffEntry;

	private StashAbleEntry stashAbleEntry;

	public SFX_Instance sfx;

	public float offAngle = -45f;

	public float onAngle = -90f;

	private float angle;

	public float spring = 15f;

	public float drag = 15f;

	public float bounce = 0.5f;

	public float vel;

	public float bounceThreshold = 1f;

	public bool IsHeld => isHeld;

	public bool IsActive => onOffEntry.on;

	private void Start()
	{
		angle = offAngle;
	}

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput())
		{
			if (Player.localPlayer.input.clickIsPressed)
			{
				if (!onOffEntry.on)
				{
					onOffEntry.on = true;
					onOffEntry.SetDirty();
				}
			}
			else if (onOffEntry.on)
			{
				onOffEntry.on = false;
				onOffEntry.SetDirty();
			}
		}
		bool num = onOffEntry.on;
		float num2 = 0f;
		vel = FRILerp.Lerp(target: (((!num) ? offAngle : onAngle) - angle) * spring, from: vel, speed: drag);
		angle += vel * Time.deltaTime;
		if (angle < onAngle && vel < 0f && Mathf.Abs(vel) > bounceThreshold)
		{
			vel *= 0f - bounce;
			sfx.Play(base.transform.position);
		}
		top.localEulerAngles = new Vector3(angle, 0f, 0f);
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		Random.State state = Random.state;
		Random.InitState(GameAPI.seed);
		GetComponents<AudioLoop>();
		Random.state = state;
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
			return;
		}
		stashAbleEntry = new StashAbleEntry
		{
			isStashAble = true
		};
		data.AddDataEntry(stashAbleEntry);
		Debug.Log("stashAbleEntry entry not found, adding new entry with false.");
	}
}
