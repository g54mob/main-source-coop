using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RSG.Muffin.MockSubmodule.MockModule;

namespace RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule
{
	public static class AssemblyTypeCache
	{
		private static readonly object _lock = new object();

		private static readonly HashSet<string> _assemblyPrefixBlacklist = new HashSet<string>
		{
			"System", "Unity", "UnityEngine", "UnityEditor", "mscorlib", "Mono.", "Microsoft.", "netstandard", "nunit", "JetBrains",
			"Zenject", "UniRx", "Newtonsoft", "DOTween"
		};

		private static ConcurrentDictionary<Type, Type> _primaryCache;

		private static ConcurrentDictionary<Type, Type> _mockCache;

		private static ConcurrentDictionary<Type, List<Type>> _primaryMultiCache;

		private static ConcurrentDictionary<Type, List<Type>> _mockMultiCache;

		public static void WarmUp()
		{
			lock (_lock)
			{
				if (_primaryCache != null)
				{
					return;
				}
				_primaryCache = new ConcurrentDictionary<Type, Type>();
				_mockCache = new ConcurrentDictionary<Type, Type>();
				_primaryMultiCache = new ConcurrentDictionary<Type, List<Type>>();
				_mockMultiCache = new ConcurrentDictionary<Type, List<Type>>();
				Parallel.ForEach(from t in (from a in AppDomain.CurrentDomain.GetAssemblies()
						where !a.IsDynamic && !_assemblyPrefixBlacklist.Any((string prefix) => a.GetName().Name.StartsWith(prefix, StringComparison.Ordinal))
						select a).AsParallel().SelectMany(delegate(Assembly a)
					{
						try
						{
							return a.GetTypes();
						}
						catch
						{
							return Array.Empty<Type>();
						}
					})
					where (object)t != null && !t.IsAbstract && !t.IsInterface
					select t, delegate(Type type)
				{
					bool flag = type.IsDefined(typeof(MockRealizationAttribute), inherit: false);
					Type[] interfaces = type.GetInterfaces();
					foreach (Type key in interfaces)
					{
						if (flag)
						{
							_mockCache.TryAdd(key, type);
							_mockMultiCache.AddOrUpdate(key, (Type _) => new List<Type> { type }, delegate(Type _, List<Type> list)
							{
								lock (list)
								{
									list.Add(type);
									return list;
								}
							});
						}
						else
						{
							_primaryCache.TryAdd(key, type);
							_primaryMultiCache.AddOrUpdate(key, (Type _) => new List<Type> { type }, delegate(Type _, List<Type> list)
							{
								lock (list)
								{
									list.Add(type);
									return list;
								}
							});
						}
					}
				});
			}
		}

		public static bool TryGetPrimaryAll(Type iface, out List<Type> impls)
		{
			return _primaryMultiCache.TryGetValue(iface, out impls);
		}

		public static bool TryGetMockAll(Type iface, out List<Type> impls)
		{
			return _mockMultiCache.TryGetValue(iface, out impls);
		}

		public static void Dispose()
		{
			_primaryCache = null;
			_mockCache = null;
			_primaryMultiCache = null;
			_mockMultiCache = null;
		}
	}
}
