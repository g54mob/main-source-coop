using System;

namespace Febucci.UI.Core
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class EffectInfoAttribute : Attribute
	{
		public readonly string tag;

		public EffectInfoAttribute(string tag)
		{
			this.tag = tag;
		}
	}
}
