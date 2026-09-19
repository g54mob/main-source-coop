using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[Header("Fade")]
	[Tooltip("Background+LoadingIcon'u birlikte fade eden CanvasGroup (Loading Canvas kökünde)")]
	public CanvasGroup group;

	public float fadeInDuration = 0.25f;

	public float fadeOutDuration = 0.25f;

	[Header("Güvenlik")]
	[Tooltip("Show() sonrası bu süre içinde Hide() hiç çağrılmazsa ekran zorla kapatılır — bir oyuncu bağlantısı kopsa/callback kaybolsa bile ekranda süresiz takılı kalınmaz. 0 = zaman aşımı yok.")]
	public float safetyTimeoutSeconds = 20f;

	[Header("LoadingIcon — Sprite Sheet Animasyonu")]
	public Image loadingIconImage;

	[Tooltip("Sırayla oynatılacak kareler (cow-loading_0..7)")]
	public Sprite[] loadingIconFrames;

	[Tooltip("Saniyede kaç kare")]
	public float frameRate = 8f;

	private Coroutine _fadeRoutine;

	private Coroutine _safetyRoutine;

	private Coroutine _iconRoutine;

	private int _showToken;

	public static LoadingScreen Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		if (group != null)
		{
			group.alpha = 0f;
			group.blocksRaycasts = false;
			group.interactable = false;
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void Show(float? timeoutOverride = null)
	{
		_showToken++;
		int showToken = _showToken;
		if (_safetyRoutine != null)
		{
			StopCoroutine(_safetyRoutine);
		}
		float num = timeoutOverride ?? safetyTimeoutSeconds;
		if (num > 0f)
		{
			_safetyRoutine = StartCoroutine(SafetyTimeoutRoutine(showToken, num));
		}
		if (_iconRoutine == null && loadingIconImage != null && loadingIconFrames != null && loadingIconFrames.Length != 0)
		{
			_iconRoutine = StartCoroutine(IconAnimationRoutine());
		}
		StartFade(1f, fadeInDuration);
	}

	public void Hide()
	{
		_showToken++;
		if (_safetyRoutine != null)
		{
			StopCoroutine(_safetyRoutine);
			_safetyRoutine = null;
		}
		StartFade(0f, fadeOutDuration);
	}

	private IEnumerator SafetyTimeoutRoutine(int token, float timeout)
	{
		yield return new WaitForSeconds(timeout);
		if (token == _showToken)
		{
			Debug.LogWarning("[LoadingScreen] Güvenlik zaman aşımı — ekran zorla kapatıldı (Hide() hiç çağrılmadı).");
			_safetyRoutine = null;
			StartFade(0f, fadeOutDuration);
		}
	}

	private void StartFade(float target, float duration)
	{
		if (!(group == null))
		{
			if (_fadeRoutine != null)
			{
				StopCoroutine(_fadeRoutine);
			}
			_fadeRoutine = StartCoroutine(FadeRoutine(target, duration));
		}
	}

	private IEnumerator FadeRoutine(float target, float duration)
	{
		bool flag = target > 0.01f;
		group.blocksRaycasts = flag;
		group.interactable = flag;
		float start = group.alpha;
		float t = 0f;
		float dur = Mathf.Max(0.01f, duration);
		while (t < dur)
		{
			t += Time.deltaTime;
			group.alpha = Mathf.Lerp(start, target, Mathf.Clamp01(t / dur));
			yield return null;
		}
		group.alpha = target;
		_fadeRoutine = null;
		if (target <= 0.01f && _iconRoutine != null)
		{
			StopCoroutine(_iconRoutine);
			_iconRoutine = null;
		}
	}

	private IEnumerator IconAnimationRoutine()
	{
		int frame = 0;
		WaitForSeconds wait = new WaitForSeconds(1f / Mathf.Max(0.01f, frameRate));
		while (true)
		{
			loadingIconImage.sprite = loadingIconFrames[frame];
			frame = (frame + 1) % loadingIconFrames.Length;
			yield return wait;
		}
	}
}
