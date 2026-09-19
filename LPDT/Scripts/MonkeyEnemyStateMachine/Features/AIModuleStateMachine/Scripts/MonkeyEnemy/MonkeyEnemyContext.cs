using System.Collections.Generic;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.AudioServiceModule.Scripts;
using Features.CollectingModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelGatesModule.Data;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyEnemyContext : MonoBehaviour
	{
		[SerializeField]
		private MonkeyEnemy _monkeyEnemy;

		public EventInstance AttackSoundInstance;

		public EventInstance MoveAngryInstance;

		public EventInstance MoveFriendlyInstance;

		public EventInstance CoinRequestSoundInstance;

		public EventInstance TakeCoinSoundInstance;

		public EventInstance HitSoundInstance;

		private PlayersGatesModelSynchronizedModel _playersGatesModel;

		private INavigationService _navigationService;

		private IEnemyAttractionZoneService _attractionZoneService;

		private MultiplayerModel _multiplayerModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private CollectItemFellEvent _collectItemFellEvent;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IPlayerStateService _playerStateService;

		private PlayersStatesSynchronizer _playerStateModel;

		private readonly List<Vector3> _emptyAvoidPositions = new List<Vector3>();

		private readonly List<Vector3> _coinSourceAvoidPositions = new List<Vector3>();

		private Vector3 _giftedItemOriginalLocalScale;

		private Tween _giftedItemScaleTween;

		private Vector3 _giftGrabPointOriginalLocalPosition;

		private bool _isGiftGrabPointOffsetApplied;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private IAudioService _audioService;

		private MonkeyItemClaimModel _itemClaimModel;

		private MonkeyItemClaimService _itemClaimService;

		[field: SerializeField]
		public EnemyTargetDetector TargetDetector { get; private set; }

		[field: SerializeField]
		public EnemyStatHealthController StatHealthController { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDeadProcessor SimpleEnemyDeadProcessor { get; private set; }

		[field: SerializeField]
		public EntityStatEntityMonoBase StatEntity { get; private set; }

		[field: SerializeField]
		public NavMeshAgent Agent { get; private set; }

		[field: SerializeField]
		public SimpleEnemyDamageable Damageable { get; private set; }

		[field: SerializeField]
		public MonkeyAnimationEvent AnimationEvent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public Transform LookTargetTransform { get; private set; }

		[field: SerializeField]
		public GameObject CoinObject { get; private set; }

		[field: SerializeField]
		public GameObject FakeCoinObject { get; private set; }

		[field: SerializeField]
		public GameObject GiftItemParent { get; private set; }

		[field: SerializeField]
		public MonkeyHitParticle HitParticle { get; private set; }

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		[field: Header("Sounds")]
		[field: SerializeField]
		public EventReference StartAttackReference { get; private set; }

		[field: SerializeField]
		public EventReference HitAttackReference { get; private set; }

		[field: SerializeField]
		public EventReference TakeCoinReference { get; private set; }

		[field: SerializeField]
		public EventReference MoveAngryReference { get; private set; }

		[field: SerializeField]
		public EventReference MoveFriendlyReference { get; private set; }

		[field: SerializeField]
		public EventReference CoinRequestReference { get; private set; }

		[field: SerializeField]
		public EventReference StepSoundReference { get; private set; }

		public PlayerRef TargetPlayer { get; set; } = PlayerRef.None;

		public PlayerRef CoinSourcePlayer { get; private set; } = PlayerRef.None;

		public NetworkBehaviour PendingGiftItemPrefab { get; private set; }

		public bool PendingGiftCanActivateItem { get; private set; }

		public float PendingGiftActivateItemChance { get; private set; }

		public float PendingGiftActivateItemDelay { get; private set; }

		public Vector3 PendingGiftSpawnOffset { get; private set; }

		public bool GiftCanActivateItem { get; private set; }

		public float GiftActivateItemChance { get; private set; }

		public float GiftActivateItemDelay { get; private set; }

		public float GiftItemSpawnElapsed { get; private set; }

		public bool GiftItemActivationAttempted { get; private set; }

		public IItem GiftedItem { get; private set; }

		public Vector3 HideoutCenterPosition { get; set; }

		public IItem TargetItem { get; set; }

		public MonkeyPlayerInteractionStateId NextPlayerInteractionState { get; set; } = MonkeyPlayerInteractionStateId.RunAround;

		public Vector3 FakeAttackTargetPosition { get; set; }

		public float StateElapsed { get; set; }

		public float StateDuration { get; set; }

		public float MovementUpdateElapsed { get; set; }

		public float MovementUpdateInterval { get; set; }

		public float ChaseElapsed { get; set; }

		public float ChaseCooldownRemaining { get; set; }

		public float MoveToItemElapsed { get; set; }

		public float HideoutFleeElapsed { get; set; }

		public float FearDestinationElapsed { get; set; }

		public bool EatingCoinConsumed { get; set; }

		public bool EatingInteractionCompleted { get; set; }

		public bool IsDead { get; set; }

		public bool IsDamageAggro { get; set; }

		public bool IsFearing { get; set; }

		public bool IsDespawnAfterFear { get; set; } = true;

		public bool IsMovementDisabled { get; private set; }

		public bool HasTarget => TargetPlayer != PlayerRef.None;

		public bool HasTargetItem
		{
			get
			{
				if (TargetItem != null && !TargetItem.IsDespawned && !TargetItem.IsConsumed)
				{
					return HasTargetItemClaim();
				}
				return false;
			}
		}

		public PlayersGatesModelSynchronizedModel PlayersGatesModel => _playersGatesModel;

		public CollectItemFellEvent CollectItemFellEvent => _collectItemFellEvent;

		public PlayersStatesSynchronizer PlayerStateModel => _playerStateModel;

		public AttractionZoneData PendingAttractionZone { get; private set; }

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, IPlayerStateService playerStateService, PlayersStatesSynchronizer playerStateModel, CollectItemFellEvent collectItemFellEvent, PlayerDamageablesTrackModel playerDamageablesTrackModel, INavigationService navigationService, IEnemyAttractionZoneService attractionZoneService, MultiplayerModel multiplayerModel, PlayersGatesModelSynchronizedModel playersGatesModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService, IAudioService audioService, MonkeyItemClaimModel itemClaimModel, MonkeyItemClaimService itemClaimService)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerStateService = playerStateService;
			_playerStateModel = playerStateModel;
			_collectItemFellEvent = collectItemFellEvent;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_navigationService = navigationService;
			_attractionZoneService = attractionZoneService;
			_multiplayerModel = multiplayerModel;
			_playersGatesModel = playersGatesModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
			_audioService = audioService;
			_itemClaimModel = itemClaimModel;
			_itemClaimService = itemClaimService;
		}

		private void OnDestroy()
		{
			KillGiftedItemScaleTween();
		}

		public bool TrySetTargetItem(IItem item)
		{
			if (!_itemClaimService.TryClaim(_itemClaimModel, TargetItem, item, _monkeyEnemy, base.gameObject.GetHashCode()))
			{
				return false;
			}
			TargetItem = item;
			return true;
		}

		public void ClearTargetItem()
		{
			_itemClaimService.Release(_itemClaimModel, _monkeyEnemy, base.gameObject.GetHashCode());
			TargetItem = null;
		}

		public void ForgetConsumedTargetItem()
		{
			_itemClaimService.ForgetConsumed(_itemClaimModel, TargetItem);
			TargetItem = null;
		}

		public bool HasTargetItemClaim()
		{
			return _itemClaimService.IsClaimOwner(_itemClaimModel, _monkeyEnemy, base.gameObject.GetHashCode());
		}

		public void ResetEatingInteractionState()
		{
			EatingCoinConsumed = false;
			EatingInteractionCompleted = false;
		}

		public bool CanEnemyInteractWithTargetPlayer()
		{
			if (HasTarget)
			{
				return _enemyPlayerAttackabilityService.CanEnemyAttackPlayer(TargetPlayer.PlayerId);
			}
			return false;
		}

		public void ValidateRequiredReferences()
		{
			AssertAssigned(TargetDetector, "TargetDetector");
			AssertAssigned(StatHealthController, "StatHealthController");
			AssertAssigned(SimpleEnemyDeadProcessor, "SimpleEnemyDeadProcessor");
			AssertAssigned(StatEntity, "StatEntity");
			AssertAssigned(Agent, "Agent");
			AssertAssigned(Damageable, "Damageable");
			AssertAssigned(AnimationEvent, "AnimationEvent");
			AssertAssigned(Animator, "Animator");
			AssertAssigned(LookTargetTransform, "LookTargetTransform");
			AssertAssigned(CoinObject, "CoinObject");
			AssertAssigned(FakeCoinObject, "FakeCoinObject");
			AssertAssigned(GiftItemParent, "GiftItemParent");
			AssertAssigned(HitParticle, "HitParticle");
			AssertAssigned(_monkeyEnemy, "_monkeyEnemy");
		}

		public void CreateFmodInstances()
		{
			AttackSoundInstance = _audioService.CreateInstance(StartAttackReference);
			MoveAngryInstance = _audioService.CreateInstance(MoveAngryReference);
			MoveFriendlyInstance = _audioService.CreateInstance(MoveFriendlyReference);
			CoinRequestSoundInstance = _audioService.CreateInstance(CoinRequestReference);
			TakeCoinSoundInstance = _audioService.CreateInstance(TakeCoinReference);
			HitSoundInstance = _audioService.CreateInstance(HitAttackReference);
		}

		public void ReleaseFmodInstances()
		{
			ReleaseInstance(ref AttackSoundInstance);
			ReleaseInstance(ref MoveAngryInstance);
			ReleaseInstance(ref MoveFriendlyInstance);
			ReleaseInstance(ref CoinRequestSoundInstance);
			ReleaseInstance(ref TakeCoinSoundInstance);
			ReleaseInstance(ref HitSoundInstance);
		}

		public void UpdateFmod3DAttributes()
		{
			ATTRIBUTES_3D attributes = SoundSourceBehaviour.SoundSourceTransform.To3DAttributes();
			AttackSoundInstance.set3DAttributes(attributes);
			MoveAngryInstance.set3DAttributes(attributes);
			MoveFriendlyInstance.set3DAttributes(attributes);
			CoinRequestSoundInstance.set3DAttributes(attributes);
			TakeCoinSoundInstance.set3DAttributes(attributes);
			HitSoundInstance.set3DAttributes(attributes);
		}

		public void EnableMovement()
		{
			IsMovementDisabled = false;
		}

		public void DisableMovement()
		{
			StopAgent();
			IsMovementDisabled = true;
		}

		public void StopAgent()
		{
			if (Agent.enabled && Agent.isOnNavMesh)
			{
				Agent.ResetPath();
			}
		}

		public void MoveToPosition(Vector3 position)
		{
			if (!IsMovementDisabled && Agent.enabled && Agent.isOnNavMesh)
			{
				Agent.SetDestination(position);
			}
		}

		public void FacePosition(Vector3 position, float rotationSpeed, float deltaTime)
		{
			Vector3 vector = position - base.transform.position;
			vector.y = 0f;
			if (!(vector.sqrMagnitude <= 0.01f))
			{
				Quaternion b = Quaternion.LookRotation(vector.normalized);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, rotationSpeed * deltaTime);
			}
		}

		public void SnapFacePosition(Vector3 position)
		{
			Vector3 vector = position - base.transform.position;
			vector.y = 0f;
			if (!(vector.sqrMagnitude <= 0.01f))
			{
				base.transform.rotation = Quaternion.LookRotation(vector.normalized);
			}
		}

		public void ClearCoinSourcePlayer()
		{
			CoinSourcePlayer = PlayerRef.None;
		}

		public void SetPendingGiftItem(NetworkBehaviour giftItemPrefab, bool canActivateItem, float activateItemChance, float activateItemDelay, Vector3 giftSpawnOffset)
		{
			PendingGiftItemPrefab = giftItemPrefab;
			PendingGiftCanActivateItem = canActivateItem;
			PendingGiftActivateItemChance = Mathf.Clamp01(activateItemChance);
			PendingGiftActivateItemDelay = Mathf.Max(0f, activateItemDelay);
			PendingGiftSpawnOffset = giftSpawnOffset;
		}

		public void ClearPendingGiftItem()
		{
			PendingGiftItemPrefab = null;
			PendingGiftCanActivateItem = false;
			PendingGiftActivateItemChance = 0f;
			PendingGiftActivateItemDelay = 0f;
			PendingGiftSpawnOffset = Vector3.zero;
		}

		public void ApplyGiftGrabPointOffset(Vector3 localOffset)
		{
			if (!(GiftItemParent == null))
			{
				Transform transform = GiftItemParent.transform;
				if (!_isGiftGrabPointOffsetApplied)
				{
					_giftGrabPointOriginalLocalPosition = transform.localPosition;
					_isGiftGrabPointOffsetApplied = true;
				}
				transform.localPosition = _giftGrabPointOriginalLocalPosition + localOffset;
			}
		}

		public void ResetGiftGrabPointOffset()
		{
			if (_isGiftGrabPointOffsetApplied && !(GiftItemParent == null))
			{
				GiftItemParent.transform.localPosition = _giftGrabPointOriginalLocalPosition;
				_isGiftGrabPointOffsetApplied = false;
			}
		}

		public void BeginGiftItemActivationTracking(bool canActivateItem, float activateItemChance, float activateItemDelay)
		{
			GiftCanActivateItem = canActivateItem;
			GiftActivateItemChance = Mathf.Clamp01(activateItemChance);
			GiftActivateItemDelay = Mathf.Max(0f, activateItemDelay);
			GiftItemSpawnElapsed = 0f;
			GiftItemActivationAttempted = false;
		}

		public void AdvanceGiftItemSpawnElapsed(float deltaTime)
		{
			if (GiftedItem != null && !GiftedItem.IsDespawned)
			{
				GiftItemSpawnElapsed += deltaTime;
			}
		}

		public void MarkGiftItemActivationAttempted()
		{
			GiftItemActivationAttempted = true;
		}

		public void ResetGiftItemActivationTracking()
		{
			GiftCanActivateItem = false;
			GiftActivateItemChance = 0f;
			GiftActivateItemDelay = 0f;
			GiftItemSpawnElapsed = 0f;
			GiftItemActivationAttempted = false;
		}

		public void SetGiftedItem(IItem item, float scaleDuration, Ease scaleEase)
		{
			GiftedItem = item;
			if (GiftedItem is NetworkBehaviour networkBehaviour)
			{
				_giftedItemOriginalLocalScale = networkBehaviour.transform.localScale;
				StartGiftedItemScaleTween(networkBehaviour.transform, scaleDuration, scaleEase);
			}
		}

		private void StartGiftedItemScaleTween(Transform itemTransform, float scaleDuration, Ease scaleEase)
		{
			KillGiftedItemScaleTween();
			itemTransform.localScale = Vector3.zero;
			_giftedItemScaleTween = itemTransform.DOScale(_giftedItemOriginalLocalScale, scaleDuration).SetEase(scaleEase).SetLink(itemTransform.gameObject);
		}

		public void SetPendingAttractionZone(AttractionZoneData zone)
		{
			PendingAttractionZone = zone;
		}

		public bool TryGetAttractionApproachPoint(int attempts, out Vector3 point)
		{
			return _attractionZoneService.TrySampleApproachPoint(_navigationService, PendingAttractionZone.Origin, PendingAttractionZone.ApproachRadius, attempts, out point);
		}

		public void RetractGiftedItem(float scaleDuration, Ease scaleEase)
		{
			if (GiftedItem is NetworkBehaviour networkBehaviour)
			{
				if (GiftedItem is MonoItem monoItem)
				{
					monoItem.SetDespawnEffectsSuppressed(suppressed: true);
				}
				KillGiftedItemScaleTween();
				_giftedItemScaleTween = networkBehaviour.transform.DOScale(Vector3.zero, scaleDuration).SetEase(scaleEase).SetLink(networkBehaviour.gameObject);
			}
		}

		public void DestroyGiftedItemOffering()
		{
			KillGiftedItemScaleTween();
			if (GiftedItem != null && !GiftedItem.IsDespawned && GiftedItem is MonoItem monoItem)
			{
				monoItem.DespawnItem();
			}
			_monkeyEnemy.ReleaseGiftedItemGrab();
			DetachGiftedItem();
		}

		public void DetachGiftedItem()
		{
			GiftedItem = null;
			ResetGiftItemActivationTracking();
			ResetGiftGrabPointOffset();
		}

		public void ClearGiftedItem()
		{
			ReleaseGiftedItemOffering();
			DetachGiftedItem();
		}

		public bool IsGiftedItemHeldByCoinSourcePlayer()
		{
			if (GiftedItem == null || GiftedItem.IsDespawned || GiftedItem.IsConsumed)
			{
				return false;
			}
			if (!(GiftedItem is MonoItem monoItem) || !monoItem.TryGetComponent<IPointGrabable>(out var component))
			{
				return false;
			}
			if (component.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			foreach (int grabbedByPlayer in component.GrabbedByPlayers)
			{
				if (TryResolvePlayerRef(grabbedByPlayer, out var playerRef) && playerRef == CoinSourcePlayer)
				{
					return true;
				}
			}
			return false;
		}

		public void ReleaseGiftedItemOffering()
		{
			KillGiftedItemScaleTween();
			if (GiftedItem != null && !GiftedItem.IsDespawned && GiftedItem is NetworkBehaviour networkBehaviour)
			{
				networkBehaviour.transform.localScale = _giftedItemOriginalLocalScale;
				_monkeyEnemy.ReleaseGiftedItemGrab();
			}
		}

		public bool TryCaptureCoinSourcePlayerFromTargetItem()
		{
			if (!(TargetItem is MonoItem monoItem) || !monoItem.TryGetComponent<IPointGrabable>(out var component))
			{
				return CoinSourcePlayer != PlayerRef.None;
			}
			if (component.GrabbedByPlayers.Count == 0)
			{
				return CoinSourcePlayer != PlayerRef.None;
			}
			if (!TryResolvePlayerRef(component.GrabbedByPlayers[0], out var playerRef))
			{
				return CoinSourcePlayer != PlayerRef.None;
			}
			CoinSourcePlayer = playerRef;
			return true;
		}

		public bool TryGetPlayerWorldPosition(PlayerRef player, out Vector3 position)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(player, out position))
			{
				return true;
			}
			if (_spawnedPlayersModel.Players.TryGetValue(player, out var value) && value.NetworkObject != null)
			{
				position = value.NetworkObject.transform.position;
				return true;
			}
			position = default(Vector3);
			return false;
		}

		public float GetDistanceToPlayer(PlayerRef player)
		{
			if (!TryGetPlayerWorldPosition(player, out var position))
			{
				return float.PositiveInfinity;
			}
			return Vector3.Distance(base.transform.position, position);
		}

		public bool TryGetHideoutFleePosition(MonkeyItemInteractionSettings settings, out Vector3 position)
		{
			position = base.transform.position;
			if (CoinSourcePlayer == PlayerRef.None)
			{
				return false;
			}
			if (!TryGetPlayerWorldPosition(CoinSourcePlayer, out var position2))
			{
				return false;
			}
			_coinSourceAvoidPositions.Clear();
			_coinSourceAvoidPositions.Add(position2);
			if (!_navigationService.TryGetRandomSafeNavmeshPosition(base.transform.position, settings.HideoutFleeSearchRadius, settings.HideoutFleePositionAttempts, _coinSourceAvoidPositions, Agent.agentTypeID, -1, out position, 0f, 1f, settings.HideoutFleeAverageAvoidDistanceWeight, settings.HideoutMinDistanceFromCoinSourcePlayer, settings.HideoutFleeTooClosePenaltyWeight))
			{
				return false;
			}
			Vector3 vector = position - position2;
			vector.y = 0f;
			return vector.magnitude >= settings.HideoutMinDistanceFromCoinSourcePlayer;
		}

		public float GetDistanceToTarget()
		{
			if (!HasTarget)
			{
				return float.PositiveInfinity;
			}
			if (_navigationService.TryGetPlayerTrackingPosition(TargetPlayer, out var position))
			{
				return Vector3.Distance(position, base.transform.position);
			}
			if (!TryGetTargetPlayer(out var player))
			{
				return float.PositiveInfinity;
			}
			return Vector3.Distance(player.NetworkObject.transform.position, base.transform.position);
		}

		public Vector3 GetTargetPosition()
		{
			if (HasTarget && _navigationService.TryGetPlayerTrackingPosition(TargetPlayer, out var position))
			{
				return position;
			}
			if (!TryGetTargetPlayer(out var player))
			{
				return base.transform.position;
			}
			return player.NetworkObject.transform.position;
		}

		public bool IsTargetAlive()
		{
			if (HasTarget)
			{
				return _playerStateService.IsPlayerAlive(TargetPlayer.PlayerId);
			}
			return false;
		}

		public bool IsPlayerEligibleForTarget(PlayerRef player)
		{
			if (ChaseCooldownRemaining > 0f)
			{
				return false;
			}
			if (!_playerStateService.IsPlayerAlive(player.PlayerId))
			{
				return false;
			}
			PlayerReachableData reachableData;
			return _navigationService.IsPlayerOnReachablePoint(player, Agent, out reachableData);
		}

		public bool TryGetNearestAlivePlayer(out PlayerRef nearestPlayer)
		{
			nearestPlayer = PlayerRef.None;
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			float num = float.PositiveInfinity;
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (_playerStateService.IsPlayerAlive(activePlayer.PlayerId) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(activePlayer.PlayerId) && TryGetPlayerWorldPosition(activePlayer, out var position))
				{
					float num2 = Vector3.Distance(base.transform.position, position);
					if (!(num2 >= num))
					{
						num = num2;
						nearestPlayer = activePlayer;
					}
				}
			}
			return nearestPlayer != PlayerRef.None;
		}

		public bool TryGetPlayerDamageable(out IDamageable damageable)
		{
			damageable = null;
			if (HasTarget)
			{
				return _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(TargetPlayer.PlayerId, out damageable);
			}
			return false;
		}

		public float GetStatValue(EntityStatType statType, float fallbackValue = 0f)
		{
			return StatEntity.GetStat(statType)?.Value ?? fallbackValue;
		}

		public Vector3 GetRandomNavmeshPosition(Vector3 center, float radius)
		{
			Vector2 vector = Random.insideUnitCircle * radius;
			if (!NavMesh.SamplePosition(center + new Vector3(vector.x, 0f, vector.y), out var hit, radius, -1))
			{
				return base.transform.position;
			}
			return hit.position;
		}

		public Vector3 GetRandomSafeNavmeshPosition(float radius, int attempts, float minDistanceFromCenter, float centerDistanceWeight, float averageAvoidDistanceWeight, float tooClosePenaltyRadius)
		{
			if (!_navigationService.TryGetRandomSafeNavmeshPosition(base.transform.position, radius, attempts, _emptyAvoidPositions, Agent.agentTypeID, -1, out var bestPosition, minDistanceFromCenter, centerDistanceWeight, averageAvoidDistanceWeight, tooClosePenaltyRadius))
			{
				return base.transform.position;
			}
			return bestPosition;
		}

		public Vector3 GetItemPosition(IItem item)
		{
			if (item is MonoItem monoItem)
			{
				return monoItem.transform.position;
			}
			if (!(item is NetworkBehaviour networkBehaviour))
			{
				return Vector3.zero;
			}
			return networkBehaviour.transform.position;
		}

		public bool IsTargetItemHeld()
		{
			if (TargetItem is MonoItem monoItem && monoItem.TryGetComponent<IPointGrabable>(out var component))
			{
				return component.GrabbedByPlayers.Count > 0;
			}
			return false;
		}

		public bool TryConsumeTargetItem(float maxDistance, float hungerReductionMultiplier, out float hungerReduction)
		{
			hungerReduction = 0f;
			if (!HasTargetItem)
			{
				return false;
			}
			Vector3 itemPosition = GetItemPosition(TargetItem);
			if (itemPosition == Vector3.zero)
			{
				return false;
			}
			if (Vector3.Distance(base.transform.position, itemPosition) > maxDistance)
			{
				return false;
			}
			hungerReduction = (float)(int)TargetItem.CurrencyValue * hungerReductionMultiplier;
			TargetItem.Consume();
			ForgetConsumedTargetItem();
			return true;
		}

		public bool TryTakeTargetCoinFromHand(float maxDistance, float hungerReductionMultiplier, out float hungerReduction)
		{
			hungerReduction = 0f;
			if (!(TargetItem is MonoItem monoItem) || !monoItem.TryGetComponent<IPointGrabable>(out var component))
			{
				return false;
			}
			if (component.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			Vector3 itemPosition = GetItemPosition(monoItem);
			if (Vector3.Distance(base.transform.position, itemPosition) > maxDistance)
			{
				return false;
			}
			hungerReduction = (float)(int)monoItem.CurrencyValue * hungerReductionMultiplier;
			monoItem.Consume();
			ForgetConsumedTargetItem();
			return true;
		}

		public bool IsEnemyVisibleByPlayers(float detectionDistance)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (!_spawnedPlayersModel.Players.TryGetValue(activePlayer, out var value))
				{
					continue;
				}
				NetworkObject networkObject = value.NetworkObject;
				if (!(networkObject == null) && networkObject.TryGetComponent<PlayerLookDetection>(out var component))
				{
					if (component.IsLookingAtObject(LookTargetTransform, angleCullEnabled: false, detectionDistance / 2f))
					{
						return true;
					}
					if (_navigationService.IsPlayerOnReachablePoint(activePlayer, Agent, detectionDistance, out var reachableData))
					{
						return true;
					}
					if (Vector3.Distance(base.transform.position, reachableData.NavMeshProjectedHit.position) < detectionDistance / 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool TryResolvePlayerRef(int playerId, out PlayerRef playerRef)
		{
			playerRef = PlayerRef.None;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerId)
				{
					playerRef = player.Key;
					return true;
				}
			}
			return false;
		}

		private bool TryGetTargetPlayer(out PlayerDataHolder player)
		{
			player = null;
			if (HasTarget)
			{
				return _spawnedPlayersModel.Players.TryGetValue(TargetPlayer, out player);
			}
			return false;
		}

		private void ReleaseInstance(ref EventInstance instance)
		{
			_audioService.StopInstance(instance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(instance);
		}

		private void KillGiftedItemScaleTween()
		{
			if (_giftedItemScaleTween != null)
			{
				_giftedItemScaleTween.Kill();
				_giftedItemScaleTween = null;
			}
		}

		private void AssertAssigned(Object reference, string fieldName)
		{
			if (reference != null)
			{
				return;
			}
			throw new MissingReferenceException("MonkeyEnemyContext on '" + base.name + "' requires '" + fieldName + "' to be assigned.");
		}
	}
}
