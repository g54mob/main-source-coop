using Zenject;

namespace Features.KnifeThrowingModule.Scripts.Installers
{
	public class KnifeThrowingTableInstaller : Installer<KnifeThrowingTableInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<KnifeThrowingTableService>().AsSingle();
		}
	}
}
