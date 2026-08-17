using UnityEngine;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal static class ExtensionMethods
	{
		public static bool IsNullOrDestroyed(this object @object)
		{
			if (@object != null)
			{
				if (@object is Object)
				{
					return (Object)@object == null;
				}
				return false;
			}
			return true;
		}
	}
}
