using System.Collections.Generic;
using EvilCore.Extensions;
using EvilCore.Localization;
using EvilCore.Networking;
using EvilCore.UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class CreateGamePanel : MonoBehaviour
	{
		[Header("Selectors")]
		[SerializeField]
		private OptionSelector worldNameSelector;

		[SerializeField]
		private OptionSelector seedSelector;

		[SerializeField]
		private OptionSelector maxPlayersSelector;

		[SerializeField]
		private OptionSelector gameTypeSelector;

		[SerializeField]
		private OptionSelector passwordSelector;

		[Header("Actions")]
		[SerializeField]
		private Button createButton;

		[SerializeField]
		private Button backButton;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IGameSaveService _gameSaveService;

		private const int GameTypePrivateIndex = 0;

		private const int GameTypePublicIndex = 1;

		private const int MinPlayers = 1;

		private const int MaxPlayersCap = 4;

		private const int WorldNameMaxLength = 24;

		private const int PasswordMaxLength = 24;

		private const string DefaultWorldName = "Nomad Drive Trip";

		private const int SeedDigits = 6;

		private const int SeedMin = 100000;

		private const int SeedMaxExclusive = 1000000;

		private void Start()
		{
			base.gameObject.InjectGameObject();
			ConfigureSelectors();
			createButton.onClick.AddListener(OnCreateClicked);
			backButton.onClick.AddListener(delegate
			{
				_uiManager.ShowMainPanel();
			});
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += OnLocaleChanged;
			}
		}

		private void OnDestroy()
		{
			createButton?.onClick.RemoveAllListeners();
			backButton?.onClick.RemoveAllListeners();
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= OnLocaleChanged;
			}
		}

		public void OnPanelShown()
		{
			seedSelector.SetTextValueWithoutNotify(GenerateSeed());
		}

		private void ConfigureSelectors()
		{
			int num = Mathf.Clamp((_networkManager != null) ? _networkManager.MaxConnections : 4, 1, 4);
			worldNameSelector.SetTextInput("Nomad Drive Trip", 24, WorldNamePlaceholder());
			seedSelector.SetTextInput(GenerateSeed(), 6, SeedPlaceholder(), masked: false, numeric: true);
			maxPlayersSelector.SetRange(1f, num, 1f);
			maxPlayersSelector.SetRangeValueWithoutNotify(num);
			gameTypeSelector.SetOptions(GameTypeOptions());
			gameTypeSelector.SetValueWithoutNotify(0);
			if (passwordSelector != null)
			{
				passwordSelector.SetTextInput("", 24, PasswordPlaceholder(), masked: true);
			}
		}

		private void OnLocaleChanged()
		{
			int currentIndex = gameTypeSelector.CurrentIndex;
			gameTypeSelector.SetOptions(GameTypeOptions());
			gameTypeSelector.SetValueWithoutNotify(currentIndex);
			worldNameSelector.SetPlaceholder(WorldNamePlaceholder());
			seedSelector.SetPlaceholder(SeedPlaceholder());
			if (passwordSelector != null)
			{
				passwordSelector.SetPlaceholder(PasswordPlaceholder());
			}
		}

		private List<string> GameTypeOptions()
		{
			return new List<string>
			{
				Localize("@create_game.private", "Private"),
				Localize("@create_game.public", "Public")
			};
		}

		private string WorldNamePlaceholder()
		{
			return Localize("@create_game.placeholder_world_name", "Enter world name");
		}

		private string PasswordPlaceholder()
		{
			return Localize("@create_game.placeholder_password", "Optional");
		}

		private string SeedPlaceholder()
		{
			return Localize("@create_game.placeholder_seed", "Enter seed");
		}

		private static string GenerateSeed()
		{
			return Random.Range(100000, 1000000).ToString();
		}

		private static int ParseSeed(string text)
		{
			if (!int.TryParse(text, out var result) || result <= 0)
			{
				return 0;
			}
			return result;
		}

		private string Localize(string key, string fallback)
		{
			if (_localizationService == null)
			{
				return fallback;
			}
			return _localizationService.Localize(key);
		}

		private void OnCreateClicked()
		{
			string currentText = worldNameSelector.CurrentText;
			if (string.IsNullOrWhiteSpace(currentText))
			{
				worldNameSelector.FocusTextInput();
				return;
			}
			string text = ((passwordSelector != null) ? passwordSelector.CurrentText : null);
			LobbyCreateOptions options = new LobbyCreateOptions
			{
				LobbyName = currentText,
				MaxPlayers = (uint)Mathf.RoundToInt(maxPlayersSelector.RangeValue),
				IsPublic = (gameTypeSelector.CurrentIndex == 1),
				Password = (string.IsNullOrEmpty(text) ? null : text),
				Seed = ParseSeed(seedSelector.CurrentText)
			};
			_gameSaveService?.MarkNewGame(currentText);
			_uiManager.ShowLoadingGame();
			_lobbyManager.CreateGameWithLobby(options);
		}
	}
}
