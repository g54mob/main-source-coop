using System;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public class BeachValidationMessage
	{
		public BeachValidationSeverity Severity { get; }

		public string Message { get; }

		public BeachValidationMessage(BeachValidationSeverity severity, string message)
		{
			Severity = severity;
			Message = message;
		}
	}
}
