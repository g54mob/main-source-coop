using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	public class SceneTransitionCanvas : MonoBehaviour
	{
		private void Awake()
		{
			Object.DontDestroyOnLoad(this);
		}
	}
}
