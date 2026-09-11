using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bot_CameraCreep : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private Bot_CameraCreep creep;

	private PhotonView view;

	private Transform head;

	private float sinceFilmed = 10f;

	public SkinnedMeshRenderer rend;

	public float dragForce = 10f;

	private float sinceTeleport = 10f;

	private MonsterAnimationHandler anim;

	private MonsterAnimationValues vals;

	private Coroutine teleportAwayCor;

	private float standingStillFor;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		vals = base.transform.root.Find("AnimationRig").GetComponentInChildren<MonsterAnimationValues>();
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		if (!view.IsMine)
		{
			return;
		}
		sinceTeleport += Time.deltaTime;
		if (player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.linearVelocity.magnitude > 0.5f)
		{
			standingStillFor = 0f;
		}
		else
		{
			standingStillFor += Time.deltaTime;
		}
		if (!bot.attacking)
		{
			bot.slowDownWhenNavigating = false;
			sinceFilmed += Time.deltaTime;
			HandleVisibility();
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

	private void HandleVisibility()
	{
		if (sinceFilmed < 0.1f)
		{
			rend.enabled = true;
			bot.jumpScareLevel = 3;
			view.RPC("RPCA_DoCreepAttack", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
			return;
		}
		if (rend.enabled)
		{
			TeleportAway();
		}
		rend.enabled = false;
		bot.jumpScareLevel = 0;
	}

	[PunRPC]
	private void RPCA_DoCreepAttack(int targetID)
	{
		StartCoroutine(IDoAttack(PlayerHandler.instance.TryGetPlayerFromViewID(targetID)));
	}

	private IEnumerator IDoAttack(Player target)
	{
		if (!target)
		{
			yield break;
		}
		player.refs.ragdoll.SetColliderLayer(0);
		bot.attacking = true;
		bot.StandStill();
		if (target != null)
		{
			bot.LookAt(target.Center());
		}
		Rigidbody targetTorso = null;
		if ((bool)bot.targetPlayer)
		{
			targetTorso = bot.targetPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
		}
		anim.PlayAnimation("CamCreepoAttack");
		float c = 0f;
		while (c < 4.3f)
		{
			if (c < 2f && (bool)bot.targetPlayer)
			{
				ChaseTarget(bot.targetPlayer);
			}
			else
			{
				bot.StandStill();
			}
			c += Time.fixedDeltaTime;
			Vector3 relativePosition_Rig = player.GetRelativePosition_Rig(BodypartType.Torso, new Vector3(0f, 0f, -0.7f));
			float num = 10f;
			if ((bool)bot.targetPlayer)
			{
				num = Vector3.Distance(relativePosition_Rig, targetTorso.position);
			}
			if (c > 1f && (bool)bot.targetPlayer && num < 1f)
			{
				targetTorso.AddForce((relativePosition_Rig - targetTorso.position).normalized * dragForce, ForceMode.Acceleration);
			}
			if (vals.rightPunch && (bool)bot.targetPlayer && bot.targetPlayer.IsLocal && !bot.targetPlayer.data.dead && num < 1f)
			{
				bot.targetPlayer.CallDie();
			}
			yield return new WaitForFixedUpdate();
		}
		if (view.IsMine)
		{
			TeleportAway();
		}
		bot.attacking = false;
	}

	private void TeleportAway()
	{
		PatrolPoint freePointWithDistance = Level.currentLevel.GetFreePointWithDistance(new List<PatrolPoint.PatrolGroup> { PatrolPoint.PatrolGroup.Bear }, bot.Center(), 30, 20f);
		if ((bool)freePointWithDistance)
		{
			view.RPC("RPCA_TeleportAway", RpcTarget.All, freePointWithDistance.transform.position);
		}
	}

	[PunRPC]
	private void RPCA_TeleportAway(Vector3 targetPos)
	{
		sinceFilmed = 10f;
		sinceTeleport = 0f;
		player.refs.ragdoll.SetColliderLayer(20);
		bot.LoseTarget();
		StartCoroutine(ITeleportAway());
		IEnumerator ITeleportAway()
		{
			yield return null;
			yield return new WaitForFixedUpdate();
			yield return null;
			player.refs.ragdoll.ExtraDrag(0f);
			player.MoveAllRigsInDirection(targetPos - bot.Center() + Vector3.up);
		}
	}

	private void Combat()
	{
		if (!bot.targetIsHiding)
		{
			_ = bot.targetUnReachable;
		}
		if ((bool)bot.targetPlayer)
		{
			ChaseTarget(bot.targetPlayer);
		}
		bot.ValidateChase(bot.Center(), 4f);
	}

	private void ChaseTarget(Player t)
	{
		Vector3 vector = t.Center() + t.data.lookDirection.Flat() * -0.8f;
		float num = HelperFunctions.FlatDistance(bot.Center(), vector);
		if (num < 3f)
		{
			bot.LookAt(t.Center(), 10f);
			if (num < 0.25f)
			{
				bot.StandStill();
			}
			else
			{
				bot.WorldMoveTo(vector);
			}
		}
		else
		{
			bot.navTargetPos_Set = vector;
			bot.Look(bot.navDirection_Read, 10f);
			bot.Walk();
		}
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center(), 1f, 30f, 6f);
	}

	private void DefaultState()
	{
		bot.Patrol(look: true, walk: true, 6f);
		bot.LookForTarget(bot.Center(), 30f, 120f, 2f);
	}

	internal void IsFilmed(Camera camera, float seenAmount, float time)
	{
		if (!(sinceTeleport < 1f) && !(Vector3.Distance(camera.transform.position, bot.Center()) > 3f) && (bot.attacking || !(standingStillFor < 0.3f)))
		{
			sinceFilmed = 0f;
		}
	}
}
