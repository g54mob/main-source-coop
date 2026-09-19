using Zenject;

namespace Features.AudioServiceModule.Scripts.Installers
{
	public class AudioServiceInstaller : Installer<AudioServiceInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<AudioModel>().AsSingle();
			base.Container.BindInterfacesTo<AudioService>().AsSingle();
			base.Container.BindInterfacesTo<AudioOneShotLifetimeSystem>().AsSingle();
		}
	}
}
