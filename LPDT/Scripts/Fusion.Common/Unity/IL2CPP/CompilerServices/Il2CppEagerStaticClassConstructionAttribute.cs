using System;
using System.Diagnostics;

namespace Unity.IL2CPP.CompilerServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	[Conditional("FUSION_UNITY")]
	public class Il2CppEagerStaticClassConstructionAttribute : Attribute
	{
	}
}
