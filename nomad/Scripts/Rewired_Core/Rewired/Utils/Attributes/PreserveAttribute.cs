using System;

namespace Rewired.Utils.Attributes
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class PreserveAttribute : Attribute
	{
	}
}
