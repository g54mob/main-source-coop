using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Attachables.UI;
using NomadDrive.Features.ColliderLOD;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Tools;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Attachables
{
	public class AttachableObject : HeldItem
	{
		[Header("Attachable Settings")]
		public List<ObjectSlotType> targetSlots;

		[Header("Audio")]
		[SerializeField]
		private SoundID attachSound;

		[SerializeField]
		private SoundID detachSound;

		[Header("Events")]
		public UnityEvent OnAttached;

		public UnityEvent OnDetached;

		public UnityEvent OnRepaired;

		private ConditionComponent _conditionComponent;

		[SyncVar(hook = "OnIsAttachedChangedInternal")]
		private bool _isAttached;

		private Vector3 _originalLocalScale = Vector3.one;

		private InteractionStateMachine<AttachableState> _attachableStateMachine;

		[Inject]
		private InputActionPromptsPanel _inputActionPromptsPanel;

		[Inject]
		private ObjectInfoPanel _objectInfoPanel;

		[Inject]
		private IGameUIManager _guiManager;

		[Inject]
		private AttachableAudioFallbackConfig _audioFallback;

		protected RepairKit HoveredRepairKit;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isAttached;

		public string ObjectName { get; set; }

		public ConditionComponent ConditionComponent => _conditionComponent;

		public IObjectSlot ObjectSlot { get; set; }

		public bool IsAttached => _isAttached;

		public Vector3 OriginalLocalScale => _originalLocalScale;

		public virtual Transform HighlightSource => base.transform;

		protected new InteractionStateMachine<AttachableState> StateMachine => _attachableStateMachine;

		public new AttachableState CurrentState => _attachableStateMachine?.CurrentState ?? AttachableState.DetachedNoTool;

		protected override bool UseDefaultStateMachine => false;

		public override bool ParticipatesInColliderLod => !IsAttached;

		protected override bool UseStateMachine => true;

		public bool Network_isAttached
		{
			get
			{
				return _isAttached;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isAttached, 512uL, _Mirror_SyncVarHookDelegate__isAttached);
			}
		}

		public static event Action<AttachableObject> OnAnyAttachedGlobal;

		protected override void Awake()
		{
			TryGetComponent<ConditionComponent>(out _conditionComponent);
			base.Awake();
			_originalLocalScale = base.transform.localScale;
			CaptureInitialAnimatedPoses();
			Validate();
		}

		protected virtual void CaptureInitialAnimatedPoses()
		{
		}

		protected virtual void ResetAnimatedPoseOnDetach()
		{
		}

		private void Validate()
		{
			if (targetSlots == null || targetSlots.Count == 0)
			{
				EvilLogger.LogError("AttachableObject " + base.name + " has no target slots", "Validate", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Attachables\\AttachableObject.cs", 154);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			OnRepaired.AddListener(OnRepair);
			OnAttached.AddListener(OnAttach);
			OnDetached.AddListener(OnDetach);
			SyncColliderLodParticipation();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			OnRepaired.RemoveListener(OnRepair);
			OnAttached.RemoveListener(OnAttach);
			OnDetached.RemoveListener(OnDetach);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			UpdateState();
		}

		protected override void InitializeStateMachine()
		{
			_attachableStateMachine = new InteractionStateMachine<AttachableState>(this);
			base.BaseStateMachine = _attachableStateMachine;
			ConfigureStates();
			_attachableStateMachine.Initialize(DetermineAttachableState());
		}

		protected override void ConfigureStates()
		{
			_attachableStateMachine.RegisterState(AttachableState.DetachedNoTool, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(AttachableState.DetachedWithTool, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithHoldInteraction(InteractionKey.Secondary, "@interaction.repair", HandleRepair, 0.5f)
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(AttachableState.AttachedNoTool, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach()).WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true))
				.RegisterState(AttachableState.AttachedWithTool, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Primary, "@interaction.repair", HandleRepair, 0.5f).WithHoldInteraction(InteractionKey.Secondary, "@interaction.detach", HandleDetach, 0.5f).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && CanDetach())
					.WithCrosshair(CrosshairType.Interact)
					.WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: true));
		}

		protected AttachableState DetermineAttachableState()
		{
			bool flag = HasCompatibleRepairKit();
			if (_isAttached)
			{
				if (!flag)
				{
					return AttachableState.AttachedNoTool;
				}
				return AttachableState.AttachedWithTool;
			}
			if (!flag)
			{
				return AttachableState.DetachedNoTool;
			}
			return AttachableState.DetachedWithTool;
		}

		public override void UpdateState()
		{
			if (_attachableStateMachine != null)
			{
				_attachableStateMachine.TransitionTo(DetermineAttachableState());
			}
		}

		protected bool HasCompatibleRepairKit()
		{
			if (_conditionComponent == null)
			{
				return false;
			}
			RepairKit equippedRepairKit = GetEquippedRepairKit();
			if (equippedRepairKit != null)
			{
				return targetSlots.Contains(equippedRepairKit.targetMechanicalPart);
			}
			return false;
		}

		protected RepairKit GetEquippedRepairKit()
		{
			if (playerService?.EquipmentManager?.EquippedEntity == null)
			{
				return null;
			}
			if (!playerService.EquipmentManager.EquippedEntity.TryGetComponent<RepairKit>(out var component))
			{
				return null;
			}
			return component;
		}

		protected override void HandleEquip()
		{
			base.HandleEquip();
		}

		protected virtual bool CanDetach()
		{
			return !IsLocalPlayerSitting();
		}

		protected virtual void HandleDetach()
		{
			if (_isAttached)
			{
				ObjectSlot?.Detach();
				CmdDetach();
			}
		}

		protected virtual void HandleRepair()
		{
			if (!(HoveredRepairKit == null))
			{
				playerService.EquipmentManager.Consume();
				CmdRepair(HoveredRepairKit.netId);
				OnRepaired.Invoke();
				ObjectivesEventBus.Raise(ObjectiveSignal.PartRepaired, this);
				UpdateState();
			}
		}

		protected virtual void OnRepair()
		{
		}

		protected virtual void OnAttach()
		{
			SoundID id = (attachSound.IsValid() ? attachSound : ((_audioFallback != null) ? _audioFallback.DefaultAttachSound : default(SoundID)));
			if (id.IsValid())
			{
				AudioManager?.PlayOneShot(id, base.transform.position);
			}
		}

		protected virtual void OnDetach()
		{
			SoundID id = (detachSound.IsValid() ? detachSound : ((_audioFallback != null) ? _audioFallback.DefaultDetachSound : default(SoundID)));
			if (id.IsValid())
			{
				AudioManager?.PlayOneShot(id, base.transform.position);
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			if (_conditionComponent != null)
			{
				_objectInfoPanel?.SetObject(_conditionComponent);
				_guiManager?.ShowCanvasGroup(GameCanvasGroupName.AttachableObjectInfo, interactable: false, blockRaycast: false);
			}
			RepairKit equippedRepairKit = GetEquippedRepairKit();
			if (equippedRepairKit != null && targetSlots.Contains(equippedRepairKit.targetMechanicalPart))
			{
				HoveredRepairKit = equippedRepairKit;
			}
			UpdateState();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			HoveredRepairKit = null;
			if (_conditionComponent != null)
			{
				_objectInfoPanel?.SetObject(null);
				_guiManager?.HideCanvasGroup(GameCanvasGroupName.AttachableObjectInfo);
			}
			DisableOutline();
		}

		private void OnIsAttachedChangedInternal(bool oldValue, bool newValue)
		{
			SyncColliderLodParticipation();
			if (IsLateJoinCompleted)
			{
				if (newValue)
				{
					ExecuteAttachActions();
					AttachableObject.OnAnyAttachedGlobal?.Invoke(this);
				}
				else
				{
					ExecuteDetachActions();
				}
				OnIsAttachedChanged(oldValue, newValue);
			}
		}

		private void SyncColliderLodParticipation()
		{
			if (IsAttached)
			{
				ColliderLodRegistry.Unregister(this);
			}
			else
			{
				ColliderLodRegistry.Register(this);
			}
		}

		protected virtual void OnIsAttachedChanged(bool oldValue, bool newValue)
		{
		}

		public void Attach()
		{
			if (!_isAttached)
			{
				CmdAttach();
			}
		}

		public virtual void ExecuteAttachActions()
		{
			SetInteractionAvailability(newValue: true);
			_inputActionPromptsPanel.Hide();
			OnAttached.Invoke();
			UpdateState();
			if (base.isServer)
			{
				EnableRigidCollidersAfterSettleAsync().Forget();
			}
		}

		private async UniTaskVoid EnableRigidCollidersAfterSettleAsync()
		{
			await UniTask.DelayFrame(5, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (!(this == null) && _isAttached)
			{
				SetRigidCollidersTriggered(newValue: false);
			}
		}

		public virtual void ExecuteDetachActions()
		{
			ResetAnimatedPoseOnDetach();
			if (!base.IsEquipped)
			{
				UnignoreHovering();
			}
			DisableOutline();
			OnDetached.Invoke();
			UpdateState();
		}

		[Command(requiresAuthority = false)]
		private void CmdAttach()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.AttachableObject::CmdAttach()", -1309656292, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		protected void CmdDetach()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.AttachableObject::CmdDetach()", 1421034734, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerDetach()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Attachables.AttachableObject::ServerDetach()' called when server was not active");
			}
			else
			{
				Network_isAttached = false;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdRepair(uint usedRepairKitId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(usedRepairKitId);
			SendCommandInternal("System.Void NomadDrive.Features.Attachables.AttachableObject::CmdRepair(System.UInt32)", 2082776506, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public AttachableObject()
		{
			_Mirror_SyncVarHookDelegate__isAttached = OnIsAttachedChangedInternal;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAttach()
		{
			Network_isAttached = true;
		}

		protected static void InvokeUserCode_CmdAttach(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAttach called on client.");
			}
			else
			{
				((AttachableObject)obj).UserCode_CmdAttach();
			}
		}

		protected void UserCode_CmdDetach()
		{
			Network_isAttached = false;
		}

		protected static void InvokeUserCode_CmdDetach(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDetach called on client.");
			}
			else
			{
				((AttachableObject)obj).UserCode_CmdDetach();
			}
		}

		protected void UserCode_CmdRepair__UInt32(uint usedRepairKitId)
		{
			if (!(_conditionComponent == null))
			{
				networkManager.TryGetNetworkObjectById(usedRepairKitId, out var networkObject);
				if (!(networkObject == null) && networkObject.TryGetComponent<RepairKit>(out var component))
				{
					_conditionComponent.ServerAddCondition(component.repairAmount);
					component.Destroy();
				}
			}
		}

		protected static void InvokeUserCode_CmdRepair__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRepair called on client.");
			}
			else
			{
				((AttachableObject)obj).UserCode_CmdRepair__UInt32(reader.ReadVarUInt());
			}
		}

		static AttachableObject()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(AttachableObject), "System.Void NomadDrive.Features.Attachables.AttachableObject::CmdAttach()", InvokeUserCode_CmdAttach, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(AttachableObject), "System.Void NomadDrive.Features.Attachables.AttachableObject::CmdDetach()", InvokeUserCode_CmdDetach, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(AttachableObject), "System.Void NomadDrive.Features.Attachables.AttachableObject::CmdRepair(System.UInt32)", InvokeUserCode_CmdRepair__UInt32, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isAttached);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_isAttached);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isAttached, _Mirror_SyncVarHookDelegate__isAttached, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isAttached, _Mirror_SyncVarHookDelegate__isAttached, reader.ReadBool());
			}
		}
	}
}
