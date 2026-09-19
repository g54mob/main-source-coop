using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[CreateAssetMenu(fileName = "PlayerAudioViewConfiguration_Default", menuName = "Configurations/Audio/PlayerAudioViewConfiguration")]
	public class PlayerAudioViewConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public PlayersSoundSettingsItemViewBase PlayersSoundSettingsItem { get; private set; }
	}
}
