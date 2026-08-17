using EvilCore.DI;
using EvilCore.DI.Core;
using EvilCore.DI.Installers;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace EvilCore
{
	public class BootstrapLifetimeScope : LifetimeScope
	{
		[SerializeField]
		private BootstrapManagersInstaller bootstrapManagersInstaller;

		[FormerlySerializedAs("scriptableInstaller")]
		[SerializeField]
		private BootstrapScriptableInstaller bootstrapScriptableInstaller;

		[SerializeField]
		private MonoInstaller[] featureInstallers;

		protected override void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
			base.Awake();
		}

		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<GameEntryPoint>(Lifetime.Singleton).As<IGameEntryPoint>();
			builder.RegisterComponent(base.gameObject.AddComponent<ContainerReferenceInitializer>());
			builder.RegisterEntryPoint<GameEntryPointStarter>();
			bootstrapManagersInstaller.Install(builder);
			bootstrapScriptableInstaller.Install(builder);
			if (featureInstallers != null)
			{
				MonoInstaller[] array = featureInstallers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]?.Install(builder);
				}
			}
		}
	}
}
