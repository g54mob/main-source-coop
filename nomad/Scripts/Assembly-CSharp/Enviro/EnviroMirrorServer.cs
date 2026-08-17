using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Enviro
{
	[AddComponentMenu("Enviro 3/Integrations/Mirror Server")]
	[RequireComponent(typeof(NetworkIdentity))]
	public class EnviroMirrorServer : NetworkBehaviour
	{
		public float updateSmoothing = 15f;

		[SyncVar]
		private float networkHours;

		[SyncVar]
		private int networkDays;

		[SyncVar]
		private int networkMonths;

		[SyncVar]
		private int networkYears;

		public float NetworknetworkHours
		{
			get
			{
				return networkHours;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref networkHours, 1uL, null);
			}
		}

		public int NetworknetworkDays
		{
			get
			{
				return networkDays;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref networkDays, 2uL, null);
			}
		}

		public int NetworknetworkMonths
		{
			get
			{
				return networkMonths;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref networkMonths, 4uL, null);
			}
		}

		public int NetworknetworkYears
		{
			get
			{
				return networkYears;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref networkYears, 8uL, null);
			}
		}

		public override void OnStartServer()
		{
			EnviroManager.instance.OnSeasonChanged += SendSeasonToClient;
			EnviroManager.instance.OnZoneWeatherChanged += SendWeatherToClient;
		}

		public override void OnStopServer()
		{
			if (!(EnviroManager.instance == null))
			{
				EnviroManager.instance.OnSeasonChanged -= SendSeasonToClient;
				EnviroManager.instance.OnZoneWeatherChanged -= SendWeatherToClient;
			}
		}

		public void Start()
		{
			if (!base.isServer)
			{
				if (EnviroManager.instance.Time != null)
				{
					EnviroManager.instance.Time.Settings.simulate = false;
				}
				if (EnviroManager.instance.Weather != null)
				{
					EnviroManager.instance.Weather.globalAutoWeatherChange = false;
				}
				if (EnviroManager.instance.Environment != null)
				{
					EnviroManager.instance.Environment.Settings.changeSeason = false;
				}
			}
		}

		private void SendWeatherToClient(EnviroWeatherType w, EnviroZone z)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			int weather = 0;
			int zone = -1;
			for (int i = 0; i < EnviroManager.instance.Weather.Settings.weatherTypes.Count; i++)
			{
				if (EnviroManager.instance.Weather.Settings.weatherTypes[i] == w)
				{
					weather = i;
				}
			}
			for (int j = 0; j < EnviroManager.instance.zones.Count; j++)
			{
				if (EnviroManager.instance.zones[j] == z)
				{
					zone = j;
				}
			}
			RpcWeatherUpdate(weather, zone);
		}

		private void SendSeasonToClient(EnviroEnvironment.Seasons s)
		{
			if (NetworkServer.active)
			{
				RpcSeasonUpdate((int)s);
			}
		}

		[ClientRpc]
		private void RpcSeasonUpdate(int season)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(season);
			SendRPCInternal("System.Void Enviro.EnviroMirrorServer::RpcSeasonUpdate(System.Int32)", 1678325993, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcWeatherUpdate(int weather, int zone)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(weather);
			writer.WriteVarInt(zone);
			SendRPCInternal("System.Void Enviro.EnviroMirrorServer::RpcWeatherUpdate(System.Int32,System.Int32)", 1545197721, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void Update()
		{
			if (EnviroManager.instance == null || EnviroManager.instance.Time == null)
			{
				return;
			}
			if (!base.isServer)
			{
				if (networkHours < 1f && EnviroManager.instance.Time.GetTimeOfDay() > 23f)
				{
					EnviroManager.instance.Time.SetTimeOfDay(networkHours);
				}
				EnviroManager.instance.Time.SetTimeOfDay(Mathf.Lerp(EnviroManager.instance.Time.GetTimeOfDay(), networkHours, Time.deltaTime * updateSmoothing));
				EnviroManager.instance.Time.years = networkYears;
				EnviroManager.instance.Time.months = networkMonths;
				EnviroManager.instance.Time.days = networkDays;
			}
			else
			{
				NetworknetworkHours = EnviroManager.instance.Time.GetTimeOfDay();
				NetworknetworkDays = EnviroManager.instance.Time.days;
				NetworknetworkMonths = EnviroManager.instance.Time.months;
				NetworknetworkYears = EnviroManager.instance.Time.years;
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcSeasonUpdate__Int32(int season)
		{
			if (EnviroManager.instance.Environment != null)
			{
				EnviroManager.instance.Environment.ChangeSeason((EnviroEnvironment.Seasons)season);
			}
		}

		protected static void InvokeUserCode_RpcSeasonUpdate__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSeasonUpdate called on server.");
			}
			else
			{
				((EnviroMirrorServer)obj).UserCode_RpcSeasonUpdate__Int32(reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcWeatherUpdate__Int32__Int32(int weather, int zone)
		{
			if (EnviroManager.instance.Weather != null)
			{
				if (zone == -1)
				{
					EnviroManager.instance.Weather.ChangeWeather(weather);
				}
				else
				{
					EnviroManager.instance.Weather.ChangeZoneWeather(weather, zone);
				}
			}
		}

		protected static void InvokeUserCode_RpcWeatherUpdate__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcWeatherUpdate called on server.");
			}
			else
			{
				((EnviroMirrorServer)obj).UserCode_RpcWeatherUpdate__Int32__Int32(reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		static EnviroMirrorServer()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorServer), "System.Void Enviro.EnviroMirrorServer::RpcSeasonUpdate(System.Int32)", InvokeUserCode_RpcSeasonUpdate__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorServer), "System.Void Enviro.EnviroMirrorServer::RpcWeatherUpdate(System.Int32,System.Int32)", InvokeUserCode_RpcWeatherUpdate__Int32__Int32);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(networkHours);
				writer.WriteVarInt(networkDays);
				writer.WriteVarInt(networkMonths);
				writer.WriteVarInt(networkYears);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteFloat(networkHours);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteVarInt(networkDays);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteVarInt(networkMonths);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteVarInt(networkYears);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref networkHours, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref networkDays, null, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref networkMonths, null, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref networkYears, null, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref networkHours, null, reader.ReadFloat());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref networkDays, null, reader.ReadVarInt());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref networkMonths, null, reader.ReadVarInt());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref networkYears, null, reader.ReadVarInt());
			}
		}
	}
}
