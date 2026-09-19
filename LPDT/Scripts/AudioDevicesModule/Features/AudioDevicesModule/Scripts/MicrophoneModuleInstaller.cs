using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public class MicrophoneModuleInstaller : Installer<MicrophoneModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<MicrophoneService>().AsSingle();
		}
	}
}
