using System;
using DG.Tweening;
using Mimicraft.Analytics;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Tutorial;
using Mimicraft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SplashScreenView : MonoBehaviour
{
	[Tooltip("Splash paneli - sahnede INAKTİF olarak bırak, bunu bu script açar/kapatır.")]
	[SerializeField]
	private GameObject splashPanel;

	[Tooltip("Bir tuşa basılınca yerini alacak ana menü paneli (Main).")]
	[SerializeField]
	private GameObject mainPanel;

	[Tooltip("Logo - girişte hafifçe büyüyerek belirir, çıkışta hafifçe büyüyüp kaybolur. Boş bırakılabilir.")]
	[SerializeField]
	private RectTransform logo;

	[Tooltip("'Bir tuşa bas' yazısı - panel tam açıldıktan sonra yanıp söner. Boş bırakılabilir, o zaman yanıp sönme olmaz.")]
	[SerializeField]
	private TextMeshProUGUI pressAnyKeyLabel;

	[Header("Kamera (isteğe bağlı)")]
	[Tooltip("Sahnenin 3B kamerası (Menu sahnesindeki 'Camera'). Boş bırakılırsa kamera hiç oynatılmaz - panel geçişi eskisi gibi çalışır.")]
	[SerializeField]
	private Transform sceneCamera;

	[Tooltip("Splash açıkken kameranın duracağı konum/açı. Boş bir obje koyup Scene view'da Move/Rotate ile istediğin yere taşı - konumu ve rotasyonu buradan okunur, kendisi hiç görünmez.")]
	[SerializeField]
	private Transform splashCameraPose;

	[Tooltip("Main panele geçince kameranın gideceği konum/açı. Aynı şekilde boş bir obje.")]
	[SerializeField]
	private Transform mainCameraPose;

	[Tooltip("Splash'ten Main'e geçerken kameranın bir konumdan diğerine kayma süresi (saniye). Panel crossfade'inden (Fade Out + Fade In) ayrı bir süre - kamera hareketi genelde biraz daha uzun tutulunca sinematik durur.")]
	[SerializeField]
	[Min(0.05f)]
	private float cameraMoveSeconds = 1.2f;

	[SerializeField]
	[Min(0.05f)]
	private float fadeInSeconds = 0.6f;

	[SerializeField]
	[Min(0.05f)]
	private float fadeOutSeconds = 0.35f;

	[Tooltip("İlk açılışta eğitime girerken ekranın kararma süresi (saniye). Menüye giriş yerine kullanılır - bkz. TutorialState.ShouldOpenOnFirstRun.")]
	[SerializeField]
	[Min(0.05f)]
	private float tutorialFadeSeconds = 0.5f;

	[SerializeField]
	[Range(0f, 0.3f)]
	private float logoPunchScale = 0.08f;

	[Tooltip("Logo tam açıldıktan sonra sağa/sola ne kadar yatacağı, derece. Bir yöne gidiş - diğer yöne gidiş toplamı değil, MERKEZDEN her iki yana bu kadar. 0 = hiç sallanmaz.")]
	[SerializeField]
	[Range(0f, 30f)]
	private float logoSwayAngle = 6f;

	[Tooltip("Bir yöne gidişin süresi (saniye) - 'sağa sola' bir TAM turun değil, YARISININ süresi. Küçük tutmak logoyu telaşlandırır, büyük tutmak neredeyse fark edilmez yapar.")]
	[SerializeField]
	[Min(0.1f)]
	private float logoSwaySeconds = 2.5f;

	private static bool shownThisLaunch;

	private CanvasGroup splashGroup;

	private CanvasGroup mainGroup;

	private Tween blink;

	private Tween sway;

	private bool acceptingInput;

	public static bool IsShowing { get; private set; }

	public static event Action Dismissed;

	private void Awake()
	{
		if (shownThisLaunch || splashPanel == null)
		{
			ApplyCameraPose(mainCameraPose);
			MusicDirector.Muffle(on: false);
			base.enabled = false;
			return;
		}
		shownThisLaunch = true;
		IsShowing = true;
		LocalizedText.AttachIfUnset(pressAnyKeyLabel, "Splash.PressAnyKey");
		ApplyCameraPose(splashCameraPose);
		MusicDirector.Muffle(on: true);
		splashGroup = splashPanel.GetComponent<CanvasGroup>();
		if (splashGroup == null)
		{
			splashGroup = splashPanel.AddComponent<CanvasGroup>();
		}
		splashGroup.alpha = 0f;
		splashGroup.blocksRaycasts = true;
		splashGroup.interactable = true;
		splashPanel.SetActive(value: true);
		if (mainPanel != null)
		{
			mainGroup = mainPanel.GetComponent<CanvasGroup>();
			if (mainGroup == null)
			{
				mainGroup = mainPanel.AddComponent<CanvasGroup>();
			}
			mainGroup.alpha = 0f;
			mainPanel.SetActive(value: false);
		}
		if (logo != null)
		{
			logo.localScale = Vector3.one * (1f - logoPunchScale);
		}
		Sequence sequence = DOTween.Sequence().SetUpdate(isIndependentUpdate: true);
		sequence.Append(splashGroup.DOFade(1f, fadeInSeconds).SetEase(Ease.OutQuad));
		if (logo != null)
		{
			sequence.Join(logo.DOScale(1f, fadeInSeconds).SetEase(Ease.OutBack));
		}
		sequence.OnComplete(BeginWaitingForInput);
	}

	private void BeginWaitingForInput()
	{
		acceptingInput = true;
		if (pressAnyKeyLabel != null)
		{
			blink = pressAnyKeyLabel.DOFade(0.15f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine)
				.SetUpdate(isIndependentUpdate: true);
		}
		BeginLogoSway();
	}

	private void BeginLogoSway()
	{
		if (!(logo == null) && !(logoSwayAngle <= 0f))
		{
			logo.localRotation = Quaternion.Euler(0f, 0f, 0f - logoSwayAngle);
			sway = logo.DOLocalRotate(new Vector3(0f, 0f, logoSwayAngle), logoSwaySeconds).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
				.SetUpdate(isIndependentUpdate: true);
		}
	}

	private void Update()
	{
		if (acceptingInput && AnyKeyPressed())
		{
			Dismiss();
		}
	}

	private static bool AnyKeyPressed()
	{
		if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
		{
			return true;
		}
		if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			return true;
		}
		if (Gamepad.current != null)
		{
			return Gamepad.current.startButton.wasPressedThisFrame;
		}
		return false;
	}

	private void Dismiss()
	{
		acceptingInput = false;
		blink?.Kill();
		sway?.Kill();
		if (AnalyticsConsentPrompt.ShouldAsk)
		{
			AnalyticsConsentPrompt.Ask(ContinueDismiss);
		}
		else
		{
			ContinueDismiss();
		}
	}

	private void ContinueDismiss()
	{
		if (TutorialState.ShouldOpenOnFirstRun)
		{
			OpenTutorial();
			return;
		}
		MusicDirector.Muffle(on: false);
		if (sceneCamera != null && mainCameraPose != null)
		{
			sceneCamera.DOMove(mainCameraPose.position, cameraMoveSeconds).SetEase(Ease.InOutSine).SetUpdate(isIndependentUpdate: true);
			sceneCamera.DORotateQuaternion(mainCameraPose.rotation, cameraMoveSeconds).SetEase(Ease.InOutSine).SetUpdate(isIndependentUpdate: true);
		}
		Sequence sequence = DOTween.Sequence().SetUpdate(isIndependentUpdate: true);
		sequence.Append(splashGroup.DOFade(0f, fadeOutSeconds).SetEase(Ease.InQuad));
		if (logo != null)
		{
			sequence.Join(logo.DOScale(1f + logoPunchScale, fadeOutSeconds).SetEase(Ease.InQuad));
		}
		sequence.OnComplete(FinishDismiss);
	}

	private void OpenTutorial()
	{
		TutorialState.MarkOffered();
		MusicDirector.Muffle(on: false);
		Image blackout = Blackout();
		Sequence sequence = DOTween.Sequence().SetUpdate(isIndependentUpdate: true);
		sequence.Append(blackout.DOFade(1f, tutorialFadeSeconds).SetEase(Ease.InQuad));
		sequence.OnComplete(delegate
		{
			if (!PracticeSession.Start(tutorial: true))
			{
				blackout.DOFade(0f, tutorialFadeSeconds).SetEase(Ease.OutQuad).SetUpdate(isIndependentUpdate: true)
					.OnComplete(delegate
					{
						UnityEngine.Object.Destroy(blackout.transform.parent.gameObject);
					});
				FinishDismiss();
			}
		});
	}

	private Image Blackout()
	{
		GameObject gameObject = new GameObject("SplashBlackout", typeof(Canvas), typeof(CanvasScaler));
		Canvas component = gameObject.GetComponent<Canvas>();
		component.renderMode = RenderMode.ScreenSpaceOverlay;
		component.sortingOrder = 32000;
		GameObject obj = new GameObject("Black", typeof(Image));
		obj.transform.SetParent(gameObject.transform, worldPositionStays: false);
		Image component2 = obj.GetComponent<Image>();
		component2.color = new Color(0f, 0f, 0f, 0f);
		component2.raycastTarget = true;
		RectTransform obj2 = (RectTransform)obj.transform;
		obj2.anchorMin = Vector2.zero;
		obj2.anchorMax = Vector2.one;
		obj2.offsetMin = Vector2.zero;
		obj2.offsetMax = Vector2.zero;
		return component2;
	}

	private void ApplyCameraPose(Transform pose)
	{
		if (!(sceneCamera == null) && !(pose == null))
		{
			sceneCamera.SetPositionAndRotation(pose.position, pose.rotation);
		}
	}

	private void FinishDismiss()
	{
		splashGroup.blocksRaycasts = false;
		splashGroup.interactable = false;
		splashPanel.SetActive(value: false);
		if (mainPanel != null)
		{
			mainPanel.SetActive(value: true);
			mainGroup.DOFade(1f, fadeInSeconds).SetEase(Ease.OutQuad).SetUpdate(isIndependentUpdate: true);
		}
		IsShowing = false;
		SplashScreenView.Dismissed?.Invoke();
		SplashScreenView.Dismissed = null;
	}
}
