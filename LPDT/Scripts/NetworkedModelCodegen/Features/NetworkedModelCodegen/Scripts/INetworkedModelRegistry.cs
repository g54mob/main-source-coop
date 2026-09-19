using System.Collections.Generic;

namespace Features.NetworkedModelCodegen.Scripts
{
	public interface INetworkedModelRegistry
	{
		IReadOnlyList<NetworkedModelBase> Models { get; }

		void Register(NetworkedModelBase model);

		void Unregister(NetworkedModelBase model);
	}
}
