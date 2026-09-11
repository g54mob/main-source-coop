using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zorro.Core.Serizalization;

namespace Zorro.PhotonUtility
{
	public static class CustomCommands<CommandEnum> where CommandEnum : struct, IConvertible
	{
		public static bool SendPackage(CustomCommandPackage<CommandEnum> commandPackage, ReceiverGroup receiverGroup)
		{
			RaiseEventOptions raiseEventOptions = RaiseEventOptions.Default;
			raiseEventOptions.Receivers = receiverGroup;
			return SendPackage(commandPackage, raiseEventOptions);
		}

		public static bool SendPackage(CustomCommandPackage<CommandEnum> commandPackage, Player player)
		{
			RaiseEventOptions raiseEventOptions = RaiseEventOptions.Default;
			raiseEventOptions.TargetActors = new int[1] { player.ActorNumber };
			return SendPackage(commandPackage, raiseEventOptions);
		}

		public static bool SendPackage(CustomCommandPackage<CommandEnum> commandPackage, RaiseEventOptions eventOptions)
		{
			byte commandEventCode = commandPackage.GetCommandEventCode();
			BinarySerializer binarySerializer = commandPackage.Serialize();
			byte[] eventContent = binarySerializer.buffer.ToArray();
			binarySerializer.Dispose();
			SendOptions sendOptions = commandPackage.GetSendOptions();
			NetworkStatistics.AddEvent_Called(commandPackage.GetCommandType().ToString());
			return PhotonNetwork.RaiseEvent(commandEventCode, eventContent, eventOptions, sendOptions);
		}

		public static T SpawnCommandListener<T>() where T : CustomCommandListener<CommandEnum>
		{
			return new GameObject("CustomCommandListener").AddComponent<T>();
		}

		public static ListenerHandle RegisterListener<T>(Action<T> onReceived) where T : CustomCommandPackage<CommandEnum>
		{
			return CustomCommandListener<CommandEnum>.RegisterListener(onReceived);
		}

		public static void UnregisterListener(ListenerHandle handle)
		{
			CustomCommandListener<CommandEnum>.UnregisterListener(handle);
		}
	}
}
