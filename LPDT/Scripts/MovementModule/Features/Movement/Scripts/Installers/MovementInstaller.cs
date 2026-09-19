using Zenject;

namespace Features.Movement.Scripts.Installers
{
	public class MovementInstaller : Installer<MovementInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerStaminaService>().AsSingle();
		}
	}
}
