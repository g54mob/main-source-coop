using System.Collections.Generic;

namespace EvilCore.Inputs
{
	public interface IInputCoActivationProvider
	{
		bool CanCoActivate(int categoryIdA, int categoryIdB);

		IReadOnlyList<int> GetCoActiveCategories(int categoryId);

		bool IsSanctionedSharedDefault(string actionNameA, string actionNameB);
	}
}
