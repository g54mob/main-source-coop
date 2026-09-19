using Zenject;

namespace Features.AudioVolumeModule.Scripts
{
	public class FmodAudioServiceInstaller : Installer<FmodAudioServiceInstaller>
	{
		public override void InstallBindings()
		{
			BindAudioServices();
		}

		private void BindAudioServices()
		{
			base.Container.BindInterfacesAndSelfTo<FmodFmodAudioVolumeService>().AsSingle();
		}
	}
}
