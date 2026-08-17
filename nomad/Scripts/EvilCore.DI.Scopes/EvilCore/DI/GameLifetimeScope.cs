using EvilCore.DI.Core;
using EvilCore.DI.Installers;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI
{
	public class GameLifetimeScope : LifetimeScope
	{
		[FormerlySerializedAs("managersInstaller")]
		[SerializeField]
		private GameManagersInstaller gameManagersInstaller;

		[FormerlySerializedAs("uiInstaller")]
		[SerializeField]
		private GameUIInstaller gameUIInstaller;

		[SerializeField]
		private MonoInstaller[] featureInstallers;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponent(base.gameObject.AddComponent<ContainerReferenceInitializer>());
			if (gameManagersInstaller != null)
			{
				gameManagersInstaller.Install(builder);
			}
			else
			{
				EvilLogger.LogError("[Game] GameManagersInstaller is null! Check Inspector references.", "Configure", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Scopes\\Game\\GameLifetimeScope.cs", 22);
			}
			if (gameUIInstaller != null)
			{
				gameUIInstaller.Install(builder);
			}
			else
			{
				EvilLogger.LogError("[Game] GameUIInstaller is null! Check Inspector references.", "Configure", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Scopes\\Game\\GameLifetimeScope.cs", 25);
			}
			if (featureInstallers != null)
			{
				MonoInstaller[] array = featureInstallers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]?.Install(builder);
				}
			}
		}

		protected override LifetimeScope FindParent()
		{
			LifetimeScope lifetimeScope = LifetimeScope.Find<BootstrapLifetimeScope>();
			if (lifetimeScope != null)
			{
				return lifetimeScope;
			}
			return base.FindParent();
		}
	}
}
