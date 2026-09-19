using System;
using Cysharp.Threading.Tasks;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SessionManagementModule.Models;
using UnityEngine;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionLoadingUiAdapter : ISessionLoadingUi
	{
		private static readonly TimeSpan RevealFloorTimeout = TimeSpan.FromSeconds(5.0);

		private readonly ILoadingScreenService _loadingScreenService;

		public bool IsBlackoutRaised => _loadingScreenService.IsBlackoutRaised;

		public SessionLoadingUiAdapter(ILoadingScreenService loadingScreenService)
		{
			_loadingScreenService = loadingScreenService;
		}

		public UniTask ShowAsync()
		{
			return _loadingScreenService.ShowAsync(LoadingScreenShowType.ShowFade);
		}

		public UniTask ShowRunStartAsync()
		{
			return _loadingScreenService.ShowAsync(LoadingScreenShowType.ShowUntilPlayersLoading);
		}

		public async UniTask HideAsync()
		{
			try
			{
				await _loadingScreenService.FadeOutAsync().Timeout(RevealFloorTimeout);
			}
			catch (TimeoutException)
			{
				Debug.LogError("[SessionHealth] transition reveal (FadeOutAsync) did not complete within " + $"{RevealFloorTimeout.TotalSeconds:F0}s — the loading-overlay hide-coalescing latch hung; force-clearing the blackout.");
			}
			if (_loadingScreenService.IsBlackoutRaised)
			{
				Debug.LogError("[SessionHealth] transition blackout still raised after the reveal — a racing overlay op left the BlackScreenWindow up; force-clearing it so the view is not hard-locked.");
				_loadingScreenService.ForceClearBlackout();
			}
		}
	}
}
