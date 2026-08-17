using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.Demo
{
	[RequireComponent(typeof(Text))]
	public class CallbackTest : MonoBehaviour
	{
		public float FadeTime = 2f;

		private Text text;

		private Coroutine coroutine;

		private void Start()
		{
			text = GetComponent<Text>();
			SetAlpha(0f);
		}

		private void SetAlpha(float a)
		{
			Color color = text.color;
			color.a = a;
			text.color = color;
		}

		public void OnEvent()
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
			coroutine = StartCoroutine(FadeCoroutine());
		}

		private IEnumerator FadeCoroutine()
		{
			float a = 1f;
			do
			{
				SetAlpha(a);
				a -= Time.deltaTime / FadeTime;
				yield return new WaitForEndOfFrame();
			}
			while (a > 0f);
		}
	}
}
