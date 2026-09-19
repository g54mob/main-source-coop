using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	public interface IResourceAssetLoaderService : IAssetLoaderService
	{
		new UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : Object;

		new TAsset LoadAsset<TAsset>(string key, string groupName = "Default") where TAsset : Object;

		new void ReleaseAssetsInGroup(string groupName = "Default");

		new void ReleaseAllAssets();

		new bool HasLoadedAsset(string key, string groupName = "Default");
	}
}
