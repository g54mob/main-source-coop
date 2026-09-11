using System.Collections;
using Portningsbolaget.Platforms;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
	public string scene = "NewMainMenu";

	private IEnumerator Start()
	{
		Debug.Log("Running Bootstrap");
		Cursor.visible = true;
		Debug.Log("Waiting for Platform to Initialise");
		IPlatform platform = PlatformManager.Platform;
		while (!platform.Initialized || !PlatformUtility.Initialised)
		{
			yield return null;
		}
		yield return LocalizationSettings.InitializationOperation;
		LocalizationKeys.MakeLocaleStrings();
		yield return null;
		Debug.Log("Loading Scene: " + scene);
		SceneManager.LoadScene(scene);
		Debug.Log("Initialising Game");
		new GameObject("Game").AddComponent<GameHandler>().Initialize();
	}
}
