using System;

namespace RSG.Muffin.ApplicationFilesModule.Core.Scripts
{
	public class MissingTypeException : Exception
	{
		public MissingTypeException(string enumName)
			: base("Type \"" + enumName + "\" doesnt exist in the project")
		{
		}
	}
}
