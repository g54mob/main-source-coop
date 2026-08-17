using System;

namespace EvilCore.DI.Core
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ServiceTagAttribute : Attribute
	{
		public string Tag { get; }

		public ServiceTagAttribute(string tag)
		{
			Tag = tag;
		}
	}
}
