using Zenject;

namespace Features.LevelGatesModule.Scripts.Installers
{
	public class LevelGatesInstaller : Installer<LevelGatesInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<GateNavigationService>().AsSingle();
		}
	}
}
