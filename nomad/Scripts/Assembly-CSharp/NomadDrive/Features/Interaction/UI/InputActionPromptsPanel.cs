using System.Collections.Generic;
using EvilCore.Inputs;
using EvilCore.Inputs.UI;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Interaction.UI
{
	public class InputActionPromptsPanel : MonoBehaviour
	{
		[SerializeField]
		private InputActionPromptUI inputActionPromptUIPrefab;

		private InputActionPromptsDatabase _inputActionPromptsDatabase;

		private IGameUIManager _guiManager;

		private IInputGlyphService _inputGlyphService;

		private ILocalizationService _localizationService;

		private readonly List<string> _activeBasePrompts = new List<string>();

		private readonly HashSet<string> _temporaryPrompts = new HashSet<string>();

		private readonly HashSet<string> _suppressedPrompts = new HashSet<string>();

		private string L(string text)
		{
			return _localizationService?.Localize(text) ?? text;
		}

		[Inject]
		private void Construct(InputActionPromptsDatabase inputActionPromptsDatabase, IGameUIManager guiManager, IInputGlyphService inputGlyphService, ILocalizationService localizationService)
		{
			_inputActionPromptsDatabase = inputActionPromptsDatabase;
			_guiManager = guiManager;
			_inputGlyphService = inputGlyphService;
			_localizationService = localizationService;
		}

		private void OnEnable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged += RebuildPrompts;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += RebuildPrompts;
			}
		}

		private void OnDisable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged -= RebuildPrompts;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RebuildPrompts;
			}
		}

		private void Show()
		{
			_guiManager.ShowCanvasGroup(GameCanvasGroupName.InputActionPrompts, interactable: false, blockRaycast: false);
		}

		public void Hide()
		{
			_activeBasePrompts.Clear();
			_temporaryPrompts.Clear();
			_suppressedPrompts.Clear();
			ClearInputActionPrompts();
			_guiManager.HideCanvasGroup(GameCanvasGroupName.InputActionPrompts);
		}

		public void AddTemporaryPrompt(string promptName)
		{
			if (_temporaryPrompts.Add(promptName))
			{
				RebuildPrompts();
			}
		}

		public void RemoveTemporaryPrompt(string promptName)
		{
			if (_temporaryPrompts.Remove(promptName))
			{
				RebuildPrompts();
			}
		}

		public void SuppressPrompt(string promptName)
		{
			if (!string.IsNullOrEmpty(promptName) && _suppressedPrompts.Add(promptName))
			{
				RebuildPrompts();
			}
		}

		public void UnsuppressPrompt(string promptName)
		{
			if (!string.IsNullOrEmpty(promptName) && _suppressedPrompts.Remove(promptName))
			{
				RebuildPrompts();
			}
		}

		private void RebuildPrompts()
		{
			ClearInputActionPrompts();
			foreach (string temporaryPrompt in _temporaryPrompts)
			{
				if (!_suppressedPrompts.Contains(temporaryPrompt))
				{
					CreateInputActionPrompt(temporaryPrompt);
				}
			}
			foreach (string activeBasePrompt in _activeBasePrompts)
			{
				if (!_suppressedPrompts.Contains(activeBasePrompt))
				{
					CreateInputActionPrompt(activeBasePrompt);
				}
			}
		}

		private void CreateInputActionPrompt(string actionPromptName)
		{
			InputActionPrompt primaryActionData = _inputActionPromptsDatabase.GetPrimaryActionData(actionPromptName);
			if (primaryActionData != null)
			{
				Sprite spriteForAction = _inputGlyphService.GetSpriteForAction(primaryActionData.rewiredActionName);
				Object.Instantiate(inputActionPromptUIPrefab, base.transform).Initialize(spriteForAction, L(primaryActionData.actionName));
			}
		}

		private void ClearInputActionPrompts()
		{
			foreach (Transform item in base.transform)
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void ActivateWithPrompts(IEnumerable<string> promptNames)
		{
			_activeBasePrompts.Clear();
			_temporaryPrompts.Clear();
			_suppressedPrompts.Clear();
			_activeBasePrompts.AddRange(promptNames);
			RebuildPrompts();
			Show();
		}
	}
}
