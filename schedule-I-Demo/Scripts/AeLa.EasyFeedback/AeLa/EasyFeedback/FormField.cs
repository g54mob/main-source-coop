using UnityEngine;

namespace AeLa.EasyFeedback
{
	public abstract class FormField : FormElement
	{
		[Tooltip("The title of this field's section on the report")]
		public string SectionTitle;

		[Tooltip("Order of the section in the report (lowest first)")]
		public int SortOrder;
	}
}
