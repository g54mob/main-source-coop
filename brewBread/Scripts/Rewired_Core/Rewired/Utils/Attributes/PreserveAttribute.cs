using System;

namespace Rewired.Utils.Attributes
{
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class PreserveAttribute : Attribute
	{
	}
}
