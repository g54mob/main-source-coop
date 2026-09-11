using UnityEngine;
using UnityEngine.SceneManagement;

namespace pworld.Scripts
{
	public class PReloadSceneShortCut : MonoBehaviour
	{
		[SerializeField]
		private KeyCode tappedKey;

		[SerializeField]
		private KeyCode heldKey;

		private void Update()
		{
			if (Input.GetKeyDown(tappedKey) && Input.GetKey(heldKey))
			{
				ReloadScene();
			}
		}

		private void ReloadScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
