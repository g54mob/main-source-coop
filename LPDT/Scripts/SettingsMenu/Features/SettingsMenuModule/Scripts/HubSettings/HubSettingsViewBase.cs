using System;
using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public abstract class HubSettingsViewBase : ViewBehaviour
	{
		[SerializeField]
		public List<HubSettingsTab> Tabs;

		public Action OnClose;

		public Action OnApply;

		public Action<HubSettingsTab> OnTabButtonClicked;

		public abstract void SetActiveTab(HubSettingsTab tab);

		public abstract void SetSaveButtonInteractable(bool interactable);
	}
}
