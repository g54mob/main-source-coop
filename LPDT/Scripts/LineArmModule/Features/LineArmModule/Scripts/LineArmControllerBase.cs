using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InputModule.Scripts.Generated;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LineArmModule.Scripts.Data;
using Features.LineArmModule.Scripts.HeavyItemData;
using Features.MagnetModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkInputModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using GameplayEvents;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class LineArmControllerBase : NetworkBehaviour, IPlayerReboundListener
	{
		private static readonly IComparer<RaycastHit> _distanceComparer = Comparer<RaycastHit>.Create((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));

		protected readonly List<IPointGrabable> _currentGrabbables = new List<IPointGrabable>();

		private readonly Dictionary<IPointGrabable, Transform> _handles = new Dictionary<IPointGrabable, Transform>();

		private readonly Dictionary<IPointGrabable, float> _jointDistances = new Dictionary<IPointGrabable, float>();

		private readonly Dictionary<NetworkId, IPointGrabable> _pointGrabablesMapper = new Dictionary<NetworkId, IPointGrabable>();

		[SerializeField]
		private Transform _grabPoint;

		[SerializeField]
		private SpringJoint _playersJoint;

		[SerializeField]
		private LayerMask _raycastLayerMask;

		[SerializeField]
		private List<Collider> _ignoredRaycastColliders = new List<Collider>();

		[SerializeField]
		private float _maxRayDistance = 20f;

		[SerializeField]
		private float _maxThrowImpulse = 10f;

		[SerializeField]
		private float _holdTime = 0.5f;

		[SerializeField]
		private float _unJoinSqrDistance = 10f;

		[SerializeField]
		private float _scrollSpeed = 8f;

		[SerializeField]
		private float _velocityResistanceFactor = 0.5f;

		[SerializeField]
		private float _gravityResistanceFactor = 0.5f;

		[SerializeField]
		private float _springMultiplier;

		[SerializeField]
		private float _damper = 10f;

		[SerializeField]
		private float _minJointDistance = 1.5f;

		[SerializeField]
		private float _maxJointDistance = 3f;

		[SerializeField]
		private float _multipleObjectGrabRadius = 0.5f;

		[SerializeField]
		private float _grabPriorityOverrideDistance = 0.6f;

		[SerializeField]
		private float _grabStrengthMultiplier = 10f;

		[SerializeField]
		private PhysicsMaterial _physicMaterialOnGrab;

		[SerializeField]
		private EntityStatType _handsDistanceStatType = EntityStatType.HandsDistance;

		[SerializeField]
		private EntityStatType _grabStrengthEntityStatType = EntityStatType.GrabStrength;

		[SerializeField]
		private EntityStatType _maxWeightCapacityStatType = EntityStatType.MaxWeightCapacity;

		public Action OnGrabbedChanged;

		public Action OnLastRaycastChanged;

		private bool _isCartGrabState;

		protected CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		private ArmInputModel _armInputModel;

		private ItemDamageDisplayModel _itemDamageDisplayModel;

		private LineArmConfiguration _armConfiguration;

		private LineArmGrabSettingsModel _lineArmGrabSettingsModel;

		private IInputService _inputService;

		private InteractModel _interactModel;

		private LineArmsModel _lineArmsModel;

		private PlayerGrabSimplePointGrabableModel _playerGrabSimplePointGrabableModel;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private GrabDistanceConfiguration _grabDistanceConfiguration;

		private IStat _handsDistanceStat;

		private IStat _grabStrengthStat;

		private IStat _maxWeightCapacityStat;

		private List<IPointGrabable> _lastRaycastedGrabbables = new List<IPointGrabable>();

		private Vector3 _jointInitialLocalPosition;

		private RaycastHit[] _hits = new RaycastHit[32];

		private const float UNJOIN_LEASH_GRACE_SECONDS = 0.3f;

		private readonly Dictionary<IPointGrabable, float> _overLeashSeconds = new Dictionary<IPointGrabable, float>();

		private Vector2 _mouseScrollValue;

		private float _currentJointDistance;

		private float _throwMultiplier;

		private bool _isThrowStarted;

		private bool _readyToGrab;

		protected bool _isGrabEnabled = true;

		private HeavyItemLocalModel _heavyItemLocalModel;

		private CancellationTokenSource _grabCancellationTokenSource = new CancellationTokenSource();

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private EntityStatEntityNetworkedBase _statEntity;

		private GameplayEventBus _gameplayEventBus;

		private PlayerReboundModel _playerReboundModel;

		protected ILoadingScreenService _loadingScreenService;

		private readonly HashSet<object> _countedDisplayClusters = new HashSet<object>();

		public abstract PhysGrabber PhysGrabber { get; }

		public Vector2 MouseScrollValue
		{
			get
			{
				return _mouseScrollValue;
			}
			set
			{
				_mouseScrollValue = value;
			}
		}

		public float MinScrollDistanceValue
		{
			get
			{
				return _minJointDistance;
			}
			set
			{
				_minJointDistance = value;
			}
		}

		public bool DisableScroll { get; set; }

		[field: SerializeField]
		public float ArmEndMoveSpeed { get; private set; } = 0.05f;

		public IReadOnlyList<IPointGrabable> CurrentGrabbables => _currentGrabbables;

		public IReadOnlyList<Transform> CurrentHandles => _handles.Values.ToList();

		public IPointGrabable LastRaycastedGrabbable
		{
			get
			{
				if (_lastRaycastedGrabbables.Count != 0 && !(_cameraModel.CameraObject == null))
				{
					return (from g in _lastRaycastedGrabbables
						orderby (!g.IsGrabPriority) ? 1 : 0, Vector3.Distance(g.GameObject.transform.position, _cameraModel.AimTransform.position)
						select g).First();
				}
				return null;
			}
		}

		public abstract LineArmType ArmType { get; }

		private bool IsCameraReady
		{
			get
			{
				if (_cameraModel != null)
				{
					return _cameraModel.CameraObject != null;
				}
				return false;
			}
		}

		public bool TryGetCurrentHandle(out IPointGrabable grabbable, out Transform handle)
		{
			using (Dictionary<IPointGrabable, Transform>.Enumerator enumerator = _handles.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<IPointGrabable, Transform> current = enumerator.Current;
					grabbable = current.Key;
					handle = current.Value;
					return true;
				}
			}
			grabbable = null;
			handle = null;
			return false;
		}

		private void Start()
		{
			_jointInitialLocalPosition = _grabPoint.localPosition;
		}

		private void RefreshHeldItemWeight()
		{
			if (_currentGrabbables.Count == 0)
			{
				return;
			}
			bool flag = false;
			float num = 1f;
			foreach (IPointGrabable currentGrabbable in _currentGrabbables)
			{
				if (currentGrabbable.IsHeavyItem)
				{
					flag = true;
					num = Mathf.Max(num, currentGrabbable.HeavyItemMultiplier);
				}
			}
			_heavyItemLocalModel.IsCurrentlyGrabbedHeavyItem = flag;
			if (flag)
			{
				_heavyItemLocalModel.HeavyItemMultiplier = num;
			}
		}

		private void Update()
		{
			if (base.Object.HasInputAuthority && !base.Object.HasStateAuthority)
			{
				_isThrowStarted = false;
				_readyToGrab = false;
			}
			if (!base.Object.HasStateAuthority)
			{
				return;
			}
			ApplyGrabStrengthFromStat();
			RefreshHeldItemWeight();
			if (_itemDamageDisplayModel == null)
			{
				return;
			}
			if (_itemDamageDisplayModel.CurrentDisplayedItem.Count == 0)
			{
				_itemDamageDisplayModel.CurrentCurrency = 0;
				return;
			}
			int num = 0;
			bool isWithChildCurrency = false;
			_countedDisplayClusters.Clear();
			foreach (MonoItem item in _itemDamageDisplayModel.CurrentDisplayedItem)
			{
				if (item.IsNeedToShowPrice)
				{
					num += ResolveDisplayedCurrency(item);
				}
				if (item.TryGetComponent<GrabObjectBase>(out var component) && component.CartItemsGrabber != null)
				{
					foreach (IPointGrabable item2 in component.CartItemsGrabber.Items)
					{
						if (!(item2.GameObject == null) && item2.GameObject.TryGetComponent<MonoItem>(out var component2) && component2.IsNeedToShowPrice)
						{
							isWithChildCurrency = true;
							num += ResolveDisplayedCurrency(component2);
						}
					}
				}
				if (!item.TryGetComponent<MagnetInteractable>(out var component3))
				{
					continue;
				}
				foreach (SimplePointGrabable grabbedItem in component3.GrabbedItems)
				{
					if (!(((IPointGrabable)grabbedItem).GameObject == null) && ((IPointGrabable)grabbedItem).GameObject.TryGetComponent<MonoItem>(out var component4) && component4.IsNeedToShowPrice)
					{
						isWithChildCurrency = true;
						num += ResolveDisplayedCurrency(component4);
					}
				}
			}
			_itemDamageDisplayModel.IsWithChildCurrency = isWithChildCurrency;
			_itemDamageDisplayModel.CurrentCurrency = num;
		}

		public override void Spawned()
		{
			base.Spawned();
			InputVector2Actions armItemDistanceChange = _inputService.ArmItemDistanceChange;
			armItemDistanceChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(armItemDistanceChange.VectorChangedPerformed, new Action<Vector2>(AccumulateMouseScroll));
			_lineArmsModel.RegisterLineArm(base.Object.InputAuthority.PlayerId, ArmType, this);
			_playerReboundModel.OnPlayerRebound += OnPlayerRebound;
		}

		protected void ProcessStatsInitialization()
		{
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(base.Object.InputAuthority.PlayerId))
			{
				InitializeStats();
			}
			else
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += RegisterStatEntity;
			}
		}

		private void InitializeStats()
		{
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(base.Object.InputAuthority.PlayerId, out var value))
			{
				_statEntity = value;
				_handsDistanceStat = _statEntity.GetStat(_handsDistanceStatType);
				_grabStrengthStat = _statEntity.GetStat(_grabStrengthEntityStatType);
				_maxWeightCapacityStat = _statEntity.GetStat(_maxWeightCapacityStatType);
				ApplyGrabStrengthFromStat();
			}
		}

		private void ApplyGrabStrengthFromStat()
		{
			if (_grabStrengthStat != null)
			{
				PhysGrabber physGrabber = PhysGrabber;
				if (!(physGrabber == null))
				{
					physGrabber.forceConstant = _grabStrengthStat.FullValue;
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerReboundModel.OnPlayerRebound -= OnPlayerRebound;
			InputVector2Actions armItemDistanceChange = _inputService.ArmItemDistanceChange;
			armItemDistanceChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(armItemDistanceChange.VectorChangedPerformed, new Action<Vector2>(AccumulateMouseScroll));
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= RegisterStatEntity;
			_lineArmsModel.UnregisterLineArm(base.Object.InputAuthority.PlayerId, ArmType);
			UnJoinAll(throwItem: false);
			CancelPendingGrabOperations();
			_grabCancellationTokenSource.Dispose();
		}

		private void RegisterStatEntity(int playerId)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerId == base.Object.InputAuthority.PlayerId)
			{
				InitializeStats();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!IsCameraReady)
			{
				return;
			}
			HandleRaycastOutline();
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && GetInput<NetworkInputActions>(out var input))
			{
				_armInputModel.IsThrowEnabled = _currentGrabbables.Count > 0;
				if (input.DropAllFromArmsPhase.IsSet(InputActionPhase.Started))
				{
					_isThrowStarted = true;
				}
				if (input.DropAllFromArmsPhase.IsSet(InputActionPhase.Canceled))
				{
					UnJoinAll(throwItem: true);
					_isThrowStarted = false;
				}
				if (_isThrowStarted)
				{
					_throwMultiplier += Time.fixedDeltaTime / _holdTime;
				}
				HandleInput(input);
				ValidateCurrentGrabbable();
				CheckUnjoinDistance();
				if (IsLocalPlayerInStore())
				{
					_playersJoint.connectedBody = null;
				}
				if (_playersJoint.connectedBody != null)
				{
					_playersJoint.spring = _currentJointDistance * _springMultiplier;
					_playersJoint.damper = _damper;
				}
				else
				{
					_playersJoint.spring = 0f;
					_playersJoint.damper = 0f;
				}
			}
		}

		public void UnJoinAll(bool throwItem)
		{
			CancelPendingGrabOperations();
			for (int num = _currentGrabbables.Count - 1; num >= 0; num--)
			{
				UnJoin(_currentGrabbables[num], throwItem);
			}
			ResetThrowState();
		}

		public void UnJoinAllMatching(Predicate<IPointGrabable> match)
		{
			for (int num = _currentGrabbables.Count - 1; num >= 0; num--)
			{
				IPointGrabable pointGrabable = _currentGrabbables[num];
				if (match(pointGrabable))
				{
					UnJoin(pointGrabable, throwItem: false);
				}
			}
		}

		public void EnableGrabbing(bool enable)
		{
			_isGrabEnabled = enable;
		}

		private bool IsLocalPlayerInStore()
		{
			if (!_playersStatesSynchronizer.TryGetState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var state))
			{
				return false;
			}
			return state == PlayerState.Store;
		}

		private int ResolveDisplayedCurrency(MonoItem monoItem)
		{
			return DisplayedCurrencyResolver.Resolve(monoItem, _countedDisplayClusters);
		}

		private float GetCurrentWeight()
		{
			float num = 0f;
			foreach (IPointGrabable currentGrabbable in _currentGrabbables)
			{
				num += currentGrabbable.Weight;
			}
			return num;
		}

		[Inject]
		private void Inject(CameraModel cameraModel, MultiplayerModel multiplayerModel, ArmInputModel armInputModel, ItemDamageDisplayModel itemDamageDisplayModel, IInputService inputService, InteractModel interactModel, LineArmConfiguration armConfiguration, LineArmGrabSettingsModel lineArmGrabSettingsModel, LineArmsModel lineArmsModel, PlayerGrabSimplePointGrabableModel playerGrabSimplePointGrabableModel, PlayersStatesSynchronizer playersStatesSynchronizer, GrabDistanceConfiguration grabDistanceConfiguration, HeavyItemLocalModel heavyItemLocalModel, SpawnedEntityStatsModel spawnedEntityStatsModel, GameplayEventBus gameplayEventBus, ILoadingScreenService loadingScreenService, PlayerReboundModel playerReboundModel)
		{
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_armInputModel = armInputModel;
			_itemDamageDisplayModel = itemDamageDisplayModel;
			_inputService = inputService;
			_interactModel = interactModel;
			_armConfiguration = armConfiguration;
			_lineArmGrabSettingsModel = lineArmGrabSettingsModel;
			_lineArmsModel = lineArmsModel;
			_playerGrabSimplePointGrabableModel = playerGrabSimplePointGrabableModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_grabDistanceConfiguration = grabDistanceConfiguration;
			_heavyItemLocalModel = heavyItemLocalModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_loadingScreenService = loadingScreenService;
			_gameplayEventBus = gameplayEventBus;
			_playerReboundModel = playerReboundModel;
		}

		public virtual void OnPlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			if (playerRef.PlayerId == base.Object.InputAuthority.PlayerId)
			{
				ClearCachedReferences(avatar);
			}
		}

		private void ClearCachedReferences(NetworkObject avatar)
		{
			UnJoinAll(throwItem: false);
			CancelPendingGrabOperations();
			_handles.Clear();
			_jointDistances.Clear();
			_pointGrabablesMapper.Clear();
			_lastRaycastedGrabbables.Clear();
			DisableLastOutline();
			_statEntity = null;
			_handsDistanceStat = null;
			_grabStrengthStat = null;
			_maxWeightCapacityStat = null;
			if (!(avatar == null) && avatar.IsValid)
			{
				ProcessStatsInitialization();
			}
		}

		private void HandleRaycastOutline()
		{
			if (!_isGrabEnabled)
			{
				DisableLastOutline();
			}
			else if (IsLocalPlayerGrabbingAnotherPlayerCharacter())
			{
				DisableLastOutline();
			}
			else
			{
				HandleRaycastByType();
			}
		}

		private bool IsLocalPlayerGrabbingAnotherPlayerCharacter()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			foreach (KeyValuePair<int, IPointGrabable> playerGrabable in _playerGrabSimplePointGrabableModel.PlayerGrabables)
			{
				if (playerGrabable.Key != playerId && playerGrabable.Value.GrabbedByPlayers.Contains(playerId))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsLocalPlayerCharacterGrabable(IPointGrabable grabable)
		{
			if (grabable == null || _playerGrabSimplePointGrabableModel == null || _multiplayerModel?.NetworkRunner == null)
			{
				return false;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playerGrabSimplePointGrabableModel.PlayerGrabables.TryGetValue(playerId, out var value))
			{
				return value == grabable;
			}
			return false;
		}

		private void HandleRaycastByType()
		{
			if (_cameraModel != null && !(_cameraModel.CameraObject == null))
			{
				if (GetCurrentWeight() == 0f)
				{
					DefaultHandleRaycastOutline();
				}
				else
				{
					HandleRaycastOutlineInArea();
				}
			}
		}

		private void DefaultHandleRaycastOutline()
		{
			Ray rayByRaycastType = GetRayByRaycastType();
			IPointGrabable pointGrabable = null;
			int num = Physics.RaycastNonAlloc(rayByRaycastType, _hits, _maxRayDistance, _raycastLayerMask);
			Array.Sort(_hits, 0, num, _distanceComparer);
			bool flag = false;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				RaycastHit hit = _hits[i];
				if (!TryResolveHitGrabbable(hit, out var grabbable))
				{
					continue;
				}
				if (!flag)
				{
					pointGrabable = grabbable;
					num2 = hit.distance;
					flag = true;
					if (grabbable == null || grabbable.IsGrabPriority)
					{
						break;
					}
					continue;
				}
				if (hit.distance > num2 + _grabPriorityOverrideDistance)
				{
					break;
				}
				if (grabbable != null && grabbable.IsGrabPriority)
				{
					pointGrabable = grabbable;
					break;
				}
			}
			if (pointGrabable == null)
			{
				pointGrabable = ResolveForcedFallbackTarget();
			}
			if (!_lastRaycastedGrabbables.Contains(pointGrabable))
			{
				DisableLastOutline();
				if (pointGrabable != null)
				{
					pointGrabable.EnableOutline(enable: true);
					_interactModel.CurrentInteractable = pointGrabable;
					_lastRaycastedGrabbables.Add(pointGrabable);
				}
				else
				{
					_lastRaycastedGrabbables.Clear();
				}
				OnLastRaycastChanged?.Invoke();
			}
		}

		private bool TryResolveHitGrabbable(RaycastHit hit, out IPointGrabable grabbable)
		{
			grabbable = null;
			if (hit.transform == null)
			{
				return false;
			}
			if (IsIgnoredRaycastCollider(hit.collider))
			{
				return false;
			}
			if (hit.transform.TryGetComponent<PointGrabableIgnore>(out var _))
			{
				return false;
			}
			if (hit.collider != null && hit.collider.TryGetComponent<PointGrabableContainer>(out var component2))
			{
				grabbable = component2.PointGrabable;
			}
			if (grabbable == null && !hit.transform.TryGetComponent<IPointGrabable>(out grabbable) && hit.transform.TryGetComponent<PointGrabableContainer>(out var component3))
			{
				grabbable = component3.PointGrabable;
			}
			if (grabbable == null)
			{
				return true;
			}
			if (_currentGrabbables.Count > 0 && grabbable.DisableGrabWhileHoldingOther)
			{
				grabbable = null;
				return false;
			}
			if (!grabbable.Initialized || grabbable.GrabBlocked || grabbable.LocalGrabBlocked)
			{
				grabbable = null;
				return false;
			}
			if (IsLocalPlayerCharacterGrabable(grabbable))
			{
				grabbable = null;
				return false;
			}
			if (grabbable.GrabbedByPlayers.Contains(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				grabbable = null;
				return false;
			}
			PlayerState state;
			bool hasPlayerState = _playersStatesSynchronizer.TryGetState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out state);
			if (!CanGrabByPlayerState(grabbable, hasPlayerState, state))
			{
				grabbable = null;
				return false;
			}
			if (_handsDistanceStat != null && _handsDistanceStat.FullValue + _grabDistanceConfiguration.GrabDistanceData[grabbable.GrabDistanceType].AdditionalGrabDistance <= Vector3.Distance(hit.point, _cameraModel.AimTransform.position))
			{
				grabbable = null;
				return true;
			}
			return true;
		}

		private IPointGrabable ResolveForcedFallbackTarget()
		{
			IPointGrabable forcedFallbackTarget = _interactModel.ForcedFallbackTarget;
			if (forcedFallbackTarget == null)
			{
				return null;
			}
			if (!forcedFallbackTarget.Initialized || forcedFallbackTarget.GrabBlocked || forcedFallbackTarget.LocalGrabBlocked)
			{
				return null;
			}
			if (_currentGrabbables.Count > 0 && forcedFallbackTarget.DisableGrabWhileHoldingOther)
			{
				return null;
			}
			if (forcedFallbackTarget.GrabbedByPlayers.Contains(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				return null;
			}
			return forcedFallbackTarget;
		}

		private void HandleRaycastOutlineInArea()
		{
			Ray rayByRaycastType = GetRayByRaycastType();
			IPointGrabable currentInteractable = null;
			int num = Physics.RaycastNonAlloc(rayByRaycastType, _hits, _maxRayDistance, _raycastLayerMask);
			if (num == 0)
			{
				return;
			}
			Array.Sort(_hits, 0, num, _distanceComparer);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _hits[i];
				if (raycastHit.transform == null || IsIgnoredRaycastCollider(raycastHit.collider))
				{
					continue;
				}
				IPointGrabable pointGrabable = ResolveGrabableFrom((raycastHit.collider != null) ? raycastHit.collider.gameObject : raycastHit.transform.gameObject);
				if (pointGrabable == null || (!_currentGrabbables.Contains(pointGrabable) && !pointGrabable.LocalGrabBlocked))
				{
					if (vector == Vector3.zero)
					{
						vector = raycastHit.point;
					}
					if (pointGrabable != null && pointGrabable.IsGrabPriority)
					{
						vector = raycastHit.point;
						break;
					}
				}
			}
			if (vector == Vector3.zero)
			{
				return;
			}
			List<IPointGrabable> list = new List<IPointGrabable>();
			Collider[] array = Physics.OverlapSphere(vector, _multipleObjectGrabRadius, _raycastLayerMask);
			foreach (Collider collider in array)
			{
				if (IsIgnoredRaycastCollider(collider))
				{
					continue;
				}
				IPointGrabable pointGrabable2 = ResolveGrabableFrom(collider.gameObject);
				if (pointGrabable2 != null && !list.Contains(pointGrabable2) && (_currentGrabbables.Count <= 0 || !pointGrabable2.DisableGrabWhileHoldingOther) && pointGrabable2.Initialized && !pointGrabable2.GrabBlocked && !pointGrabable2.LocalGrabBlocked && !IsLocalPlayerCharacterGrabable(pointGrabable2) && !pointGrabable2.GrabbedByPlayers.Contains(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) && (_handsDistanceStat == null || !(_handsDistanceStat.FullValue + _grabDistanceConfiguration.GrabDistanceData[pointGrabable2.GrabDistanceType].AdditionalGrabDistance <= Vector3.Distance(pointGrabable2.GameObject.transform.position, _cameraModel.AimTransform.position))))
				{
					PlayerState state;
					bool hasPlayerState = _playersStatesSynchronizer.TryGetState(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out state);
					if (CanGrabByPlayerState(pointGrabable2, hasPlayerState, state) && (_maxWeightCapacityStat == null || !(GetCurrentWeight() + pointGrabable2.Weight > _maxWeightCapacityStat.FullValue)))
					{
						list.Add(pointGrabable2);
					}
				}
			}
			if (list.Any((IPointGrabable g) => g.IsGrabPriority))
			{
				list.RemoveAll((IPointGrabable g) => !g.IsGrabPriority);
			}
			foreach (IPointGrabable lastRaycastedGrabbable in _lastRaycastedGrabbables)
			{
				if (!list.Contains(lastRaycastedGrabbable))
				{
					lastRaycastedGrabbable.EnableOutline(enable: false);
				}
			}
			foreach (IPointGrabable item in list)
			{
				if (!_lastRaycastedGrabbables.Contains(item))
				{
					item.EnableOutline(enable: true);
				}
			}
			_interactModel.CurrentInteractable = currentInteractable;
			_lastRaycastedGrabbables = list;
			if (!list.SequenceEqual(_lastRaycastedGrabbables))
			{
				OnLastRaycastChanged?.Invoke();
			}
		}

		protected abstract Ray GetRayByRaycastType();

		private bool CanGrabByPlayerState(IPointGrabable pointGrabable, bool hasPlayerState, PlayerState state)
		{
			if (!hasPlayerState)
			{
				return true;
			}
			return state switch
			{
				PlayerState.Dead => false, 
				PlayerState.PreDeadCrouch => pointGrabable.CanGrabInStun, 
				_ => true, 
			};
		}

		protected void DisableLastOutline()
		{
			if (_lastRaycastedGrabbables.Count == 0)
			{
				return;
			}
			foreach (IPointGrabable lastRaycastedGrabbable in _lastRaycastedGrabbables)
			{
				lastRaycastedGrabbable.EnableOutline(enable: false);
			}
			_lastRaycastedGrabbables.Clear();
			OnLastRaycastChanged?.Invoke();
			_interactModel.CurrentInteractable = null;
		}

		private void HandleInput(NetworkInputActions input)
		{
			if (input.ItemInteractPhase.IsSet(InputActionPhase.Started))
			{
				if (_interactModel.CurrentInteractable != null)
				{
					_interactModel.CurrentInteractable.Interactable?.Interact();
				}
				foreach (IPointGrabable currentGrabbable in _currentGrabbables)
				{
					currentGrabbable?.Interactable?.Interact();
				}
			}
			if (input.GrabItemPhase.IsSet(InputActionPhase.Started))
			{
				_readyToGrab = true;
				TryGrab(isAutoGrab: false);
			}
			if (input.GrabItemPhase.IsSet(InputActionPhase.Canceled))
			{
				_readyToGrab = false;
				if (!_lineArmGrabSettingsModel.IsLatchGrabEnabled)
				{
					UnJoinAll(throwItem: false);
				}
			}
			if (_currentGrabbables.Count >= 1 && _readyToGrab)
			{
				if (_armConfiguration.AutoMultiGrab)
				{
					TryGrab(isAutoGrab: true);
				}
				else if (input.ItemInteractPhase.IsSet(InputActionPhase.Started))
				{
					TryGrab(isAutoGrab: false);
				}
			}
			if (_readyToGrab && _currentGrabbables.Count == 0 && _armConfiguration.AutoFirstGrab)
			{
				TryGrab(isAutoGrab: true);
			}
			if (input.DropAllFromArmsPhase.IsSet(InputActionPhase.Performed))
			{
				UnJoinAll(throwItem: true);
			}
			HandleJointScroll();
		}

		private async UniTask<bool> TryGrab(bool isAutoGrab)
		{
			CancellationToken token = _grabCancellationTokenSource.Token;
			if (token.IsCancellationRequested)
			{
				return false;
			}
			if (_lastRaycastedGrabbables.Count == 0)
			{
				return false;
			}
			if (_currentGrabbables.Any((IPointGrabable g) => _lastRaycastedGrabbables.Contains(g)))
			{
				return false;
			}
			IPointGrabable grabbable = (from g in _lastRaycastedGrabbables
				orderby (!g.IsGrabPriority) ? 1 : 0, Vector3.Distance(g.GameObject.transform.position, _cameraModel.AimTransform.position)
				select g).First();
			if (GetCurrentWeight() + grabbable.Weight > _maxWeightCapacityStat.FullValue)
			{
				return false;
			}
			if (!grabbable.IsAutoGrabEnabled && isAutoGrab)
			{
				return false;
			}
			if (_currentGrabbables.Count == 0)
			{
				_currentGrabbables.Add(grabbable);
				InvokeGrabbedChangedCallbacks();
				_isCartGrabState = grabbable.InCart;
			}
			else
			{
				if (_isCartGrabState != grabbable.InCart)
				{
					return false;
				}
				_currentGrabbables.Add(grabbable);
				InvokeGrabbedChangedCallbacks();
			}
			DisableLastOutline();
			Transform nearestHandle = grabbable.GetNearestHandle(base.transform.position);
			_handles[grabbable] = nearestHandle;
			if (_cameraModel.CameraObject == null || nearestHandle == null)
			{
				return false;
			}
			float value = Vector3.Distance(_cameraModel.AimTransform.position, nearestHandle.position);
			_jointDistances[grabbable] = value;
			GrabDistanceData grabDistanceData = _grabDistanceConfiguration.GrabDistanceData[_currentGrabbables.First().GrabDistanceType];
			_currentJointDistance = Mathf.Clamp(value, grabDistanceData.AdditionalMinJointDistance + _minJointDistance, grabDistanceData.AdditionalMaxJointDistance + _handsDistanceStat.FullValue);
			_mouseScrollValue = Vector2.zero;
			Vector3 position = _cameraModel.AimTransform.position;
			Vector3 forward = _cameraModel.AimTransform.forward;
			Vector3 vector = ((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position;
			Vector3 vector2 = vector - Vector3.Project(vector, forward);
			_grabPoint.transform.position = position + vector2 + forward * _currentJointDistance;
			if (_currentGrabbables.Count > 1)
			{
				foreach (IPointGrabable currentGrabbable in _currentGrabbables)
				{
					currentGrabbable.GrabObject.GrabStrengthMultiplier = _grabStrengthMultiplier;
				}
				try
				{
					await UniTask.Delay((int)(ArmEndMoveSpeed * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
				}
				catch (OperationCanceledException)
				{
					return false;
				}
				if (token.IsCancellationRequested)
				{
					return false;
				}
				foreach (IPointGrabable currentGrabbable2 in _currentGrabbables)
				{
					currentGrabbable2.GrabObject.GrabStrengthMultiplier = 1f;
				}
			}
			if (token.IsCancellationRequested)
			{
				return false;
			}
			_heavyItemLocalModel.IsCurrentlyGrabbedHeavyItem = grabbable.IsHeavyItem;
			if (_heavyItemLocalModel.IsCurrentlyGrabbedHeavyItem)
			{
				_heavyItemLocalModel.HeavyItemMultiplier = grabbable.HeavyItemMultiplier;
			}
			grabbable.OnCleanup += delegate
			{
				HandleGrabbableCleanup(grabbable);
			};
			grabbable.GrabbedByPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (token.IsCancellationRequested || !_currentGrabbables.Contains(grabbable))
			{
				return false;
			}
			grabbable.InvokeOnGrab();
			if (grabbable.Rigidbody.isKinematic && grabbable.GrabbableStaticType == GrabbableStaticType.StaticHook && !IsLocalPlayerInStore())
			{
				_playersJoint.connectedBody = grabbable.Rigidbody;
			}
			grabbable.SetPhysicsMaterialToColliders(_physicMaterialOnGrab);
			TryGrabRpc(grabbable.NetworkObject);
			UpdateDisplayedItem();
			return true;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 993184785u)]
		private void TryGrabRpc([RpcPayload(4)] NetworkId id)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(993184785u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.LineArmModule.Scripts.LineArmControllerBase::TryGrabRpc(Fusion.NetworkId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(id, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.TryFindObject(id, out var networkObject))
			{
				return;
			}
			IPointGrabable component = networkObject.GetComponent<IPointGrabable>();
			if (component == null)
			{
				return;
			}
			_pointGrabablesMapper.TryAdd(id, component);
			if (!_currentGrabbables.Contains(component))
			{
				_currentGrabbables.Add(component);
				InvokeGrabbedChangedCallbacks();
			}
			Rigidbody rigidbody = component.Rigidbody;
			if (component.FreezeRotationOnGrab && rigidbody != null)
			{
				rigidbody.freezeRotation = true;
			}
			Transform transform = component.GetNearestHandle(base.transform.position) ?? component.GameObject.transform;
			_handles[component] = transform;
			_jointDistances[component] = ((_cameraModel.CameraObject != null) ? Vector3.Distance(_cameraModel.AimTransform.position, transform.position) : _currentJointDistance);
			if (base.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer)
			{
				_currentJointDistance = _jointDistances[component];
				GrabDistanceData grabDistanceData = _grabDistanceConfiguration.GrabDistanceData[_currentGrabbables.First().GrabDistanceType];
				_currentJointDistance = (grabDistanceData.IsWithGrabJointDistanceOverride ? Mathf.Clamp(_currentJointDistance, grabDistanceData.AdditionalMinJointDistance + _minJointDistance, grabDistanceData.GrabJointDistanceOverride) : Mathf.Clamp(_currentJointDistance, grabDistanceData.AdditionalMinJointDistance + _minJointDistance, grabDistanceData.AdditionalMaxJointDistance + _handsDistanceStat.FullValue));
				_mouseScrollValue = Vector2.zero;
				if (_cameraModel.CameraObject != null)
				{
					Vector3 position = _cameraModel.AimTransform.position;
					Vector3 forward = _cameraModel.AimTransform.forward;
					Vector3 vector = ((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position;
					Vector3 vector2 = vector - Vector3.Project(vector, forward);
					_grabPoint.transform.position = position + vector2 + forward * _currentJointDistance;
				}
			}
			PhysGrabber physGrabber = PhysGrabber;
			if (!(physGrabber == null))
			{
				physGrabber.IsProcessPhysGrabbing = true;
				if (!physGrabber.physGrabPoints.TryAdd(component.GrabObject, transform))
				{
					physGrabber.physGrabPoints[component.GrabObject] = transform;
				}
				component.GrabObject.Grabbers.Add(physGrabber);
			}
		}

		private void CancelPendingGrabOperations()
		{
			if (!_grabCancellationTokenSource.IsCancellationRequested)
			{
				_grabCancellationTokenSource.Cancel();
				_grabCancellationTokenSource.Dispose();
				_grabCancellationTokenSource = new CancellationTokenSource();
			}
		}

		private void UnJoin(IPointGrabable g, bool throwItem)
		{
			if (!(_cameraModel.CameraObject == null) && !(base.Object == null) && _currentGrabbables.Contains(g))
			{
				if (_currentGrabbables.Count == 1)
				{
					_mouseScrollValue = Vector2.zero;
					Vector3 position = _cameraModel.AimTransform.position;
					Vector3 vector = ((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position;
					float value = Vector3.Dot(vector, _cameraModel.AimTransform.forward);
					GrabDistanceData grabDistanceData = _grabDistanceConfiguration.GrabDistanceData[_currentGrabbables.First().GrabDistanceType];
					_currentJointDistance = Mathf.Clamp(value, grabDistanceData.AdditionalMinJointDistance + _minJointDistance, grabDistanceData.AdditionalMaxJointDistance + _handsDistanceStat.FullValue);
					Vector3 forward = _cameraModel.AimTransform.forward;
					Vector3 vector2 = vector - Vector3.Project(vector, forward);
					_grabPoint.transform.position = position + vector2 + forward * _currentJointDistance;
				}
				_heavyItemLocalModel.IsCurrentlyGrabbedHeavyItem = false;
				_heavyItemLocalModel.HeavyItemMultiplier = 1f;
				g.UnGrabbedByPlayer(base.Object.StateAuthority.PlayerId);
				g.InvokeOnUnGrab();
				if (g.Rigidbody != null && g.Rigidbody.isKinematic)
				{
					_playersJoint.connectedBody = null;
				}
				g.SetPhysicsMaterialToColliders(null);
				UnJoinRpc(g.NetworkObject, throwItem, _cameraModel.AimTransform.forward);
				if (g.GrabbedByPlayers.Count == 0)
				{
					g.Interactable?.OnInteractEnd();
				}
				UpdateDisplayedItem();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1343587657u)]
		private void UnJoinRpc([RpcPayload(4)] NetworkId id, [RpcPayload(4)] bool throwItem, [RpcPayload(12)] Vector3Compressed dir)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetPayloadSize(throwItem);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1343587657u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.LineArmModule.Scripts.LineArmControllerBase::UnJoinRpc(Fusion.NetworkId,System.Boolean,Fusion.Vector3Compressed)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(id, 4);
						writer.Write(throwItem);
						writer.Write(dir, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			NetworkObject networkObject = _multiplayerModel.NetworkRunner.FindObject(id);
			if (networkObject == null)
			{
				if (!_pointGrabablesMapper.TryGetValue(id, out var value))
				{
					return;
				}
				PhysGrabber physGrabber = PhysGrabber;
				GrabObjectBase grabObject = value.GrabObject;
				if (physGrabber != null)
				{
					if (grabObject != null)
					{
						grabObject.Grabbers.Remove(physGrabber);
					}
					physGrabber.physGrabPoints.Remove(grabObject);
				}
				_handles.Remove(value);
				_jointDistances.Remove(value);
				_currentGrabbables.Remove(value);
				_overLeashSeconds.Remove(value);
				InvokeGrabbedChangedCallbacks();
				if (value.IsMainRagdollGrabable && _gameplayEventBus != null)
				{
					_gameplayEventBus.Publish(new OnLineArmGrabNullResolveGameplayEvent(base.Object.InputAuthority.PlayerId, _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, id.Raw));
				}
				if (_currentGrabbables.Count == 0 && base.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer)
				{
					_mouseScrollValue = Vector2.zero;
					Vector3 position = _cameraModel.AimTransform.position;
					float value2 = Vector3.Dot(((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position, _cameraModel.AimTransform.forward);
					_currentJointDistance = Mathf.Clamp(value2, _minJointDistance, _handsDistanceStat.FullValue);
				}
				return;
			}
			IPointGrabable component = networkObject.GetComponent<IPointGrabable>();
			if (_currentGrabbables.Contains(component))
			{
				if (component.GrabbedByPlayers.Count == 0 && component.FreezeRotationOnGrab)
				{
					component.Rigidbody.freezeRotation = false;
				}
				if (throwItem && (bool)component.Rigidbody)
				{
					component.Rigidbody.AddForce((Vector3)dir * _throwMultiplier * _maxThrowImpulse, ForceMode.Impulse);
				}
				PhysGrabber physGrabber2 = PhysGrabber;
				if (physGrabber2 != null)
				{
					component.GrabObject.Grabbers.Remove(physGrabber2);
					physGrabber2.physGrabPoints.Remove(component.GrabObject);
				}
				_handles.Remove(component);
				_jointDistances.Remove(component);
				_currentGrabbables.Remove(component);
				_overLeashSeconds.Remove(component);
				InvokeGrabbedChangedCallbacks();
				if (_currentGrabbables.Count == 0 && base.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer)
				{
					_mouseScrollValue = Vector2.zero;
					Vector3 position2 = _cameraModel.AimTransform.position;
					float value3 = Vector3.Dot(((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position2, _cameraModel.AimTransform.forward);
					_currentJointDistance = Mathf.Clamp(value3, _minJointDistance, _handsDistanceStat.FullValue);
				}
				if (component.IsReturnsAuthorityToHost && _multiplayerModel.NetworkRunner.LocalPlayer == _multiplayerModel.NetworkRunner.ActivePlayers.OrderBy((PlayerRef p) => p.PlayerId).First())
				{
					component.RequestStateAuthorityRPC(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
				}
			}
		}

		private void CheckUnjoinDistance()
		{
			for (int num = _currentGrabbables.Count - 1; num >= 0; num--)
			{
				IPointGrabable pointGrabable = _currentGrabbables[num];
				if ((pointGrabable.GameObject.transform.position - base.transform.position).sqrMagnitude > _unJoinSqrDistance + _grabDistanceConfiguration.GrabDistanceData[pointGrabable.GrabDistanceType].AdditionalUnJoinSqrDistance + _handsDistanceStat.FullValue * _handsDistanceStat.FullValue)
				{
					_overLeashSeconds.TryGetValue(pointGrabable, out var value);
					value += base.Runner.DeltaTime;
					if (value >= 0.3f)
					{
						_overLeashSeconds.Remove(pointGrabable);
						UnJoin(pointGrabable, throwItem: false);
					}
					else
					{
						_overLeashSeconds[pointGrabable] = value;
					}
				}
				else
				{
					_overLeashSeconds.Remove(pointGrabable);
				}
			}
		}

		private void ValidateCurrentGrabbable()
		{
			for (int num = _currentGrabbables.Count - 1; num >= 0; num--)
			{
				IPointGrabable pointGrabable = _currentGrabbables[num];
				if (pointGrabable.NetworkObject == null)
				{
					UnJoin(pointGrabable, throwItem: false);
				}
			}
		}

		public void HandleJointScroll(bool forced = false)
		{
			if ((DisableScroll && !forced) || _currentGrabbables.Count == 0)
			{
				return;
			}
			float y = _mouseScrollValue.y;
			if (Mathf.Abs(y) <= 0.01f)
			{
				return;
			}
			List<Rigidbody> list = (from g in _currentGrabbables
				select g.Rigidbody into rb
				where rb != null
				select rb).ToList();
			if (list.Count == 0)
			{
				return;
			}
			Vector3 forward = _cameraModel.AimTransform.forward;
			float num = Mathf.Sign(y);
			float num2 = Vector3.Dot(forward * num, Vector3.up);
			float num3 = 1f;
			if (num2 > 0f)
			{
				num3 = 1f + num2 * _gravityResistanceFactor;
			}
			else if (num2 < 0f)
			{
				num3 = 1f / (1f + Mathf.Abs(num2) * 0.5f);
			}
			float num4 = 1f;
			float num5 = 0f;
			foreach (Rigidbody item in list)
			{
				num5 += item.mass;
				if (item.linearVelocity.sqrMagnitude > 0.001f)
				{
					float num6 = Vector3.Dot(item.linearVelocity.normalized, forward * num);
					if (num6 < 0f)
					{
						float num7 = Mathf.Clamp01(item.linearVelocity.magnitude / 10f);
						num4 += Mathf.Abs(num6) * num7 * _velocityResistanceFactor;
					}
				}
			}
			float num8 = Mathf.Clamp(num5 / 10f, 0.5f, 3f);
			float num9 = num3 * num4 * num8;
			float num10 = _scrollSpeed / num9 * Time.fixedDeltaTime;
			_currentJointDistance += y * num10;
			GrabDistanceData grabDistanceData = _grabDistanceConfiguration.GrabDistanceData[_currentGrabbables.First().GrabDistanceType];
			_currentJointDistance = Mathf.Clamp(_currentJointDistance, grabDistanceData.AdditionalMinJointDistance + _minJointDistance, grabDistanceData.AdditionalMaxJointDistance + _handsDistanceStat.FullValue);
			Vector3 position = _cameraModel.AimTransform.position;
			Vector3 vector = ((_grabPoint.transform.parent != null) ? _grabPoint.transform.parent.TransformPoint(_jointInitialLocalPosition) : _jointInitialLocalPosition) - position;
			Vector3 vector2 = vector - Vector3.Project(vector, forward);
			_grabPoint.transform.position = position + vector2 + forward * _currentJointDistance;
			_mouseScrollValue = Vector2.zero;
		}

		private void HandleGrabbableCleanup(IPointGrabable g)
		{
			if (_currentGrabbables.Contains(g))
			{
				UnJoin(g, throwItem: false);
			}
		}

		private void ResetThrowState()
		{
			_throwMultiplier = 0f;
			_isThrowStarted = false;
		}

		private void UpdateDisplayedItem()
		{
			if (_currentGrabbables.Count == 0)
			{
				_itemDamageDisplayModel.ClearItems();
				return;
			}
			List<MonoItem> list = new List<MonoItem>();
			foreach (IPointGrabable currentGrabbable in _currentGrabbables)
			{
				if (currentGrabbable.GameObject.TryGetComponent<MonoItem>(out var component))
				{
					list.Add(component);
				}
			}
			_itemDamageDisplayModel.OverrideList(list);
		}

		private void AccumulateMouseScroll(Vector2 delta)
		{
			_mouseScrollValue += delta;
		}

		private bool IsIgnoredRaycastCollider(Collider collider)
		{
			if (collider == null || _ignoredRaycastColliders == null || _ignoredRaycastColliders.Count == 0)
			{
				return false;
			}
			Transform transform = collider.transform;
			for (int i = 0; i < _ignoredRaycastColliders.Count; i++)
			{
				Collider collider2 = _ignoredRaycastColliders[i];
				if (!(collider2 == null))
				{
					if (collider2 == collider || collider2.gameObject == collider.gameObject)
					{
						return true;
					}
					if (transform.IsChildOf(collider2.transform) || collider2.transform.IsChildOf(transform))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddIgnoredRaycastCollider(Collider collider)
		{
			if (!(collider == null))
			{
				if (_ignoredRaycastColliders == null)
				{
					_ignoredRaycastColliders = new List<Collider>();
				}
				if (!_ignoredRaycastColliders.Contains(collider))
				{
					_ignoredRaycastColliders.Add(collider);
				}
			}
		}

		public void RemoveIgnoredRaycastCollider(Collider collider)
		{
			if (!(collider == null) && _ignoredRaycastColliders != null)
			{
				_ignoredRaycastColliders.Remove(collider);
			}
		}

		public void ClearIgnoredRaycastColliders()
		{
			_ignoredRaycastColliders?.Clear();
		}

		private IPointGrabable ResolveGrabableFrom(GameObject source)
		{
			if (source.TryGetComponent<PointGrabableContainer>(out var component))
			{
				return component.PointGrabable;
			}
			return FindComponent<IPointGrabable>(source);
		}

		private T FindComponent<T>(GameObject other)
		{
			T val = other.GetComponent<T>();
			if (val == null)
			{
				val = other.GetComponentInParent<T>();
			}
			if (val == null)
			{
				val = other.GetComponentInChildren<T>();
			}
			return val;
		}

		private void InvokeGrabbedChangedCallbacks()
		{
			OnGrabbedChanged?.Invoke();
			PublishLineArmGrabOrReleaseGameplayEvents();
		}

		private void PublishLineArmGrabOrReleaseGameplayEvents()
		{
			if (_gameplayEventBus != null)
			{
				int playerId = base.Object.InputAuthority.PlayerId;
				bool hasInputAuthority = base.Object.HasInputAuthority;
				string lineArmTypeName = ArmType.ToString();
				if (_currentGrabbables.Count == 0)
				{
					_gameplayEventBus.Publish(new OnItemGrabbedStateChangeEvent(playerId, hasInputAuthority, lineArmTypeName, null));
				}
				else
				{
					_gameplayEventBus.Publish(new OnItemGrabbedStateChangeEvent(playerId, hasInputAuthority, lineArmTypeName, _currentGrabbables[0].NetworkObject.name));
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(993184785u)]
		[Preserve]
		[WeaverGenerated]
		protected static void TryGrabRpc_0040Invoker993184785([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out NetworkId value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((LineArmControllerBase)context.TargetBehaviour).TryGrabRpc(value);
		}

		[NetworkRpcWeavedInvoker(1343587657u)]
		[Preserve]
		[WeaverGenerated]
		protected static void UnJoinRpc_0040Invoker1343587657([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out NetworkId value, 4);
			payloadReader.Read(out bool value2);
			payloadReader.Read(out Vector3Compressed value3, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((LineArmControllerBase)context.TargetBehaviour).UnJoinRpc(value, value2, value3);
		}
	}
}
