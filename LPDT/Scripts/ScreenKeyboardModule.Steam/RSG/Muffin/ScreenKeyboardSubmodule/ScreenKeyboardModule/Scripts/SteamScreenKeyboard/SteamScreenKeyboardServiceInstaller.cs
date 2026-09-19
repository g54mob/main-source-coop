using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.ScreenKeyboardSubmodule.ScreenKeyboardModule.Scripts.SteamScreenKeyboard
{
	public class SteamScreenKeyboardServiceInstaller : Installer<SteamScreenKeyboardServiceInstaller>, IScreenKeyboardServiceInstaller, IMockableInstaller
	{
		public void CallInstall(DiContainer container)
		{
			Installer<SteamScreenKeyboardServiceInstaller>.Install(container);
		}

		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SteamScreenKeyboard>().AsSingle();
		}
	}
}
