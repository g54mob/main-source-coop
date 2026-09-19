using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.UserReport.Di
{
	public class UserReportInstaller : Installer<UserReportInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindAndInstall<IUserReportServiceInstaller>();
		}
	}
}
