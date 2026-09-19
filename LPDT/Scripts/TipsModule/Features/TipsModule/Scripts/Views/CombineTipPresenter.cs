using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.TipsModule.Scripts.Views
{
	[PublicAPI]
	public class CombineTipPresenter : PresenterBehaviour<CombineTipViewBase>
	{
		private readonly TutorialModel _tutorialModel;

		public CombineTipPresenter(TutorialModel tutorialModel)
		{
			_tutorialModel = tutorialModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			UpdateTipsGlowing();
		}

		private void UpdateTipsGlowing()
		{
			if (_tutorialModel.IsTutorialInProgress)
			{
				base.View.GlowableTipObject.EnablePulsating();
			}
			else
			{
				base.View.GlowableTipObject.DisablePulsating();
			}
		}
	}
}
