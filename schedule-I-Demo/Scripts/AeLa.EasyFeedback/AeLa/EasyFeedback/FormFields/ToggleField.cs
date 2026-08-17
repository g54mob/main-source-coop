using System;
using UnityEngine;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.FormFields
{
	[RequireComponent(typeof(Toggle))]
	internal class ToggleField : FormField
	{
		[Tooltip("The label to prepend to this field on the report (won't be included if left blank)")]
		public string Label;

		[Tooltip("The default value of the toggle")]
		public bool Default;

		private Toggle sourceField;

		public override void Awake()
		{
			base.Awake();
			sourceField = GetComponent<Toggle>();
			sourceField.isOn = Default;
		}

		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
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
			string text = ((!string.IsNullOrEmpty(Label)) ? $"{Label}: {sourceField.isOn}" : sourceField.isOn.ToString());
			Form.CurrentReport[SectionTitle].SetText(text);
			Form.CurrentReport[SectionTitle].SortOrder = SortOrder;
			sourceField.isOn = Default;
		}
	}
}
