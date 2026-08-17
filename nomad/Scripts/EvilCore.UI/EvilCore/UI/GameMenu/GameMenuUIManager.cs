using Cysharp.Threading.Tasks;
using EvilCore.Networking;
using EvilCore.UI.Scripts;
using EvilCore.UI.Settings;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.GameMenu
{
	public class GameMenuUIManager : MonoBehaviour, IGameMenuUIManager
	{
		[Header("Always-open")]
		[Tooltip("The right-side players / game info panel. Stays open; hidden only while Settings is shown.")]
		[SerializeField]
		private BaseCanvasGroupController gameInfoPanel;

		[Header("Settings (slides in over the content area)")]
		[SerializeField]
		private SettingsPanel settingsPanel;

		[Tooltip("Same component the MainMenu uses. sidePanel = left Resume/Settings/Back panel, contentPanel = Settings.")]
		[SerializeField]
		private UISlidePanelTransition settingsTransition;

		[Header("Open/close animation (set each UIAppear's playOnEnable = false; driven here)")]
		[Tooltip("Side panel. Set direction = Right so it enters from the left (and exits back left).")]
		[SerializeField]
		private UIAppear sidePanelAppear;

		[Tooltip("Game info. Set direction = Left so it enters from the right (keep its useFade off).")]
		[SerializeField]
		private UIAppear gameInfoAppear;

		[Inject]
		private ISceneFlowManager _sceneFlowManager;

		[Inject]
		private INetworkManager _networkManager;

		private bool _closing;

		private bool _pausedTime;

		private void Start()
		{
			gameInfoPanel?.Initialize();
			if (settingsPanel != null)
			{
				settingsPanel.Initialize(CloseSettings);
			}
			gameInfoPanel?.Show(interactable: true, blockRaycast: true);
			PlayEntrance();
			if (_networkManager != null && _networkManager.IsSingleplayerSession)
			{
				Time.timeScale = 0f;
				_pausedTime = true;
			}
		}

		private void OnDestroy()
		{
			if (_pausedTime)
			{
				Time.timeScale = 1f;
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (settingsTransition != null && settingsTransition.IsContentShown)
				{
					CloseSettings();
				}
				else
				{
					CloseMenu();
				}
			}
		}

		private void PlayEntrance()
		{
			sidePanelAppear?.Play();
			gameInfoAppear?.Play();
		}

		public void CloseMenu()
		{
			int pending;
			if (!_closing)
			{
				_closing = true;
				pending = ((sidePanelAppear != null) ? 1 : 0) + ((gameInfoAppear != null) ? 1 : 0);
				if (pending == 0)
				{
					_sceneFlowManager?.UnloadGameMenuSceneAsync().Forget();
					return;
				}
				sidePanelAppear?.PlayExit(Done);
				gameInfoAppear?.PlayExit(Done);
			}
			void Done()
			{
				if (--pending <= 0)
				{
					_sceneFlowManager?.UnloadGameMenuSceneAsync().Forget();
				}
			}
		}

		public void ShowSettings()
		{
			gameInfoPanel?.Hide();
			settingsTransition?.ShowContent();
			if (settingsPanel != null)
			{
				settingsPanel.OnPanelShown();
			}
		}

		public void CloseSettings()
		{
			settingsTransition?.ShowSide();
			gameInfoPanel?.Show(interactable: true, blockRaycast: true);
		}
	}
}
