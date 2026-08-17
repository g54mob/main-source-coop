using System;

namespace EvilCore.DI.Core
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class SoftRequireComponentAttribute : Attribute
	{
		public Type RequiredType { get; }

		public SoftRequireComponentAttribute(Type requiredType)
		{
			RequiredType = requiredType;
		}
	}
}
