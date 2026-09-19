using Zenject;

namespace Features.ProgressSavingModule.Scripts.Implementation
{
	public class SavingServiceInstaller : Installer<SavingServiceInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SavingService>().AsSingle();
			base.Container.BindInterfacesTo<LoadDataAtGameStartSystem>().AsSingle();
		}
	}
}
