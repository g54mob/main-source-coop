using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DeathHandlingModule.Scripts.Views
{
	[PublicAPI]
	public class PreDeadTimerPresenter : PresenterBehaviour<PreDeadTimerViewBase>
	{
		private readonly PreDeadTimerModel _preDeadTimerModel;

		private bool _isGhostActivated;

		public PreDeadTimerPresenter(PreDeadTimerModel preDeadTimerModel)
		{
			_preDeadTimerModel = preDeadTimerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			UpdateSlider(_preDeadTimerModel.CurrentTimer - base.View.GhostDuration, _preDeadTimerModel.MaxTimer - base.View.GhostDuration);
			_preDeadTimerModel.OnTimerChanged += UpdateSlider;
			_preDeadTimerModel.OnTimerRunningChanged += base.View.SetVisible;
			ChangeVisibilityByTimer(_preDeadTimerModel.IsTimerRunning);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_preDeadTimerModel.OnTimerChanged -= UpdateSlider;
			_preDeadTimerModel.OnTimerRunningChanged -= base.View.SetVisible;
		}

		private void UpdateSlider(float current, float max)
		{
			if (!_preDeadTimerModel.IsTimerRunning)
			{
				return;
			}
			float normalizedValue = ((max > 0f) ? Mathf.Clamp01((current - base.View.GhostDuration) / (max - base.View.GhostDuration)) : 0f);
			base.View.SetNormalizedValue(normalizedValue);
			if (current <= base.View.GhostDuration)
			{
				if (!_isGhostActivated)
				{
					base.View.ActivateGhost();
					_isGhostActivated = true;
				}
			}
			else
			{
				_isGhostActivated = false;
			}
		}

		private void ChangeVisibilityByTimer(bool isRunning)
		{
			base.View.SetVisible(isRunning);
		}
	}
}
