using System;

namespace Fusion
{
	[Serializable]
	public class NetworkPrefabSourceAddressable : NetworkAssetSourceAddressable<NetworkObject>, INetworkPrefabSource, INetworkAssetSource<NetworkObject>
	{
		public NetworkObjectGuid AssetGuid;

		public string AssetPath;

		NetworkObjectGuid INetworkPrefabSource.AssetGuid => AssetGuid;

		string INetworkPrefabSource.AssetPath => AssetPath;
	}
}
