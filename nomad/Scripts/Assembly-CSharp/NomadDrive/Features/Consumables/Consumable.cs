using System;
using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction.UI;
using NomadDrive.Features.Player;
using NomadDrive.Features.Tools;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Consumables
{
	public abstract class Consumable : DirectHeldItem
	{
		public ConsumableConfig consumableConfig;

		[Header("Audio")]
		[SerializeField]
		private SoundID consumeSound;

		public SerializableDictionary<PlayerStatType, float> currentPlayerStatCollection;

		[Inject]
		private InputActionPromptsPanel _inputActionPromptsPanel;

		protected override void Start()
		{
			base.Start();
			if (consumableConfig == null)
			{
				throw new Exception("ConsumableConfig is not set for " + base.gameObject.name);
			}
			currentPlayerStatCollection = new SerializableDictionary<PlayerStatType, float>();
			foreach (KeyValuePair<PlayerStatType, float> item in consumableConfig.playerStatCollection)
			{
				currentPlayerStatCollection.Add(item.Key, item.Value);
			}
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			_inputActionPromptsPanel.Hide();
			playerService.StatsManager.ApplyConsumableEffects(this);
			if (consumeSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(consumeSound, base.transform.position);
			}
			uint consumedNetId = base.netId;
			playerService.EquipmentManager.Consume();
			CmdDestroyConsumed(consumedNetId);
		}

		[Command(requiresAuthority = false)]
		private void CmdDestroyConsumed(uint consumedNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(consumedNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.Consumable::CmdDestroyConsumed(System.UInt32)", 99959648, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdDestroyConsumed__UInt32(uint consumedNetId)
		{
			if (!networkManager.TryGetNetworkObjectById(consumedNetId, out var networkObject))
			{
				EvilLogger.LogError($"[Consumable] CmdDestroyConsumed: Could not find object {consumedNetId}", "CmdDestroyConsumed", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Consumables\\Scripts\\Consumable.cs", 60);
			}
			else
			{
				NetworkServer.Destroy(networkObject);
			}
		}

		protected static void InvokeUserCode_CmdDestroyConsumed__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDestroyConsumed called on client.");
			}
			else
			{
				((Consumable)obj).UserCode_CmdDestroyConsumed__UInt32(reader.ReadVarUInt());
			}
		}

		static Consumable()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Consumable), "System.Void NomadDrive.Features.Consumables.Consumable::CmdDestroyConsumed(System.UInt32)", InvokeUserCode_CmdDestroyConsumed__UInt32, requiresAuthority: false);
		}
	}
}
