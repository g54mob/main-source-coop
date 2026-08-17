using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using NomadDrive.Features.Vehicle;
using NomadDrive.Features.Vehicle.Parts.Seats;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Furnitures
{
	[RequireComponent(typeof(NetworkedTransform))]
	public class SittableSurface : Interactable, ISittable
	{
		private enum StandingPointType
		{
			None = 0,
			Left = 1,
			Right = 2,
			Forward = 3
		}

		[Header("Sitting")]
		[Tooltip("Where the player is locked while seated. Auto-assigned to NetworkedTransform.alternativeSyncTransform.")]
		[SerializeField]
		private Transform sittingTransform;

		[Tooltip("Where the player is repositioned when standing up. If null, a default point is created in front.")]
		[SerializeField]
		private Transform standingPoint;

		[SerializeField]
		private float defaultStandingPointDistance = 0.6f;

		[Header("Vehicle Standing Points (side-based exit)")]
		[Tooltip("Used only when this sittable is a vehicle seat. The vehicle slot type selects which one is the exit point (LeftSeat -> left, RightSeat -> right, generic Seat -> forward). Leave empty for standalone furniture; then 'standingPoint' / the auto default is used.")]
		[SerializeField]
		private Transform leftStandingPoint;

		[SerializeField]
		private Transform rightStandingPoint;

		[SerializeField]
		private Transform forwardStandingPoint;

		[Header("Sitting Rig Targets (optional)")]
		[SerializeField]
		private Transform leftHandTarget;

		[SerializeField]
		private Transform rightHandTarget;

		[SerializeField]
		private Transform leftFootTarget;

		[SerializeField]
		private Transform rightFootTarget;

		[SerializeField]
		private Transform leftElbowBendGoal;

		[SerializeField]
		private Transform rightElbowBendGoal;

		[SerializeField]
		private Transform leftKneeBendGoal;

		[SerializeField]
		private Transform rightKneeBendGoal;

		[Header("Hover Visual")]
		[Tooltip("Renderers shown only while hovered (idle = invisible). If empty, all child renderers are used.")]
		[SerializeField]
		private Renderer[] hoverRenderers;

		[Tooltip("Optional override for the invisible material applied to the visual mesh while hovered so only the selection outline shows (the fill is invisible). Leave empty: a bundled invisible material is auto-loaded from Resources, so no inspector wiring is required.")]
		[SerializeField]
		private Material outlineOnlyMaterial;

		[Header("Audio")]
		[SerializeField]
		private SoundID sitDownSound;

		[SerializeField]
		private SoundID standUpSound;

		[Header("Vehicle Stand-Up Safety Cap")]
		[Tooltip("Snapped SitPoints only: above this vehicle speed (km/h) standing up is blocked with a warning, as a safety net for the ride-along hand-off. Normal speeds stand up fine and ride along. Ground sit points and built-in vehicle seats are never blocked.")]
		[SerializeField]
		private float standUpMaxVehicleSpeedKmh = 15f;

		[SyncVar(hook = "OnOccupiedChanged")]
		private bool _isOccupied;

		[SyncVar]
		private bool _isDriverSeat;

		[SyncVar]
		private StandingPointType _standingPointType;

		private int _occupantConnectionId = -1;

		private static readonly HashSet<SittableSurface> ServerSittables;

		[Inject]
		private IPlayerService _playerService;

		private Transform _defaultStandingPoint;

		private Renderer[] _visualRenderers;

		private HeldItem _ownerFurniture;

		private Material[][] _originalSharedMaterials;

		private Material _outlineMaterial;

		private const string OutlineOnlyMaterialResourceName = "SittableOutlineOnlyMaterial";

		private InteractionStateMachine<SittableState> _stateMachine;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOccupied;

		protected override bool UseStateMachine => true;

		public bool IsOccupied => _isOccupied;

		public SittableState CurrentState
		{
			get
			{
				if (!_isOccupied)
				{
					return SittableState.Empty;
				}
				return SittableState.Occupied;
			}
		}

		public uint NetId => base.netId;

		public bool IsDriverSeat => _isDriverSeat;

		public VehicleSeatSlot VehicleSeatSlot { get; private set; }

		public NetworkedTransform NetworkedTransform { get; private set; }

		public Transform CurrentStandingPoint => _standingPointType switch
		{
			StandingPointType.Left => leftStandingPoint, 
			StandingPointType.Right => rightStandingPoint, 
			StandingPointType.Forward => forwardStandingPoint, 
			_ => (standingPoint != null) ? standingPoint : _defaultStandingPoint, 
		};

		public Transform LeftHandTarget => leftHandTarget;

		public Transform RightHandTarget => rightHandTarget;

		public Transform LeftFootTarget => leftFootTarget;

		public Transform RightFootTarget => rightFootTarget;

		public Transform LeftElbowBendGoal => leftElbowBendGoal;

		public Transform RightElbowBendGoal => rightElbowBendGoal;

		public Transform LeftKneeBendGoal => leftKneeBendGoal;

		public Transform RightKneeBendGoal => rightKneeBendGoal;

		public bool Network_isOccupied
		{
			get
			{
				return _isOccupied;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isOccupied, 64uL, _Mirror_SyncVarHookDelegate__isOccupied);
			}
		}

		public bool Network_isDriverSeat
		{
			get
			{
				return _isDriverSeat;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isDriverSeat, 128uL, null);
			}
		}

		public StandingPointType Network_standingPointType
		{
			get
			{
				return _standingPointType;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _standingPointType, 256uL, null);
			}
		}

		public void SetOccupied(bool value)
		{
			CmdSetOccupied(value);
		}

		protected override void Awake()
		{
			base.Awake();
			NetworkedTransform = GetComponent<NetworkedTransform>();
			CreateDefaultStandingPoint();
			CacheVisualRenderers();
			_ownerFurniture = GetComponentInParent<HeldItem>();
			_outlineMaterial = ((outlineOnlyMaterial != null) ? outlineOnlyMaterial : Resources.Load<Material>("SittableOutlineOnlyMaterial"));
		}

		protected override void Start()
		{
			base.Start();
			SetVisualRenderersVisible(visible: false);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			ServerSittables.Add(this);
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			ServerSittables.Remove(this);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			UpdateState();
			if (_isOccupied)
			{
				SetInteractionAvailability(newValue: false);
				SetOwnerFurnitureHoverIgnored(ignored: true);
			}
		}

		private void CreateDefaultStandingPoint()
		{
			GameObject gameObject = new GameObject("DefaultStandingPoint");
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.forward * defaultStandingPointDistance;
			gameObject.transform.localRotation = Quaternion.identity;
			_defaultStandingPoint = gameObject.transform;
		}

		private void CacheVisualRenderers()
		{
			_visualRenderers = ((hoverRenderers != null && hoverRenderers.Length != 0) ? hoverRenderers : GetComponentsInChildren<Renderer>(includeInactive: true));
			_originalSharedMaterials = new Material[_visualRenderers.Length][];
			for (int i = 0; i < _visualRenderers.Length; i++)
			{
				_originalSharedMaterials[i] = ((_visualRenderers[i] != null) ? _visualRenderers[i].sharedMaterials : null);
			}
		}

		private void SetVisualRenderersVisible(bool visible)
		{
			if (_visualRenderers == null)
			{
				return;
			}
			Renderer[] visualRenderers = _visualRenderers;
			foreach (Renderer renderer in visualRenderers)
			{
				if (renderer != null)
				{
					renderer.enabled = visible;
				}
			}
		}

		private void ApplyOutlineOnlyMaterials()
		{
			if (_visualRenderers == null || _outlineMaterial == null)
			{
				return;
			}
			Renderer[] visualRenderers = _visualRenderers;
			foreach (Renderer renderer in visualRenderers)
			{
				if (!(renderer == null))
				{
					Material[] array = new Material[renderer.sharedMaterials.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = _outlineMaterial;
					}
					renderer.sharedMaterials = array;
				}
			}
		}

		private void RestoreOriginalMaterials()
		{
			if (_visualRenderers == null || _originalSharedMaterials == null)
			{
				return;
			}
			for (int i = 0; i < _visualRenderers.Length; i++)
			{
				Renderer renderer = _visualRenderers[i];
				if (!(renderer == null) && _originalSharedMaterials[i] != null)
				{
					renderer.sharedMaterials = _originalSharedMaterials[i];
				}
			}
		}

		public override bool IsHoveringIgnored()
		{
			if (!base.IsHoveringIgnored())
			{
				return _playerService?.EquipmentManager?.IsItemEquipped == true;
			}
			return true;
		}

		protected override void OnHovered()
		{
			SetVisualRenderersVisible(visible: true);
			ApplyOutlineOnlyMaterials();
			base.OnHovered();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			RestoreOriginalMaterials();
			SetVisualRenderersVisible(visible: false);
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<SittableState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(SittableState.Empty, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.sit", HandleSit).WithCondition(CanSitOrSwapHere).WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(SittableState.Occupied, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default).WithNameLabelVisibility(visible: false).WithInteractionLabelVisibility(visible: false));
		}

		private SittableState DetermineState()
		{
			return CurrentState;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		private bool CanSitOrSwapHere()
		{
			NomadDrive.Features.Player.Player player = _playerService?.LocalPlayer;
			if (player == null || !player.IsPlayerSitting())
			{
				return true;
			}
			return BelongsToSameFurnitureAsPlayerSeat();
		}

		private bool BelongsToSameFurnitureAsPlayerSeat()
		{
			if (!(_playerService?.LocalPlayer?.GetPlayerSeat() is Component component))
			{
				return false;
			}
			HeldItem componentInParent = GetComponentInParent<HeldItem>();
			if (componentInParent == null)
			{
				return false;
			}
			HeldItem componentInParent2 = component.GetComponentInParent<HeldItem>();
			if (componentInParent2 != null)
			{
				return componentInParent2 == componentInParent;
			}
			return false;
		}

		private void HandleSit()
		{
			if (!_isOccupied && !(_playerService?.LocalPlayer == null) && _playerService?.EquipmentManager?.IsItemEquipped != true)
			{
				SetOccupied(value: true);
				_playerService.LocalPlayer.OnPlayerSit.Invoke(this);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetOccupied(bool value, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(value);
			SendCommandInternal("System.Void NomadDrive.Features.Furnitures.SittableSurface::CmdSetOccupied(System.Boolean,Mirror.NetworkConnectionToClient)", -2006293879, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnOccupiedChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				AudioManager?.PlayOneShotAttached(newValue ? sitDownSound : standUpSound, base.gameObject);
				UpdateState();
				SetInteractionAvailability(!newValue);
				SetOwnerFurnitureHoverIgnored(newValue);
			}
		}

		private void SetOwnerFurnitureHoverIgnored(bool ignored)
		{
			if (!(VehicleSeatSlot != null) && !(_ownerFurniture == null))
			{
				if (ignored)
				{
					_ownerFurniture.IgnoreHovering();
				}
				else
				{
					_ownerFurniture.UnignoreHovering();
				}
			}
		}

		public bool TryGetMountVehicle(out VehicleManager vehicle)
		{
			vehicle = null;
			if (VehicleSeatSlot != null)
			{
				vehicle = VehicleSeatSlot.VehicleManager;
				return vehicle != null;
			}
			Transform parent = base.transform;
			while (parent != null)
			{
				if (parent.TryGetComponent<NetworkedTransform>(out var component) && component.ParentTransform != null)
				{
					vehicle = component.ParentTransform.GetComponentInParent<VehicleManager>();
					if (vehicle != null)
					{
						return true;
					}
				}
				parent = parent.parent;
			}
			return false;
		}

		public bool CanStandUpNow()
		{
			if (VehicleSeatSlot != null)
			{
				return true;
			}
			if (!TryGetMountVehicle(out var vehicle))
			{
				return true;
			}
			if (vehicle.NetworkSync == null)
			{
				return true;
			}
			return Mathf.Abs(vehicle.NetworkSync.VehicleSpeedKmh) <= standUpMaxVehicleSpeedKmh;
		}

		public void ConfigureAsVehicleSeat(VehicleSeatSlot slot, bool isDriver)
		{
			VehicleSeatSlot = slot;
			if (base.isServer)
			{
				Network_isDriverSeat = isDriver;
				Network_standingPointType = MapSlotTypeToStandingPoint((slot != null) ? slot.SlotType : ObjectSlotType.None);
			}
			UpdateState();
		}

		public void ClearVehicleSeat()
		{
			VehicleSeatSlot = null;
			if (base.isServer)
			{
				Network_isDriverSeat = false;
				Network_standingPointType = StandingPointType.None;
			}
			UpdateState();
		}

		private static StandingPointType MapSlotTypeToStandingPoint(ObjectSlotType slotType)
		{
			return slotType switch
			{
				ObjectSlotType.LeftSeat => StandingPointType.Left, 
				ObjectSlotType.RightSeat => StandingPointType.Right, 
				ObjectSlotType.Seat => StandingPointType.Forward, 
				_ => StandingPointType.None, 
			};
		}

		public static void FreeSittablesOccupiedByConnection(int connectionId)
		{
			foreach (SittableSurface serverSittable in ServerSittables)
			{
				if (serverSittable != null && serverSittable._isOccupied && serverSittable._occupantConnectionId == connectionId)
				{
					serverSittable.ServerForceVacate();
				}
			}
		}

		[Server]
		public void ServerVacate()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Furnitures.SittableSurface::ServerVacate()' called when server was not active");
			}
			else if (_isOccupied)
			{
				Network_isOccupied = false;
				_occupantConnectionId = -1;
			}
		}

		[Server]
		private void ServerForceVacate()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Furnitures.SittableSurface::ServerForceVacate()' called when server was not active");
			}
			else if (_isOccupied)
			{
				ServerVacate();
			}
		}

		public SittableSurface()
		{
			_Mirror_SyncVarHookDelegate__isOccupied = OnOccupiedChanged;
		}

		static SittableSurface()
		{
			ServerSittables = new HashSet<SittableSurface>();
			RemoteProcedureCalls.RegisterCommand(typeof(SittableSurface), "System.Void NomadDrive.Features.Furnitures.SittableSurface::CmdSetOccupied(System.Boolean,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdSetOccupied__Boolean__NetworkConnectionToClient, requiresAuthority: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetOccupied__Boolean__NetworkConnectionToClient(bool value, NetworkConnectionToClient sender)
		{
			if (_isOccupied != value)
			{
				Network_isOccupied = value;
				_occupantConnectionId = ((!value) ? (-1) : (sender?.connectionId ?? (-1)));
			}
		}

		protected static void InvokeUserCode_CmdSetOccupied__Boolean__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetOccupied called on client.");
			}
			else
			{
				((SittableSurface)obj).UserCode_CmdSetOccupied__Boolean__NetworkConnectionToClient(reader.ReadBool(), senderConnection);
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isOccupied);
				writer.WriteBool(_isDriverSeat);
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(writer, _standingPointType);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteBool(_isOccupied);
			}
			if ((syncVarDirtyBits & 0x80L) != 0L)
			{
				writer.WriteBool(_isDriverSeat);
			}
			if ((syncVarDirtyBits & 0x100L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(writer, _standingPointType);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isOccupied, _Mirror_SyncVarHookDelegate__isOccupied, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _isDriverSeat, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _standingPointType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOccupied, _Mirror_SyncVarHookDelegate__isOccupied, reader.ReadBool());
			}
			if ((num & 0x80L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isDriverSeat, null, reader.ReadBool());
			}
			if ((num & 0x100L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _standingPointType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(reader));
			}
		}
	}
}
