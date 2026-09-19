using Features.ViewSystemModule.Scripts.Windows;
using Zenject;

namespace Features.ViewSystemModule.Scripts.Installers
{
	public class LobbyUIInstaller : Installer<LobbyUIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<LobbyWindow>().AsSingle();
		}
	}
}
