using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.GameModeModule.Scripts.Views
{
	public abstract class GameModeViewBase : ViewBehaviour
	{
		public abstract event Action<string> OnGameModeChanged;

		public abstract void RefreshGameModeDropdown(GameModeType activeGameMode);

		public abstract void SetGameModeContainerActive(bool isActive);
	}
}
