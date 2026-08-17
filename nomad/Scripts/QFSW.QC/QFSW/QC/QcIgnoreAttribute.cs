using System;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class QcIgnoreAttribute : Attribute
	{
	}
}
