using System;
using Features.InputModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.AudioDevicesModule.Scripts.PushToTalk.Views
{
	[PublicAPI]
	public class PushToTalkHintPresenter : PresenterBehaviour<PushToTalkHintViewBase>
	{
		private readonly MicrophoneModel _microphoneModel;

		private readonly IInputService _inputService;

		private readonly LocalInputActions _localInputActions;

		private readonly ILocalizationService _localizationService;

		private readonly ILanguageService _languageService;

		private readonly PushToTalkViewModel _pushToTalkViewModel;

		private bool _visible = true;

		public PushToTalkHintPresenter(MicrophoneModel microphoneModel, IInputService inputService, LocalInputActions localInputActions, ILocalizationService localizationService, ILanguageService languageService, PushToTalkViewModel pushToTalkViewModel)
		{
			_microphoneModel = microphoneModel;
			_inputService = inputService;
			_localInputActions = localInputActions;
			_localizationService = localizationService;
			_languageService = languageService;
			_pushToTalkViewModel = pushToTalkViewModel;
		}

		protected override void OnViewSet()
		{
			_microphoneModel.OnPushToTalkEnabledChanged += OnMicrophoneSettingsChanged;
			_microphoneModel.OnMicrophoneEnabledChanged += OnMicrophoneSettingsChanged;
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Combine(pushToTalk.Started, new Action(OnPushToTalkStarted));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Canceled = (Action)Delegate.Combine(pushToTalk2.Canceled, new Action(OnPushToTalkCanceled));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(OnLanguageChanged));
			_pushToTalkViewModel.PushToTalkVisibilityChanged += ChangeVisibility;
			Refresh();
		}

		protected override void OnDisposed()
		{
			_microphoneModel.OnPushToTalkEnabledChanged -= OnMicrophoneSettingsChanged;
			_microphoneModel.OnMicrophoneEnabledChanged -= OnMicrophoneSettingsChanged;
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Remove(pushToTalk.Started, new Action(OnPushToTalkStarted));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Canceled = (Action)Delegate.Remove(pushToTalk2.Canceled, new Action(OnPushToTalkCanceled));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(OnLanguageChanged));
			_pushToTalkViewModel.PushToTalkVisibilityChanged -= ChangeVisibility;
		}

		private void ChangeVisibility(bool visible)
		{
			if (!base.View.IsVisibleOnDeadState)
			{
				_visible = visible;
				Refresh();
			}
		}

		private void OnMicrophoneSettingsChanged()
		{
			Refresh();
		}

		private void OnLanguageChanged()
		{
			if (ShouldShowHint())
			{
				RefreshHintText();
			}
		}

		private void OnPushToTalkStarted()
		{
			if (ShouldShowHint())
			{
				base.View.SetPressedVisual(isPressed: true);
			}
		}

		private void OnPushToTalkCanceled()
		{
			if (ShouldShowHint())
			{
				base.View.SetPressedVisual(isPressed: false);
			}
		}

		private void Refresh()
		{
			bool flag = ShouldShowHint() && _visible;
			base.View.SetVisible(flag);
			if (flag)
			{
				RefreshHintText();
				base.View.SetPressedVisual(_localInputActions.Voice.PushToTalk.IsPressed());
			}
		}

		private bool ShouldShowHint()
		{
			if (_microphoneModel.IsPushToTalkEnabled)
			{
				return _microphoneModel.IsMicrophoneEnabled;
			}
			return false;
		}

		private void RefreshHintText()
		{
			string bindingDisplayString = _localInputActions.Voice.PushToTalk.GetBindingDisplayString();
			base.View.SetHintText(bindingDisplayString);
		}

		public void SetParent(Transform parent, bool worlPositionStays = false)
		{
			base.View.transform.SetParent(parent, worlPositionStays);
		}
	}
}
