using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	public class AddressableAssetLoaderService : IAddressablesAssetLoaderService, IAssetLoaderService
	{
		private const float TIME_OUT_THRESHOLD = 10f;

		protected readonly Dictionary<string, AddressablesGroupHandleContainer> _handlesContainerByGroupName = new Dictionary<string, AddressablesGroupHandleContainer>();

		public async UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			AddressablesGroupHandleContainer handleContainer = GetHandleContainer(groupName);
			AsyncOperationHandle value;
			return (!handleContainer.CompletedHandles.TryGetValue(key, out value)) ? (await ProcessHandleAsync<TAsset>(key, handleContainer)) : (value.Result as TAsset);
		}

		public TAsset LoadAsset<TAsset>(string key, string groupName = "Default") where TAsset : UnityEngine.Object
		{
			AddressablesGroupHandleContainer handleContainer = GetHandleContainer(groupName);
			if (!handleContainer.CompletedHandles.TryGetValue(key, out var value))
			{
				return ProcessHandle<TAsset>(key, handleContainer);
			}
			return value.Result as TAsset;
		}

		public void ReleaseAssetsInGroup(string groupName = "Default")
		{
			if (!_handlesContainerByGroupName.TryGetValue(groupName, out var value))
			{
				return;
			}
			foreach (KeyValuePair<string, List<AsyncOperationHandle>> allHandle in value.AllHandles)
			{
				foreach (AsyncOperationHandle item in allHandle.Value)
				{
					Addressables.Release(item);
				}
			}
			value.AllHandles.Clear();
			value.CompletedHandles.Clear();
		}

		public void ReleaseAllAssets()
		{
			foreach (KeyValuePair<string, AddressablesGroupHandleContainer> item in _handlesContainerByGroupName.ToList())
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

		private async UniTask<TAsset> ProcessHandleAsync<TAsset>(string key, AddressablesGroupHandleContainer handleContainer) where TAsset : UnityEngine.Object
		{
			AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(key);
			handle.Completed += delegate(AsyncOperationHandle<TAsset> completedHandle)
			{
				handleContainer.CompletedHandles[key] = completedHandle;
			};
			AddHandle(key, handle, handleContainer);
			return await handle.Task;
		}

		private TAsset ProcessHandle<TAsset>(string key, AddressablesGroupHandleContainer handleContainer) where TAsset : UnityEngine.Object
		{
			AsyncOperationHandle<TAsset> asyncOperationHandle = Addressables.LoadAssetAsync<TAsset>(key);
			handleContainer.CompletedHandles[key] = asyncOperationHandle;
			AddHandle(key, asyncOperationHandle, handleContainer);
			return asyncOperationHandle.WaitForCompletion();
		}

		private void AddHandle<TAsset>(string key, AsyncOperationHandle<TAsset> handle, AddressablesGroupHandleContainer handleContainer) where TAsset : UnityEngine.Object
		{
			if (!handleContainer.AllHandles.TryGetValue(key, out var value))
			{
				value = new List<AsyncOperationHandle>();
				handleContainer.AllHandles[key] = value;
			}
			value.Add(handle);
		}

		private bool ProcessTimeOut<TAsset>(AsyncOperationHandle<TAsset> handle) where TAsset : UnityEngine.Object
		{
			DateTime dateTime = DateTime.Now.AddSeconds(10.0);
			while (!handle.IsDone)
			{
				if (DateTime.Now > dateTime)
				{
					return true;
				}
			}
			return false;
		}

		private AddressablesGroupHandleContainer GetHandleContainer(string groupName)
		{
			if (_handlesContainerByGroupName.TryGetValue(groupName, out var value))
			{
				return value;
			}
			value = new AddressablesGroupHandleContainer();
			_handlesContainerByGroupName[groupName] = value;
			return value;
		}
	}
}
