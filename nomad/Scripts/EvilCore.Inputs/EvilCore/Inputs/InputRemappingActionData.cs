using Rewired;

namespace EvilCore.Inputs
{
	public class InputRemappingActionData
	{
		public int ActionId { get; set; }

		public string ActionName { get; set; }

		public string ActionDescriptiveName { get; set; }

		public InputActionType ActionType { get; set; }

		public AxisRange AxisRange { get; set; }

		public string CurrentBindingName { get; set; }

		public int ActionElementMapId { get; set; }

		public int MapCategoryId { get; set; }
	}
}
