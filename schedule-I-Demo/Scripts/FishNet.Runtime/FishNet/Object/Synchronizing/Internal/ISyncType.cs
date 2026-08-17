using System;
using FishNet.Managing;
using FishNet.Serializing;

namespace FishNet.Object.Synchronizing.Internal
{
	public interface ISyncType
	{
		bool IsDirty { get; }

		void SetRegistered();

		void PreInitialize(NetworkManager networkManager);

		void WriteDelta(PooledWriter writer, bool resetSyncTick = true);

		void WriteFull(PooledWriter writer);

		void Read(PooledReader reader);

		[Obsolete("Use ResetState().")]
		void Reset();

		void ResetState();
	}
}
