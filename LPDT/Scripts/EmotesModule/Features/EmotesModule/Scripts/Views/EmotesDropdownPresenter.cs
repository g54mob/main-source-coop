using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Global.Modules.Localization_Module.Scripts;
using Global.SerializableDictionary;
using JetBrains.Annotations;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.EmotesModule.Scripts.Views
{
	[PublicAPI]
	public class EmotesDropdownPresenter : PresenterBehaviour<EmotesDropdownViewBase>, IBackButtonProcessor
	{
		private static readonly int _isUsed = Animator.StringToHash("IsUsed");

		private static readonly EmoteGroup[] _navigableGroups = new EmoteGroup[3]
		{
			EmoteGroup.Body,
			EmoteGroup.Face,
			EmoteGroup.Hand
		};

		private const int NO_NAVIGATION_SELECTION = -1;

		private readonly IEmoteByGroupTriggerService _emoteByGroupTriggerService;

		private readonly EmotesTriggerModel _emotesTriggerModel;

		private readonly ILocalizationService _localizationService;

		private readonly EmotesMapConfig _emotesMapConfig;

		private readonly EmoteAvailabilityModel _emoteAvailabilityModel;

		private readonly IInputService _inputService;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly CameraModel _cameraModel;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly IInputDeviceService _inputDeviceService;

		private Tween _groupSelectionTween;

		private Coroutine _activeCoroutine;

		private Coroutine _autoHideCoroutine;

		private EmoteGroup _currentEmoteGroup;

		private int _navigationSelectionIndex = -1;

		private int _activeEmotesCount;

		private Vector2Int _lastNavigationDirection;

		public BackButtonProcessorType Type => BackButtonProcessorType.UIElement;

		public EmotesDropdownPresenter(EmotesTriggerModel emotesTriggerModel, IEmoteByGroupTriggerService emoteByGroupTriggerService, EmotesMapConfig emotesMapConfig, ILocalizationService localizationService, EmoteAvailabilityModel emoteAvailabilityModel, IInputService inputService, PlayerMovableModel playerMovableModel, IUIBackButtonRegistrationService backButtonRegistrationService, CameraModel cameraModel, IInputDeviceService inputDeviceService)
		{
			_emotesTriggerModel = emotesTriggerModel;
			_emoteByGroupTriggerService = emoteByGroupTriggerService;
			_emotesMapConfig = emotesMapConfig;
			_localizationService = localizationService;
			_emoteAvailabilityModel = emoteAvailabilityModel;
			_inputService = inputService;
			_playerMovableModel = playerMovableModel;
			_backButtonRegistrationService = backButtonRegistrationService;
			_cameraModel = cameraModel;
			_inputDeviceService = inputDeviceService;
		}

		public bool CanHandleBack()
		{
			return _currentEmoteGroup != EmoteGroup.None;
		}

		public void OnBack()
		{
			StartHideView();
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			base.View.CanvasGroup.alpha = 0f;
			ProcessEmotesHotbarList();
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_emotesTriggerModel.OnEmoteGroupViewCalled += StartShowView;
			_emotesTriggerModel.OnHotBarActivated += TriggerEmote;
			_emoteAvailabilityModel.OnEmoteActiveChanged += OnEmoteActiveChanged;
			InputVector2Actions emoteNavigation = _inputService.EmoteNavigation;
			emoteNavigation.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(emoteNavigation.VectorChangedPerformed, new Action<Vector2>(OnEmoteNavigationPerformed));
			InputVector2Actions emoteNavigation2 = _inputService.EmoteNavigation;
			emoteNavigation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(emoteNavigation2.VectorChangedCanceled, new Action<Vector2>(OnEmoteNavigationCanceled));
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Performed = (Action)Delegate.Combine(uIApply.Performed, new Action(OnUIApply));
			_backButtonRegistrationService.Register(this);
			_inputDeviceService.OnCurrentActiveDeviceChange += OnCurrentActiveDeviceChange;
			ApplyInputDeviceHints();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_emotesTriggerModel.OnEmoteGroupViewCalled -= StartShowView;
			_emotesTriggerModel.OnHotBarActivated -= TriggerEmote;
			_emoteAvailabilityModel.OnEmoteActiveChanged -= OnEmoteActiveChanged;
			InputVector2Actions emoteNavigation = _inputService.EmoteNavigation;
			emoteNavigation.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(emoteNavigation.VectorChangedPerformed, new Action<Vector2>(OnEmoteNavigationPerformed));
			InputVector2Actions emoteNavigation2 = _inputService.EmoteNavigation;
			emoteNavigation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(emoteNavigation2.VectorChangedCanceled, new Action<Vector2>(OnEmoteNavigationCanceled));
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Performed = (Action)Delegate.Remove(uIApply.Performed, new Action(OnUIApply));
			_backButtonRegistrationService.Unregister(this);
			_inputDeviceService.OnCurrentActiveDeviceChange -= OnCurrentActiveDeviceChange;
			SetCurrentEmoteGroup(EmoteGroup.None);
			ResetNavigationSelection();
		}

		private void SetCurrentEmoteGroup(EmoteGroup emoteGroup)
		{
			_currentEmoteGroup = emoteGroup;
			if (emoteGroup == EmoteGroup.None)
			{
				_cameraModel.RemoveZoomInputDisableReason(ZoomInputDisableReasonEnum.EmoteDropdown);
			}
			else
			{
				_cameraModel.AddZoomInputDisableReason(ZoomInputDisableReasonEnum.EmoteDropdown);
			}
		}

		private void OnEmoteActiveChanged(bool isActive)
		{
			if (!isActive)
			{
				ForceHideView();
			}
			base.View.EmotesHotBarContainer.SetActive(isActive);
			ApplyInputDeviceHints();
		}

		private void ForceHideView()
		{
			if (_activeCoroutine != null)
			{
				base.View.StopCoroutine(_activeCoroutine);
				_activeCoroutine = null;
			}
			if (_autoHideCoroutine != null)
			{
				base.View.StopCoroutine(_autoHideCoroutine);
				_autoHideCoroutine = null;
			}
			_groupSelectionTween?.Kill();
			DeselectAllHotbar();
			base.View.CanvasGroup.alpha = 0f;
			SetCurrentEmoteGroup(EmoteGroup.None);
			ResetNavigationSelection();
		}

		private void StartHideView()
		{
			if (_activeCoroutine != null)
			{
				base.View.StopCoroutine(_activeCoroutine);
				_activeCoroutine = null;
			}
			if (_autoHideCoroutine != null)
			{
				base.View.StopCoroutine(_autoHideCoroutine);
				_autoHideCoroutine = null;
			}
			_groupSelectionTween?.Kill();
			SetCurrentEmoteGroup(EmoteGroup.None);
			ResetNavigationSelection();
			_activeCoroutine = base.View.StartCoroutine(HideViewCoroutine());
		}

		private IEnumerator HideViewCoroutine()
		{
			DeselectAllHotbar();
			yield return FadeCoroutine(base.View.CanvasGroup.alpha, 0f);
			_activeCoroutine = null;
		}

		private void StartShowView(EmoteGroup emoteGroup)
		{
			if (_emoteAvailabilityModel.IsEmoteActive)
			{
				if (_activeCoroutine != null)
				{
					base.View.StopCoroutine(_activeCoroutine);
				}
				if (_autoHideCoroutine != null)
				{
					base.View.StopCoroutine(_autoHideCoroutine);
					_autoHideCoroutine = null;
				}
				_activeCoroutine = base.View.StartCoroutine(ShowViewCoroutine(emoteGroup));
			}
		}

		private IEnumerator ShowViewCoroutine(EmoteGroup emoteGroup)
		{
			DeselectAllHotbar();
			if (_currentEmoteGroup != EmoteGroup.None)
			{
				yield return FadeCoroutine(base.View.CanvasGroup.alpha, 0f);
			}
			base.View.EmotesGridLayoutGroup.spacing = base.View.EmotesGridLayoutGroupAnimationSpacing;
			_groupSelectionTween?.Kill();
			_groupSelectionTween = DOTween.Sequence();
			_groupSelectionTween = DOTween.To(() => base.View.EmotesGridLayoutGroup.spacing, delegate(Vector2 value)
			{
				base.View.EmotesGridLayoutGroup.spacing = value;
			}, Vector2.zero, base.View.EmotesGridLayoutGroupAnimationDuration).SetEase(Ease.OutBack);
			switch (emoteGroup)
			{
			case EmoteGroup.Body:
				base.View.EmotesGridLayoutGroup.constraintCount = 1;
				ProcessEmotesList(_emotesMapConfig.BodyEmotesMap);
				break;
			case EmoteGroup.Face:
				base.View.EmotesGridLayoutGroup.constraintCount = 2;
				ProcessEmotesList(_emotesMapConfig.FaceEmotesMap);
				break;
			case EmoteGroup.Hand:
				base.View.EmotesGridLayoutGroup.constraintCount = 1;
				ProcessEmotesList(_emotesMapConfig.HandEmotesMap);
				break;
			default:
				throw new ArgumentOutOfRangeException("emoteGroup", emoteGroup, null);
			}
			RefreshNavigationHighlight();
			if (_currentEmoteGroup != emoteGroup)
			{
				SetCurrentEmoteGroup(emoteGroup);
				EmoteHotbarItemViewBase emoteHotbarItemViewBase = base.View.EmotesHotbarList.FirstOrDefault((EmoteHotbarItemViewBase itemViewBase) => itemViewBase.EmoteGroup == emoteGroup);
				if (emoteHotbarItemViewBase != null)
				{
					emoteHotbarItemViewBase.Animator.SetBool(_isUsed, value: true);
				}
				yield return FadeCoroutine(base.View.CanvasGroup.alpha, 1f);
				_autoHideCoroutine = base.View.StartCoroutine(AutoHideCoroutine());
			}
			else
			{
				SetCurrentEmoteGroup(EmoteGroup.None);
				ResetNavigationSelection();
			}
			_activeCoroutine = null;
		}

		private void DeselectAllHotbar()
		{
			foreach (EmoteHotbarItemViewBase emotesHotbar in base.View.EmotesHotbarList)
			{
				emotesHotbar.Animator.SetBool(_isUsed, value: false);
			}
		}

		private IEnumerator AutoHideCoroutine()
		{
			yield return new WaitForSeconds(base.View.AutoHideDelay);
			DeselectAllHotbar();
			yield return FadeCoroutine(base.View.CanvasGroup.alpha, 0f);
			SetCurrentEmoteGroup(EmoteGroup.None);
			ResetNavigationSelection();
			_autoHideCoroutine = null;
		}

		private IEnumerator FadeCoroutine(float from, float to)
		{
			float elapsed = 0f;
			base.View.CanvasGroup.alpha = from;
			while (elapsed < base.View.FadeDuration)
			{
				elapsed += Time.deltaTime;
				base.View.CanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / base.View.FadeDuration);
				yield return null;
			}
			base.View.CanvasGroup.alpha = to;
		}

		private void TriggerEmote(int index)
		{
			if (_currentEmoteGroup == EmoteGroup.None)
			{
				return;
			}
			if (_activeCoroutine == null)
			{
				if (_autoHideCoroutine != null)
				{
					base.View.StopCoroutine(_autoHideCoroutine);
				}
				if (base.View.CanvasGroup.alpha < 1f)
				{
					base.View.CanvasGroup.alpha = 1f;
				}
				_autoHideCoroutine = base.View.StartCoroutine(AutoHideCoroutine());
			}
			foreach (EmoteListItemViewBase emotes in base.View.EmotesList)
			{
				emotes.Animator.SetBool(_isUsed, value: false);
				if (emotes.EmoteIndex == index)
				{
					emotes.Animator.SetBool(_isUsed, value: true);
				}
			}
			_emoteByGroupTriggerService.TriggerEmoteByGroup(_currentEmoteGroup, index);
		}

		private void ProcessEmotesList<TEnum>(SerializableDictionary<int, EmotesViewData<TEnum>> emotesMap) where TEnum : Enum
		{
			_activeEmotesCount = Mathf.Min(base.View.EmotesList.Count, emotesMap.Count);
			for (int i = 0; i < base.View.EmotesList.Count; i++)
			{
				EmoteListItemViewBase emoteListItemViewBase = base.View.EmotesList[i];
				if (i >= emotesMap.Count)
				{
					emoteListItemViewBase.gameObject.SetActive(value: false);
					continue;
				}
				emoteListItemViewBase.gameObject.SetActive(value: true);
				KeyValuePair<int, EmotesViewData<TEnum>> keyValuePair = emotesMap.ElementAt(i);
				emoteListItemViewBase.EmoteIndex = keyValuePair.Key;
				emoteListItemViewBase.KeyText.SetText($"{emoteListItemViewBase.EmoteIndex}");
				emoteListItemViewBase.EmoteImage.sprite = keyValuePair.Value.EmoteSprite;
				emoteListItemViewBase.Animator.SetBool(_isUsed, value: false);
			}
			foreach (RectTransform rectTransformToRebuild in base.View.RectTransformToRebuilds)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformToRebuild);
			}
		}

		private void ProcessEmotesHotbarList()
		{
			foreach (EmoteHotbarItemViewBase emotesHotbar in base.View.EmotesHotbarList)
			{
				emotesHotbar.EmoteImage.sprite = _emotesMapConfig.HotbarEmotesMap[emotesHotbar.EmoteGroup].EmoteSprite;
				emotesHotbar.KeyText.SetText(_emotesMapConfig.HotbarEmotesMap[emotesHotbar.EmoteGroup].hotKeyText);
			}
		}

		private void ApplyInputDeviceHints()
		{
			bool flag = IsGamepadMode();
			base.View.GamepadTip.SetActive(flag && _emoteAvailabilityModel.IsEmoteActive);
			foreach (EmoteHotbarItemViewBase emotesHotbar in base.View.EmotesHotbarList)
			{
				emotesHotbar.KeyText.gameObject.SetActive(!flag);
			}
		}

		private bool IsGamepadMode()
		{
			if (!_inputDeviceService.IsCurrentActiveDeviceGamepad())
			{
				return _inputDeviceService.IsCurrentActiveDeviceJoystick();
			}
			return true;
		}

		private void SwitchNavigationGroup(int step)
		{
			int num = Array.IndexOf(_navigableGroups, _currentEmoteGroup);
			if (num >= 0)
			{
				int num2 = (num + step + _navigableGroups.Length) % _navigableGroups.Length;
				StartShowView(_navigableGroups[num2]);
			}
		}

		private void MoveNavigationSelection(int step)
		{
			if (_activeEmotesCount > 0)
			{
				if (_navigationSelectionIndex == -1)
				{
					_navigationSelectionIndex = ((step <= 0) ? (_activeEmotesCount - 1) : 0);
				}
				else
				{
					_navigationSelectionIndex = (_navigationSelectionIndex + step + _activeEmotesCount) % _activeEmotesCount;
				}
				_playerMovableModel.AddJumpBlockReason(JumpBlockReasonEnum.EmoteNavigation);
				RefreshNavigationHighlight();
				RestartAutoHide();
			}
		}

		private void RefreshNavigationHighlight()
		{
			if (_navigationSelectionIndex == -1)
			{
				return;
			}
			if (_activeEmotesCount <= 0)
			{
				ResetNavigationSelection();
				return;
			}
			_navigationSelectionIndex = Mathf.Min(_navigationSelectionIndex, _activeEmotesCount - 1);
			for (int i = 0; i < base.View.EmotesList.Count; i++)
			{
				base.View.EmotesList[i].Animator.SetBool(_isUsed, i == _navigationSelectionIndex);
			}
		}

		private void RestartAutoHide()
		{
			if (_activeCoroutine == null)
			{
				if (_autoHideCoroutine != null)
				{
					base.View.StopCoroutine(_autoHideCoroutine);
				}
				_autoHideCoroutine = base.View.StartCoroutine(AutoHideCoroutine());
			}
		}

		private void ResetNavigationSelection()
		{
			_navigationSelectionIndex = -1;
			_playerMovableModel.RemoveJumpBlockReason(JumpBlockReasonEnum.EmoteNavigation);
		}

		private static Vector2Int ToNavigationDirection(Vector2 navigation)
		{
			if (Mathf.Abs(navigation.x) > Mathf.Abs(navigation.y))
			{
				if (!(Mathf.Abs(navigation.x) < 0.5f))
				{
					return new Vector2Int((int)Mathf.Sign(navigation.x), 0);
				}
				return Vector2Int.zero;
			}
			if (!(Mathf.Abs(navigation.y) < 0.5f))
			{
				return new Vector2Int(0, (int)Mathf.Sign(navigation.y));
			}
			return Vector2Int.zero;
		}

		private void OnEmoteNavigationPerformed(Vector2 navigation)
		{
			Vector2Int vector2Int = ToNavigationDirection(navigation);
			if (vector2Int == _lastNavigationDirection)
			{
				return;
			}
			_lastNavigationDirection = vector2Int;
			if (!(vector2Int == Vector2Int.zero) && _emoteAvailabilityModel.IsEmoteActive && _currentEmoteGroup != EmoteGroup.None)
			{
				if (vector2Int.x != 0)
				{
					SwitchNavigationGroup(vector2Int.x);
				}
				else
				{
					MoveNavigationSelection(-vector2Int.y);
				}
			}
		}

		private void OnEmoteNavigationCanceled(Vector2 navigation)
		{
			_lastNavigationDirection = Vector2Int.zero;
		}

		private void OnCurrentActiveDeviceChange(InputDevice inputDevice)
		{
			ApplyInputDeviceHints();
		}

		private void OnUIApply()
		{
			if (_currentEmoteGroup != EmoteGroup.None && _navigationSelectionIndex != -1 && _emoteAvailabilityModel.IsEmoteActive)
			{
				TriggerEmote(base.View.EmotesList[_navigationSelectionIndex].EmoteIndex);
			}
		}
	}
}
