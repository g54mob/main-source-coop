using System;

namespace Unity.IL2CPP.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate, Inherited = false, AllowMultiple = true)]
	public class Il2CppSetOptionAttribute : Attribute
	{
		public Option Option { get; private set; }

		public object Value { get; private set; }

		public Il2CppSetOptionAttribute(Option option, object value)
		{
			Option = option;
			Value = value;
		}
	}
}
