using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.Downed;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.SaveSystem
{
	public class PlayerSaveSync : NetworkBehaviour, IPlayerComponent
	{
		private const float ReportIntervalSeconds = 3f;

		private const byte SurvivalBlobVersion = 1;

		[Inject]
		private IGameSaveService _gameSave;

		private NomadDrive.Features.Player.Player _player;

		private PlayerStatsManager _stats;

		private EquipmentManager _equipment;

		public int SetupPriority => 40;

		private void Awake()
		{
			_player = GetComponent<NomadDrive.Features.Player.Player>();
			_stats = GetComponent<PlayerStatsManager>();
			_equipment = GetComponent<EquipmentManager>();
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer && base.isOwned && !base.isServer)
			{
				ClientOwnerFlowAsync(this.GetCancellationTokenOnDestroy()).Forget();
			}
		}

		private async UniTaskVoid ClientOwnerFlowAsync(CancellationToken ct)
		{
			for (int i = 0; i < 200; i++)
			{
				if (!(_stats == null) && _stats.IsInitialized)
				{
					break;
				}
				await UniTask.Delay(100, ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
			}
			if (_stats != null && _stats.IsInitialized)
			{
				CmdRequestRestore();
			}
			while (!ct.IsCancellationRequested)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(3.0), ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
				PushSnapshot();
			}
		}

		private void PushSnapshot()
		{
			if (!(_stats == null) && _stats.IsInitialized)
			{
				CmdReport((_player != null) ? _player.DisplayName : string.Empty, BuildSurvivalBlob(), ResolveEquippedNetId());
			}
		}

		private uint ResolveEquippedNetId()
		{
			if (_equipment == null || !_equipment.IsItemEquipped)
			{
				return 0u;
			}
			HeldItem equippedEntity = _equipment.EquippedEntity;
			if (!(equippedEntity != null))
			{
				return 0u;
			}
			return equippedEntity.netId;
		}

		private byte[] BuildSurvivalBlob()
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (EvilWriter evilWriter = new EvilWriter(memoryStream))
			{
				evilWriter.Write((byte)1);
				evilWriter.Write(_stats.Nutrition.CurrentValue);
				evilWriter.Write(_stats.Hydration.CurrentValue);
				evilWriter.Write(_stats.Energy.CurrentValue);
				evilWriter.Write(_stats.Health.CurrentValue);
				evilWriter.Write(_stats.Poison.CurrentValue);
			}
			return memoryStream.ToArray();
		}

		[Command]
		private void CmdReport(string displayName, byte[] survivalBlob, uint equippedNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(displayName);
			writer.WriteBytesAndSize(survivalBlob);
			writer.WriteVarUInt(equippedNetId);
			SendCommandInternal("System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::CmdReport(System.String,System.Byte[],System.UInt32)", 1842130977, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdRequestRestore()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::CmdRequestRestore()", -341810375, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetApplyRestore(NetworkConnectionToClient target, byte[] survivalBlob, uint equippedNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBytesAndSize(survivalBlob);
			writer.WriteVarUInt(equippedNetId);
			SendTargetRPCInternal(target, "System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::TargetApplyRestore(Mirror.NetworkConnectionToClient,System.Byte[],System.UInt32)", 1604125067, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void ApplySurvivalBlob(byte[] blob)
		{
			if (blob == null || blob.Length == 0 || _stats == null || !_stats.IsInitialized)
			{
				return;
			}
			try
			{
				using MemoryStream stream = new MemoryStream(blob);
				using EvilReader evilReader = new EvilReader(stream);
				evilReader.ReadByte();
				float nutrition = evilReader.ReadFloat();
				float hydration = evilReader.ReadFloat();
				float energy = evilReader.ReadFloat();
				float health = evilReader.ReadFloat();
				float poison = evilReader.ReadFloat();
				_stats.RestoreFromSave(nutrition, hydration, energy, health, poison);
			}
			catch (Exception)
			{
			}
		}

		private void ApplyEquip(uint itemNetId)
		{
			if (!(_equipment == null) && NetworkClient.spawned.TryGetValue(itemNetId, out var value) && !(value == null) && value.TryGetComponent<HeldItem>(out var component))
			{
				_equipment.Equip(component);
			}
		}

		private static string ResolveGuidFromNetId(uint netId)
		{
			if (netId == 0 || !NetworkServer.spawned.TryGetValue(netId, out var value) || value == null)
			{
				return string.Empty;
			}
			PersistentId component = value.GetComponent<PersistentId>();
			if (!(component != null) || !component.HasGuid)
			{
				return string.Empty;
			}
			return component.Guid;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdReport__String__Byte_005B_005D__UInt32(string displayName, byte[] survivalBlob, uint equippedNetId)
		{
			string puid = ((_player != null) ? _player.EosProductUserId : null);
			_gameSave?.UpdateRemotePlayerRecord(puid, displayName, survivalBlob, ResolveGuidFromNetId(equippedNetId));
		}

		protected static void InvokeUserCode_CmdReport__String__Byte_005B_005D__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReport called on client.");
			}
			else
			{
				((PlayerSaveSync)obj).UserCode_CmdReport__String__Byte_005B_005D__UInt32(reader.ReadString(), reader.ReadBytesAndSize(), reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdRequestRestore()
		{
			string text = ((_player != null) ? _player.EosProductUserId : null);
			if (string.IsNullOrEmpty(text) || _gameSave == null || !_gameSave.TryGetRemotePlayerRecord(text, out var survivalBlob, out var equippedItemGuid, out var isDowned, out var downedPosition, out var downedEulerAngles))
			{
				return;
			}
			if (isDowned)
			{
				PlayerDeathController component = GetComponent<PlayerDeathController>();
				if (component != null)
				{
					Vector3 chickenPos = FloatingOriginManager.ToRenderWorld(downedPosition);
					component.ServerForceDowned(chickenPos, Quaternion.Euler(downedEulerAngles));
				}
			}
			else
			{
				uint equippedNetId = 0u;
				if (!string.IsNullOrEmpty(equippedItemGuid))
				{
					PersistentIdRegistry.TryResolveNetId(equippedItemGuid, out equippedNetId);
				}
				TargetApplyRestore(base.connectionToClient, survivalBlob, equippedNetId);
			}
		}

		protected static void InvokeUserCode_CmdRequestRestore(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestRestore called on client.");
			}
			else
			{
				((PlayerSaveSync)obj).UserCode_CmdRequestRestore();
			}
		}

		protected void UserCode_TargetApplyRestore__NetworkConnectionToClient__Byte_005B_005D__UInt32(NetworkConnectionToClient target, byte[] survivalBlob, uint equippedNetId)
		{
			ApplySurvivalBlob(survivalBlob);
			if (equippedNetId != 0)
			{
				ApplyEquip(equippedNetId);
			}
		}

		protected static void InvokeUserCode_TargetApplyRestore__NetworkConnectionToClient__Byte_005B_005D__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetApplyRestore called on server.");
			}
			else
			{
				((PlayerSaveSync)obj).UserCode_TargetApplyRestore__NetworkConnectionToClient__Byte_005B_005D__UInt32(null, reader.ReadBytesAndSize(), reader.ReadVarUInt());
			}
		}

		static PlayerSaveSync()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerSaveSync), "System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::CmdReport(System.String,System.Byte[],System.UInt32)", InvokeUserCode_CmdReport__String__Byte_005B_005D__UInt32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerSaveSync), "System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::CmdRequestRestore()", InvokeUserCode_CmdRequestRestore, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerSaveSync), "System.Void NomadDrive.Features.SaveSystem.PlayerSaveSync::TargetApplyRestore(Mirror.NetworkConnectionToClient,System.Byte[],System.UInt32)", InvokeUserCode_TargetApplyRestore__NetworkConnectionToClient__Byte_005B_005D__UInt32);
		}
	}
}
