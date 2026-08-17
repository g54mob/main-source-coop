using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualDesignCafe.Nature
{
	internal class RuntimeGlobalWindInitializer
	{
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			OnActiveSceneChanged(default(Scene), SceneManager.GetActiveScene());
		}

		private static void OnActiveSceneChanged(Scene previousScene, Scene activeScene)
		{
			GameObject gameObject = activeScene.GetRootGameObjects().FirstOrDefault((GameObject g) => g.GetComponentInChildren<GlobalWind>(includeInactive: true) != null);
			if (gameObject == null)
			{
				ApplyDefaultWind();
			}
		}

		private static void ApplyDefaultWind()
		{
			WindSettings.Calm.Apply();
		}
	}
}
