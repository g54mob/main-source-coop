using Zenject;

namespace Features.VoiceControlModule.Scripts
{
	public class VoiceControllerInstaller : Installer<VoiceControllerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IVoiceService>().To<VoiceService>().AsSingle();
		}
	}
}
