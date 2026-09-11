using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class Bot_Infiltrator : MonoBehaviour
{
	private enum AKWARD_SOVLER
	{
		NOTHING = 0,
		RUN_AWAY = 1,
		LOOK_AWAY = 2,
		FLICKER = 3,
		EMOTE = 4
	}

	public class VisorSettings
	{
		public float FaceRotation;

		public float FaceSize;

		public float hue;

		public int visorColorIndex;

		public string visorFaceText;

		public VisorSettings(PlayerVisor visor)
		{
			hue = visor.hue.Value;
			visorColorIndex = visor.visorColorIndex;
			visorFaceText = visor.visorFaceText.text;
			FaceRotation = visor.FaceRotation;
			FaceSize = visor.FaceSize;
		}
	}

	public bool debugNoAwkwardSolvers;

	public bool debugNoAngry;

	public bool debug;

	[HideInInspector]
	public Player player;

	public float customRotationSpeed = 6f;

	public float minTimeBeforeWalkSprintSwitch = 1f;

	public float runUntilThisCloseFromTarget = 9f;

	public float stopWhenThisCloseToTarget = 8f;

	public float distanceToBeConsideredAlone = 100f;

	public Player hitTarget;

	public Player mimickingPlayer;

	public float angryThreshold = 10f;

	public bool isAngry;

	public float jabForce = 30f;

	public Item strangleEmote;

	public float stranglePullForce = 20f;

	public float jabDistance = 1.5f;

	public string angryFaceText = ">:(";

	public float angryFaceRotation = -90f;

	public Color angryFaceColor;

	public float lookAwayChance = 0.3f;

	public float runAwayChance = 0.1f;

	public float flickerChance = 0.05f;

	public float nothingChance = 0.3f;

	public float emoteChance = 0.4f;

	public float awkwardThreshHold = 5f;

	public List<Item> emotes = new List<Item>();

	private readonly Vector3 faceSizeMinMax = new Vector3(0.025f, 0.035f);

	private float angryMeter;

	private bool awkwardLookAway;

	private Vector3 awkwardLookDir;

	private float awkwardTime;

	private Bot bot;

	private bool doEmote;

	private bool exfiltrate;

	private FakeFlashLight fakeFlashLight_grc;

	private bool followPlayer;

	private bool hasAnyoneEverSeenMe;

	private bool isStrangling;

	private VisorSettings mimicVisorSettings;

	private Vector3 moveAwayPos;

	private PatrolPoint randomPoint;

	private bool runFromPlayer;

	private bool runToPlayer;

	private bool startedEmote;

	private float timeAlive;

	private float timeExfiltrating;

	private float timeSinceJab;

	private float timeSinceWalkSprintSwitch;

	private float timeTargetSpentLookingAtMe;

	private float timeTargetSpentNotLookingAtMe;

	private float timeToJab;

	private PhotonView view;

	private bool walkAway;

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
				timeSinceWalkSprintSwitch = UnityEngine.Random.Range(minTimeBeforeWalkSprintSwitch * -0.5f, minTimeBeforeWalkSprintSwitch * 0.5f);
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

	public bool IsInfiltrating { get; set; }

	private void Awake()
	{
		player = GetComponentInParent<Player>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		followPlayer = true;
		fakeFlashLight_grc = base.transform.root.GetComponentInChildren<FakeFlashLight>();
	}

	private void Update()
	{
		if (!player.refs.view.IsMine)
		{
			return;
		}
		if (!IsInfiltrating)
		{
			Debug.LogError("Not Infiltrating");
			Debug.LogError("Needs to be spawned by infitraltor spanwer");
			return;
		}
		timeAlive += Time.deltaTime;
		timeSinceJab += Time.deltaTime;
		timeSinceWalkSprintSwitch += Time.deltaTime;
		if (bot.targetPlayer != null)
		{
			bot.navTargetPos_Set = bot.targetPlayer.Center();
		}
		bool flag = IsTargetLookingAtMe();
		IamLookingAtTarget();
		bool flag2 = false;
		bool isAnyoneLookingAtTarget = IsAnyoneLookingAtTarget();
		bool flag3 = AnyoneLookingAtMe();
		if (!hasAnyoneEverSeenMe && flag3)
		{
			hasAnyoneEverSeenMe = true;
		}
		bool isAnyoneLookingAtMeExceptTarget = AnyoneLookingAtMeExceptTarget();
		if (mimickingPlayer != null)
		{
			if (mimickingPlayer.CanSee(player.HeadPosition()))
			{
				_ = PlayerHandler.instance.playersAlive.Count > 1;
			}
			else
				_ = 0;
			flag2 = hitTarget.CanSee(mimickingPlayer.HeadPosition()) && PlayerHandler.instance.playersAlive.Count > 1;
		}
		PlayerHandler.instance.GetPlayersClosestDistanceToAnotherPlayer(hitTarget);
		if (mimickingPlayer != null)
		{
			player.data.remainingOxygen = mimickingPlayer.data.remainingOxygen;
		}
		if (hasAnyoneEverSeenMe && !flag3 && !isAnyoneLookingAtTarget && DistanceToHitTarget < runUntilThisCloseFromTarget * 1.2f)
		{
			angryMeter += Time.deltaTime;
			if (angryMeter > angryThreshold && !isAngry && !exfiltrate)
			{
				StartCoroutine(DoAngry());
			}
		}
		else
		{
			angryMeter = Mathf.MoveTowards(angryMeter, 0f, Time.deltaTime);
		}
		if (flag)
		{
			timeTargetSpentLookingAtMe += Time.deltaTime;
			timeTargetSpentNotLookingAtMe = 0f;
		}
		else
		{
			timeTargetSpentLookingAtMe = 0f;
			timeTargetSpentNotLookingAtMe += Time.deltaTime;
		}
		if ((mimickingPlayer == null || bot.targetPlayer == null || hitTarget == null || hitTarget.data.dead) && !exfiltrate)
		{
			Exfiltrate();
		}
		if (flag2 && timeAlive > 60f)
		{
			exfiltrate = true;
		}
		if (exfiltrate)
		{
			bool num = bot.Patrol(look: true, walk: false, customRotationSpeed, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false);
			bot.syncData.movementInput = new Vector2(0f, 1f);
			if (num && (bool)bot.lastPatrolPoint)
			{
				moveAwayPos = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			timeExfiltrating += Time.deltaTime;
			if (!flag3 && timeExfiltrating > 15f)
			{
				Despawn();
			}
			return;
		}
		if (walkAway)
		{
			bool num2 = bot.Patrol(look: true, walk: false, customRotationSpeed, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false);
			bot.syncData.movementInput = new Vector2(0f, 1f);
			if (num2 && (bool)bot.lastPatrolPoint)
			{
				moveAwayPos = bot.lastPatrolPoint.transform.position;
			}
			if (!runFromPlayer && DistanceToHitTarget < runUntilThisCloseFromTarget)
			{
				runFromPlayer = true;
			}
			if (runFromPlayer && DistanceToHitTarget > runUntilThisCloseFromTarget * 1.25f)
			{
				runFromPlayer = false;
			}
			Sprint = runFromPlayer;
			if (timeTargetSpentNotLookingAtMe > 5f && !exfiltrate)
			{
				walkAway = false;
				followPlayer = true;
			}
		}
		if (doEmote)
		{
			bot.LookAt(bot.targetPlayer.HeadPosition());
			float num3 = bot.AngleToLookDirection((bot.targetPlayer.HeadPosition() - bot.Center()).normalized);
			if (!startedEmote && num3 < 10f)
			{
				Debug.LogWarning("Started Emote");
				startedEmote = true;
				if (emotes.Count > 0)
				{
					player.refs.emotes.PlayEmote(emotes.GetRnd());
				}
			}
			if (startedEmote && player.data.emoteTime <= 0f)
			{
				doEmote = false;
				followPlayer = true;
				startedEmote = false;
			}
		}
		if (!followPlayer)
		{
			return;
		}
		bot.navTargetPos_Set = bot.targetPlayer.Center();
		if (Vector3.Distance(player.Center(), bot.navTargetPos_Set) < stopWhenThisCloseToTarget)
		{
			bot.StandStill();
			if (flag)
			{
				awkwardTime += Time.deltaTime;
				if (awkwardTime > awkwardThreshHold)
				{
					MakeItLessAwkward();
				}
			}
			else
			{
				awkwardTime = Mathf.MoveTowards(awkwardTime, 0f, Time.deltaTime);
			}
			if (awkwardLookAway)
			{
				if (timeTargetSpentNotLookingAtMe > 3f)
				{
					awkwardLookAway = false;
				}
				bot.Look(awkwardLookDir);
				Debug.DrawRay(bot.Center(), awkwardLookDir.normalized, Color.magenta);
			}
			else
			{
				bot.Look(bot.navDirection_Read);
			}
		}
		else
		{
			awkwardLookAway = false;
			bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
			awkwardTime = Mathf.MoveTowards(awkwardTime, 0f, Time.deltaTime);
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
		IEnumerator DoAngry()
		{
			ResetStates();
			isAngry = true;
			view.RPC("RPC_AngryVisuals", RpcTarget.All);
			isStrangling = false;
			float timeSinceDamage = float.MaxValue;
			float damageInterval = 0.5f;
			float killTime = 15f;
			float timeStrangling = 0f;
			while (isAngry)
			{
				angryMeter = 0f;
				bot.navTargetPos_Set = bot.targetPlayer.Center();
				bot.Look(bot.navDirection_Read);
				if (player.NoControl())
				{
					Exfiltrate();
				}
				if (isStrangling)
				{
					timeStrangling += Time.deltaTime;
					if (player.data.emoteTime <= 0f)
					{
						player.refs.emotes.PlayEmote(strangleEmote);
					}
					hitTarget.data.strangledForSeconds = 0.5f;
					if (hitTarget.data.emoteTime <= 0f)
					{
						hitTarget.refs.emotes.PlayChokedAnimation();
					}
					Vector3 vector = player.HeadPosition() - hitTarget.HeadPosition();
					hitTarget.SetLookDirection(Vector3.MoveTowards(hitTarget.data.lookDirection, vector.normalized, Time.deltaTime * 25f));
					bot.StandStill();
					bot.Look(-vector.normalized);
					timeSinceDamage += Time.deltaTime;
					if (timeSinceDamage > damageInterval)
					{
						float damage = Player.PlayerData.maxHealth / (killTime / damageInterval);
						hitTarget.CallTakeDamage(damage);
						timeSinceDamage = 0f;
					}
				}
				else if (isAnyoneLookingAtMeExceptTarget || isAnyoneLookingAtTarget)
				{
					ResetStates();
					followPlayer = true;
					view.RPC("RPC_ImitateVisuals", RpcTarget.All);
					isAngry = false;
				}
				else
				{
					Debug.LogWarning("Is Angry but not strangling, trying to hit target");
					if (timeToJab < 0f)
					{
						Bodypart bodypart = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R);
						Vector3 vector2 = bot.targetPlayer.HeadPosition() - bodypart.transform.position;
						Vector3 normalized = vector2.normalized;
						if (vector2.magnitude < jabDistance)
						{
							bodypart.rig.AddForce(normalized * jabForce, ForceMode.VelocityChange);
							timeToJab = 1f;
							timeSinceJab = 0f;
						}
						else
						{
							Debug.Log("Not close enough to hit");
						}
					}
					timeToJab -= Time.deltaTime;
					timeSinceWalkSprintSwitch = float.MaxValue;
					bot.syncData.sprint = true;
					bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
				}
				yield return null;
			}
		}
	}

	private void FixedUpdate()
	{
		if (isStrangling)
		{
			Vector3 vector = player.HeadPosition() - hitTarget.HeadPosition();
			bot.StandStill();
			bot.Look(-vector.normalized);
			Rigidbody rig = hitTarget.refs.ragdoll.GetBodypart(BodypartType.Head).rig;
			Bodypart bodypart = player.refs.ragdoll.GetBodypart(BodypartType.Hand_L);
			Vector3 vector2 = Vector3.Lerp(b: player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).transform.position, a: bodypart.transform.position, t: 0.5f) - rig.position;
			Vector3 vector3 = vector2.normalized * (Mathf.Clamp(vector2.magnitude, 0.3f, 1f) * stranglePullForce);
			rig.AddForce(vector3 * Time.deltaTime, ForceMode.Acceleration);
			player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.AddForce(-vector3 * Time.deltaTime, ForceMode.Acceleration);
		}
		if (!view.IsMine || !isAngry || isStrangling || !(timeSinceJab < 0.25f))
		{
			return;
		}
		Collider[] array = Physics.OverlapSphere(player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).transform.position, 1f);
		for (int i = 0; i < array.Length; i++)
		{
			Player componentInChildren = array[i].transform.root.GetComponentInChildren<Player>();
			if (isStrangling)
			{
				break;
			}
			if ((bool)componentInChildren && componentInChildren == hitTarget)
			{
				view.RPC("RPCA_StartStrangling", RpcTarget.All);
			}
		}
	}

	private void Exfiltrate()
	{
		Debug.Log("infiltrator Exfiltrate");
		ResetStates();
		exfiltrate = true;
		if (isStrangling)
		{
			view.RPC("RPCA_StopStrangling", RpcTarget.All);
		}
		moveAwayPos = player.Center();
	}

	[PunRPC]
	private void RPCA_StartStrangling()
	{
		isStrangling = true;
		player.refs.emotes.PlayEmote(strangleEmote);
		hitTarget.refs.emotes.PlayChokedAnimation();
	}

	[PunRPC]
	private void RPCA_StopStrangling()
	{
		isStrangling = false;
		hitTarget.data.strangledForSeconds = 0f;
	}

	private bool FindMimicTarget(out Player hitTarget, out Player mimicTarget)
	{
		if (debug)
		{
			hitTarget = Player.localPlayer;
			mimicTarget = Player.localPlayer;
			return true;
		}
		if (PlayerHandler.instance.GetLargestClosestDistanceBetweenPlayers(out var maxMinDistanceBetweenPlayers, out var mostAlonePlayer) && maxMinDistanceBetweenPlayers > distanceToBeConsideredAlone)
		{
			mimicTarget = mostAlonePlayer;
			hitTarget = PlayerHandler.instance.GetFurthestPlayerFromPlayer(mimicTarget);
			return true;
		}
		hitTarget = null;
		mimicTarget = null;
		return false;
	}

	private void SearchForTarget()
	{
		if (FindMimicTarget(out var player, out var mimicTarget))
		{
			foreach (PatrolPoint item in Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), player.Center(), 10f, 4f))
			{
				if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
				{
					Vector3 position = item.transform.position + Vector3.up;
					view.RPC("RPC_MimicPlayer", RpcTarget.All, mimicTarget.refs.view.ViewID, player.refs.view.ViewID);
					bot.Teleport(position);
					break;
				}
			}
		}
		if (!IsInfiltrating)
		{
			this.player.data.sinceGrounded = 0f;
			bot.Teleport(Vector3.up * 100f);
		}
	}

	public void Despawn()
	{
		PhotonNetwork.Destroy(base.transform.root.gameObject);
	}

	public void ResetStates()
	{
		walkAway = false;
		awkwardLookAway = false;
		followPlayer = false;
		doEmote = false;
		exfiltrate = false;
		isAngry = false;
	}

	public void DebugAwkardSolvers()
	{
		new List<AKWARD_SOVLER>
		{
			AKWARD_SOVLER.NOTHING,
			AKWARD_SOVLER.RUN_AWAY,
			AKWARD_SOVLER.LOOK_AWAY,
			AKWARD_SOVLER.FLICKER,
			AKWARD_SOVLER.EMOTE
		}.GetLedgerOfChances(GetWeight, 1000).PrintLedger(1000);
		float GetWeight(AKWARD_SOVLER akwardSovler)
		{
			return akwardSovler switch
			{
				AKWARD_SOVLER.NOTHING => nothingChance, 
				AKWARD_SOVLER.RUN_AWAY => runAwayChance, 
				AKWARD_SOVLER.LOOK_AWAY => lookAwayChance, 
				AKWARD_SOVLER.FLICKER => flickerChance, 
				AKWARD_SOVLER.EMOTE => emoteChance, 
				_ => throw new ArgumentOutOfRangeException("akwardSovler", akwardSovler, null), 
			};
		}
	}

	private void MakeItLessAwkward()
	{
		AKWARD_SOVLER weightedRandom = new List<AKWARD_SOVLER>
		{
			AKWARD_SOVLER.NOTHING,
			AKWARD_SOVLER.RUN_AWAY,
			AKWARD_SOVLER.LOOK_AWAY,
			AKWARD_SOVLER.FLICKER,
			AKWARD_SOVLER.EMOTE
		}.GetWeightedRandom(GetWeight);
		ResetStates();
		switch (weightedRandom)
		{
		case AKWARD_SOVLER.NOTHING:
			followPlayer = true;
			awkwardTime = UnityEngine.Random.Range(0f, awkwardThreshHold * 0.5f);
			break;
		case AKWARD_SOVLER.RUN_AWAY:
			walkAway = true;
			moveAwayPos = player.Center() - player.data.lookDirection.xoz() * 3f;
			awkwardTime = UnityEngine.Random.Range(0f, awkwardThreshHold * 0.1f);
			break;
		case AKWARD_SOVLER.LOOK_AWAY:
			awkwardLookAway = true;
			followPlayer = true;
			awkwardLookDir = UnityEngine.Random.insideUnitSphere;
			awkwardTime = UnityEngine.Random.Range(0f, awkwardThreshHold * 0.5f);
			break;
		case AKWARD_SOVLER.FLICKER:
			StartCoroutine(FlickerRed());
			awkwardTime = UnityEngine.Random.Range(0f, awkwardThreshHold * 0.9f);
			followPlayer = true;
			break;
		case AKWARD_SOVLER.EMOTE:
			doEmote = true;
			awkwardTime = UnityEngine.Random.Range(0f, awkwardThreshHold * 0.5f);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		float GetWeight(AKWARD_SOVLER akwardSovler)
		{
			return akwardSovler switch
			{
				AKWARD_SOVLER.NOTHING => nothingChance, 
				AKWARD_SOVLER.RUN_AWAY => runAwayChance, 
				AKWARD_SOVLER.LOOK_AWAY => lookAwayChance, 
				AKWARD_SOVLER.FLICKER => flickerChance, 
				AKWARD_SOVLER.EMOTE => emoteChance, 
				_ => throw new ArgumentOutOfRangeException("akwardSovler", akwardSovler, null), 
			};
		}
	}

	private bool AnyoneLookingAtMe()
	{
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if (item.CanSee(player.HeadPosition(), 65f, checkLineOfSight: true, HelperFunctions.LayerType.Terrain))
			{
				return true;
			}
		}
		return false;
	}

	private bool AnyoneLookingAtMeExceptTarget()
	{
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if (!(item == hitTarget) && item.CanSee(player.HeadPosition(), 65f, checkLineOfSight: true, HelperFunctions.LayerType.Terrain))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAnyoneLookingAtTarget()
	{
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if (!(item == hitTarget) && item.CanSee(hitTarget.HeadPosition(), 70f, checkLineOfSight: true, HelperFunctions.LayerType.Terrain))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsTargetLookingAtMe()
	{
		if (bot.targetPlayer == null)
		{
			return false;
		}
		return bot.targetPlayer.CanSee(player.HeadPosition(), 65f, checkLineOfSight: true, HelperFunctions.LayerType.Terrain);
	}

	private bool IamLookingAtTarget()
	{
		if (bot.targetPlayer == null)
		{
			return false;
		}
		return player.CanSee(bot.targetPlayer.HeadPosition());
	}

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
		view.RPC("RPC_ImitateVisuals", RpcTarget.All);
	}

	public void Init(Player mimicPlayer, Player hitTarget)
	{
		view.RPC("RPC_MimicPlayer", RpcTarget.All, mimicPlayer.refs.view.ViewID, hitTarget.refs.view.ViewID);
	}

	[PunRPC]
	private void RPC_MimicPlayer(int mimicID, int hitTargetID)
	{
		IsInfiltrating = true;
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(mimicID);
		Player player2 = PlayerHandler.instance.TryGetPlayerFromViewID(hitTargetID);
		Debug.Log($"Infiltrating Mimic:{player} HitTarget: {player2}");
		if (player == null || player2 == null)
		{
			Debug.LogError("Mimic or hitTarget is null!");
			return;
		}
		mimickingPlayer = player;
		mimicVisorSettings = new VisorSettings(mimickingPlayer.refs.visor);
		hitTarget = player2;
		base.transform.root.GetComponentInChildren<FakeFlashLight>().Toggle(on: true);
		bot.SetTargetPlayer(player2);
		this.player.Call_EquipHat((mimickingPlayer.data.currentHat != null) ? mimickingPlayer.data.currentHat.runtimeHatIndex : (-1));
		RPC_ImitateVisuals();
	}

	private void EquipHat(int i)
	{
		player.Call_EquipHat(i);
	}

	[PunRPC]
	private void RPC_ImitateVisuals()
	{
		if (mimicVisorSettings == null)
		{
			Debug.LogError("mimicingVisor is null");
		}
		PlayerVisor visor = player.refs.visor;
		if (visor == null)
		{
			Debug.LogError("myVisor is null");
		}
		base.transform.root.GetComponentInChildren<FakeFlashLight>().ColorDefault();
		Debug.LogWarning($"mimicVisor {mimicVisorSettings.hue} {mimicVisorSettings.visorColorIndex} {mimicVisorSettings.visorFaceText} {mimicVisorSettings.FaceRotation} {mimicVisorSettings.FaceSize}");
		visor.SetAllFaceSettings(mimicVisorSettings.hue, mimicVisorSettings.visorColorIndex, mimicVisorSettings.visorFaceText, mimicVisorSettings.FaceRotation, mimicVisorSettings.FaceSize);
	}

	private void Jump()
	{
		view.RPC("RPCA_MimicJump", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_MimicJump()
	{
		player.refs.controller.TryJump();
	}
}
