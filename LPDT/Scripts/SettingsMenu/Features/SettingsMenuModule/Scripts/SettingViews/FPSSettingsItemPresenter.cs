using System;
using System.Collections.Generic;
using System.Linq;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class FPSSettingsItemPresenter : PresenterBehaviour<FPSSettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private CurrentSettingsModel _currentSettingsModel;

		private QualitySettingsOptionsConfiguration _qualitySettingsOptionsConfiguration;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel, QualitySettingsOptionsConfiguration qualitySettingsOptionsConfiguration)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
			_qualitySettingsOptionsConfiguration = qualitySettingsOptionsConfiguration;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			InitValue();
			FPSSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Combine(view.OnValueChanged, new Action<int>(OnFPSChanged));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			FPSSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Remove(view.OnValueChanged, new Action<int>(OnFPSChanged));
		}

		private void InitValue()
		{
			List<string> fpsValues = _qualitySettingsOptionsConfiguration.AllFpsLimits.Select((int l) => l.ToString()).ToList();
			base.View.SetFpsValues(fpsValues);
			int item = (_notAppliedSettingsModel.IsFpsLimitChanged ? _notAppliedSettingsModel.FpsLimit : _currentSettingsModel.FPSLimit);
			int num = _qualitySettingsOptionsConfiguration.AllFpsLimits.IndexOf(item);
			if (num < 0)
			{
				num = 0;
			}
			base.View.SetValueIndex(num);
			base.View.SetFpsLimitText(_qualitySettingsOptionsConfiguration.AllFpsLimits[num]);
		}

		private void OnFPSChanged(int valueIndex)
		{
			if (valueIndex >= 0 && valueIndex < _qualitySettingsOptionsConfiguration.AllFpsLimits.Count)
			{
				int num = _qualitySettingsOptionsConfiguration.AllFpsLimits[valueIndex];
				_notAppliedSettingsModel.FpsLimit = num;
				base.View.SetFpsLimitText(num);
			}
		}
	}
}
