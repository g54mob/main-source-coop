using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RSG.Muffin.AssetLoaderModule.Core;
using RSG.Muffin.Plugins.Zenject.Addons.AddressablesConfigurationsLoader;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Features.BootstrapModule.Scripts.Core
{
	public class ConfigurationBindingBatch
	{
		private readonly struct ConfigurationBindingEntry
		{
			public Func<AsyncOperationHandle> StartLoad { get; }

			public Action<DiContainer> Bind { get; }

			public ConfigurationBindingEntry(Func<AsyncOperationHandle> startLoad, Action<DiContainer> bind)
			{
				StartLoad = startLoad;
				Bind = bind;
			}
		}

		private readonly DiContainer _container;

		private readonly IAssetLoaderFacadeService _assetLoaderFacadeService;

		private readonly List<ConfigurationBindingEntry> _entries = new List<ConfigurationBindingEntry>();

		public ConfigurationBindingBatch(DiContainer container, IAssetLoaderFacadeService assetLoaderFacadeService)
		{
			_container = container;
			_assetLoaderFacadeService = assetLoaderFacadeService;
		}

		public ConfigurationBindingBatch Add<TConfiguration>(string addressableKey) where TConfiguration : ScriptableObject
		{
			_entries.Add(new ConfigurationBindingEntry(() => Addressables.LoadAssetAsync<TConfiguration>(addressableKey), delegate(DiContainer container)
			{
				container.BindConfigurationFromAddressables<TConfiguration>(addressableKey, _assetLoaderFacadeService).AsSingle();
			}));
			return this;
		}

		public ConfigurationBindingBatch AddAs<TConfiguration, TImplementation>(string addressableKey) where TConfiguration : ScriptableObject where TImplementation : TConfiguration
		{
			_entries.Add(new ConfigurationBindingEntry(() => Addressables.LoadAssetAsync<TImplementation>(addressableKey), delegate(DiContainer container)
			{
				container.Bind<TConfiguration>().To<TImplementation>().FromScriptableObject(_assetLoaderFacadeService.LoadAsset<TImplementation>(addressableKey, AssetLoadSource.Addressables))
					.AsSingle();
			}));
			return this;
		}

		public void LoadAllAndBind()
		{
			List<AsyncOperationHandle> list = StartAllLoads();
			foreach (AsyncOperationHandle item in list)
			{
				item.WaitForCompletion();
			}
			BindAll(list);
		}

		public async UniTask LoadAllAndBindAsync()
		{
			List<AsyncOperationHandle> handles = StartAllLoads();
			await Task.WhenAll(handles.Select((AsyncOperationHandle handle) => handle.Task));
			BindAll(handles);
		}

		private List<AsyncOperationHandle> StartAllLoads()
		{
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>(_entries.Count);
			foreach (ConfigurationBindingEntry entry in _entries)
			{
				list.Add(entry.StartLoad());
			}
			return list;
		}

		private void BindAll(List<AsyncOperationHandle> handles)
		{
			foreach (ConfigurationBindingEntry entry in _entries)
			{
				entry.Bind(_container);
			}
			foreach (AsyncOperationHandle handle in handles)
			{
				Addressables.Release(handle);
			}
			_entries.Clear();
		}
	}
}
