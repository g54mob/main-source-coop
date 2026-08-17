using System;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public sealed class CommandParameterDescriptionAttribute : Attribute
	{
		public readonly string Description;

		public readonly bool Valid;

		public CommandParameterDescriptionAttribute(string description)
		{
			Description = description;
			Valid = !string.IsNullOrWhiteSpace(description);
		}
	}
}
