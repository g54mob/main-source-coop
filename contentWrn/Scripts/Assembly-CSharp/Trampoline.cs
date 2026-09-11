using System.Collections.Generic;
using Portningsbolaget.Platforms;
using UnityEngine;

public class Trampoline : SurfaceCollisionEffect
{
	public float fallVel = 3f;

	public Transform wobbler;

	public float launchForce = 1f;

	public float launchForceRagdoll = 1f;

	public float bounceForce;

	public float upMass = 100f;

	public float downMass = 25f;

	public float spring;

	public float springUp;

	public float damper;

	private float targetPos;

	private Rigidbody rig;

	private List<Player> players = new List<Player>();

	public SFX_Instance[] jumpSFX;

	public SFX_Instance[] jumpSFX_Medium;

	public SFX_Instance[] jumpSFX_Big;

	public override void CollideWithSurface(Collision col, Bodypart part)
	{
		Player componentInParent = part.GetComponentInParent<Player>();
		if (componentInParent.data.sinceJump < 0.3f)
		{
			return;
		}
		float num = Mathf.Clamp(componentInParent.GetRig(BodypartType.Hip).linearVelocity.y, -100f, 0f);
		if (componentInParent.data.fallTime > 0f)
		{
			num *= 0.1f;
		}
		rig.AddForce(num * Vector3.up * bounceForce, ForceMode.VelocityChange);
		if (!players.Contains(componentInParent))
		{
			if (num < -11f)
			{
				for (int i = 0; i < jumpSFX_Big.Length; i++)
				{
					jumpSFX_Big[i].Play(base.transform.position);
				}
			}
			else if (num < -8f)
			{
				for (int j = 0; j < jumpSFX_Medium.Length; j++)
				{
					jumpSFX_Medium[j].Play(base.transform.position);
				}
			}
			else
			{
				for (int k = 0; k < jumpSFX.Length; k++)
				{
					jumpSFX[k].Play(base.transform.position);
				}
			}
			players.Add(componentInParent);
		}
		componentInParent.data.sinceTrampoline = 0f;
	}

	private void Start()
	{
		targetPos = wobbler.localPosition.z;
		rig = GetComponentInParent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		float y = rig.linearVelocity.y;
		for (int num = players.Count - 1; num >= 0; num--)
		{
			Player player = players[num];
			if (player.data.sinceJump < 0.3f)
			{
				if (player.IsLocal)
				{
					player.CallTakeDamageAndAddForceAndFall(0f, Vector3.up * launchForce * 3f * Mathf.Clamp(y, 0f, 100f), 0f);
				}
				players.Remove(player);
			}
			else if (wobbler.localPosition.z > targetPos && y > fallVel)
			{
				if (player.IsLocal)
				{
					player.CallTakeDamageAndAddForceAndFall(0f, Vector3.up * launchForceRagdoll * 0.5f * Mathf.Clamp(y, 0f, 100f), 2f);
					player.CallAddForceToBodyParts(new int[1] { player.refs.ragdoll.GetBodyPartID(BodypartType.Head) }, new Vector3[1] { Random.onUnitSphere * Mathf.Clamp(y, 0f, 100f) * 5f });
					PlatformManager.UnlockAchievement(Achievements.ACH_TRAMPOLINE_SLIP);
				}
				players.Remove(player);
			}
			if (player.data.sinceTrampoline > 0.3f)
			{
				players.Remove(player);
			}
		}
		float num2 = spring;
		if (y > 0f && players.Count > 0)
		{
			num2 = springUp;
			rig.mass = upMass;
		}
		else
		{
			rig.mass = downMass;
		}
		rig.AddForce((targetPos - wobbler.localPosition.z) * Vector3.up * num2, ForceMode.Acceleration);
		rig.linearVelocity *= damper;
	}
}
