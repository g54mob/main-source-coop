using Features.ViewSystemModule.Scripts.Windows;
using Zenject;

namespace Features.ViewSystemModule.Scripts.Installers
{
	public class MenuUIInstaller : Installer<MenuUIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<MenuWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<CreditsWindow>().AsSingle();
		}
	}
}
