using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class LoadingScreenView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan kök obje. Boş bırakılırsa bu objenin kendisi kullanılır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Dolan çubuk. Image Type'ı FILLED olmalı - Fill Amount 0..1 arası sürülüyor. Simple bırakırsan hiçbir şey olmaz, çünkü fillAmount o modda çizimi etkilemez.")]
		[SerializeField]
		private Image fillImage;

		[Tooltip("Yüzde yazısı - '%42'. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI percentLabel;

		[Tooltip("Ne yapıldığını yazan satır - 'Harita yükleniyor'. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI stepLabel;

		[Tooltip("Her açılışta rastgele bir ipucu yazılan satır. İsteğe bağlı. İpuçları UI tablosunda Loading.Tip.1, Loading.Tip.2, ... anahtarlarıdır; yeni ipucu eklemek için tabloya bir sonraki numarayla satır ekle, kodda değişiklik gerekmez.")]
		[SerializeField]
		private TextMeshProUGUI tipLabel;

		[Tooltip("Yumuşak açılıp kapanma için. Boş bırakılırsa ekran anında görünüp kaybolur.")]
		[SerializeField]
		private CanvasGroup fade;

		[Tooltip("Açılma/kapanma süresi, saniye. Fade bağlı değilse önemsiz.")]
		[SerializeField]
		[Min(0f)]
		private float fadeSeconds = 0.25f;

		[Tooltip("Çubuğun hedefe yetişme hızı, saniyede oran olarak. 0 = anında.\n\nYumuşatmanın sebebi kozmetik değil: ilerleme gerçekten sıçramalı geliyor - bir sahne olayı bitince yüzde bir anda yerinden zıplıyor - ve zıplayan bir çubuk, duran bir çubuktan daha çok 'bozuk' görünüyor.")]
		[SerializeField]
		[Min(0f)]
		private float fillSpeed = 2.5f;

		private float target;

		private float shown;

		private bool visible;

		private const string TipKeyPrefix = "Loading.Tip.";

		private static int tipCount = -1;

		private static int lastTip;

		private int tip;

		private string stepKey;

		public bool IsVisible => visible;

		private void Awake()
		{
			if (panel == null)
			{
				panel = base.gameObject;
			}
			visible = false;
			shown = 0f;
			target = 0f;
			if (fade != null)
			{
				fade.alpha = 0f;
			}
			panel.SetActive(value: false);
		}

		public void Show(string stepKey)
		{
			visible = true;
			shown = 0f;
			target = 0f;
			SetStep(stepKey);
			PickTip();
			panel.SetActive(value: true);
			Apply();
		}

		private void PickTip()
		{
			if (tipLabel == null)
			{
				return;
			}
			int num = CountTips();
			if (num <= 0)
			{
				tip = 0;
				tipLabel.text = "";
				return;
			}
			int num2 = Random.Range(1, num + 1);
			if (num > 1 && num2 == lastTip)
			{
				num2 = num2 % num + 1;
			}
			tip = num2;
			lastTip = num2;
			ApplyTip();
		}

		private void ApplyTip()
		{
			if (tipLabel != null)
			{
				tipLabel.text = ((tip > 0) ? Loc.Get("Loading.Tip." + tip) : "");
			}
		}

		private static int CountTips()
		{
			if (tipCount >= 0)
			{
				return tipCount;
			}
			int i;
			for (i = 0; i < 200 && Loc.Has("Loading.Tip." + (i + 1)); i++)
			{
			}
			tipCount = i;
			return i;
		}

		public void Hide()
		{
			visible = false;
			shown = 1f;
			target = 1f;
			Apply();
			if (fade == null || fadeSeconds <= 0f)
			{
				panel.SetActive(value: false);
			}
		}

		public void SetStep(string stepKey)
		{
			this.stepKey = stepKey;
			if (stepLabel != null)
			{
				stepLabel.text = (string.IsNullOrEmpty(stepKey) ? "" : Loc.Get(stepKey));
			}
		}

		public void SetProgress(float value)
		{
			target = Mathf.Max(target, Mathf.Clamp01(value));
		}

		private void OnEnable()
		{
			Loc.Changed += ReapplyStep;
		}

		private void OnDisable()
		{
			Loc.Changed -= ReapplyStep;
		}

		private void ReapplyStep()
		{
			SetStep(stepKey);
			tipCount = -1;
			ApplyTip();
		}

		private void Update()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			shown = ((fillSpeed <= 0f) ? target : Mathf.MoveTowards(shown, target, fillSpeed * unscaledDeltaTime));
			if (fade != null && fadeSeconds > 0f)
			{
				fade.alpha = Mathf.MoveTowards(fade.alpha, visible ? 1f : 0f, unscaledDeltaTime / fadeSeconds);
				if (!visible && fade.alpha <= 0f && panel.activeSelf)
				{
					panel.SetActive(value: false);
				}
			}
			Apply();
		}

		private void Apply()
		{
			if (fillImage != null)
			{
				fillImage.fillAmount = shown;
			}
			if (percentLabel != null)
			{
				percentLabel.text = Loc.Format("Loading.Percent", Mathf.RoundToInt(shown * 100f));
			}
		}
	}
}
