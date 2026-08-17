using System;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class CommandDescriptionAttribute : Attribute
	{
		public readonly string Description;

		public readonly bool Valid;

		public CommandDescriptionAttribute(string description)
		{
			Description = description;
			Valid = !string.IsNullOrWhiteSpace(description);
		}
	}
}
