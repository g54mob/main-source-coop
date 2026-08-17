using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Cooking
{
	public class CookingPotSlot : SnappingPlane
	{
		[SyncVar(hook = "OnSlotStateByteChanged")]
		private byte _slotStateByte;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__slotStateByte;

		protected UnityEvent OnCookingPotSlotIgnited { get; } = new UnityEvent();

		protected UnityEvent OnCookingPotSlotExtinguished { get; } = new UnityEvent();

		protected UnityEvent OnCookingPotSlotLateJoinerIgnited { get; } = new UnityEvent();

		public CookingPotSlotState SlotState
		{
			get
			{
				return (CookingPotSlotState)_slotStateByte;
			}
			set
			{
				Network_slotStateByte = (byte)value;
			}
		}

		public byte Network_slotStateByte
		{
			get
			{
				return _slotStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _slotStateByte, 1uL, _Mirror_SyncVarHookDelegate__slotStateByte);
			}
		}

		private void OnSlotStateByteChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				switch (newValue)
				{
				case 1:
					OnCookingPotSlotIgnited.Invoke();
					break;
				case 0:
					OnCookingPotSlotExtinguished.Invoke();
					break;
				}
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_slotStateByte == 1)
			{
				OnCookingPotSlotLateJoinerIgnited.Invoke();
			}
		}

		public void SetSlotState(CookingPotSlotState newSlotState)
		{
			CmdSetSlotState(newSlotState);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetSlotState(CookingPotSlotState newSlotState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState(writer, newSlotState);
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.CookingPotSlot::CmdSetSlotState(NomadDrive.Features.Cooking.CookingPotSlotState)", 649627015, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public CookingPotSlot()
		{
			_Mirror_SyncVarHookDelegate__slotStateByte = OnSlotStateByteChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetSlotState__CookingPotSlotState(CookingPotSlotState newSlotState)
		{
			SlotState = newSlotState;
		}

		protected static void InvokeUserCode_CmdSetSlotState__CookingPotSlotState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSlotState called on client.");
			}
			else
			{
				((CookingPotSlot)obj).UserCode_CmdSetSlotState__CookingPotSlotState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState(reader));
			}
		}

		static CookingPotSlot()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(CookingPotSlot), "System.Void NomadDrive.Features.Cooking.CookingPotSlot::CmdSetSlotState(NomadDrive.Features.Cooking.CookingPotSlotState)", InvokeUserCode_CmdSetSlotState__CookingPotSlotState, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _slotStateByte);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _slotStateByte);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _slotStateByte, _Mirror_SyncVarHookDelegate__slotStateByte, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _slotStateByte, _Mirror_SyncVarHookDelegate__slotStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
