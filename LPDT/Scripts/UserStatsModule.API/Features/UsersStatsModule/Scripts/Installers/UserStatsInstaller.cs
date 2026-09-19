using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.UsersStatsModule.Scripts.Installers
{
	public class UserStatsInstaller : Installer<UserStatsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindMockableRealizationAsSingle<IUserStatsService>();
		}
	}
}
