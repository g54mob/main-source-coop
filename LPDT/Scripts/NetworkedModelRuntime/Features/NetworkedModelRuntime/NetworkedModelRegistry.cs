using System.Collections.Generic;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.NetworkedModelRuntime
{
	public class NetworkedModelRegistry : INetworkedModelRegistry
	{
		private readonly List<NetworkedModelBase> _models = new List<NetworkedModelBase>();

		public IReadOnlyList<NetworkedModelBase> Models => _models;

		public void Register(NetworkedModelBase model)
		{
			if (!_models.Contains(model))
			{
				_models.Add(model);
			}
		}

		public void Unregister(NetworkedModelBase model)
		{
			_models.Remove(model);
		}
	}
}
