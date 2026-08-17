using EvilCore.Inputs;
using EvilCore.Inputs.UI;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Inputs;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Player.UI
{
	public class StandupActionPromptPanel : GameCanvasGroup
	{
		private const string StandupInputId = "Standup";

		[SerializeField]
		private Image _standupGlyphImage;

		[SerializeField]
		private TextMeshProUGUI _actionText;

		[SerializeField]
		private Image _standupFillImage;

		[SerializeField]
		private float _fillingDuration = 0.4f;

		private bool _isActive;

		private bool _standBlockedThisHold;

		private Sequence _imageFillSequence;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IInputGlyphService _inputGlyphService;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private InputActionPromptsDatabase _inputActionPromptsDatabase;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		private void OnEnable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged += RefreshGlyph;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += RefreshGlyph;
			}
		}

		private void OnDisable()
		{
			if (_inputGlyphService != null)
			{
				_inputGlyphService.OnGlyphsChanged -= RefreshGlyph;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RefreshGlyph;
			}
		}

		private void Update()
		{
			if (SittingInputs.IsStandupButton())
			{
				if (_isActive && !_standBlockedThisHold && !_imageFillSequence.isAlive)
				{
					_imageFillSequence = Sequence.Create().Chain(Tween.UIFillAmount(_standupFillImage, 1f, _fillingDuration, Ease.Linear)).ChainCallback(ActionComplete);
				}
				return;
			}
			_standBlockedThisHold = false;
			if (_imageFillSequence.isAlive)
			{
				_imageFillSequence.Stop();
				_standupFillImage.fillAmount = 0f;
			}
		}

		private void ActionComplete()
		{
			if (!_playerService.IsPlayerSpawned)
			{
				Hide();
			}
			else if (!_playerService.LocalPlayer.CanStandUpFromSeat())
			{
				_standBlockedThisHold = true;
				_standupFillImage.fillAmount = 0f;
				_uiFeedbackManager?.CreateFloatingMessage("@vehicle.too_fast_to_stand_up", FeedbackType.Warning);
			}
			else if (!_playerService.LocalPlayer.CanExitThroughAssociatedDoor())
			{
				_standBlockedThisHold = true;
				_standupFillImage.fillAmount = 0f;
				_uiFeedbackManager?.CreateFloatingMessage("@vehicle.exit_door_blocked", FeedbackType.Warning);
			}
			else
			{
				Hide();
				_playerService.LocalPlayer.OnPlayerStand.Invoke();
			}
		}

		public override void Show(bool interactable, bool blockRaycast)
		{
			base.Show(interactable, blockRaycast);
			if (!_isActive)
			{
				_isActive = true;
				_imageFillSequence.Stop();
				RefreshGlyph();
			}
		}

		private void RefreshGlyph()
		{
			if (_inputGlyphService == null || _standupGlyphImage == null || _inputActionPromptsDatabase == null)
			{
				return;
			}
			InputActionPrompt primaryActionData = _inputActionPromptsDatabase.GetPrimaryActionData("Standup");
			if (primaryActionData != null)
			{
				_standupGlyphImage.sprite = _inputGlyphService.GetSpriteForAction(primaryActionData.rewiredActionName);
				if (_actionText != null)
				{
					_actionText.text = ((_localizationService != null) ? _localizationService.Localize(primaryActionData.actionName) : primaryActionData.actionName);
				}
			}
		}

		public override void Hide()
		{
			base.Hide();
			_isActive = false;
			_imageFillSequence.Stop();
			_standupFillImage.fillAmount = 0f;
		}
	}
}
