using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidCap : LiquidContainerBridge, ILiquidSnapTarget
	{
		[Header("Lid Animation")]
		[SerializeField]
		private Vector3 lidOpenRotation = new Vector3(0f, 0f, 125f);

		[SerializeField]
		private Vector3 lidClosedRotation = new Vector3(0f, 0f, 0f);

		[SerializeField]
		private Ease lidOpenEase = Ease.OutBounce;

		[SerializeField]
		private Ease lidCloseEase = Ease.OutBounce;

		[SerializeField]
		private float lidOpenDuration = 1f;

		[SerializeField]
		private float lidCloseDuration = 1f;

		[Header("Transfer Settings")]
		[SerializeField]
		private float defaultTransferSpeed = 0.75f;

		[Header("Audio")]
		[SerializeField]
		private SoundID capOpenSound;

		[SerializeField]
		private SoundID capCloseSound;

		[Header("Snap")]
		[SerializeField]
		private Transform fillAnchor;

		[FormerlySerializedAs("capType")]
		[SerializeField]
		private LiquidSnapTargetType targetType;

		[SyncVar(hook = "OnLidStateChanged")]
		private bool _isLidOpen;

		[Inject]
		private IPlayerService _playerReferenceService;

		private bool _isProcessCheckable;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isLidOpen;

		public Transform FillAnchor => fillAnchor;

		public LiquidSnapTargetType TargetType => targetType;

		public uint SnapNetId => base.netId;

		public bool Network_isLidOpen
		{
			get
			{
				return _isLidOpen;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isLidOpen, 64uL, _Mirror_SyncVarHookDelegate__isLidOpen);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (base.ModelTransform != null)
			{
				base.ModelTransform.localRotation = Quaternion.Euler(_isLidOpen ? lidOpenRotation : lidClosedRotation);
			}
		}

		[Command(requiresAuthority = false)]
		private void OpenLid()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidCap::OpenLid()", -731843142, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CloseLid()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidCap::CloseLid()", -550386666, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnLidStateChanged(bool oldState, bool newState)
		{
			if (IsLateJoinCompleted && !(base.ModelTransform == null))
			{
				if (newState)
				{
					Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(lidOpenRotation), lidOpenDuration, lidOpenEase);
					return;
				}
				_isProcessCheckable = false;
				Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(lidClosedRotation), lidCloseDuration, lidCloseEase);
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			TriggerSnap(active: true);
			OpenLid();
			UpdateInteractions();
			if (capOpenSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(capOpenSound, base.transform.position);
			}
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			TriggerSnap(active: false);
			CloseLid();
			if (capCloseSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(capCloseSound, base.transform.position);
			}
		}

		private void TriggerSnap(bool active)
		{
			if (RoutedLiquidContainer == null)
			{
				return;
			}
			HeldItem heldItem = _playerReferenceService?.EquipmentManager?.EquippedEntity;
			if (!(heldItem == null) && heldItem.TryGetComponent<LiquidContainerSnapHandler>(out var component))
			{
				if (active)
				{
					component.Snap(this);
				}
				else
				{
					component.Unsnap();
				}
			}
		}

		private void UpdateInteractions()
		{
			if (!_playerReferenceService.EquipmentManager.TryGetEquippedILiquidContainer(out var _))
			{
				DeactivateAllInteractions();
			}
			else
			{
				ActivateAllInteractions();
			}
		}

		public LiquidCap()
		{
			_Mirror_SyncVarHookDelegate__isLidOpen = OnLidStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_OpenLid()
		{
			if (!_isLidOpen)
			{
				Network_isLidOpen = true;
			}
		}

		protected static void InvokeUserCode_OpenLid(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command OpenLid called on client.");
			}
			else
			{
				((LiquidCap)obj).UserCode_OpenLid();
			}
		}

		protected void UserCode_CloseLid()
		{
			if (_isLidOpen)
			{
				Network_isLidOpen = false;
			}
		}

		protected static void InvokeUserCode_CloseLid(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CloseLid called on client.");
			}
			else
			{
				((LiquidCap)obj).UserCode_CloseLid();
			}
		}

		static LiquidCap()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidCap), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidCap::OpenLid()", InvokeUserCode_OpenLid, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidCap), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidCap::CloseLid()", InvokeUserCode_CloseLid, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isLidOpen);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteBool(_isLidOpen);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isLidOpen, _Mirror_SyncVarHookDelegate__isLidOpen, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isLidOpen, _Mirror_SyncVarHookDelegate__isLidOpen, reader.ReadBool());
			}
		}
	}
}
