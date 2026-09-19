using Features.InputModule.Scripts.Generated;
using Zenject;

namespace Features.NetworkInputModule.Scripts
{
	public class LocalInputModuleInstaller : Installer<LocalInputModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<LocalInputActions>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
		}
	}
}
