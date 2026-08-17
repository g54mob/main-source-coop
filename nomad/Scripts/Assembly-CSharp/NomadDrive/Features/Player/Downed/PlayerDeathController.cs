using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore;
using EvilCore.Audio;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using EvilCore.Particles;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.Downed.Chicken;
using NomadDrive.Features.Player.FirstPerson;
using NomadDrive.Features.Player.PlayerStateMachine;
using NomadDrive.Features.Player.Survival;
using NomadDrive.Features.WorldGeneration;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.Downed
{
	public class PlayerDeathController : NetworkBehaviour, IPlayerComponent
	{
		[Header("Chicken")]
		[Tooltip("Networked Chicken prefab (root has ChickenForm : HeldItem). Spawned at the death spot.")]
		[SerializeField]
		private GameObject chickenPrefab;

		[Header("Spectator Camera")]
		[SerializeField]
		private ThirdPersonSpectatorCamera spectatorCamera;

		[Header("Transform / Revive FX (networked — all clients see/hear)")]
		[Tooltip("Particle burst when the player turns into a chicken (played at the death spot).")]
		[SerializeField]
		private ParticleKey transformParticle;

		[SerializeField]
		private SoundID transformSound;

		[Tooltip("Particle burst when the chicken is revived back into a human (played at the chicken's spot).")]
		[SerializeField]
		private ParticleKey reviveParticle;

		[SerializeField]
		private SoundID reviveSound;

		[Header("Last-Stand Auto-Respawn (whole party down → last-downed player comes back, no popup)")]
		[Tooltip("Seconds after the whole party is down before the last-downed player auto-respawns.")]
		[SerializeField]
		private float lastStandRespawnDelay = 3f;

		[Tooltip("Horizontal distance from the relocation origin for the respawn point (min..max metres).")]
		[SerializeField]
		private float lastStandMinAway = 40f;

		[SerializeField]
		private float lastStandMaxAway = 50f;

		[Tooltip("Drop height above the sampled terrain surface. MUST stay below the FallDamageConfig lethal fall height (~18m) so the landing is a heavy but NON-lethal penalty — otherwise the player re-downs on impact and loops.")]
		[SerializeField]
		private float lastStandDropHeight = 12f;

		[Tooltip("Random directions tried before falling back to an in-place revive (never under the world).")]
		[SerializeField]
		private int lastStandSpawnAttempts = 8;

		[Header("Debug")]
		[Tooltip("When OFF, total-party-down does NOT trigger the last-stand respawn — lets you observe the downed (chicken) state SOLO without auto-respawning. Leave ON for real play.")]
		[SerializeField]
		private bool enableTotalPartyDownGameOver = true;

		[SyncVar]
		private bool _reviveFullHealth;

		[SyncVar(hook = "OnIsDownedChanged")]
		private bool _isDowned;

		[SyncVar]
		private uint _chickenNetId;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IGameOverService _gameOverService;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IWorldGenerator _worldGenerator;

		[Inject]
		private INetworkedParticlesManager _networkedParticles;

		[Inject]
		private INetworkedAudioManager _networkedAudio;

		[Inject]
		private ICastingManager _castingManager;

		private Player _player;

		private PlayerBodyVisibility _bodyVisibility;

		private PlayerScreenFeedbackController _screenFeedback;

		private PlayerStatsManager _subscribedStats;

		private Renderer[] _bodyRenderers;

		private bool _isLocal;

		private bool _ready;

		private bool _lastStandScheduled;

		private Transform _chickenFocusTransform;

		private ChickenForm _chickenFocusForm;

		private uint _resolvedChickenFocusNetId;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isDowned;

		public int SetupPriority => 17;

		public bool IsDowned => _isDowned;

		public uint ChickenNetId => _chickenNetId;

		public Transform DownedFocusTransform
		{
			get
			{
				if (!_isDowned || _chickenNetId == 0)
				{
					return null;
				}
				ResolveChickenFocusRefs();
				if (_chickenFocusForm != null)
				{
					Transform ridingVehicleTransform = _chickenFocusForm.RidingVehicleTransform;
					if (ridingVehicleTransform != null)
					{
						return ridingVehicleTransform;
					}
				}
				return _chickenFocusTransform;
			}
		}

		private LootInterestManagement Aoi => NetworkServer.aoi as LootInterestManagement;

		public bool Network_reviveFullHealth
		{
			get
			{
				return _reviveFullHealth;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _reviveFullHealth, 1uL, null);
			}
		}

		public bool Network_isDowned
		{
			get
			{
				return _isDowned;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isDowned, 2uL, _Mirror_SyncVarHookDelegate__isDowned);
			}
		}

		public uint Network_chickenNetId
		{
			get
			{
				return _chickenNetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _chickenNetId, 4uL, null);
			}
		}

		private void ResolveChickenFocusRefs()
		{
			if (_resolvedChickenFocusNetId != _chickenNetId || !(_chickenFocusForm != null))
			{
				_chickenFocusTransform = null;
				_chickenFocusForm = null;
				_resolvedChickenFocusNetId = 0u;
				if (_networkManager != null && _networkManager.TryGetNetworkObjectById(_chickenNetId, out var networkObject) && networkObject != null)
				{
					_chickenFocusTransform = networkObject.transform;
					networkObject.TryGetComponent<ChickenForm>(out _chickenFocusForm);
					_resolvedChickenFocusNetId = _chickenNetId;
				}
			}
		}

		public void ReleaseFollowCameraParent()
		{
			if (spectatorCamera != null)
			{
				spectatorCamera.EmergencyUnparentCamera();
			}
		}

		public void ReleaseTerrainObserver()
		{
			_worldGenerator?.SetTerrainObserver(null);
		}

		public void SuspendDownedLook(bool suspended)
		{
			if (spectatorCamera != null)
			{
				spectatorCamera.SetLookEnabled(!suspended);
			}
		}

		private void Awake()
		{
			_player = GetComponent<Player>();
			_bodyVisibility = GetComponent<PlayerBodyVisibility>();
			_screenFeedback = GetComponent<PlayerScreenFeedbackController>();
			SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			MeshRenderer[] componentsInChildren2 = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			_bodyRenderers = new Renderer[componentsInChildren.Length + componentsInChildren2.Length];
			componentsInChildren.CopyTo(_bodyRenderers, 0);
			componentsInChildren2.CopyTo(_bodyRenderers, componentsInChildren.Length);
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocal = isLocalPlayer;
			base.enabled = true;
			if (isLocalPlayer && _playerService != null && _playerService.TryGetStatsManager(out var manager))
			{
				manager.OnDeath += OnLocalDeath;
				_subscribedStats = manager;
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (DownedPlayerRegistry.GameOver == null && _gameOverService != null)
			{
				DownedPlayerRegistry.GameOver = _gameOverService;
			}
			DownedPlayerRegistry.GameOverEnabled = enableTotalPartyDownGameOver;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			_ready = true;
			if (_isDowned)
			{
				ApplyDownedState(downed: true);
			}
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			Aoi?.ClearObserverCenter(base.connectionToClient);
			if (_chickenNetId != 0 && NetworkServer.spawned.TryGetValue(_chickenNetId, out var value) && value != null)
			{
				NetworkServer.Destroy(value.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (_subscribedStats != null)
			{
				_subscribedStats.OnDeath -= OnLocalDeath;
			}
		}

		private void OnLocalDeath()
		{
			if (!_isDowned && !(_playerService?.LocalPlayer == null))
			{
				Player localPlayer = _playerService.LocalPlayer;
				SittableSurface seat = null;
				if (localPlayer.IsPlayerSitting() && localPlayer.GetPlayerSeat() != null)
				{
					seat = localPlayer.GetPlayerSeat() as SittableSurface;
					localPlayer.LeaveSeatForDowned();
				}
				CmdGoDown(seat);
			}
		}

		[Command]
		private void CmdGoDown(SittableSurface seat)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkBehaviour(seat);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::CmdGoDown(NomadDrive.Features.Furnitures.SittableSurface)", 942104158, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerForceDowned()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerForceDowned()' called when server was not active");
			}
			else
			{
				ServerForceDowned(base.transform.position, base.transform.rotation);
			}
		}

		[Server]
		public void ServerForceDowned(Vector3 chickenPos, Quaternion chickenRot)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerForceDowned(UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
			}
			else if (!_isDowned)
			{
				Network_isDowned = true;
				DownedPlayerRegistry.ServerMarkDowned(base.netId);
				if (_player != null)
				{
					_player.ServerSetLegsAnimator(enabledState: false);
				}
				ServerSpawnChicken(null, chickenPos, chickenRot);
			}
		}

		[Server]
		private void ServerSpawnChicken(SittableSurface seat)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerSpawnChicken(NomadDrive.Features.Furnitures.SittableSurface)' called when server was not active");
			}
			else
			{
				ServerSpawnChicken(seat, base.transform.position, base.transform.rotation);
			}
		}

		[Server]
		private void ServerSpawnChicken(SittableSurface seat, Vector3 spawnPos, Quaternion spawnRot)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerSpawnChicken(NomadDrive.Features.Furnitures.SittableSurface,UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
				return;
			}
			if (chickenPrefab == null)
			{
				EvilLogger.LogError("[PlayerDeathController] chickenPrefab is not assigned — cannot spawn the downed chicken.", "ServerSpawnChicken", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Downed\\PlayerDeathController.cs", 320);
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(chickenPrefab, spawnPos, spawnRot);
			if (gameObject.TryGetComponent<ChickenForm>(out var component))
			{
				component.ServerInitController(base.netId);
				if (seat != null)
				{
					component.ServerInitSeat(seat);
				}
			}
			NetworkServer.Spawn(gameObject);
			Network_chickenNetId = gameObject.GetComponent<NetworkIdentity>().netId;
			Aoi?.SetObserverCenter(base.connectionToClient, gameObject.transform);
			if (seat != null && seat.NetworkedTransform != null && gameObject.TryGetComponent<NetworkedTransform>(out var component2))
			{
				NetworkedTransform networkedTransform = seat.NetworkedTransform;
				component2.ServerSetParent(networkedTransform, default(NetworkedTransformParentingConfig), networkedTransform.NetworkedTransformIndex);
			}
			else
			{
				_ = seat != null;
			}
			ServerPlayDownedFx(transformParticle, transformSound, gameObject.transform.position);
		}

		[Server]
		private void ServerPlayDownedFx(ParticleKey particle, SoundID sound, Vector3 position)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerPlayDownedFx(EvilCore.Particles.ParticleKey,Ami.BroAudio.SoundID,UnityEngine.Vector3)' called when server was not active");
				return;
			}
			if (particle.IsValid)
			{
				_networkedParticles?.PlayNetworkedOneShot(particle, position, Quaternion.identity);
			}
			if (sound.IsValid())
			{
				_networkedAudio?.PlayOneShot(sound, position);
			}
		}

		private void OnIsDownedChanged(bool _, bool newValue)
		{
			if (_ready)
			{
				ApplyDownedState(newValue);
			}
		}

		private void ApplyDownedState(bool downed)
		{
			SetBodyRenderersVisible(!downed);
			if (base.isOwned)
			{
				if (downed)
				{
					EnterDownedLocal();
				}
				else
				{
					ExitDownedLocal();
				}
			}
		}

		private void SetBodyRenderersVisible(bool visible)
		{
			if (_bodyRenderers == null)
			{
				return;
			}
			Renderer[] bodyRenderers = _bodyRenderers;
			foreach (Renderer renderer in bodyRenderers)
			{
				if (renderer != null)
				{
					renderer.enabled = visible;
				}
			}
		}

		private void EnterDownedLocal()
		{
			if (_playerService.TryGetEquipmentManager(out var manager) && manager.IsItemEquipped)
			{
				manager.DropInstant();
			}
			if (_playerService.TryGetObjectPlacementManager(out var manager2))
			{
				manager2.Reset();
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.DisableMovement();
				controller.DisableJump();
				controller.DisableCharacterRotate();
				controller.DisableCrouch();
				controller.DisableSprint();
				controller.DisableCameraRotate();
				controller.SetGravityScale(0f);
			}
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				movement.velocity = Vector3.zero;
				movement.ClearAccumulatedForces();
			}
			if (_playerService.CapsuleCollider != null)
			{
				_playerService.CapsuleCollider.enabled = false;
			}
			if (_playerService.TryGetInteractionManager(out var manager3))
			{
				manager3.DisableInteraction();
			}
			ActionMapsManager.EnterDownedMode();
			if (spectatorCamera == null)
			{
				spectatorCamera = GetComponentInChildren<ThirdPersonSpectatorCamera>(includeInactive: true) ?? base.gameObject.AddComponent<ThirdPersonSpectatorCamera>();
			}
			FirstPersonController controller2;
			Transform transform = (_playerService.TryGetFirstPersonController(out controller2) ? controller2.DrivenCameraTransform : null);
			if (transform != null)
			{
				controller2.ActivateCamera();
				spectatorCamera.Activate(base.transform, transform, controller2);
				FollowChickenAsync().Forget();
			}
			_subscribedStats?.SetDownedFormDisplay(active: true);
			_subscribedStats?.SetDeathFrozen(frozen: true);
			_screenFeedback?.SetFeedbackSuppressed(suppressed: true);
		}

		private async UniTaskVoid FollowChickenAsync()
		{
			if (spectatorCamera == null)
			{
				return;
			}
			int guard = 0;
			while (_chickenNetId == 0 && _isDowned && guard++ < 300)
			{
				await UniTask.Yield(this.GetCancellationTokenOnDestroy());
			}
			if (_isDowned && _chickenNetId != 0)
			{
				var (flag, gameObject) = await _networkManager.TryGetNetworkObjectByIdAsync(_chickenNetId, 50, 100, this.GetCancellationTokenOnDestroy());
				if (flag && gameObject != null && _isDowned)
				{
					spectatorCamera.SetTarget(gameObject.transform);
					_worldGenerator?.SetTerrainObserver(gameObject.transform);
				}
			}
		}

		private void ExitDownedLocal()
		{
			_chickenFocusTransform = null;
			_chickenFocusForm = null;
			_resolvedChickenFocusNetId = 0u;
			_subscribedStats?.SetDownedFormDisplay(active: false);
			if (_subscribedStats != null)
			{
				_subscribedStats.ReviveReset(_reviveFullHealth ? 1f : 0.5f);
			}
			_screenFeedback?.SetFeedbackSuppressed(suppressed: false);
			_worldGenerator?.SetTerrainObserver(null);
			if (spectatorCamera != null)
			{
				spectatorCamera.Deactivate();
				_bodyVisibility?.HideForFirstPerson();
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				if (controller.FirstPersonControllerSettings != null && _playerService.TryGetCharacterMovement(out var movement))
				{
					movement.SetHeight(controller.FirstPersonControllerSettings.standingCapsuleHeight);
				}
				if (_playerService.CapsuleCollider != null)
				{
					_playerService.CapsuleCollider.enabled = true;
				}
				controller.SetPlayerBehaviour(PlayerState.Idle);
				controller.SetGravityScale(1f);
				controller.EnableMovement();
				controller.EnableJump();
				controller.EnableCharacterRotate();
				controller.EnableCrouch();
				controller.EnableSprint();
				controller.EnableCameraRotate();
				controller.ActivateCamera();
			}
			if (_playerService.TryGetInteractionManager(out var manager))
			{
				manager.EnableInteraction();
			}
			ActionMapsManager.ExitDownedMode();
		}

		[Server]
		public void ServerReviveFromChicken(Vector3 pos, Quaternion rot, SittableSurface seat, bool fullHealth)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerReviveFromChicken(UnityEngine.Vector3,UnityEngine.Quaternion,NomadDrive.Features.Furnitures.SittableSurface,System.Boolean)' called when server was not active");
			}
			else if (_isDowned)
			{
				Network_reviveFullHealth = fullHealth;
				Network_isDowned = false;
				DownedPlayerRegistry.ServerMarkRevived(base.netId);
				if (_player != null)
				{
					_player.ServerSetLegsAnimator(enabledState: true);
				}
				Network_chickenNetId = 0u;
				Aoi?.ClearObserverCenter(base.connectionToClient);
				base.transform.SetPositionAndRotation(pos, rot);
				ServerPlayDownedFx(reviveParticle, reviveSound, pos);
				RpcReviveAt(pos, rot);
				TargetApplyReviveStats(base.connectionToClient, fullHealth);
				if (seat != null)
				{
					TargetReSeatAfterRevive(base.connectionToClient, seat);
				}
			}
		}

		[TargetRpc]
		private void TargetReSeatAfterRevive(NetworkConnectionToClient target, SittableSurface seat)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkBehaviour(seat);
			SendTargetRPCInternal(target, "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::TargetReSeatAfterRevive(Mirror.NetworkConnectionToClient,NomadDrive.Features.Furnitures.SittableSurface)", 894282743, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private async UniTaskVoid ReSeatNextFrameAsync(SittableSurface seat)
		{
			await UniTask.Yield(this.GetCancellationTokenOnDestroy());
			if (!_isDowned && !(seat == null) && _playerService?.LocalPlayer != null)
			{
				_playerService.LocalPlayer.ReSitAfterRevive(seat);
			}
		}

		[ClientRpc]
		private void RpcReviveAt(Vector3 pos, Quaternion rot)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			SendRPCInternal("System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::RpcReviveAt(UnityEngine.Vector3,UnityEngine.Quaternion)", -232505107, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetApplyReviveStats(NetworkConnectionToClient target, bool fullHealth)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(fullHealth);
			SendTargetRPCInternal(target, "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::TargetApplyReviveStats(Mirror.NetworkConnectionToClient,System.Boolean)", 1229955825, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerScheduleLastStandRespawn()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::ServerScheduleLastStandRespawn()' called when server was not active");
			}
			else if (_isDowned && !_lastStandScheduled)
			{
				_lastStandScheduled = true;
				ServerLastStandRespawnAsync().Forget();
			}
		}

		private async UniTaskVoid ServerLastStandRespawnAsync()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(lastStandRespawnDelay), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (!_isDowned)
			{
				_lastStandScheduled = false;
				return;
			}
			uint chickenNetId = _chickenNetId;
			Vector3 position = base.transform.position;
			if (chickenNetId != 0 && NetworkServer.spawned.TryGetValue(chickenNetId, out var value) && value != null)
			{
				position = value.transform.position;
			}
			if (!ServerTryFindFarSpawnAboveTerrain(position, out var spawnPos, out var spawnRot))
			{
				spawnPos = position;
				spawnRot = base.transform.rotation;
			}
			ServerReviveFromChicken(spawnPos, spawnRot, null, fullHealth: true);
			if (chickenNetId != 0 && NetworkServer.spawned.TryGetValue(chickenNetId, out var value2) && value2 != null)
			{
				NetworkServer.Destroy(value2.gameObject);
			}
			_lastStandScheduled = false;
		}

		[Server]
		private bool ServerTryFindFarSpawnAboveTerrain(Vector3 origin, out Vector3 spawnPos, out Quaternion spawnRot)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Boolean NomadDrive.Features.Player.Downed.PlayerDeathController::ServerTryFindFarSpawnAboveTerrain(UnityEngine.Vector3,UnityEngine.Vector3&,UnityEngine.Quaternion&)' called when server was not active");
				spawnPos = default(Vector3);
				spawnRot = default(Quaternion);
				return default(bool);
			}
			spawnPos = origin;
			spawnRot = base.transform.rotation;
			if (_castingManager == null)
			{
				return false;
			}
			LayerMask layerMask = LayerMask.GetMask("Terrain");
			if ((int)layerMask == 0)
			{
				return false;
			}
			for (int i = 0; i < lastStandSpawnAttempts; i++)
			{
				float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
				float num = UnityEngine.Random.Range(lastStandMinAway, lastStandMaxAway);
				Vector3 vector = origin + new Vector3(Mathf.Cos(f) * num, 0f, Mathf.Sin(f) * num);
				CastRequest request = new CastRequest
				{
					Origin = null,
					Offset = new Vector3(vector.x, vector.y + 500f, vector.z),
					UseTransformForward = false,
					Direction = Vector3.down,
					Type = CastType.Ray,
					Distance = 1000f,
					LayerMask = layerMask,
					TriggerInteraction = QueryTriggerInteraction.Ignore,
					MaxHits = 8
				};
				CastResult castResult = _castingManager.CastImmediate(request);
				if (castResult.DidHit)
				{
					spawnPos = new Vector3(vector.x, castResult.HitPoint.y + lastStandDropHeight, vector.z);
					Vector3 forward = origin - spawnPos;
					forward.y = 0f;
					spawnRot = ((forward.sqrMagnitude > 0.01f) ? Quaternion.LookRotation(forward) : base.transform.rotation);
					return true;
				}
			}
			return false;
		}

		public PlayerDeathController()
		{
			_Mirror_SyncVarHookDelegate__isDowned = OnIsDownedChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdGoDown__SittableSurface(SittableSurface seat)
		{
			if (!_isDowned)
			{
				Network_isDowned = true;
				DownedPlayerRegistry.ServerMarkDowned(base.netId);
				if (_player != null)
				{
					_player.ServerSetLegsAnimator(enabledState: false);
				}
				ServerSpawnChicken(seat);
			}
		}

		protected static void InvokeUserCode_CmdGoDown__SittableSurface(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdGoDown called on client.");
			}
			else
			{
				((PlayerDeathController)obj).UserCode_CmdGoDown__SittableSurface(reader.ReadNetworkBehaviour<SittableSurface>());
			}
		}

		protected void UserCode_TargetReSeatAfterRevive__NetworkConnectionToClient__SittableSurface(NetworkConnectionToClient target, SittableSurface seat)
		{
			ReSeatNextFrameAsync(seat).Forget();
		}

		protected static void InvokeUserCode_TargetReSeatAfterRevive__NetworkConnectionToClient__SittableSurface(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetReSeatAfterRevive called on server.");
			}
			else
			{
				((PlayerDeathController)obj).UserCode_TargetReSeatAfterRevive__NetworkConnectionToClient__SittableSurface(null, reader.ReadNetworkBehaviour<SittableSurface>());
			}
		}

		protected void UserCode_RpcReviveAt__Vector3__Quaternion(Vector3 pos, Quaternion rot)
		{
			if (base.isOwned && _playerService?.LocalPlayer != null)
			{
				_playerService.LocalPlayer.SetPositionAndRotation(pos, rot.eulerAngles);
			}
			else
			{
				base.transform.SetPositionAndRotation(pos, rot);
			}
		}

		protected static void InvokeUserCode_RpcReviveAt__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReviveAt called on server.");
			}
			else
			{
				((PlayerDeathController)obj).UserCode_RpcReviveAt__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_TargetApplyReviveStats__NetworkConnectionToClient__Boolean(NetworkConnectionToClient target, bool fullHealth)
		{
			if (_playerService != null && _playerService.TryGetStatsManager(out var manager))
			{
				manager.ReviveReset(fullHealth ? 1f : 0.5f);
			}
		}

		protected static void InvokeUserCode_TargetApplyReviveStats__NetworkConnectionToClient__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetApplyReviveStats called on server.");
			}
			else
			{
				((PlayerDeathController)obj).UserCode_TargetApplyReviveStats__NetworkConnectionToClient__Boolean(null, reader.ReadBool());
			}
		}

		static PlayerDeathController()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerDeathController), "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::CmdGoDown(NomadDrive.Features.Furnitures.SittableSurface)", InvokeUserCode_CmdGoDown__SittableSurface, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerDeathController), "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::RpcReviveAt(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcReviveAt__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerDeathController), "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::TargetReSeatAfterRevive(Mirror.NetworkConnectionToClient,NomadDrive.Features.Furnitures.SittableSurface)", InvokeUserCode_TargetReSeatAfterRevive__NetworkConnectionToClient__SittableSurface);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerDeathController), "System.Void NomadDrive.Features.Player.Downed.PlayerDeathController::TargetApplyReviveStats(Mirror.NetworkConnectionToClient,System.Boolean)", InvokeUserCode_TargetApplyReviveStats__NetworkConnectionToClient__Boolean);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_reviveFullHealth);
				writer.WriteBool(_isDowned);
				writer.WriteVarUInt(_chickenNetId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(_reviveFullHealth);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteBool(_isDowned);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteVarUInt(_chickenNetId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _reviveFullHealth, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isDowned, _Mirror_SyncVarHookDelegate__isDowned, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _chickenNetId, null, reader.ReadVarUInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _reviveFullHealth, null, reader.ReadBool());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isDowned, _Mirror_SyncVarHookDelegate__isDowned, reader.ReadBool());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _chickenNetId, null, reader.ReadVarUInt());
			}
		}
	}
}
