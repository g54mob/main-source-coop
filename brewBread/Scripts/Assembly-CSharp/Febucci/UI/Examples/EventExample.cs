using UnityEngine;

namespace Febucci.UI.Examples
{
	[AddComponentMenu("")]
	public class EventExample : MonoBehaviour
	{
		public TextAnimatorPlayer textAnimatorPlayer;

		public Camera cam;

		private int lastBGIndex;

		public Color[] bgColors;

		private void Awake()
		{
			textAnimatorPlayer.textAnimator.onEvent += OnEvent;
		}

		private void OnEvent(string message)
		{
			if (message != null && message == "bg")
			{
				cam.backgroundColor = bgColors[lastBGIndex];
				lastBGIndex++;
				if (lastBGIndex >= bgColors.Length)
				{
					lastBGIndex = 0;
				}
			}
		}
	}
}
