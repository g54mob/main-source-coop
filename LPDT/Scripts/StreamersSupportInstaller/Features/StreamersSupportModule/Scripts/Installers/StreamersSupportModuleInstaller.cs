using Features.StreamersSupportModule.Scripts.Systems;
using Zenject;

namespace Features.StreamersSupportModule.Scripts.Installers
{
	public class StreamersSupportModuleInstaller : Installer<StreamersSupportModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<StreamersStandFillSystem>().AsSingle();
		}
	}
}
