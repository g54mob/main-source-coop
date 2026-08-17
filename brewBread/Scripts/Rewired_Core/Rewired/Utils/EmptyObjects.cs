using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class EmptyObjects<T>
	{
		private static T[] nGXUNsriBWFFnaAvKCPXkGJVjEVR;

		private static IList<T> wqzSytQFRvrnoGCNRPcoSKigmogb;

		public static T[] array => nGXUNsriBWFFnaAvKCPXkGJVjEVR ?? (nGXUNsriBWFFnaAvKCPXkGJVjEVR = new T[0]);

		public static IList<T> EmptyReadOnlyIListT => wqzSytQFRvrnoGCNRPcoSKigmogb ?? (wqzSytQFRvrnoGCNRPcoSKigmogb = new ReadOnlyCollection<T>(new List<T>()));
	}
}
