using System;
using System.IO;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	public class NetworkAssetSourceAssetBundle<T> where T : UnityEngine.Object
	{
		public string AssetBundleName;

		public string AssetName;

		public string NestedAssetName;

		[NonSerialized]
		private object _state;

		[NonSerialized]
		private int _acquireCount;

		public bool IsCompleted
		{
			get
			{
				if (_state == null)
				{
					return false;
				}
				if (_state is AssetBundleRequest { isDone: false })
				{
					return false;
				}
				return true;
			}
		}

		public string Description => "AssetBundle: " + AssetBundleName + ", " + AssetName + (string.IsNullOrEmpty(NestedAssetName) ? "" : ("[" + NestedAssetName + "]"));

		public void Acquire(bool synchronous)
		{
			if (_acquireCount == 0)
			{
				LoadInternal(synchronous);
			}
			_acquireCount++;
		}

		public void Release()
		{
			if (_acquireCount <= 0)
			{
				throw new Exception("Asset is not loaded");
			}
			if (--_acquireCount == 0)
			{
				UnloadInternal();
			}
		}

		public T WaitForResult()
		{
			if (_state is AssetBundleRequest assetBundleRequest)
			{
				if (assetBundleRequest.isDone)
				{
					if (string.IsNullOrEmpty(NestedAssetName))
					{
						_state = assetBundleRequest.asset;
					}
					else
					{
						_state = NetworkAssetSourceAssetBundle.FindAsset(assetBundleRequest.allAssets, NestedAssetName);
					}
				}
				else
				{
					_state = null;
					LoadInternal(synchronous: true);
				}
			}
			return ValidateResult(_state);
		}

		private void LoadInternal(bool synchronous)
		{
			try
			{
				AssetBundle assetBundle = GetAssetBundle(AssetBundleName);
				if (assetBundle == null)
				{
					throw new InvalidOperationException("Unable to load asset bundle " + AssetBundleName);
				}
				if (typeof(T).IsSubclassOf(typeof(Component)))
				{
					_state = NetworkAssetSourceAssetBundle.LoadAssetFromBundle<GameObject>(assetBundle, AssetName, NestedAssetName, synchronous);
				}
				else
				{
					_state = NetworkAssetSourceAssetBundle.LoadAssetFromBundle<T>(assetBundle, AssetName, NestedAssetName, synchronous);
				}
			}
			catch (Exception source)
			{
				_state = ExceptionDispatchInfo.Capture(source);
			}
		}

		private void UnloadInternal()
		{
			if (_state is AssetBundleRequest assetBundleRequest)
			{
				assetBundleRequest.completed += delegate(AsyncOperation op)
				{
					UnityEngine.Object asset = ((AssetBundleRequest)op).asset;
					if ((bool)asset)
					{
						NetworkAssetSourceAssetBundle.AssetUnloaded?.Invoke(asset);
					}
				};
			}
			else if (_state is UnityEngine.Object obj)
			{
				NetworkAssetSourceAssetBundle.AssetUnloaded?.Invoke(obj);
			}
			_state = null;
		}

		private static AssetBundle GetAssetBundle(string bundleName)
		{
			foreach (AssetBundle allLoadedAssetBundle in AssetBundle.GetAllLoadedAssetBundles())
			{
				if (string.Equals(allLoadedAssetBundle.name, bundleName, StringComparison.Ordinal))
				{
					return allLoadedAssetBundle;
				}
			}
			if (NetworkAssetSourceAssetBundle.AssetBundleRequested != null)
			{
				return NetworkAssetSourceAssetBundle.AssetBundleRequested(bundleName);
			}
			return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
		}

		private T ValidateResult(object result)
		{
			if (_state == null)
			{
				throw new InvalidOperationException("Missing AssetBundle " + AssetBundleName + " asset: " + AssetName + (string.IsNullOrEmpty(NestedAssetName) ? "" : ("[" + NestedAssetName + "]")));
			}
			if (result is ExceptionDispatchInfo exceptionDispatchInfo)
			{
				exceptionDispatchInfo.Throw();
				throw new NotSupportedException();
			}
			if (result == null)
			{
				throw new InvalidOperationException("Failed to load asset: " + AssetBundleName + "[" + AssetName + "]; asset is null");
			}
			if (typeof(T).IsSubclassOf(typeof(Component)))
			{
				if (!(result is GameObject))
				{
					throw new InvalidOperationException($"Failed to load asset: {AssetBundleName}[{AssetName}]; asset is not a GameObject, but a {result.GetType()}");
				}
				T component = ((GameObject)result).GetComponent<T>();
				if (!component)
				{
					throw new InvalidOperationException($"Failed to load asset: {AssetBundleName}[{AssetName}]; asset does not contain component {typeof(T)}");
				}
				return component;
			}
			if (result is T result2)
			{
				return result2;
			}
			throw new InvalidOperationException($"Failed to load asset: {AssetBundleName}[{AssetName}]; asset is not of type {typeof(T)}, but {result.GetType()}");
		}
	}
	public static class NetworkAssetSourceAssetBundle
	{
		public static Func<string, AssetBundle> AssetBundleRequested;

		public static Action<UnityEngine.Object> AssetUnloaded;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticFields()
		{
			AssetUnloaded = null;
			AssetBundleRequested = null;
		}

		internal static object LoadAssetFromBundle<T>(AssetBundle bundle, string assetName, string nestedAssetName, bool synchronous) where T : UnityEngine.Object
		{
			if (synchronous)
			{
				if (string.IsNullOrEmpty(nestedAssetName))
				{
					return bundle.LoadAsset<T>(assetName);
				}
				T[] array = bundle.LoadAssetWithSubAssets<T>(assetName);
				return (array != null) ? FindAsset(array, nestedAssetName) : null;
			}
			if (string.IsNullOrEmpty(nestedAssetName))
			{
				return bundle.LoadAssetAsync<T>(assetName);
			}
			return bundle.LoadAssetWithSubAssetsAsync<T>(assetName);
		}

		internal static T FindAsset<T>(T[] assets, string assetName) where T : UnityEngine.Object
		{
			foreach (T val in assets)
			{
				if ((bool)val && val.name == assetName)
				{
					return val;
				}
			}
			return null;
		}
	}
}
