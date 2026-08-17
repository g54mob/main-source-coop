using System.Collections.Generic;
using EvilCore.Inputs;

namespace NomadDrive.Features.Inputs
{
	public sealed class InputMapCoActivationProvider : IInputCoActivationProvider
	{
		public bool CanCoActivate(int categoryIdA, int categoryIdB)
		{
			return InputMapCoActivation.CanCoActivate(categoryIdA, categoryIdB);
		}

		public IReadOnlyList<int> GetCoActiveCategories(int categoryId)
		{
			return InputMapCoActivation.GetCoActiveCategories(categoryId);
		}

		public bool IsSanctionedSharedDefault(string actionNameA, string actionNameB)
		{
			return InputMapCoActivation.IsSanctionedSharedDefault(actionNameA, actionNameB);
		}
	}
}
