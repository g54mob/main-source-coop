using Zenject;

namespace Features.GoogleFormModule.Scripts
{
	public class GoogleFormInstaller : Installer<GoogleFormInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<GoogleFormOpenOnApplicationQuiteSystem>().AsSingle();
		}
	}
}
