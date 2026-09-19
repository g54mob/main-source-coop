using System;

namespace Features.StoreModule.Scripts
{
	public sealed class StoreSeatingStepFailedException : Exception
	{
		public StoreSeatingStepFailedException(string message)
			: base(message)
		{
		}
	}
}
