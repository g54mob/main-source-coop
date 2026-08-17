using EvilCore.DI.Installers;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.GameMenu.Panels;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EvilCore
{
	public class GameMenuLifetimeScope : LifetimeScope
	{
		[SerializeField]
		private GameMenuUIInstaller gameMenuUIInstaller;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<OnlineGameInfoPanel>();
			builder.RegisterComponentInHierarchy<GameMenuRootPanel>();
			if (gameMenuUIInstaller != null)
			{
				gameMenuUIInstaller.Install(builder);
			}
			else
			{
				EvilLogger.LogError("[GameMenu] GameMenuUIInstaller is null! Check Inspector references.", "Configure", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Scopes\\GameMenu\\GameMenuLifetimeScope.cs", 20);
			}
		}

		protected override LifetimeScope FindParent()
		{
			return LifetimeScope.Find<BootstrapLifetimeScope>() ?? base.FindParent();
		}
	}
}
