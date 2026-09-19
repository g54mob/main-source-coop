using System;
using System.Threading.Tasks;
using RSG.Muffin.AssetLoaderModule.Core;
using UnityEngine;
using Zenject;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class MonoWindowsFactory : IWindowsFactory
	{
		private const string ROOT_KEY = "WindowInstances/";

		private const string DEFAULT_GROUP_NAME = "Default";

		private readonly IAssetLoaderFacadeService _assetLoaderFacadeService;

		private readonly PreloadedWindowsModel _preloadedWindowsModel;

		private DiContainer _container;

		public MonoWindowsFactory(IAssetLoaderFacadeService assetLoaderFacadeService, DiContainer container, PreloadedWindowsModel preloadedWindowsModel)
		{
			_container = container;
			_assetLoaderFacadeService = assetLoaderFacadeService ?? throw new ArgumentNullException("assetLoaderFacadeService");
			_preloadedWindowsModel = preloadedWindowsModel ?? throw new ArgumentNullException("preloadedWindowsModel");
		}

		public IWindowInstance GetWindowInstanceForWindowType(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName = "Default")
		{
			MonoWindowInstance monoWindowInstance;
			try
			{
				monoWindowInstance = DownloadWindowInstance(windowType, assetLoadSource, assetGroupName);
			}
			catch (Exception innerException)
			{
				throw new WindowPrefabCouldNotBeLoadedException(windowType, "WindowInstances/" + windowType, assetLoadSource, innerException);
			}
			if (monoWindowInstance == null)
			{
				throw new WindowPrefabCouldNotBeLoadedException(windowType, "WindowInstances/" + windowType, assetLoadSource);
			}
			return _container.InstantiatePrefabForComponent<IWindowInstance>(monoWindowInstance);
		}

		public async Task<IWindowInstance> GetWindowInstanceForWindowTypeAsync(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName = "Default")
		{
			MonoWindowInstance monoWindowInstance;
			try
			{
				monoWindowInstance = await DownloadWindowInstanceAsync(windowType, assetLoadSource, assetGroupName);
			}
			catch (Exception innerException)
			{
				throw new WindowPrefabCouldNotBeLoadedException(windowType, "WindowInstances/" + windowType, assetLoadSource, innerException);
			}
			if (monoWindowInstance == null)
			{
				throw new WindowPrefabCouldNotBeLoadedException(windowType, "WindowInstances/" + windowType, assetLoadSource);
			}
			return _container.InstantiatePrefabForComponent<IWindowInstance>(monoWindowInstance);
		}

		public IWindowInstance GetPreloadWindowInstanceForWindowType(Type windowType)
		{
			if (!_preloadedWindowsModel.PreloadedWindows.TryGetValue(windowType.Name, out var value))
			{
				throw new WindowPrefabCouldNotBeLoadedException(windowType, "WindowInstances/" + windowType);
			}
			return _container.InstantiatePrefabForComponent<IWindowInstance>(value);
		}

		private MonoWindowInstance DownloadWindowInstance(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName)
		{
			return assetLoadSource switch
			{
				AssetLoadSource.Addressables => DownloadInstanceFromAddressables(windowType, assetGroupName), 
				AssetLoadSource.Resources => DownloadInstanceFromResources(windowType, assetGroupName), 
				_ => throw new ArgumentOutOfRangeException("assetLoadSource", assetLoadSource, null), 
			};
		}

		private async Task<MonoWindowInstance> DownloadWindowInstanceAsync(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName)
		{
			return assetLoadSource switch
			{
				AssetLoadSource.Addressables => await DownloadInstanceFromAddressablesAsync(windowType, assetGroupName), 
				AssetLoadSource.Resources => await DownloadInstanceFromResourcesAsync(windowType, assetGroupName), 
				_ => throw new ArgumentOutOfRangeException("assetLoadSource", assetLoadSource, null), 
			};
		}

		private MonoWindowInstance DownloadInstanceFromAddressables(Type windowType, string assetGroupName)
		{
			return _assetLoaderFacadeService.GetAssetLoaderService(AssetLoadSource.Addressables).LoadAsset<GameObject>(windowType.Name, assetGroupName).GetComponent<MonoWindowInstance>();
		}

		private MonoWindowInstance DownloadInstanceFromResources(Type windowType, string assetGroupName)
		{
			return _assetLoaderFacadeService.GetAssetLoaderService(AssetLoadSource.Resources).LoadAsset<MonoWindowInstance>("WindowInstances/" + windowType.Name, assetGroupName);
		}

		private async Task<MonoWindowInstance> DownloadInstanceFromAddressablesAsync(Type windowType, string assetGroupName)
		{
			return (await _assetLoaderFacadeService.GetAssetLoaderService(AssetLoadSource.Addressables).LoadAssetAsync<GameObject>(windowType.Name, assetGroupName)).GetComponent<MonoWindowInstance>();
		}

		private async Task<MonoWindowInstance> DownloadInstanceFromResourcesAsync(Type windowType, string assetGroupName)
		{
			return await _assetLoaderFacadeService.GetAssetLoaderService(AssetLoadSource.Resources).LoadAssetAsync<MonoWindowInstance>("WindowInstances/" + windowType.Name, assetGroupName);
		}
	}
}
