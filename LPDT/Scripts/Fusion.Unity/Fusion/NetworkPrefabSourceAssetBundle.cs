using System;

namespace Fusion
{
	[Serializable]
	public class NetworkPrefabSourceAssetBundle : NetworkAssetSourceAssetBundle<NetworkObject>, INetworkPrefabSource, INetworkAssetSource<NetworkObject>
	{
		public NetworkObjectGuid AssetGuid;

		NetworkObjectGuid INetworkPrefabSource.AssetGuid => AssetGuid;
	}
}
