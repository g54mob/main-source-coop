using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public class BeachValidationResult
	{
		private readonly List<BeachValidationMessage> _messages = new List<BeachValidationMessage>();

		public IReadOnlyList<BeachValidationMessage> Messages => _messages;

		public bool IsValid => _messages.All((BeachValidationMessage message) => message.Severity != BeachValidationSeverity.Error);

		public bool HasWarnings => _messages.Any((BeachValidationMessage message) => message.Severity == BeachValidationSeverity.Warning);

		public void AddError(string message)
		{
			_messages.Add(new BeachValidationMessage(BeachValidationSeverity.Error, message));
		}

		public void AddWarning(string message)
		{
			_messages.Add(new BeachValidationMessage(BeachValidationSeverity.Warning, message));
		}

		public void AddInfo(string message)
		{
			_messages.Add(new BeachValidationMessage(BeachValidationSeverity.Info, message));
		}

		public void Merge(BeachValidationResult other)
		{
			if (other != null)
			{
				_messages.AddRange(other.Messages);
			}
		}
	}
}
