using System;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules.Slots
{
	public abstract class VehicleSlot : VehicleInteractableBase, IObjectSlot
	{
		[Header("Slot")]
		[SerializeField]
		private VehicleSlotId _slotId;

		[SerializeField]
		public ObjectSlotType slotType;

		[SerializeField]
		protected AttachableObject hoveredObject;

		[SerializeField]
		protected AttachableObject attachedObject;

		private AttachableObject _pendingAttachObject;

		private int _attachEpoch;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IPlayerService _playerReferenceService;

		[Inject]
		private INetworkObjectSpawnWatcher _spawnWatcher;

		private StaticInteractionStateMachine<VehicleSlotState> _stateMachine;

		private bool _isSlotActive = true;

		private HighlightModel _highlightModel;

		public NetworkedTransform NetworkedTransform { get; private set; }

		public ObjectSlotType SlotType => slotType;

		public VehicleSlotId SlotId => _slotId;

		public VehicleSlotState CurrentSlotState => _stateMachine?.CurrentState ?? VehicleSlotState.Empty;

		public bool IsOccupied => attachedObject != null;

		protected UnityEvent OnObjectAttached { get; set; } = new UnityEvent();

		protected UnityEvent OnObjectDetached { get; set; } = new UnityEvent();

		protected StaticInteractionStateMachine<VehicleSlotState> SlotStateMachine => _stateMachine;

		protected override bool UseStateMachine => true;

		protected override bool HasNetworkState => false;

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<VehicleSlotState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineSlotState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(VehicleSlotState.Empty, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Interact)).RegisterState(VehicleSlotState.EmptyWithCompatible, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.attach", HandleAttach, 0.31f).WithCrosshair(CrosshairType.Interact).WithInteractionLabelVisibility(visible: true)
				.WithNameLabelVisibility(visible: false)).RegisterState(VehicleSlotState.Occupied, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		protected virtual VehicleSlotState DetermineSlotState()
		{
			if (attachedObject != null)
			{
				return VehicleSlotState.Occupied;
			}
			if (HasCompatibleEquippedObject())
			{
				return VehicleSlotState.EmptyWithCompatible;
			}
			return VehicleSlotState.Empty;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineSlotState());
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
				EvilLogger.LogError("NetworkedTransform component missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\Slots\\VehicleSlot.cs", 122);
			}
		}

		protected override void Start()
		{
			base.NetworkSync?.RegisterSlot(_slotId, this);
			IgnoreHovering();
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

		public override void OnHovered()
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

		public override void OnUnhovered()
		{
			base.OnUnhovered();
			hoveredObject = null;
		}

		protected virtual void HandleAttach()
		{
			if (!(hoveredObject == null) && !(attachedObject != null))
			{
				AttachableObject attachableObject = (_pendingAttachObject = hoveredObject);
				base.NetworkSync.CmdAttachToSlot((byte)_slotId, attachableObject.netId);
				attachableObject.Unequip();
				hoveredObject = null;
				SetTriggerCollidersEnabled(enabled: false);
			}
		}

		public void OnAttachRejected(uint objectNetId)
		{
			if (!(_pendingAttachObject == null) && _pendingAttachObject.netId == objectNetId)
			{
				AttachableObject pendingAttachObject = _pendingAttachObject;
				_pendingAttachObject = null;
				pendingAttachObject.Equip();
				SetTriggerCollidersEnabled(enabled: true);
				UpdateState();
			}
		}

		public void Detach()
		{
			IgnoreHovering();
			if (attachedObject == null)
			{
				EvilLogger.LogError("Attached object is null in Detach", "Detach", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\Slots\\VehicleSlot.cs", 209);
				return;
			}
			attachedObject.Equip();
			base.NetworkSync.CmdDetachFromSlot((byte)_slotId);
		}

		public void ServerAttach(uint objectNetId)
		{
			base.NetworkSync.ServerAttachToSlot((byte)_slotId, objectNetId);
		}

		public void ServerDetach()
		{
			base.NetworkSync.ServerDetachFromSlot((byte)_slotId);
		}

		public void AttachFromPlacement(uint objectNetId)
		{
			if (!(attachedObject != null))
			{
				base.NetworkSync.CmdAttachToSlot((byte)_slotId, objectNetId);
				SetTriggerCollidersEnabled(enabled: false);
			}
		}

		public void OnNetworkAttachmentChanged(uint newNetId)
		{
			int epoch = ++_attachEpoch;
			if (newNetId == 0)
			{
				HandleNetworkDetachment();
			}
			else
			{
				HandleNetworkAttachmentAsync(newNetId, epoch).Forget();
			}
		}

		private void HandleNetworkDetachment()
		{
			if (attachedObject != null)
			{
				attachedObject.transform.localScale = attachedObject.OriginalLocalScale;
				OnObjectDetached.Invoke();
				attachedObject = null;
			}
			SetInteractionAvailability(newValue: true);
			SetTriggerCollidersEnabled(enabled: true);
			UpdateState();
		}

		private async UniTaskVoid HandleNetworkAttachmentAsync(uint newNetId, int epoch)
		{
			(bool, GameObject) tuple = await _networkManager.TryGetNetworkObjectByIdAsync(newNetId, 50, 100, this.GetCancellationTokenOnDestroy());
			if (epoch == _attachEpoch && base.NetworkSync.GetSlotAttachment(_slotId) == newNetId)
			{
				if (!tuple.Item1)
				{
					WatchForPart(newNetId, epoch);
				}
				else
				{
					ApplyResolvedAttachment(tuple.Item2, newNetId, epoch);
				}
			}
		}

		private void WatchForPart(uint netId, int epoch)
		{
			if (_spawnWatcher == null)
			{
				return;
			}
			_spawnWatcher.RegisterPendingLookup(netId, delegate(GameObject spawnedObject)
			{
				if (spawnedObject != null)
				{
					ApplyResolvedAttachment(spawnedObject, netId, epoch);
				}
			}, "VehicleSlot:" + base.name, 120f);
		}

		private void ApplyResolvedAttachment(GameObject networkObject, uint netId, int epoch)
		{
			if (this == null || base.NetworkSync == null || epoch != _attachEpoch || base.NetworkSync.GetSlotAttachment(_slotId) != netId)
			{
				return;
			}
			if (!networkObject.TryGetComponent<AttachableObject>(out var component))
			{
				EvilLogger.LogError($"[VehicleSlot] Network object with ID {netId} has no AttachableObject component", "ApplyResolvedAttachment", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\Slots\\VehicleSlot.cs", 326);
				return;
			}
			attachedObject = component;
			_pendingAttachObject = null;
			attachedObject.ObjectSlot = this;
			attachedObject.Attach();
			try
			{
				component.NetworkedTransform.SetParent(subTransformIndex: NetworkedTransform.NetworkedTransformIndex, parentingConfig: new NetworkedTransformParentingConfig
				{
					PositionOffset = Vector3.zero,
					RotationOffset = Vector3.zero,
					KeepPositionAxes = false,
					KeepRotationAxes = false
				}, parent: NetworkedTransform);
				component.transform.localScale = Vector3.Scale(base.transform.lossyScale, component.OriginalLocalScale);
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[VehicleSlot] Parenting failed for " + base.gameObject.name + ": " + ex.Message, "ApplyResolvedAttachment", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\Slots\\VehicleSlot.cs", 351);
			}
			OnObjectAttached.Invoke();
			UpdateState();
		}

		public virtual void Deactivate()
		{
			_isSlotActive = false;
			IgnoreHovering();
			SetTriggerCollidersEnabled(enabled: false);
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
			SetTriggerCollidersEnabled(enabled: true);
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

		protected virtual Quaternion AdjustHighlightLocalRotation(Quaternion localRotation)
		{
			return localRotation;
		}

		public virtual void SetupHighlightModel(Transform sourceModel, Vector3 localPosition, Quaternion localRotation, Material overrideMaterial = null)
		{
			ClearHighlightModel();
			base.ModelTransform.localPosition = localPosition;
			base.ModelTransform.localRotation = AdjustHighlightLocalRotation(localRotation);
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

		public void RefreshAttachedPartInteraction()
		{
			if (attachedObject != null)
			{
				attachedObject.RefreshInteractionState();
			}
		}

		public virtual void DriveAttachedPartPose(Transform vehicleRoot)
		{
			if (attachedObject == null)
			{
				return;
			}
			NetworkedTransform networkedTransform = attachedObject.NetworkedTransform;
			if (!(networkedTransform == null))
			{
				Transform parentTransform = networkedTransform.ParentTransform;
				if (!(parentTransform == null) && parentTransform.IsChildOf(vehicleRoot))
				{
					attachedObject.transform.SetPositionAndRotation(parentTransform.position, parentTransform.rotation);
				}
			}
		}

		public virtual void ApplyAttachPreview(AttachableObject attachable)
		{
		}

		public virtual void ClearAttachPreview(AttachableObject attachable)
		{
		}
	}
}
