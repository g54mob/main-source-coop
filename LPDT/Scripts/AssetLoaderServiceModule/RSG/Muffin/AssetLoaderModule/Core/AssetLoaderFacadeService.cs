using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	public class AssetLoaderFacadeService : IAssetLoaderFacadeService
	{
		private readonly IAssetLoaderService _resourceAssetLoaderService;

		private readonly IAssetLoaderService _addressablesAssetLoaderService;

		public AssetLoaderFacadeService(IResourceAssetLoaderService resourceAssetLoaderService, IAddressablesAssetLoaderService addressablesAssetLoaderFacadeService)
		{
			_resourceAssetLoaderService = resourceAssetLoaderService ?? throw new ArgumentNullException("resourceAssetLoaderService");
			_addressablesAssetLoaderService = addressablesAssetLoaderFacadeService ?? throw new ArgumentNullException("addressablesAssetLoaderFacadeService");
		}

		public async UniTask<TAsset> LoadAssetAsync<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			return await GetAssetLoaderService(assetLoadSource).LoadAssetAsync<TAsset>(key, groupName);
		}

		public TAsset LoadAsset<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			return GetAssetLoaderService(assetLoadSource).LoadAsset<TAsset>(key, groupName);
		}

		public void ReleaseAssetsInGroup(AssetLoadSource assetLoadSource, string groupName = "Default")
		{
			GetAssetLoaderService(assetLoadSource).ReleaseAssetsInGroup(groupName);
		}

		public void ReleaseAllAssets(AssetLoadSource assetLoadSource)
		{
			GetAssetLoaderService(assetLoadSource).ReleaseAllAssets();
		}

		public bool HasLoadedAsset(string key, AssetLoadSource assetLoadSource, string groupName = "Default")
		{
			return GetAssetLoaderService(assetLoadSource).HasLoadedAsset(key, groupName);
		}

		public IAssetLoaderService GetAssetLoaderService(AssetLoadSource assetLoadSource)
		{
			return assetLoadSource switch
			{
				AssetLoadSource.Addressables => _addressablesAssetLoaderService, 
				AssetLoadSource.Resources => _resourceAssetLoaderService, 
				_ => throw new ArgumentOutOfRangeException("assetLoadSource", assetLoadSource, null), 
			};
		}
	}
}
