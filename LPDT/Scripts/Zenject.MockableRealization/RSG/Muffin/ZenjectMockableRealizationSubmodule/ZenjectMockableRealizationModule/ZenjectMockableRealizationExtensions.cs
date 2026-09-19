using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule
{
	public static class ZenjectMockableRealizationExtensions
	{
		public static void BindMockableRealizationAsSingle<T>(this DiContainer container)
		{
			AssemblyTypeCache.WarmUp();
			if (AssemblyTypeCache.TryGetPrimaryAll(typeof(T), out var impls))
			{
				if (impls.Count > 1)
				{
					throw new Exception("[DI] Multiple implementations found for " + typeof(T).Name + ": " + string.Join(", ", impls.Select((Type t) => t.Name)));
				}
				container.Bind<T>().To(impls[0]).AsSingle();
				return;
			}
			if (AssemblyTypeCache.TryGetMockAll(typeof(T), out var impls2))
			{
				if (impls2.Count > 1)
				{
					throw new Exception("[DI] Multiple mock implementations found for " + typeof(T).Name + ": " + string.Join(", ", impls2.Select((Type t) => t.Name)));
				}
				container.Bind<T>().To(impls2[0]).AsSingle();
				return;
			}
			throw new Exception("[DI] No implementation found for " + typeof(T).Name);
		}

		public static void BindAndInstall<T>(this DiContainer container) where T : IMockableInstaller
		{
			AssemblyTypeCache.WarmUp();
			if (AssemblyTypeCache.TryGetPrimaryAll(typeof(T), out var impls))
			{
				if (impls.Count > 1)
				{
					throw new Exception("[DI] Multiple implementations found for " + typeof(T).Name + ": " + string.Join(", ", impls.Select((Type t) => t.Name)));
				}
				container.Bind<T>().To(impls[0]).AsSingle();
			}
			else
			{
				if (!AssemblyTypeCache.TryGetMockAll(typeof(T), out var impls2))
				{
					throw new Exception("[DI] No implementation found for " + typeof(T).Name);
				}
				if (impls2.Count > 1)
				{
					throw new Exception("[DI] Multiple mock implementations found for " + typeof(T).Name + ": " + string.Join(", ", impls2.Select((Type t) => t.Name)));
				}
				container.Bind<T>().To(impls2[0]).AsSingle();
			}
			container.Resolve<T>().InstallBindings();
		}

		public static void BindAndInstallAll<T>(this DiContainer container) where T : IMockableInstaller
		{
			AssemblyTypeCache.WarmUp();
			List<Type> impls = null;
			if (!AssemblyTypeCache.TryGetPrimaryAll(typeof(T), out impls) && !AssemblyTypeCache.TryGetMockAll(typeof(T), out impls))
			{
				throw new Exception("[DI] No implementations found for " + typeof(T).Name);
			}
			foreach (Type item in impls)
			{
				container.Bind<T>().To(item).AsSingle();
			}
			container.ResolveAll<T>().ForEach(delegate(T installer)
			{
				installer.InstallBindings();
			});
		}
	}
}
