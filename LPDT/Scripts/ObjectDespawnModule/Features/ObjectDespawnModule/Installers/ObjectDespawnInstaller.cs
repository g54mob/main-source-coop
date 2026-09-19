using Features.ObjectDespawnModule.Scripts;
using Zenject;

namespace Features.ObjectDespawnModule.Installers
{
	public class ObjectDespawnInstaller : Installer<ObjectDespawnInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ObjectDespawnModel>().AsSingle();
			base.Container.BindInterfacesTo<ObjectDespawnSystem>().AsSingle();
			base.Container.BindInterfacesTo<PointGrabbableCleanupSystem>().AsSingle();
		}
	}
}
