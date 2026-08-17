using AeLa.EasyFeedback.UI;
using UnityEngine;

namespace AeLa.EasyFeedback.FormElements
{
	public class ReportTitle : FormElement
	{
		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
		}

		protected override void FormSubmitted()
		{
			Form.CurrentReport.Title = UIInterop.GetInputField(base.gameObject).Text + " (" + Application.version + ")";
		}
	}
}
