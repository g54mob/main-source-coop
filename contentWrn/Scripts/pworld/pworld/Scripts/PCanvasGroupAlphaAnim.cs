using UnityEngine;

namespace pworld.Scripts
{
	public class PCanvasGroupAlphaAnim : MonoBehaviour
	{
		public AnimationCurve curve;

		public bool playOnAwake;

		private CanvasGroup canvasGroup;

		public float Alpha
		{
			get
			{
				return canvasGroup.alpha;
			}
			set
			{
				canvasGroup.alpha = value;
			}
		}

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			if (playOnAwake)
			{
				Play();
			}
		}

		public void SetStartAlpha()
		{
			Alpha = curve.Evaluate(0f);
		}

		public void Play()
		{
		}
	}
}
