using System;

namespace Features.GameModeModule.Scripts
{
	public class GameModeModel
	{
		private GameModeType _currentGameMode = GameModeType.Default;

		public GameModeType CurrentGameMode
		{
			get
			{
				return _currentGameMode;
			}
			set
			{
				_currentGameMode = value;
				this.OnCurrentGameModeChanged?.Invoke(_currentGameMode);
			}
		}

		public event Action<GameModeType> OnCurrentGameModeChanged;
	}
}
