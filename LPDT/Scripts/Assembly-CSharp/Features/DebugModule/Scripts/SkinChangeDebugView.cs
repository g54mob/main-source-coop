using System;
using System.Collections.Generic;
using System.Linq;
using Features.SkinChangeModule.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class SkinChangeDebugView : SkinChangeDebugViewBase
	{
		[SerializeField]
		private TMP_Dropdown _skinPartDropdown;

		[SerializeField]
		private TMP_InputField _skinIdInputField;

		[SerializeField]
		private Button _applyButton;

		private SkinPartType[] _skinPartTypes;

		protected override void OnEnable()
		{
			base.OnEnable();
			InitializeSkinPartDropdown();
			_applyButton.onClick.AddListener(HandleApplyButtonClick);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_applyButton.onClick.RemoveListener(HandleApplyButtonClick);
		}

		private void InitializeSkinPartDropdown()
		{
			_skinPartDropdown.ClearOptions();
			_skinPartTypes = Enum.GetValues(typeof(SkinPartType)).Cast<SkinPartType>().ToArray();
			List<string> options = _skinPartTypes.Select((SkinPartType skinPartType) => skinPartType.ToString()).ToList();
			_skinPartDropdown.AddOptions(options);
		}

		private void HandleApplyButtonClick()
		{
			if (int.TryParse(_skinIdInputField.text, out var result))
			{
				SkinPartType arg = _skinPartTypes[_skinPartDropdown.value];
				OnSkinChangeRequested?.Invoke(arg, result);
			}
		}
	}
}
