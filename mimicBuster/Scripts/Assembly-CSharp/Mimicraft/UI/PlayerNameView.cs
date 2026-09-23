using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PlayerNameView : MonoBehaviour
	{
		private const float Width = 320f;

		private const float AvatarSize = 40f;

		[SerializeField]
		private RectTransform root;

		[SerializeField]
		private RectTransform steamSection;

		[SerializeField]
		private Image avatarImage;

		[SerializeField]
		private TextMeshProUGUI steamNameLabel;

		[SerializeField]
		private Button toggleNameButton;

		[SerializeField]
		private RectTransform nameFieldSection;

		[SerializeField]
		private TMP_InputField nameField;

		private bool lastSteamConnected;

		private bool hasCheckedSteamOnce;

		private bool isNameFieldExpanded;

		private bool avatarFetchStarted;

		public static PlayerNameView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "PlayerName");
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(0f, 0f);
			rectTransform.pivot = new Vector2(0f, 0f);
			rectTransform.anchoredPosition = new Vector2(30f, 30f);
			rectTransform.sizeDelta = new Vector2(320f, 0f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 6f;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childControlWidth = false;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "SteamSection");
			rectTransform2.sizeDelta = new Vector2(320f, 40f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = true;
			Image image = UIFactory.CreatePanel(rectTransform2, "Avatar", new Color(1f, 1f, 1f, 0.08f));
			((RectTransform)image.transform).sizeDelta = new Vector2(40f, 40f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform2, "SteamName", "", 15);
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.fontStyle = FontStyles.Bold;
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(264f, 40f);
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "ToggleRow");
			rectTransform3.sizeDelta = new Vector2(320f, 26f);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform3, "ToggleNameButton", "Yerel oyuncu adını ayarla", out text);
			text.fontSize = 12f;
			RectTransform obj = (RectTransform)button.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			RectTransform rectTransform4 = UIFactory.CreateRect(rectTransform, "NameFieldSection");
			rectTransform4.sizeDelta = new Vector2(320f, 54f);
			VerticalLayoutGroup verticalLayoutGroup2 = rectTransform4.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 2f;
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childControlWidth = false;
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform4, "Caption", "Oyuncu ismin", 13);
			textMeshProUGUI2.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI2.color = new Color(1f, 1f, 1f, 0.65f);
			((RectTransform)textMeshProUGUI2.transform).sizeDelta = new Vector2(320f, 18f);
			TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform4, "NameField", "Oyuncu ismin");
			tMP_InputField.characterLimit = 16;
			((RectTransform)tMP_InputField.transform).sizeDelta = new Vector2(320f, 30f);
			PlayerNameView playerNameView = rectTransform.gameObject.AddComponent<PlayerNameView>();
			playerNameView.root = rectTransform;
			playerNameView.steamSection = rectTransform2;
			playerNameView.avatarImage = image;
			playerNameView.steamNameLabel = textMeshProUGUI;
			playerNameView.toggleNameButton = button;
			playerNameView.nameFieldSection = rectTransform4;
			playerNameView.nameField = tMP_InputField;
			return playerNameView;
		}

		private void Awake()
		{
			nameField.SetTextWithoutNotify(PlayerNameStore.Get());
			nameField.onValueChanged.RemoveAllListeners();
			nameField.onValueChanged.AddListener(PlayerNameStore.Set);
			toggleNameButton.onClick.RemoveAllListeners();
			toggleNameButton.onClick.AddListener(ToggleNameField);
			ApplySteamConnectionState(SteamManager.IsInitialized);
		}

		private void Update()
		{
			bool isInitialized = SteamManager.IsInitialized;
			if (!hasCheckedSteamOnce || isInitialized != lastSteamConnected)
			{
				hasCheckedSteamOnce = true;
				ApplySteamConnectionState(isInitialized);
			}
		}

		private void ApplySteamConnectionState(bool connected)
		{
			lastSteamConnected = connected;
			steamSection.gameObject.SetActive(connected);
			toggleNameButton.gameObject.SetActive(connected);
			isNameFieldExpanded = !connected;
			nameFieldSection.gameObject.SetActive(isNameFieldExpanded);
			if (connected)
			{
				steamNameLabel.text = SteamManager.LocalName;
				if (!PlayerNameStore.HasRealName)
				{
					nameField.SetTextWithoutNotify(PlayerNameStore.Get());
				}
				if (!avatarFetchStarted)
				{
					avatarFetchStarted = true;
					FetchAvatar();
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(root);
		}

		private void ToggleNameField()
		{
			isNameFieldExpanded = !isNameFieldExpanded;
			nameFieldSection.gameObject.SetActive(isNameFieldExpanded);
			LayoutRebuilder.ForceRebuildLayoutImmediate(root);
		}

		private async void FetchAvatar()
		{
			Texture2D texture2D = await SteamManager.FetchLocalAvatarAsync();
			if (!(texture2D == null) && !(avatarImage == null))
			{
				avatarImage.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
				avatarImage.color = Color.white;
			}
		}
	}
}
