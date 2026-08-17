using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.GameMenu.Panels
{
	public class GameMenuRootPanel : MonoBehaviour
	{
		[SerializeField]
		private Button resumeButton;

		[SerializeField]
		private Button settingsButton;

		[SerializeField]
		private Button backToMenuButton;

		[SerializeField]
		private Button respawnButton;

		[SerializeField]
		private Button saveButton;

		[Tooltip("Respawn label color when the button is disabled (player not standing) — grey + faded.")]
		[SerializeField]
		private Color respawnDisabledLabelColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		[Inject]
		private IGameMenuUIManager _gameMenuUIManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private IPlayerRespawnService _respawnService;

		[Inject]
		private IGameSaveService _gameSaveService;

		private void Start()
		{
			resumeButton?.onClick.AddListener(OnResume);
			settingsButton?.onClick.AddListener(OnSettings);
			backToMenuButton?.onClick.AddListener(OnBackToMenu);
			respawnButton?.onClick.AddListener(OnRespawn);
			saveButton?.onClick.AddListener(OnSave);
			if (respawnButton != null)
			{
				bool flag = _respawnService?.CanRespawn ?? false;
				respawnButton.interactable = flag;
				TMP_Text componentInChildren = respawnButton.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (componentInChildren != null && !flag)
				{
					componentInChildren.color = respawnDisabledLabelColor;
				}
			}
			if (saveButton != null)
			{
				bool flag2 = _gameSaveService?.CanSaveNow ?? false;
				saveButton.interactable = flag2;
				TMP_Text componentInChildren2 = saveButton.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (componentInChildren2 != null && !flag2)
				{
					componentInChildren2.color = respawnDisabledLabelColor;
				}
			}
		}

		private void OnResume()
		{
			_gameMenuUIManager.CloseMenu();
		}

		private void OnSettings()
		{
			_gameMenuUIManager.ShowSettings();
		}

		private void OnBackToMenu()
		{
			_networkManager.Disconnect();
		}

		private void OnRespawn()
		{
			_respawnService?.RequestRespawn();
			_gameMenuUIManager.CloseMenu();
		}

		private void OnSave()
		{
			_gameSaveService?.RequestSave();
		}

		private void OnDestroy()
		{
			resumeButton?.onClick.RemoveAllListeners();
			settingsButton?.onClick.RemoveAllListeners();
			backToMenuButton?.onClick.RemoveAllListeners();
			respawnButton?.onClick.RemoveAllListeners();
			saveButton?.onClick.RemoveAllListeners();
		}
	}
}
