using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Attack_Punch : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private MonsterAnimationHandler anim;

	private PhotonView view;

	public MonsterAnimationValues val;

	private List<Player> ignoredPlayers = new List<Player>();

	private bool punch;

	public SFX_Instance punchSFX;

	private float power = 1f;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = base.transform.GetComponent<PhotonView>();
		val = base.transform.root.Find("AnimationRig").GetComponentInChildren<MonsterAnimationValues>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Update()
	{
		power = Mathf.MoveTowards(power, 1f, Time.deltaTime);
		if (view.IsMine && !(bot.targetPlayer == null) && !bot.BusyOrAttacking() && bot.aggro && !(bot.distanceToTarget > 7f) && !bot.targetIsHiding && !(bot.sinceAttack < 0.1f))
		{
			Attack();
		}
	}

	private void Collide(Collision col, Bodypart part)
	{
		if (punch && !(base.transform.root == col.transform.root))
		{
			Player componentInParent = col.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent && !ignoredPlayers.Contains(componentInParent) && !componentInParent.ai && !componentInParent.data.dead && componentInParent.refs.view.IsMine && (part.bodypartType == BodypartType.Hand_L || part.bodypartType == BodypartType.Hand_R || part.bodypartType == BodypartType.Elbow_L || part.bodypartType == BodypartType.Elbow_R || part.bodypartType == BodypartType.Foot_L || part.bodypartType == BodypartType.Foot_R || part.bodypartType == BodypartType.Knee_L || part.bodypartType == BodypartType.Knee_R) && !(part.rig.linearVelocity.magnitude < 5f))
			{
				view.RPC("RPCA_Hit", RpcTarget.All, componentInParent.refs.view.ViewID, componentInParent.refs.ragdoll.GetBodypartIDFromCollider(col.collider), part.rig.linearVelocity.normalized);
				StartCoroutine(IIgnorePlayer(componentInParent));
			}
		}
	}

	private IEnumerator IIgnorePlayer(Player p)
	{
		ignoredPlayers.Add(p);
		yield return new WaitForSeconds(0.5f);
		if ((bool)p && ignoredPlayers.Contains(p))
		{
			ignoredPlayers.Remove(p);
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
				bodypartFromID.rig.AddForce(force * 20f, ForceMode.VelocityChange);
				player.refs.ragdoll.AddForce(force * 20f, ForceMode.VelocityChange);
				Debug.Log("AddForce");
			}
			player.TakeDamageLocalIKnowWhatImDoing(51f);
			player.refs.ragdoll.Fall(2f);
			punchSFX.Play(player.Center(), player.data.isLocal);
		}
	}

	private void Attack()
	{
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
			while (c < 5f && !bot.targetIsHiding && (!(c > 1.5f) || !(bot.distanceToTarget > 8f)))
			{
				power = Mathf.MoveTowards(power, 10f, Time.deltaTime * 2f);
				float num = 1f + power * 0.2f;
				if ((bool)bot.targetPlayer)
				{
					if (bot.targetPlayer.data.dead)
					{
						break;
					}
					if (val.leftPunch)
					{
						handl.AddForce((bot.targetPlayer.Center() - handl.position).normalized * num * Time.deltaTime * 50000f, ForceMode.Acceleration);
					}
					if (val.rightPunch)
					{
						handR.AddForce((bot.targetPlayer.Center() - handR.position).normalized * num * Time.deltaTime * 50000f, ForceMode.Acceleration);
					}
					torso.AddForce((bot.targetPlayer.Center() - torso.position).normalized * num * Time.deltaTime * 10000f, ForceMode.Acceleration);
				}
				bot.attacking = true;
				c += Time.deltaTime;
				if ((bool)bot.targetPlayer)
				{
					bot.ChaseTarget(player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position, 0.5f);
				}
				yield return null;
			}
			bot.attacking = false;
			punch = false;
		}
	}
}
