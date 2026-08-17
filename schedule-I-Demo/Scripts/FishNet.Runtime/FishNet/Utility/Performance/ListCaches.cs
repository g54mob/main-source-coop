using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Utility.Performance
{
	[Obsolete("ListCache has been discovered potentially contain a small memory leak depending on the type being cached. Use ObjectCaches, ResettableObjectCaches, CollectionCaches, ResettableCollectionCaches instead.")]
	public static class ListCaches
	{
		private static Stack<ListCache<NetworkObject>> _networkObjectCaches = new Stack<ListCache<NetworkObject>>();

		private static Stack<ListCache<NetworkBehaviour>> _networkBehaviourCaches = new Stack<ListCache<NetworkBehaviour>>();

		private static Stack<ListCache<Transform>> _transformCaches = new Stack<ListCache<Transform>>();

		private static Stack<ListCache<NetworkConnection>> _networkConnectionCaches = new Stack<ListCache<NetworkConnection>>();

		private static Stack<ListCache<int>> _intCaches = new Stack<ListCache<int>>();

		[Obsolete("Use RetrieveNetworkObjectCache().")]
		public static ListCache<NetworkObject> GetNetworkObjectCache()
		{
			return RetrieveNetworkObjectCache();
		}

		public static ListCache<NetworkObject> RetrieveNetworkObjectCache()
		{
			if (_networkObjectCaches.Count == 0)
			{
				return new ListCache<NetworkObject>();
			}
			return _networkObjectCaches.Pop();
		}

		[Obsolete("Use RetrieveNetworkConnectionCache().")]
		public static ListCache<NetworkConnection> GetNetworkConnectionCache()
		{
			return RetrieveNetworkConnectionCache();
		}

		public static ListCache<NetworkConnection> RetrieveNetworkConnectionCache()
		{
			if (_networkConnectionCaches.Count == 0)
			{
				return new ListCache<NetworkConnection>();
			}
			return _networkConnectionCaches.Pop();
		}

		[Obsolete("Use RetrieveTransformCache().")]
		public static ListCache<Transform> GetTransformCache()
		{
			return RetrieveTransformCache();
		}

		public static ListCache<Transform> RetrieveTransformCache()
		{
			if (_transformCaches.Count == 0)
			{
				return new ListCache<Transform>();
			}
			return _transformCaches.Pop();
		}

		[Obsolete("Use RetrieveNetworkBehaviourCache().")]
		public static ListCache<NetworkBehaviour> GetNetworkBehaviourCache()
		{
			return RetrieveNetworkBehaviourCache();
		}

		public static ListCache<NetworkBehaviour> RetrieveNetworkBehaviourCache()
		{
			if (_networkBehaviourCaches.Count == 0)
			{
				return new ListCache<NetworkBehaviour>();
			}
			return _networkBehaviourCaches.Pop();
		}

		[Obsolete("Use RetrieveGetIntCache().")]
		public static ListCache<int> GetIntCache()
		{
			return RetrieveIntCache();
		}

		public static ListCache<int> RetrieveIntCache()
		{
			if (_intCaches.Count == 0)
			{
				return new ListCache<int>();
			}
			return _intCaches.Pop();
		}

		public static void StoreCache(ListCache<NetworkObject> cache)
		{
			cache.Reset();
			_networkObjectCaches.Push(cache);
		}

		public static void StoreCache(ListCache<NetworkConnection> cache)
		{
			cache.Reset();
			_networkConnectionCaches.Push(cache);
		}

		public static void StoreCache(ListCache<Transform> cache)
		{
			cache.Reset();
			_transformCaches.Push(cache);
		}

		public static void StoreCache(ListCache<NetworkBehaviour> cache)
		{
			cache.Reset();
			_networkBehaviourCaches.Push(cache);
		}

		public static void StoreCache(ListCache<int> cache)
		{
			cache.Reset();
			_intCaches.Push(cache);
		}
	}
}
