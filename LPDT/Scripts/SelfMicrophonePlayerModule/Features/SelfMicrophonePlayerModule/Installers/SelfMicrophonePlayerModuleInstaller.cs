using Zenject;

namespace Features.SelfMicrophonePlayerModule.Installers
{
	public class SelfMicrophonePlayerModuleInstaller : Installer<SelfMicrophonePlayerModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<FmodMicSelfMonitorService>().AsSingle();
		}
	}
}
