using System;
using System.Collections;
using UnityEngine;

public class PunchScaleText : MonoBehaviour
{
	[Tooltip("Darbe anındaki en büyük ölçek çarpanı")]
	public float punchScale = 1.25f;

	[Tooltip("Büyüyüp eski haline dönme süresi (sn)")]
	public float duration = 0.18f;

	private RectTransform _rect;

	private Vector3 _baseScale = Vector3.one;

	private Coroutine _routine;

	private void Awake()
	{
		_rect = base.transform as RectTransform;
		if (_rect != null)
		{
			_baseScale = _rect.localScale;
		}
	}

	public void Play()
	{
		if (!(_rect == null) && base.gameObject.activeInHierarchy)
		{
			if (_routine != null)
			{
				StopCoroutine(_routine);
			}
			_routine = StartCoroutine(PunchRoutine());
		}
	}

	private IEnumerator PunchRoutine()
	{
		float t = 0f;
		while (t < duration)
		{
			t += Time.deltaTime;
			float num = Mathf.Sin(Mathf.Clamp01(t / duration) * MathF.PI);
			_rect.localScale = _baseScale * (1f + (punchScale - 1f) * num);
			yield return null;
		}
		_rect.localScale = _baseScale;
		_routine = null;
	}
}
