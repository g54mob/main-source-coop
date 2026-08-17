using System.Collections;
using UnityEngine;

public class FadeOutSprites : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] _spriteRenderers;

	[SerializeField]
	private float _fadeTime;

	private bool _fading;

	private void Start()
	{
	}

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
		Color newColor = Color.white;
		SpriteRenderer[] spriteRenderers;
		do
		{
			yield return new WaitForEndOfFrame();
			spriteRenderers = _spriteRenderers;
			foreach (SpriteRenderer spriteRenderer in spriteRenderers)
			{
				newColor.a = Mathf.Lerp(1f, 0f, t);
				spriteRenderer.color = newColor;
				t += Time.deltaTime;
				if (t >= 1f)
				{
					Object.Destroy(spriteRenderer.gameObject);
					break;
				}
			}
		}
		while (!(t >= 1f));
		spriteRenderers = _spriteRenderers;
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			Object.Destroy(spriteRenderers[i].gameObject);
		}
	}
}
