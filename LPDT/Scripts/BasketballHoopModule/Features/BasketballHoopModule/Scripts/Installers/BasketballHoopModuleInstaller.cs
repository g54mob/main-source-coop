using Zenject;

namespace Features.BasketballHoopModule.Scripts.Installers
{
	public sealed class BasketballHoopModuleInstaller : Installer<BasketballHoopModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<BasketballService>().AsSingle();
		}
	}
}
