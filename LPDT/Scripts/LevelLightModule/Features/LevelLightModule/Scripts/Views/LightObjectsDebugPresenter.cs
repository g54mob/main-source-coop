using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.LevelLightModule.Scripts.Views
{
	[PublicAPI]
	public class LightObjectsDebugPresenter : PresenterBehaviour<LightObjectsDebugViewBase>
	{
		private ILightObjectsGroupService _lightObjectsGroupService;

		public LightObjectsDebugPresenter(ILightObjectsGroupService lightObjectsGroupService)
		{
			_lightObjectsGroupService = lightObjectsGroupService;
		}

		protected override void OnViewSet()
		{
			AvailableLightObjectGroupSetup();
			base.View.EnableLightObjectsGroupButton.onClick.AddListener(EnableSelectedLightObjectsGroup);
			base.View.DisableLightObjectsGroupButton.onClick.AddListener(DisableSelectedLightObjectsGroup);
		}

		protected override void OnDisposed()
		{
			base.View.EnableLightObjectsGroupButton.onClick.RemoveListener(EnableSelectedLightObjectsGroup);
			base.View.DisableLightObjectsGroupButton.onClick.RemoveListener(DisableSelectedLightObjectsGroup);
		}

		private void EnableSelectedLightObjectsGroup()
		{
			_lightObjectsGroupService.SetLightObjectGroupState((LightObjectGroup)base.View.TargetLightObjectsGroupDropdown.value, state: true);
		}

		private void DisableSelectedLightObjectsGroup()
		{
			_lightObjectsGroupService.SetLightObjectGroupState((LightObjectGroup)base.View.TargetLightObjectsGroupDropdown.value, state: false);
		}

		private void AvailableLightObjectGroupSetup()
		{
			base.View.TargetLightObjectsGroupDropdown.ClearOptions();
			string[] names = Enum.GetNames(typeof(LightObjectGroup));
			base.View.TargetLightObjectsGroupDropdown.AddOptions(new List<string>(names));
		}
	}
}
