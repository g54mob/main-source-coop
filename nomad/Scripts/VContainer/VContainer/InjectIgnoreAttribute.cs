using System;

namespace VContainer
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public class InjectIgnoreAttribute : Attribute
	{
	}
}
