using System;
using System.Collections.Generic;
using System.Text;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Inputs;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class SettingsControlsTab : MonoBehaviour
	{
		[Header("List")]
		[SerializeField]
		private Transform contentParent;

		[SerializeField]
		private InputRemappingRow rowPrefab;

		[SerializeField]
		private GameObject categoryHeaderPrefab;

		[Header("Category Tabs (optional — empty = show all categories)")]
		[SerializeField]
		private List<Button> categoryTabButtons = new List<Button>();

		[SerializeField]
		private List<TextMeshProUGUI> categoryTabLabels = new List<TextMeshProUGUI>();

		[SerializeField]
		private List<Sprite> categoryBadgeSprites = new List<Sprite>();

		[SerializeField]
		private Color activeCategoryColor = Color.white;

		[SerializeField]
		private Color inactiveCategoryColor = new Color(0.6f, 0.6f, 0.6f, 1f);

		[SerializeField]
		private SoundID categoryChangeSound;

		[Header("Listen Overlay")]
		[SerializeField]
		private GameObject listeningOverlay;

		[SerializeField]
		private TextMeshProUGUI listeningText;

		[SerializeField]
		private Button cancelListenButton;

		[Header("Conflict Confirmation Overlay (reserved for mouse-button confirm)")]
		[SerializeField]
		private GameObject confirmationOverlay;

		[SerializeField]
		private TextMeshProUGUI confirmationText;

		[SerializeField]
		private Button confirmButton;

		[SerializeField]
		private Button revertButton;

		[Header("Block Message Overlay")]
		[SerializeField]
		private GameObject blockMessageOverlay;

		[SerializeField]
		private TextMeshProUGUI blockMessageText;

		[SerializeField]
		private Button blockMessageOkButton;

		[Header("Buttons")]
		[SerializeField]
		private Button resetToDefaultsButton;

		[Header("Sound")]
		[SerializeField]
		private SoundID restoreSound;

		[Inject]
		private IInputRemappingService _remappingService;

		[Inject]
		private IInputGlyphService _glyphService;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private ILocalizationService _localizationService;

		private static readonly int[] CategoryOrder = new int[6] { 1, 2, 3, 4, 5, 6 };

		private bool _initialized;

		private bool _pendingConflict;

		private bool _blockActive;

		private int _activeCategoryId = -1;

		private readonly List<InputRemappingRow> _activeRows = new List<InputRemappingRow>();

		private readonly List<GameObject> _activeHeaders = new List<GameObject>();

		private string Loc(string key, string english)
		{
			if (_localizationService == null)
			{
				return english;
			}
			return _localizationService.Localize(key);
		}

		private void OnEnable()
		{
			if (_initialized)
			{
				Refresh();
			}
		}

		public void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				if (listeningOverlay != null)
				{
					listeningOverlay.SetActive(value: false);
				}
				if (confirmationOverlay != null)
				{
					confirmationOverlay.SetActive(value: false);
				}
				if (blockMessageOverlay != null)
				{
					blockMessageOverlay.SetActive(value: false);
				}
				resetToDefaultsButton?.onClick.AddListener(OnResetClicked);
				cancelListenButton?.onClick.AddListener(OnCancelClicked);
				confirmButton?.onClick.AddListener(OnConfirmClicked);
				revertButton?.onClick.AddListener(OnRevertClicked);
				blockMessageOkButton?.onClick.AddListener(OnBlockMessageOkClicked);
				WireCategoryTabs();
				if (_remappingService != null)
				{
					_remappingService.OnListeningStarted += OnListeningStarted;
					_remappingService.OnListeningStopped += OnListeningStopped;
					_remappingService.OnInputRemapped += OnInputRemapped;
					_remappingService.OnRemapOutcome += OnRemapOutcome;
				}
				if (_glyphService != null)
				{
					_glyphService.OnGlyphsChanged += RefreshRowGlyphs;
				}
				if (_localizationService != null)
				{
					_localizationService.OnLocaleChanged += Refresh;
				}
				Refresh();
			}
		}

		private void OnDestroy()
		{
			resetToDefaultsButton?.onClick.RemoveAllListeners();
			cancelListenButton?.onClick.RemoveAllListeners();
			confirmButton?.onClick.RemoveAllListeners();
			revertButton?.onClick.RemoveAllListeners();
			blockMessageOkButton?.onClick.RemoveAllListeners();
			if (categoryTabButtons != null)
			{
				foreach (Button categoryTabButton in categoryTabButtons)
				{
					categoryTabButton?.onClick.RemoveAllListeners();
				}
			}
			if (_remappingService != null)
			{
				_remappingService.OnListeningStarted -= OnListeningStarted;
				_remappingService.OnListeningStopped -= OnListeningStopped;
				_remappingService.OnInputRemapped -= OnInputRemapped;
				_remappingService.OnRemapOutcome -= OnRemapOutcome;
			}
			if (_glyphService != null)
			{
				_glyphService.OnGlyphsChanged -= RefreshRowGlyphs;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= Refresh;
			}
		}

		private void WireCategoryTabs()
		{
			if (categoryTabButtons == null)
			{
				return;
			}
			for (int i = 0; i < categoryTabButtons.Count && i < CategoryOrder.Length; i++)
			{
				int categoryId = CategoryOrder[i];
				categoryTabButtons[i]?.onClick.AddListener(delegate
				{
					ShowCategory(categoryId);
				});
			}
		}

		private bool HasCategoryTabs()
		{
			if (categoryTabButtons != null)
			{
				return categoryTabButtons.Count > 0;
			}
			return false;
		}

		public void Refresh()
		{
			if (_remappingService == null || contentParent == null || rowPrefab == null)
			{
				return;
			}
			ClearRows();
			Dictionary<int, List<InputRemappingActionData>> allActions = _remappingService.GetAllActions();
			if (HasCategoryTabs())
			{
				EnsureActiveCategory(allActions);
				if (allActions.TryGetValue(_activeCategoryId, out var value))
				{
					BuildRows(_activeCategoryId, value);
				}
				UpdateTabVisuals(allActions);
			}
			else
			{
				foreach (KeyValuePair<int, List<InputRemappingActionData>> item in allActions)
				{
					if (item.Value == null || item.Value.Count == 0)
					{
						continue;
					}
					if (categoryHeaderPrefab != null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(categoryHeaderPrefab, contentParent);
						TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
						if (componentInChildren != null)
						{
							componentInChildren.text = LocalizeCategory(item.Key);
						}
						_activeHeaders.Add(gameObject);
					}
					BuildRows(item.Key, item.Value);
				}
			}
			RecomputeConflictHighlights();
		}

		private void BuildRows(int categoryId, List<InputRemappingActionData> actions)
		{
			if (actions == null || actions.Count == 0)
			{
				return;
			}
			string label = LocalizeCategory(categoryId);
			Sprite categoryBadgeSprite = GetCategoryBadgeSprite(categoryId);
			foreach (InputRemappingActionData action in actions)
			{
				InputRemappingRow inputRemappingRow = UnityEngine.Object.Instantiate(rowPrefab, contentParent);
				inputRemappingRow.Setup(action, OnRebindRequested, ResolveSprite(action.CurrentBindingName), LocalizeAction(action), InputElementNameMap.GetFriendly(action.CurrentBindingName));
				inputRemappingRow.SetContext(label, categoryBadgeSprite);
				inputRemappingRow.SetProtected(_remappingService.IsActionProtected(action.ActionId));
				_activeRows.Add(inputRemappingRow);
			}
		}

		private void EnsureActiveCategory(Dictionary<int, List<InputRemappingActionData>> all)
		{
			if (all.ContainsKey(_activeCategoryId))
			{
				return;
			}
			int[] categoryOrder = CategoryOrder;
			foreach (int num in categoryOrder)
			{
				if (all.ContainsKey(num))
				{
					_activeCategoryId = num;
					break;
				}
			}
		}

		private void ShowCategory(int categoryId)
		{
			if (_activeCategoryId != categoryId)
			{
				_activeCategoryId = categoryId;
				PlaySound(categoryChangeSound);
				Refresh();
			}
		}

		private void UpdateTabVisuals(Dictionary<int, List<InputRemappingActionData>> all)
		{
			if (categoryTabButtons == null)
			{
				return;
			}
			for (int i = 0; i < categoryTabButtons.Count && i < CategoryOrder.Length; i++)
			{
				int num = CategoryOrder[i];
				bool interactable = all?.ContainsKey(num) ?? false;
				if (categoryTabButtons[i] != null)
				{
					categoryTabButtons[i].interactable = interactable;
				}
				if (categoryTabLabels != null && i < categoryTabLabels.Count && categoryTabLabels[i] != null)
				{
					categoryTabLabels[i].text = LocalizeCategory(num);
					categoryTabLabels[i].color = ((num == _activeCategoryId) ? activeCategoryColor : inactiveCategoryColor);
				}
			}
		}

		private Sprite GetCategoryBadgeSprite(int categoryId)
		{
			if (categoryBadgeSprites == null)
			{
				return null;
			}
			int num = Array.IndexOf(CategoryOrder, categoryId);
			if (num < 0 || num >= categoryBadgeSprites.Count)
			{
				return null;
			}
			return categoryBadgeSprites[num];
		}

		private void OnRebindRequested(InputRemappingActionData data)
		{
			if (_remappingService != null)
			{
				_remappingService.StartKeyboardMouseRemapping(data.ActionId, data.AxisRange, data.ActionElementMapId);
			}
		}

		private void OnListeningStarted()
		{
			if (listeningOverlay != null)
			{
				listeningOverlay.SetActive(value: true);
			}
			if (listeningText != null)
			{
				listeningText.text = Loc("@settings.press_key", "Press any key...");
			}
			SetRowsInteractable(interactable: false);
		}

		private void OnListeningStopped()
		{
			if (!_pendingConflict && !_blockActive)
			{
				if (listeningOverlay != null)
				{
					listeningOverlay.SetActive(value: false);
				}
				SetRowsInteractable(interactable: true);
			}
		}

		private void OnInputRemapped(InputRemappingResult result)
		{
			if (listeningOverlay != null)
			{
				listeningOverlay.SetActive(value: false);
			}
			RefreshAllRowData();
			SetRowsInteractable(interactable: true);
			RecomputeConflictHighlights();
		}

		private void OnRemapOutcome(InputRemappingResult result)
		{
			RemapOutcome outcome = result.Outcome;
			if ((uint)(outcome - 1) <= 1u)
			{
				ShowBlockMessage(result);
			}
		}

		private void ShowBlockMessage(InputRemappingResult result)
		{
			if (listeningOverlay != null)
			{
				listeningOverlay.SetActive(value: false);
			}
			string text = (string.IsNullOrEmpty(result.ConflictingActionName) ? ((_localizationService != null) ? _localizationService.Localize("@settings.protected_action", result.ActionName) : ("\"" + result.ActionName + "\" cannot be rebound.")) : ((_localizationService != null) ? _localizationService.Localize("@settings.conflict", result.NewElementName, result.ConflictingActionName) : ("\"" + result.NewElementName + "\" is already assigned to \"" + result.ConflictingActionName + "\".")));
			if (blockMessageText != null)
			{
				blockMessageText.text = text;
			}
			if (blockMessageOverlay != null)
			{
				blockMessageOverlay.SetActive(value: true);
			}
			_blockActive = true;
			SetRowsInteractable(interactable: false);
		}

		private void OnBlockMessageOkClicked()
		{
			_blockActive = false;
			if (blockMessageOverlay != null)
			{
				blockMessageOverlay.SetActive(value: false);
			}
			RefreshAllRowData();
			SetRowsInteractable(interactable: true);
			RecomputeConflictHighlights();
		}

		private void OnCancelClicked()
		{
			_remappingService?.CancelListening();
		}

		private void OnConfirmClicked()
		{
			_pendingConflict = false;
			if (confirmationOverlay != null)
			{
				confirmationOverlay.SetActive(value: false);
			}
			_remappingService?.SaveBindings();
			SetRowsInteractable(interactable: true);
			RecomputeConflictHighlights();
		}

		private void OnRevertClicked()
		{
			_pendingConflict = false;
			if (confirmationOverlay != null)
			{
				confirmationOverlay.SetActive(value: false);
			}
			_remappingService?.RevertLastRemap();
			Refresh();
			SetRowsInteractable(interactable: true);
		}

		private void OnResetClicked()
		{
			PlaySound(restoreSound);
			_remappingService?.ResetToDefaults();
			Refresh();
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}

		private void RefreshAllRowData()
		{
			if (_remappingService == null)
			{
				return;
			}
			Dictionary<int, List<InputRemappingActionData>> allActions = _remappingService.GetAllActions();
			Dictionary<(int, int), InputRemappingActionData> dictionary = new Dictionary<(int, int), InputRemappingActionData>();
			Dictionary<int, InputRemappingActionData> dictionary2 = new Dictionary<int, InputRemappingActionData>();
			foreach (KeyValuePair<int, List<InputRemappingActionData>> item in allActions)
			{
				foreach (InputRemappingActionData item2 in item.Value)
				{
					dictionary[(item2.ActionId, (int)item2.AxisRange)] = item2;
					if (!dictionary2.ContainsKey(item2.ActionId))
					{
						dictionary2[item2.ActionId] = item2;
					}
				}
			}
			foreach (InputRemappingRow activeRow in _activeRows)
			{
				if (!(activeRow == null) && activeRow.Data != null)
				{
					if (!dictionary.TryGetValue((activeRow.ActionId, (int)activeRow.Data.AxisRange), out var value))
					{
						dictionary2.TryGetValue(activeRow.ActionId, out value);
					}
					if (value != null)
					{
						activeRow.UpdateBinding(value, ResolveSprite(value.CurrentBindingName), LocalizeAction(value), InputElementNameMap.GetFriendly(value.CurrentBindingName));
					}
				}
			}
		}

		private string LocalizeCategory(int categoryId)
		{
			string categoryDisplayName = _remappingService.GetCategoryDisplayName(categoryId);
			if (_localizationService == null)
			{
				return categoryDisplayName;
			}
			string key = "@controls.category_" + NormalizeKey(categoryDisplayName);
			if (!_localizationService.HasKey(key))
			{
				return categoryDisplayName;
			}
			return _localizationService.Localize(key);
		}

		private string LocalizeAction(InputRemappingActionData data)
		{
			string result = (string.IsNullOrEmpty(data.ActionDescriptiveName) ? data.ActionName : data.ActionDescriptiveName);
			if (_localizationService == null || string.IsNullOrEmpty(data.ActionName))
			{
				return result;
			}
			string key = "@controls.action_" + NormalizeKey(data.ActionName);
			if (!_localizationService.HasKey(key))
			{
				return result;
			}
			return _localizationService.Localize(key);
		}

		private static string NormalizeKey(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(name.Length + 4);
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				if (c == ' ')
				{
					stringBuilder.Append('_');
					continue;
				}
				if (char.IsUpper(c) && i > 0 && name[i - 1] != ' ' && !char.IsUpper(name[i - 1]))
				{
					stringBuilder.Append('_');
				}
				stringBuilder.Append(char.ToLowerInvariant(c));
			}
			return stringBuilder.ToString();
		}

		private Sprite ResolveSprite(string elementName)
		{
			if (_glyphService == null)
			{
				return null;
			}
			return _glyphService.GetSpriteForElement(elementName);
		}

		private void RefreshRowGlyphs()
		{
			foreach (InputRemappingRow activeRow in _activeRows)
			{
				activeRow?.SetBindingSprite(ResolveSprite(activeRow.CurrentBindingName));
			}
		}

		private void RecomputeConflictHighlights()
		{
			Dictionary<int, List<InputRemappingRow>> dictionary = new Dictionary<int, List<InputRemappingRow>>();
			foreach (InputRemappingRow activeRow in _activeRows)
			{
				if (!(activeRow == null))
				{
					if (!dictionary.TryGetValue(activeRow.MapCategoryId, out var value))
					{
						value = new List<InputRemappingRow>();
						dictionary[activeRow.MapCategoryId] = value;
					}
					value.Add(activeRow);
				}
			}
			foreach (List<InputRemappingRow> value4 in dictionary.Values)
			{
				Dictionary<string, HashSet<int>> dictionary2 = new Dictionary<string, HashSet<int>>();
				foreach (InputRemappingRow item in value4)
				{
					string currentBindingName = item.CurrentBindingName;
					if (!string.IsNullOrEmpty(currentBindingName))
					{
						if (!dictionary2.TryGetValue(currentBindingName, out var value2))
						{
							value2 = (dictionary2[currentBindingName] = new HashSet<int>());
						}
						value2.Add(item.ActionId);
					}
				}
				foreach (InputRemappingRow item2 in value4)
				{
					string currentBindingName2 = item2.CurrentBindingName;
					HashSet<int> value3;
					bool conflictHighlight = !string.IsNullOrEmpty(currentBindingName2) && dictionary2.TryGetValue(currentBindingName2, out value3) && value3.Count >= 2;
					item2.SetConflictHighlight(conflictHighlight);
				}
			}
		}

		private void SetRowsInteractable(bool interactable)
		{
			foreach (InputRemappingRow activeRow in _activeRows)
			{
				activeRow?.SetInteractable(interactable);
			}
			if (resetToDefaultsButton != null)
			{
				resetToDefaultsButton.interactable = interactable;
			}
		}

		private void ClearRows()
		{
			foreach (InputRemappingRow activeRow in _activeRows)
			{
				if (activeRow != null)
				{
					UnityEngine.Object.Destroy(activeRow.gameObject);
				}
			}
			_activeRows.Clear();
			foreach (GameObject activeHeader in _activeHeaders)
			{
				if (activeHeader != null)
				{
					UnityEngine.Object.Destroy(activeHeader);
				}
			}
			_activeHeaders.Clear();
		}
	}
}
