using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayerAudioViewFactory : IPlayerAudioViewFactory
	{
		private readonly PlayerAudioViewConfiguration _playerAudioViewConfiguration;

		private readonly DiContainer _container;

		public PlayerAudioViewFactory(PlayerAudioViewConfiguration playerAudioViewConfiguration, DiContainer container)
		{
			_playerAudioViewConfiguration = playerAudioViewConfiguration;
			_container = container;
		}

		public PlayersSoundSettingsItemPresenter CreatePlayerAudioButtonView(Transform parent, FocusableWindowBehaviour windowBehaviour)
		{
			PlayersSoundSettingsItemViewBase view = CreatePlayerAudioButtonViewBase(parent);
			return windowBehaviour.GetPresenterForView<PlayersSoundSettingsItemPresenter>(view);
		}

		private PlayersSoundSettingsItemViewBase CreatePlayerAudioButtonViewBase(Transform parent)
		{
			return _container.InstantiatePrefabForComponent<PlayersSoundSettingsItemViewBase>(_playerAudioViewConfiguration.PlayersSoundSettingsItem, parent);
		}
	}
}
