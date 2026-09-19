using Zenject;

namespace Features.DamageableTrackModule.Scripts.Installers
{
	public class DamageableTrackModuleInstaller : Installer<DamageableTrackModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerScreenShakeByDamageSystem>().AsSingle();
			base.Container.Bind<FallDamageGateModel>().AsSingle();
		}
	}
}
