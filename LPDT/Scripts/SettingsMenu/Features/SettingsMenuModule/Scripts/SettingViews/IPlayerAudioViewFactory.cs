using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public interface IPlayerAudioViewFactory
	{
		PlayersSoundSettingsItemPresenter CreatePlayerAudioButtonView(Transform parent, FocusableWindowBehaviour windowBehaviour);
	}
}
