using UnityEngine;

namespace EvilCore
{
	[CreateAssetMenu(menuName = "EvilCore/Scene Flow Config", fileName = "SceneFlowConfig")]
	public class SceneFlowConfig : ScriptableObject
	{
		[SerializeField]
		private string gameSceneName = "Game";

		[SerializeField]
		private string mainMenuSceneName = "MainMenu";

		[SerializeField]
		private string gameMenuSceneName = "GameMenu";

		public string GameSceneName => gameSceneName;

		public string MainMenuSceneName => mainMenuSceneName;

		public string GameMenuSceneName => gameMenuSceneName;
	}
}
