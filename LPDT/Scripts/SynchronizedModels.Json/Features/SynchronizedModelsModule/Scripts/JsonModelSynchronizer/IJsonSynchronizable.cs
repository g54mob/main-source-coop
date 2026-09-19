using System;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public interface IJsonSynchronizable
	{
		RPCType RPCType { get; }

		bool IsNeedToSynchronizeOnSpawn { get; }

		event Action<IJsonSynchronizable> CallSynchronize;

		void Synchronize(bool isWithTrigggerEvent = false);

		internal void SynchronizeOnStart();

		internal bool TryGetValue(out byte[] value);

		internal void SynchronizeInternal(byte[] value);

		internal void ReSynchronizeInternal();

		void OnSpawned();

		void OnDespawned();

		void OnDisconnect();
	}
}
