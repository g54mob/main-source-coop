using Zenject;

namespace Features.GuillotineModule.Scripts
{
	public class GuillotineModuleInstaller : Installer<GuillotineModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<GuillotineExecuteDataHolder>().AsSingle();
		}
	}
}
