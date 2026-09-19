using Zenject;

namespace Features.CurrencyModule.Scripts.Installers
{
	public class CurrencyInstaller : Installer<CurrencyInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<CurrencyModel>().AsSingle();
			base.Container.BindInterfacesTo<AddCurrencySystem>().AsSingle();
		}
	}
}
