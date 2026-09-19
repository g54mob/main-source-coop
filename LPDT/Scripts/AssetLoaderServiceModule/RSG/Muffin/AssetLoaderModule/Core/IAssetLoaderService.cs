using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	public interface IAssetLoaderService
	{
		UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : Object;

		TAsset LoadAsset<TAsset>(string key, string groupName = "Default") where TAsset : Object;

		void ReleaseAssetsInGroup(string groupName = "Default");

		void ReleaseAllAssets();

		bool HasLoadedAsset(string key, string groupName = "Default");
	}
}
