using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.StruggleBarModule.Scripts.Views
{
	[PublicAPI]
	public class StruggleBarPresenter : PresenterBehaviour<StruggleBarViewBase>
	{
		private readonly StruggleBarModel _struggleBarModel;

		public StruggleBarPresenter(StruggleBarModel struggleBarModel)
		{
			_struggleBarModel = struggleBarModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.SetNormalizedValue(_struggleBarModel.CurrentFill);
			base.View.SetVisible(_struggleBarModel.IsActive);
			_struggleBarModel.OnFillChanged += base.View.SetNormalizedValue;
			_struggleBarModel.OnActiveChanged += base.View.SetVisible;
			_struggleBarModel.OnBoostApplied += base.View.PlayPressScalePulse;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_struggleBarModel.OnFillChanged -= base.View.SetNormalizedValue;
			_struggleBarModel.OnActiveChanged -= base.View.SetVisible;
			_struggleBarModel.OnBoostApplied -= base.View.PlayPressScalePulse;
		}
	}
}
