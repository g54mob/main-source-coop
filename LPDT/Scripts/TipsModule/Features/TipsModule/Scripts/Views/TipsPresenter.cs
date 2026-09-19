using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.CoroutineUtils.Scripts;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.TipsModule.Scripts.Data;
using Features.TipsModule.Scripts.Display;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Views
{
	[PublicAPI]
	public class TipsPresenter : PresenterBehaviour<TipsViewBase>
	{
		private readonly LineArmsModel _lineArmsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly TipsConfiguration _tipsConfiguration;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly IInputDeviceService _inputDeviceService;

		private LineArmControllerBase _lineArmController;

		private Coroutine _coroutine;

		private readonly InputModel _inputModel;

		private readonly TipBindContext _tipBindContext;

		private List<TipType> _visibleTips = new List<TipType>();

		private readonly List<IPointGrabable> _subscribedTipGrabbables = new List<IPointGrabable>();

		private int _currentTipsCount;

		public TipsPresenter(LineArmsModel lineArmsModel, MultiplayerModel multiplayerModel, TipsConfiguration tipsConfiguration, ICoroutineRunner coroutineRunner, ILocalizationService localizationService, IInputDeviceService inputDeviceService, InputModel inputModel, IInputDeviceActions inputActions)
		{
			_lineArmsModel = lineArmsModel;
			_multiplayerModel = multiplayerModel;
			_tipsConfiguration = tipsConfiguration;
			_coroutineRunner = coroutineRunner;
			_inputDeviceService = inputDeviceService;
			_inputModel = inputModel;
			_tipBindContext = new TipBindContext(localizationService, inputDeviceService);
		}

		protected override void OnViewSet()
		{
			HideAllTips();
			base.View.ContainerCanvasGroup.alpha = 0f;
			base.View.ContainerRect.localScale = Vector3.zero;
		}

		protected override void OnViewEnabled()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange += OnInputDeviceChanged;
			if (_lineArmsModel.TryGetLineArmForPlayer(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out _lineArmController))
			{
				LineArmControllerBase lineArmController = _lineArmController;
				lineArmController.OnGrabbedChanged = (Action)Delegate.Combine(lineArmController.OnGrabbedChanged, new Action(UpdateGrabbableTips));
				LineArmControllerBase lineArmController2 = _lineArmController;
				lineArmController2.OnLastRaycastChanged = (Action)Delegate.Combine(lineArmController2.OnLastRaycastChanged, new Action(UpdateGrabbableTips));
			}
			else
			{
				LineArmsModel lineArmsModel = _lineArmsModel;
				lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Combine(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(SubscribeByPlayer));
			}
		}

		protected override void OnViewDisabled()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange -= OnInputDeviceChanged;
			LineArmsModel lineArmsModel = _lineArmsModel;
			lineArmsModel.OnLineArmPlayerRegistered = (Action<int>)Delegate.Remove(lineArmsModel.OnLineArmPlayerRegistered, new Action<int>(SubscribeByPlayer));
			ClearGrabbableTipSubscriptions();
			if (!(_lineArmController == null))
			{
				LineArmControllerBase lineArmController = _lineArmController;
				lineArmController.OnGrabbedChanged = (Action)Delegate.Remove(lineArmController.OnGrabbedChanged, new Action(UpdateGrabbableTips));
				LineArmControllerBase lineArmController2 = _lineArmController;
				lineArmController2.OnLastRaycastChanged = (Action)Delegate.Remove(lineArmController2.OnLastRaycastChanged, new Action(UpdateGrabbableTips));
			}
		}

		protected override void OnDisposed()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange -= OnInputDeviceChanged;
		}

		private void OnInputDeviceChanged(InputDevice _)
		{
			if (!(_lineArmController == null))
			{
				UpdateGrabbableTips();
			}
		}

		private void SubscribeByPlayer(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId && _lineArmsModel.GetAllLineArmsForPlayer(playerId).ContainsKey(LineArmType.RightArmDefault))
			{
				_lineArmController = _lineArmsModel.GetAllLineArmsForPlayer(playerId)[LineArmType.RightArmDefault];
				LineArmControllerBase lineArmController = _lineArmController;
				lineArmController.OnGrabbedChanged = (Action)Delegate.Combine(lineArmController.OnGrabbedChanged, new Action(UpdateGrabbableTips));
				LineArmControllerBase lineArmController2 = _lineArmController;
				lineArmController2.OnLastRaycastChanged = (Action)Delegate.Combine(lineArmController2.OnLastRaycastChanged, new Action(UpdateGrabbableTips));
			}
		}

		private void UpdateGrabbableTips()
		{
			RefreshGrabbableTipSubscriptions();
			if (_coroutine != null)
			{
				_coroutineRunner.StopCoroutine(_coroutine);
			}
			_coroutine = _coroutineRunner.StartCoroutine(UpdateGrabbableTipsCoroutine());
		}

		private void OnHeldGrabbableTipsChanged(bool repaintAllTipsOnChange)
		{
			if (repaintAllTipsOnChange)
			{
				UpdateGrabbableTips();
			}
			else if (!TryUpdateToggleTipIncrementally())
			{
				UpdateGrabbableTips();
			}
		}

		private bool TryUpdateToggleTipIncrementally()
		{
			if (_lineArmController == null || _currentTipsCount == 0 || _visibleTips.Count == 0)
			{
				return false;
			}
			if (!TryCollectVisibleTipTypes(out var currentTips))
			{
				return false;
			}
			if (currentTips.Count != _visibleTips.Count)
			{
				return false;
			}
			int num = -1;
			for (int i = 0; i < currentTips.Count; i++)
			{
				if (currentTips[i] != _visibleTips[i])
				{
					if (num >= 0 || !IsActivateDeactivatePair(_visibleTips[i], currentTips[i]))
					{
						return false;
					}
					num = i;
				}
			}
			if (num < 0)
			{
				return true;
			}
			if (num >= base.View.ActiveSlots.Count)
			{
				return false;
			}
			if (!(base.View.ActiveSlots[num] is TipViewBase view))
			{
				return false;
			}
			TipType tipType = currentTips[num];
			if (!_tipsConfiguration.TryGetDisplay(tipType, out var display) || !(display is SimpleTipDisplayData simpleTipDisplayData))
			{
				return false;
			}
			simpleTipDisplayData.ApplyTo(view, _tipBindContext);
			_visibleTips[num] = tipType;
			return true;
		}

		private bool IsActivateDeactivatePair(TipType previousTip, TipType nextTip)
		{
			if ((previousTip == TipType.ActivateItemTip || previousTip == TipType.DeactivateItemTip) && (nextTip == TipType.ActivateItemTip || nextTip == TipType.DeactivateItemTip))
			{
				return previousTip != nextTip;
			}
			return false;
		}

		private void RefreshGrabbableTipSubscriptions()
		{
			if (_lineArmController == null)
			{
				ClearGrabbableTipSubscriptions();
				return;
			}
			for (int num = _subscribedTipGrabbables.Count - 1; num >= 0; num--)
			{
				IPointGrabable pointGrabable = _subscribedTipGrabbables[num];
				if (pointGrabable == null || !_lineArmController.CurrentGrabbables.Contains(pointGrabable))
				{
					if (pointGrabable != null)
					{
						pointGrabable.OnTipsChanged -= OnHeldGrabbableTipsChanged;
					}
					_subscribedTipGrabbables.RemoveAt(num);
				}
			}
			foreach (IPointGrabable currentGrabbable in _lineArmController.CurrentGrabbables)
			{
				if (currentGrabbable != null && !_subscribedTipGrabbables.Contains(currentGrabbable))
				{
					currentGrabbable.OnTipsChanged += OnHeldGrabbableTipsChanged;
					_subscribedTipGrabbables.Add(currentGrabbable);
				}
			}
		}

		private void ClearGrabbableTipSubscriptions()
		{
			foreach (IPointGrabable subscribedTipGrabbable in _subscribedTipGrabbables)
			{
				if (subscribedTipGrabbable != null)
				{
					subscribedTipGrabbable.OnTipsChanged -= OnHeldGrabbableTipsChanged;
				}
			}
			_subscribedTipGrabbables.Clear();
		}

		private IEnumerator UpdateGrabbableTipsCoroutine()
		{
			if (_currentTipsCount != 0)
			{
				float elapsed = GetElapsedFromCurveValue(base.View.HideCurve, base.View.ContainerCanvasGroup.alpha, base.View.AnimationDuration);
				while (elapsed < base.View.AnimationDuration)
				{
					elapsed += Time.deltaTime;
					float time = Mathf.Clamp01(elapsed / base.View.AnimationDuration);
					float num = base.View.HideCurve.Evaluate(time);
					base.View.ContainerCanvasGroup.alpha = num;
					base.View.ContainerRect.localScale = Vector3.one * num;
					yield return null;
				}
				HideAllTips();
			}
			if (!TryCollectVisibleTipTypes(out var currentTips))
			{
				_currentTipsCount = 0;
				yield break;
			}
			_currentTipsCount = currentTips.Count;
			if (_currentTipsCount != 0)
			{
				ShowTips(currentTips);
				float elapsed = GetElapsedFromCurveValue(base.View.ShowCurve, base.View.ContainerCanvasGroup.alpha, base.View.AnimationDuration);
				while (elapsed < base.View.AnimationDuration)
				{
					elapsed += Time.deltaTime;
					float time2 = Mathf.Clamp01(elapsed / base.View.AnimationDuration);
					float num2 = base.View.ShowCurve.Evaluate(time2);
					base.View.ContainerCanvasGroup.alpha = num2;
					base.View.ContainerRect.localScale = Vector3.one * num2;
					yield return null;
				}
			}
		}

		private bool TryCollectVisibleTipTypes(out List<TipType> currentTips)
		{
			currentTips = new List<TipType>();
			if (_lineArmController == null)
			{
				return false;
			}
			foreach (IPointGrabable currentGrabbable in _lineArmController.CurrentGrabbables)
			{
				foreach (TipType tip in currentGrabbable.GetTips())
				{
					if (!currentTips.Contains(tip))
					{
						currentTips.Add(tip);
					}
				}
			}
			if (_lineArmController.LastRaycastedGrabbable != null)
			{
				foreach (TipType raycastTip in _lineArmController.LastRaycastedGrabbable.GetRaycastTips())
				{
					if (!currentTips.Contains(raycastTip))
					{
						currentTips.Add(raycastTip);
					}
				}
			}
			for (int num = currentTips.Count - 1; num >= 0; num--)
			{
				TipType tipType = currentTips[num];
				if (!_tipsConfiguration.TryGetDisplay(tipType, out var display) || !display.IsVisibleForDevice(_inputModel.CurrentActiveDevice))
				{
					currentTips.RemoveAt(num);
				}
			}
			return true;
		}

		private float GetElapsedFromCurveValue(AnimationCurve curve, float currentValue, float duration, int samples = 64)
		{
			float num = 0f;
			float num2 = float.MaxValue;
			for (int i = 0; i <= samples; i++)
			{
				float num3 = (float)i / (float)samples;
				float num4 = Mathf.Abs(curve.Evaluate(num3) - currentValue);
				if (num4 < num2)
				{
					num2 = num4;
					num = num3;
				}
			}
			return num * duration;
		}

		private void ShowTips(List<TipType> currentTips)
		{
			_visibleTips = new List<TipType>(currentTips);
			base.View.ReleaseAll();
			for (int i = 0; i < currentTips.Count; i++)
			{
				TipType tipType = currentTips[i];
				if (_tipsConfiguration.TryGetDisplay(tipType, out var display))
				{
					display.Activate(base.View, _tipBindContext);
				}
			}
		}

		private void HideAllTips()
		{
			_visibleTips.Clear();
			base.View.ReleaseAll();
		}
	}
}
