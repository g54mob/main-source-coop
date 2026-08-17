namespace QFSW.QC.Actions
{
	public class RemoveLog : ICommandAction
	{
		public bool IsFinished => true;

		public bool StartsIdle => false;

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			context.Console.RemoveLogTrace();
		}
	}
}
