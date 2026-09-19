using System;
using System.Diagnostics;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	[Conditional("FUSION_ANALYZER")]
	internal sealed class FieldNotWordAlignedAttribute : Attribute
	{
	}
}
