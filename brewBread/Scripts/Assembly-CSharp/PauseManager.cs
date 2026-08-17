using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	public GameObject eventSystem;

	private void Start()
	{
		StaticInstance<TransitionSystem>.Instance.OnSceneUnloaded += OnSceneUnloaded;
	}

	private void OnDestroy()
	{
		StaticInstance<TransitionSystem>.Instance.OnSceneUnloaded -= OnSceneUnloaded;
	}

	private void OnSceneUnloaded(string sceneName)
	{
		if (StaticInstance<TransitionSystem>.Instance.ScenesCount == 3)
		{
			eventSystem.SetActive(value: true);
		}
	}

	public void ResumeGame()
	{
		StaticInstance<Pause>.Instance.ResumeGame();
	}

	public void Options()
	{
		eventSystem.SetActive(value: false);
		StaticInstance<TransitionSystem>.Instance.LoadSceneAsync(Scenes.Options, LoadSceneMode.Additive);
	}

	public void AssistMode()
	{
		eventSystem.SetActive(value: false);
		StaticInstance<TransitionSystem>.Instance.LoadSceneAsync(Scenes.AssistModeMenu, LoadSceneMode.Additive);
	}

	public void SaveToMenu()
	{
		SaveSystem.SaveData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		ResumeGame();
		StaticInstance<DataBetweenScenes>.Instance.RestartPlayersInputs();
		StaticInstance<TransitionSystem>.Instance.LoadScene(Scenes.MainMenu);
	}

	public void SaveandExit()
	{
		Time.timeScale = 1f;
		SaveSystem.SaveData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		Application.Quit();
	}
}
