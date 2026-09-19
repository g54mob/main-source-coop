using RSG.Muffin.ApplicationFilesModule.Core.Scripts;
using Zenject;

namespace Features.ApplicationFilesRealizationModule.Scripts.Installers
{
	public class ApplicationFilesRealizationInstaller : Installer<ApplicationFilesRealizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ApplicationFilesHolder>().AsSingle();
		}
	}
}
