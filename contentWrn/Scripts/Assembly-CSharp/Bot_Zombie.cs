using Photon.Pun;
using UnityEngine;

public class Bot_Zombie : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private Vector3 randomDir;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		randomDir = Random.onUnitSphere.Flat();
	}

	private void Update()
	{
		if (view.IsMine)
		{
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
	}

	private void Combat()
	{
		bot.navOffset = randomDir * (0.7f * bot.distanceToTarget_Flat);
		bot.SetSyncAttacking(bot.distanceToTarget < 2f);
		bot.ChaseTarget(bot.Center(), 0.8f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
		bot.ValidateChase(bot.Center(), 3.5f);
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
