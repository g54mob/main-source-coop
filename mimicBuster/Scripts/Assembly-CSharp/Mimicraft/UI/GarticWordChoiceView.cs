using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class GarticWordChoiceView : MonoBehaviour
	{
		[Tooltip("Panelin kökü. Seçim yapılmadığı sürece kapalı durur.")]
		[SerializeField]
		private GameObject root;

		[Tooltip("Üç seçenek butonu, sırayla. Daha az kelime gelirse fazlası gizlenir.")]
		[SerializeField]
		private Button[] choiceButtons = new Button[3];

		[Tooltip("Her butonun içindeki etiket, butonlarla aynı sırada.")]
		[SerializeField]
		private TextMeshProUGUI[] choiceLabels = new TextMeshProUGUI[3];

		public static GarticWordChoiceView Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			Hide();
			for (int i = 0; i < choiceButtons.Length; i++)
			{
				if (!(choiceButtons[i] == null))
				{
					int index = i;
					choiceButtons[i].onClick.RemoveAllListeners();
					choiceButtons[i].onClick.AddListener(delegate
					{
						Choose(index);
					});
				}
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Show(string[] choices)
		{
			if (root == null || choices == null)
			{
				return;
			}
			for (int i = 0; i < choiceButtons.Length; i++)
			{
				bool flag = i < choices.Length;
				if (choiceButtons[i] != null)
				{
					choiceButtons[i].gameObject.SetActive(flag);
				}
				if (flag && i < choiceLabels.Length && choiceLabels[i] != null)
				{
					choiceLabels[i].text = choices[i];
				}
			}
			root.SetActive(value: true);
		}

		public void Hide()
		{
			if (root != null)
			{
				root.SetActive(value: false);
			}
		}

		private void Choose(int index)
		{
			Hide();
			if (GameModeController.Current is GarticRoundManager garticRoundManager)
			{
				garticRoundManager.SelectWordServerRpc(index);
			}
		}
	}
}
