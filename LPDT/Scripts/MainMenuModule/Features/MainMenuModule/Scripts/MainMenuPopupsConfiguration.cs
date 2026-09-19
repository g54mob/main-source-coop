using UnityEngine;

namespace Features.MainMenuModule.Scripts
{
	[CreateAssetMenu(fileName = "MainMenuPopupsConfiguration_Default", menuName = "Configurations/MainMenu/MainMenuPopupsConfiguration")]
	public class MainMenuPopupsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public bool ReleaseAnnouncePopupEnabled { get; private set; } = true;
	}
}
