using System;
using Fusion.Sockets;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public interface IDataStreamSynchronizable
	{
		bool IsNeedToSynchronizeOnSpawn { get; }

		event Action<IDataStreamSynchronizable> CallSynchronize;

		event Action<IDataStreamSynchronizable, PlayerIdDataHolder> SynchronizeRequest;

		void Synchronize(bool isWithTrigggerEvent = false);

		internal bool TryGetDataInBytes(out byte[] value);

		internal void SetDataInBytes(byte[] bytesData);

		internal void ReSetDataInBytes();

		internal ReliableKey GetReliableKey();

		void OnDisconnect();
	}
}
