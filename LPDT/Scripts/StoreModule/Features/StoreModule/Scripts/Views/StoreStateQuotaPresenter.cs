using System.Collections;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.QuotaModule.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.StoreModule.Scripts.Views
{
	[PublicAPI]
	public class StoreStateQuotaPresenter : PresenterBehaviour<StoreStateQuotaViewBase>
	{
		private readonly CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private readonly StoreDraftMoneyModel _storeDraftMoneyModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private float _displayedMoney;

		private float _targetMoney;

		private Coroutine _moneyLerpCoroutine;

		public StoreStateQuotaPresenter(CurrentWalletSynchronizedModel currentWalletSynchronizedModel, StoreDraftMoneyModel storeDraftMoneyModel, PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, ICoroutineRunner coroutineRunner)
		{
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_coroutineRunner = coroutineRunner;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit += OnPlayerStateExit;
			_storeDraftMoneyModel.OnDraftMoneyChanged += OnDraftMoneyChanged;
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged += OnCurrentSessionMoneyChanged;
			RefreshStoreVisibility();
			if (IsLocalStoreActive())
			{
				PushMoneyDisplay(immediate: true);
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
			_playersStatesSynchronizer.OnSomePlayerStateExit -= OnPlayerStateExit;
			_storeDraftMoneyModel.OnDraftMoneyChanged -= OnDraftMoneyChanged;
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged -= OnCurrentSessionMoneyChanged;
			if (_moneyLerpCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_moneyLerpCoroutine);
				_moneyLerpCoroutine = null;
			}
		}

		private void OnPlayerStateChanged(PlayerStateData data)
		{
			if (data.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				RefreshStoreVisibility();
				if (data.PlayerState == PlayerState.Store)
				{
					PushMoneyDisplay(immediate: false);
				}
			}
		}

		private void OnPlayerStateExit(PlayerStateData data)
		{
			if (data.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && data.PlayerState == PlayerState.Store)
			{
				RefreshStoreVisibility();
			}
		}

		private void RefreshStoreVisibility()
		{
			bool visibilityRootActive = IsLocalStoreActive();
			base.View.SetVisibilityRootActive(visibilityRootActive);
		}

		private bool IsLocalStoreActive()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playersStatesSynchronizer.TryGetState(playerId, out var state))
			{
				return state == PlayerState.Store;
			}
			return false;
		}

		private void OnDraftMoneyChanged(float _)
		{
			PushMoneyDisplay(immediate: false);
		}

		private void OnCurrentSessionMoneyChanged(float _)
		{
			PushMoneyDisplay(immediate: false);
		}

		private float ComputeTargetMoney()
		{
			return _currentWalletSynchronizedModel.CurrentSessionMoney - _storeDraftMoneyModel.DraftMoney;
		}

		private void PushMoneyDisplay(bool immediate)
		{
			if (!IsLocalStoreActive())
			{
				return;
			}
			_targetMoney = ComputeTargetMoney();
			if (immediate)
			{
				if (_moneyLerpCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_moneyLerpCoroutine);
					_moneyLerpCoroutine = null;
				}
				_displayedMoney = _targetMoney;
				ApplyText();
			}
			else
			{
				if (_moneyLerpCoroutine != null)
				{
					_coroutineRunner.StopCoroutine(_moneyLerpCoroutine);
				}
				_moneyLerpCoroutine = _coroutineRunner.StartCoroutine(LerpMoneyDisplay());
			}
		}

		private IEnumerator LerpMoneyDisplay()
		{
			float elapsedTime = 0f;
			float duration = 1f / Mathf.Max(0.001f, base.View.LerpSpeed);
			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				float time = Mathf.Clamp01(elapsedTime / duration);
				float t = base.View.LerpCurve.Evaluate(time);
				_displayedMoney = Mathf.Lerp(_displayedMoney, _targetMoney, t);
				ApplyText();
				yield return null;
			}
			_displayedMoney = _targetMoney;
			ApplyText();
			_moneyLerpCoroutine = null;
		}

		private void ApplyText()
		{
			base.View.SetQuotaLabel($"<#FFF932><size=35>¢</size></color>{_displayedMoney:F0}");
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.View.Container);
		}
	}
}
