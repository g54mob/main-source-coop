using Features.HeadwearModule.Scripts;
using Zenject;

namespace Features.DeadPartsModule.Scripts.Installers
{
	public class DeadPartsInstaller : Installer<DeadPartsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerDeadPartSpawnService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DeadPartConnectSystem>().AsSingle();
			base.Container.BindInterfacesTo<DeadPartsSpawnSystem>().AsSingle();
			base.Container.BindInterfacesTo<HeadwearSelfRemovalSystem>().AsSingle();
			base.Container.BindInterfacesTo<HeadwearPlayerDisconnectSystem>().AsSingle();
			base.Container.BindInterfacesTo<HeadwearDeathDropSystem>().AsSingle();
			base.Container.BindInterfacesTo<HeadwearOrphanHealSystem>().AsSingle();
			base.Container.BindInterfacesTo<CauldronStealthSystem>().AsSingle();
			base.Container.BindInterfacesTo<HeadwearDamageDropSystem>().AsSingle();
		}
	}
}
