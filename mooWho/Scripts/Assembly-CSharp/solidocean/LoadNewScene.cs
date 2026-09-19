using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace solidocean
{
	public class LoadNewScene : MonoBehaviour
	{
		public GameObject LoadingPanel;

		public Image LoadingBar;

		public void LoadANewScene(string scene)
		{
			StartCoroutine(LoadSceneAsync(scene));
		}

		private IEnumerator LoadSceneAsync(string scene)
		{
			AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
			LoadingPanel.SetActive(value: true);
			while (operation.isDone)
			{
				float fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
				LoadingBar.fillAmount = fillAmount;
				yield return null;
			}
		}
	}
}
