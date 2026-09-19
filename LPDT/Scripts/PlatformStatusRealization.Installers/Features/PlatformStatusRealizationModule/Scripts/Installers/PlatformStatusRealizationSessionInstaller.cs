using Features.PlatformStatusRealizationModule.Scripts.Systems;
using Zenject;

namespace Features.PlatformStatusRealizationModule.Scripts.Installers
{
	public class PlatformStatusRealizationSessionInstaller : Installer<PlatformStatusRealizationSessionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<GameStatusSyncSystem>().AsSingle();
		}
	}
}
