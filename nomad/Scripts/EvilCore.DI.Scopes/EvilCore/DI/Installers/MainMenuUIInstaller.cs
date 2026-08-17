using System;
using EvilCore.DI.Core;
using EvilCore.UI.MainMenu;
using EvilCore.UI.MainMenu.Panels;
using EvilCore.UI.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.DI.Installers
{
	[Serializable]
	public class MainMenuUIInstaller : MonoInstaller
	{
		[SerializeField]
		private MainMenuUIManager mainMenuUIManagerReference;

		[SerializeField]
		private MainMenuPanel mainMenuPanelReference;

		[SerializeField]
		private JoinGamePanel joinGamePanelReference;

		[SerializeField]
		private CreateGamePanel createGamePanelReference;

		[SerializeField]
		private SinglePlayerPanel singlePlayerPanelReference;

		[SerializeField]
		private LoadGamePanel loadGamePanelReference;

		[SerializeField]
		private LoadingGamePanel loadingGamePanelReference;

		[SerializeField]
		private MainMenuLiveBackgroundManager liveBackgroundManagerReference;

		[SerializeField]
		private SettingsPanel settingsPanelReference;

		[SerializeField]
		private NetworkErrorPopup networkErrorPopupReference;

		[SerializeField]
		private FirstLaunchConsentPopup firstLaunchConsentPopupReference;

		[SerializeField]
		private SaveCompatibilityNoticePopup saveNoticePopupReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, mainMenuUIManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IMainMenuUIManager>();
			});
			RegisterIfNotNull(builder, mainMenuPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<MainMenuPanel>();
			});
			RegisterIfNotNull(builder, joinGamePanelReference, delegate(RegistrationBuilder c)
			{
				c.As<JoinGamePanel>();
			});
			RegisterIfNotNull(builder, createGamePanelReference, delegate(RegistrationBuilder c)
			{
				c.As<CreateGamePanel>();
			});
			RegisterIfNotNull(builder, singlePlayerPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<SinglePlayerPanel>();
			});
			RegisterIfNotNull(builder, loadGamePanelReference, delegate(RegistrationBuilder c)
			{
				c.As<LoadGamePanel>();
			});
			RegisterIfNotNull(builder, loadingGamePanelReference);
			RegisterIfNotNull(builder, liveBackgroundManagerReference);
			RegisterIfNotNull(builder, settingsPanelReference);
			RegisterIfNotNull(builder, networkErrorPopupReference);
			RegisterIfNotNull(builder, firstLaunchConsentPopupReference);
			RegisterIfNotNull(builder, saveNoticePopupReference);
			_ = saveNoticePopupReference == null;
			_ = firstLaunchConsentPopupReference == null;
			_ = networkErrorPopupReference == null;
		}
	}
}
