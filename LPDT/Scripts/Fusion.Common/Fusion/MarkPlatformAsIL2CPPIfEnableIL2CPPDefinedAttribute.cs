using System;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Assembly)]
	[Conditional("ENABLE_IL2CPP")]
	[Preserve]
	public class MarkPlatformAsIL2CPPIfEnableIL2CPPDefinedAttribute : Attribute
	{
	}
}
