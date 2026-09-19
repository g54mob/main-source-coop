using Zenject;

namespace Features.EnemyFearingModule.Scripts.Installers
{
	public class EnemyFearingInstaller : Installer<EnemyFearingInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<EnemyFearService>().AsSingle();
		}
	}
}
