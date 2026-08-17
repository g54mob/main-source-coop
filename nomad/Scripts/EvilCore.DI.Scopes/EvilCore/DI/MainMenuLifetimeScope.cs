using EvilCore.DI.Installers;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI
{
	public class MainMenuLifetimeScope : LifetimeScope
	{
		[SerializeField]
		private MainMenuUIInstaller mainMenuUIInstaller;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponent(base.gameObject.AddComponent<ContainerReferenceInitializer>());
			if (mainMenuUIInstaller != null)
			{
				mainMenuUIInstaller.Install(builder);
			}
			else
			{
				EvilLogger.LogError("[MainMenu] MainMenuUIInstaller is null! Check Inspector references.", "Configure", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Scopes\\MainMenu\\MainMenuLifetimeScope.cs", 18);
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
