using System.Collections.Generic;
using EvilCore.Inputs;
using EvilCore.Inputs.UI;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.UI
{
	public class DrivingActionsPanel : GameCanvasGroup
	{
		[Header("Driving Actions")]
		[Tooltip("Row prefab instantiated per action (icon + label). The shared InputActionPromptUI prefab works.")]
		[SerializeField]
		private InputActionPromptUI rowPrefab;

		[Tooltip("Parent the rows are spawned under. Falls back to this transform when unset — assign a dedicated child so other panel art is never cleared.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Alpha applied to a row whose control is currently unavailable (greyed look).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float disabledAlpha = 0.4f;

		[Inject]
		private IInputGlyphService _inputGlyphService;

		[Inject]
		private InputActionPromptsDatabase _inputActionPromptsDatabase;

		[Inject]
		private ILocalizationService _localizationService;

		private readonly List<DrivingActionEntry> _entries = new List<DrivingActionEntry>();

		private Transform Container
		{
			get
			{
				if (!(rowContainer != null))
				{
					return base.transform;
				}
				return rowContainer;
			}
		}

		private void OnEnable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged += Render;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += Render;
			}
		}

		private void OnDisable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged -= Render;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= Render;
			}
		}

		public void SetActions(IReadOnlyList<DrivingActionEntry> entries)
		{
			_entries.Clear();
			if (entries != null)
			{
				_entries.AddRange(entries);
			}
			Render();
		}

		private void Render()
		{
			if (rowPrefab == null || _inputActionPromptsDatabase == null || _inputGlyphService == null)
			{
				return;
			}
			ClearRows();
			foreach (DrivingActionEntry entry in _entries)
			{
				InputActionPrompt primaryActionData = _inputActionPromptsDatabase.GetPrimaryActionData(entry.InputId);
				if (primaryActionData != null)
				{
					Sprite spriteForAction = _inputGlyphService.GetSpriteForAction(primaryActionData.rewiredActionName);
					string actionString = ((_localizationService != null) ? _localizationService.Localize(primaryActionData.actionName) : primaryActionData.actionName);
					InputActionPromptUI inputActionPromptUI = Object.Instantiate(rowPrefab, Container);
					inputActionPromptUI.Initialize(spriteForAction, actionString);
					CanvasGroup canvasGroup = inputActionPromptUI.GetComponent<CanvasGroup>();
					if (canvasGroup == null)
					{
						canvasGroup = inputActionPromptUI.gameObject.AddComponent<CanvasGroup>();
					}
					canvasGroup.alpha = (entry.Enabled ? 1f : disabledAlpha);
				}
			}
		}

		private void ClearRows()
		{
			Transform container = Container;
			for (int num = container.childCount - 1; num >= 0; num--)
			{
				Object.Destroy(container.GetChild(num).gameObject);
			}
		}

		public override void Hide()
		{
			base.Hide();
			ClearRows();
			_entries.Clear();
		}
	}
}
