using Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API;
using Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.Internal;
using Zenject;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.Di
{
	public class ReportInstaller : Installer<ReportInstaller>
	{
		public override void InstallBindings()
		{
			BindReportService();
		}

		private void BindReportService()
		{
			base.Container.Bind<IReportService>().To<ReportService>().AsSingle();
		}
	}
}
