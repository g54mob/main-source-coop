using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zorro.Core;

public class TransitionHandler : RetrievableResourceSingleton<TransitionHandler>
{
	public AudioMixerGroup fadeMixer;

	public Image m_fadeImage;

	public AnimationCurve fadeToBlackCurve;

	private bool m_isFaded;

	private Coroutine m_Transition;

	protected override void OnCreated()
	{
		base.OnCreated();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void AbortTransition()
	{
		if (m_Transition != null)
		{
			StopCoroutine(m_Transition);
		}
		FadeOut(null);
	}

	public void TransitionToBlack(float addedDelay, Action onComplete, float startDelay)
	{
		m_isFaded = true;
		m_Transition = StartCoroutine(TransitionToBlackCoroutine(onComplete));
		IEnumerator TransitionToBlackCoroutine(Action action)
		{
			yield return new WaitForSecondsRealtime(startDelay);
			yield return fadeToBlackCurve.YieldForCurve(delegate(float f)
			{
				Color color = m_fadeImage.color;
				color.a = f;
				fadeMixer.audioMixer.SetFloat("FadeVolume", Mathf.Lerp(-80f, 0f, 1f - f));
				m_fadeImage.color = color;
			});
			yield return new WaitForSecondsRealtime(0.1f + addedDelay);
			action?.Invoke();
		}
	}

	public void FadeOut(Action onComplete, float delay = 0f)
	{
		if (m_isFaded)
		{
			m_Transition = StartCoroutine(FadeOutCoroutine());
		}
		IEnumerator FadeOutCoroutine()
		{
			yield return new WaitForSecondsRealtime(delay);
			yield return fadeToBlackCurve.YieldForCurve(delegate(float f)
			{
				Color color = m_fadeImage.color;
				color.a = 1f - f;
				fadeMixer.audioMixer.SetFloat("FadeVolume", Mathf.Lerp(-80f, 0f, f));
				m_fadeImage.color = color;
			});
			m_isFaded = false;
			onComplete?.Invoke();
		}
	}
}
