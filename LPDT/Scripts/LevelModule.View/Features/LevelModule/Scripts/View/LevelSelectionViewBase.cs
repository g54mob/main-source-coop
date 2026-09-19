using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.LevelModule.Scripts.View
{
	public abstract class LevelSelectionViewBase : ViewBehaviour
	{
		public abstract event Action<string> OnSelectedLevelChanged;

		public abstract void RefreshLevelSelectionDropdown(LevelType currentLevelType);

		public abstract void SetLevelSelectionActive(bool isActive);

		public abstract void InitializeLevelSelectionDropdown();
	}
}
