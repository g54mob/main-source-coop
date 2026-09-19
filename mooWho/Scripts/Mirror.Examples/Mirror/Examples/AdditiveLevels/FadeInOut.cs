using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.AdditiveLevels
{
	public class FadeInOut : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField]
		private Image panelImage;

		[Header("Settings")]
		[SerializeField]
		[Range(1f, 10f)]
		[Tooltip("Time in seconds to fade in")]
		private byte fadeInTime = 2;

		[SerializeField]
		[Range(1f, 10f)]
		[Tooltip("Time in seconds to fade out")]
		private byte fadeOutTime = 2;

		private bool isFading;

		private void OnValidate()
		{
			if (panelImage == null)
			{
				panelImage = GetComponentInChildren<Image>();
			}
			fadeInTime = (byte)Mathf.Max(fadeInTime, 1);
			fadeOutTime = (byte)Mathf.Max(fadeOutTime, 1);
		}

		public float GetFadeInTime()
		{
			return (float)(int)fadeInTime + Time.fixedDeltaTime;
		}

		public IEnumerator FadeIn()
		{
			yield return FadeImage(0f, 1f, (int)fadeInTime);
		}

		public float GetFadeOutTime()
		{
			return (float)(int)fadeOutTime + Time.fixedDeltaTime;
		}

		public IEnumerator FadeOut()
		{
			yield return FadeImage(1f, 0f, (int)fadeOutTime);
		}

		private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
		{
			if (panelImage == null || isFading)
			{
				yield break;
			}
			Color color = panelImage.color;
			if (!Mathf.Approximately(color.a, endAlpha))
			{
				isFading = true;
				float elapsedTime = 0f;
				float fixedDeltaTime = Time.fixedDeltaTime;
				while (elapsedTime < duration)
				{
					elapsedTime += fixedDeltaTime;
					float a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
					panelImage.color = new Color(color.r, color.g, color.b, a);
					yield return new WaitForFixedUpdate();
				}
				panelImage.color = new Color(color.r, color.g, color.b, endAlpha);
				isFading = false;
			}
		}
	}
}
