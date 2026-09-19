using System;
using System.Collections.Generic;
using System.Linq;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ScreenResolutionSettingsItemPresenter : PresenterBehaviour<ScreenResolutionSettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private CurrentSettingsModel _currentSettingsModel;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_currentSettingsModel = currentSettingsModel;
			_notAppliedSettingsModel = notAppliedSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			InitValue();
			ScreenResolutionSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<Vector2Int>)Delegate.Combine(view.OnValueChanged, new Action<Vector2Int>(OnResolutionChanged));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			ScreenResolutionSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<Vector2Int>)Delegate.Remove(view.OnValueChanged, new Action<Vector2Int>(OnResolutionChanged));
		}

		private void InitValue()
		{
			List<Vector2Int> list = Screen.resolutions.Select((Resolution res) => new Vector2Int(res.width, res.height)).ToList();
			Vector2Int item = (_notAppliedSettingsModel.IsResolutionChanged ? _notAppliedSettingsModel.Resolution : _currentSettingsModel.Resolution);
			if (!list.Contains(item))
			{
				list.Insert(0, item);
			}
			base.View.SetResolutionDropdownValues(list);
			base.View.SetDropdownResolutionValue(item.x, item.y);
		}

		private void OnResolutionChanged(Vector2Int resolution)
		{
			_notAppliedSettingsModel.Resolution = resolution;
		}
	}
}
