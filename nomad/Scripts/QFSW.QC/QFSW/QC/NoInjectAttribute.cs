using System;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class NoInjectAttribute : Attribute
	{
	}
}
