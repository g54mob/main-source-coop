using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	public class ResourceAssetLoaderService : IResourceAssetLoaderService, IAssetLoaderService
	{
		private const float TIME_OUT_THRESHOLD = 10f;

		protected readonly Dictionary<string, ResourcesGroupHandleContainer> _handlesContainerByGroupName = new Dictionary<string, ResourcesGroupHandleContainer>();

		public async UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			ResourcesGroupHandleContainer handleContainer = GetHandleContainer(groupName);
			ResourceRequest value;
			return (!handleContainer.CompletedHandles.TryGetValue(key, out value)) ? (await ProcessHandleAsync<TAsset>(key, handleContainer)) : (value.asset as TAsset);
		}

		public TAsset LoadAsset<TAsset>(string key, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			ResourcesGroupHandleContainer handleContainer = GetHandleContainer(groupName);
			if (!handleContainer.CompletedHandles.TryGetValue(key, out var value))
			{
				return ProcessHandle<TAsset>(key, handleContainer);
			}
			return value.asset as TAsset;
		}

		public void ReleaseAssetsInGroup(string groupName = "Default")
		{
			if (_handlesContainerByGroupName.TryGetValue(groupName, out var _))
			{
				_handlesContainerByGroupName[groupName] = new ResourcesGroupHandleContainer();
			}
		}

		public void ReleaseAllAssets()
		{
			foreach (KeyValuePair<string, ResourcesGroupHandleContainer> item in _handlesContainerByGroupName.ToList())
			{
				ReleaseAssetsInGroup(item.Key);
			}
		}

		public bool HasLoadedAsset(string key, string groupName = "Default")
		{
			if (_handlesContainerByGroupName.TryGetValue(groupName, out var value))
			{
				return value.CompletedHandles.ContainsKey(key);
			}
			return false;
		}

		private async UniTask<TAsset> ProcessHandleAsync<TAsset>(string key, ResourcesGroupHandleContainer handleContainer) where TAsset : UnityEngine.Object
		{
			ResourceRequest handle = Resources.LoadAsync<TAsset>(key);
			handle.completed += delegate
			{
				handleContainer.CompletedHandles[key] = handle;
			};
			AddHandle(key, handle, handleContainer);
			return (await handle) as TAsset;
		}

		private void AddHandle(string key, ResourceRequest handle, ResourcesGroupHandleContainer handleContainer)
		{
			if (!handleContainer.AllHandles.TryGetValue(key, out var value))
			{
				value = new List<ResourceRequest>();
				handleContainer.AllHandles[key] = value;
			}
			value.Add(handle);
		}

		private bool ProcessTimeOut(ResourceRequest handle)
		{
			DateTime dateTime = DateTime.Now.AddSeconds(10.0);
			while (handle.asset == null)
			{
				if (DateTime.Now > dateTime)
				{
					return true;
				}
			}
			return false;
		}

		private TAsset ProcessHandle<TAsset>(string key, ResourcesGroupHandleContainer handleContainer) where TAsset : UnityEngine.Object
		{
			ResourceRequest resourceRequest = Resources.LoadAsync<TAsset>(key);
			if (ProcessTimeOut(resourceRequest))
			{
				throw new TimeoutException("Can't load asset from resources by key " + key + ".");
			}
			handleContainer.CompletedHandles[key] = resourceRequest;
			AddHandle(key, resourceRequest, handleContainer);
			return resourceRequest.asset as TAsset;
		}

		private ResourcesGroupHandleContainer GetHandleContainer(string groupName)
		{
			if (_handlesContainerByGroupName.TryGetValue(groupName, out var value))
			{
				return value;
			}
			value = new ResourcesGroupHandleContainer();
			_handlesContainerByGroupName[groupName] = value;
			return value;
		}
	}
}
