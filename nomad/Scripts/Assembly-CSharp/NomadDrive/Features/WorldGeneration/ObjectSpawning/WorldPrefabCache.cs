using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public static class WorldPrefabCache
	{
		private static readonly Dictionary<string, AsyncOperationHandle<GameObject>> _handles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

		private static readonly List<AsyncOperationHandle<IList<GameObject>>> _labelHandles = new List<AsyncOperationHandle<IList<GameObject>>>();

		public static void Preload(IEnumerable<string> guids)
		{
			if (guids == null)
			{
				return;
			}
			foreach (string guid in guids)
			{
				if (!string.IsNullOrEmpty(guid) && !_handles.ContainsKey(guid))
				{
					try
					{
						_handles[guid] = Addressables.LoadAssetAsync<GameObject>(guid);
					}
					catch (Exception ex)
					{
						EvilLogger.LogError("[WorldPrefabCache] Preload failed for GUID " + guid + ": " + ex.Message, "Preload", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\WorldPrefabCache.cs", 49);
					}
				}
			}
		}

		public static void PreloadByLabel(string label)
		{
			if (string.IsNullOrEmpty(label))
			{
				return;
			}
			try
			{
				_labelHandles.Add(Addressables.LoadAssetsAsync<GameObject>(label));
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[WorldPrefabCache] PreloadByLabel failed for label '" + label + "': " + ex.Message, "PreloadByLabel", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\WorldPrefabCache.cs", 70);
			}
		}

		public static async UniTask<GameObject> GetOrLoadAsync(string guid)
		{
			if (string.IsNullOrEmpty(guid))
			{
				return null;
			}
			if (!_handles.TryGetValue(guid, out var value))
			{
				value = Addressables.LoadAssetAsync<GameObject>(guid);
				_handles[guid] = value;
			}
			try
			{
				GameObject obj = await value.Task;
				if (obj == null)
				{
					EvilLogger.LogError("[WorldPrefabCache] Loaded a null prefab for GUID: " + guid, "GetOrLoadAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\WorldPrefabCache.cs", 93);
				}
				return obj;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[WorldPrefabCache] Load failed for GUID " + guid + ": " + ex.Message, "GetOrLoadAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\ObjectSpawning\\WorldPrefabCache.cs", 98);
				return null;
			}
		}

		public static void ReleaseAll()
		{
			foreach (AsyncOperationHandle<GameObject> value in _handles.Values)
			{
				if (value.IsValid())
				{
					Addressables.Release(value);
				}
			}
			_handles.Clear();
			foreach (AsyncOperationHandle<IList<GameObject>> labelHandle in _labelHandles)
			{
				if (labelHandle.IsValid())
				{
					Addressables.Release(labelHandle);
				}
			}
			_labelHandles.Clear();
		}
	}
}
