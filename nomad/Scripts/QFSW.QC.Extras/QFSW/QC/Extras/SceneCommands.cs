using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QFSW.QC.Suggestors.Tags;
using QFSW.QC.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Extras
{
	public static class SceneCommands
	{
		private static async Task PollUntilAsync(int pollInterval, Func<bool> predicate)
		{
			while (!predicate())
			{
				await Task.Delay(pollInterval);
			}
		}

		[Command("load-scene", "loads a scene by name into the game", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task LoadScene([SceneName] string sceneName, [CommandParameterDescription("'Single' mode replaces the current scene with the new scene, whereas 'Additive' merges them")] LoadSceneMode loadMode = LoadSceneMode.Single)
		{
			AsyncOperation asyncOperation = SceneUtilities.LoadSceneAsync(sceneName, loadMode);
			await PollUntilAsync(16, () => asyncOperation.isDone);
		}

		[Command("load-scene-index", "loads a scene by index into the game", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task LoadScene(int sceneIndex, [CommandParameterDescription("'Single' mode replaces the current scene with the new scene, whereas 'Additive' merges them")] LoadSceneMode loadMode = LoadSceneMode.Single)
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadMode);
			await PollUntilAsync(16, () => asyncOperation.isDone);
		}

		[Command("unload-scene", "unloads a scene by name", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task UnloadScene([SceneName(LoadedOnly = true)] string sceneName)
		{
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
			await PollUntilAsync(16, () => asyncOperation.isDone);
		}

		[Command("unload-scene-index", "unloads a scene by index", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task UnloadScene(int sceneIndex)
		{
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneIndex);
			await PollUntilAsync(16, () => asyncOperation.isDone);
		}

		[Command("all-scenes", "gets the name and index of every scene included in the build", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerable<KeyValuePair<int, string>> GetAllScenes()
		{
			int sceneIndex = 0;
			foreach (string allSceneName in SceneUtilities.GetAllSceneNames())
			{
				yield return new KeyValuePair<int, string>(sceneIndex++, allSceneName);
			}
		}

		[Command("loaded-scenes", "gets the name and index of every scene currently loaded", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerable<KeyValuePair<int, string>> GetLoadedScenes()
		{
			return from x in SceneUtilities.GetLoadedScenes()
				orderby x.buildIndex
				select new KeyValuePair<int, string>(x.buildIndex, x.name);
		}

		[Command("active-scene", "gets the name of the active primary scene", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GetCurrentScene()
		{
			return SceneManager.GetActiveScene().name;
		}

		[Command("set-active-scene", "sets the active scene to the scene with name 'sceneName'", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SetActiveScene([SceneName(LoadedOnly = true)] string sceneName)
		{
			Scene sceneByName = SceneManager.GetSceneByName(sceneName);
			if (!sceneByName.isLoaded)
			{
				throw new ArgumentException("Scene " + sceneName + " must be loaded before it can be set active");
			}
			SceneManager.SetActiveScene(sceneByName);
		}
	}
}
