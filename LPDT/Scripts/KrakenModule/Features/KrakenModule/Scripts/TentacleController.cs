using System;
using System.Collections;
using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.ItemCollisionModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SwimmingModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class TentacleController : NetworkBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private Transform _catchBone;

		[SerializeField]
		private PhysGrabber _physGrabber;

		[SerializeField]
		private float _catchMoveDuration = 0.15f;

		[SerializeField]
		private float _catchArriveDistance = 0.12f;

		[SerializeField]
		private float _catchMaxCatchTime = 1.5f;

		[SerializeField]
		private float _throwDuration = 0.6f;

		[SerializeField]
		private float _throwHorizontalSpeed = 8f;

		[SerializeField]
		private float _throwAnimationFallbackTime = 1.25f;

		[SerializeField]
		private float _throwFallbackDistance = 5f;

		[SerializeField]
		private float _throwTargetHorizontalOffsetMax = 0.4f;

		[SerializeField]
		private float _throwTargetVerticalOffsetMax = 0.35f;

		[SerializeField]
		private float _directThrowbackChance = 0.1f;

		[SerializeField]
		private float _nearThrowHorizontalOffset = 2f;

		[SerializeField]
		private float _throwbackDamage = 15f;

		[SerializeField]
		private float _throwbackDamageTrackingTime = 3f;

		[SerializeField]
		private float _clientTargetInterpolationSpeed = 45f;

		[SerializeField]
		private PhysGrabber _compositeThrowGrabberPrefab;

		[SerializeField]
		private TentacleSide _tentacleSide;

		[SerializeField]
		private TentacleAnimationController _tentacleAnimationController;

		[SerializeField]
		private TentacleAnimationEvent _tentacleAnimationEvent;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private PlayerRaycastPointsModel _playerRaycastPointsModel;

		private ItemCollisionModel _itemCollisionModel;

		private PlayerStatesConfiguration _playerStatesConfiguration;

		private IKrakenItemThrowService _krakenItemThrowService;

		private IPointGrabable _assignedItem;

		private Coroutine _catchCoroutine;

		private bool _isItemCaught;

		private bool _wasKinematic;

		private bool _wasUseGravity;

		private bool _isAimingAtPlayer;

		private bool _hasThrown;

		private bool _isItemCollisionIgnored;

		private PlayerRef _throwTargetPlayer;

		private bool _isHelpThrowToAlive;

		private bool _isDirectDamagingThrowback;

		private Vector3 _helpThrowTargetPosition;

		private int _collisionIgnoreOwnerId;

		private Coroutine _throwCoroutine;

		private Coroutine _throwFallbackCoroutine;

		private GrabObjectBase _physGrabbedObject;

		private Rigidbody _physGrabbedRigidbody;

		private PhysGrabber _spawnedThrowGrabber;

		private readonly List<GrabObjectBase> _compositeGrabbedObjects = new List<GrabObjectBase>();

		private readonly List<PlayerDamageable> _rockHitDamageTargets = new List<PlayerDamageable>();

		private readonly HashSet<NetworkId> _rockHitPlayerNetworkIds = new HashSet<NetworkId>();

		private static readonly Collider[] RockHitOverlapBuffer = new Collider[32];

		private NetworkObject _throwbackDamageItem;

		private PlayerRef _throwbackDamageTargetPlayer;

		private float _throwbackDamageExpiresAt;

		private bool _isAggressiveRockThrow;

		private float _aggressiveRockDamage;

		private float _aggressiveRockKnockbackForce;

		private float _aggressiveRockHitRadius;

		private bool _isSpawned;

		[WeaverGenerated]
		[DefaultForProperty("AssignedItemNetworkId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _AssignedItemNetworkId;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkId AssignedItemNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TentacleController.AssignedItemNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing TentacleController.AssignedItemNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		public TentacleSide TentacleSide => _tentacleSide;

		public Transform Target => _target;

		public bool IsBusy { get; private set; }

		public IPointGrabable AssignedItem => _assignedItem;

		private bool IsNearThrowback
		{
			get
			{
				if (!_isHelpThrowToAlive && !_isAggressiveRockThrow)
				{
					return !_isDirectDamagingThrowback;
				}
				return false;
			}
		}

		[Inject]
		private void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, PlayerRaycastPointsModel playerRaycastPointsModel, ItemCollisionModel itemCollisionModel, PlayerStatesConfiguration playerStatesConfiguration, IKrakenItemThrowService krakenItemThrowService)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerRaycastPointsModel = playerRaycastPointsModel;
			_itemCollisionModel = itemCollisionModel;
			_playerStatesConfiguration = playerStatesConfiguration;
			_krakenItemThrowService = krakenItemThrowService;
		}

		public override void Spawned()
		{
			_isSpawned = true;
			_collisionIgnoreOwnerId = base.gameObject.GetHashCode();
			_tentacleAnimationEvent.OnCatch += HandleCatchAnimationEvent;
			_tentacleAnimationEvent.OnStartThrow += HandleStartThrowAnimationEvent;
			_tentacleAnimationEvent.OnThrow += HandleThrowAnimationEvent;
			_itemCollisionModel.OnCollisionAdded += HandleItemCollision;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			_tentacleAnimationEvent.OnCatch -= HandleCatchAnimationEvent;
			_tentacleAnimationEvent.OnStartThrow -= HandleStartThrowAnimationEvent;
			_tentacleAnimationEvent.OnThrow -= HandleThrowAnimationEvent;
			_itemCollisionModel.OnCollisionAdded -= HandleItemCollision;
			StopCatchCoroutine();
			StopThrowCoroutine();
			StopThrowFallbackCoroutine();
			UnregisterPhysGrabber(restorePhysics: false);
			UnregisterCompositeThrowGrabber(despawn: true);
			ClearThrowbackDamageTracking();
		}

		private void Update()
		{
			if (!_isSpawned)
			{
				return;
			}
			ClearExpiredThrowbackDamageTracking();
			ResolveAssignedItemFromNetwork();
			if (!IsAssignedItemValid())
			{
				if (IsBusy)
				{
					ReleaseItem(restorePhysics: true, base.HasStateAuthority);
				}
			}
			else if (KrakenThrowAssignmentService.IsWornHeadwear(_assignedItem))
			{
				ReleaseItem(restorePhysics: false, base.HasStateAuthority);
			}
			else
			{
				UpdateTargetPosition();
			}
		}

		public bool TryAssignCatch(IPointGrabable item)
		{
			return TryAssignCatch(KrakenThrowContext.ThrowBack(item));
		}

		public bool TryAssignCatchForHelpThrow(IPointGrabable item, PlayerRef aliveTarget, Vector3 targetPosition)
		{
			return TryAssignCatch(new KrakenThrowContext(KrakenThrowMode.HelpDeadPart, item, aliveTarget, targetPosition, KrakenThrowDamageProfile.None, KrakenTentacleAssignmentStrategy.NearestFree));
		}

		public bool TryAssignCatchForAggressiveRock(IPointGrabable item, PlayerRef target, float damage, float knockbackForce, float hitTrackingTime, float hitRadius)
		{
			return TryAssignCatch(new KrakenThrowContext(KrakenThrowMode.AggressiveRock, item, target, default(Vector3), new KrakenThrowDamageProfile(damage, knockbackForce, hitTrackingTime, despawnItemAfterHit: true, hitRadius), KrakenTentacleAssignmentStrategy.RandomFree));
		}

		public bool TryAssignCatch(KrakenThrowContext context)
		{
			if (IsBusy || context.Item == null)
			{
				return false;
			}
			if (context.Mode == KrakenThrowMode.AggressiveRock && (context.TargetPlayer == PlayerRef.None || !context.TargetPlayer.IsRealPlayer))
			{
				return false;
			}
			_assignedItem = context.Item;
			AssignedItemNetworkId = ((context.Item.NetworkObject != null) ? context.Item.NetworkObject.Id : default(NetworkId));
			_isHelpThrowToAlive = context.Mode == KrakenThrowMode.HelpDeadPart || context.HasExplicitTargetPosition;
			_helpThrowTargetPosition = context.TargetPosition;
			_isDirectDamagingThrowback = context.Mode == KrakenThrowMode.ThrowBack && base.HasStateAuthority && UnityEngine.Random.value < _directThrowbackChance;
			_isAggressiveRockThrow = context.Mode == KrakenThrowMode.AggressiveRock;
			_aggressiveRockDamage = context.DamageProfile.Damage;
			_aggressiveRockKnockbackForce = context.DamageProfile.KnockbackForce;
			_aggressiveRockHitRadius = context.DamageProfile.HitRadius;
			_throwbackDamageTrackingTime = ((context.DamageProfile.HitTrackingTime > 0f) ? context.DamageProfile.HitTrackingTime : _throwbackDamageTrackingTime);
			_throwTargetPlayer = ((context.TargetPlayer != PlayerRef.None) ? context.TargetPlayer : ((context.Item.NetworkObject != null) ? context.Item.NetworkObject.StateAuthority : PlayerRef.None));
			RequestAssignedItemStateAuthorityIfNeeded();
			_isAimingAtPlayer = false;
			_hasThrown = false;
			IsBusy = true;
			_isItemCaught = false;
			SetItemCollisionIgnored(ignored: true);
			UpdateTargetPosition();
			return true;
		}

		private bool TryAssignCatchInternal(IPointGrabable item, PlayerRef aliveTarget, Vector3 targetPosition, bool isHelpThrowToAlive)
		{
			if (IsBusy || item == null)
			{
				return false;
			}
			_assignedItem = item;
			AssignedItemNetworkId = ((item.NetworkObject != null) ? item.NetworkObject.Id : default(NetworkId));
			_isHelpThrowToAlive = isHelpThrowToAlive;
			_helpThrowTargetPosition = targetPosition;
			_isDirectDamagingThrowback = false;
			_isAggressiveRockThrow = false;
			_aggressiveRockDamage = 0f;
			_aggressiveRockKnockbackForce = 0f;
			_aggressiveRockHitRadius = 0f;
			_throwTargetPlayer = (isHelpThrowToAlive ? aliveTarget : ((item.NetworkObject != null) ? item.NetworkObject.StateAuthority : PlayerRef.None));
			RequestAssignedItemStateAuthorityIfNeeded();
			_isAimingAtPlayer = false;
			_hasThrown = false;
			IsBusy = true;
			_isItemCaught = false;
			SetItemCollisionIgnored(ignored: true);
			UpdateTargetPosition();
			return true;
		}

		public void UpdateTargetPosition()
		{
			if (IsAssignedItemValid() && !(_target == null))
			{
				Vector3 position = ((_catchBone != null) ? _catchBone.position : base.transform.position);
				Transform nearestHandle = _assignedItem.GetNearestHandle(position);
				Vector3 vector = ((nearestHandle != null) ? nearestHandle.position : _assignedItem.GameObject.transform.position);
				if (base.HasStateAuthority)
				{
					_target.position = vector;
					return;
				}
				float t = 1f - Mathf.Exp((0f - _clientTargetInterpolationSpeed) * Time.deltaTime);
				_target.position = Vector3.Lerp(_target.position, vector, t);
			}
		}

		private bool TryResolveThrowTargetPosition(out Vector3 position)
		{
			position = Vector3.zero;
			if (_isHelpThrowToAlive)
			{
				position = _helpThrowTargetPosition;
				return true;
			}
			if (_throwTargetPlayer == PlayerRef.None || !_throwTargetPlayer.IsRealPlayer)
			{
				return false;
			}
			if (_playerRaycastPointsModel != null && _playerRaycastPointsModel.TryGetRaycastPoint(_throwTargetPlayer, PlayerRaycastPoint.MiddleBelt, out var point) && point != null)
			{
				position = point.position;
				return true;
			}
			if (TryGetPlayerHolder(_throwTargetPlayer, out var holder) && holder?.NetworkObject != null)
			{
				position = holder.NetworkObject.transform.position;
				return true;
			}
			return false;
		}

		private Vector3 ResolveThrowTargetPosition()
		{
			if (TryResolveThrowTargetPosition(out var position))
			{
				if (_isHelpThrowToAlive)
				{
					return position;
				}
				if (_isDirectDamagingThrowback || _isAggressiveRockThrow)
				{
					return ApplyThrowTargetRandomOffset(position);
				}
				return ApplyNearThrowOffset(position);
			}
			Vector3 basePosition = ((_catchBone != null) ? (_catchBone.position + _catchBone.forward * _throwFallbackDistance) : (base.transform.position + base.transform.forward * _throwFallbackDistance));
			return ApplyThrowTargetRandomOffset(basePosition);
		}

		private Vector3 ApplyThrowTargetRandomOffset(Vector3 basePosition)
		{
			if (_throwTargetHorizontalOffsetMax <= 0f && _throwTargetVerticalOffsetMax <= 0f)
			{
				return basePosition;
			}
			Vector3 vector = ((_catchBone != null) ? _catchBone.position : base.transform.position);
			Vector3 vector2 = basePosition - vector;
			vector2.y = 0f;
			Vector3 vector3 = ((vector2.sqrMagnitude > 0.0001f) ? Vector3.Cross(Vector3.up, vector2.normalized) : base.transform.right);
			if (_throwTargetHorizontalOffsetMax > 0f && (_throwTargetVerticalOffsetMax <= 0f || UnityEngine.Random.value < 0.5f))
			{
				return basePosition + vector3 * UnityEngine.Random.Range(0f - _throwTargetHorizontalOffsetMax, _throwTargetHorizontalOffsetMax);
			}
			if (_throwTargetVerticalOffsetMax > 0f)
			{
				return basePosition + Vector3.up * UnityEngine.Random.Range(0f, _throwTargetVerticalOffsetMax);
			}
			return basePosition;
		}

		private Vector3 ApplyNearThrowOffset(Vector3 basePosition)
		{
			if (_nearThrowHorizontalOffset <= 0f)
			{
				return basePosition;
			}
			Vector3 vector = ((_catchBone != null) ? _catchBone.position : base.transform.position);
			Vector3 vector2 = basePosition - vector;
			vector2.y = 0f;
			Vector3 vector3 = ((vector2.sqrMagnitude > 0.0001f) ? Vector3.Cross(Vector3.up, vector2.normalized) : base.transform.right);
			float num = ((UnityEngine.Random.value < 0.5f) ? (-1f) : 1f);
			return basePosition + vector3 * (_nearThrowHorizontalOffset * num);
		}

		public void PlayCatchAnimation(bool isHeavyItem)
		{
			_tentacleAnimationController.PlayCatchAnimation(isHeavyItem);
		}

		private void HandleCatchAnimationEvent()
		{
			if (base.HasStateAuthority)
			{
				CatchItem();
			}
		}

		private void HandleStartThrowAnimationEvent()
		{
			if (base.HasStateAuthority)
			{
				_isAimingAtPlayer = true;
				StartThrowFallbackCoroutine();
			}
		}

		private void HandleThrowAnimationEvent()
		{
			if (base.HasStateAuthority)
			{
				TryStartThrow();
			}
		}

		private bool TryStartThrow()
		{
			if (_hasThrown || _throwCoroutine != null)
			{
				return false;
			}
			if (!IsAssignedItemValid())
			{
				ReleaseItem();
				return false;
			}
			Rigidbody rigidbody = _assignedItem.Rigidbody;
			if (rigidbody == null)
			{
				ReleaseItem();
				return false;
			}
			RequestAssignedItemStateAuthorityIfNeeded();
			StopThrowFallbackCoroutine();
			_throwCoroutine = StartCoroutine(ThrowItemCoroutine(rigidbody));
			return true;
		}

		private void StartThrowFallbackCoroutine()
		{
			if (_throwFallbackCoroutine == null && _throwCoroutine == null && !_hasThrown && IsAssignedItemValid())
			{
				_throwFallbackCoroutine = StartCoroutine(ThrowAnimationFallbackCoroutine());
			}
		}

		private IEnumerator ThrowAnimationFallbackCoroutine()
		{
			yield return new WaitForSeconds(Mathf.Max(0.01f, _throwAnimationFallbackTime));
			_throwFallbackCoroutine = null;
			if (base.HasStateAuthority && _isAimingAtPlayer && !_hasThrown && _throwCoroutine == null)
			{
				TryStartThrow();
			}
		}

		private IEnumerator ThrowItemCoroutine(Rigidbody rigidbody)
		{
			bool releaseItemAfterCoroutine = true;
			try
			{
				ResetConnectedGrabableVelocities();
				yield return new WaitForFixedUpdate();
				if (!IsAssignedItemValid())
				{
					yield break;
				}
				Vector3 targetPosition = ResolveThrowTargetPosition();
				UnregisterPhysGrabber(restorePhysics: false);
				SetItemCollisionIgnored(ignored: false);
				if (HasConnectedGrabables())
				{
					Vector3 vector = ((_catchBone != null) ? _catchBone.position : rigidbody.position);
					if (TrySpawnCompositeThrowGrabber(vector) && RegisterCompositeGrabablesOnThrowGrabber())
					{
						yield return MoveThrowGrabberAlongArcCoroutine(vector, targetPosition);
					}
					else
					{
						ThrowRigidbody(rigidbody, targetPosition);
					}
					EnableGravityOnConnectedGrabables();
					UnregisterCompositeThrowGrabber(despawn: true);
				}
				else
				{
					ThrowRigidbody(rigidbody, targetPosition);
				}
				TrackThrowbackDamage();
				_hasThrown = true;
				releaseItemAfterCoroutine = !IsThrowbackDamageTrackingActive();
			}
			finally
			{
				TentacleController tentacleController = this;
				tentacleController._throwCoroutine = null;
				tentacleController.UnregisterCompositeThrowGrabber(despawn: true);
				if (releaseItemAfterCoroutine)
				{
					tentacleController.ReleaseItem(restorePhysics: false);
				}
			}
		}

		public void CatchItem()
		{
			if (IsAssignedItemValid() && !_isItemCaught && _catchCoroutine == null)
			{
				StopCatchCoroutine();
				_catchCoroutine = StartCoroutine(MoveItemToCatchBoneCoroutine());
			}
		}

		private IEnumerator MoveItemToCatchBoneCoroutine()
		{
			if (_assignedItem == null || _catchBone == null)
			{
				ReleaseItem();
				_catchCoroutine = null;
				yield break;
			}
			RequestAssignedItemStateAuthorityIfNeeded();
			Rigidbody rigidbody = _assignedItem.Rigidbody;
			if (rigidbody == null)
			{
				ReleaseItem();
				_catchCoroutine = null;
				yield break;
			}
			PrepareItemForPhysGrabber(rigidbody);
			if (!RegisterPhysGrabber())
			{
				ReleaseItem();
				_catchCoroutine = null;
				yield break;
			}
			float elapsed = 0f;
			float maxCatchTime = Mathf.Max(_catchMaxCatchTime, _catchMoveDuration);
			while (elapsed < maxCatchTime)
			{
				if (!IsAssignedItemValid() || _catchBone == null)
				{
					ReleaseItem();
					_catchCoroutine = null;
					yield break;
				}
				if (Vector3.Distance(rigidbody.position, _catchBone.position) <= _catchArriveDistance)
				{
					break;
				}
				elapsed += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			rigidbody.linearVelocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			_isItemCaught = true;
			_catchCoroutine = null;
		}

		private void PrepareItemForPhysGrabber(Rigidbody rigidbody)
		{
			_wasKinematic = rigidbody.isKinematic;
			_wasUseGravity = rigidbody.useGravity;
			rigidbody.isKinematic = false;
			rigidbody.useGravity = false;
			rigidbody.linearVelocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.WakeUp();
			_physGrabbedRigidbody = rigidbody;
		}

		private bool RegisterPhysGrabber()
		{
			if (_physGrabber == null || !IsAssignedItemValid() || _assignedItem.GrabObject == null || _catchBone == null)
			{
				return false;
			}
			GrabObjectBase grabObject = _assignedItem.GrabObject;
			Transform nearestHandle = _assignedItem.GetNearestHandle(_catchBone.position);
			if (_physGrabber.physGrabPointPullerPosition == null)
			{
				_physGrabber.physGrabPointPullerPosition = _catchBone;
			}
			_physGrabber.IsProcessPhysGrabbing = true;
			if (!_physGrabber.physGrabPoints.TryAdd(grabObject, nearestHandle))
			{
				_physGrabber.physGrabPoints[grabObject] = nearestHandle;
			}
			if (!grabObject.Grabbers.Contains(_physGrabber))
			{
				grabObject.Grabbers.Add(_physGrabber);
			}
			_physGrabbedObject = grabObject;
			return true;
		}

		private void UnregisterPhysGrabber(bool restorePhysics)
		{
			if (_physGrabbedObject != null && _physGrabber != null)
			{
				_physGrabbedObject.Grabbers.Remove(_physGrabber);
				_physGrabber.physGrabPoints.Remove(_physGrabbedObject);
				_physGrabber.IsProcessPhysGrabbing = _physGrabber.physGrabPoints.Count > 0;
			}
			if (restorePhysics && _physGrabbedRigidbody != null)
			{
				_physGrabbedRigidbody.linearVelocity = Vector3.zero;
				_physGrabbedRigidbody.angularVelocity = Vector3.zero;
				_physGrabbedRigidbody.useGravity = _wasUseGravity;
				_physGrabbedRigidbody.isKinematic = _wasKinematic;
			}
			_physGrabbedObject = null;
			_physGrabbedRigidbody = null;
		}

		private void ResetConnectedGrabableVelocities()
		{
			HashSet<Rigidbody> rigidbodies = new HashSet<Rigidbody>();
			TryAdd(_assignedItem);
			if (_assignedItem is SimplePointGrabable simplePointGrabable)
			{
				foreach (SimplePointGrabable connectedGrabable in simplePointGrabable.ConnectedGrabables)
				{
					TryAdd(connectedGrabable);
				}
			}
			foreach (Rigidbody item in rigidbodies)
			{
				item.linearVelocity = Vector3.zero;
				item.angularVelocity = Vector3.zero;
			}
			void TryAdd(IPointGrabable grabable)
			{
				if (!(grabable?.Rigidbody == null))
				{
					rigidbodies.Add(grabable.Rigidbody);
				}
			}
		}

		private bool HasConnectedGrabables()
		{
			if (_assignedItem is SimplePointGrabable simplePointGrabable)
			{
				return simplePointGrabable.ConnectedGrabables.Count > 0;
			}
			return false;
		}

		private bool TryGetConnectedGrabables(out List<SimplePointGrabable> grabables)
		{
			grabables = new List<SimplePointGrabable>();
			if (!IsAssignedItemValid() || !(_assignedItem is SimplePointGrabable simplePointGrabable))
			{
				return false;
			}
			HashSet<SimplePointGrabable> hashSet = new HashSet<SimplePointGrabable> { simplePointGrabable };
			grabables.Add(simplePointGrabable);
			foreach (SimplePointGrabable connectedGrabable in simplePointGrabable.ConnectedGrabables)
			{
				if (!(connectedGrabable == null) && hashSet.Add(connectedGrabable))
				{
					grabables.Add(connectedGrabable);
				}
			}
			return grabables.Count > 0;
		}

		private bool TrySpawnCompositeThrowGrabber(Vector3 spawnPosition)
		{
			if (_compositeThrowGrabberPrefab == null || base.Runner == null || !base.Runner.IsRunning)
			{
				return false;
			}
			UnregisterCompositeThrowGrabber(despawn: true);
			if (base.Runner.TrySpawn(_compositeThrowGrabberPrefab, out _spawnedThrowGrabber, spawnPosition) == NetworkSpawnStatus.Spawned)
			{
				return _spawnedThrowGrabber != null;
			}
			return false;
		}

		private bool RegisterCompositeGrabablesOnThrowGrabber()
		{
			if (_spawnedThrowGrabber == null || !TryGetConnectedGrabables(out var grabables))
			{
				return false;
			}
			_compositeGrabbedObjects.Clear();
			_spawnedThrowGrabber.IsProcessPhysGrabbing = true;
			Vector3 position = _spawnedThrowGrabber.transform.position;
			foreach (SimplePointGrabable item in grabables)
			{
				if (!(item?.GrabObject == null) && !(item.Rigidbody == null))
				{
					Rigidbody rigidbody = item.Rigidbody;
					rigidbody.isKinematic = false;
					rigidbody.useGravity = false;
					rigidbody.linearVelocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
					rigidbody.WakeUp();
					GrabObjectBase grabObject = item.GrabObject;
					Transform value = item.GetNearestHandle(position) ?? item.GameObject.transform;
					if (!_spawnedThrowGrabber.physGrabPoints.TryAdd(grabObject, value))
					{
						_spawnedThrowGrabber.physGrabPoints[grabObject] = value;
					}
					if (!grabObject.Grabbers.Contains(_spawnedThrowGrabber))
					{
						grabObject.Grabbers.Add(_spawnedThrowGrabber);
					}
					_compositeGrabbedObjects.Add(grabObject);
				}
			}
			return _compositeGrabbedObjects.Count > 0;
		}

		private void UnregisterCompositeThrowGrabber(bool despawn)
		{
			if (_spawnedThrowGrabber != null)
			{
				foreach (GrabObjectBase compositeGrabbedObject in _compositeGrabbedObjects)
				{
					if (!(compositeGrabbedObject == null))
					{
						compositeGrabbedObject.Grabbers.Remove(_spawnedThrowGrabber);
						_spawnedThrowGrabber.physGrabPoints.Remove(compositeGrabbedObject);
					}
				}
				_spawnedThrowGrabber.IsProcessPhysGrabbing = _spawnedThrowGrabber.physGrabPoints.Count > 0;
				if (despawn && _spawnedThrowGrabber.Object != null && base.Runner != null && base.Runner.IsRunning)
				{
					base.Runner.Despawn(_spawnedThrowGrabber.Object);
				}
				_spawnedThrowGrabber = null;
			}
			_compositeGrabbedObjects.Clear();
		}

		private void EnableGravityOnConnectedGrabables()
		{
			if (!TryGetConnectedGrabables(out var grabables))
			{
				return;
			}
			foreach (SimplePointGrabable item in grabables)
			{
				if (!(item?.Rigidbody == null))
				{
					item.Rigidbody.useGravity = true;
				}
			}
		}

		private IEnumerator MoveThrowGrabberAlongArcCoroutine(Vector3 startPosition, Vector3 targetPosition)
		{
			if (_spawnedThrowGrabber == null)
			{
				yield break;
			}
			float duration;
			Vector3 throwVelocity = _krakenItemThrowService.CalculateThrowVelocity(startPosition, targetPosition, _throwDuration, _throwHorizontalSpeed, out duration);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (!IsAssignedItemValid() || _spawnedThrowGrabber == null)
				{
					yield break;
				}
				elapsed += Time.fixedDeltaTime;
				float elapsed2 = Mathf.Min(elapsed, duration);
				_spawnedThrowGrabber.transform.position = _krakenItemThrowService.EvaluateBallisticPosition(startPosition, throwVelocity, elapsed2);
				yield return new WaitForFixedUpdate();
			}
			if (_spawnedThrowGrabber != null)
			{
				_spawnedThrowGrabber.transform.position = targetPosition;
			}
		}

		private void ThrowRigidbody(Rigidbody rigidbody, Vector3 targetPosition)
		{
			_krakenItemThrowService.ThrowRigidbody(rigidbody, targetPosition, _throwDuration, _throwHorizontalSpeed);
		}

		private void TrackThrowbackDamage()
		{
			if (IsAssignedItemValid() && !(_assignedItem.NetworkObject == null) && !(_throwTargetPlayer == PlayerRef.None) && _throwTargetPlayer.IsRealPlayer)
			{
				_throwbackDamageItem = _assignedItem.NetworkObject;
				_throwbackDamageTargetPlayer = _throwTargetPlayer;
				_throwbackDamageExpiresAt = Time.time + Mathf.Max(0.01f, _throwbackDamageTrackingTime);
			}
		}

		private void HandleItemCollision(ItemCollisionData itemCollisionData)
		{
			if (!base.HasStateAuthority || itemCollisionData?.Item == null || !IsThrowbackDamageTrackingActive())
			{
				return;
			}
			if (Time.time > _throwbackDamageExpiresAt)
			{
				ClearThrowbackDamageTracking();
				if (_isAggressiveRockThrow)
				{
					DespawnAggressiveRockAndRelease();
				}
				else
				{
					ReleaseItem(restorePhysics: false);
				}
			}
			else
			{
				if (itemCollisionData.Item.NetworkObject != _throwbackDamageItem)
				{
					return;
				}
				PlayerDamageable playerDamageable;
				PlayerRef playerRef;
				if (_isAggressiveRockThrow)
				{
					if (TryCollectAggressiveRockHitTargets(itemCollisionData, _rockHitDamageTargets))
					{
						ApplyAggressiveRockDamage(_rockHitDamageTargets, itemCollisionData);
					}
				}
				else if (TryGetPlayerDamageableFromCollider(itemCollisionData.CollisionCollider, out playerDamageable, out playerRef) && (!IsNearThrowback || !(playerRef == _throwbackDamageTargetPlayer)))
				{
					playerDamageable.DamageRPC(_throwbackDamage, _throwbackDamageTargetPlayer.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForEnvironment(DamageType.KrakenThrowback)));
					ClearThrowbackDamageTracking();
					ReleaseItem(restorePhysics: false);
				}
			}
		}

		private bool TryCollectAggressiveRockHitTargets(ItemCollisionData itemCollisionData, List<PlayerDamageable> hitTargets)
		{
			hitTargets.Clear();
			_rockHitPlayerNetworkIds.Clear();
			if (itemCollisionData?.Item?.NetworkObject == null)
			{
				return false;
			}
			Vector3 position = itemCollisionData.Item.NetworkObject.transform.position;
			Vector3 position2 = ((itemCollisionData.CollisionCollider != null) ? itemCollisionData.CollisionCollider.ClosestPoint(position) : position);
			float radius = Mathf.Max(0.01f, _aggressiveRockHitRadius);
			int num = Physics.OverlapSphereNonAlloc(position2, radius, RockHitOverlapBuffer, -1, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				Collider collider = RockHitOverlapBuffer[i];
				if (!(collider == null) && TryGetPlayerDamageableFromCollider(collider, out var playerDamageable) && !(playerDamageable.NetworkObject == null) && _rockHitPlayerNetworkIds.Add(playerDamageable.NetworkObject.Id))
				{
					hitTargets.Add(playerDamageable);
				}
			}
			return hitTargets.Count > 0;
		}

		private void ApplyAggressiveRockDamage(IReadOnlyList<PlayerDamageable> playerDamageables, ItemCollisionData itemCollisionData)
		{
			float num = ((_playerStatesConfiguration != null) ? _playerStatesConfiguration.StunThrowMultiplier : 1f);
			foreach (PlayerDamageable playerDamageable in playerDamageables)
			{
				if (!(playerDamageable == null))
				{
					Vector3 direction = ((itemCollisionData.Item?.NetworkObject != null) ? (playerDamageable.transform.position - itemCollisionData.Item.NetworkObject.transform.position) : base.transform.forward);
					if (direction.sqrMagnitude > 0.0001f)
					{
						direction.Normalize();
					}
					else
					{
						direction = -playerDamageable.transform.forward;
					}
					playerDamageable.Damage(new DamageData
					{
						Damage = _aggressiveRockDamage,
						Position = playerDamageable.transform.position,
						Direction = direction,
						Force = _aggressiveRockKnockbackForce * num,
						ForceMode = ForceMode.Impulse,
						IsStunning = true,
						DamageDealerPlayerID = _throwbackDamageTargetPlayer.PlayerId,
						Source = DamageDataSourceExtensions.ForEnvironment(DamageType.KrakenAggressiveRock)
					});
				}
			}
			ClearThrowbackDamageTracking();
			DespawnAggressiveRockAndRelease();
		}

		private bool TryGetPlayerHolder(PlayerRef playerRef, out PlayerDataHolder holder)
		{
			holder = null;
			if (_spawnedPlayersModel == null || playerRef == PlayerRef.None)
			{
				return false;
			}
			if (_spawnedPlayersModel.Players.TryGetValue(playerRef, out holder))
			{
				return holder != null;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == playerRef.PlayerId)
				{
					holder = player.Value;
					return holder != null;
				}
			}
			return false;
		}

		private bool TryGetPlayerDamageableFromCollider(Collider collisionCollider, out PlayerDamageable playerDamageable)
		{
			PlayerRef playerRef;
			return TryGetPlayerDamageableFromCollider(collisionCollider, out playerDamageable, out playerRef);
		}

		private bool TryGetPlayerDamageableFromCollider(Collider collisionCollider, out PlayerDamageable playerDamageable, out PlayerRef playerRef)
		{
			playerDamageable = null;
			playerRef = PlayerRef.None;
			if (collisionCollider == null || _spawnedPlayersModel == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player.Value;
				if (!(value?.NetworkObject == null))
				{
					Transform transform = value.NetworkObject.transform;
					Transform transform2 = collisionCollider.transform;
					if ((!(transform2 != transform) || transform2.IsChildOf(transform)) && value.NetworkObject.TryGetComponent<PlayerDamageable>(out playerDamageable))
					{
						playerRef = player.Key;
						return true;
					}
				}
			}
			return false;
		}

		private void ClearExpiredThrowbackDamageTracking()
		{
			if (IsThrowbackDamageTrackingActive() && !(Time.time <= _throwbackDamageExpiresAt))
			{
				ClearThrowbackDamageTracking();
				if (_isAggressiveRockThrow)
				{
					DespawnAggressiveRockAndRelease();
				}
				else
				{
					ReleaseItem(restorePhysics: false);
				}
			}
		}

		private bool IsThrowbackDamageTrackingActive()
		{
			if (_throwbackDamageItem != null)
			{
				return _throwbackDamageTargetPlayer != PlayerRef.None;
			}
			return false;
		}

		private void ClearThrowbackDamageTracking()
		{
			_throwbackDamageItem = null;
			_throwbackDamageTargetPlayer = PlayerRef.None;
			_throwbackDamageExpiresAt = 0f;
		}

		private void DespawnAggressiveRockAndRelease()
		{
			if (_assignedItem?.NetworkObject != null && base.Runner != null && base.Runner.IsRunning && _assignedItem.NetworkObject.IsValid)
			{
				base.Runner.Despawn(_assignedItem.NetworkObject);
			}
			ReleaseItem(restorePhysics: false);
		}

		public void HandleTargetPlayerDisconnected(PlayerRef player)
		{
			if (!base.HasStateAuthority || player == PlayerRef.None)
			{
				return;
			}
			if (IsThrowbackDamageTrackingActive() && _throwbackDamageTargetPlayer.PlayerId == player.PlayerId)
			{
				ClearThrowbackDamageTracking();
				if (_isAggressiveRockThrow)
				{
					DespawnAggressiveRockAndRelease();
				}
				else
				{
					ReleaseItem(restorePhysics: false);
				}
			}
			else if (IsBusy && !_hasThrown && !(_throwTargetPlayer == PlayerRef.None) && _throwTargetPlayer.PlayerId == player.PlayerId)
			{
				_throwTargetPlayer = (TryFindNearestRemainingPlayer(player, out var nearestPlayer) ? nearestPlayer : PlayerRef.None);
				_isHelpThrowToAlive = false;
				_helpThrowTargetPosition = Vector3.zero;
			}
		}

		private bool TryFindNearestRemainingPlayer(PlayerRef excludedPlayer, out PlayerRef nearestPlayer)
		{
			nearestPlayer = PlayerRef.None;
			if (_spawnedPlayersModel == null)
			{
				return false;
			}
			Vector3 a = ((_catchBone != null) ? _catchBone.position : base.transform.position);
			float num = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				if (player.Key.PlayerId == excludedPlayer.PlayerId)
				{
					continue;
				}
				PlayerDataHolder value = player.Value;
				if (!(value?.NetworkObject == null))
				{
					float num2 = Vector3.Distance(a, value.NetworkObject.transform.position);
					if (!(num2 >= num))
					{
						num = num2;
						nearestPlayer = player.Key;
					}
				}
			}
			return nearestPlayer != PlayerRef.None;
		}

		public void ReleaseItem(bool restorePhysics = true)
		{
			ReleaseItem(restorePhysics, clearNetworkAssignment: true);
		}

		private void ReleaseItem(bool restorePhysics, bool clearNetworkAssignment)
		{
			StopCatchCoroutine();
			StopThrowCoroutine();
			StopThrowFallbackCoroutine();
			SetItemCollisionIgnored(ignored: false);
			UnregisterPhysGrabber(restorePhysics);
			UnregisterCompositeThrowGrabber(despawn: true);
			EnsureItemSwimmingEnabled(_assignedItem);
			_assignedItem = null;
			_isItemCaught = false;
			_isAimingAtPlayer = false;
			_hasThrown = false;
			_throwTargetPlayer = PlayerRef.None;
			_isHelpThrowToAlive = false;
			_isDirectDamagingThrowback = false;
			_helpThrowTargetPosition = Vector3.zero;
			_isAggressiveRockThrow = false;
			_aggressiveRockDamage = 0f;
			_aggressiveRockKnockbackForce = 0f;
			_aggressiveRockHitRadius = 0f;
			IsBusy = false;
			if (clearNetworkAssignment && base.HasStateAuthority)
			{
				AssignedItemNetworkId = default(NetworkId);
			}
		}

		private void StopCatchCoroutine()
		{
			if (_catchCoroutine != null)
			{
				StopCoroutine(_catchCoroutine);
				_catchCoroutine = null;
			}
		}

		private void StopThrowCoroutine()
		{
			if (_throwCoroutine != null)
			{
				Coroutine throwCoroutine = _throwCoroutine;
				_throwCoroutine = null;
				StopCoroutine(throwCoroutine);
				UnregisterCompositeThrowGrabber(despawn: true);
			}
		}

		private void StopThrowFallbackCoroutine()
		{
			if (_throwFallbackCoroutine != null)
			{
				Coroutine throwFallbackCoroutine = _throwFallbackCoroutine;
				_throwFallbackCoroutine = null;
				StopCoroutine(throwFallbackCoroutine);
			}
		}

		private static void EnsureItemSwimmingEnabled(IPointGrabable item)
		{
			if (item != null && (!(item is UnityEngine.Object obj) || !(obj == null)))
			{
				GameObject gameObject = item.GameObject;
				if (!(gameObject == null) && gameObject.TryGetComponent<SwimmingComponent>(out var component) && component.HasStateAuthority)
				{
					component.IsSwimmingDisabled = false;
				}
			}
		}

		private bool IsAssignedItemValid()
		{
			if (_assignedItem != null)
			{
				return _assignedItem as UnityEngine.Object != null;
			}
			return false;
		}

		private void ResolveAssignedItemFromNetwork()
		{
			if (!_isSpawned || base.HasStateAuthority)
			{
				return;
			}
			if (AssignedItemNetworkId == default(NetworkId))
			{
				if (_assignedItem != null || IsBusy)
				{
					ReleaseItem(restorePhysics: false, clearNetworkAssignment: false);
				}
			}
			else if (!IsAssignedNetworkItemCurrent())
			{
				if (_assignedItem != null || IsBusy)
				{
					ReleaseItem(restorePhysics: false, clearNetworkAssignment: false);
				}
				if (TryResolveAssignedItem(AssignedItemNetworkId, out var assignedItem))
				{
					_assignedItem = assignedItem;
					IsBusy = true;
					UpdateTargetPosition();
				}
			}
		}

		private bool IsAssignedNetworkItemCurrent()
		{
			if (!IsAssignedItemValid() || _assignedItem.NetworkObject == null)
			{
				return false;
			}
			return _assignedItem.NetworkObject.Id == AssignedItemNetworkId;
		}

		private bool TryResolveAssignedItem(NetworkId assignedItemNetworkId, out IPointGrabable assignedItem)
		{
			assignedItem = null;
			if (base.Runner == null || !base.Runner.IsRunning)
			{
				return false;
			}
			if (!base.Runner.TryFindObject(assignedItemNetworkId, out var networkObject) || networkObject == null)
			{
				return false;
			}
			assignedItem = networkObject.GetComponent<IPointGrabable>();
			if (assignedItem != null)
			{
				return assignedItem.GameObject != null;
			}
			return false;
		}

		private void RequestAssignedItemStateAuthorityIfNeeded()
		{
			if (IsAssignedItemValid() && !(_assignedItem.NetworkObject == null) && !(base.Object == null))
			{
				PlayerRef stateAuthority = base.Object.StateAuthority;
				if (!(stateAuthority == PlayerRef.None) && !(_assignedItem.NetworkObject.StateAuthority == stateAuthority))
				{
					_assignedItem.RequestStateAuthorityRPC(stateAuthority.PlayerId);
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (Application.isPlaying && TryGetThrowEndPositionForGizmo(out var position))
			{
				Vector3 obj = ((_catchBone != null) ? _catchBone.position : base.transform.position);
				Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.9f);
				Gizmos.DrawWireSphere(position, 0.3f);
				Gizmos.DrawSphere(position, 0.12f);
				Gizmos.color = new Color(1f, 0.85f, 0.1f, 0.9f);
				Gizmos.DrawLine(obj, position);
			}
		}

		private bool TryGetThrowEndPositionForGizmo(out Vector3 position)
		{
			position = Vector3.zero;
			if (!IsBusy || _throwTargetPlayer == PlayerRef.None)
			{
				return false;
			}
			return TryResolveThrowTargetPosition(out position);
		}

		private void SetItemCollisionIgnored(bool ignored)
		{
			if (!ignored)
			{
				if (_isItemCollisionIgnored)
				{
					if (IsAssignedItemValid())
					{
						_assignedItem.RemoveIgnoreItemsCollisionRequest(_collisionIgnoreOwnerId);
					}
					_isItemCollisionIgnored = false;
				}
			}
			else if (!_isItemCollisionIgnored && IsAssignedItemValid())
			{
				_assignedItem.AddIgnoreItemsCollisionRequest(_collisionIgnoreOwnerId);
				_isItemCollisionIgnored = true;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			AssignedItemNetworkId = _AssignedItemNetworkId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_AssignedItemNetworkId = AssignedItemNetworkId;
		}
	}
}
