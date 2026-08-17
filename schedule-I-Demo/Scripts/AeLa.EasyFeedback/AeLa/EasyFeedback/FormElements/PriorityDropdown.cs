using System.Collections.Generic;
using System.Linq;
using AeLa.EasyFeedback.APIs;
using AeLa.EasyFeedback.UI;
using AeLa.EasyFeedback.UI.Interfaces;

namespace AeLa.EasyFeedback.FormElements
{
	public class PriorityDropdown : FormElement
	{
		private IDropdown priorityDropdown;

		private IEnumerable<Label> labels;

		public override void Awake()
		{
			base.Awake();
			priorityDropdown = UIInterop.GetDropdown(base.gameObject);
			priorityDropdown.ClearOptions();
			labels = Form.Config.Board.Labels.OrderBy((Label l) => l.order);
			foreach (Label label in labels)
			{
				priorityDropdown.AddOption(label.name);
			}
			priorityDropdown.Value = 0;
			priorityDropdown.RefreshShownValue();
		}

		protected override void FormClosed()
		{
		}

		protected override void FormOpened()
		{
		}

		protected override void FormSubmitted()
		{
			Form.CurrentReport.AddLabel(labels.ElementAt(priorityDropdown.Value));
		}
	}
}
