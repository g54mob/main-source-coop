using Zenject;

namespace Features.PlayerMuteModule.Scripts
{
	public class PlayersVoiceVolumeSystemInstaller : Installer<PlayersVoiceVolumeSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<PlayersVoiceVolumeSystem>().AsSingle();
		}
	}
}
