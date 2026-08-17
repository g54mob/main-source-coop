using System;
using System.Collections;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using ECM2;
using EvilCore;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using FIMSpace.FProceduralAnimation;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.Downed;
using NomadDrive.Features.Player.FirstPerson;
using NomadDrive.Features.Player.IK;
using NomadDrive.Features.Player.PlayerStateMachine;
using NomadDrive.Features.Vehicle;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.Vehicle.Parts.Doors;
using NomadDrive.Features.Vehicle.Parts.Seats;
using Steamworks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class Player : NetworkBehaviour, INetworkPlayer
	{
		private ISittable _currentSeat;

		private VehicleDoor _associatedDoor;

		private VehicleDoorSlot _associatedDoorSlot;

		[Tooltip("Stand-up ride hand-off: only force-engage the vehicle surface ride on stand-up if the vehicle is moving faster than this (km/h). On a PARKED vehicle the hand-off needlessly suspends the player's world NetworkTransform, which can freeze the player at the exit pose for remote observers until a jump. Keep well below SittableSurface.standUpMaxVehicleSpeedKmh.")]
		[SerializeField]
		private float surfaceRideHandoffMinSpeedKmh = 1.5f;

		[Header("Player Components")]
		[SerializeField]
		private GameObject cameraRootObject;

		[SerializeField]
		private GameObject characterModel;

		[Header("Respawn (Unstuck)")]
		[SerializeField]
		private float respawnSearchRadius = 40f;

		[SerializeField]
		private float respawnRingStep = 2f;

		[SerializeField]
		private float respawnVerticalProbe = 6f;

		[SerializeField]
		private float respawnMaxDrop = 24f;

		[Tooltip("Lift the respawn spot this far above the surface so the player drops in and settles, escaping thin embedding.")]
		[SerializeField]
		private float respawnGroundClearance = 0.5f;

		[Tooltip("Manual unstuck only: minimum distance the player is relocated, so a stuck-but-clear-column player is moved clear of the spot instead of re-placed on it. The fall-through guard ignores this.")]
		[SerializeField]
		private float respawnMinDisplacement = 5f;

		[Header("Spawn / Fall-through Safety")]
		[Tooltip("Terrain-only layer for the fall-through guard surface probe. Leave empty to resolve the \"Terrain\" layer by name.")]
		[SerializeField]
		private LayerMask terrainMask;

		[SerializeField]
		private float footingProbeDistance = 60f;

		[SerializeField]
		private float unfreezeGroundPollSeconds = 0.25f;

		[SerializeField]
		private float unfreezeMaxGroundWaitSeconds = 30f;

		[Tooltip("Absolute backstop: unfreeze the player this many seconds after spawn even if loading never reports complete — prevents a permanent frozen-in-air state on a stalled late join.")]
		[SerializeField]
		private float safetyUnfreezeHardCapSeconds = 90f;

		[SerializeField]
		private float fallthroughCheckInterval = 0.5f;

		[SerializeField]
		private float fallthroughDepthBelowSurface = 8f;

		[SerializeField]
		private float killplaneProbeHeight = 5000f;

		private PlayerComponentOrchestrator _orchestrator;

		private IPlayerService _playerService;

		private IGameUIManager _guiManager;

		private InteractionUI _interactionUI;

		private PlayerBodyVisibility _bodyVisibility;

		private DriverVehicleControlsInput _driverControls;

		[SyncVar(hook = "OnLegsAnimatorEnabledChanged")]
		private bool _isLegsAnimatorEnabled = true;

		private RemoteLegsAnimatorLOD _legsAnimatorLod;

		[SyncVar]
		private string _displayName;

		[SyncVar]
		private ushort _pingMs;

		[SyncVar]
		private string _eosProductUserId;

		[SyncVar(hook = "OnIsSpeakingChanged")]
		private bool _isSpeaking;

		private PlayerDeathController _deathController;

		private PlayerVehicleSurfaceAttachment _surfaceAttachment;

		private bool _isLocalPlayer;

		private IGameLoadingManager _loadingManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private ICastingManager _castingManager;

		private Rigidbody _localPlayerRigidbody;

		private Vector3 _pendingInitialSpawn;

		private Vector3 _pendingServerShift;

		private bool _hasPendingInitialSpawn;

		private bool _unfrozen;

		private LayerMask _resolvedTerrainMask;

		private bool _terrainMaskResolved;

		private bool _topDownCameraActive;

		private bool _isHudHidden;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isLegsAnimatorEnabled;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isSpeaking;

		[Header("Player Sitting Settings")]
		public UnityEvent<ISittable> OnPlayerSit { get; } = new UnityEvent<ISittable>();

		public UnityEvent OnPlayerStand { get; } = new UnityEvent();

		public UnityEvent OnPlayerSwapSeat { get; } = new UnityEvent();

		public bool LegsAnimatorSyncedEnabled => _isLegsAnimatorEnabled;

		public string DisplayName => _displayName;

		public ushort PingMs => _pingMs;

		public string EosProductUserId => _eosProductUserId;

		public bool IsSpeaking => _isSpeaking;

		public bool IsDowned
		{
			get
			{
				if (_deathController == null)
				{
					_deathController = GetComponent<PlayerDeathController>();
				}
				if (_deathController != null)
				{
					return _deathController.IsDowned;
				}
				return false;
			}
		}

		uint INetworkPlayer.NetId => base.netId;

		int INetworkPlayer.ConnectionId => base.connectionToClient?.connectionId ?? (-1);

		public Transform SeatedVehicleFocusTransform
		{
			get
			{
				VehicleManager currentVehicleManager = GetCurrentVehicleManager();
				if (!(currentVehicleManager != null))
				{
					return null;
				}
				return currentVehicleManager.transform;
			}
		}

		public Transform DownedFocusTransform
		{
			get
			{
				if (_deathController == null)
				{
					_deathController = GetComponent<PlayerDeathController>();
				}
				if (!(_deathController != null))
				{
					return null;
				}
				return _deathController.DownedFocusTransform;
			}
		}

		private LayerMask EffectiveTerrainMask
		{
			get
			{
				if (_terrainMaskResolved)
				{
					return _resolvedTerrainMask;
				}
				_resolvedTerrainMask = ((terrainMask.value != 0) ? terrainMask : ((LayerMask)LayerMask.GetMask("Terrain")));
				_terrainMaskResolved = true;
				return _resolvedTerrainMask;
			}
		}

		public bool Network_isLegsAnimatorEnabled
		{
			get
			{
				return _isLegsAnimatorEnabled;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isLegsAnimatorEnabled, 1uL, _Mirror_SyncVarHookDelegate__isLegsAnimatorEnabled);
			}
		}

		public string Network_displayName
		{
			get
			{
				return _displayName;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _displayName, 2uL, null);
			}
		}

		public ushort Network_pingMs
		{
			get
			{
				return _pingMs;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _pingMs, 4uL, null);
			}
		}

		public string Network_eosProductUserId
		{
			get
			{
				return _eosProductUserId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _eosProductUserId, 8uL, null);
			}
		}

		public bool Network_isSpeaking
		{
			get
			{
				return _isSpeaking;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isSpeaking, 16uL, _Mirror_SyncVarHookDelegate__isSpeaking);
			}
		}

		public event Action<bool> OnSpeakingChanged;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			if (cameraRootObject != null)
			{
				cameraRootObject.SetActive(value: false);
			}
			CharacterMovement component = GetComponent<CharacterMovement>();
			if (component != null)
			{
				component.enabled = false;
			}
			CapsuleCollider component2 = GetComponent<CapsuleCollider>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
		}

		[Inject]
		private void Construct(IPlayerService playerService, InteractionUI interactionDisplayer, IGameUIManager guiManager, IGameLoadingManager loadingManager)
		{
			_interactionUI = interactionDisplayer;
			_playerService = playerService;
			_guiManager = guiManager;
			_loadingManager = loadingManager;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			_networkManager?.RegisterPlayer(this);
			Setup();
		}

		public override void OnStopClient()
		{
			_networkManager?.UnregisterPlayer(this);
			if (_localPlayerRigidbody != null)
			{
				VehiclePlayerContactFilter.UnregisterPlayer(_localPlayerRigidbody);
				_localPlayerRigidbody = null;
			}
			base.OnStopClient();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			DownedPlayerRegistry.ServerRegisterPlayer(base.netId);
		}

		public override void OnStopServer()
		{
			if (base.connectionToClient != null)
			{
				SittableSurface.FreeSittablesOccupiedByConnection(base.connectionToClient.connectionId);
			}
			DownedPlayerRegistry.ServerUnregisterPlayer(base.netId);
			base.OnStopServer();
		}

		private void Setup()
		{
			_orchestrator = GetComponent<PlayerComponentOrchestrator>();
			if (_orchestrator == null)
			{
				_orchestrator = base.gameObject.AddComponent<PlayerComponentOrchestrator>();
			}
			_isLocalPlayer = base.isLocalPlayer;
			if (_isLocalPlayer)
			{
				SetupLocalPlayer();
			}
			else
			{
				SetupRemotePlayer();
			}
		}

		private void SetupLocalPlayer()
		{
			try
			{
				if (_playerService == null)
				{
					EvilLogger.LogError("PlayerService is null during initialization!", "SetupLocalPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 238);
					return;
				}
				if (cameraRootObject != null)
				{
					cameraRootObject.SetActive(value: true);
				}
				CharacterMovement component = GetComponent<CharacterMovement>();
				if (component != null)
				{
					component.enabled = true;
				}
				CapsuleCollider component2 = GetComponent<CapsuleCollider>();
				if (component2 != null)
				{
					component2.enabled = true;
				}
				OnPlayerSit.AddListener(OnPlayerSitActions);
				OnPlayerStand.AddListener(OnPlayerStandupActions);
				_loadingManager.SetState(20);
				_loadingManager.OnLoadingComplete.AddListener(UnfreezePlayer);
				_loadingManager.OnTimeout.AddListener(UnfreezePlayer);
				StartCoroutine(SafetyUnfreezeAfterTimeoutCoroutine());
				_playerService.Register(this);
				ActionMapsManager.ResetToDefaultGameplayMaps();
				_localPlayerRigidbody = GetComponent<Rigidbody>();
				if (component2 != null)
				{
					component2.hasModifiableContacts = true;
				}
				VehiclePlayerContactFilter.RegisterPlayer(_localPlayerRigidbody);
				_orchestrator.Setup(isLocalPlayer: true);
				_bodyVisibility = GetComponent<PlayerBodyVisibility>();
				_driverControls = GetComponent<DriverVehicleControlsInput>();
				_interactionUI.Init();
				WaitAndSetSteamName().Forget();
				StartCoroutine(FallthroughGuardLoop());
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("Error setting up local player: " + ex.Message + "\n" + ex.StackTrace, "SetupLocalPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 296);
			}
		}

		private void SetupRemotePlayer()
		{
			_orchestrator.Setup(isLocalPlayer: false);
		}

		private void Update()
		{
			if (!_isLocalPlayer)
			{
				return;
			}
			if (SittingInputs.IsSwitchCameraButtonDown())
			{
				if (!_topDownCameraActive)
				{
					SwitchTopDownCameraMode();
				}
				else
				{
					SwitchDefaultCameraMode();
				}
			}
			if (Input.GetKeyDown(KeyCode.J) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && !_guiManager.IsGameMenuOpen)
			{
				_isHudHidden = !_isHudHidden;
				if (_isHudHidden)
				{
					HideGameHUD();
				}
				else
				{
					RestoreGameHUD();
				}
			}
		}

		private void HideGameHUD()
		{
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.Crosshair);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.Game);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.Interaction);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.InfoMessage);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.InputActionPrompts);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.LiquidContainerInfo);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.AttachableObjectInfo);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.StandupActionPrompt);
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo);
		}

		private void RestoreGameHUD()
		{
			_guiManager?.SetCanvasVisibilityForGameStart();
		}

		private VehicleManager GetCurrentVehicleManager()
		{
			if (_currentSeat == null)
			{
				return null;
			}
			if (_currentSeat.VehicleSeatSlot == null)
			{
				return null;
			}
			return _currentSeat.VehicleSeatSlot.VehicleManager;
		}

		public bool IsRidingVehicle(VehicleManager vehicleManager)
		{
			if (vehicleManager != null)
			{
				return GetCurrentVehicleManager() == vehicleManager;
			}
			return false;
		}

		public bool IsMountedOnVehicle(VehicleManager vehicleManager)
		{
			if (vehicleManager == null || _currentSeat == null || !IsPlayerSitting())
			{
				return false;
			}
			if (_currentSeat.TryGetMountVehicle(out var vehicle))
			{
				return vehicle == vehicleManager;
			}
			return false;
		}

		public bool IsSurfaceRidingVehicle(uint vehicleNetId)
		{
			if (vehicleNetId == 0)
			{
				return false;
			}
			if (_surfaceAttachment == null)
			{
				_surfaceAttachment = GetComponent<PlayerVehicleSurfaceAttachment>();
			}
			if (_surfaceAttachment != null && _surfaceAttachment.IsAttached)
			{
				return _surfaceAttachment.AttachedVehicleNetId == vehicleNetId;
			}
			return false;
		}

		private void UnfreezePlayer()
		{
			_loadingManager?.OnLoadingComplete.RemoveListener(UnfreezePlayer);
			_loadingManager?.OnTimeout.RemoveListener(UnfreezePlayer);
			if (!_unfrozen)
			{
				_unfrozen = true;
				UnfreezeWhenGroundedAsync().Forget();
			}
		}

		private IEnumerator SafetyUnfreezeAfterTimeoutCoroutine()
		{
			yield return new WaitForSeconds(safetyUnfreezeHardCapSeconds);
			if (!_unfrozen)
			{
				UnfreezePlayer();
			}
		}

		private async UniTaskVoid UnfreezeWhenGroundedAsync()
		{
			TryApplyPendingInitialSpawn();
			float waited;
			for (waited = 0f; waited < unfreezeMaxGroundWaitSeconds; waited += unfreezeGroundPollSeconds)
			{
				if (this == null)
				{
					return;
				}
				if (HasGroundBeneathPlayer())
				{
					break;
				}
				await UniTask.Delay(TimeSpan.FromSeconds(unfreezeGroundPollSeconds), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
			if (!(this == null))
			{
				_ = waited;
				_ = unfreezeMaxGroundWaitSeconds;
				if (!IsDowned)
				{
					EnablePlayerControlAfterSpawn();
				}
			}
		}

		private bool HasGroundBeneathPlayer()
		{
			if (_castingManager == null)
			{
				return false;
			}
			if (!_playerService.TryGetCharacterMovement(out var movement))
			{
				return false;
			}
			Vector3 position = base.transform.position;
			CastRequest request = new CastRequest
			{
				Origin = null,
				Offset = position + Vector3.up * 0.5f,
				UseTransformForward = false,
				Direction = Vector3.down,
				Type = CastType.Ray,
				Distance = 0.5f + footingProbeDistance,
				LayerMask = movement.collisionLayers,
				TriggerInteraction = QueryTriggerInteraction.Ignore,
				MaxHits = 8
			};
			return _castingManager.CastImmediate(request).DidHit;
		}

		private void EnablePlayerControlAfterSpawn()
		{
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				movement.SetPositionAndRotation(base.transform.position, base.transform.rotation, updateGround: true);
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.SetGravityScale(1f);
				controller.EnableMovement();
				controller.EnableCameraRotate();
			}
			if (_playerService.TryGetInteractionManager(out var manager))
			{
				manager.EnableInteraction();
			}
		}

		private IEnumerator FallthroughGuardLoop()
		{
			WaitForSeconds wait = new WaitForSeconds(fallthroughCheckInterval);
			while (true)
			{
				yield return wait;
				if (!_isLocalPlayer)
				{
					break;
				}
				if (_unfrozen && !IsPlayerSitting() && !IsDowned)
				{
					TryRescueIfBelowWorld();
				}
			}
		}

		private void TryRescueIfBelowWorld()
		{
			if (_castingManager == null)
			{
				return;
			}
			LayerMask effectiveTerrainMask = EffectiveTerrainMask;
			if ((int)effectiveTerrainMask != 0)
			{
				Vector3 position = base.transform.position;
				CastRequest request = new CastRequest
				{
					Origin = null,
					Offset = new Vector3(position.x, position.y + killplaneProbeHeight, position.z),
					UseTransformForward = false,
					Direction = Vector3.down,
					Type = CastType.Ray,
					Distance = killplaneProbeHeight * 2f,
					LayerMask = effectiveTerrainMask,
					TriggerInteraction = QueryTriggerInteraction.Ignore,
					MaxHits = 8
				};
				CastResult castResult = _castingManager.CastImmediate(request);
				if (castResult.DidHit && !(position.y >= castResult.HitPoint.y - fallthroughDepthBelowSurface))
				{
					RescueToSurface(castResult.HitPoint);
				}
			}
		}

		private void RescueToSurface(Vector3 surfacePoint)
		{
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				Vector3 vector = surfacePoint + Vector3.up * 0.1f;
				LayerMask collisionLayers = movement.collisionLayers;
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
				if (!PlayerRespawnLocator.TryFindSafePosition(vector, _playerService.CapsuleCollider, collisionLayers, componentsInChildren, _castingManager, respawnSearchRadius, 0f, respawnRingStep, respawnVerticalProbe, respawnMaxDrop, respawnGroundClearance, out var result))
				{
					result = vector;
				}
				movement.SetPositionAndRotation(result, base.transform.rotation, updateGround: true);
				movement.velocity = Vector3.zero;
				movement.ClearAccumulatedForces();
			}
		}

		private void SwitchTopDownCameraMode()
		{
			if (_currentSeat != null && !(_currentSeat.VehicleSeatSlot == null))
			{
				GetCurrentVehicleManager().TopDownVehicleCamera.ActivateController();
				if (_playerService.TryGetFirstPersonController(out var controller))
				{
					controller.DeactivateCamera();
					controller.DisableCameraRotate();
					controller.ResetHeadRotation();
				}
				if (_playerService.TryGetInteractionManager(out var manager))
				{
					manager.DisableInteraction();
				}
				_topDownCameraActive = true;
				_bodyVisibility?.ShowAll();
			}
		}

		private void SwitchDefaultCameraMode()
		{
			if (_currentSeat != null && !(_currentSeat.VehicleSeatSlot == null))
			{
				GetCurrentVehicleManager().TopDownVehicleCamera.DeactivateController();
				if (_playerService.TryGetFirstPersonController(out var controller))
				{
					controller.ActivateCamera();
					controller.EnableCameraRotate();
				}
				if (_playerService.TryGetInteractionManager(out var manager))
				{
					manager.EnableInteraction();
				}
				_topDownCameraActive = false;
				_bodyVisibility?.HideForFirstPerson();
			}
		}

		public void SetPositionAndRotation(Vector3 pos, Vector3 rot = default(Vector3), bool snapToGround = false)
		{
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				bool updateGround = snapToGround && movement.isGrounded;
				movement.SetPositionAndRotation(pos, Quaternion.Euler(rot), updateGround);
			}
		}

		public void ShiftSeatedRootByDelta(Vector3 delta)
		{
			base.transform.position += delta;
		}

		public void RespawnUnstuck()
		{
			if (base.isOwned && !IsPlayerSitting() && _playerService.TryGetCharacterMovement(out var movement))
			{
				Vector3 position = base.transform.position;
				LayerMask collisionLayers = movement.collisionLayers;
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
				if (!PlayerRespawnLocator.TryFindSafePosition(position, _playerService.CapsuleCollider, collisionLayers, componentsInChildren, _castingManager, respawnSearchRadius, respawnMinDisplacement, respawnRingStep, respawnVerticalProbe, respawnMaxDrop, respawnGroundClearance, out var result))
				{
					result = position + Vector3.up * 2f;
				}
				SetPositionAndRotation(result);
				movement.velocity = Vector3.zero;
				movement.ClearAccumulatedForces();
			}
		}

		public void ServerApplyInitialSpawnNearHost(Vector3 position)
		{
			FloatingOriginManager instance = FloatingOriginManager.Instance;
			Vector3 serverShift = ((instance != null) ? instance.TotalShift : Vector3.zero);
			TargetApplyInitialSpawn(base.connectionToClient, position, serverShift);
		}

		[TargetRpc]
		private void TargetApplyInitialSpawn(NetworkConnectionToClient target, Vector3 position, Vector3 serverShift)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteVector3(serverShift);
			SendTargetRPCInternal(target, "System.Void NomadDrive.Features.Player.Player::TargetApplyInitialSpawn(Mirror.NetworkConnectionToClient,UnityEngine.Vector3,UnityEngine.Vector3)", -645736360, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void TryApplyPendingInitialSpawn()
		{
			if (_hasPendingInitialSpawn && _playerService != null && _playerService.TryGetCharacterMovement(out var _))
			{
				FloatingOriginManager instance = FloatingOriginManager.Instance;
				Vector3 pos = ((instance != null) ? (_pendingInitialSpawn + (instance.TotalShift - _pendingServerShift)) : _pendingInitialSpawn);
				SetPositionAndRotation(pos);
				_hasPendingInitialSpawn = false;
			}
		}

		public void ReSitAfterRevive(ISittable seat)
		{
			if (seat != null)
			{
				OnPlayerSit.Invoke(seat);
			}
		}

		private void OnPlayerSitActions(ISittable targetSeat)
		{
			if (_currentSeat != null && IsPlayerSitting())
			{
				SeatSwapActions(_currentSeat, targetSeat);
				return;
			}
			_currentSeat = targetSeat;
			ActionMapsManager.EnterSittingMode();
			if (_playerService.CapsuleCollider != null)
			{
				_playerService.CapsuleCollider.enabled = false;
			}
			ApplyCapsuleHeightForState(PlayerState.Sit);
			NetworkedTransform networkedTransform = targetSeat.NetworkedTransform;
			GetComponent<NetworkedTransform>().SetParent(networkedTransform, default(NetworkedTransformParentingConfig), networkedTransform.NetworkedTransformIndex);
			DisableLegsAnimator();
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.SetPlayerBehaviour(PlayerState.Sit);
				controller.IsSitting = true;
				controller.SetPlayerLookingCameraState(FirstPersonController.PlayerLookingCameraState.Sitting);
			}
			_guiManager.ShowCanvasGroup(GameCanvasGroupName.StandupActionPrompt, interactable: true, blockRaycast: true);
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				movement.SetPlaneConstraint(PlaneConstraint.ConstrainYAxis, Vector3.up);
			}
			DisableMovement();
			if (targetSeat.VehicleSeatSlot != null)
			{
				VehicleManager currentVehicleManager = GetCurrentVehicleManager();
				currentVehicleManager.DeactivateInteractablesForFrontSeatsTaken();
				if (targetSeat.IsDriverSeat)
				{
					ActionMapsManager.EnterDrivingMode();
					_driverControls?.BeginDriving(currentVehicleManager);
					if (currentVehicleManager != null)
					{
						NetworkedNWHVehicle component = currentVehicleManager.GetComponent<NetworkedNWHVehicle>();
						if (component != null)
						{
							CmdDriverEnterVehicle(component.netIdentity, base.connectionToClient);
						}
						else
						{
							EvilLogger.LogError("NetworkedNWHVehicle component not found on vehicle!", "OnPlayerSitActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 888);
						}
					}
				}
			}
			SetupAssociatedDoor(targetSeat.VehicleSeatSlot);
		}

		private void SeatSwapActions(ISittable oldSeat, ISittable newSeat)
		{
			UnsubscribeDoorEvents();
			VehicleManager currentVehicleManager = GetCurrentVehicleManager();
			oldSeat.SetOccupied(value: false);
			if (oldSeat.VehicleSeatSlot != null)
			{
				currentVehicleManager.ActivateInteractablesForFrontSeatsTaken();
				if (oldSeat.IsDriverSeat)
				{
					ActionMapsManager.ExitDrivingMode();
					_driverControls?.EndDriving();
					if (currentVehicleManager != null)
					{
						NetworkedNWHVehicle component = currentVehicleManager.GetComponent<NetworkedNWHVehicle>();
						if (component != null)
						{
							CmdDriverExitVehicle(component.netIdentity);
						}
					}
				}
			}
			newSeat.SetOccupied(value: true);
			_currentSeat = newSeat;
			NetworkedTransform networkedTransform = newSeat.NetworkedTransform;
			GetComponent<NetworkedTransform>().SetParent(networkedTransform, default(NetworkedTransformParentingConfig), networkedTransform.NetworkedTransformIndex);
			VehicleSeatSlot vehicleSeatSlot = newSeat.VehicleSeatSlot;
			if (vehicleSeatSlot != null)
			{
				VehicleManager vehicleManager = vehicleSeatSlot.VehicleManager;
				vehicleManager.DeactivateInteractablesForFrontSeatsTaken();
				if (newSeat.IsDriverSeat)
				{
					ActionMapsManager.EnterDrivingMode();
					_driverControls?.BeginDriving(vehicleManager);
					if (vehicleManager != null)
					{
						NetworkedNWHVehicle component2 = vehicleManager.GetComponent<NetworkedNWHVehicle>();
						if (component2 != null)
						{
							CmdDriverEnterVehicle(component2.netIdentity, base.connectionToClient);
						}
						else
						{
							EvilLogger.LogError("NetworkedNWHVehicle component not found on vehicle!", "SeatSwapActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 966);
						}
					}
				}
			}
			SetupAssociatedDoor(newSeat.VehicleSeatSlot);
		}

		private void OnPlayerStandupActions()
		{
			if (_currentSeat == null)
			{
				EvilLogger.LogError("Player standup failed: _seatOfPlayer is null", "OnPlayerStandupActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 984);
				return;
			}
			if (_currentSeat.CurrentStandingPoint == null)
			{
				EvilLogger.LogError("Player standup failed: StandingTransform is null on seat", "OnPlayerStandupActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 990);
				return;
			}
			if (_topDownCameraActive)
			{
				SwitchDefaultCameraMode();
			}
			if (_currentSeat.VehicleSeatSlot != null)
			{
				VehicleManager currentVehicleManager = GetCurrentVehicleManager();
				currentVehicleManager.ActivateInteractablesForFrontSeatsTaken();
				if (_currentSeat.IsDriverSeat)
				{
					ActionMapsManager.ExitDrivingMode();
					_driverControls?.EndDriving();
					if (currentVehicleManager != null)
					{
						NetworkedNWHVehicle component = currentVehicleManager.GetComponent<NetworkedNWHVehicle>();
						if (component != null)
						{
							CmdDriverExitVehicle(component.netIdentity);
						}
					}
				}
			}
			ActionMapsManager.ExitSittingMode();
			StartCoroutine(HandlePlayerStandupSequence());
		}

		private IEnumerator HandlePlayerStandupSequence()
		{
			GetComponent<NetworkedTransform>().SetParent(null, default(NetworkedTransformParentingConfig), 0);
			ParentConstraint parentConstraint = GetComponent<ParentConstraint>();
			if (parentConstraint != null)
			{
				while (parentConstraint.enabled && parentConstraint.constraintActive)
				{
					yield return null;
				}
			}
			yield return null;
			if (TryGetComponent<PlayerSittingIK>(out var component))
			{
				component.ReleaseSeatedIK();
			}
			Vector3 position = _currentSeat.CurrentStandingPoint.position;
			Vector3 euler = new Vector3(0f, _currentSeat.CurrentStandingPoint.rotation.eulerAngles.y, 0f);
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				movement.SetPositionAndRotation(position, Quaternion.Euler(euler));
				movement.SetPlaneConstraint(PlaneConstraint.None, Vector3.up);
			}
			_currentSeat.SetOccupied(value: false);
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.SetPlayerBehaviour(PlayerState.Idle);
				controller.IsSitting = false;
				controller.SetPlayerLookingCameraState(FirstPersonController.PlayerLookingCameraState.Standing);
			}
			ApplyCapsuleHeightForState(PlayerState.Idle);
			if (_playerService.CapsuleCollider != null)
			{
				_playerService.CapsuleCollider.enabled = true;
			}
			EnableMovement();
			EnableLegsAnimator();
			UnsubscribeDoorEvents();
			if (_playerService.TryGetEquipmentManager(out var manager) && manager.IsItemEquipped)
			{
				ActionMapsManager.EnterEquippingMode();
			}
			if (_currentSeat.TryGetMountVehicle(out var vehicle) && vehicle.TryGetComponent<NetworkedNWHVehicle>(out var component2) && TryGetComponent<PlayerVehicleSurfaceAttachment>(out var component3) && Mathf.Abs(vehicle.NetworkSync.VehicleSpeedKmh) > surfaceRideHandoffMinSpeedKmh)
			{
				component3.OwnerForceAttach(component2);
			}
			_currentSeat = null;
		}

		public void LeaveSeatForDowned()
		{
			if (_currentSeat == null)
			{
				return;
			}
			if (_topDownCameraActive)
			{
				SwitchDefaultCameraMode();
			}
			if (_currentSeat.VehicleSeatSlot != null)
			{
				VehicleManager currentVehicleManager = GetCurrentVehicleManager();
				currentVehicleManager.ActivateInteractablesForFrontSeatsTaken();
				if (_currentSeat.IsDriverSeat)
				{
					ActionMapsManager.ExitDrivingMode();
					_driverControls?.EndDriving();
					if (currentVehicleManager != null)
					{
						NetworkedNWHVehicle component = currentVehicleManager.GetComponent<NetworkedNWHVehicle>();
						if (component != null)
						{
							CmdDriverExitVehicle(component.netIdentity);
						}
					}
				}
			}
			ActionMapsManager.ExitSittingMode();
			UnsubscribeDoorEvents();
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.StandupActionPrompt);
			GetComponent<NetworkedTransform>().SetParent(null, default(NetworkedTransformParentingConfig), 0);
			if (TryGetComponent<PlayerSittingIK>(out var component2))
			{
				component2.ReleaseSeatedIK();
				component2.ClearSeatedIKNetworked();
			}
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				movement.SetPlaneConstraint(PlaneConstraint.None, Vector3.up);
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.SetPlayerBehaviour(PlayerState.Idle);
				controller.IsSitting = false;
				controller.SetPlayerLookingCameraState(FirstPersonController.PlayerLookingCameraState.Standing);
			}
			_currentSeat = null;
		}

		public ISittable GetPlayerSeat()
		{
			return _currentSeat;
		}

		public bool CanStandUpFromSeat()
		{
			if (_currentSeat != null)
			{
				return _currentSeat.CanStandUpNow();
			}
			return true;
		}

		public bool IsPlayerSitting()
		{
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				return controller.PlayerState.Equals(PlayerState.Sit);
			}
			return false;
		}

		private void EnableMovement()
		{
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.EnableMovement();
				controller.EnableJump();
				controller.EnableCharacterRotate();
				controller.EnableCrouch();
				controller.EnableSprint();
			}
		}

		private void ApplyCapsuleHeightForState(PlayerState state)
		{
			if (_playerService.TryGetFirstPersonController(out var controller) && !(controller.FirstPersonControllerSettings == null) && _playerService.TryGetCharacterMovement(out var movement))
			{
				float height = state switch
				{
					PlayerState.Sit => controller.FirstPersonControllerSettings.sittingCapsuleHeight, 
					PlayerState.CrouchedIdle => controller.FirstPersonControllerSettings.crouchedCapsuleHeight, 
					PlayerState.CrouchedWalk => controller.FirstPersonControllerSettings.crouchedCapsuleHeight, 
					PlayerState.CrouchedSprint => controller.FirstPersonControllerSettings.crouchedCapsuleHeight, 
					_ => controller.FirstPersonControllerSettings.standingCapsuleHeight, 
				};
				movement.SetHeight(height);
			}
		}

		private void DisableMovement()
		{
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.DisableMovement();
				controller.DisableJump();
				controller.DisableCharacterRotate();
				controller.DisableCrouch();
				controller.DisableSprint();
			}
		}

		private void SetupAssociatedDoor(VehicleSeatSlot seatSlot)
		{
			if (seatSlot == null)
			{
				return;
			}
			VehicleManager vehicleManager = seatSlot.VehicleManager;
			if (vehicleManager == null)
			{
				return;
			}
			VehicleSeatModule module = vehicleManager.GetModule<VehicleSeatModule>();
			VehicleDoorModule module2 = vehicleManager.GetModule<VehicleDoorModule>();
			if (module == null || module2 == null)
			{
				return;
			}
			if (seatSlot == module.FrontLeftSeatSlotRef)
			{
				_associatedDoorSlot = module2.LeftDoorSlotRef;
			}
			else
			{
				if (!(seatSlot == module.FrontRightSeatSlotRef))
				{
					return;
				}
				_associatedDoorSlot = module2.RightDoorSlotRef;
			}
			if (!(_associatedDoorSlot == null))
			{
				_associatedDoor = _associatedDoorSlot.AttachedDoor;
				SubscribeDoorEvents();
			}
		}

		private void SubscribeDoorEvents()
		{
			if (_associatedDoorSlot != null)
			{
				_associatedDoorSlot.OnDoorAttached.AddListener(OnAssociatedDoorAttached);
				_associatedDoorSlot.OnDoorDetached.AddListener(OnAssociatedDoorDetached);
			}
		}

		private void UnsubscribeDoorEvents()
		{
			if (_associatedDoorSlot != null)
			{
				_associatedDoorSlot.OnDoorAttached.RemoveListener(OnAssociatedDoorAttached);
				_associatedDoorSlot.OnDoorDetached.RemoveListener(OnAssociatedDoorDetached);
			}
			_associatedDoor = null;
			_associatedDoorSlot = null;
		}

		private void OnAssociatedDoorDetached()
		{
			_associatedDoor = null;
		}

		private void OnAssociatedDoorAttached(VehicleDoor door)
		{
			_associatedDoor = door;
		}

		public bool CanExitThroughAssociatedDoor()
		{
			if (_associatedDoor == null)
			{
				return true;
			}
			VehicleDoorState vehicleDoorState = _associatedDoor.VehicleDoorState;
			if (vehicleDoorState != VehicleDoorState.Opened)
			{
				return vehicleDoorState == VehicleDoorState.Detached;
			}
			return true;
		}

		[Command(requiresAuthority = false)]
		private void CmdDriverEnterVehicle(NetworkIdentity vehicleIdentity, NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(vehicleIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdDriverEnterVehicle(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", 1340789657, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdDriverExitVehicle(NetworkIdentity vehicleIdentity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(vehicleIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdDriverExitVehicle(Mirror.NetworkIdentity)", -1270382134, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcTeleport(Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			SendRPCInternal("System.Void NomadDrive.Features.Player.Player::RpcTeleport(UnityEngine.Vector3)", -788504844, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void OnLegsAnimatorEnabledChanged(bool oldValue, bool newValue)
		{
			if (!base.isLocalPlayer)
			{
				if (_legsAnimatorLod == null)
				{
					_legsAnimatorLod = GetComponentInChildren<RemoteLegsAnimatorLOD>(includeInactive: true);
				}
				if (_legsAnimatorLod != null)
				{
					_legsAnimatorLod.NotifySyncedEnabledChanged(newValue);
					return;
				}
			}
			LegsAnimator componentInChildren = GetComponentInChildren<LegsAnimator>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = newValue;
			}
			else
			{
				EvilLogger.LogError("LegsAnimator component not found in children when trying to change enabled state.", "OnLegsAnimatorEnabledChanged", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 1390);
			}
		}

		private void EnableLegsAnimator()
		{
			CmdEnableLegsAnimator();
		}

		private void DisableLegsAnimator()
		{
			CmdDisableLegsAnimator();
		}

		[Command(requiresAuthority = false)]
		private void CmdEnableLegsAnimator()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdEnableLegsAnimator()", 372871320, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdDisableLegsAnimator()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdDisableLegsAnimator()", 405180679, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSetLegsAnimator(bool enabledState)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Player::ServerSetLegsAnimator(System.Boolean)' called when server was not active");
			}
			else
			{
				Network_isLegsAnimatorEnabled = enabledState;
			}
		}

		private async UniTaskVoid WaitAndSetSteamName()
		{
			for (int i = 0; i < 30; i++)
			{
				if (SteamClient.IsValid)
				{
					CmdSetDisplayName(SteamClient.Name);
					return;
				}
				await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
			CmdSetDisplayName(null);
		}

		[Command]
		private void CmdSetDisplayName(string clientName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(clientName);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdSetDisplayName(System.String)", -471829052, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSetPing(ushort ping)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Player::ServerSetPing(System.UInt16)' called when server was not active");
			}
			else
			{
				Network_pingMs = ping;
			}
		}

		[Server]
		public void ServerSetEosProductUserId(string id)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Player::ServerSetEosProductUserId(System.String)' called when server was not active");
			}
			else
			{
				Network_eosProductUserId = id;
			}
		}

		[Command(requiresAuthority = true)]
		public void CmdSetSpeaking(bool speaking)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(speaking);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Player::CmdSetSpeaking(System.Boolean)", -27115972, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void OnIsSpeakingChanged(bool _, bool newValue)
		{
			this.OnSpeakingChanged?.Invoke(newValue);
		}

		private static string GenerateRandomPlayerName()
		{
			char c = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[UnityEngine.Random.Range(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)];
			char c2 = "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)];
			return $"Player {c}{c2}";
		}

		public Player()
		{
			_Mirror_SyncVarHookDelegate__isLegsAnimatorEnabled = OnLegsAnimatorEnabledChanged;
			_Mirror_SyncVarHookDelegate__isSpeaking = OnIsSpeakingChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_TargetApplyInitialSpawn__NetworkConnectionToClient__Vector3__Vector3(NetworkConnectionToClient target, Vector3 position, Vector3 serverShift)
		{
			_pendingInitialSpawn = position;
			_pendingServerShift = serverShift;
			_hasPendingInitialSpawn = true;
			TryApplyPendingInitialSpawn();
		}

		protected static void InvokeUserCode_TargetApplyInitialSpawn__NetworkConnectionToClient__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetApplyInitialSpawn called on server.");
			}
			else
			{
				((Player)obj).UserCode_TargetApplyInitialSpawn__NetworkConnectionToClient__Vector3__Vector3(null, reader.ReadVector3(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdDriverEnterVehicle__NetworkIdentity__NetworkConnectionToClient(NetworkIdentity vehicleIdentity, NetworkConnectionToClient conn)
		{
			if (vehicleIdentity == null)
			{
				EvilLogger.LogError("CmdDriverEnterVehicle: vehicleIdentity is null", "CmdDriverEnterVehicle", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 1325);
				return;
			}
			NetworkedNWHVehicle component = vehicleIdentity.GetComponent<NetworkedNWHVehicle>();
			if (component != null)
			{
				component.OnDriverEntered(conn);
			}
			else
			{
				EvilLogger.LogError("NetworkedNWHVehicle component not found!", "CmdDriverEnterVehicle", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 1337);
			}
		}

		protected static void InvokeUserCode_CmdDriverEnterVehicle__NetworkIdentity__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDriverEnterVehicle called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdDriverEnterVehicle__NetworkIdentity__NetworkConnectionToClient(reader.ReadNetworkIdentity(), senderConnection);
			}
		}

		protected void UserCode_CmdDriverExitVehicle__NetworkIdentity(NetworkIdentity vehicleIdentity)
		{
			if (vehicleIdentity == null)
			{
				EvilLogger.LogError("CmdDriverExitVehicle: vehicleIdentity is null", "CmdDriverExitVehicle", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 1346);
				return;
			}
			NetworkedNWHVehicle component = vehicleIdentity.GetComponent<NetworkedNWHVehicle>();
			if (component != null)
			{
				component.OnDriverExited();
			}
			else
			{
				EvilLogger.LogError("NetworkedNWHVehicle component not found!", "CmdDriverExitVehicle", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Player.cs", 1358);
			}
		}

		protected static void InvokeUserCode_CmdDriverExitVehicle__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDriverExitVehicle called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdDriverExitVehicle__NetworkIdentity(reader.ReadNetworkIdentity());
			}
		}

		protected void UserCode_RpcTeleport__Vector3(Vector3 position)
		{
		}

		protected static void InvokeUserCode_RpcTeleport__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcTeleport called on server.");
			}
			else
			{
				((Player)obj).UserCode_RpcTeleport__Vector3(reader.ReadVector3());
			}
		}

		protected void UserCode_CmdEnableLegsAnimator()
		{
			Network_isLegsAnimatorEnabled = true;
		}

		protected static void InvokeUserCode_CmdEnableLegsAnimator(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdEnableLegsAnimator called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdEnableLegsAnimator();
			}
		}

		protected void UserCode_CmdDisableLegsAnimator()
		{
			Network_isLegsAnimatorEnabled = false;
		}

		protected static void InvokeUserCode_CmdDisableLegsAnimator(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDisableLegsAnimator called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdDisableLegsAnimator();
			}
		}

		protected void UserCode_CmdSetDisplayName__String(string clientName)
		{
			if (!string.IsNullOrWhiteSpace(clientName))
			{
				Network_displayName = clientName;
			}
			else
			{
				Network_displayName = GenerateRandomPlayerName();
			}
		}

		protected static void InvokeUserCode_CmdSetDisplayName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetDisplayName called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdSetDisplayName__String(reader.ReadString());
			}
		}

		protected void UserCode_CmdSetSpeaking__Boolean(bool speaking)
		{
			Network_isSpeaking = speaking;
		}

		protected static void InvokeUserCode_CmdSetSpeaking__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSpeaking called on client.");
			}
			else
			{
				((Player)obj).UserCode_CmdSetSpeaking__Boolean(reader.ReadBool());
			}
		}

		static Player()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdDriverEnterVehicle(Mirror.NetworkIdentity,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdDriverEnterVehicle__NetworkIdentity__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdDriverExitVehicle(Mirror.NetworkIdentity)", InvokeUserCode_CmdDriverExitVehicle__NetworkIdentity, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdEnableLegsAnimator()", InvokeUserCode_CmdEnableLegsAnimator, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdDisableLegsAnimator()", InvokeUserCode_CmdDisableLegsAnimator, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdSetDisplayName(System.String)", InvokeUserCode_CmdSetDisplayName__String, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(Player), "System.Void NomadDrive.Features.Player.Player::CmdSetSpeaking(System.Boolean)", InvokeUserCode_CmdSetSpeaking__Boolean, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(Player), "System.Void NomadDrive.Features.Player.Player::RpcTeleport(UnityEngine.Vector3)", InvokeUserCode_RpcTeleport__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(Player), "System.Void NomadDrive.Features.Player.Player::TargetApplyInitialSpawn(Mirror.NetworkConnectionToClient,UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_TargetApplyInitialSpawn__NetworkConnectionToClient__Vector3__Vector3);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isLegsAnimatorEnabled);
				writer.WriteString(_displayName);
				writer.WriteUShort(_pingMs);
				writer.WriteString(_eosProductUserId);
				writer.WriteBool(_isSpeaking);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(_isLegsAnimatorEnabled);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteString(_displayName);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteUShort(_pingMs);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteString(_eosProductUserId);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				writer.WriteBool(_isSpeaking);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isLegsAnimatorEnabled, _Mirror_SyncVarHookDelegate__isLegsAnimatorEnabled, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _displayName, null, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _pingMs, null, reader.ReadUShort());
				GeneratedSyncVarDeserialize(ref _eosProductUserId, null, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _isSpeaking, _Mirror_SyncVarHookDelegate__isSpeaking, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isLegsAnimatorEnabled, _Mirror_SyncVarHookDelegate__isLegsAnimatorEnabled, reader.ReadBool());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _displayName, null, reader.ReadString());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _pingMs, null, reader.ReadUShort());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _eosProductUserId, null, reader.ReadString());
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isSpeaking, _Mirror_SyncVarHookDelegate__isSpeaking, reader.ReadBool());
			}
		}
	}
}
