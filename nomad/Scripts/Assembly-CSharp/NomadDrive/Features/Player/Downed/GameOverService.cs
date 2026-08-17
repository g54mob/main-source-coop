using System;
using Cysharp.Threading.Tasks;
using EvilCore;
using EvilCore.Networking;

namespace NomadDrive.Features.Player.Downed
{
	public class GameOverService : IGameOverService
	{
		private readonly INetworkManager _networkManager;

		private IReloadLastSaveHandler _reloadHandler;

		private bool _isGameOver;

		public bool IsGameOver => _isGameOver;

		public event Action<GameOverReason> OnGameOver;

		public GameOverService(INetworkManager networkManager)
		{
			_networkManager = networkManager;
		}

		public void ServerTriggerGameOver(GameOverReason reason)
		{
			if (!_isGameOver)
			{
				_isGameOver = true;
				TriggerDeferredAsync(reason).Forget();
			}
		}

		private async UniTaskVoid TriggerDeferredAsync(GameOverReason reason)
		{
			await UniTask.NextFrame();
			this.OnGameOver?.Invoke(reason);
		}

		public void ServerResetGameOver()
		{
			_isGameOver = false;
		}

		public void RegisterReloadHandler(IReloadLastSaveHandler handler)
		{
			_reloadHandler = handler;
		}

		public void ReloadLastSave()
		{
			if (_reloadHandler != null)
			{
				_reloadHandler.ReloadLastSaveAsync().Forget();
			}
			else
			{
				_networkManager?.Disconnect();
			}
		}
	}
}
