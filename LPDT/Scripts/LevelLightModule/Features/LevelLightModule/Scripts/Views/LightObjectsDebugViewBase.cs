using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.LevelLightModule.Scripts.Views
{
	public class LightObjectsDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button DisableLightObjectsGroupButton { get; private set; }

		[field: SerializeField]
		public Button EnableLightObjectsGroupButton { get; private set; }

		[field: SerializeField]
		public TMP_Dropdown TargetLightObjectsGroupDropdown { get; private set; }
	}
}
