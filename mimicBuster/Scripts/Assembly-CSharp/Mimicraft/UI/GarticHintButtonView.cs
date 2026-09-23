using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class GarticHintButtonView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan blok - ampul butonu ve yanındaki yazı. Boş bırakılırsa bu objenin ÇOCUKLARI gizlenir, çünkü bu bileşen kapalı bir objede kalırsa onu geri açacak kimse olmaz.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Ampul butonu.")]
		[SerializeField]
		private Button button;

		[Tooltip("Kaç harf kaldığını yazan etiket - '2/4'. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI countLabel;

		[Tooltip("Bekleme süresi yazısı - '3sn'. Bekleme yokken boşalır. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI cooldownLabel;

		private int appliedGiven = -1;

		private int appliedMax = -1;

		private int appliedSeconds = -1;

		[Tooltip("Bekleme süresini dolduran görsel. Image Type'ı FILLED olmalı. İsteğe bağlı.")]
		[SerializeField]
		private Image cooldownFill;

		private GarticRoundManager mode;

		private bool applied;

		private bool lastShown;

		private void Awake()
		{
			if (panel == null)
			{
				panel = base.gameObject;
			}
			if (button != null)
			{
				button.onClick.AddListener(Press);
			}
		}

		private void Press()
		{
			if (mode != null)
			{
				mode.RequestHintServerRpc();
			}
		}

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as GarticRoundManager;
				if (mode == null)
				{
					SetShown(shown: false);
					return;
				}
			}
			bool localPlayerOwnsHints = mode.LocalPlayerOwnsHints;
			SetShown(localPlayerOwnsHints);
			if (localPlayerOwnsHints)
			{
				bool canLocalPlayerGiveHint = mode.CanLocalPlayerGiveHint;
				if (button != null && button.interactable != canLocalPlayerGiveHint)
				{
					button.interactable = canLocalPlayerGiveHint;
				}
				int value = mode.HintsGiven.Value;
				int value2 = mode.MaxHints.Value;
				if (countLabel != null && (value != appliedGiven || value2 != appliedMax))
				{
					appliedGiven = value;
					appliedMax = value2;
					countLabel.SetText("{0}/{1}", value, value2);
				}
				float hintCooldownRemaining = mode.HintCooldownRemaining;
				bool flag = value >= value2;
				int num = ((hintCooldownRemaining > 0f && !flag) ? Mathf.CeilToInt(hintCooldownRemaining) : 0);
				if (cooldownLabel != null && num != appliedSeconds)
				{
					appliedSeconds = num;
					cooldownLabel.text = ((num > 0) ? Loc.Format("Common.Seconds", num) : "");
				}
				if (cooldownFill != null)
				{
					float num2 = 8f;
					cooldownFill.fillAmount = (flag ? 0f : (1f - Mathf.Clamp01(hintCooldownRemaining / num2)));
				}
			}
		}

		private void SetShown(bool shown)
		{
			if (applied && shown == lastShown)
			{
				return;
			}
			applied = true;
			lastShown = shown;
			if (panel != base.gameObject)
			{
				panel.SetActive(shown);
				return;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (gameObject.activeSelf != shown)
				{
					gameObject.SetActive(shown);
				}
			}
		}
	}
}
