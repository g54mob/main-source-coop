using System.Collections.Generic;

namespace Fusion.Matchmaking.Extensions
{
	internal static class RealtimeExtensionsDictionaryProperties
	{
		public static int CalculateTotalSize(Dictionary<string, SessionProperty> dictionary)
		{
			return dictionary.ConvertToHashtable().CalculateTotalSize();
		}
	}
}
