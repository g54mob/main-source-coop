using Photon.Pun;
using UnityEngine;

public class Bot_Chaser : MonoBehaviour
{
	[HideInInspector]
	public bool aggroState;

	public float exhastionTime = 20f;

	public float fleeForSeconds = 20f;

	private Player player;

	private Bot bot;

	private PhotonView view;

	public float hidingExhastionMultiplier = 1f;

	public float timeToLoseTarget = 2.5f;

	public float targetDistance = 0.8f;

	public bool backUpIfTooClose = true;

	public bool useWorldMoveInChase;

	public bool canRotateWhenStandingStill = true;

	public float chaseTurnRate = 6f;

	public float fleeTurnRate = 6f;

	public float investigateTurnRate = 3f;

	public float patrolTurnRate = 3f;

	public float maxRange = 70f;

	public float maxAngle = 110f;

	public float timeToSeeTarget = 1f;

	private float exhaustion;

	private float fleeFor;

	private float hasFledFor;

	private Vector3 fleeFromPoint;

	private Transform head;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		if (!view.IsMine || bot.BusyOrAttacking())
		{
			return;
		}
		bot.slowDownWhenNavigating = true;
		if (fleeFor > 0f)
		{
			player.data.currentStamina = player.refs.controller.maxStamina;
			fleeFor -= Time.deltaTime;
			if (bot.Patrol(look: true, walk: true, fleeTurnRate, listenToNoise: false, (bot.Center() - fleeFromPoint).normalized, alertable: false) && (bool)bot.lastPatrolPoint)
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
			if (exhaustion > exhastionTime)
			{
				fleeFromPoint = bot.targetPlayer.Center();
				fleeFor = fleeForSeconds;
				bot.LoseTarget();
				exhaustion = 0f;
				return;
			}
		}
		if (aggroState != bot.aggro)
		{
			aggroState = bot.aggro;
			view.RPC("RPCA_SetAggroState", RpcTarget.Others, aggroState);
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
			TargetIsHidingBehaviour();
			return;
		}
		bot.ChaseTarget(bot.Center(), targetDistance, 1f, lookForBetterTarget: true, chaseTurnRate, loseInterestIfUnreachable: false, backUpIfTooClose, canRotateWhenStandingStill);
		if (useWorldMoveInChase && bot.syncData.movementInput.y > 0.1f)
		{
			bot.SetMovementWorld(bot.navDirection_Read);
		}
		bot.ValidateChase(bot.Center(), timeToLoseTarget);
	}

	private void TargetIsHidingBehaviour()
	{
		exhaustion += Time.deltaTime * hidingExhastionMultiplier;
		bot.LookForBetterTarget(head.position);
		float num = 12f;
		if (bot.CanSee(head.position, bot.targetPlayer.HeadPosition(), 20f, 400f) && bot.distanceToTarget < num)
		{
			bot.LookAt(bot.targetPlayer.Center());
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

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center(), 1f, 30f, investigateTurnRate);
	}

	private void DefaultState()
	{
		bot.Patrol(look: true, walk: true, patrolTurnRate);
		bot.LookForTarget(bot.Center(), maxRange, maxAngle, timeToSeeTarget);
	}

	[PunRPC]
	public void RPCA_SetAggroState(bool aggro)
	{
		aggroState = aggro;
	}
}
