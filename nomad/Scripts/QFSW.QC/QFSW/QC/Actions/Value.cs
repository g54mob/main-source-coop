namespace QFSW.QC.Actions
{
	public class Value : ICommandAction
	{
		private readonly object _value;

		private readonly bool _newline;

		public bool IsFinished => true;

		public bool StartsIdle => false;

		public Value(object value, bool newline = true)
		{
			_value = value;
			_newline = newline;
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			QuantumConsole console = context.Console;
			string logText = (_value as string) ?? console.Serialize(_value);
			console.LogToConsole(logText, _newline);
		}
	}
}
