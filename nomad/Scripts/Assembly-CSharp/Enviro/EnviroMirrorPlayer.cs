using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Enviro
{
	[AddComponentMenu("Enviro 3/Integrations/Mirror Player")]
	[RequireComponent(typeof(NetworkIdentity))]
	public class EnviroMirrorPlayer : NetworkBehaviour
	{
		public bool assignOnStart = true;

		public bool findSceneCamera = true;

		public Camera Camera;

		public void Start()
		{
			if (!base.isLocalPlayer && !base.isServer)
			{
				base.enabled = false;
				return;
			}
			if (Camera == null && findSceneCamera)
			{
				Camera = Camera.main;
			}
			if (base.isLocalPlayer)
			{
				if (assignOnStart && Camera != null)
				{
					EnviroManager.instance.Camera = Camera;
				}
				Cmd_RequestSeason();
				Cmd_RequestCurrentWeather();
			}
		}

		[Command]
		private void Cmd_RequestSeason()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Enviro.EnviroMirrorPlayer::Cmd_RequestSeason()", -708302654, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRequestSeason(int season)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(season);
			SendRPCInternal("System.Void Enviro.EnviroMirrorPlayer::RpcRequestSeason(System.Int32)", 1829990293, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void Cmd_RequestCurrentWeather()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Enviro.EnviroMirrorPlayer::Cmd_RequestCurrentWeather()", -2060441832, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRequestCurrentWeather(int weather)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(weather);
			SendRPCInternal("System.Void Enviro.EnviroMirrorPlayer::RpcRequestCurrentWeather(System.Int32)", 826551891, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_Cmd_RequestSeason()
		{
			if (EnviroManager.instance.Environment != null)
			{
				RpcRequestSeason((int)EnviroManager.instance.Environment.Settings.season);
			}
		}

		protected static void InvokeUserCode_Cmd_RequestSeason(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command Cmd_RequestSeason called on client.");
			}
			else
			{
				((EnviroMirrorPlayer)obj).UserCode_Cmd_RequestSeason();
			}
		}

		protected void UserCode_RpcRequestSeason__Int32(int season)
		{
			if (EnviroManager.instance.Environment != null)
			{
				EnviroManager.instance.Environment.ChangeSeason((EnviroEnvironment.Seasons)season);
			}
		}

		protected static void InvokeUserCode_RpcRequestSeason__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRequestSeason called on server.");
			}
			else
			{
				((EnviroMirrorPlayer)obj).UserCode_RpcRequestSeason__Int32(reader.ReadVarInt());
			}
		}

		protected void UserCode_Cmd_RequestCurrentWeather()
		{
			if (!(EnviroManager.instance.Weather != null))
			{
				return;
			}
			for (int i = 0; i < EnviroManager.instance.Weather.Settings.weatherTypes.Count; i++)
			{
				if (EnviroManager.instance.Weather.Settings.weatherTypes[i] == EnviroManager.instance.Weather.targetWeatherType)
				{
					RpcRequestCurrentWeather(i);
				}
			}
		}

		protected static void InvokeUserCode_Cmd_RequestCurrentWeather(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command Cmd_RequestCurrentWeather called on client.");
			}
			else
			{
				((EnviroMirrorPlayer)obj).UserCode_Cmd_RequestCurrentWeather();
			}
		}

		protected void UserCode_RpcRequestCurrentWeather__Int32(int weather)
		{
			if (EnviroManager.instance.Weather != null)
			{
				EnviroManager.instance.Weather.ChangeWeatherInstant(EnviroManager.instance.Weather.Settings.weatherTypes[weather]);
			}
		}

		protected static void InvokeUserCode_RpcRequestCurrentWeather__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRequestCurrentWeather called on server.");
			}
			else
			{
				((EnviroMirrorPlayer)obj).UserCode_RpcRequestCurrentWeather__Int32(reader.ReadVarInt());
			}
		}

		static EnviroMirrorPlayer()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(EnviroMirrorPlayer), "System.Void Enviro.EnviroMirrorPlayer::Cmd_RequestSeason()", InvokeUserCode_Cmd_RequestSeason, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(EnviroMirrorPlayer), "System.Void Enviro.EnviroMirrorPlayer::Cmd_RequestCurrentWeather()", InvokeUserCode_Cmd_RequestCurrentWeather, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorPlayer), "System.Void Enviro.EnviroMirrorPlayer::RpcRequestSeason(System.Int32)", InvokeUserCode_RpcRequestSeason__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorPlayer), "System.Void Enviro.EnviroMirrorPlayer::RpcRequestCurrentWeather(System.Int32)", InvokeUserCode_RpcRequestCurrentWeather__Int32);
		}
	}
}
