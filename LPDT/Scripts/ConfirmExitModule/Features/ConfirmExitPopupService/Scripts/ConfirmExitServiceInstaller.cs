using Zenject;

namespace Features.ConfirmExitPopupService.Scripts
{
	public class ConfirmExitServiceInstaller : Installer<ConfirmExitServiceInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<ConfirmExitService>().AsSingle();
		}
	}
}
