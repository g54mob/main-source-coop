using System;
using System.Runtime.CompilerServices;
using FishNet.Managing;
using FishNet.Managing.Timing;
using FishNet.Serializing;
using FishNet.Transporting;

namespace FishNet.Object.Synchronizing.Internal
{
	public class SyncBase : ISyncType
	{
		public Settings Settings = new Settings();

		public NetworkManager NetworkManager;

		public NetworkBehaviour NetworkBehaviour;

		public uint NextSyncTick;

		private uint _timeToTicks;

		private Channel _currentChannel;

		public bool IsRegistered { get; private set; }

		public bool IsNetworkInitialized
		{
			get
			{
				if (IsRegistered)
				{
					if (!NetworkBehaviour.IsServer)
					{
						return NetworkBehaviour.IsClient;
					}
					return true;
				}
				return false;
			}
		}

		public bool IsSyncObject { get; private set; }

		public float SendRate => Settings.SendRate;

		public bool IsDirty { get; private set; }

		public bool OnStartServerCalled { get; private set; }

		public bool OnStartClientCalled { get; private set; }

		public uint SyncIndex { get; protected set; }

		internal Channel Channel => _currentChannel;

		public void InitializeInstance(NetworkBehaviour nb, uint syncIndex, WritePermission writePermissions, ReadPermission readPermissions, float tickRate, Channel channel, bool isSyncObject)
		{
			NetworkBehaviour = nb;
			SyncIndex = syncIndex;
			_currentChannel = channel;
			IsSyncObject = isSyncObject;
			Settings = new Settings
			{
				WritePermission = writePermissions,
				ReadPermission = readPermissions,
				SendRate = tickRate,
				Channel = channel
			};
			NetworkBehaviour.RegisterSyncType(this, SyncIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetRegistered()
		{
			Registered();
		}

		protected virtual void Registered()
		{
			IsRegistered = true;
		}

		public void PreInitialize(NetworkManager networkManager)
		{
			NetworkManager = networkManager;
			if (Settings.SendRate < 0f)
			{
				Settings.SendRate = networkManager.ServerManager.GetSynctypeRate();
			}
			_timeToTicks = NetworkManager.TimeManager.TimeToTicks(Settings.SendRate, TickRounding.RoundUp);
		}

		public virtual void OnStartCallback(bool asServer)
		{
			if (asServer)
			{
				OnStartServerCalled = true;
			}
			else
			{
				OnStartClientCalled = true;
			}
		}

		public virtual void OnStopCallback(bool asServer)
		{
			if (asServer)
			{
				OnStartServerCalled = false;
			}
			else
			{
				OnStartClientCalled = false;
			}
		}

		protected bool CanNetworkSetValues(bool warn = true)
		{
			if (!IsRegistered)
			{
				return true;
			}
			if (!IsNetworkInitialized)
			{
				return true;
			}
			if (NetworkBehaviour.IsServer)
			{
				return true;
			}
			if (NetworkManager != null && NetworkManager.PredictionManager.GetAllowPredictedSpawning() && NetworkBehaviour.NetworkObject.AllowPredictedSpawning)
			{
				return true;
			}
			bool num = Settings.WritePermission == WritePermission.ClientUnsynchronized || (Settings.ReadPermission == ReadPermission.ExcludeOwner && NetworkBehaviour.IsOwner);
			if (!num && warn)
			{
				LogServerNotActiveWarning();
			}
			return num;
		}

		protected void LogServerNotActiveWarning()
		{
			if (NetworkManager != null)
			{
				NetworkManager.LogWarning("Cannot complete operation as server when server is not active. You can disable this warning by setting WritePermissions to " + WritePermission.ClientUnsynchronized.ToString() + ".");
			}
		}

		public bool Dirty()
		{
			_currentChannel = Settings.Channel;
			bool flag = NetworkBehaviour.DirtySyncType(IsSyncObject);
			IsDirty |= flag;
			return flag;
		}

		internal void ResetDirty()
		{
			if (!IsSyncObject && Settings.Channel == Channel.Unreliable)
			{
				if (_currentChannel == Channel.Unreliable)
				{
					_currentChannel = Channel.Reliable;
				}
				else
				{
					IsDirty = false;
				}
			}
			else
			{
				IsDirty = false;
			}
		}

		internal bool SyncTimeMet(uint tick)
		{
			if (IsDirty)
			{
				return tick >= NextSyncTick;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			WriteHeader(writer, resetSyncTick);
		}

		protected virtual void WriteHeader(PooledWriter writer, bool resetSyncTick = true)
		{
			if (resetSyncTick)
			{
				NextSyncTick = NetworkManager.TimeManager.LocalTick + _timeToTicks;
			}
			writer.WriteByte((byte)SyncIndex);
		}

		public virtual void WriteFull(PooledWriter writer)
		{
		}

		[Obsolete("Use Read(PooledReader, bool).")]
		public virtual void Read(PooledReader reader)
		{
		}

		public virtual void Read(PooledReader reader, bool asServer)
		{
		}

		[Obsolete("Use ResetState().")]
		public virtual void Reset()
		{
		}

		public virtual void ResetState()
		{
			NextSyncTick = 0u;
			ResetDirty();
		}
	}
}
