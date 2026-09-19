using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.ViewSystemModule.Scripts.Windows;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayerStatesModule.Scripts.Views
{
	[PublicAPI]
	public class DeathPresenter : PresenterBehaviour<DeathViewBase>
	{
		private readonly SpectatorWindow _spectatorWindow;

		private readonly DeathWindow _deathWindow;

		private readonly SpectatorViewModel _spectatorViewModel;

		private readonly PlayerStatesConfiguration _playerStatesConfiguration;

		private CancellationTokenSource _cts;

		public DeathPresenter(SpectatorWindow spectatorWindow, DeathWindow deathWindow, SpectatorViewModel spectatorViewModel, PlayerStatesConfiguration playerStatesConfiguration)
		{
			_spectatorWindow = spectatorWindow;
			_deathWindow = deathWindow;
			_spectatorViewModel = spectatorViewModel;
			_playerStatesConfiguration = playerStatesConfiguration;
		}

		protected override void OnViewSet()
		{
			_deathWindow.OnWindowOpened += OnDeathWindowOpened;
			_deathWindow.OnWindowClosed += OnDeathWindowClosed;
		}

		protected override void OnDisposed()
		{
			_deathWindow.OnWindowOpened -= OnDeathWindowOpened;
			_deathWindow.OnWindowClosed -= OnDeathWindowClosed;
			CancelTimer();
		}

		private void OnDeathWindowOpened(Type _)
		{
			CancelTimer();
			_cts = new CancellationTokenSource();
			TransitionToSpectatorDelayed(_cts.Token).Forget();
		}

		private void OnDeathWindowClosed(Type _)
		{
			CancelTimer();
		}

		private void CancelTimer()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_cts = null;
		}

		private async UniTaskVoid TransitionToSpectatorDelayed(CancellationToken cancellationToken)
		{
			try
			{
				await UniTask.WaitForSeconds(base.View.DeadToSpectatorDelay, ignoreTimeScale: false, PlayerLoopTiming.Update, cancellationToken);
				_spectatorViewModel.ShowWithFade = true;
				_deathWindow.Close();
				_spectatorWindow.Open();
			}
			catch (OperationCanceledException)
			{
			}
		}
	}
}
