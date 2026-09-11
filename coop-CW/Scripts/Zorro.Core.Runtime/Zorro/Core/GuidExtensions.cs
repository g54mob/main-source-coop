using System;

namespace Zorro.Core
{
	public static class GuidExtensions
	{
		public static string ToShortString(this Guid id)
		{
			return id.ToString().Substring(0, 8);
		}
	}
}
