using Zenject;

namespace Features.StruggleBarModule.Scripts.Installers
{
	public class StruggleBarInstaller : Installer<StruggleBarInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<StruggleBarService>().AsSingle();
			base.Container.BindInterfacesTo<StruggleBarSystem>().AsSingle();
		}
	}
}
