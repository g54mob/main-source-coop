using System;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Interaction.IK;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Networking;
using RootMotion.FinalIK;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.IK
{
	[DefaultExecutionOrder(-10)]
	public class PlayerIKBridge : NetworkBehaviour, IPlayerComponent
	{
		[Header("IK Components")]
		[SerializeField]
		private FullBodyBipedIK fbbik;

		[SerializeField]
		private InteractionSystem interactionSystem;

		[SyncVar(hook = "OnGripTargetChanged")]
		private uint _gripTargetNetId;

		[SyncVar(hook = "OnDominantHandChanged")]
		private byte _dominantHandByte;

		[SyncVar(hook = "OnHoverTargetChanged")]
		private ulong _hoverTargetPacked;

		[SyncVar(hook = "OnInteractionAnimActiveChanged")]
		private bool _isInteractionAnimActive;

		[SyncVar(hook = "OnGripPausedChanged")]
		private bool _isGripPaused;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private INetworkObjectSpawnWatcher _spawnWatcher;

		private const byte HoverType_None = 0;

		private const byte HoverType_Networked = 1;

		private const byte HoverType_Vehicle = 2;

		private InteractableIKSetup _activeSetup;

		private ResolvedHand _activeResolvedHand;

		private bool _isHoverInteractionActive;

		private bool _isPlayingInteractionAnim;

		private float _hoverReEngageTimer;

		private InteractableIKSetup _pendingReEngageSetup;

		private ResolvedHand _pendingReEngageHand;

		private bool _isGripping;

		private bool _gripUsesLeftHand;

		private bool _gripUsesRightHand;

		private bool _isLocalPlayer;

		private bool _isLateJoinCompleted;

		private PlayerSittingIK _sittingIK;

		public Action<uint, uint> _Mirror_SyncVarHookDelegate__gripTargetNetId;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__dominantHandByte;

		public Action<ulong, ulong> _Mirror_SyncVarHookDelegate__hoverTargetPacked;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isInteractionAnimActive;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isGripPaused;

		public int SetupPriority => 25;

		public DominantHand DominantHandSetting => (DominantHand)_dominantHandByte;

		public uint Network_gripTargetNetId
		{
			get
			{
				return _gripTargetNetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _gripTargetNetId, 1uL, _Mirror_SyncVarHookDelegate__gripTargetNetId);
			}
		}

		public byte Network_dominantHandByte
		{
			get
			{
				return _dominantHandByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _dominantHandByte, 2uL, _Mirror_SyncVarHookDelegate__dominantHandByte);
			}
		}

		public ulong Network_hoverTargetPacked
		{
			get
			{
				return _hoverTargetPacked;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _hoverTargetPacked, 4uL, _Mirror_SyncVarHookDelegate__hoverTargetPacked);
			}
		}

		public bool Network_isInteractionAnimActive
		{
			get
			{
				return _isInteractionAnimActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isInteractionAnimActive, 8uL, _Mirror_SyncVarHookDelegate__isInteractionAnimActive);
			}
		}

		public bool Network_isGripPaused
		{
			get
			{
				return _isGripPaused;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isGripPaused, 16uL, _Mirror_SyncVarHookDelegate__isGripPaused);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_isLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				SetForLateJoiner();
			}
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			if (_sittingIK == null)
			{
				_sittingIK = GetComponentInParent<PlayerSittingIK>(includeInactive: true);
			}
			if (isLocalPlayer)
			{
				SubscribeToLocalEvents();
				if (fbbik != null)
				{
					fbbik.enabled = true;
				}
				if (interactionSystem != null)
				{
					interactionSystem.enabled = true;
				}
			}
			else
			{
				if (fbbik != null)
				{
					fbbik.enabled = false;
				}
				if (interactionSystem != null)
				{
					interactionSystem.enabled = false;
				}
			}
			base.enabled = true;
		}

		private void EnsureRemoteSolversEnabled()
		{
			if (!_isLocalPlayer)
			{
				if (fbbik != null && !fbbik.enabled)
				{
					fbbik.enabled = true;
				}
				if (interactionSystem != null && !interactionSystem.enabled)
				{
					interactionSystem.enabled = true;
				}
			}
		}

		private void RefreshRemoteIKSolversState()
		{
			if (!_isLocalPlayer)
			{
				bool flag = _isHoverInteractionActive || _isPlayingInteractionAnim || _isGripping || _hoverReEngageTimer > 0f || (_sittingIK != null && _sittingIK.RequiresRemoteSolvers);
				if (fbbik != null && fbbik.enabled != flag)
				{
					fbbik.enabled = flag;
				}
				if (interactionSystem != null && interactionSystem.enabled != flag)
				{
					interactionSystem.enabled = flag;
				}
			}
		}

		private void SetForLateJoiner()
		{
			if (_gripTargetNetId != 0 || _hoverTargetPacked != 0L || _isInteractionAnimActive)
			{
				EnsureRemoteSolversEnabled();
			}
			if (_gripTargetNetId != 0)
			{
				RegisterGripSpawnWatcher(_gripTargetNetId);
			}
			if (_hoverTargetPacked != 0L)
			{
				ApplyHoverFromPacked(_hoverTargetPacked);
				StartHoverInteraction();
			}
			if (_isInteractionAnimActive && _activeResolvedHand.IsValid)
			{
				StopHoverInteraction();
				StartInteractionAnimFromResolvedHand();
			}
			_isLateJoinCompleted = true;
			RefreshRemoteIKSolversState();
		}

		private void RegisterGripSpawnWatcher(uint gripNetId)
		{
			if (_spawnWatcher != null)
			{
				_spawnWatcher.RegisterPendingLookup(gripNetId, OnGripObjectSpawned, "PlayerIKBridge.Grip");
			}
		}

		private void OnGripObjectSpawned(GameObject gripObject)
		{
			if (!(gripObject == null) && gripObject.TryGetComponent<HeldItemIKSetup>(out var component) && !(interactionSystem == null))
			{
				if (_isGripPaused)
				{
					_isGripping = true;
					TrackGripHands(component);
				}
				else
				{
					StartGrip(component);
				}
			}
		}

		private void SubscribeToLocalEvents()
		{
			IInteractionManager interactionManager = _playerService?.InteractionManager;
			if (interactionManager != null)
			{
				interactionManager.OnInteractableHovered += OnInteractableHovered;
				interactionManager.OnInteractableUnhovered += OnInteractableUnhovered;
				interactionManager.OnInteractionAnimTriggered += OnInteractionAnimTriggered;
			}
			IEquipmentManager equipmentManager = _playerService?.EquipmentManager;
			if (equipmentManager != null)
			{
				equipmentManager.OnItemEquipped.AddListener(OnLocalItemEquipped);
				equipmentManager.OnItemUnequipped.AddListener(OnLocalItemUnequipped);
				equipmentManager.OnGripPauseRequested.AddListener(OnLocalGripPauseRequested);
				equipmentManager.OnGripResumeRequested.AddListener(OnLocalGripResumeRequested);
			}
			else
			{
				EvilLogger.LogError("equipment manager not found!!", "SubscribeToLocalEvents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\IK\\Scripts\\PlayerIKBridge.cs", 247);
			}
		}

		private void UnsubscribeFromLocalEvents()
		{
			IInteractionManager interactionManager = _playerService?.InteractionManager;
			if (interactionManager != null)
			{
				interactionManager.OnInteractableHovered -= OnInteractableHovered;
				interactionManager.OnInteractableUnhovered -= OnInteractableUnhovered;
				interactionManager.OnInteractionAnimTriggered -= OnInteractionAnimTriggered;
			}
			IEquipmentManager equipmentManager = _playerService?.EquipmentManager;
			if (equipmentManager != null)
			{
				equipmentManager.OnItemEquipped.RemoveListener(OnLocalItemEquipped);
				equipmentManager.OnItemUnequipped.RemoveListener(OnLocalItemUnequipped);
				equipmentManager.OnGripPauseRequested.RemoveListener(OnLocalGripPauseRequested);
				equipmentManager.OnGripResumeRequested.RemoveListener(OnLocalGripResumeRequested);
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSetDominantHand(byte hand)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, hand);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetDominantHand(System.Byte)", 193561126, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnDominantHandChanged(byte oldValue, byte newValue)
		{
			if (_isLateJoinCompleted && _activeSetup != null)
			{
				StopHoverInteraction();
				_activeResolvedHand = ResolveHand(_activeSetup, _activeSetup.transform);
				StartHoverInteraction();
			}
		}

		private bool ShouldActivateInteractableIK(GameObject target)
		{
			if (!target.TryGetComponent<AttachableObject>(out var component))
			{
				return true;
			}
			return component.IsAttached;
		}

		private ResolvedHand ResolveHand(InteractableIKSetup setup, Transform interactableTransform)
		{
			return setup.HandSelection switch
			{
				IKHandSelection.None => ResolvedHand.Invalid, 
				IKHandSelection.LeftHand => new ResolvedHand(FullBodyBipedEffector.LeftHand, setup.LeftHandInteractionObject), 
				IKHandSelection.RightHand => new ResolvedHand(FullBodyBipedEffector.RightHand, setup.RightHandInteractionObject), 
				IKHandSelection.BothHands => new ResolvedHand(FullBodyBipedEffector.LeftHand, setup.LeftHandInteractionObject, FullBodyBipedEffector.RightHand, setup.RightHandInteractionObject), 
				IKHandSelection.DominantHand => ResolveDominantHand(setup), 
				IKHandSelection.AutoSelect => ResolveAutoSelect(setup, interactableTransform), 
				_ => ResolvedHand.Invalid, 
			};
		}

		private ResolvedHand ResolveDominantHand(InteractableIKSetup setup)
		{
			if (DominantHandSetting == DominantHand.Left)
			{
				return new ResolvedHand(FullBodyBipedEffector.LeftHand, setup.LeftHandInteractionObject);
			}
			return new ResolvedHand(FullBodyBipedEffector.RightHand, setup.RightHandInteractionObject);
		}

		private ResolvedHand ResolveAutoSelect(InteractableIKSetup setup, Transform interactableTransform)
		{
			FullBodyBipedEffector effector = ((base.transform.InverseTransformPoint(interactableTransform.position).x >= 0f) ? FullBodyBipedEffector.RightHand : FullBodyBipedEffector.LeftHand);
			InteractionObject interactionObjectForEffector = setup.GetInteractionObjectForEffector(effector);
			return new ResolvedHand(effector, interactionObjectForEffector);
		}

		private static ulong PackHover(byte type, uint id, byte subId = 0)
		{
			return ((ulong)type << 56) | ((ulong)subId << 32) | id;
		}

		private static void UnpackHover(ulong packed, out byte type, out uint id, out byte subId)
		{
			type = (byte)(packed >> 56);
			id = (uint)(packed & 0xFFFFFFFFu);
			subId = (byte)((packed >> 32) & 0xFF);
		}

		private ulong ResolveHoverTargetPacked(IInteractable interactable)
		{
			if (interactable is VehicleInteractable vehicleInteractable)
			{
				NetworkIdentity networkIdentity = vehicleInteractable.VehicleManager?.GetComponent<NetworkIdentity>();
				if (networkIdentity != null)
				{
					return PackHover(2, networkIdentity.netId, vehicleInteractable.NetworkKey);
				}
				return 0uL;
			}
			if (interactable.gameObject.TryGetComponent<NetworkIdentity>(out var component))
			{
				return PackHover(1, component.netId, 0);
			}
			return 0uL;
		}

		private void OnInteractableHovered(IInteractable interactable)
		{
			if (interactable.gameObject.TryGetComponent<InteractableIKSetup>(out var component) && ShouldActivateInteractableIK(interactable.gameObject))
			{
				_activeSetup = component;
				_activeResolvedHand = ResolveHand(component, interactable.transform);
				StartHoverInteraction();
				CmdSetHoverTarget(ResolveHoverTargetPacked(interactable));
			}
		}

		private void OnInteractableUnhovered(IInteractable interactable)
		{
			ForceStopAllHoverIK();
			CmdSetHoverTarget(0uL);
		}

		private void StartHoverInteraction()
		{
			if (_activeResolvedHand.IsValid && !(interactionSystem == null))
			{
				if (_activeSetup != null)
				{
					interactionSystem.speed = _activeSetup.InteractionSpeed;
				}
				if (_activeResolvedHand.PrimaryInteractionObject != null)
				{
					interactionSystem.StartInteraction(_activeResolvedHand.PrimaryEffector, _activeResolvedHand.PrimaryInteractionObject, interrupt: true);
				}
				if (_activeResolvedHand.UseBothHands && _activeResolvedHand.SecondaryInteractionObject != null)
				{
					interactionSystem.StartInteraction(_activeResolvedHand.SecondaryEffector, _activeResolvedHand.SecondaryInteractionObject, interrupt: true);
				}
				_isHoverInteractionActive = true;
			}
		}

		private void StopHoverInteraction()
		{
			if (_isHoverInteractionActive && !(interactionSystem == null))
			{
				interactionSystem.StopInteraction(_activeResolvedHand.PrimaryEffector);
				if (_activeResolvedHand.UseBothHands)
				{
					interactionSystem.StopInteraction(_activeResolvedHand.SecondaryEffector);
				}
				interactionSystem.speed = 1f;
				_isHoverInteractionActive = false;
			}
		}

		private void ForceStopAllHoverIK()
		{
			if (interactionSystem != null && _activeResolvedHand.IsValid)
			{
				interactionSystem.StopInteraction(_activeResolvedHand.PrimaryEffector);
				if (_activeResolvedHand.UseBothHands)
				{
					interactionSystem.StopInteraction(_activeResolvedHand.SecondaryEffector);
				}
				interactionSystem.speed = 1f;
			}
			_isHoverInteractionActive = false;
			_isPlayingInteractionAnim = false;
			_activeSetup = null;
			_activeResolvedHand = ResolvedHand.Invalid;
			_pendingReEngageSetup = null;
			_hoverReEngageTimer = 0f;
		}

		[Command(requiresAuthority = false)]
		private void CmdSetHoverTarget(ulong packed)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarULong(packed);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetHoverTarget(System.UInt64)", 1852958158, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnHoverTargetChanged(ulong oldValue, ulong newValue)
		{
			if (_isLateJoinCompleted && !_isLocalPlayer)
			{
				if (newValue == 0L)
				{
					ForceStopAllHoverIK();
					RefreshRemoteIKSolversState();
					return;
				}
				EnsureRemoteSolversEnabled();
				StopHoverInteraction();
				ApplyHoverFromPacked(newValue);
				StartHoverInteraction();
			}
		}

		private void ApplyHoverFromPacked(ulong packed)
		{
			UnpackHover(packed, out var type, out var id, out var subId);
			switch (type)
			{
			case 1:
				ApplyHoverFromNetworked(id);
				break;
			case 2:
				ApplyHoverFromVehicle(id, subId);
				break;
			}
		}

		private void ApplyHoverFromNetworked(uint netId)
		{
			if (_networkManager.TryGetNetworkObjectById(netId, out var networkObject) && networkObject.TryGetComponent<InteractableIKSetup>(out var component) && ShouldActivateInteractableIK(networkObject))
			{
				_activeSetup = component;
				_activeResolvedHand = ResolveHand(component, networkObject.transform);
			}
		}

		private void ApplyHoverFromVehicle(uint vehicleNetId, byte interactableId)
		{
			if (_networkManager.TryGetNetworkObjectById(vehicleNetId, out var networkObject) && networkObject.TryGetComponent<VehicleNetworkSync>(out var component) && component.TryGetInteractable(interactableId, out var interactable) && interactable.TryGetComponent<InteractableIKSetup>(out var component2))
			{
				_activeSetup = component2;
				_activeResolvedHand = ResolveHand(component2, interactable.transform);
			}
		}

		private void OnInteractionAnimTriggered(IInteractable interactable)
		{
			if (!(_activeSetup == null) && _activeSetup.PlayAnimationOnInteract && _activeResolvedHand.IsValid && !(interactionSystem == null))
			{
				if (_isHoverInteractionActive)
				{
					ResumeHoverForInteractionAnim();
				}
				else
				{
					StartInteractionAnimFromResolvedHand();
				}
				_isPlayingInteractionAnim = true;
				CmdSetInteractionAnimActive(active: true);
			}
		}

		private void ResumeHoverForInteractionAnim()
		{
			interactionSystem.ResumeInteraction(_activeResolvedHand.PrimaryEffector);
			if (_activeResolvedHand.UseBothHands)
			{
				interactionSystem.ResumeInteraction(_activeResolvedHand.SecondaryEffector);
			}
			_isHoverInteractionActive = false;
			_isPlayingInteractionAnim = true;
		}

		[Command(requiresAuthority = false)]
		private void CmdSetInteractionAnimActive(bool active)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(active);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetInteractionAnimActive(System.Boolean)", -790717746, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnInteractionAnimActiveChanged(bool oldValue, bool newValue)
		{
			if (!_isLateJoinCompleted || _isLocalPlayer)
			{
				return;
			}
			if (newValue)
			{
				EnsureRemoteSolversEnabled();
				if (_isHoverInteractionActive)
				{
					ResumeHoverForInteractionAnim();
				}
				else
				{
					StartInteractionAnimFromResolvedHand();
				}
				return;
			}
			_isPlayingInteractionAnim = false;
			if (_activeSetup != null && _activeResolvedHand.IsValid)
			{
				_pendingReEngageSetup = _activeSetup;
				_pendingReEngageHand = _activeResolvedHand;
				_hoverReEngageTimer = _activeSetup.HoverReEngageDelay;
			}
			_activeSetup = null;
			_activeResolvedHand = ResolvedHand.Invalid;
			RefreshRemoteIKSolversState();
		}

		private void StartInteractionAnimFromResolvedHand()
		{
			if (!_activeResolvedHand.IsValid || interactionSystem == null)
			{
				return;
			}
			_isPlayingInteractionAnim = true;
			if (_activeResolvedHand.UseBothHands)
			{
				if (_activeResolvedHand.PrimaryInteractionObject != null)
				{
					interactionSystem.StartInteraction(_activeResolvedHand.PrimaryEffector, _activeResolvedHand.PrimaryInteractionObject, interrupt: true);
				}
				if (_activeResolvedHand.SecondaryInteractionObject != null)
				{
					interactionSystem.StartInteraction(_activeResolvedHand.SecondaryEffector, _activeResolvedHand.SecondaryInteractionObject, interrupt: true);
				}
			}
			else if (_activeResolvedHand.PrimaryInteractionObject != null)
			{
				interactionSystem.StartInteraction(_activeResolvedHand.PrimaryEffector, _activeResolvedHand.PrimaryInteractionObject, interrupt: true);
			}
		}

		private void OnLocalItemEquipped()
		{
			HeldItem equippedEntity = _playerService.EquipmentManager.EquippedEntity;
			if (!(equippedEntity == null))
			{
				NetworkIdentity component = equippedEntity.GetComponent<NetworkIdentity>();
				if (!(component == null))
				{
					CmdSetGripTarget(component.netId);
				}
			}
		}

		private void OnLocalItemUnequipped()
		{
			if (_isGripping)
			{
				StopGrip();
			}
			CmdSetGripTarget(0u);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetGripTarget(uint targetNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(targetNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetGripTarget(System.UInt32)", -907838209, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnGripTargetChanged(uint oldValue, uint newValue)
		{
			if (!_isLateJoinCompleted || interactionSystem == null)
			{
				return;
			}
			if (newValue == 0)
			{
				StopGrip();
				RefreshRemoteIKSolversState();
				return;
			}
			EnsureRemoteSolversEnabled();
			if (_networkManager.TryGetNetworkObjectById(newValue, out var networkObject) && networkObject.TryGetComponent<HeldItemIKSetup>(out var component))
			{
				StartGrip(component);
			}
		}

		private void StartGrip(HeldItemIKSetup gripData)
		{
			_gripUsesLeftHand = false;
			_gripUsesRightHand = false;
			switch (gripData.GripHandSelection)
			{
			case IKHandSelection.None:
				return;
			case IKHandSelection.LeftHand:
				if (gripData.LeftHandInteraction != null)
				{
					interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, gripData.LeftHandInteraction, interrupt: true);
					_gripUsesLeftHand = true;
				}
				break;
			case IKHandSelection.RightHand:
				if (gripData.RightHandInteraction != null)
				{
					interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, gripData.RightHandInteraction, interrupt: true);
					_gripUsesRightHand = true;
				}
				break;
			case IKHandSelection.DominantHand:
				if (DominantHandSetting == DominantHand.Left)
				{
					if (gripData.LeftHandInteraction != null)
					{
						interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, gripData.LeftHandInteraction, interrupt: true);
						_gripUsesLeftHand = true;
					}
				}
				else if (gripData.RightHandInteraction != null)
				{
					interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, gripData.RightHandInteraction, interrupt: true);
					_gripUsesRightHand = true;
				}
				break;
			default:
				if (gripData.LeftHandInteraction != null)
				{
					interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, gripData.LeftHandInteraction, interrupt: true);
					_gripUsesLeftHand = true;
				}
				if (gripData.RightHandInteraction != null)
				{
					interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, gripData.RightHandInteraction, interrupt: true);
					_gripUsesRightHand = true;
				}
				break;
			}
			_isGripping = true;
		}

		private void TrackGripHands(HeldItemIKSetup gripData)
		{
			_gripUsesLeftHand = false;
			_gripUsesRightHand = false;
			switch (gripData.GripHandSelection)
			{
			case IKHandSelection.LeftHand:
				_gripUsesLeftHand = gripData.LeftHandInteraction != null;
				break;
			case IKHandSelection.RightHand:
				_gripUsesRightHand = gripData.RightHandInteraction != null;
				break;
			case IKHandSelection.DominantHand:
				if (DominantHandSetting == DominantHand.Left)
				{
					_gripUsesLeftHand = gripData.LeftHandInteraction != null;
				}
				else
				{
					_gripUsesRightHand = gripData.RightHandInteraction != null;
				}
				break;
			default:
				_gripUsesLeftHand = gripData.LeftHandInteraction != null;
				_gripUsesRightHand = gripData.RightHandInteraction != null;
				break;
			}
		}

		private void StopGrip()
		{
			if (interactionSystem != null)
			{
				if (_gripUsesLeftHand)
				{
					interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
				}
				if (_gripUsesRightHand)
				{
					interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
				}
			}
			_isGripping = false;
			_gripUsesLeftHand = false;
			_gripUsesRightHand = false;
		}

		private void OnLocalGripPauseRequested()
		{
			CmdSetGripPaused(paused: true);
		}

		private void OnLocalGripResumeRequested()
		{
			CmdSetGripPaused(paused: false);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetGripPaused(bool paused)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(paused);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetGripPaused(System.Boolean)", -1311613535, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnGripPausedChanged(bool oldValue, bool newValue)
		{
			if (_isLateJoinCompleted && _isGripping)
			{
				if (newValue)
				{
					PauseGrip();
					return;
				}
				EnsureRemoteSolversEnabled();
				ResumeGrip();
			}
		}

		private void PauseGrip()
		{
			if (!(interactionSystem == null))
			{
				if (_gripUsesLeftHand)
				{
					interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
				}
				if (_gripUsesRightHand)
				{
					interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
				}
			}
		}

		private void ResumeGrip()
		{
			if (!(interactionSystem == null) && _gripTargetNetId != 0 && _networkManager.TryGetNetworkObjectById(_gripTargetNetId, out var networkObject) && networkObject.TryGetComponent<HeldItemIKSetup>(out var component))
			{
				StartGrip(component);
			}
		}

		private void LateUpdate()
		{
			if (_isGripping && !_isGripPaused)
			{
				return;
			}
			if (_hoverReEngageTimer > 0f)
			{
				_hoverReEngageTimer -= Time.deltaTime;
				if (_hoverReEngageTimer <= 0f && _pendingReEngageSetup != null)
				{
					EnsureRemoteSolversEnabled();
					_activeSetup = _pendingReEngageSetup;
					_activeResolvedHand = _pendingReEngageHand;
					_pendingReEngageSetup = null;
					StartHoverInteraction();
				}
			}
			else if (_isLocalPlayer || _isHoverInteractionActive || _isPlayingInteractionAnim || _isGripping || (!(_sittingIK == null) && _sittingIK.RequiresRemoteSolvers) || (!(fbbik == null) && fbbik.enabled) || (!(interactionSystem == null) && interactionSystem.enabled))
			{
				CheckInteractionAnimComplete();
				RefreshRemoteIKSolversState();
			}
		}

		private void CheckInteractionAnimComplete()
		{
			if (_isPlayingInteractionAnim && !(interactionSystem == null) && !interactionSystem.IsPaused() && !interactionSystem.IsInInteraction(FullBodyBipedEffector.LeftHand) && !interactionSystem.IsInInteraction(FullBodyBipedEffector.RightHand))
			{
				_isPlayingInteractionAnim = false;
				if (_activeSetup != null && _activeResolvedHand.IsValid)
				{
					_pendingReEngageSetup = _activeSetup;
					_pendingReEngageHand = _activeResolvedHand;
					_hoverReEngageTimer = _activeSetup.HoverReEngageDelay;
				}
				_activeSetup = null;
				_activeResolvedHand = ResolvedHand.Invalid;
				if (_isLocalPlayer)
				{
					CmdSetInteractionAnimActive(active: false);
				}
			}
		}

		private void OnDisable()
		{
			if (_isLocalPlayer)
			{
				UnsubscribeFromLocalEvents();
			}
			_spawnWatcher?.UnregisterPendingLookup(_gripTargetNetId, OnGripObjectSpawned);
			if (interactionSystem != null)
			{
				interactionSystem.StopAll();
			}
			_activeSetup = null;
			_activeResolvedHand = ResolvedHand.Invalid;
			_isPlayingInteractionAnim = false;
			_isHoverInteractionActive = false;
			_gripUsesLeftHand = false;
			_gripUsesRightHand = false;
			_pendingReEngageSetup = null;
			_hoverReEngageTimer = 0f;
		}

		public PlayerIKBridge()
		{
			_Mirror_SyncVarHookDelegate__gripTargetNetId = OnGripTargetChanged;
			_Mirror_SyncVarHookDelegate__dominantHandByte = OnDominantHandChanged;
			_Mirror_SyncVarHookDelegate__hoverTargetPacked = OnHoverTargetChanged;
			_Mirror_SyncVarHookDelegate__isInteractionAnimActive = OnInteractionAnimActiveChanged;
			_Mirror_SyncVarHookDelegate__isGripPaused = OnGripPausedChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetDominantHand__Byte(byte hand)
		{
			Network_dominantHandByte = hand;
		}

		protected static void InvokeUserCode_CmdSetDominantHand__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetDominantHand called on client.");
			}
			else
			{
				((PlayerIKBridge)obj).UserCode_CmdSetDominantHand__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdSetHoverTarget__UInt64(ulong packed)
		{
			Network_hoverTargetPacked = packed;
		}

		protected static void InvokeUserCode_CmdSetHoverTarget__UInt64(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetHoverTarget called on client.");
			}
			else
			{
				((PlayerIKBridge)obj).UserCode_CmdSetHoverTarget__UInt64(reader.ReadVarULong());
			}
		}

		protected void UserCode_CmdSetInteractionAnimActive__Boolean(bool active)
		{
			Network_isInteractionAnimActive = active;
		}

		protected static void InvokeUserCode_CmdSetInteractionAnimActive__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetInteractionAnimActive called on client.");
			}
			else
			{
				((PlayerIKBridge)obj).UserCode_CmdSetInteractionAnimActive__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_CmdSetGripTarget__UInt32(uint targetNetId)
		{
			Network_gripTargetNetId = targetNetId;
			if (targetNetId == 0)
			{
				Network_isGripPaused = false;
			}
		}

		protected static void InvokeUserCode_CmdSetGripTarget__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetGripTarget called on client.");
			}
			else
			{
				((PlayerIKBridge)obj).UserCode_CmdSetGripTarget__UInt32(reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdSetGripPaused__Boolean(bool paused)
		{
			Network_isGripPaused = paused;
		}

		protected static void InvokeUserCode_CmdSetGripPaused__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetGripPaused called on client.");
			}
			else
			{
				((PlayerIKBridge)obj).UserCode_CmdSetGripPaused__Boolean(reader.ReadBool());
			}
		}

		static PlayerIKBridge()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerIKBridge), "System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetDominantHand(System.Byte)", InvokeUserCode_CmdSetDominantHand__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerIKBridge), "System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetHoverTarget(System.UInt64)", InvokeUserCode_CmdSetHoverTarget__UInt64, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerIKBridge), "System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetInteractionAnimActive(System.Boolean)", InvokeUserCode_CmdSetInteractionAnimActive__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerIKBridge), "System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetGripTarget(System.UInt32)", InvokeUserCode_CmdSetGripTarget__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerIKBridge), "System.Void NomadDrive.Features.Player.IK.PlayerIKBridge::CmdSetGripPaused(System.Boolean)", InvokeUserCode_CmdSetGripPaused__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarUInt(_gripTargetNetId);
				NetworkWriterExtensions.WriteByte(writer, _dominantHandByte);
				writer.WriteVarULong(_hoverTargetPacked);
				writer.WriteBool(_isInteractionAnimActive);
				writer.WriteBool(_isGripPaused);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarUInt(_gripTargetNetId);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _dominantHandByte);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteVarULong(_hoverTargetPacked);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteBool(_isInteractionAnimActive);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				writer.WriteBool(_isGripPaused);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _gripTargetNetId, _Mirror_SyncVarHookDelegate__gripTargetNetId, reader.ReadVarUInt());
				GeneratedSyncVarDeserialize(ref _dominantHandByte, _Mirror_SyncVarHookDelegate__dominantHandByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _hoverTargetPacked, _Mirror_SyncVarHookDelegate__hoverTargetPacked, reader.ReadVarULong());
				GeneratedSyncVarDeserialize(ref _isInteractionAnimActive, _Mirror_SyncVarHookDelegate__isInteractionAnimActive, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isGripPaused, _Mirror_SyncVarHookDelegate__isGripPaused, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _gripTargetNetId, _Mirror_SyncVarHookDelegate__gripTargetNetId, reader.ReadVarUInt());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _dominantHandByte, _Mirror_SyncVarHookDelegate__dominantHandByte, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _hoverTargetPacked, _Mirror_SyncVarHookDelegate__hoverTargetPacked, reader.ReadVarULong());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isInteractionAnimActive, _Mirror_SyncVarHookDelegate__isInteractionAnimActive, reader.ReadBool());
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isGripPaused, _Mirror_SyncVarHookDelegate__isGripPaused, reader.ReadBool());
			}
		}
	}
}
