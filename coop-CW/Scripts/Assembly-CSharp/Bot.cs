using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

public class Bot : MonoBehaviour, IHasPatrolGroup
{
	[Serializable]
	public class SyncData
	{
		public int targetPlayerId = -1;

		public Vector3 lookDireciton;

		public Vector2 movementInput;

		public bool sprint;
	}

	public int jumpScareLevel = 2;

	public bool canJumpScareFromBehind;

	public List<PatrolPoint.PatrolGroup> patrolGroups = new List<PatrolPoint.PatrolGroup>();

	public Transform centerTransform;

	public Transform groundTransform;

	public bool flashLightRequireLineOfSight;

	public int lightLevelRequired;

	public SyncData syncData;

	public int lastPlayerID = -1;

	public Player targetPlayer;

	public Rigidbody targetBodypart;

	public float seeBodypartValue;

	public float sinceLookForTarget = 10f;

	public Vector3 navTargetPos_Set;

	public Vector3 navDestination_Read;

	public Vector3 navDirection_Read;

	public PatrolPoint lastPatrolPoint;

	internal PhotonView view;

	public int currentPlayerCheckID;

	public float suspicionValue;

	public float targetAngle;

	public float targetAngle_Nav;

	public int framesSincePatrol = 10;

	public PatrolPoint patrolPoint;

	public float remainingNavDistance;

	public bool targetIsHiding;

	public float sinceAttack = 10f;

	public bool attacking;

	public bool aggro;

	private UniversalBotSounds botSounds;

	public List<Player> ignoredPlayers = new List<Player>();

	public float distanceToTarget_Flat;

	public float distanceToTarget;

	public bool slowDownWhenNavigating = true;

	public float moveSpeedMultiplier = 1f;

	public float navigationSpeedAdjustment = 1f;

	public float sinceFlashLit = 10f;

	public float sinceFlashLit_LevelLight = 10f;

	public float sinceFlashLit_PlayerLight = 10f;

	private Action m_OnTargetChangeAction;

	public bool targetUnReachable;

	public float targetOutOfRangeFor;

	public float sinceLastSawTarget;

	public Vector3 lastNoisePos;

	public float sinceHeardNoise = 10f;

	public bool hurt;

	public Vector3 navOffset;

	public Vector3 lastGodNavPos;

	public bool busy;

	private float sinceSyncAttacking = 10f;

	public bool hasSearchPoint;

	public Vector3 searchPoint;

	public float hasBeenSearchingFor;

	public float sinceAlert = 10f;

	public int nrOfAlerts;

	internal bool alertable = true;

	public Bot patrolLeader;

	public Action<Vector3> teleportAction;

	public float turnVel;

	public float animMoveSpeedFactor = 1f;

	public int attackType;

	public bool hasConditionalObject;

	public GameObject conditionalObject;

	private void Start()
	{
		BotHandler.instance.bots.Add(this);
		syncData.lookDireciton = base.transform.forward;
		navDirection_Read = base.transform.forward;
		navTargetPos_Set = base.transform.position;
		currentPlayerCheckID = UnityEngine.Random.Range(0, 4);
		SFX_Player instance = SFX_Player.instance;
		instance.playNoiseAction = (Action<Vector3, float, int>)Delegate.Combine(instance.playNoiseAction, new Action<Vector3, float, int>(OnNoisePlayed));
		view = GetComponent<PhotonView>();
		botSounds = GetComponentInParent<UniversalBotSounds>();
	}

	private void OnDestroy()
	{
		BotHandler.instance.bots.Remove(this);
		SFX_Player instance = SFX_Player.instance;
		instance.playNoiseAction = (Action<Vector3, float, int>)Delegate.Remove(instance.playNoiseAction, new Action<Vector3, float, int>(OnNoisePlayed));
	}

	private void Update()
	{
		SetTransform();
		GetTargetPlayer();
		UpdateLocalVariables();
		if (view.IsMine)
		{
			UpdateVariables();
			if (view.IsMine && hasConditionalObject && conditionalObject == null)
			{
				hasConditionalObject = false;
				PhotonNetwork.Destroy(base.transform.root.gameObject);
			}
			DoNavigationSlowing();
		}
	}

	private void DoNavigationSlowing()
	{
		navigationSpeedAdjustment = 1f;
		if (slowDownWhenNavigating)
		{
			navigationSpeedAdjustment = Mathf.Lerp(0f, 1f, remainingNavDistance * 0.2f);
			navigationSpeedAdjustment *= Mathf.Lerp(1f, 0f, targetAngle_Nav * 0.011f);
		}
	}

	public float SpeedFactor()
	{
		return navigationSpeedAdjustment * moveSpeedMultiplier * animMoveSpeedFactor;
	}

	public bool Moving()
	{
		if (syncData.movementInput.x == 0f)
		{
			return syncData.movementInput.y != 0f;
		}
		return true;
	}

	public bool BusyOrAttacking()
	{
		if (busy)
		{
			return true;
		}
		if (attacking)
		{
			return true;
		}
		return false;
	}

	private void SetTransform()
	{
		base.transform.SetPositionAndRotation(groundTransform.position, Quaternion.LookRotation(syncData.lookDireciton));
	}

	private void UpdateLocalVariables()
	{
		if ((bool)targetPlayer)
		{
			Vector3 vector = Center();
			Vector3 vector2 = targetPlayer.Center();
			distanceToTarget_Flat = HelperFunctions.FlatDistance(vector, vector2);
			distanceToTarget = Vector3.Distance(vector, vector2);
			if (base.transform.InverseTransformPoint(vector2).x < 0f)
			{
				targetAngle = 0f - Vector3.Angle(syncData.lookDireciton, vector2 - vector);
			}
			else
			{
				targetAngle = Vector3.Angle(syncData.lookDireciton, vector2 - vector);
			}
		}
		else
		{
			targetAngle = 0f;
		}
		sinceFlashLit += Time.deltaTime;
		sinceFlashLit_LevelLight += Time.deltaTime;
		sinceFlashLit_PlayerLight += Time.deltaTime;
	}

	private void UpdateVariables()
	{
		seeBodypartValue = Mathf.MoveTowards(seeBodypartValue, 0f, Time.deltaTime);
		sinceLookForTarget += Time.deltaTime;
		if (sinceLookForTarget > 0.5f)
		{
			targetBodypart = null;
		}
		if ((bool)patrolLeader && patrolLeader.framesSincePatrol < 10 && (bool)patrolLeader.patrolPoint && patrolPoint != patrolLeader.patrolPoint)
		{
			if (HelperFunctions.BoxDistance(Center(), patrolLeader.Center()) > 20f)
			{
				patrolLeader = null;
			}
			else
			{
				patrolPoint = patrolLeader.patrolPoint;
			}
		}
		if (sinceAlert > 5f)
		{
			nrOfAlerts = 0;
		}
		sinceAlert += Time.deltaTime;
		framesSincePatrol++;
		if (targetUnReachable)
		{
			targetOutOfRangeFor += Time.deltaTime;
		}
		else
		{
			targetOutOfRangeFor = 0f;
		}
		if ((bool)targetPlayer && targetPlayer.data.dead)
		{
			LoseTarget();
		}
		if (attacking)
		{
			sinceAttack = 0f;
		}
		else
		{
			sinceAttack += Time.deltaTime;
		}
	}

	private void GetTargetPlayer()
	{
		if (lastPlayerID != syncData.targetPlayerId)
		{
			lastPlayerID = syncData.targetPlayerId;
			if (syncData.targetPlayerId == -1)
			{
				targetPlayer = null;
				m_OnTargetChangeAction?.Invoke();
			}
			else
			{
				PhotonView photonView = PhotonNetwork.GetPhotonView(lastPlayerID);
				if ((bool)photonView)
				{
					targetPlayer = photonView.GetComponent<Player>();
					m_OnTargetChangeAction?.Invoke();
				}
			}
		}
		if (view.IsMine && targetPlayer == null && syncData.targetPlayerId != -1)
		{
			LoseTarget();
		}
	}

	internal Vector3 Center()
	{
		if (centerTransform == null)
		{
			if (base.gameObject == null)
			{
				Debug.LogError("Gameobject is null");
			}
			else
			{
				Debug.LogError(base.gameObject.name + " has no center transform", base.gameObject);
			}
		}
		return centerTransform.position;
	}

	private void SearchPoint(float rotationSpeed)
	{
		navTargetPos_Set = searchPoint;
		Look(navDirection_Read, rotationSpeed);
		syncData.movementInput = new Vector2(0f, 1f);
		hasBeenSearchingFor += Time.deltaTime;
		if (HelperFunctions.FlatDistance(Center(), searchPoint) < 1f)
		{
			hasSearchPoint = false;
		}
		if (hasBeenSearchingFor > 15f)
		{
			hasSearchPoint = false;
		}
	}

	public void Alert(Vector3 alertPosition, int alerts = 1)
	{
		if (alertable && !aggro && !(sinceAlert < 0.25f))
		{
			nrOfAlerts += alerts;
			sinceAlert = 0f;
			searchPoint = alertPosition;
			if ((bool)botSounds)
			{
				float spookAmount = Mathf.Clamp01((float)nrOfAlerts * 0.1f);
				botSounds.PlayAlertSound(spookAmount);
			}
			if (nrOfAlerts > 10)
			{
				hasSearchPoint = true;
				nrOfAlerts = 0;
				hasBeenSearchingFor = 0f;
			}
		}
	}

	public bool Patrol(bool look = true, bool walk = true, float rotationSpeed = 3f, bool listenToNoise = true, Vector3 preferedDirection = default(Vector3), bool alertable = true)
	{
		framesSincePatrol = 0;
		if (hasSearchPoint && alertable)
		{
			SearchPoint(rotationSpeed);
			return false;
		}
		if (listenToNoise && HasRecentNoise())
		{
			LookAt(lastNoisePos, rotationSpeed);
			StandStill();
			return false;
		}
		if (framesSincePatrol > 5 || this.patrolPoint == null)
		{
			this.patrolPoint = Level.currentLevel.GetClosestPoint(patrolGroups, Center(), this.patrolPoint, 4f, includeTemporary: true);
			VerboseDebug.Log("FIRST PATROL");
		}
		bool result = false;
		if (!this.patrolPoint)
		{
			return false;
		}
		if (HelperFunctions.FlatDistance(Center(), this.patrolPoint.transform.position) < 3f)
		{
			if (preferedDirection.sqrMagnitude < 0.01f && lastPatrolPoint != null && lastPatrolPoint != this.patrolPoint)
			{
				preferedDirection = (Center() - lastPatrolPoint.transform.position).normalized;
			}
			PatrolPoint patrolPoint = lastPatrolPoint;
			lastPatrolPoint = this.patrolPoint;
			this.patrolPoint = this.patrolPoint.GetNeighbor(patrolGroups, lastPatrolPoint, preferedDirection);
			if (!this.patrolPoint)
			{
				return false;
			}
			if ((bool)patrolPoint && patrolPoint == this.patrolPoint)
			{
				result = true;
			}
		}
		navTargetPos_Set = this.patrolPoint.transform.position;
		if (look)
		{
			Look(navDirection_Read, rotationSpeed);
		}
		if (walk)
		{
			Walk();
		}
		return result;
	}

	internal void KeepDistanceHover(Vector3 pos, float distance, float sideMove)
	{
		Vector3 normalized = (Center() - pos).Flat().normalized;
		Vector3 vector = Vector3.Cross(normalized, Vector3.up);
		navTargetPos_Set = pos + normalized * distance + vector * sideMove;
		SetMovementWorld(navDirection_Read);
		LookAt(pos);
	}

	internal bool CanSee(Vector3 from, Vector3 to, float maxDistance, float maxAngle)
	{
		if (Vector3.SqrMagnitude(from - to) > maxDistance * maxDistance)
		{
			return false;
		}
		if ((bool)HelperFunctions.LineCheck(from, to, HelperFunctions.LayerType.TerrainProp).transform)
		{
			return false;
		}
		if (Vector3.Angle(syncData.lookDireciton, to - from) > maxAngle)
		{
			return false;
		}
		return true;
	}

	public void StandStill()
	{
		syncData.movementInput = new Vector2(0f, 0f);
	}

	public void Walk()
	{
		syncData.movementInput = new Vector2(0f, 1f);
		syncData.sprint = false;
	}

	public void Run()
	{
		syncData.movementInput = new Vector2(0f, 1f);
		syncData.sprint = true;
	}

	internal void WorldMoveTo(Vector3 position)
	{
		navTargetPos_Set = position;
		SetMovementWorld(navDirection_Read);
	}

	public void SetMovementWorld(Vector3 worldMove)
	{
		Vector3 vector = base.transform.InverseTransformDirection(worldMove);
		syncData.movementInput = new Vector2(vector.x, vector.z);
	}

	internal void Investigate()
	{
		StandStill();
		navTargetPos_Set = targetPlayer.Center();
		LookAt(targetPlayer.Center());
	}

	internal void LoseTarget()
	{
		suspicionValue = 0f;
		aggro = false;
		syncData.targetPlayerId = -1;
		Player player = targetPlayer;
		targetPlayer = null;
		if (player != targetPlayer)
		{
			m_OnTargetChangeAction?.Invoke();
		}
	}

	public void ChaseTarget(Vector3 headPosition, float targetDistance = 1.75f, float sidePrediction = 1f, bool lookForBetterTarget = true, float rotationSpeed = 3f, bool loseInterestIfUnreachable = false, bool backUpIfTooClose = true, bool canRotateWhenStandingStill = true)
	{
		if (lookForBetterTarget)
		{
			LookForBetterTarget(headPosition);
		}
		Vector3 vector = Vector3.ProjectOnPlane(targetPlayer.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity * sidePrediction, targetPlayer.Center() - Center()).Flat();
		vector *= Mathf.InverseLerp(0f, 30f, Vector3.Distance(Center(), targetPlayer.Center()));
		Vector3 vector2 = targetPlayer.CenterGroundPos() + vector + navOffset;
		navTargetPos_Set = vector2;
		if (targetUnReachable)
		{
			if (canRotateWhenStandingStill)
			{
				LookAt(targetPlayer.Center(), rotationSpeed);
			}
			StandStill();
			if (loseInterestIfUnreachable && targetOutOfRangeFor > 1f)
			{
				WalkAway((Center() - targetPlayer.Center()).Flat());
				IgnoreTargetFor(targetPlayer, 3f);
				LoseTarget();
			}
		}
		else if (remainingNavDistance > targetDistance)
		{
			Look(navDirection_Read, rotationSpeed);
			Run();
		}
		else
		{
			if (canRotateWhenStandingStill)
			{
				LookAt(targetPlayer.Center(), rotationSpeed);
			}
			if (remainingNavDistance < targetDistance - 0.3f && backUpIfTooClose)
			{
				Back();
			}
			else
			{
				StandStill();
			}
		}
	}

	private void WalkAway(Vector3 walkAwayDir)
	{
		patrolPoint = Level.currentLevel.GetClosestPoint(patrolGroups, Center() + walkAwayDir.normalized * 10f, patrolPoint, 1000000f, includeTemporary: true);
	}

	public void IgnoreTargetFor(Player target, float ignoreFor)
	{
		StartCoroutine(IIgnoreTargetFor(target, ignoreFor));
		IEnumerator IIgnoreTargetFor(Player item, float seconds)
		{
			ignoredPlayers.Add(item);
			yield return new WaitForSeconds(seconds);
			if (ignoredPlayers.Contains(item))
			{
				ignoredPlayers.Remove(item);
			}
		}
	}

	private void Back()
	{
		syncData.movementInput = new Vector2(0f, -0.3f);
		syncData.sprint = false;
	}

	public void LookAt(Vector3 position, float rotationSpeed = 3f)
	{
		Look(position - Center(), rotationSpeed);
	}

	public float AngleToLookDirection(Vector3 direction)
	{
		return Vector3.Angle(syncData.lookDireciton, direction);
	}

	public void Look(Vector3 direction, float rotationSpeed = 3f)
	{
		targetAngle_Nav = HelperFunctions.FlatAngle(syncData.lookDireciton, direction);
		syncData.lookDireciton = Vector3.RotateTowards(syncData.lookDireciton, direction, Time.deltaTime * rotationSpeed, 0f);
	}

	internal bool CanSeeTarget(Vector3 headPosition, float angle = 110f)
	{
		Vector3 vector = targetPlayer.Center();
		if ((bool)targetBodypart)
		{
			vector = targetBodypart.position;
		}
		if (ignoredPlayers.Contains(targetPlayer))
		{
			return false;
		}
		if (Vector3.SqrMagnitude(Center() - vector) > 4900f)
		{
			return false;
		}
		if (Vector3.Angle(vector - Center(), syncData.lookDireciton) > angle)
		{
			return false;
		}
		if ((bool)HelperFunctions.LineCheck(headPosition, vector, HelperFunctions.LayerType.TerrainProp).transform)
		{
			return false;
		}
		return true;
	}

	public Rigidbody SearchForTargetBodyPart(Vector3 headPosition, float maxRange = 70f, float maxAngle = 110f)
	{
		Player nextPlayerAlive = PlayerHandler.instance.GetNextPlayerAlive(ref currentPlayerCheckID);
		if (!nextPlayerAlive)
		{
			return null;
		}
		if (ignoredPlayers.Contains(nextPlayerAlive))
		{
			return null;
		}
		if (Vector3.SqrMagnitude(Center() - nextPlayerAlive.Center()) > maxRange * maxRange)
		{
			return null;
		}
		if (Vector3.Angle(nextPlayerAlive.Center() - Center(), syncData.lookDireciton) > maxAngle)
		{
			return null;
		}
		Rigidbody rig = nextPlayerAlive.refs.ragdoll.GetRandomBodypart().rig;
		if ((bool)HelperFunctions.LineCheck(headPosition, rig.transform.position, HelperFunctions.LayerType.TerrainProp).transform)
		{
			return null;
		}
		targetBodypart = rig;
		return rig;
	}

	private void OnNoisePlayed(Vector3 noisePosition, float noiseRange, int alerts = 1)
	{
		Vector3 vector = Center();
		Vector3 vector2 = noisePosition - vector;
		if (!(Mathf.Abs(vector2.x) > noiseRange) && !(Mathf.Abs(vector2.y) > noiseRange * 0.3f) && !(Mathf.Abs(vector2.z) > noiseRange) && !(vector2.magnitude > noiseRange))
		{
			Alert(noisePosition, alerts);
			lastNoisePos = noisePosition;
			sinceHeardNoise = Time.time;
		}
	}

	public bool HasRecentNoise()
	{
		return sinceHeardNoise + 1f > Time.time;
	}

	public bool LookForTarget(Vector3 headPosition, float maxRange = 70f, float maxAngle = 110f, float timeToSeeTarget = 1f)
	{
		sinceLookForTarget = 0f;
		if ((bool)targetBodypart)
		{
			if (CanSee(headPosition, targetBodypart.position, maxRange, maxAngle))
			{
				seeBodypartValue = Mathf.MoveTowards(seeBodypartValue, timeToSeeTarget + 0.2f, Time.deltaTime * 2f);
			}
			else
			{
				targetBodypart = null;
			}
			if (seeBodypartValue > timeToSeeTarget)
			{
				seeBodypartValue = 0f;
				syncData.targetPlayerId = targetBodypart.GetComponentInParent<Player>().refs.view.ViewID;
				return true;
			}
		}
		else
		{
			SearchForTargetBodyPart(headPosition, maxRange, maxAngle);
		}
		return false;
	}

	public Player TryToReturnTarget(Vector3 headPosition, float maxRange = 70f, float maxAngle = 110f)
	{
		Rigidbody rigidbody = SearchForTargetBodyPart(headPosition, maxRange, maxAngle);
		if (!rigidbody)
		{
			return null;
		}
		return rigidbody.GetComponentInParent<Player>();
	}

	public void LookForBetterTarget(Vector3 headPosition, float maxRange = 70f, float maxAngle = 110f)
	{
		Rigidbody rigidbody = SearchForTargetBodyPart(headPosition, maxRange, maxAngle);
		if (!rigidbody)
		{
			return;
		}
		Player componentInParent = rigidbody.GetComponentInParent<Player>();
		if (!componentInParent)
		{
			return;
		}
		if (!HelperFunctions.LineCheck(headPosition, targetPlayer.Center(), HelperFunctions.LayerType.TerrainProp).transform)
		{
			float num = Vector3.Distance(headPosition, rigidbody.position) / componentInParent.Visibility();
			float num2 = Vector3.Distance(headPosition, targetPlayer.Center()) / targetPlayer.Visibility();
			if (!(num < num2))
			{
				return;
			}
		}
		syncData.targetPlayerId = componentInParent.refs.view.ViewID;
	}

	public void RotateThenMove(Vector3 dir, float rotationSpeed = 3f)
	{
		float num = HelperFunctions.FlatAngle(dir, syncData.lookDireciton);
		float num2 = 5f;
		if (num < num2)
		{
			syncData.movementInput = new Vector2(0f, 1f);
		}
		else
		{
			StandStill();
		}
		Look(dir, rotationSpeed);
	}

	public void SetTargetPlayer(Player player)
	{
		Player player2 = targetPlayer;
		targetPlayer = player;
		syncData.targetPlayerId = targetPlayer.refs.view.ViewID;
		if (targetPlayer != player2)
		{
			m_OnTargetChangeAction?.Invoke();
		}
	}

	public void AddOnTargetChangeAction(Action a)
	{
		m_OnTargetChangeAction = (Action)Delegate.Combine(m_OnTargetChangeAction, a);
	}

	public void RemoveTargetChangeAction(Action a)
	{
		m_OnTargetChangeAction = (Action)Delegate.Remove(m_OnTargetChangeAction, a);
	}

	public void Destroy()
	{
		PhotonNetwork.Destroy(base.transform.root.gameObject);
	}

	internal void InvestigateCurrentTarget(Vector3 headPos, float aggroSpeed = 1f, float maxRange = 30f, float rotationSpeed = 3f, bool lookAt = true, bool searchPoint = true)
	{
		LookForBetterTarget(headPos);
		if (hasSearchPoint && searchPoint)
		{
			SearchPoint(5f);
		}
		else
		{
			StandStill();
			navTargetPos_Set = targetPlayer.Center();
			if (lookAt)
			{
				LookAt(targetPlayer.Center(), rotationSpeed);
			}
		}
		if (CanSeeTarget(headPos))
		{
			Alert(targetPlayer.Center());
			float t = Mathf.InverseLerp(5f, maxRange, Vector3.Distance(headPos, targetPlayer.Center()));
			t = Mathf.Lerp(1f, 0.2f, t);
			float num = 0.8f;
			if (targetPlayer.data.isCrouching)
			{
				num *= 0.5f;
			}
			else if (targetPlayer.data.isSprinting)
			{
				num *= 1.5f;
			}
			if (targetPlayer.data.microphoneValue > 0.5f)
			{
				num *= 2f;
			}
			suspicionValue += Time.deltaTime * t * aggroSpeed * num;
			if (suspicionValue > 1f)
			{
				aggro = true;
				suspicionValue = 0f;
				sinceLastSawTarget = 0f;
			}
		}
		else
		{
			suspicionValue -= Time.deltaTime;
			suspicionValue = Mathf.Clamp(suspicionValue, -1.1f, 100f);
			if (suspicionValue < -1f)
			{
				LoseTarget();
			}
		}
	}

	public void ValidateChase(Vector3 headPosition, float loseTargetTime = 4.5f)
	{
		if (!targetPlayer || ignoredPlayers.Contains(targetPlayer) || (bool)HelperFunctions.LineCheck(headPosition, targetPlayer.Center(), HelperFunctions.LayerType.TerrainProp).transform)
		{
			sinceLastSawTarget += Time.deltaTime;
			if (sinceLastSawTarget > loseTargetTime)
			{
				syncData.targetPlayerId = -1;
				aggro = false;
				suspicionValue = 0f;
			}
		}
		else
		{
			sinceLastSawTarget -= Time.deltaTime;
			sinceLastSawTarget = Mathf.Clamp(sinceLastSawTarget, 0f, 100f);
		}
	}

	internal void SetSyncAttacking(bool setAttacking)
	{
		if (!(sinceSyncAttacking < 0.5f) && setAttacking != attacking)
		{
			view.RPC("RPCA_BotSetAttacking", RpcTarget.All, setAttacking);
		}
	}

	[PunRPC]
	private void RPCA_BotSetAttacking(bool setAttacking)
	{
		attacking = setAttacking;
	}

	internal void DoNothing()
	{
		syncData.movementInput = Vector2.zero;
		syncData.sprint = false;
	}

	public List<PatrolPoint.PatrolGroup> GetGroup()
	{
		return patrolGroups;
	}

	public void SetLeader(Bot leader)
	{
		patrolLeader = leader;
	}

	internal void Teleport(Vector3 position)
	{
		teleportAction?.Invoke(position);
		patrolPoint = null;
	}

	internal Vector3 GroundPos()
	{
		return groundTransform.position;
	}

	internal bool AbleToAttack(float range = 4f, float minSinceAttack = 2.5f, Player playerReference = null)
	{
		if (!view.IsMine)
		{
			return false;
		}
		if (targetPlayer == null)
		{
			return false;
		}
		if (BusyOrAttacking())
		{
			return false;
		}
		if (!aggro)
		{
			return false;
		}
		if (distanceToTarget > range)
		{
			return false;
		}
		if (sinceAttack < minSinceAttack)
		{
			return false;
		}
		if ((bool)playerReference && playerReference.NoControl())
		{
			return false;
		}
		return true;
	}

	internal Player GetNearbyPlayerInSight(float range, float angle)
	{
		for (int i = 0; i < PlayerHandler.instance.playersAlive.Count; i++)
		{
			Player player = PlayerHandler.instance.playersAlive[i];
			if (!ignoredPlayers.Contains(player) && CanSee(Center(), player.Center(), range, angle))
			{
				return player;
			}
		}
		return null;
	}

	internal float TargetLookY()
	{
		if (targetPlayer == null)
		{
			return 0f;
		}
		return (targetPlayer.Center() - Center()).normalized.y;
	}
}
