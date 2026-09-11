using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bot_Nav_Navmesh : MonoBehaviour
{
	private NavMeshAgent agent;

	private Bot bot;

	private PlayerController m_PlayerController;

	private bool recalculate;

	private float sinceCheckForProblems;

	private bool isSeen;

	private bool simpleRagdoll;

	private Vector3 lastCheckPosition = Vector3.zero;

	private Transform hipRig;

	private Vector3 targetPos;

	private Vector3 up100 = Vector3.up * 100f;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		bot = GetComponent<Bot>();
		m_PlayerController = GetComponentInParent<PlayerController>();
		Bot obj = bot;
		obj.teleportAction = (Action<Vector3>)Delegate.Combine(obj.teleportAction, new Action<Vector3>(Teleport));
		lastCheckPosition = bot.Center();
		if (base.transform.parent.TryGetComponent<PlayerRagdoll>(out var component))
		{
			hipRig = component.GetBodypart(BodypartType.Hip).transform;
		}
	}

	private void Teleport(Vector3 vector)
	{
		StartCoroutine(IResetNav());
		IEnumerator IResetNav()
		{
			yield return null;
			yield return new WaitForEndOfFrame();
			FlickerBackToNav();
		}
	}

	private void Update()
	{
		if (!(Player.localPlayer == null) && bot.view.IsMine)
		{
			CheckForProblems();
			if (agent.enabled)
			{
				NormalNav();
				ApplyNav();
			}
		}
	}

	private void CheckForProblems()
	{
		sinceCheckForProblems += Time.deltaTime;
		if (sinceCheckForProblems > 5f)
		{
			sinceCheckForProblems = 0f;
			FlickerBackToNav();
		}
	}

	private void FlickerBackToNav()
	{
		MonoFunctions.instance.DelayCall(MoveAndEnable, 0.1f);
		agent.enabled = false;
	}

	private void MoveAndEnable()
	{
		base.transform.position = bot.Center();
		agent.enabled = true;
		recalculate = true;
		if (agent.isOnNavMesh)
		{
			agent.ResetPath();
			agent.SetDestination(bot.navTargetPos_Set);
		}
	}

	private void NormalNav()
	{
		Debug.DrawRay(bot.lastGodNavPos, bot.lastGodNavPos + up100, Color.magenta);
		bot.targetIsHiding = false;
		bot.targetUnReachable = false;
		if ((bool)bot.targetPlayer)
		{
			NavMeshQueryFilter filter = new NavMeshQueryFilter
			{
				agentTypeID = agent.agentTypeID,
				areaMask = -1
			};
			NavMesh.SamplePosition(bot.targetPlayer.CenterGroundPos(), out var hit, 4f, filter);
			if (hit.hit)
			{
				if (hit.distance > 1f)
				{
					bot.targetIsHiding = true;
				}
				else if (Vector3.SqrMagnitude(agent.destination - agent.pathEndPosition) > 1f)
				{
					bot.targetUnReachable = true;
				}
				else if (Vector3.SqrMagnitude(hit.position - agent.pathEndPosition) < 1f)
				{
					bot.lastGodNavPos = hit.position;
				}
			}
			else
			{
				bot.targetUnReachable = true;
			}
			if (bot.navOffset.sqrMagnitude > 0.01f)
			{
				NavMesh.SamplePosition(bot.targetPlayer.CenterGroundPos() + bot.navOffset, out var hit2, 1f, filter);
				if (hit2.hit)
				{
					bot.navTargetPos_Set = hit2.position;
				}
			}
			if (agent.isOnNavMesh)
			{
				agent.SetDestination(bot.navTargetPos_Set);
			}
		}
		else if (bot.navTargetPos_Set != targetPos || recalculate)
		{
			targetPos = bot.navTargetPos_Set;
			if (agent.isOnNavMesh)
			{
				agent.SetDestination(bot.navTargetPos_Set);
			}
			recalculate = false;
		}
		bot.navDestination_Read = agent.pathEndPosition;
	}

	private void ApplyNav()
	{
		bot.navDirection_Read = agent.desiredVelocity.normalized;
		if (agent.isOnNavMesh)
		{
			bot.remainingNavDistance = agent.remainingDistance;
		}
	}
}
