using EvilCore.Extensions;
using EvilCore.Localization;
using EvilCore.Networking;
using EvilCore.UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class SinglePlayerPanel : MonoBehaviour
	{
		[Header("Selectors")]
		[SerializeField]
		private OptionSelector worldNameSelector;

		[SerializeField]
		private OptionSelector seedSelector;

		[Header("Actions")]
		[SerializeField]
		private Button startButton;

		[SerializeField]
		private Button backButton;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private ILocalizationService _localizationService;

		private const int WorldNameMaxLength = 24;

		private const string DefaultWorldName = "Nomad Drive Trip";

		private const int SeedDigits = 6;

		private const int SeedMin = 100000;

		private const int SeedMaxExclusive = 1000000;

		private void Start()
		{
			base.gameObject.InjectGameObject();
			ConfigureSelectors();
			startButton.onClick.AddListener(OnStartClicked);
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
			startButton?.onClick.RemoveAllListeners();
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
			worldNameSelector.SetTextInput("Nomad Drive Trip", 24, WorldNamePlaceholder());
			seedSelector.SetTextInput(GenerateSeed(), 6, SeedPlaceholder(), masked: false, numeric: true);
		}

		private void OnLocaleChanged()
		{
			worldNameSelector.SetPlaceholder(WorldNamePlaceholder());
			seedSelector.SetPlaceholder(SeedPlaceholder());
		}

		private string WorldNamePlaceholder()
		{
			return Localize("@create_game.placeholder_world_name", "Enter world name");
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

		private void OnStartClicked()
		{
			string currentText = worldNameSelector.CurrentText;
			if (string.IsNullOrWhiteSpace(currentText))
			{
				worldNameSelector.FocusTextInput();
				return;
			}
			_uiManager.ShowLoadingGame();
			_networkManager.StartSinglePlayerHost(currentText, ParseSeed(seedSelector.CurrentText));
		}
	}
}
