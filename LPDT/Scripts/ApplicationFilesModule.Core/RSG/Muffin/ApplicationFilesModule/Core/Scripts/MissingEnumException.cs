using System;

namespace RSG.Muffin.ApplicationFilesModule.Core.Scripts
{
	public class MissingEnumException : Exception
	{
		public MissingEnumException(string enumName)
			: base("Enum \"" + enumName + "\" doesnt exist in the project")
		{
		}
	}
}
