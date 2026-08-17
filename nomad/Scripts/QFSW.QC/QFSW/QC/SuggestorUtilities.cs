using QFSW.QC.Utilities;

namespace QFSW.QC
{
	public static class SuggestorUtilities
	{
		public static bool IsCompatible(string prompt, string suggestion, SuggestorOptions options)
		{
			if (prompt.Length > suggestion.Length)
			{
				return false;
			}
			if (options.Fuzzy)
			{
				if (!options.CaseSensitive)
				{
					return suggestion.ContainsCaseInsensitive(prompt);
				}
				return suggestion.Contains(prompt);
			}
			return suggestion.StartsWith(prompt, !options.CaseSensitive, null);
		}
	}
}
