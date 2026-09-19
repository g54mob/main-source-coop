using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	[PublicAPI]
	public interface IAssetLoaderFacadeService
	{
		UniTask<TAsset> LoadAssetAsync<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object;

		TAsset LoadAsset<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object;

		void ReleaseAssetsInGroup(AssetLoadSource assetLoadSource, string groupName = "Default");

		void ReleaseAllAssets(AssetLoadSource assetLoadSource);

		bool HasLoadedAsset(string key, AssetLoadSource assetLoadSource, string groupName = "Default");

		IAssetLoaderService GetAssetLoaderService(AssetLoadSource assetLoadSource);
	}
}
