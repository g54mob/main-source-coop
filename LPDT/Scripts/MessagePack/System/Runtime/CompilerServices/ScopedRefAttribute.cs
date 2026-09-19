using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	internal sealed class ScopedRefAttribute : Attribute
	{
	}
}
