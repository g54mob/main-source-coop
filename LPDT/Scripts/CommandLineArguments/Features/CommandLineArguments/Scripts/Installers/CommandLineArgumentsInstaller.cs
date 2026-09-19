using Zenject;

namespace Features.CommandLineArguments.Scripts.Installers
{
	public class CommandLineArgumentsInstaller : Installer<CommandLineArgumentsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ICommandLineArgumentsService>().To<CommandLineArgumentsService>().AsSingle();
		}
	}
}
