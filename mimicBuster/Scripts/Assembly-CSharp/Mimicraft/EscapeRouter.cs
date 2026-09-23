using UnityEngine;

namespace Mimicraft
{
	public class EscapeRouter : MonoBehaviour
	{
		private static EscapeRouter instance;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("EscapeRouter");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<EscapeRouter>();
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		private void LateUpdate()
		{
			GameMenuState.ResolveEscape();
		}
	}
}
