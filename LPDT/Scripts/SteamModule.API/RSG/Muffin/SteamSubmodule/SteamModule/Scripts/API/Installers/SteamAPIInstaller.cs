using Zenject;

namespace RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API.Installers
{
	public class SteamAPIInstaller : Installer<SteamAPIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SteamModel>().AsSingle();
		}
	}
}
