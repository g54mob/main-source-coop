using System;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Assembly)]
	[Conditional("UNITY_WEBGL")]
	[Preserve]
	public class MarkPlatformAsWebIfUnityWebGlDefinedAttribute : Attribute
	{
	}
}
