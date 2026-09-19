using Zenject;

namespace Features.AudioModule.Scripts
{
	public class MusicSystemInstaller : Installer<MusicSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<MusicSystem>().AsSingle();
		}
	}
}
