using UnityEngine;

namespace AeLa.EasyFeedback.FormFields
{
	internal class GraphicsInfoCollector : FormField
	{
		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
		}

		protected override void FormSubmitted()
		{
			if (!Form.CurrentReport.HasSection(SectionTitle))
			{
				Form.CurrentReport.AddSection(SectionTitle, SortOrder);
			}
			Form.CurrentReport["Additional Info"].AppendLine("Quality Level: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
			Form.CurrentReport["Additional Info"].AppendLine("Resolution: " + Screen.width + "x" + Screen.height);
			Form.CurrentReport["Additional Info"].AppendLine("Full Screen: " + Screen.fullScreen);
		}
	}
}
