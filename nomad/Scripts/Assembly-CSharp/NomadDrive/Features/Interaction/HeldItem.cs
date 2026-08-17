using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Interaction
{
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(NetworkedTransform))]
	[RequireComponent(typeof(LootItemDefinition))]
	public class HeldItem : Interactable, IPlaceable, IEquippable, IConnectionOwnedCleanup
	{
		[SerializeField]
		public HeldItemConfig heldItemConfig;

		[Header("Audio Override (optional)")]
		[Tooltip("Atanirsa EquipmentManager'in default equip sesini override eder. Bos birakilirsa default ses calar.")]
		[SerializeField]
		private SoundID equipSoundOverride;

		private Vector3 _boundsCenterLocalOffset;

		private bool _boundsCenterLocalOffsetCalculated;

		[SyncVar(hook = "OnPlacementNetworkDataChanged")]
		private PlacementNetworkData _placementNetworkData;

		[SyncVar(hook = "OnIsEquippedChanged")]
		private bool _isEquipped;

		[SerializeField]
		private LayerMask collisionLayers = -1;

		[SerializeField]
		private float wakeUpRadius = 1.5f;

		private readonly HashSet<Collider> _activeCollisions = new HashSet<Collider>();

		private readonly HashSet<Collider> _snappedObjectColliders = new HashSet<Collider>();

		private Collider[] _ownColliders;

		private List<SnappingPlane> _childSnappingPlanes = new List<SnappingPlane>();

		[Inject]
		protected IPlayerService playerService;

		[Inject]
		protected INetworkManager networkManager;

		private InteractionStateMachine<HeldItemState> _stateMachine;

		private Vector3 _streamTargetPos;

		private Quaternion _streamTargetRot;

		private bool _isReceivingPlacementStream;

		private float _placementStreamTimeoutTimer;

		private Coroutine _placementStreamCo;

		private double _placementStreamFinalizedTime = -1.0 / 0.0;

		private const float PlacementStreamTimeout = 0.4f;

		private const float PlacementStreamLerpSpeed = 18f;

		[SyncVar(hook = "OnSnapStreamActiveChanged")]
		private bool _snapStreamActive;

		private Vector3 _snapStreamTargetPos;

		private Quaternion _snapStreamTargetRot;

		private bool _isReceivingSnapStream;

		private bool _snapConstraintSuspended;

		private float _snapStreamTimeoutTimer;

		private Coroutine _snapStreamCo;

		private float _lastSnapStreamSendTime;

		private const float SnapStreamTimeout = 0.5f;

		private const float SnapStreamLerpSpeed = 22f;

		private const float SnapStreamSendInterval = 1f / 30f;

		public Action<PlacementNetworkData, PlacementNetworkData> _Mirror_SyncVarHookDelegate__placementNetworkData;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isEquipped;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__snapStreamActive;

		public SoundID EquipSoundOverride => equipSoundOverride;

		public IObjectPlacementManager PlacementManager { get; set; }

		public bool CanPlaceable { get; set; }

		public Vector3 DefaultPlacingRotation { get; set; }

		public NomadDrive.Features.ObjectPlacement.RotationAxis PlacementRotationAxis { get; set; }

		public Vector3 PositionOffsetForPlacement { get; set; }

		public NetworkedTransform NetworkedTransform { get; set; }

		public Vector3 BoundsCenterLocalOffset
		{
			get
			{
				if (!_boundsCenterLocalOffsetCalculated)
				{
					_boundsCenterLocalOffset = CalculateBoundsCenterLocalOffset();
					_boundsCenterLocalOffsetCalculated = true;
				}
				return _boundsCenterLocalOffset;
			}
		}

		public float SnappingAreaDistance { get; set; }

		public float PlacementShaderScale { get; set; }

		public GhostPreviewMode GhostPreviewMode { get; set; }

		public Vector3 GhostPreviewScale { get; set; }

		public float PlacementRadiusMultiplier { get; set; } = 1f;

		public float DefaultPlacementDistance { get; set; }

		public float MinPlacementDistance { get; set; }

		public float MaxPlacementDistance { get; set; }

		public SnappingPlane AttachedSnappingPlane { get; set; }

		public ISnappingPlaneContainer AttachedSnappingPlaneContainer { get; set; }

		public PlacementNetworkData PlacementNetworkData
		{
			get
			{
				return _placementNetworkData;
			}
			set
			{
				Network_placementNetworkData = value;
			}
		}

		public Vector3 PositionOnHand
		{
			get
			{
				if (!(heldItemConfig != null))
				{
					return Vector3.zero;
				}
				return heldItemConfig.positionOnHand;
			}
		}

		public Vector3 RotationOnHand
		{
			get
			{
				if (!(heldItemConfig != null))
				{
					return Vector3.zero;
				}
				return heldItemConfig.rotationOnHand;
			}
		}

		public ItemMaterial DropMaterial
		{
			get
			{
				if (!(heldItemConfig != null))
				{
					return ItemMaterial.Generic;
				}
				return heldItemConfig.itemMaterial;
			}
		}

		public UnityEvent OnEquipped { get; set; } = new UnityEvent();

		public UnityEvent OnUnequipped { get; set; } = new UnityEvent();

		public UnityEvent OnDropped { get; set; } = new UnityEvent();

		public bool IsEquipped
		{
			get
			{
				return _isEquipped;
			}
			set
			{
				Network_isEquipped = value;
			}
		}

		public LayerMask CollisionLayers => collisionLayers;

		protected InteractionStateMachine<HeldItemState> StateMachine => _stateMachine;

		public HeldItemState CurrentState => _stateMachine?.CurrentState ?? HeldItemState.Idle;

		protected virtual bool UseDefaultStateMachine => true;

		protected override bool UseStateMachine => UseDefaultStateMachine;

		public PlacementNetworkData Network_placementNetworkData
		{
			get
			{
				return _placementNetworkData;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _placementNetworkData, 64uL, _Mirror_SyncVarHookDelegate__placementNetworkData);
			}
		}

		public bool Network_isEquipped
		{
			get
			{
				return _isEquipped;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isEquipped, 128uL, _Mirror_SyncVarHookDelegate__isEquipped);
			}
		}

		public bool Network_snapStreamActive
		{
			get
			{
				return _snapStreamActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _snapStreamActive, 256uL, _Mirror_SyncVarHookDelegate__snapStreamActive);
			}
		}

		protected override void InitializeStateMachine()
		{
			if (UseDefaultStateMachine)
			{
				_stateMachine = new InteractionStateMachine<HeldItemState>(this);
				base.BaseStateMachine = _stateMachine;
				ConfigureStates();
				_stateMachine.Initialize(DetermineAdvancedEntityState());
			}
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(HeldItemState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(HeldItemState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		protected virtual HeldItemState DetermineAdvancedEntityState()
		{
			if (!_isEquipped)
			{
				return HeldItemState.Idle;
			}
			return HeldItemState.Equipped;
		}

		public override void UpdateState()
		{
			if (_stateMachine != null)
			{
				_stateMachine.TransitionTo(DetermineAdvancedEntityState());
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_isEquipped)
			{
				IgnoreHovering();
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_ = heldItemConfig == null;
			if (TryGetComponent<NetworkedTransform>(out var component))
			{
				NetworkedTransform = component;
			}
			else
			{
				EvilLogger.LogError("NetworkedTransform component missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\Interactables\\HeldItem.cs", 197);
			}
			_ownColliders = GetComponentsInChildren<Collider>();
			RefreshChildSnappingPlanes();
			if (heldItemConfig != null)
			{
				SetupFromConfig();
			}
			_boundsCenterLocalOffset = CalculateBoundsCenterLocalOffset();
			_boundsCenterLocalOffsetCalculated = true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			OnEquipped.AddListener(OnEquip);
			OnUnequipped.AddListener(OnUnequip);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			OnEquipped.RemoveListener(OnEquip);
			OnUnequipped.RemoveListener(OnUnequip);
			EndSnapFollowLocal();
		}

		protected override void OnHovered()
		{
			base.OnHovered();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
		}

		private void OnPlacementNetworkDataChanged(PlacementNetworkData oldValue, PlacementNetworkData newValue)
		{
			if (IsLateJoinCompleted && newValue.ParentNetworkID != 0)
			{
				HandlePlacementNetworkDataChangedAsync(newValue).Forget();
			}
		}

		private async UniTaskVoid HandlePlacementNetworkDataChangedAsync(PlacementNetworkData newValue)
		{
			(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(newValue.ParentNetworkID, 50, 100, this.GetCancellationTokenOnDestroy());
			ISnappingPlaneContainer component;
			if (!tuple.Item1)
			{
				EvilLogger.LogError($"<color=red>[AdvancedEntity]</color> Parent object with NetID {newValue.ParentNetworkID} not found for {base.gameObject.name} after multiple attempts", "HandlePlacementNetworkDataChangedAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\Interactables\\HeldItem.cs", 264);
			}
			else if (tuple.Item2.TryGetComponent<ISnappingPlaneContainer>(out component))
			{
				AttachedSnappingPlaneContainer = component;
				try
				{
					AttachedSnappingPlane = AttachedSnappingPlaneContainer.GetSnappingPlaneByIndex(newValue.SnappingPlaneIndex);
				}
				catch (Exception)
				{
					AttachedSnappingPlane = null;
				}
			}
		}

		private void OnIsEquippedChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					IgnoreHovering();
				}
				else
				{
					UnignoreHovering();
				}
				UpdateState();
			}
		}

		private void SetupFromConfig()
		{
			if (!(heldItemConfig == null))
			{
				DefaultPlacingRotation = heldItemConfig.enterPlacingModeStartRotation;
				PlacementRotationAxis = heldItemConfig.placingModeRotationAxis;
				PositionOffsetForPlacement = heldItemConfig.positionOffsetForPlacement;
				PlacementShaderScale = heldItemConfig.placementShaderScale;
				GhostPreviewMode = heldItemConfig.ghostPreviewMode;
				GhostPreviewScale = heldItemConfig.ghostPreviewScale;
				PlacementRadiusMultiplier = heldItemConfig.placementRadiusMultiplier;
				DefaultPlacementDistance = heldItemConfig.defaultPlacementDistance;
				MinPlacementDistance = heldItemConfig.minPlacementDistance;
				MaxPlacementDistance = heldItemConfig.maxPlacementDistance;
			}
		}

		public void SetPlacementNetworkData(uint parentNetworkID, byte snappingPlaneIndex)
		{
			CmdSetPlacementNetworkData(parentNetworkID, snappingPlaneIndex);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetPlacementNetworkData(uint parentNetworkID, byte snappingPlaneIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(parentNetworkID);
			NetworkWriterExtensions.WriteByte(writer, snappingPlaneIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetPlacementNetworkData(System.UInt32,System.Byte)", -1123970623, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public virtual void StartPlacing()
		{
			IEquipmentManager equipmentManager = playerService.EquipmentManager;
			if (equipmentManager.EquippedEntity != null)
			{
				equipmentManager.Unequip(this, keepSlotDetection: true);
			}
			CmdStartPlacing();
			PlacementManager = playerService.ObjectPlacementManager;
			PlacementManager.Execute(base.gameObject);
		}

		[Command(requiresAuthority = false)]
		private void CmdStartPlacing(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdStartPlacing(Mirror.NetworkConnectionToClient)", -1823816146, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void Release()
		{
			CmdRelease();
		}

		[Command]
		private void CmdRelease()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdRelease()", 1020761712, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public virtual void OnEquip()
		{
			IgnoreHovering();
		}

		public virtual void OnUnequip()
		{
			UnignoreHovering();
		}

		public virtual void OnPlacementModeEnter()
		{
		}

		public virtual void OnPlacementModeExit()
		{
		}

		public virtual void OnPlacedActions()
		{
		}

		public virtual void OnFallenActions()
		{
		}

		public virtual void OnRemovedActions()
		{
		}

		public void SyncGroundDrop(Vector3 position, Quaternion rotation)
		{
			CmdSyncGroundDrop(position, rotation, NetworkTime.time);
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncGroundDrop(Vector3 position, Quaternion rotation, double finalizeTime, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteDouble(finalizeTime);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdSyncGroundDrop(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double,Mirror.NetworkConnectionToClient)", -691500815, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSyncGroundDrop(Vector3 position, Quaternion rotation, double finalizeTime)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteDouble(finalizeTime);
			SendRPCInternal("System.Void NomadDrive.Features.Interaction.HeldItem::RpcSyncGroundDrop(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", -1879051133, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public void StreamPlacementPose(Vector3 pos, Quaternion rot)
		{
			CmdStreamPlacementPose(pos, rot, NetworkTime.time);
		}

		[Command(channel = 1, requiresAuthority = false)]
		private void CmdStreamPlacementPose(Vector3 pos, Quaternion rot, double sendTime)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			writer.WriteDouble(sendTime);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdStreamPlacementPose(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", -264620993, writer, 1, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcStreamPlacementPose(Vector3 pos, Quaternion rot, double sendTime)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			writer.WriteDouble(sendTime);
			SendRPCInternal("System.Void NomadDrive.Features.Interaction.HeldItem::RpcStreamPlacementPose(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", 1339369854, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void ApplyPlacementStream(Vector3 pos, Quaternion rot, double sendTime)
		{
			if (!base.isOwned && !(sendTime <= _placementStreamFinalizedTime))
			{
				_streamTargetPos = pos;
				_streamTargetRot = rot;
				_isReceivingPlacementStream = true;
				_placementStreamTimeoutTimer = 0.4f;
				if (_placementStreamCo == null)
				{
					_placementStreamCo = StartCoroutine(PlacementStreamInterpolation());
				}
			}
		}

		private IEnumerator PlacementStreamInterpolation()
		{
			while (_isReceivingPlacementStream)
			{
				_placementStreamTimeoutTimer -= Time.deltaTime;
				if (_placementStreamTimeoutTimer <= 0f)
				{
					_isReceivingPlacementStream = false;
					break;
				}
				base.transform.SetPositionAndRotation(Vector3.Lerp(base.transform.position, _streamTargetPos, Time.deltaTime * 18f), Quaternion.Slerp(base.transform.rotation, _streamTargetRot, Time.deltaTime * 18f));
				yield return null;
			}
			_placementStreamCo = null;
		}

		public void StopPlacementStream()
		{
			SetPlacementStreamFinalized(NetworkTime.time);
		}

		private void SetPlacementStreamFinalized(double time)
		{
			if (time > _placementStreamFinalizedTime)
			{
				_placementStreamFinalizedTime = time;
			}
			_isReceivingPlacementStream = false;
		}

		public void BeginSnapStream()
		{
			if (base.isOwned && base.netId != 0)
			{
				CmdSetSnapStream(active: true);
			}
		}

		public void EndSnapStream()
		{
			if (base.isOwned && base.netId != 0)
			{
				CmdSetSnapStream(active: false);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetSnapStream(bool active)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(active);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetSnapStream(System.Boolean)", 703990476, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnSnapStreamActiveChanged(bool _, bool active)
		{
			if (!base.isOwned)
			{
				if (active)
				{
					BeginRemoteSnapFollow();
				}
				else
				{
					EndRemoteSnapFollow();
				}
			}
		}

		public void StreamSnapPose(Vector3 pos, Quaternion rot)
		{
			if (base.isOwned && !(Time.time - _lastSnapStreamSendTime < 1f / 30f))
			{
				_lastSnapStreamSendTime = Time.time;
				CmdStreamSnapPose(pos, rot);
			}
		}

		[Command(channel = 1, requiresAuthority = false)]
		private void CmdStreamSnapPose(Vector3 pos, Quaternion rot)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdStreamSnapPose(UnityEngine.Vector3,UnityEngine.Quaternion)", 197675404, writer, 1, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcStreamSnapPose(Vector3 pos, Quaternion rot)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			SendRPCInternal("System.Void NomadDrive.Features.Interaction.HeldItem::RpcStreamSnapPose(UnityEngine.Vector3,UnityEngine.Quaternion)", 1618103751, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void ApplySnapStream(Vector3 pos, Quaternion rot)
		{
			if (!base.isOwned)
			{
				_snapStreamTargetPos = pos;
				_snapStreamTargetRot = rot;
				_snapStreamTimeoutTimer = 0.5f;
				if (!_isReceivingSnapStream)
				{
					BeginRemoteSnapFollow();
				}
			}
		}

		private void BeginRemoteSnapFollow()
		{
			if (!base.isOwned)
			{
				_isReceivingSnapStream = true;
				_snapStreamTimeoutTimer = 0.5f;
				SuspendSnapConstraint(suspend: true);
				if (_snapStreamCo == null)
				{
					_snapStreamCo = StartCoroutine(SnapStreamInterpolation());
				}
			}
		}

		private void EndRemoteSnapFollow()
		{
			_isReceivingSnapStream = false;
		}

		private void EndSnapFollowLocal()
		{
			_isReceivingSnapStream = false;
			if (_snapStreamCo != null)
			{
				StopCoroutine(_snapStreamCo);
				_snapStreamCo = null;
			}
			SuspendSnapConstraint(suspend: false);
		}

		private IEnumerator SnapStreamInterpolation()
		{
			while (_isReceivingSnapStream)
			{
				_snapStreamTimeoutTimer -= Time.deltaTime;
				if (_snapStreamTimeoutTimer <= 0f)
				{
					break;
				}
				SuspendSnapConstraint(suspend: true);
				base.transform.SetPositionAndRotation(Vector3.Lerp(base.transform.position, _snapStreamTargetPos, Time.deltaTime * 22f), Quaternion.Slerp(base.transform.rotation, _snapStreamTargetRot, Time.deltaTime * 22f));
				yield return null;
			}
			_isReceivingSnapStream = false;
			SuspendSnapConstraint(suspend: false);
			_snapStreamCo = null;
		}

		public void SetSnapConstraintSuspended(bool suspend)
		{
			SuspendSnapConstraint(suspend);
		}

		private void SuspendSnapConstraint(bool suspend)
		{
			ParentConstraint parentConstraint = ((NetworkedTransform != null) ? NetworkedTransform.ParentConstraint : null);
			if (parentConstraint == null || parentConstraint.sourceCount == 0)
			{
				return;
			}
			if (suspend)
			{
				if (parentConstraint.constraintActive)
				{
					parentConstraint.constraintActive = false;
					_snapConstraintSuspended = true;
				}
			}
			else if (_snapConstraintSuspended)
			{
				parentConstraint.constraintActive = true;
				_snapConstraintSuspended = false;
			}
		}

		public void OnOwnerDisconnecting(NetworkConnectionToClient conn)
		{
			if (base.netIdentity.connectionToClient == conn)
			{
				base.netIdentity.RemoveClientAuthority();
				if (NetworkServer.localConnection != null)
				{
					base.netIdentity.AssignClientAuthority(NetworkServer.localConnection);
				}
				if (IsEquipped)
				{
					IsEquipped = false;
					SetInteractionAvailability(newValue: true);
					NetworkedTransform.ServerSetParent(null, default(NetworkedTransformParentingConfig), 0);
					ServerDropToGround();
					SetRigidCollidersTriggered(newValue: false);
					ServerSetSnappedDescendantsTriggered(triggered: false);
				}
			}
		}

		[Server]
		private void ServerDropToGround()
		{
			RaycastHit hitInfo;
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.HeldItem::ServerDropToGround()' called when server was not active");
			}
			else if (Physics.Raycast(base.transform.position + Vector3.up * 0.2f, Vector3.down, out hitInfo, 5f, CollisionLayers, QueryTriggerInteraction.Ignore))
			{
				Vector3 point = hitInfo.point;
				Quaternion rotation = base.transform.rotation;
				base.transform.SetPositionAndRotation(point, rotation);
				RpcSyncGroundDrop(point, rotation, NetworkTime.time);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsValidCollision(other))
			{
				_activeCollisions.Add(other);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			_activeCollisions.Remove(other);
		}

		private bool IsValidCollision(Collider other)
		{
			if (other == null || !other.gameObject.activeSelf || other.isTrigger || IsColliderTempChild(other))
			{
				return false;
			}
			return (collisionLayers.value & (1 << other.gameObject.layer)) != 0;
		}

		public bool IsCollidingAny()
		{
			_activeCollisions.RemoveWhere((Collider col) => col == null || !col.enabled || !col.gameObject.activeSelf);
			return _activeCollisions.Count > 0;
		}

		public void Enable()
		{
			base.enabled = true;
		}

		public void Disable()
		{
			base.enabled = false;
		}

		public void ClearColliders()
		{
			_activeCollisions.Clear();
		}

		private Vector3 CalculateBoundsCenterLocalOffset()
		{
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			if (!PlacementGeometry.TryCalculateRootLocalColliderBounds(base.transform, componentsInChildren, out var localBounds))
			{
				return Vector3.zero;
			}
			return localBounds.center;
		}

		public void SetCollidersTrigger(bool value)
		{
			Collider[] ownColliders = _ownColliders;
			for (int i = 0; i < ownColliders.Length; i++)
			{
				ownColliders[i].isTrigger = value;
			}
		}

		public bool IsCollidingWith(Collider other)
		{
			return _activeCollisions.Contains(other);
		}

		private bool IsColliderTempChild(Collider col)
		{
			return _snappedObjectColliders.Contains(col);
		}

		public void RefreshChildSnappingPlanes()
		{
			_childSnappingPlanes = GetComponentsInChildren<SnappingPlane>(includeInactive: true).ToList();
		}

		public void CollectSnappedObjectColliders()
		{
			RefreshChildSnappingPlanes();
			_snappedObjectColliders.Clear();
			List<GameObject> list = new List<GameObject>();
			GetAllSnappedObjectGameObjectsRecursive(list);
			foreach (GameObject item in list)
			{
				if (!item.TryGetComponent<Interactable>(out var component))
				{
					continue;
				}
				Collider[] rigidColliders = component.GetRigidColliders();
				if (rigidColliders == null)
				{
					continue;
				}
				Collider[] array = rigidColliders;
				foreach (Collider collider in array)
				{
					if (collider != null)
					{
						_snappedObjectColliders.Add(collider);
					}
				}
			}
		}

		public void ClearSnappedObjectColliders()
		{
			_snappedObjectColliders.Clear();
		}

		public void AddForwardedCollision(Collider other)
		{
			_activeCollisions.Add(other);
		}

		public void RemoveForwardedCollision(Collider other)
		{
			_activeCollisions.Remove(other);
		}

		public void GetAllSnappedObjectGameObjectsRecursive(List<GameObject> results)
		{
			foreach (SnappingPlane childSnappingPlane in _childSnappingPlanes)
			{
				foreach (uint placedEntityNetworkID in childSnappingPlane.PlacedEntityNetworkIDs)
				{
					if (networkManager.TryGetNetworkObjectById(placedEntityNetworkID, out var networkObject) && !results.Contains(networkObject))
					{
						results.Add(networkObject);
						CollectSnappedRecursive(networkObject, results);
					}
				}
			}
		}

		private void CollectSnappedRecursive(GameObject obj, List<GameObject> results)
		{
			SnappingPlane[] componentsInChildren = obj.GetComponentsInChildren<SnappingPlane>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (uint placedEntityNetworkID in componentsInChildren[i].PlacedEntityNetworkIDs)
				{
					if (networkManager.TryGetNetworkObjectById(placedEntityNetworkID, out var networkObject) && !results.Contains(networkObject))
					{
						results.Add(networkObject);
						CollectSnappedRecursive(networkObject, results);
					}
				}
			}
		}

		public void GetSnappedDescendantNetIds(HashSet<uint> results)
		{
			foreach (SnappingPlane childSnappingPlane in _childSnappingPlanes)
			{
				if (childSnappingPlane == null)
				{
					continue;
				}
				foreach (uint placedEntityNetworkID in childSnappingPlane.PlacedEntityNetworkIDs)
				{
					if (results.Add(placedEntityNetworkID) && networkManager.TryGetNetworkObjectById(placedEntityNetworkID, out var networkObject))
					{
						CollectSnappedNetIdsRecursive(networkObject, results);
					}
				}
			}
		}

		private void CollectSnappedNetIdsRecursive(GameObject obj, HashSet<uint> results)
		{
			SnappingPlane[] componentsInChildren = obj.GetComponentsInChildren<SnappingPlane>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (uint placedEntityNetworkID in componentsInChildren[i].PlacedEntityNetworkIDs)
				{
					if (results.Add(placedEntityNetworkID) && networkManager.TryGetNetworkObjectById(placedEntityNetworkID, out var networkObject))
					{
						CollectSnappedNetIdsRecursive(networkObject, results);
					}
				}
			}
		}

		[Server]
		public void ServerSetSnappedDescendantsTriggered(bool triggered)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Interaction.HeldItem::ServerSetSnappedDescendantsTriggered(System.Boolean)' called when server was not active");
				return;
			}
			List<GameObject> list = new List<GameObject>();
			GetAllSnappedObjectGameObjectsRecursive(list);
			foreach (GameObject item in list)
			{
				if (!(item == null) && item.TryGetComponent<Interactable>(out var component))
				{
					component.SetRigidCollidersTriggered(triggered);
				}
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSetSnappedDescendantsTriggered(bool triggered)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(triggered);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetSnappedDescendantsTriggered(System.Boolean)", 80395912, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void SetVehicleIsolationRecursive(bool isolated)
		{
			SetVehicleColliderIsolated(isolated);
			List<GameObject> list = new List<GameObject>();
			GetAllSnappedObjectGameObjectsRecursive(list);
			foreach (GameObject item in list)
			{
				if (!(item == null) && item.TryGetComponent<Interactable>(out var component))
				{
					component.SetVehicleColliderIsolated(isolated);
				}
			}
		}

		protected bool IsLocalPlayerSitting()
		{
			return playerService?.LocalPlayer?.IsPlayerSitting() == true;
		}

		protected virtual void HandleEquip()
		{
			if (!IsLocalPlayerSitting())
			{
				Equip();
			}
		}

		public void Equip()
		{
			playerService.EquipmentManager.Equip(this);
		}

		public void Unequip()
		{
			playerService.EquipmentManager.Unequip(this);
		}

		public void Drop()
		{
			playerService.EquipmentManager.Drop(this);
		}

		private void WakeUpNearbyRigidbodies()
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, wakeUpRadius);
			foreach (Collider collider in array)
			{
				if ((_ownColliders == null || Array.IndexOf(_ownColliders, collider) < 0) && collider.attachedRigidbody != null && !collider.attachedRigidbody.isKinematic)
				{
					collider.attachedRigidbody.WakeUp();
				}
			}
		}

		public HeldItem()
		{
			_Mirror_SyncVarHookDelegate__placementNetworkData = OnPlacementNetworkDataChanged;
			_Mirror_SyncVarHookDelegate__isEquipped = OnIsEquippedChanged;
			_Mirror_SyncVarHookDelegate__snapStreamActive = OnSnapStreamActiveChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetPlacementNetworkData__UInt32__Byte(uint parentNetworkID, byte snappingPlaneIndex)
		{
			PlacementNetworkData placementNetworkData = new PlacementNetworkData
			{
				ParentNetworkID = parentNetworkID,
				SnappingPlaneIndex = snappingPlaneIndex
			};
			PlacementNetworkData = placementNetworkData;
		}

		protected static void InvokeUserCode_CmdSetPlacementNetworkData__UInt32__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetPlacementNetworkData called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdSetPlacementNetworkData__UInt32__Byte(reader.ReadVarUInt(), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdStartPlacing__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			if (!(base.netIdentity == null) && sender != null)
			{
				if (base.netIdentity.connectionToClient != null)
				{
					base.netIdentity.RemoveClientAuthority();
				}
				base.netIdentity.AssignClientAuthority(sender);
			}
		}

		protected static void InvokeUserCode_CmdStartPlacing__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStartPlacing called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdStartPlacing__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_CmdRelease()
		{
			if (base.netIdentity == null)
			{
				return;
			}
			if (base.netIdentity.connectionToClient != null)
			{
				base.netIdentity.RemoveClientAuthority();
			}
			if (NetworkServer.localConnection != null)
			{
				base.netIdentity.AssignClientAuthority(NetworkServer.localConnection);
			}
			else
			{
				if (NetworkServer.connections.Count <= 0)
				{
					return;
				}
				foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
				{
					if (value != null && value.isReady)
					{
						base.netIdentity.AssignClientAuthority(value);
						break;
					}
				}
			}
		}

		protected static void InvokeUserCode_CmdRelease(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRelease called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdRelease();
			}
		}

		protected void UserCode_CmdSyncGroundDrop__Vector3__Quaternion__Double__NetworkConnectionToClient(Vector3 position, Quaternion rotation, double finalizeTime, NetworkConnectionToClient sender)
		{
			base.transform.SetPositionAndRotation(position, rotation);
			RpcSyncGroundDrop(position, rotation, finalizeTime);
		}

		protected static void InvokeUserCode_CmdSyncGroundDrop__Vector3__Quaternion__Double__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncGroundDrop called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdSyncGroundDrop__Vector3__Quaternion__Double__NetworkConnectionToClient(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadDouble(), senderConnection);
			}
		}

		protected void UserCode_RpcSyncGroundDrop__Vector3__Quaternion__Double(Vector3 position, Quaternion rotation, double finalizeTime)
		{
			SetPlacementStreamFinalized(finalizeTime);
			_streamTargetPos = position;
			_streamTargetRot = rotation;
			EndSnapFollowLocal();
			base.transform.SetPositionAndRotation(position, rotation);
		}

		protected static void InvokeUserCode_RpcSyncGroundDrop__Vector3__Quaternion__Double(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSyncGroundDrop called on server.");
			}
			else
			{
				((HeldItem)obj).UserCode_RpcSyncGroundDrop__Vector3__Quaternion__Double(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadDouble());
			}
		}

		protected void UserCode_CmdStreamPlacementPose__Vector3__Quaternion__Double(Vector3 pos, Quaternion rot, double sendTime)
		{
			ApplyPlacementStream(pos, rot, sendTime);
			RpcStreamPlacementPose(pos, rot, sendTime);
		}

		protected static void InvokeUserCode_CmdStreamPlacementPose__Vector3__Quaternion__Double(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStreamPlacementPose called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdStreamPlacementPose__Vector3__Quaternion__Double(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadDouble());
			}
		}

		protected void UserCode_RpcStreamPlacementPose__Vector3__Quaternion__Double(Vector3 pos, Quaternion rot, double sendTime)
		{
			if (!base.isServer)
			{
				ApplyPlacementStream(pos, rot, sendTime);
			}
		}

		protected static void InvokeUserCode_RpcStreamPlacementPose__Vector3__Quaternion__Double(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStreamPlacementPose called on server.");
			}
			else
			{
				((HeldItem)obj).UserCode_RpcStreamPlacementPose__Vector3__Quaternion__Double(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadDouble());
			}
		}

		protected void UserCode_CmdSetSnapStream__Boolean(bool active)
		{
			Network_snapStreamActive = active;
		}

		protected static void InvokeUserCode_CmdSetSnapStream__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSnapStream called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdSetSnapStream__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_CmdStreamSnapPose__Vector3__Quaternion(Vector3 pos, Quaternion rot)
		{
			ApplySnapStream(pos, rot);
			RpcStreamSnapPose(pos, rot);
		}

		protected static void InvokeUserCode_CmdStreamSnapPose__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStreamSnapPose called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdStreamSnapPose__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_RpcStreamSnapPose__Vector3__Quaternion(Vector3 pos, Quaternion rot)
		{
			if (!base.isServer)
			{
				ApplySnapStream(pos, rot);
			}
		}

		protected static void InvokeUserCode_RpcStreamSnapPose__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcStreamSnapPose called on server.");
			}
			else
			{
				((HeldItem)obj).UserCode_RpcStreamSnapPose__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdSetSnappedDescendantsTriggered__Boolean(bool triggered)
		{
			ServerSetSnappedDescendantsTriggered(triggered);
		}

		protected static void InvokeUserCode_CmdSetSnappedDescendantsTriggered__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSnappedDescendantsTriggered called on client.");
			}
			else
			{
				((HeldItem)obj).UserCode_CmdSetSnappedDescendantsTriggered__Boolean(reader.ReadBool());
			}
		}

		static HeldItem()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetPlacementNetworkData(System.UInt32,System.Byte)", InvokeUserCode_CmdSetPlacementNetworkData__UInt32__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdStartPlacing(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdStartPlacing__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdRelease()", InvokeUserCode_CmdRelease, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdSyncGroundDrop(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdSyncGroundDrop__Vector3__Quaternion__Double__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdStreamPlacementPose(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", InvokeUserCode_CmdStreamPlacementPose__Vector3__Quaternion__Double, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetSnapStream(System.Boolean)", InvokeUserCode_CmdSetSnapStream__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdStreamSnapPose(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdStreamSnapPose__Vector3__Quaternion, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::CmdSetSnappedDescendantsTriggered(System.Boolean)", InvokeUserCode_CmdSetSnappedDescendantsTriggered__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::RpcSyncGroundDrop(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", InvokeUserCode_RpcSyncGroundDrop__Vector3__Quaternion__Double);
			RemoteProcedureCalls.RegisterRpc(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::RpcStreamPlacementPose(UnityEngine.Vector3,UnityEngine.Quaternion,System.Double)", InvokeUserCode_RpcStreamPlacementPose__Vector3__Quaternion__Double);
			RemoteProcedureCalls.RegisterRpc(typeof(HeldItem), "System.Void NomadDrive.Features.Interaction.HeldItem::RpcStreamSnapPose(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcStreamSnapPose__Vector3__Quaternion);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(writer, _placementNetworkData);
				writer.WriteBool(_isEquipped);
				writer.WriteBool(_snapStreamActive);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(writer, _placementNetworkData);
			}
			if ((syncVarDirtyBits & 0x80L) != 0L)
			{
				writer.WriteBool(_isEquipped);
			}
			if ((syncVarDirtyBits & 0x100L) != 0L)
			{
				writer.WriteBool(_snapStreamActive);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _placementNetworkData, _Mirror_SyncVarHookDelegate__placementNetworkData, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(reader));
				GeneratedSyncVarDeserialize(ref _isEquipped, _Mirror_SyncVarHookDelegate__isEquipped, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _snapStreamActive, _Mirror_SyncVarHookDelegate__snapStreamActive, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _placementNetworkData, _Mirror_SyncVarHookDelegate__placementNetworkData, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(reader));
			}
			if ((num & 0x80L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isEquipped, _Mirror_SyncVarHookDelegate__isEquipped, reader.ReadBool());
			}
			if ((num & 0x100L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _snapStreamActive, _Mirror_SyncVarHookDelegate__snapStreamActive, reader.ReadBool());
			}
		}
	}
}
