using System;

namespace QFSW.QC.Actions
{
	public class WaitWhile : ICommandAction
	{
		private readonly Func<bool> _condition;

		public bool IsFinished => !_condition();

		public bool StartsIdle => true;

		public WaitWhile(Func<bool> condition)
		{
			_condition = condition;
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
		}
	}
}
