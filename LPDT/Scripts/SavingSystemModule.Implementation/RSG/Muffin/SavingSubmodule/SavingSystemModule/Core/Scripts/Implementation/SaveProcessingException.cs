using System;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SaveProcessingException : Exception
	{
		public SaveProcessingException(string message)
			: base(message)
		{
		}
	}
}
