namespace QFSW.QC
{
	public interface ICommandAction
	{
		bool IsFinished { get; }

		bool StartsIdle { get; }

		void Start(ActionContext context);

		void Finalize(ActionContext context);
	}
}
