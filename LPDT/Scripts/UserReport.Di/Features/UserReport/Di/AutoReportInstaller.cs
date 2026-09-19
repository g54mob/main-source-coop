using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.UserReport.Di
{
	public class AutoReportInstaller : Installer<AutoReportInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindAndInstall<IAutoReportInstaller>();
		}
	}
}
