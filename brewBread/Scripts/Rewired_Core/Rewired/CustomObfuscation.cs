using System;
using System.Runtime.InteropServices;

namespace Rewired
{
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	internal sealed class CustomObfuscation : Attribute
	{
		public bool rename;
	}
}
