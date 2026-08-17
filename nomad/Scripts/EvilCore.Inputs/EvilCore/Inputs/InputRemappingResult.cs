using Rewired;

namespace EvilCore.Inputs
{
	public struct InputRemappingResult
	{
		public int ActionId;

		public string ActionName;

		public string NewElementName;

		public ControllerType ControllerType;

		public int MapCategoryId;

		public RemapOutcome Outcome;

		public string ConflictingActionName;

		public string ConflictingCategoryName;
	}
}
