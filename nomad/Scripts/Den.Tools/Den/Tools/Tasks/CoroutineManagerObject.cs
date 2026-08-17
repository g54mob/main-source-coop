using UnityEngine;

namespace Den.Tools.Tasks
{
	[ExecuteInEditMode]
	public class CoroutineManagerObject : MonoBehaviour
	{
		public int timePerFrame = 30;

		public void OnEnable()
		{
		}

		public void Update()
		{
			CoroutineManager.timePerFrame = timePerFrame;
			CoroutineManager.Update();
		}
	}
}
