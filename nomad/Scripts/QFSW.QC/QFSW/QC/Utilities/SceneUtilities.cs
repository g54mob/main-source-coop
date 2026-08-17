using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Utilities
{
	public static class SceneUtilities
	{
		public static IEnumerable<Scene> GetScenesInBuild()
		{
			int sceneCount = SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				yield return SceneManager.GetSceneByBuildIndex(i);
			}
		}

		public static IEnumerable<Scene> GetLoadedScenes()
		{
			int sceneCount = SceneManager.sceneCount;
			for (int i = 0; i < sceneCount; i++)
			{
				yield return SceneManager.GetSceneAt(i);
			}
		}

		public static IEnumerable<Scene> GetAllScenes()
		{
			return GetScenesInBuild();
		}

		public static IEnumerable<string> GetAllScenePaths()
		{
			int sceneCount = SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				yield return SceneUtility.GetScenePathByBuildIndex(i);
			}
		}

		public static IEnumerable<string> GetAllSceneNames()
		{
			return GetAllScenePaths().Select(Path.GetFileNameWithoutExtension);
		}

		public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode)
		{
			return SceneManager.LoadSceneAsync(sceneName, mode);
		}
	}
}
