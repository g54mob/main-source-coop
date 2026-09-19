using System;
using Fusion.Sockets;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public interface ICustomDataStreamSynchronizable
	{
		bool IsNeedToSynchronizeOnSpawn { get; }

		event Action<ICustomDataStreamSynchronizable> CallCustomSynchronize;

		event Action<ICustomDataStreamSynchronizable, PlayerIdDataHolder> CustomSynchronizeRequest;

		void CustomSynchronize();

		internal byte[] GetCustomDataInBytes();

		internal void SetCustomDataInBytes(byte[] bytesData);

		internal ReliableKey GetCustomReliableKey();

		void OnDisconnect();
	}
}
