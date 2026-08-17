using System.Collections.Generic;
using FishNet.Serializing;
using GameKit.Utilities;

namespace FishNet.Managing.Server
{
	internal static class ConnectedClientsBroadcastSerializers
	{
		public static void WriteConnectedClientsBroadcast(this PooledWriter writer, ConnectedClientsBroadcast value)
		{
			writer.WriteList(value.Values);
		}

		public static ConnectedClientsBroadcast ReadConnectedClientsBroadcast(this PooledReader reader)
		{
			List<int> collection = CollectionCaches<int>.RetrieveList();
			reader.ReadList(ref collection);
			return new ConnectedClientsBroadcast
			{
				Values = collection
			};
		}
	}
}
