using System;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.NetworkedModelRuntime
{
	public sealed class NetworkedModelShadowDescriptor
	{
		public Type ModelType { get; }

		public ModelScope Scope { get; }

		public ModelOwnership Ownership { get; }

		public string PrefabResource { get; }

		public Type TransportType { get; }

		public Func<NetworkedModelBase, INetworkedModelShadowBridge> CreateBridge { get; }

		public NetworkedModelShadowDescriptor(Type modelType, ModelScope scope, ModelOwnership ownership, string prefabResource, Type transportType, Func<NetworkedModelBase, INetworkedModelShadowBridge> createBridge)
		{
			ModelType = modelType;
			Scope = scope;
			Ownership = ownership;
			PrefabResource = prefabResource;
			TransportType = transportType;
			CreateBridge = createBridge;
		}
	}
}
