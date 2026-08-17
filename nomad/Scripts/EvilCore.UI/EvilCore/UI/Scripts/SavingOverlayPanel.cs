using System;
using Cysharp.Threading.Tasks;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class SavingOverlayPanel : GameCanvasGroup
	{
		[SerializeField]
		private TextMeshProUGUI savingText;

		[Tooltip("Keep the overlay visible at least this long so a fast save doesn't flash on screen.")]
		[SerializeField]
		private float minVisibleSeconds = 0.6f;

		[Inject]
		private IGameSaveService _gameSaveService;

		[Inject]
		private ILocalizationService _localizationService;

		private bool _hidePending;

		private void Start()
		{
			if (_canvasGroup == null)
			{
				Initialize();
			}
			RefreshText();
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += RefreshText;
			}
			if (_gameSaveService != null)
			{
				_gameSaveService.OnSaveStarted += HandleSaveStarted;
				_gameSaveService.OnSaveCompleted += HandleSaveCompleted;
			}
		}

		private void OnDestroy()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= RefreshText;
			}
			if (_gameSaveService != null)
			{
				_gameSaveService.OnSaveStarted -= HandleSaveStarted;
				_gameSaveService.OnSaveCompleted -= HandleSaveCompleted;
			}
		}

		private void RefreshText()
		{
			if (savingText != null)
			{
				savingText.text = Localize("@game_menu.saving");
			}
		}

		private void HandleSaveStarted()
		{
			_hidePending = false;
			Show(interactable: false, blockRaycast: true);
		}

		private void HandleSaveCompleted()
		{
			_hidePending = true;
			HideAfterDelayAsync(minVisibleSeconds).Forget();
		}

		private async UniTaskVoid HideAfterDelayAsync(float delaySeconds)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds), ignoreTimeScale: true, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (_hidePending)
			{
				_hidePending = false;
				Hide();
			}
		}

		private string Localize(string key)
		{
			if (_localizationService == null)
			{
				return key;
			}
			return _localizationService.Localize(key);
		}
	}
}
