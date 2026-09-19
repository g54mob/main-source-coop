using Zenject;

namespace Features.PhysicsUtilsModule.Scripts.Installers
{
	public class PhysicsUtilsInstaller : Installer<PhysicsUtilsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PhysicsOverlapService>().AsSingle();
		}
	}
}
