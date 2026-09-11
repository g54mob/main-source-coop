using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using pworld.Scripts.Extensions;

public class Bot_MimicInfiltrator : MonoBehaviour
{
	public float customRotationSpeed = 6f;

	public float minTimeBeforeWalkSprintSwitch = 1f;

	public float minTimeBetweenJumps = 10f;

	public float maxTimeBetweenJumps = 10f;

	[HideInInspector]
	public Player player;

	public float distanceToBeConsideredAlone = 30f;

	public Player hitTarget;

	public Player mimickingPlayer;

	public float runUntilThisCloseFromTarget = 5f;

	public float stopWhenThisCloseToTarget = 2f;

	public bool isAngry;

	public float timeBetweenJabs = 1f;

	public float jabForce = 30f;

	public string angryFaceText = ">:(";

	public float angryFaceRotation = -90f;

	public Color angryFaceColor;

	public float damageOnHit;

	[SerializeField]
	private float fallOnHit;

	[SerializeField]
	private float forceOnHit;

	public List<Player> ignorePlayers = new List<Player>();

	public float distanceForHitToBeConsideredAlone = 30f;

	public float timeThatFistIsDangerous = 0.25f;

	public float lookAwayChance = 0.3f;

	[FormerlySerializedAs("runAwayChange")]
	public float runAwayChance = 0.1f;

	public float flickerChance = 0.05f;

	public float angryThreshold = 10f;

	private readonly Vector3 faceSizeMinMax = new Vector3(0.025f, 0.035f);

	public float maxTimeAwkward = 5f;

	private float angryMeter;

	private bool awkwardLookAway;

	private Vector3 awkwardLookDir;

	private float awkwardTime;

	private Bot bot;

	private FakeFlashLight fakeFlashLight_grc;

	private bool followPlayer;

	private bool goToRandomPoint;

	private Vector3 moveAwayPos;

	private PatrolPoint randomPoint;

	private bool runToPlayer;

	private float timeSinceJab;

	private float timeSinceWalkSprintSwitch;

	private float timeSpentLookingAtMe;

	private float timeSpentNotLookingAtMe;

	private float timeToJab;

	private float timeToNextJump;

	private PhotonView view;

	public float hitDistance = 2f;

	private float timeAlive;

	public bool debug;

	public float nothingChance = 0.3f;

	public float DistToTarget => HelperFunctions.FlatDistance(base.transform.position, bot.targetPlayer.Center());

	public bool Sprint
	{
		get
		{
			return bot.syncData.sprint;
		}
		set
		{
			if (value != Sprint && !(timeSinceWalkSprintSwitch < minTimeBeforeWalkSprintSwitch))
			{
				timeSinceWalkSprintSwitch = Random.Range(minTimeBeforeWalkSprintSwitch * -0.5f, minTimeBeforeWalkSprintSwitch * 0.5f);
				bot.syncData.sprint = value;
			}
		}
	}

	public float DistanceToHitTarget => Vector3.Distance(hitTarget.Center(), player.Center());

	public float DistanceToMimickingPlayer
	{
		get
		{
			if (mimickingPlayer != null)
			{
				return Vector3.Distance(mimickingPlayer.Center(), player.Center());
			}
			return float.MaxValue;
		}
	}

	public bool IsInfiltrating { get; private set; }

	private void Awake()
	{
		player = GetComponentInParent<Player>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		followPlayer = true;
		fakeFlashLight_grc = base.transform.root.GetComponentInChildren<FakeFlashLight>();
	}

	private void FindMimicTarget()
	{
		if (PlayerHandler.instance.GetLargestClosestDistanceBetweenPlayers(out var maxMinDistanceBetweenPlayers, out var mostAlonePlayer))
		{
			if (!(maxMinDistanceBetweenPlayers < distanceToBeConsideredAlone))
			{
				return;
			}
			mimickingPlayer = mostAlonePlayer;
			hitTarget = PlayerHandler.instance.GetFurthestPlayerFromPlayer(mimickingPlayer);
			{
				foreach (PatrolPoint item in Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), hitTarget.Center(), 10f, 4f))
				{
					if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
					{
						Debug.LogWarning("MimickPlayer!");
						MimickPlayer(mimickingPlayer, hitTarget);
						base.transform.position = item.transform.position + Vector3.up;
						break;
					}
				}
				return;
			}
		}
		player.data.sinceGrounded = 0f;
		base.transform.position = Vector3.up * 100f;
	}

	private void SearchForTarget()
	{
		if (!debug)
		{
			FindMimicTarget();
		}
		else
		{
			Player localPlayer = Player.localPlayer;
			Player localPlayer2 = Player.localPlayer;
			foreach (PatrolPoint item in Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), localPlayer.Center(), 10f, 4f))
			{
				if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
				{
					Infiltrate(localPlayer2, localPlayer, item.transform.position + Vector3.up);
					break;
				}
			}
		}
		if (!IsInfiltrating)
		{
			player.data.sinceGrounded = 0f;
			base.transform.position = Vector3.up * 100f;
		}
	}

	private void Infiltrate(Player micingPlayer, Player hitTarget, Vector3 spawnPosition)
	{
		IsInfiltrating = true;
		MimickPlayer(micingPlayer, hitTarget);
		base.transform.position = spawnPosition;
	}

	private void Update()
	{
		if (!player.refs.view.IsMine || IsInfiltrating)
		{
			return;
		}
		SearchForTarget();
		timeAlive += Time.deltaTime;
		timeToNextJump -= Time.deltaTime;
		timeSinceJab += Time.deltaTime;
		timeSinceWalkSprintSwitch += Time.deltaTime;
		if (bot.targetPlayer != null)
		{
			bot.navTargetPos_Set = bot.targetPlayer.Center();
			bot.Look(bot.navDirection_Read);
		}
		bool flag = IsTargetLookingAtMe();
		bool flag2 = IamLookingAtTarget();
		bool flag3 = false;
		bool flag4 = false;
		if (mimickingPlayer != null)
		{
			flag3 = mimickingPlayer.CanSee(player.HeadPosition()) && PlayerHandler.instance.playersAlive.Count > 1;
			flag4 = hitTarget.CanSee(mimickingPlayer.HeadPosition()) && PlayerHandler.instance.playersAlive.Count > 1;
		}
		float playersClosestDistanceToAnotherPlayer = PlayerHandler.instance.GetPlayersClosestDistanceToAnotherPlayer(hitTarget);
		Debug.LogWarning($"agnryMeter {angryMeter} angryThreshold {angryThreshold}");
		if (!isAngry && angryMeter > angryThreshold)
		{
			MakeAngry();
		}
		Debug.LogWarning($"alone {distanceForHitToBeConsideredAlone < playersClosestDistanceToAnotherPlayer} dist {playersClosestDistanceToAnotherPlayer} distToHit {DistanceToHitTarget} targetIsLookingAtMe {flag} imLookingAtTarget {flag2} followPlayer {followPlayer}");
		if (distanceForHitToBeConsideredAlone < playersClosestDistanceToAnotherPlayer && DistanceToHitTarget < 10f && flag && flag2 && followPlayer)
		{
			angryMeter += Time.deltaTime * 0.5f;
			Debug.LogWarning("Getting angry because target is alone");
		}
		if (DistanceToMimickingPlayer < 10f && flag3)
		{
			angryMeter += Time.deltaTime * 1f;
			Debug.LogWarning("Getting mimictarget is close and looks at me");
		}
		if (mimickingPlayer == null || bot.targetPlayer == null || hitTarget == null || hitTarget.data.dead)
		{
			goToRandomPoint = true;
			followPlayer = false;
			moveAwayPos = player.Center();
			isAngry = false;
		}
		if (hitTarget.data.dead && !AnyoneLookingAtMe())
		{
			Despawn();
		}
		if (flag4 && !AnyoneLookingAtMe() && timeAlive > 60f)
		{
			Despawn();
			return;
		}
		if (angryMeter > angryThreshold * 0.5f)
		{
			awkwardTime = 0f;
			followPlayer = true;
		}
		if (isAngry && (bool)bot.targetPlayer)
		{
			bot.LookForBetterTarget(player.HeadPosition());
			bot.navTargetPos_Set = bot.targetPlayer.Center();
			bot.Look(bot.navDirection_Read);
			if (timeToJab < 0f)
			{
				Bodypart bodypart = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R);
				Vector3 vector = bot.targetPlayer.HeadPosition() - bodypart.transform.position;
				Vector3 normalized = vector.normalized;
				if (vector.magnitude < hitDistance)
				{
					bodypart.rig.AddForce(normalized * jabForce, ForceMode.VelocityChange);
					timeToJab = timeBetweenJabs;
					timeSinceJab = 0f;
				}
			}
			timeToJab -= Time.deltaTime;
			timeSinceWalkSprintSwitch = float.MaxValue;
			bot.syncData.sprint = true;
			bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
			return;
		}
		Debug.LogWarning("Is Target looking at me " + flag);
		if (flag)
		{
			timeSpentLookingAtMe += Time.deltaTime;
			timeSpentNotLookingAtMe = 0f;
		}
		else
		{
			timeSpentLookingAtMe = 0f;
			timeSpentNotLookingAtMe += Time.deltaTime;
		}
		if (goToRandomPoint)
		{
			if (bot.Patrol(look: true, DistanceToHitTarget < (float)(5 + (Sprint ? 3 : (-1))), customRotationSpeed, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				moveAwayPos = bot.lastPatrolPoint.transform.position;
			}
			if (timeSpentNotLookingAtMe > 5f && bot.targetPlayer != null && hitTarget != null && mimickingPlayer != null && !hitTarget.data.dead)
			{
				goToRandomPoint = false;
				Debug.LogWarning("looked away for long time making followPlayer true");
				followPlayer = true;
			}
		}
		if (followPlayer)
		{
			bot.navTargetPos_Set = bot.targetPlayer.Center();
			bot.Look(bot.navDirection_Read);
			if (Vector3.Distance(player.Center(), bot.navTargetPos_Set) < stopWhenThisCloseToTarget)
			{
				Debug.LogWarning("close enough to target");
				bot.StandStill();
				if (flag)
				{
					awkwardTime += Time.deltaTime;
				}
				else
				{
					awkwardTime = Mathf.MoveTowards(awkwardTime, 0f, Time.deltaTime);
				}
				if (awkwardLookAway)
				{
					bot.Look(awkwardLookDir);
				}
			}
			else
			{
				Debug.LogWarning("move close to target");
				bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
				awkwardTime = Mathf.MoveTowards(awkwardTime, 0f, Time.deltaTime);
			}
		}
		Debug.LogWarning($"AkwardTime {awkwardTime} maxTimeAwkward {maxTimeAwkward}");
		if (awkwardTime > maxTimeAwkward)
		{
			MakeItLessAwkward();
		}
		if (!runToPlayer && bot.remainingNavDistance > runUntilThisCloseFromTarget * 1.25f)
		{
			runToPlayer = true;
		}
		if (runToPlayer && bot.remainingNavDistance < runUntilThisCloseFromTarget)
		{
			runToPlayer = false;
		}
		Sprint = runToPlayer;
	}

	private void FixedUpdate()
	{
		if (!view.IsMine || !isAngry || !(timeSinceJab < timeThatFistIsDangerous))
		{
			return;
		}
		Collider[] array = Physics.OverlapSphere(player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).transform.position, 1f);
		foreach (Collider collider in array)
		{
			Player hitPlayer = collider.transform.root.GetComponentInChildren<Player>();
			if ((bool)hitPlayer && hitPlayer != player && !ignorePlayers.Contains(hitPlayer))
			{
				Vector3 force = forceOnHit * (hitPlayer.Center() - player.Center()).normalized;
				hitPlayer.CallTakeDamageAndAddForceAndFall(damageOnHit, force, fallOnHit);
				StartCoroutine(IgnorePlayerForTime(timeBetweenJabs * 0.8f));
			}
			IEnumerator IgnorePlayerForTime(float time)
			{
				ignorePlayers.Remove(hitPlayer);
				yield return new WaitForSeconds(time);
				ignorePlayers.Remove(hitPlayer);
			}
		}
	}

	private bool AnyoneLookingAtMe()
	{
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if (item.CanSee(player.HeadPosition()))
			{
				return true;
			}
		}
		return false;
	}

	public void Despawn()
	{
		PhotonNetwork.Destroy(base.transform.root.gameObject);
	}

	private void MakeItLessAwkward()
	{
		float me = Random.Range(0f, flickerChance + runAwayChance + lookAwayChance + nothingChance);
		goToRandomPoint = false;
		awkwardLookAway = false;
		followPlayer = false;
		float num = 0f;
		if (me.IsBetween(num, flickerChance))
		{
			StartCoroutine(FlickerRed());
			Debug.LogWarning("Solve awkwardness by flickering away from player");
			awkwardTime = Random.Range(0f, maxTimeAwkward * 0.9f);
			followPlayer = true;
			return;
		}
		num += flickerChance;
		if (me.IsBetween(num, runAwayChance))
		{
			Debug.LogWarning("Solve awkwardness by moving away from player");
			goToRandomPoint = true;
			moveAwayPos = player.Center() - player.data.lookDirection.xoz() * 3f;
			awkwardTime = Random.Range(0f, maxTimeAwkward * 0.1f);
			return;
		}
		num += runAwayChance;
		if (me.IsBetween(num, lookAwayChance))
		{
			Debug.LogWarning("Solve awkwardness by looking away from player");
			awkwardLookAway = true;
			followPlayer = true;
			awkwardLookDir = Random.insideUnitSphere;
			awkwardTime = Random.Range(0f, maxTimeAwkward * 0.5f);
		}
		else
		{
			num += nothingChance;
			if (me.IsBetween(num, nothingChance))
			{
				Debug.LogWarning("Solve awkwardness by doing nothing");
				return;
			}
			followPlayer = true;
			awkwardTime = Random.Range(0f, maxTimeAwkward * 0.5f);
		}
	}

	private bool IsTargetLookingAtMe()
	{
		if (bot.targetPlayer == null)
		{
			return false;
		}
		return bot.targetPlayer.CanSee(player.HeadPosition());
	}

	private bool IamLookingAtTarget()
	{
		if (bot.targetPlayer == null)
		{
			return false;
		}
		return player.CanSee(bot.targetPlayer.HeadPosition());
	}

	[PunRPC]
	public void MakeAngry()
	{
		isAngry = true;
		timeToJab = 0f;
		bot.syncData.sprint = true;
		bot.aggro = true;
		view.RPC("RPC_AngryVisuals", RpcTarget.All);
	}

	[PunRPC]
	private void RPC_AngryVisuals()
	{
		base.transform.root.GetComponentInChildren<FakeFlashLight>().ColorRed();
		player.refs.visor.SetAllFaceSettings(PlayerVisor.GetHueFromColor(angryFaceColor), 0, angryFaceText, angryFaceRotation, faceSizeMinMax.y);
	}

	private IEnumerator FlickerRed()
	{
		view.RPC("RPC_AngryVisuals", RpcTarget.All);
		yield return new WaitForSeconds(0.35f);
		base.transform.root.GetComponentInChildren<FakeFlashLight>().ColorDefault();
		view.RPC("RPC_ImitateVisuals", RpcTarget.All);
	}

	public void MimickPlayer(Player playerToMimick, Player hitTarget)
	{
		view.RPC("RPC_MimicPlayer", RpcTarget.All, playerToMimick.refs.view.ViewID, hitTarget.refs.view.ViewID);
	}

	[PunRPC]
	private void RPC_MimicPlayer(int mimicID, int hitTargetID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(mimicID);
		Player player2 = PlayerHandler.instance.TryGetPlayerFromViewID(hitTargetID);
		if ((bool)player && (bool)player2)
		{
			mimickingPlayer = player;
			base.transform.root.GetComponentInChildren<FakeFlashLight>().Toggle(on: true);
			hitTarget = player2;
			view.RPC("RPC_ImitateVisuals", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPC_ImitateVisuals()
	{
		PlayerVisor visor = mimickingPlayer.refs.visor;
		PlayerVisor visor2 = player.refs.visor;
		bot.SetTargetPlayer(hitTarget);
		Debug.LogWarning($"mimicVisor {visor.hue.Value} {visor.visorColorIndex} {visor.visorFaceText.text} {visor.FaceRotation} {visor.FaceSize}");
		visor2.SetAllFaceSettings(visor.hue.Value, visor.visorColorIndex, visor.visorFaceText.text, visor.FaceRotation, visor.FaceSize);
	}

	private void Jump()
	{
		timeToNextJump = Random.Range(minTimeBetweenJumps, maxTimeBetweenJumps);
		view.RPC("RPCA_MimicJump", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_MimicJump()
	{
		player.refs.controller.TryJump();
	}
}
