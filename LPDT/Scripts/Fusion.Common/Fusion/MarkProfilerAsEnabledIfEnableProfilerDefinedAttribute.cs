using System;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Assembly)]
	[Conditional("ENABLE_PROFILER")]
	[Preserve]
	public class MarkProfilerAsEnabledIfEnableProfilerDefinedAttribute : Attribute
	{
	}
}
