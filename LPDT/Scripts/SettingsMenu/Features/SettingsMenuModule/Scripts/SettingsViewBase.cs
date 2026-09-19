using System;
using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts
{
	public abstract class SettingsViewBase : ViewBehaviour
	{
		[SerializeField]
		public List<SettingsTab> Tabs;

		public Action OnClose;

		public Action OnApply;

		public Action OnSendReport;

		public Action<SettingsTab> OnTabButtonClicked;

		[field: SerializeField]
		public RectTransform AnimationTarget { get; private set; }

		[field: SerializeField]
		public bool IsMenu { get; private set; }

		public abstract Dictionary<SettingsTab, SelectableSettingsTab> TabsMap { get; }

		public abstract void SetActiveTab(SettingsTab tab);

		public abstract bool TrySetSaveButtonInteractable(bool interactable);
	}
}
