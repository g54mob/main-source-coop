using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Features.NetworkedModelCodegen.Scripts;
using Zenject;

namespace Features.NetworkedModelRuntime
{
	public class NetworkedModelRuntimeInstaller : Installer<NetworkedModelRuntimeInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<INetworkedModelRegistry>().To<NetworkedModelRegistry>().AsSingle();
			base.Container.Bind<INetworkedModelInstanceProvider>().To<NetworkedModelInstanceProvider>().AsSingle();
			base.Container.BindInterfacesTo<NetworkedModelSessionSystem>().AsSingle();
			foreach (Type item in FindShadowInstallers())
			{
				((INetworkedModelShadowInstaller)Activator.CreateInstance(item)).Install(base.Container);
			}
		}

		private static List<Type> FindShadowInstallers()
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] array;
				try
				{
					array = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					array = ex.Types.Where((Type type2) => type2 != null).ToArray();
				}
				Type[] array2 = array;
				foreach (Type type in array2)
				{
					if (!(type == null) && !type.IsAbstract && !type.IsInterface && typeof(INetworkedModelShadowInstaller).IsAssignableFrom(type))
					{
						list.Add(type);
					}
				}
			}
			return list;
		}
	}
}
