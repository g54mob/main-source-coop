using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Bot_BigSlap : MonoBehaviour
{
	public float sinceAttack;

	private Player player;

	private Bot bot;

	private Bot_Nav_Navmesh nav;

	private MonsterSyncer syncer;

	private Transform head;

	private float jumpCounter;

	private PhotonView view;

	public AnimationCurve jumpCurve;

	public float jumpForceForward;

	public float jumpForceUp;

	private void Start()
	{
		syncer = GetComponentInParent<MonsterSyncer>();
		view = GetComponent<PhotonView>();
		nav = GetComponent<Bot_Nav_Navmesh>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		UpdatedValues();
		if (player.refs.view.IsMine)
		{
			GatherInfo();
			ComputeState();
		}
	}

	private void GatherInfo()
	{
		if (!bot.targetPlayer && bot.LookForTarget(HeadPosition()))
		{
			bot.suspicionValue = 0f;
		}
	}

	private void ComputeState()
	{
		if (bot.BusyOrAttacking())
		{
			return;
		}
		if ((bool)bot.targetPlayer)
		{
			if (bot.aggro)
			{
				if (bot.targetIsHiding || bot.targetUnReachable)
				{
					bot.LookForBetterTarget(head.position);
					float num = 15f;
					if (bot.CanSee(head.position, bot.targetPlayer.HeadPosition(), 20f, 400f) && bot.distanceToTarget < num)
					{
						jumpCounter = Mathf.MoveTowards(jumpCounter, 5.1f, Time.deltaTime * 2f);
						if (jumpCounter > 5f)
						{
							jumpCounter = 0f;
							view.RPC("RPCA_JumpAttackBigSlap", RpcTarget.All, bot.syncData.targetPlayerId);
						}
						bot.LookAt(bot.targetPlayer.Center(), 15f);
						bot.StandStill();
					}
					else if (bot.distanceToTarget > num * 0.8f)
					{
						bot.LookAt(bot.targetPlayer.Center());
						bot.navTargetPos_Set = bot.lastGodNavPos;
						bot.SetMovementWorld(bot.navDirection_Read);
						bot.syncData.sprint = true;
						bot.slowDownWhenNavigating = false;
					}
					else
					{
						bot.navTargetPos_Set = bot.Center() + (bot.Center() - bot.targetPlayer.Center()).Flat().normalized;
						bot.LookAt(bot.targetPlayer.Center(), 6f);
						bot.SetMovementWorld(bot.navDirection_Read);
						bot.syncData.sprint = true;
						bot.slowDownWhenNavigating = false;
					}
				}
				else
				{
					bot.ChaseTarget(HeadPosition(), 1f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
					if (bot.aggro)
					{
						bot.ValidateChase(HeadPosition());
					}
				}
			}
			else
			{
				bot.InvestigateCurrentTarget(HeadPosition());
			}
		}
		else
		{
			bot.Patrol();
		}
	}

	[PunRPC]
	private void RPCA_JumpAttackBigSlap(int targetID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		if ((bool)player)
		{
			StartCoroutine(IJump(player));
		}
		IEnumerator IJump(Player target)
		{
			float c = 0f;
			float t = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
			Rigidbody torso = this.player.refs.ragdoll.GetBodypart(BodypartType.Spine_3).rig;
			Rigidbody handL = this.player.refs.ragdoll.GetBodypart(BodypartType.Hand_L).rig;
			Rigidbody handR = this.player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
			Vector3 dir = target.Center() - torso.position;
			dir = Vector3.Lerp(dir, dir.normalized, 0.75f);
			while (c < t)
			{
				Vector3 vector = jumpCurve.Evaluate(c) * (Vector3.up * jumpForceUp + dir * jumpForceForward);
				Vector3 vector2 = (target.Center() - bot.Center()).normalized * 5f;
				Vector3 force = jumpCurve.Evaluate(c) * (vector2 * jumpForceForward);
				torso.AddForce(vector, ForceMode.Acceleration);
				handL.AddForce(force, ForceMode.Acceleration);
				handR.AddForce(force, ForceMode.Acceleration);
				this.player.refs.ragdoll.AddForce(vector * 0.3f, ForceMode.Acceleration);
				c += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
	}

	private void UpdatedValues()
	{
		if (bot.attacking)
		{
			bot.slowDownWhenNavigating = false;
		}
		else
		{
			bot.slowDownWhenNavigating = true;
		}
		sinceAttack += Time.deltaTime;
	}

	private Vector3 HeadPosition()
	{
		return player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position;
	}
}
