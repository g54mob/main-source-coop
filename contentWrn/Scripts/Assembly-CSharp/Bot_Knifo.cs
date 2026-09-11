using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Bot_Knifo : MonoBehaviour
{
	public BodypartType mainRig = BodypartType.Torso;

	private Player player;

	private Bot bot;

	private PhotonView view;

	public float targetDistance = 0.8f;

	private float exhaustion;

	private float fleeFor;

	private float hasFledFor;

	private Vector3 fleeFromPoint;

	private Transform head;

	private float jumpCounter;

	public AnimationCurve jumpCurve;

	public float jumpForceForward;

	public float jumpForceUp;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		if (!view.IsMine || player.NoControl())
		{
			return;
		}
		jumpCounter = Mathf.MoveTowards(jumpCounter, 0f, Time.deltaTime);
		bot.slowDownWhenNavigating = true;
		if (fleeFor > 0f)
		{
			player.data.currentStamina = player.refs.controller.maxStamina;
			fleeFor -= Time.deltaTime;
			if (bot.Patrol(look: true, walk: true, 6f, listenToNoise: false, (bot.Center() - fleeFromPoint).normalized, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				fleeFromPoint = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			bot.moveSpeedMultiplier = 2f;
			hasFledFor += Time.deltaTime;
			if (hasFledFor > 3f)
			{
				bot.LookForTarget(bot.Center(), 5f, 120f);
			}
			return;
		}
		bot.moveSpeedMultiplier = 1f;
		hasFledFor = 0f;
		if ((bool)bot.targetPlayer)
		{
			exhaustion += Time.deltaTime;
			if (exhaustion > 20f)
			{
				fleeFromPoint = bot.targetPlayer.Center();
				fleeFor = 20f;
				bot.LoseTarget();
				exhaustion = 0f;
				return;
			}
		}
		if (bot.aggro)
		{
			Combat();
		}
		else if ((bool)bot.targetPlayer)
		{
			Investigate();
		}
		else
		{
			DefaultState();
		}
	}

	private void Combat()
	{
		if (bot.targetIsHiding || bot.targetUnReachable)
		{
			exhaustion += Time.deltaTime;
			bot.LookForBetterTarget(head.position);
			float num = 12f;
			if (bot.CanSee(head.position, bot.targetPlayer.HeadPosition(), 20f, 400f) && bot.distanceToTarget < num)
			{
				jumpCounter = Mathf.MoveTowards(jumpCounter, 2.1f, Time.deltaTime * 2f);
				if (jumpCounter > 2f)
				{
					jumpCounter = 0f;
					view.RPC("RPCA_JumpAttack", RpcTarget.All, bot.syncData.targetPlayerId);
				}
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
			bot.ChaseTarget(bot.Center(), targetDistance, 1f, lookForBetterTarget: true, 6f);
			bot.ValidateChase(bot.Center(), 2.5f);
		}
	}

	[PunRPC]
	private void RPCA_JumpAttack(int targetID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		if ((bool)player)
		{
			StartCoroutine(IJump(player));
		}
		IEnumerator IJump(Player target)
		{
			GetComponentInParent<MonsterAnimationHandler>().PlayAnimation("Jump");
			float c = 0f;
			float t = jumpCurve.keys[jumpCurve.keys.Length - 1].time;
			Rigidbody torso = this.player.refs.ragdoll.GetBodypart(mainRig).rig;
			Vector3 dir = target.Center() - torso.position;
			dir = Vector3.Lerp(dir, dir.normalized, 0.75f);
			while (c < t)
			{
				Vector3 force = jumpCurve.Evaluate(c) * (Vector3.up * jumpForceUp + dir * jumpForceForward);
				torso.AddForce(force, ForceMode.Acceleration);
				this.player.refs.ragdoll.AddForce(force, ForceMode.Acceleration);
				c += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center());
	}

	private void DefaultState()
	{
		bot.Patrol();
		bot.LookForTarget(bot.Center());
	}
}
