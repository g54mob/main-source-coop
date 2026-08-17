using System;

namespace MapMagic.Nodes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class SharedValueDefaultAttribute : Attribute
	{
		public string name;

		public object val;

		public SharedValueDefaultAttribute(string name, object val)
		{
			this.name = name;
			this.val = val;
		}
	}
}
