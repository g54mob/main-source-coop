using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public float fadeTime;

	private bool _fading;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_fading)
		{
			StartCoroutine(FadeOutObject());
		}
	}

	private IEnumerator FadeOutObject()
	{
		_fading = true;
		float t = 0f;
		do
		{
			Color white = Color.white;
			white.a = Mathf.Lerp(1f, 0f, t);
			spriteRenderer.color = white;
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (!(t >= 1f));
		Object.Destroy(spriteRenderer.gameObject);
		Object.Destroy(base.gameObject);
	}
}
