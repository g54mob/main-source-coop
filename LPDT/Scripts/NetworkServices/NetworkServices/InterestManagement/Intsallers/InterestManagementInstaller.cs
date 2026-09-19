using Features.NetworkServices.Scripts.InterestManagement;
using Zenject;

namespace NetworkServices.InterestManagement.Intsallers
{
	public class InterestManagementInstaller : Installer<InterestManagementInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IObjectInterestService>().To<ObjectInterestService>().AsSingle();
		}
	}
}
