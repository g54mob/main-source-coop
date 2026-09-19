using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public class HeadCrabVignetteInstaller : Installer<HeadCrabVignetteInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<HeadCrabVignetteService>().AsSingle();
		}
	}
}
