using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceNotificationUI : MonoBehaviour
{
	[Header("Buzzing Bildirimi (sinek uyarısı)")]
	public GameObject buzzingNotification;

	[Tooltip("Buzzing bekleme geri sayımı (5→0), opsiyonel")]
	public TextMeshProUGUI buzzingCountdownText;

	[Tooltip("buzzingCountdownText ile aynı objede — süre her düştüğünde küçük bir scale-up darbesi oynatır")]
	public PunchScaleText buzzingCountdownPunch;

	[Tooltip("Bildirimin açılış/kapanış pop in/out animasyon süresi (sn) — direkt görünüp kaybolmasın diye")]
	public float buzzingPopDuration = 0.18f;

	[Tooltip("Sinek evresi (gerçek sinekler görünür oldu) süresince bildirim büyüyüp küçülerek nabız gibi atar — ne kadar büyüyüp küçüleceği")]
	public float pulseScaleAmount = 0.045f;

	[Tooltip("Nabız hızı")]
	public float pulseSpeed = 2.5f;

	private bool _buzzingVisible;

	private Coroutine _buzzingPopRoutine;

	private bool _pulsing;

	private Coroutine _pulseRoutine;

	[Header("Make Sound Bildirimi (tekme yedin)")]
	public GameObject makeSoundNotification;

	[Tooltip("Bildirimin ekranda kalma süresi (sn)")]
	public float makeSoundDuration = 4f;

	[Header("Ses Eşiği (sinek/buzzing'i temizlemek için gereken ses)")]
	[Tooltip("PlayerVoiceMonitor'den taşındı — bu amplitude üstü 'ses çıkardı' sayılır (Dissonance amplitude ölçeği, ~0.02-0.05).")]
	public float speakThreshold = 0.025f;

	[Tooltip("PlayerVoiceMonitor'den taşındı — eşik üstünde en az bu kadar KESİNTİSİZ ses gerekli (sn), sinek/buzzing evresini temizlemek için.")]
	public float minSpeakDuration = 0.05f;

	[Header("Mic Seviye Barı (vertical fill)")]
	public Image micLevelBarFill;

	public float levelGain = 12f;

	public float levelSmoothing = 14f;

	private float _barValue;

	private float _makeSoundTimer;

	[Header("Fly Countdown (dolum barı)")]
	[Tooltip("Sinek/buzzing'e kalan süreyi gösteren dolum barı (metin sistemi iptal edildi, yerine bu geldi)")]
	public Image flyCountdownBarFill;

	[Tooltip("Bar + arkaplanının kapsayıcısı (FlyCountdownBox) — buzzing bildirimiyle üst üste binmemesi için Y ekseninde kayarak gizlenir/gösterilir")]
	public RectTransform flyCountdownBoxRect;

	[Tooltip("Bar görünürken Y (anchoredPosition.y) — varsayılan oyun başındaki konum")]
	public float flyCountdownVisibleY = -65f;

	[Tooltip("Bar gizliyken (buzzing bildirimi ekrandayken) Y")]
	public float flyCountdownHiddenY = 175f;

	public float flyCountdownSlideSpeed = 8f;

	[Tooltip("İSTEK: kaymanın YANINDA CanvasGroup.alpha da gizlensin/gösterilsin — sadece pozisyonla gizlemek farklı çözünürlüklerde 'gizli' konumun hâlâ ekran içinde kalabilmesi yüzünden sorun çıkarıyordu. Alpha'nın 0↔1 arası geçiş hızı.")]
	public float flyCountdownFadeSpeed = 10f;

	private CanvasGroup _flyCountdownCanvasGroup;

	[Tooltip("Yerken (Eat) sinek barı yavaşlarken bu renge döner (turuncumsu) — bırakınca normal rengine geri döner")]
	public Color flyCountdownEatingColor = new Color32(byte.MaxValue, 154, 46, byte.MaxValue);

	[Tooltip("Yemiyorken (normal) sinek barının rengi")]
	public Color flyCountdownNormalColor = new Color32(85, 201, 74, byte.MaxValue);

	private float _flyCountdownTargetY;

	private bool _flyCountdownEating;

	public static VoiceNotificationUI Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (buzzingNotification != null)
		{
			buzzingNotification.SetActive(value: false);
		}
		if (makeSoundNotification != null)
		{
			makeSoundNotification.SetActive(value: false);
		}
		if (micLevelBarFill != null)
		{
			micLevelBarFill.fillOrigin = 0;
		}
		ResetMicLevel();
		if (flyCountdownBoxRect != null)
		{
			_flyCountdownTargetY = flyCountdownVisibleY;
			Vector2 anchoredPosition = flyCountdownBoxRect.anchoredPosition;
			anchoredPosition.y = flyCountdownVisibleY;
			flyCountdownBoxRect.anchoredPosition = anchoredPosition;
			_flyCountdownCanvasGroup = flyCountdownBoxRect.GetComponent<CanvasGroup>();
			if (_flyCountdownCanvasGroup == null)
			{
				_flyCountdownCanvasGroup = flyCountdownBoxRect.gameObject.AddComponent<CanvasGroup>();
			}
			_flyCountdownCanvasGroup.alpha = 1f;
			flyCountdownBoxRect.gameObject.SetActive(value: true);
		}
		if (flyCountdownBarFill != null)
		{
			flyCountdownBarFill.color = flyCountdownNormalColor;
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void Update()
	{
		if (_makeSoundTimer > 0f)
		{
			_makeSoundTimer -= Time.deltaTime;
			if (_makeSoundTimer <= 0f && makeSoundNotification != null)
			{
				makeSoundNotification.SetActive(value: false);
			}
		}
		if (!(flyCountdownBoxRect != null))
		{
			return;
		}
		Vector2 anchoredPosition = flyCountdownBoxRect.anchoredPosition;
		anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, _flyCountdownTargetY, Time.deltaTime * flyCountdownSlideSpeed);
		flyCountdownBoxRect.anchoredPosition = anchoredPosition;
		if (_flyCountdownCanvasGroup != null)
		{
			bool num = _flyCountdownTargetY == flyCountdownVisibleY;
			float target = (num ? 1f : 0f);
			_flyCountdownCanvasGroup.alpha = Mathf.MoveTowards(_flyCountdownCanvasGroup.alpha, target, Time.deltaTime * flyCountdownFadeSpeed);
			if (!num && _flyCountdownCanvasGroup.alpha <= 0.01f && flyCountdownBoxRect.gameObject.activeSelf)
			{
				flyCountdownBoxRect.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetFlyCountdown(float remaining, float total)
	{
		bool flag = remaining >= 0f;
		_flyCountdownTargetY = (flag ? flyCountdownVisibleY : flyCountdownHiddenY);
		if (flag && flyCountdownBoxRect != null && !flyCountdownBoxRect.gameObject.activeSelf)
		{
			flyCountdownBoxRect.gameObject.SetActive(value: true);
		}
		if (flag && flyCountdownBarFill != null)
		{
			flyCountdownBarFill.fillAmount = ((total > 0f) ? Mathf.Clamp01(1f - remaining / total) : 1f);
		}
	}

	public void SetFlyFillExternal(bool visible, float fill01)
	{
		_flyCountdownTargetY = (visible ? flyCountdownVisibleY : flyCountdownHiddenY);
		if (visible && flyCountdownBoxRect != null && !flyCountdownBoxRect.gameObject.activeSelf)
		{
			flyCountdownBoxRect.gameObject.SetActive(value: true);
		}
		if (visible && flyCountdownBarFill != null)
		{
			flyCountdownBarFill.fillAmount = Mathf.Clamp01(fill01);
		}
	}

	public void SetFlyCountdownEating(bool eating)
	{
		if (!(flyCountdownBarFill == null) && eating != _flyCountdownEating)
		{
			_flyCountdownEating = eating;
			flyCountdownBarFill.color = (eating ? flyCountdownEatingColor : flyCountdownNormalColor);
		}
	}

	public void ShowBuzzing()
	{
		if (!(buzzingNotification == null) && !_buzzingVisible)
		{
			_buzzingVisible = true;
			buzzingNotification.SetActive(value: true);
			if (_buzzingPopRoutine != null)
			{
				StopCoroutine(_buzzingPopRoutine);
			}
			_buzzingPopRoutine = StartCoroutine(PopBuzzingNotification(show: true));
		}
	}

	public void HideBuzzing()
	{
		if (buzzingCountdownText != null)
		{
			buzzingCountdownText.text = "";
		}
		SetFlyPulsing(pulsing: false);
		if (!(buzzingNotification == null) && _buzzingVisible)
		{
			_buzzingVisible = false;
			if (_buzzingPopRoutine != null)
			{
				StopCoroutine(_buzzingPopRoutine);
			}
			_buzzingPopRoutine = StartCoroutine(PopBuzzingNotification(show: false));
		}
	}

	public void SetFlyPulsing(bool pulsing)
	{
		if (pulsing == _pulsing)
		{
			return;
		}
		_pulsing = pulsing;
		if (pulsing)
		{
			if (_pulseRoutine != null)
			{
				StopCoroutine(_pulseRoutine);
			}
			_pulseRoutine = StartCoroutine(PulseLoop());
		}
		else if (_pulseRoutine != null)
		{
			StopCoroutine(_pulseRoutine);
			_pulseRoutine = null;
			RectTransform rectTransform = ((buzzingNotification != null) ? buzzingNotification.GetComponent<RectTransform>() : null);
			if (rectTransform != null)
			{
				rectTransform.localScale = Vector3.one;
			}
		}
	}

	private IEnumerator PulseLoop()
	{
		RectTransform rt = ((buzzingNotification != null) ? buzzingNotification.GetComponent<RectTransform>() : null);
		if (rt == null)
		{
			yield break;
		}
		float t = 0f;
		while (true)
		{
			t += Time.deltaTime * pulseSpeed;
			rt.localScale = Vector3.one * (1f + Mathf.Sin(t) * pulseScaleAmount);
			yield return null;
		}
	}

	private IEnumerator PopBuzzingNotification(bool show)
	{
		CanvasGroup cg = buzzingNotification.GetComponent<CanvasGroup>();
		if (cg == null)
		{
			cg = buzzingNotification.AddComponent<CanvasGroup>();
		}
		RectTransform rt = buzzingNotification.GetComponent<RectTransform>();
		float fromAlpha = (show ? 0f : 1f);
		float toAlpha = (show ? 1f : 0f);
		float fromScale = (show ? 0.8f : 1f);
		float toScale = (show ? 1f : 0.8f);
		cg.alpha = fromAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * fromScale;
		}
		float t = 0f;
		while (t < buzzingPopDuration)
		{
			t += Time.deltaTime;
			float t2 = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / buzzingPopDuration), 3f);
			cg.alpha = Mathf.Lerp(fromAlpha, toAlpha, t2);
			if (rt != null)
			{
				rt.localScale = Vector3.one * Mathf.Lerp(fromScale, toScale, t2);
			}
			yield return null;
		}
		cg.alpha = toAlpha;
		if (rt != null)
		{
			rt.localScale = Vector3.one * toScale;
		}
		if (!show)
		{
			buzzingNotification.SetActive(value: false);
		}
		_buzzingPopRoutine = null;
	}

	public void SetBuzzingCountdown(float seconds)
	{
		if (!(buzzingCountdownText == null))
		{
			string text = Mathf.CeilToInt(Mathf.Max(0f, seconds)).ToString();
			if (!(text == buzzingCountdownText.text))
			{
				buzzingCountdownText.text = text;
				buzzingCountdownPunch?.Play();
			}
		}
	}

	public void ShowMakeSound()
	{
		if (makeSoundNotification != null)
		{
			makeSoundNotification.SetActive(value: true);
		}
		_makeSoundTimer = makeSoundDuration;
	}

	public void UpdateMicLevel(float amplitude01)
	{
		if (!(micLevelBarFill == null))
		{
			float b = Mathf.Clamp01(amplitude01 * levelGain);
			_barValue = Mathf.Lerp(_barValue, b, Time.deltaTime * levelSmoothing);
			micLevelBarFill.fillAmount = _barValue;
		}
	}

	public void ResetMicLevel()
	{
		_barValue = 0f;
		if (micLevelBarFill != null)
		{
			micLevelBarFill.fillAmount = 0f;
		}
	}
}
