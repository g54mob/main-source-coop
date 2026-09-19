using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Dissonance.Integrations.MirrorIgnorance
{
	[RequireComponent(typeof(NetworkIdentity))]
	public class MirrorIgnorancePlayer : NetworkBehaviour, IDissonancePlayer
	{
		private static readonly Log Log;

		private DissonanceComms _comms;

		[SyncVar]
		private string _playerId;

		private const float SelfHealIntervalSeconds = 1f;

		private float _nextSelfHealCheck;

		public bool IsTracking { get; private set; }

		public string PlayerId => _playerId;

		public Vector3 Position => base.transform.position;

		public Quaternion Rotation => base.transform.rotation;

		public NetworkPlayerType Type
		{
			get
			{
				if (_comms == null || _playerId == null)
				{
					return NetworkPlayerType.Unknown;
				}
				if (!_comms.LocalPlayerName.Equals(_playerId))
				{
					return NetworkPlayerType.Remote;
				}
				return NetworkPlayerType.Local;
			}
		}

		public string Network_playerId
		{
			get
			{
				return _playerId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _playerId, 1uL, null);
			}
		}

		public void OnDestroy()
		{
			if (_comms != null)
			{
				_comms.LocalPlayerNameChanged -= SetPlayerName;
			}
		}

		public void OnEnable()
		{
			_comms = DissonanceComms.GetSingleton();
			if (!IsTracking && !string.IsNullOrEmpty(_playerId))
			{
				StartTracking();
			}
		}

		public void OnDisable()
		{
			StopTracking();
		}

		private void Update()
		{
			if (Time.unscaledTime < _nextSelfHealCheck)
			{
				return;
			}
			_nextSelfHealCheck = Time.unscaledTime + 1f;
			if (!string.IsNullOrEmpty(_playerId))
			{
				if (_comms == null)
				{
					_comms = DissonanceComms.GetSingleton();
				}
				if (_comms != null && !IsTracking)
				{
					StartTracking();
				}
			}
		}

		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();
			DissonanceComms singleton = DissonanceComms.GetSingleton();
			if (singleton == null)
			{
				throw Log.CreateUserErrorException("cannot find DissonanceComms component in scene", "not placing a DissonanceComms component on a game object in the scene", "https://dissonance.readthedocs.io/en/latest/Basics/Quick-Start-MirrorIgnorance/", "2D90A6C3-5F2B-4859-994C-EBBDDD4A10F4");
			}
			if (singleton.LocalPlayerName != null)
			{
				SetPlayerName(singleton.LocalPlayerName);
			}
			singleton.LocalPlayerNameChanged += SetPlayerName;
		}

		private void SetPlayerName(string playerName)
		{
			StopTracking();
			Network_playerId = playerName;
			StartTracking();
			if (base.isLocalPlayer)
			{
				CmdSetPlayerName(playerName);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!string.IsNullOrEmpty(PlayerId))
			{
				StartTracking();
			}
		}

		[Command]
		private void CmdSetPlayerName(string playerName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(playerName);
			SendCommandInternal("System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::CmdSetPlayerName(System.String)", 2094266474, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSetPlayerName(string playerName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(playerName);
			SendRPCInternal("System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::RpcSetPlayerName(System.String)", -1860767819, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void StartTracking()
		{
			if (!IsTracking && _comms != null)
			{
				_comms.TrackPlayerPosition(this);
				IsTracking = true;
			}
		}

		private void StopTracking()
		{
			if (IsTracking)
			{
				if (_comms != null)
				{
					_comms.StopTracking(this);
				}
				IsTracking = false;
			}
		}

		static MirrorIgnorancePlayer()
		{
			Log = Logs.Create(LogCategory.Network, "Mirror Player Component");
			RemoteProcedureCalls.RegisterCommand(typeof(MirrorIgnorancePlayer), "System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::CmdSetPlayerName(System.String)", InvokeUserCode_CmdSetPlayerName__String, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(MirrorIgnorancePlayer), "System.Void Dissonance.Integrations.MirrorIgnorance.MirrorIgnorancePlayer::RpcSetPlayerName(System.String)", InvokeUserCode_RpcSetPlayerName__String);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetPlayerName__String(string playerName)
		{
			Network_playerId = playerName;
			RpcSetPlayerName(playerName);
		}

		protected static void InvokeUserCode_CmdSetPlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetPlayerName called on client.");
			}
			else
			{
				((MirrorIgnorancePlayer)obj).UserCode_CmdSetPlayerName__String(reader.ReadString());
			}
		}

		protected void UserCode_RpcSetPlayerName__String(string playerName)
		{
			if (!base.isLocalPlayer)
			{
				SetPlayerName(playerName);
			}
		}

		protected static void InvokeUserCode_RpcSetPlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetPlayerName called on server.");
			}
			else
			{
				((MirrorIgnorancePlayer)obj).UserCode_RpcSetPlayerName__String(reader.ReadString());
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteString(_playerId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteString(_playerId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _playerId, null, reader.ReadString());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _playerId, null, reader.ReadString());
			}
		}
	}
}
