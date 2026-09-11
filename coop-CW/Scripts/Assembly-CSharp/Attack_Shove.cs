using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attack_Shove : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	public MonsterAnimationValues val;

	public SFX_Instance punchSFX;

	private bool punch;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		val = base.transform.root.Find("AnimationRig").GetComponentInChildren<MonsterAnimationValues>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Update()
	{
		if (view.IsMine && !bot.BusyOrAttacking() && bot.aggro && !(bot.distanceToTarget > 8f) && !(bot.sinceAttack < 2.5f) && !player.NoControl())
		{
			Attack();
		}
	}

	private void Collide(Collision col, Bodypart part)
	{
		if (punch)
		{
			Player componentInParent = col.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent && componentInParent.refs.view.IsMine && !componentInParent.Ragdoll() && (part.bodypartType == BodypartType.Hand_L || part.bodypartType == BodypartType.Hand_R || part.bodypartType == BodypartType.Elbow_L || part.bodypartType == BodypartType.Elbow_R || part.bodypartType == BodypartType.Foot_L || part.bodypartType == BodypartType.Foot_R || part.bodypartType == BodypartType.Knee_L || part.bodypartType == BodypartType.Knee_R) && !(part.rig.linearVelocity.magnitude < 5f))
			{
				view.RPC("RPCA_Hit", RpcTarget.All, componentInParent.refs.view.ViewID, player.refs.ragdoll.GetBodypartIDFromCollider(col.collider), part.rig.linearVelocity.normalized);
			}
		}
	}

	[PunRPC]
	public void RPCA_Hit(int viewID, int bodyPartID, Vector3 force)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
		if ((bool)player)
		{
			Bodypart bodypartFromID = player.refs.ragdoll.GetBodypartFromID(bodyPartID);
			if ((bool)bodypartFromID)
			{
				bodypartFromID.rig.AddForce(force * 15f, ForceMode.VelocityChange);
				player.refs.ragdoll.AddForce(force * 10f, ForceMode.VelocityChange);
			}
			player.TakeDamageLocalIKnowWhatImDoing(20f);
			player.refs.ragdoll.Fall(1f);
			if ((bool)punchSFX)
			{
				punchSFX.Play(player.Center());
			}
		}
	}

	private void Attack()
	{
		bot.attacking = true;
		view.RPC("RPCA_Punch", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_Punch()
	{
		Attack_Grab componentInChildren = base.transform.root.GetComponentInChildren<Attack_Grab>();
		if ((bool)componentInChildren)
		{
			componentInChildren.LetGo();
		}
		bot.attacking = true;
		punch = true;
		Rigidbody handl = player.refs.ragdoll.GetBodypart(BodypartType.Hand_L).rig;
		Rigidbody handR = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
		Rigidbody torso = player.refs.ragdoll.GetBodypart(BodypartType.Spine_2).rig;
		StartCoroutine(DoPunch());
		IEnumerator DoPunch()
		{
			float c = 0f;
			while (c < 1f && !bot.hurt && !player.NoControl())
			{
				if ((bool)bot.targetPlayer)
				{
					if (val.leftPunch)
					{
						handl.AddForce((bot.targetPlayer.Center() - handl.position).normalized * Time.deltaTime * 50000f, ForceMode.Acceleration);
					}
					if (val.rightPunch)
					{
						handR.AddForce((bot.targetPlayer.Center() - handR.position).normalized * Time.deltaTime * 50000f, ForceMode.Acceleration);
					}
					torso.AddForce((bot.targetPlayer.Center() - torso.position).normalized * Time.deltaTime * 10000f, ForceMode.Acceleration);
				}
				bot.attacking = true;
				c += Time.deltaTime;
				if ((bool)bot.targetPlayer)
				{
					bot.ChaseTarget(player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position, 2f);
				}
				yield return null;
			}
			bot.attacking = false;
			punch = false;
		}
	}
}
