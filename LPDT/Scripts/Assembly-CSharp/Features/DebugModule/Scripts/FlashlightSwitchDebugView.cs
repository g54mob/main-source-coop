using System.Collections.Generic;
using System.Linq;
using Features.LevelLightModule.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class FlashlightSwitchDebugView : FlashlightSwitchDebugViewBase
	{
		[SerializeField]
		private TMP_Dropdown _flashlightTypeDropdown;

		[SerializeField]
		private Button _applyButton;

		private FlashlightType[] _flashlightTypes = new FlashlightType[0];

		protected override void OnEnable()
		{
			base.OnEnable();
			_applyButton.onClick.AddListener(OnApplyButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_applyButton.onClick.RemoveListener(OnApplyButtonClicked);
		}

		public override void SetFlashlightTypes(IReadOnlyList<FlashlightType> flashlightTypes)
		{
			_flashlightTypes = flashlightTypes.ToArray();
			List<string> options = _flashlightTypes.Select((FlashlightType flashlightType) => flashlightType.ToString()).ToList();
			_flashlightTypeDropdown.ClearOptions();
			_flashlightTypeDropdown.AddOptions(options);
			_flashlightTypeDropdown.SetValueWithoutNotify(0);
		}

		private void OnApplyButtonClicked()
		{
			int value = _flashlightTypeDropdown.value;
			if (value >= 0 && value < _flashlightTypes.Length)
			{
				InvokeFlashlightTypeApplyRequested(_flashlightTypes[value]);
			}
		}
	}
}
