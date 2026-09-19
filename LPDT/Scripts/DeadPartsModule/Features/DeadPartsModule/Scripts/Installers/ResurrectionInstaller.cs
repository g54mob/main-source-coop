using Zenject;

namespace Features.DeadPartsModule.Scripts.Installers
{
	public class ResurrectionInstaller : Installer<ResurrectionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerResurrectionService>().AsSingle();
		}
	}
}
