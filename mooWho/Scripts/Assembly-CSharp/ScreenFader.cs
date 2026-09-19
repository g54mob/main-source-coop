using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
	[Tooltip("Tam ekranı kaplayan siyah Image'ın CanvasGroup'u")]
	public CanvasGroup fadeGroup;

	public static ScreenFader Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		if (fadeGroup != null)
		{
			fadeGroup.alpha = 0f;
			fadeGroup.blocksRaycasts = false;
			fadeGroup.interactable = false;
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public IEnumerator FadeTo(float targetAlpha, float duration)
	{
		if (!(fadeGroup == null))
		{
			bool flag = targetAlpha > 0.01f;
			fadeGroup.blocksRaycasts = flag;
			fadeGroup.interactable = flag;
			float start = fadeGroup.alpha;
			float t = 0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				fadeGroup.alpha = Mathf.Lerp(start, targetAlpha, Mathf.Clamp01(t / duration));
				yield return null;
			}
			fadeGroup.alpha = targetAlpha;
		}
	}
}
