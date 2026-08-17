using UnityEngine;

namespace AeLa.EasyFeedback.FormFields
{
	internal class PlayerInfoCollector : FormField
	{
		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
		}

		protected override void FormSubmitted()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (!(gameObject == null))
			{
				if (!Form.CurrentReport.HasSection(SectionTitle))
				{
					Form.CurrentReport.AddSection(SectionTitle, SortOrder);
				}
				Form.CurrentReport["Additional Info"].AppendLine("Player Position: " + gameObject.transform.position.ToString());
			}
		}
	}
}
