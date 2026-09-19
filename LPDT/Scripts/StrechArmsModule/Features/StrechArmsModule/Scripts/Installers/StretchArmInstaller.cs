using Zenject;

namespace Features.StrechArmsModule.Scripts.Installers
{
	public class StretchArmInstaller : Installer<StretchArmInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerArmService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<ArmVisualsControllerModel>().AsSingle();
		}
	}
}
