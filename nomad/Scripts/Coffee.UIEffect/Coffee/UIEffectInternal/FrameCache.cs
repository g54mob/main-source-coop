using System;
using System.Collections.Generic;

namespace Coffee.UIEffectInternal
{
	internal static class FrameCache
	{
		private interface IFrameCache
		{
			void Clear();
		}

		private class FrameCacheContainer<T> : IFrameCache
		{
			private readonly Dictionary<(int, int), T> _caches = new Dictionary<(int, int), T>();

			public void Clear()
			{
				_caches.Clear();
			}

			public bool TryGet((int, int) key, out T result)
			{
				return _caches.TryGetValue(key, out result);
			}

			public void Set((int, int) key, T result)
			{
				_caches[key] = result;
			}
		}

		private static readonly Dictionary<Type, IFrameCache> s_Caches;

		static FrameCache()
		{
			s_Caches = new Dictionary<Type, IFrameCache>();
			s_Caches.Clear();
			UIExtraCallbacks.onLateAfterCanvasRebuild += ClearAllCache;
		}

		public static bool TryGet<T>(object key1, string key2, out T result)
		{
			return GetFrameCache<T>().TryGet((key1.GetHashCode(), key2.GetHashCode()), out result);
		}

		public static bool TryGet<T>(object key1, string key2, int key3, out T result)
		{
			return GetFrameCache<T>().TryGet((key1.GetHashCode(), key2.GetHashCode() + key3), out result);
		}

		public static void Set<T>(object key1, string key2, T result)
		{
			GetFrameCache<T>().Set((key1.GetHashCode(), key2.GetHashCode()), result);
		}

		public static void Set<T>(object key1, string key2, int key3, T result)
		{
			GetFrameCache<T>().Set((key1.GetHashCode(), key2.GetHashCode() + key3), result);
		}

		private static void ClearAllCache()
		{
			foreach (IFrameCache value in s_Caches.Values)
			{
				value.Clear();
			}
		}

		private static FrameCacheContainer<T> GetFrameCache<T>()
		{
			Type typeFromHandle = typeof(T);
			if (s_Caches.TryGetValue(typeFromHandle, out var value))
			{
				return value as FrameCacheContainer<T>;
			}
			value = new FrameCacheContainer<T>();
			s_Caches.Add(typeFromHandle, value);
			return (FrameCacheContainer<T>)value;
		}
	}
}
