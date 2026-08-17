using System.Collections.Generic;

namespace NomadDrive.Features.Inputs
{
	public static class InputMapCoActivation
	{
		private static readonly Dictionary<int, HashSet<int>> CoActive = new Dictionary<int, HashSet<int>>
		{
			{
				1,
				new HashSet<int> { 1, 2, 3, 4, 5, 6 }
			},
			{
				2,
				new HashSet<int> { 1, 2, 4, 5, 6 }
			},
			{
				3,
				new HashSet<int> { 1, 3, 4, 6 }
			},
			{
				4,
				new HashSet<int> { 1, 2, 3, 4, 5, 6 }
			},
			{
				5,
				new HashSet<int> { 1, 2, 4, 5 }
			},
			{
				6,
				new HashSet<int> { 1, 2, 3, 4, 6 }
			}
		};

		private static readonly HashSet<string> SanctionedSharedDefaults = new HashSet<string>
		{
			Pair("Respawn", "ObjectivesPanelLock"),
			Pair("Respawn", "ToggleWipers"),
			Pair("ObjectivesPanelLock", "ToggleWipers"),
			Pair("ThrowObject", "ToggleHeadlights"),
			Pair("PrimaryInteraction", "HeldItemSecondaryUse"),
			Pair("PrimaryInteraction", "LiquidTransfer"),
			Pair("HeldItemSecondaryUse", "LiquidTransfer")
		};

		public static bool CanCoActivate(int categoryIdA, int categoryIdB)
		{
			if (categoryIdA == categoryIdB)
			{
				return true;
			}
			if (CoActive.TryGetValue(categoryIdA, out var value))
			{
				return value.Contains(categoryIdB);
			}
			return false;
		}

		public static IReadOnlyList<int> GetCoActiveCategories(int categoryId)
		{
			if (CoActive.TryGetValue(categoryId, out var value))
			{
				return new List<int>(value);
			}
			return new List<int> { categoryId };
		}

		public static bool IsSanctionedSharedDefault(string actionNameA, string actionNameB)
		{
			if (string.IsNullOrEmpty(actionNameA) || string.IsNullOrEmpty(actionNameB))
			{
				return false;
			}
			return SanctionedSharedDefaults.Contains(Pair(actionNameA, actionNameB));
		}

		private static string Pair(string a, string b)
		{
			if (string.CompareOrdinal(a, b) > 0)
			{
				return b + "|" + a;
			}
			return a + "|" + b;
		}
	}
}
