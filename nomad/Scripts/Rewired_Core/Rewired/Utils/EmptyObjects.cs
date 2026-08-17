using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class EmptyObjects<T>
	{
		private static T[] RtkyLBAFjcjMzezrFPZdHmNECcJEA;

		private static IList<T> UANyWDMudYEQvLmlgbEEKNjzXVOoA;

		public static T[] array => RtkyLBAFjcjMzezrFPZdHmNECcJEA ?? (RtkyLBAFjcjMzezrFPZdHmNECcJEA = new T[0]);

		public static IList<T> EmptyReadOnlyIListT => UANyWDMudYEQvLmlgbEEKNjzXVOoA ?? (UANyWDMudYEQvLmlgbEEKNjzXVOoA = new ReadOnlyCollection<T>(new List<T>()));
	}
}
