using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunGen
{
	public abstract class TilePlacementResult
	{
		public abstract string DisplayName { get; }

		public static string ProduceReport(IEnumerable<TilePlacementResult> results, int maxRetryAttempts)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"=== Failed to generate dungeon {maxRetryAttempts} times ===");
			stringBuilder.AppendLine("This could indicate an issue with the way your tiles are set up.");
			stringBuilder.AppendLine("Here is a list of all the reasons tile placement failed while trying to generate the dungeon:\n");
			foreach (IGrouping<Type, TilePlacementResult> item in from r in results
				group r by r.GetType() into g
				orderby g.Count() descending
				select g)
			{
				stringBuilder.AppendLine($"\t- {item.First().DisplayName} (x{item.Count()})");
			}
			return stringBuilder.ToString();
		}
	}
}
