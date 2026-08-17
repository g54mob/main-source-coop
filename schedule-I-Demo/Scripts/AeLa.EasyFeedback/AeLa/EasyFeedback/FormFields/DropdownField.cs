using System;
using AeLa.EasyFeedback.UI;
using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine;

namespace AeLa.EasyFeedback.FormFields
{
	internal class DropdownField : FormField
	{
		[Tooltip("The label to prepend to this field on the report (won't be included if left blank)")]
		public string Label;

		private IDropdown sourceField;

		public override void Awake()
		{
			base.Awake();
			sourceField = UIInterop.GetDropdown(base.gameObject);
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
			string text = ((!string.IsNullOrEmpty(Label)) ? $"{Label}: {sourceField.CaptionText.Text}" : sourceField.CaptionText.Text);
			Form.CurrentReport[SectionTitle].SetText(text);
			Form.CurrentReport[SectionTitle].SortOrder = SortOrder;
		}
	}
}
