using Zenject;

namespace Features.RuporModule.Scripts
{
	public class RuporInteractionInstaller : Installer<RuporInteractionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<RuporInteractableSystem>().AsSingle();
		}
	}
}
