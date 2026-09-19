using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	public interface IUITipService
	{
		bool IsReady { get; }

		UITipHandle CreateTip(UITipType tipType, Vector2? anchoredPosition = null, bool animated = true);

		UITipHandle CreateTip<TPresenter>(UITipType tipType, Vector2? anchoredPosition = null, bool animated = true) where TPresenter : PresenterBehaviour;

		TPresenter GetPresenter<TPresenter>(UITipHandle handle) where TPresenter : PresenterBehaviour;

		void KillTip(UITipHandle handle, bool animated = true);

		void KillAll(bool animated = true);
	}
}
