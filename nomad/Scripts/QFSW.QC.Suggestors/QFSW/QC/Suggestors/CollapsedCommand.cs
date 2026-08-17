namespace QFSW.QC.Suggestors
{
	public struct CollapsedCommand
	{
		public CommandData Command;

		public int NumOptionalParams;

		public CollapsedCommand(CommandData command)
		{
			Command = command;
			NumOptionalParams = 0;
		}
	}
}
