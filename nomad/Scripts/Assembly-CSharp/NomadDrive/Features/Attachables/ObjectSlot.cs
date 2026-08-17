using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Attachables
{
	public class ObjectSlot : Interactable, IObjectSlot
	{
		[SerializeField]
		public ObjectSlotType slotType;

		[SerializeField]
		protected AttachableObject hoveredObject;

		[SerializeField]
		protected AttachableObject attachedObject;

		[SyncVar(hook = "OnAttachedObjectNetIdChanged")]
		private uint _attachedObjectNetId;

		private bool _isSlotActive = true;

		private int _attachEpoch;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IPlayerService _playerReferenceService;

		[Inject]
		private INetworkObjectSpawnWatcher _spawnWatcher;

		private InteractionStateMachine<ObjectSlotState> _stateMachine;

		private HighlightModel _highlightModel;

		public Action<uint, uint> _Mirror_SyncVarHookDelegate__attachedObjectNetId;

		public ObjectSlotType SlotType => slotType;

		public bool IsOccupied => attachedObject != null;

		public NetworkedTransform NetworkedTransform { get; set; }

		protected UnityEvent OnObjectAttached { get; set; } = new UnityEvent();

		protected UnityEvent OnObjectDetached { get; set; } = new UnityEvent();

		protected InteractionStateMachine<ObjectSlotState> StateMachine => _stateMachine;

		public ObjectSlotState CurrentState => _stateMachine?.CurrentState ?? ObjectSlotState.Empty;

		protected override bool UseStateMachine => true;

		public uint Network_attachedObjectNetId
		{
			get
			{
				return _attachedObjectNetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _attachedObjectNetId, 64uL, _Mirror_SyncVarHookDelegate__attachedObjectNetId);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<ObjectSlotState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineSlotState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(ObjectSlotState.Empty, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Interact)).RegisterState(ObjectSlotState.EmptyWithCompatible, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.attach", HandleAttach, 0.31f).WithCrosshair(CrosshairType.Interact).WithInteractionLabelVisibility(visible: true)
				.WithNameLabelVisibility(visible: false)).RegisterState(ObjectSlotState.Occupied, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		protected virtual ObjectSlotState DetermineSlotState()
		{
			if (attachedObject != null)
			{
				return ObjectSlotState.Occupied;
			}
			if (HasCompatibleEquippedObject())
			{
				return ObjectSlotState.EmptyWithCompatible;
			}
			return ObjectSlotState.Empty;
		}

		public override void UpdateState()
		{
			if (_stateMachine != null)
			{
				_stateMachine.TransitionTo(DetermineSlotState());
			}
		}

		private bool HasCompatibleEquippedObject()
		{
			if (_playerReferenceService?.EquipmentManager?.EquippedEntity == null)
			{
				return false;
			}
			if (!_playerReferenceService.EquipmentManager.IsEquippedObjectAttachable())
			{
				return false;
			}
			if (_playerReferenceService.EquipmentManager.EquippedEntity.gameObject.TryGetComponent<AttachableObject>(out var component))
			{
				return component.targetSlots.Contains(slotType);
			}
			return false;
		}

		protected override void Awake()
		{
			base.Awake();
			if (TryGetComponent<NetworkedTransform>(out var component))
			{
				NetworkedTransform = component;
			}
			else
			{
				EvilLogger.LogError("NetworkedTransform component missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Attachables\\ObjectSlot.cs", 163);
			}
		}

		protected override void Start()
		{
			base.Start();
			IgnoreHovering();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			int epoch = ++_attachEpoch;
			if (_attachedObjectNetId == 0)
			{
				attachedObject = null;
				UpdateState();
			}
			else
			{
				SetForLateJoinerAsync(_attachedObjectNetId, epoch).Forget();
			}
		}

		private async UniTaskVoid SetForLateJoinerAsync(uint netId, int epoch)
		{
			(bool, GameObject) tuple = await _networkManager.TryGetNetworkObjectByIdAsync(netId, 50, 100, this.GetCancellationTokenOnDestroy());
			if (epoch == _attachEpoch && _attachedObjectNetId == netId)
			{
				if (!tuple.Item1)
				{
					WatchForPart(netId, epoch, reparent: false);
				}
				else
				{
					ApplyResolvedAttachment(tuple.Item2, netId, epoch, reparent: false);
				}
			}
		}

		private void OnAttachedObjectNetIdChanged(uint _, uint newAttachedNetId)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			int epoch = ++_attachEpoch;
			if (newAttachedNetId == 0)
			{
				if (attachedObject != null)
				{
					attachedObject.transform.localScale = attachedObject.OriginalLocalScale;
					OnObjectDetached.Invoke();
					attachedObject = null;
				}
				SetInteractionAvailability(newValue: true);
				SetTriggerCollidersEnabled(newValue: true);
				UpdateState();
			}
			else
			{
				HandleAttachmentAsync(newAttachedNetId, epoch).Forget();
			}
		}

		private async UniTaskVoid HandleAttachmentAsync(uint newAttachedNetId, int epoch)
		{
			(bool, GameObject) tuple = await _networkManager.TryGetNetworkObjectByIdAsync(newAttachedNetId, 50, 100, this.GetCancellationTokenOnDestroy());
			if (epoch == _attachEpoch && _attachedObjectNetId == newAttachedNetId)
			{
				if (!tuple.Item1)
				{
					WatchForPart(newAttachedNetId, epoch, reparent: true);
				}
				else
				{
					ApplyResolvedAttachment(tuple.Item2, newAttachedNetId, epoch, reparent: true);
				}
			}
		}

		private void WatchForPart(uint netId, int epoch, bool reparent)
		{
			if (_spawnWatcher == null)
			{
				return;
			}
			_spawnWatcher.RegisterPendingLookup(netId, delegate(GameObject spawnedObject)
			{
				if (spawnedObject != null)
				{
					ApplyResolvedAttachment(spawnedObject, netId, epoch, reparent);
				}
			}, "ObjectSlot:" + base.name, 120f);
		}

		private void ApplyResolvedAttachment(GameObject networkObject, uint netId, int epoch, bool reparent)
		{
			if (this == null || epoch != _attachEpoch || _attachedObjectNetId != netId)
			{
				return;
			}
			if (!networkObject.TryGetComponent<AttachableObject>(out var component))
			{
				EvilLogger.LogError($"<color=red>[ObjectSlot]</color> Network object with ID {netId} has no AttachableObject component", "ApplyResolvedAttachment", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Attachables\\ObjectSlot.cs", 293);
				if (base.isServer)
				{
					Network_attachedObjectNetId = 0u;
				}
				return;
			}
			attachedObject = component;
			attachedObject.Attach();
			attachedObject.ObjectSlot = this;
			if (reparent)
			{
				component.NetworkedTransform.SetParent(subTransformIndex: NetworkedTransform.NetworkedTransformIndex, parentingConfig: new NetworkedTransformParentingConfig
				{
					PositionOffset = Vector3.zero,
					RotationOffset = Vector3.zero,
					KeepPositionAxes = false,
					KeepRotationAxes = false
				}, parent: NetworkedTransform);
			}
			component.transform.localScale = Vector3.Scale(base.transform.lossyScale, component.OriginalLocalScale);
			OnObjectAttached.Invoke();
			UpdateState();
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			if (_isSlotActive)
			{
				if (HasCompatibleEquippedObject())
				{
					GameObject gameObject = _playerReferenceService.EquipmentManager.EquippedEntity.gameObject;
					hoveredObject = gameObject.GetComponent<AttachableObject>();
				}
				UpdateState();
			}
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			hoveredObject = null;
		}

		public virtual void Deactivate()
		{
			_isSlotActive = false;
			IgnoreHovering();
			SetTriggerCollidersEnabled(newValue: false);
			base.isNameLabelVisible = true;
			SetInteractionAvailability(newValue: false);
			if (attachedObject != null)
			{
				attachedObject.isNameLabelVisible = true;
				attachedObject.DeactivateAllInteractions();
				attachedObject.SetInteractionAvailability(newValue: false);
			}
		}

		public virtual void Activate()
		{
			_isSlotActive = true;
			UnignoreHovering();
			SetTriggerCollidersEnabled(newValue: true);
			base.isNameLabelVisible = false;
			if (attachedObject != null)
			{
				SetInteractionAvailability(newValue: false);
				attachedObject.isNameLabelVisible = false;
				attachedObject.ActivateAllInteractions();
				attachedObject.SetInteractionAvailability(newValue: true);
			}
			else
			{
				SetInteractionAvailability(newValue: true);
			}
			UpdateState();
		}

		protected virtual void HandleAttach()
		{
			if (!(hoveredObject == null) && !(attachedObject != null))
			{
				hoveredObject.Unequip();
				CmdAttach(hoveredObject.netId);
				hoveredObject = null;
				SetTriggerCollidersEnabled(newValue: false);
			}
		}

		public void Detach()
		{
			IgnoreHovering();
			if (attachedObject == null)
			{
				EvilLogger.LogError("Attached object is null in Detach", "Detach", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Attachables\\ObjectSlot.cs", 408);
				return;
			}
			attachedObject.Equip();
			CmdDetach();
		}

		[Command(requiresAuthority = false)]
		private void CmdAttach(uint hoveredObjectNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(hoveredObjectNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.ObjectSlot::CmdAttach(System.UInt32)", 1763585071, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdDetach()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.ObjectSlot::CmdDetach()", 1599678185, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerAttach(uint objectNetId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.ObjectSlot::ServerAttach(System.UInt32)' called when server was not active");
			}
			else
			{
				Network_attachedObjectNetId = objectNetId;
			}
		}

		[Server]
		public void ServerDetach()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.ObjectSlot::ServerDetach()' called when server was not active");
			}
			else
			{
				Network_attachedObjectNetId = 0u;
			}
		}

		public void SetupHighlightModel(Transform sourceModel, Vector3 localPosition, Quaternion localRotation, Material overrideMaterial = null)
		{
			ClearHighlightModel();
			base.ModelTransform.localPosition = localPosition;
			base.ModelTransform.localRotation = localRotation;
			if (base.ModelTransform.TryGetComponent<MeshFilter>(out var component))
			{
				component.mesh = null;
			}
			_highlightModel = HighlightModelFactory.CreateFrom(sourceModel, base.ModelTransform);
			Material material = overrideMaterial;
			if (material == null && base.ModelTransform.TryGetComponent<MeshRenderer>(out var component2))
			{
				material = component2.sharedMaterial;
			}
			if (material != null)
			{
				_highlightModel.ApplyMaterial(material);
			}
		}

		public void ClearHighlightModel()
		{
			_highlightModel?.Destroy();
			_highlightModel = null;
		}

		public void ApplyAttachPreview(AttachableObject attachable)
		{
		}

		public void ClearAttachPreview(AttachableObject attachable)
		{
		}

		public void AttachFromPlacement(uint objectNetId)
		{
			if (!(attachedObject != null))
			{
				CmdAttach(objectNetId);
				SetTriggerCollidersEnabled(newValue: false);
			}
		}

		public ObjectSlot()
		{
			_Mirror_SyncVarHookDelegate__attachedObjectNetId = OnAttachedObjectNetIdChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAttach__UInt32(uint hoveredObjectNetId)
		{
			Network_attachedObjectNetId = hoveredObjectNetId;
		}

		protected static void InvokeUserCode_CmdAttach__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAttach called on client.");
			}
			else
			{
				((ObjectSlot)obj).UserCode_CmdAttach__UInt32(reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdDetach()
		{
			Network_attachedObjectNetId = 0u;
		}

		protected static void InvokeUserCode_CmdDetach(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDetach called on client.");
			}
			else
			{
				((ObjectSlot)obj).UserCode_CmdDetach();
			}
		}

		static ObjectSlot()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(ObjectSlot), "System.Void NomadDrive.Features.Attachables.ObjectSlot::CmdAttach(System.UInt32)", InvokeUserCode_CmdAttach__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(ObjectSlot), "System.Void NomadDrive.Features.Attachables.ObjectSlot::CmdDetach()", InvokeUserCode_CmdDetach, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarUInt(_attachedObjectNetId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteVarUInt(_attachedObjectNetId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _attachedObjectNetId, _Mirror_SyncVarHookDelegate__attachedObjectNetId, reader.ReadVarUInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _attachedObjectNetId, _Mirror_SyncVarHookDelegate__attachedObjectNetId, reader.ReadVarUInt());
			}
		}
	}
}
