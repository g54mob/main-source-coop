using Zenject;

namespace Features.EnumHelpersModule.Scripts
{
	public class EnumHelpersInstaller : Installer<EnumHelpersInstaller>
	{
		public override void InstallBindings()
		{
			BindEnumParser();
			BindEnumValuesProvider();
		}

		private void BindEnumValuesProvider()
		{
			base.Container.Bind<IEnumValuesProvider>().To<EnumValuesProvider>().AsSingle();
		}

		private void BindEnumParser()
		{
			base.Container.Bind<IEnumParser>().To<EnumParser>().AsSingle();
		}
	}
}
