using System;

namespace VisualDesignCafe.ShaderX
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ShaderProperty : Attribute
	{
		public readonly string Name;

		public ShaderProperty(string name)
		{
			Name = name;
		}
	}
}
