using Photon.Pun;
using UnityEngine;

public class Bot_Mouth : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private Vector3 fleeFromPos;

	private float fleeTime;

	private float lookingAtMeFor;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		bot.alertable = false;
	}

	private void Update()
	{
		if (!view.IsMine)
		{
			return;
		}
		if (fleeTime > 0f)
		{
			fleeTime -= Time.deltaTime;
			if (bot.Patrol(look: true, walk: true, 10f, listenToNoise: false, bot.Center() - fleeFromPos, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				fleeFromPos = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			return;
		}
		if ((bool)bot.targetPlayer)
		{
			if (Vector3.Angle(bot.Center() - bot.targetPlayer.Center(), bot.targetPlayer.data.lookDirection) < 60f)
			{
				lookingAtMeFor += Time.deltaTime;
			}
			else
			{
				lookingAtMeFor = 0f;
			}
		}
		if (lookingAtMeFor > 2f)
		{
			Flee();
		}
		else if ((bool)bot.targetPlayer)
		{
			if (bot.aggro)
			{
				Combat();
			}
			else
			{
				Investigate();
			}
		}
		else
		{
			DefaultState();
		}
	}

	private void Flee()
	{
		bot.LoseTarget();
		if ((bool)bot.targetPlayer)
		{
			fleeFromPos = bot.targetPlayer.Center();
		}
		else
		{
			fleeFromPos = bot.Center() + bot.syncData.lookDireciton * 2f;
		}
		fleeTime = 10f;
		lookingAtMeFor = 0f;
		bot.aggro = false;
	}

	private void Combat()
	{
		bot.SetSyncAttacking(bot.distanceToTarget < 3f && lookingAtMeFor < 0.5f);
		bot.ChaseTarget(bot.Center(), 2.5f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
		bot.ValidateChase(bot.Center());
		bot.syncData.sprint = false;
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
