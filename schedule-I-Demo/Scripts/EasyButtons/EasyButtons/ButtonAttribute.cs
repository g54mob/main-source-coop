using System;
using JetBrains.Annotations;

namespace EasyButtons
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class ButtonAttribute : Attribute
	{
		public readonly string Name;

		[PublicAPI]
		public ButtonMode Mode { get; set; }

		[PublicAPI]
		public ButtonSpacing Spacing { get; set; }

		[PublicAPI]
		public bool Expanded { get; set; }

		public ButtonAttribute()
		{
		}

		public ButtonAttribute(string name)
		{
			Name = name;
		}
	}
}
