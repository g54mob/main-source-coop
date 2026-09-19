using Features.SettingsMenuModule.Scripts;
using Features.SettingsMenuModule.Scripts.SettingViews;
using Zenject;

namespace Features.SettingsMenuModule.Installers
{
	public class SettingsMenuInstallers : Installer<SettingsMenuInstallers>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerAudioViewFactory>().AsSingle();
			base.Container.BindInterfacesTo<PlayersVolumeSystem>().AsSingle();
			base.Container.BindInterfacesTo<PlayersVolumeChangeTracingSystem>().AsSingle();
			base.Container.BindInterfacesTo<CallSettingsViewByInputSystem>().AsSingle();
		}
	}
}
