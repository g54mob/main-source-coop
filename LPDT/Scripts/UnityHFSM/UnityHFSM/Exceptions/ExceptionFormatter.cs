namespace UnityHFSM.Exceptions
{
	public static class ExceptionFormatter
	{
		public static string Format(string context = null, string problem = null, string solution = null)
		{
			return Format(null, context, problem, solution);
		}

		public static string Format(string location, string context, string problem, string solution)
		{
			string text = "\n";
			if (location != null)
			{
				text = text + "In " + location + "\n";
			}
			if (context != null)
			{
				text = text + "Context: " + context + "\n";
			}
			if (problem != null)
			{
				text = text + "Problem: " + problem + "\n";
			}
			if (solution != null)
			{
				text = text + "Solution: " + solution + "\n";
			}
			return text;
		}
	}
}
