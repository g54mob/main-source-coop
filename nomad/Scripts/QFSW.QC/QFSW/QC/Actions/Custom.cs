using System;

namespace QFSW.QC.Actions
{
	public class Custom : ICommandAction
	{
		private readonly Func<bool> _isFinished;

		private readonly Func<bool> _startsIdle;

		private readonly Action<ActionContext> _start;

		private readonly Action<ActionContext> _finalize;

		public bool IsFinished => _isFinished();

		public bool StartsIdle => _startsIdle();

		public Custom(Func<bool> isFinished, Func<bool> startsIdle, Action<ActionContext> start, Action<ActionContext> finalize)
		{
			_isFinished = isFinished;
			_startsIdle = startsIdle;
			_start = start;
			_finalize = finalize;
		}

		public void Start(ActionContext context)
		{
			_start(context);
		}

		public void Finalize(ActionContext context)
		{
			_finalize(context);
		}
	}
}
