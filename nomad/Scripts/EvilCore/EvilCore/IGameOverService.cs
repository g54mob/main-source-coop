using System;

namespace EvilCore
{
	public interface IGameOverService
	{
		bool IsGameOver { get; }

		event Action<GameOverReason> OnGameOver;

		void ServerTriggerGameOver(GameOverReason reason);

		void ServerResetGameOver();

		void RegisterReloadHandler(IReloadLastSaveHandler handler);

		void ReloadLastSave();
	}
}
