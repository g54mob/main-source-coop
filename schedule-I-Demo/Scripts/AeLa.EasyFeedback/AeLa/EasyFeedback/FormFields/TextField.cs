using System;
using AeLa.EasyFeedback.UI;
using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine;

namespace AeLa.EasyFeedback.FormFields
{
	internal class TextField : FormField
	{
		[Tooltip("The label to prepend to this field on the report (won't be included if left blank)")]
		public string Label;

		private IInputField sourceField;

		private string lastValue;

		public override void Awake()
		{
			base.Awake();
			sourceField = UIInterop.GetInputField(base.gameObject);
			sourceField.OnValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(string val)
		{
			if (!Input.GetKey(KeyCode.Escape))
			{
				lastValue = val;
			}
		}

		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
			sourceField.Text = lastValue;
		}

		protected override void FormSubmitted()
		{
			if (string.IsNullOrEmpty(SectionTitle))
			{
				throw new NullReferenceException("The section title for this field is not set!");
			}
			if (!Form.CurrentReport.HasSection(SectionTitle))
			{
				Form.CurrentReport.AddSection(SectionTitle, SortOrder);
			}
			else
			{
				Debug.LogWarning("The section " + SectionTitle + " already exists! Overwriting.");
			}
			string text = ((!string.IsNullOrEmpty(Label)) ? $"{Label}: {sourceField.Text}" : sourceField.Text);
			Form.CurrentReport[SectionTitle].SetText(text);
			Form.CurrentReport[SectionTitle].SortOrder = SortOrder;
			lastValue = string.Empty;
		}
	}
}
