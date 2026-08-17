using System;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal static class Factory
	{
		public static object CreateInstance(Type type, object[] args = null)
		{
			if ((object)type == null)
			{
				return null;
			}
			if ((object)type == typeof(SerializedObject))
			{
				return new SerializedObject(null, SerializedObject.ObjectType.List, (args != null && args.Length != 0) ? ((int)args[0]) : 0);
			}
			return Activator.CreateInstance(type);
		}
	}
}
