using System;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public interface ICustomJsonSynchronizable
	{
		RPCType RPCType { get; }

		bool IsNeedToSynchronizeOnSpawn { get; }

		event Action<ICustomJsonSynchronizable> OnCallCustomSynchronize;

		void CustomSynchronize();

		internal void CustomSynchronizeInternal(byte[] value);

		internal byte[] GetCustomValue();

		void OnSpawned();

		void OnDespawned();

		void OnDisconnect();
	}
}
