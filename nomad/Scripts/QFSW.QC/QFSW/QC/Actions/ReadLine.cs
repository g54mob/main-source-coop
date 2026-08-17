using System;

namespace QFSW.QC.Actions
{
	public class ReadLine : ICommandAction
	{
		private readonly Action<string> _getInput;

		private readonly ResponseConfig _config;

		private QuantumConsole _console;

		private string _response;

		public bool IsFinished => _response != null;

		public bool StartsIdle => true;

		public ReadLine(Action<string> getInput, ResponseConfig config)
		{
			if (getInput == null)
			{
				throw new ArgumentNullException("getInput");
			}
			_getInput = getInput;
			_config = config;
			_console = null;
			_response = null;
		}

		public ReadLine(Action<string> getInput)
			: this(getInput, ResponseConfig.Default)
		{
		}

		public void Finalize(ActionContext context)
		{
			_getInput(_response);
		}

		public void Start(ActionContext context)
		{
			_response = null;
			_console = context.Console;
			_console.BeginResponse(OnResponseSubmittedHandler, _config);
		}

		private void OnResponseSubmittedHandler(string response)
		{
			_response = response;
		}
	}
}
