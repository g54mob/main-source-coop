using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class PlayersAudioViewBase : ViewBehaviour
	{
		public abstract Transform SettingsItemContainer { get; }

		public abstract SelectableSettingsTab SelectableSettingsTab { get; }
	}
}
