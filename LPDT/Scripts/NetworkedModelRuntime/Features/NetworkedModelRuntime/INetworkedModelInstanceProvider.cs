using System;
using System.Collections.Generic;
using Fusion;

namespace Features.NetworkedModelRuntime
{
	public interface INetworkedModelInstanceProvider
	{
		event Action<NetworkBehaviour> Added;

		event Action<NetworkBehaviour> Removed;

		void Register(NetworkBehaviour instance);

		void Unregister(NetworkBehaviour instance);

		IReadOnlyCollection<NetworkBehaviour> GetAll(Type transportType);

		IEnumerable<T> GetAll<T>() where T : NetworkBehaviour;
	}
}
